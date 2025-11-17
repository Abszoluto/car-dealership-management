import datetime
import calendar
import json
from decimal import Decimal
from django.shortcuts import render, redirect
from django.http import HttpResponse
from django.template.loader import render_to_string
from django.utils import timezone
from django.contrib.auth import logout as auth_logout
from django.contrib.auth.views import LoginView
from django.contrib.auth.decorators import login_required, user_passes_test
from django.contrib.auth.mixins import UserPassesTestMixin
from django.contrib import messages
from django.contrib.messages.views import SuccessMessageMixin
from django.urls import reverse_lazy
from django.views.generic import ListView, CreateView, UpdateView, DeleteView
from django.db.models import Q, Count
from django.db.models.functions import TruncDay
from django.contrib.auth.models import User
from .models import Venda, Veiculo, Cliente
from .forms import RegistrarVendaForm, ClienteForm, VeiculoForm

#Função verifica se o user é gerente
def is_gerente(user):
    if user.is_superuser:
        return True
    eh_gerente = user.groups.filter(name='Gerente').exists()
    
    if not eh_gerente:
        print(f"Erro de permissão. User: {user.username}")
        print(f"Grupo esperado: Gerente")
    return eh_gerente

#Métodos de autenticação

class CustomLoginView(LoginView):
    template_name = 'login.html'

def logout_view(request):
    auth_logout(request)
    return redirect('login')

#Dashboard
@login_required
def dashboard_view(request):
    hoje = timezone.now()
    ano_selecionado = request.GET.get('ano', hoje.year)
    mes_selecionado = request.GET.get('mes', hoje.month)
    
    try:
        ano_selecionado = int(ano_selecionado)
        mes_selecionado = int(mes_selecionado)
    except ValueError:
        ano_selecionado = hoje.year
        mes_selecionado = hoje.month

    anos_disponiveis = list(range(2020, hoje.year + 2)) 
    meses_disponiveis = [
        {"valor": 1, "nome": "Janeiro"}, {"valor": 2, "nome": "Fevereiro"},
        {"valor": 3, "nome": "Março"}, {"valor": 4, "nome": "Abril"},
        {"valor": 5, "nome": "Maio"}, {"valor": 6, "nome": "Junho"},
        {"valor": 7, "nome": "Julho"}, {"valor": 8, "nome": "Agosto"},
        {"valor": 9, "nome": "Setembro"}, {"valor": 10, "nome": "Outubro"},
        {"valor": 11, "nome": "Novembro"}, {"valor": 12, "nome": "Dezembro"},
    ]

    vendas_mes_qs = Venda.objects.filter(
        data_venda__year=ano_selecionado, 
        data_venda__month=mes_selecionado
    )
    
    vendas_mes_count = vendas_mes_qs.count()
    faturamento_mes = sum(v.preco_final for v in vendas_mes_qs)
    custo_mes = sum(v.veiculo.valor_compra for v in vendas_mes_qs)
    lucro_mes = faturamento_mes - custo_mes
    veiculos_disponiveis = Veiculo.objects.filter(status='disponivel').count()

    #Gráficos
    dias_no_mes = calendar.monthrange(ano_selecionado, mes_selecionado)[1]
    labels_dias = list(range(1, dias_no_mes + 1))
    dados_vendas_dia = [0] * dias_no_mes

    vendas_por_dia = vendas_mes_qs.annotate(dia=TruncDay('data_venda')).values('dia').annotate(contagem=Count('id')).values('dia', 'contagem')

    for venda_dia in vendas_por_dia:
        dia_index = venda_dia['dia'].day - 1
        dados_vendas_dia[dia_index] = venda_dia['contagem']

    labels_marcas = []
    dados_marcas = []
    marcas_do_mes = vendas_mes_qs.values_list('veiculo__marca', flat=True).distinct()
    
    for marca in marcas_do_mes:
        contagem = vendas_mes_qs.filter(veiculo__marca=marca).count()
        labels_marcas.append(marca)
        dados_marcas.append(contagem)

    context = {
        'vendas_mes_count': vendas_mes_count,
        'faturamento_mes': faturamento_mes,
        'custo_mes': custo_mes,
        'lucro_mes': lucro_mes,
        'veiculos_disponiveis': veiculos_disponiveis,
        'ano_selecionado': ano_selecionado,
        'mes_selecionado': mes_selecionado,
        'anos_disponiveis': anos_disponiveis,
        'meses_disponiveis': meses_disponiveis,
        'nome_mes_selecionado': next(m['nome'] for m in meses_disponiveis if m['valor'] == mes_selecionado),
        'chart_vendas_dia_labels': json.dumps(labels_dias),
        'chart_vendas_dia_data': json.dumps(dados_vendas_dia),
        'chart_marcas_labels': json.dumps(labels_marcas),
        'chart_marcas_data': json.dumps(dados_marcas),
    }
    return render(request, 'dashboard.html', context)


@login_required
def registrar_venda_view(request):
    if request.method == 'POST':
        form = RegistrarVendaForm(request.POST)
        if form.is_valid():
            venda = form.save(commit=False)
            venda.funcionario = request.user 
            venda.save()
            messages.success(request, 'Venda registrada com sucesso!')
            return redirect('dashboard')
    else:
        veiculo_id = request.GET.get('veiculo')
        if veiculo_id:
            form = RegistrarVendaForm(initial={'veiculo': veiculo_id})
        else:
            form = RegistrarVendaForm()
            
    return render(request, 'registrar_venda.html', {'form': form})

class VendaListView(ListView):
    model = Venda
    template_name = 'venda_list.html'
    context_object_name = 'vendas'
    paginate_by = 10
    ordering = ['-data_venda']

    def get_queryset(self):
        queryset = super().get_queryset().order_by('-data_venda')
        
        #Se o user não for gerente, verá apenas as próprias vendas.
        if not is_gerente(self.request.user):
            queryset = queryset.filter(funcionario=self.request.user)
        
        busca = self.request.GET.get('q')
        if busca:
            queryset = queryset.filter(
                Q(veiculo__modelo__icontains=busca) | 
                Q(cliente__nome__icontains=busca) |
                Q(funcionario__username__icontains=busca)
            )
        return queryset

class VendaDeleteView(UserPassesTestMixin, SuccessMessageMixin, DeleteView):
    model = Venda
    template_name = 'confirm_delete.html'
    success_url = reverse_lazy('venda_list')
    success_message = "Venda cancelada/excluída com sucesso!"

    def test_func(self): return is_gerente(self.request.user)

    def delete(self, request, *args, **kwargs):
        messages.success(self.request, self.success_message)
        return super().delete(request, *args, **kwargs)

#Gerar relatórios (PDF)
@login_required
@user_passes_test(is_gerente)
def exportar_relatorio_pdf_view(request):
    hoje = timezone.now()
    ano_param = request.GET.get('ano', hoje.year)
    mes_param = request.GET.get('mes', hoje.month)
    
    try:
        ano_selecionado = int(ano_param)
        mes_selecionado = int(mes_param)
    except ValueError:
        ano_selecionado = hoje.year
        mes_selecionado = hoje.month

    vendas = Venda.objects.filter(
        data_venda__year=ano_selecionado, 
        data_venda__month=mes_selecionado
    ).order_by('data_venda')

    faturamento_total = sum(v.preco_final for v in vendas)
    custo_total = sum(v.veiculo.valor_compra for v in vendas)
    lucro_total = faturamento_total - custo_total
    
    total_vendas = vendas.count()
    ticket_medio = faturamento_total / total_vendas if total_vendas > 0 else 0
    
    data_referencia = datetime.date(ano_selecionado, mes_selecionado, 1)
    nome_mes = data_referencia.strftime('%B')
    
    context = {
        'vendas': vendas,
        'faturamento_total': faturamento_total,
        'custo_total': custo_total,
        'lucro_total': lucro_total,
        'total_vendas': total_vendas,
        'ticket_medio': ticket_medio,
        'periodo': f"{nome_mes.capitalize()}/{ano_selecionado}",
        'data_geracao': hoje,
        'usuario_gerador': request.user
    }
    
    html_string = render_to_string('relatorio_pdf.html', context)
    base_url = request.build_absolute_uri('/')

    try:
        from weasyprint import HTML
        pdf = HTML(string=html_string, base_url=base_url).write_pdf()
        response = HttpResponse(pdf, content_type='application/pdf')
        filename = f"Relatorio_Financeiro_{mes_selecionado}_{ano_selecionado}.pdf"
        response['Content-Disposition'] = f'attachment; filename="{filename}"'
        return response
    except ImportError:
        return HttpResponse("Erro ao gerar PDF: WeasyPrint não encontrado.", status=500)

#Clientes
class ClienteListView(ListView):
    model = Cliente
    template_name = 'cliente_list.html'
    context_object_name = 'clientes'
    paginate_by = 10

    def get_queryset(self):
        queryset = super().get_queryset()
        busca = self.request.GET.get('q')
        if busca:
            queryset = queryset.filter(Q(nome__icontains=busca) | Q(email__icontains=busca))
        return queryset

class ClienteCreateView(SuccessMessageMixin, CreateView):
    model = Cliente
    form_class = ClienteForm
    template_name = 'cliente_form.html'
    success_url = reverse_lazy('cliente_list')
    success_message = "Cliente cadastrado com sucesso!"

class ClienteUpdateView(SuccessMessageMixin, UpdateView):
    model = Cliente
    form_class = ClienteForm
    template_name = 'cliente_form.html'
    success_url = reverse_lazy('cliente_list')
    success_message = "Cliente atualizado com sucesso!"

class ClienteDeleteView(UserPassesTestMixin, SuccessMessageMixin, DeleteView):
    model = Cliente
    template_name = 'confirm_delete.html'
    success_url = reverse_lazy('cliente_list')
    success_message = "Cliente excluído com sucesso!"

    def test_func(self): return is_gerente(self.request.user)

    def delete(self, request, *args, **kwargs):
        messages.success(self.request, self.success_message)
        return super().delete(request, *args, **kwargs)

#Veiculos
class VeiculoListView(ListView):
    model = Veiculo
    template_name = 'veiculo_list.html'
    context_object_name = 'veiculos'
    paginate_by = 10

    def get_queryset(self):
        queryset = super().get_queryset().filter(status='disponivel')
        busca = self.request.GET.get('q')
        if busca:
            queryset = queryset.filter(
                Q(modelo__icontains=busca) | 
                Q(marca__icontains=busca) |
                Q(numero_documento__icontains=busca)
            )
        return queryset

class VeiculoCreateView(UserPassesTestMixin, SuccessMessageMixin, CreateView):
    model = Veiculo
    form_class = VeiculoForm
    template_name = 'veiculo_form.html'
    success_url = reverse_lazy('veiculo_list')
    success_message = "Veículo adicionado com sucesso!"

    def test_func(self): return is_gerente(self.request.user)

class VeiculoUpdateView(UserPassesTestMixin, SuccessMessageMixin, UpdateView):
    model = Veiculo
    form_class = VeiculoForm
    template_name = 'veiculo_form.html'
    success_url = reverse_lazy('veiculo_list')
    success_message = "Veículo atualizado com sucesso!"

    def test_func(self): return is_gerente(self.request.user)

class VeiculoDeleteView(UserPassesTestMixin, SuccessMessageMixin, DeleteView):
    model = Veiculo
    template_name = 'confirm_delete.html'
    success_url = reverse_lazy('veiculo_list')
    success_message = "Veículo excluído com sucesso!"

    def test_func(self): return is_gerente(self.request.user)

    def delete(self, request, *args, **kwargs):
        messages.success(self.request, self.success_message)
        return super().delete(request, *args, **kwargs)

#Funcionarios

class FuncionarioListView(UserPassesTestMixin, ListView):
    model = User
    template_name = 'funcionario_list.html'
    context_object_name = 'funcionarios'
    paginate_by = 10

    def test_func(self): return is_gerente(self.request.user)

    def get_queryset(self):
        queryset = User.objects.all().order_by('username')
        busca = self.request.GET.get('q')
        if busca:
            queryset = queryset.filter(username__icontains=busca)
        return queryset

class FuncionarioDeleteView(UserPassesTestMixin, SuccessMessageMixin, DeleteView):
    model = User
    template_name = 'confirm_delete.html'
    success_url = reverse_lazy('funcionario_list')
    success_message = "Funcionário excluído com sucesso!"

    def test_func(self):
        usuario_alvo = self.get_object()
        return is_gerente(self.request.user) and usuario_alvo != self.request.user

    def delete(self, request, *args, **kwargs):
        messages.success(self.request, self.success_message)
        return super().delete(request, *args, **kwargs)