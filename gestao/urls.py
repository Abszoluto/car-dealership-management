# Em gestao/urls.py
from django.urls import path
from . import views

urlpatterns = [
    #oauth
    path('login/', views.CustomLoginView.as_view(), name='login'),
    path('logout/', views.logout_view, name='logout'),
    
    path('', views.dashboard_view, name='dashboard'),
    
    #Vendas
    path('vendas/registrar/', views.registrar_venda_view, name='registrar_venda'),

    #Historico de vendas
    path('vendas/historico/', views.VendaListView.as_view(), name='venda_list'),
    path('vendas/excluir/<int:pk>/', views.VendaDeleteView.as_view(), name='venda_delete'),
    
    #Relatorio
    path('relatorio/pdf/', views.exportar_relatorio_pdf_view, name='exportar_relatorio_pdf'),

    #Clientes
    path('clientes/', views.ClienteListView.as_view(), name='cliente_list'),
    path('clientes/adicionar/', views.ClienteCreateView.as_view(), name='cliente_add'),
    path('clientes/editar/<int:pk>/', views.ClienteUpdateView.as_view(), name='cliente_edit'),
    path('clientes/excluir/<int:pk>/', views.ClienteDeleteView.as_view(), name='cliente_delete'),
    
    #Veiculos
    path('veiculos/', views.VeiculoListView.as_view(), name='veiculo_list'),
    path('veiculos/adicionar/', views.VeiculoCreateView.as_view(), name='veiculo_add'),
    path('veiculos/editar/<int:pk>/', views.VeiculoUpdateView.as_view(), name='veiculo_edit'),
    path('veiculos/excluir/<int:pk>/', views.VeiculoDeleteView.as_view(), name='veiculo_delete'),

    #Funcionarios
    path('funcionarios/', views.FuncionarioListView.as_view(), name='funcionario_list'),
    path('funcionarios/excluir/<int:pk>/', views.FuncionarioDeleteView.as_view(), name='funcionario_delete'),
]