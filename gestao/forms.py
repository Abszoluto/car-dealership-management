from django import forms
from django.core.exceptions import ValidationError
from .models import Venda, Veiculo, Cliente
import re 

#Validar cpf
def validar_cpf_algoritmo(numbers):
    if len(numbers) != 11:
        return False

    def calcular_digito(digitos):
        soma = 0
        peso = len(digitos) + 1
        for n in digitos:
            soma += int(n) * peso
            peso -= 1
        resto = soma % 11
        return 0 if resto < 2 else 11 - resto

    digito1 = calcular_digito(numbers[:9])
    digito2 = calcular_digito(numbers[:9] + [str(digito1)])
    
    return numbers == list(numbers[:9]) + [str(digito1), str(digito2)]

#Classes de formulários
class RegistrarVendaForm(forms.ModelForm):
    veiculo = forms.ModelChoiceField(
        queryset=Veiculo.objects.filter(status='disponivel'),
        widget=forms.Select(attrs={'class': 'form-select'})
    )
    cliente = forms.ModelChoiceField(
        queryset=Cliente.objects.all(),
        widget=forms.Select(attrs={'class': 'form-select'})
    )
    class Meta:
        model = Venda
        fields = ['veiculo', 'cliente', 'tipo_pagamento', 'preco_final']
        widgets = {
            'preco_final': forms.NumberInput(attrs={'class': 'form-control'}),
            'tipo_pagamento': forms.Select(attrs={'class': 'form-select'}),
        }

class ClienteForm(forms.ModelForm):
    class Meta:
        model = Cliente
        fields = ['nome', 'cpf', 'email', 'telefone', 'cep', 'endereco', 'complemento']
        widgets = {
            'nome': forms.TextInput(attrs={'class': 'form-control'}),
            'cpf': forms.TextInput(attrs={'class': 'form-control', 'id': 'id_cpf', 'placeholder': '000.000.000-00'}),
            'email': forms.EmailInput(attrs={'class': 'form-control'}),
            'telefone': forms.TextInput(attrs={'class': 'form-control', 'id': 'id_telefone', 'placeholder': '(00) 00000-0000'}),
            'cep': forms.TextInput(attrs={'class': 'form-control', 'id': 'id_cep', 'placeholder': '00000-000'}),
            'endereco': forms.TextInput(attrs={'class': 'form-control'}),
            'complemento': forms.TextInput(attrs={'class': 'form-control'}),
        }

    #Validar cpf
    def clean_cpf(self):
        cpf = self.cleaned_data.get('cpf')
        cpf_limpo = re.sub(r'\D', '', cpf)
        
        if not validar_cpf_algoritmo(list(cpf_limpo)):
            raise ValidationError("CPF inválido. Verifique os números digitados.")
        
        return f"{cpf_limpo[:3]}.{cpf_limpo[3:6]}.{cpf_limpo[6:9]}-{cpf_limpo[9:]}"

    #Validar num telefone
    def clean_telefone(self):
        telefone = self.cleaned_data.get('telefone')
        tel_limpo = re.sub(r'\D', '', telefone)
        
        if len(tel_limpo) < 10 or len(tel_limpo) > 11:
             raise ValidationError("Telefone inválido. Deve conter DDD + Número.")
        
        if len(tel_limpo) == 11:
            return f"({tel_limpo[:2]}) {tel_limpo[2:7]}-{tel_limpo[7:]}"
        return f"({tel_limpo[:2]}) {tel_limpo[2:6]}-{tel_limpo[6:]}"

class VeiculoForm(forms.ModelForm):
    class Meta:
        model = Veiculo
        fields = ['marca', 'modelo', 'ano', 'numero_documento', 'valor_compra', 'preco', 'foto']
        widgets = {
            'marca': forms.TextInput(attrs={'class': 'form-control'}),
            'modelo': forms.TextInput(attrs={'class': 'form-control'}),
            'ano': forms.NumberInput(attrs={'class': 'form-control'}),
            'numero_documento': forms.TextInput(attrs={'class': 'form-control', 'placeholder': 'Ex: RENAVAM'}),
            'valor_compra': forms.NumberInput(attrs={'class': 'form-control', 'placeholder': 'Quanto a loja pagou?'}),
            'preco': forms.NumberInput(attrs={'class': 'form-control', 'placeholder': 'Preço de venda sugerido'}),
            'foto': forms.FileInput(attrs={'class': 'form-control'}),
        }
        
    def clean_ano(self):
        ano = self.cleaned_data.get('ano')
        if ano < 1900 or ano > 2030:
            raise ValidationError("Ano inválido.")
        return ano