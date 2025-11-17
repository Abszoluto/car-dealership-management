from django.contrib import admin
from .models import Cliente, Veiculo, Venda

@admin.register(Cliente)
class ClienteAdmin(admin.ModelAdmin):
    list_display = ('nome', 'email', 'telefone', 'data_cadastro')
    search_fields = ('nome', 'email')

@admin.register(Veiculo)
class VeiculoAdmin(admin.ModelAdmin):
    list_display = ('marca', 'modelo', 'ano', 'preco', 'status')
    list_filter = ('status', 'marca')
    search_fields = ('modelo', 'marca')

@admin.register(Venda)
class VendaAdmin(admin.ModelAdmin):
    list_display = ('data_venda', 'veiculo', 'cliente', 'funcionario', 'preco_final')
    list_filter = ('data_venda', 'funcionario')
    search_fields = ('veiculo__modelo', 'cliente__nome')
    autocomplete_fields = ['veiculo', 'cliente', 'funcionario']