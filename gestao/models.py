from django.db import models
from django.contrib.auth.models import User
from decimal import Decimal

class Cliente(models.Model):
    nome = models.CharField(max_length=100)
    cpf = models.CharField(max_length=14, unique=True, verbose_name="CPF") 
    email = models.EmailField(unique=True)
    telefone = models.CharField(max_length=20, blank=True, null=True)
    
    cep = models.CharField(max_length=9, blank=True, null=True, verbose_name="CEP")
    endereco = models.CharField(max_length=200, blank=True, null=True, verbose_name="Endereço Completo")
    complemento = models.CharField(max_length=100, blank=True, null=True)

    data_cadastro = models.DateTimeField(auto_now_add=True)

    def __str__(self):
        return f"{self.nome} ({self.cpf})"

class Veiculo(models.Model):
    STATUS_CHOICES = [
        ('disponivel', 'Disponível'),
        ('vendido', 'Vendido'),
    ]
    marca = models.CharField(max_length=50)
    modelo = models.CharField(max_length=50)
    ano = models.PositiveIntegerField()
    numero_documento = models.CharField(max_length=30, blank=True, null=True)
    valor_compra = models.DecimalField(max_digits=10, decimal_places=2, default=0)
    preco = models.DecimalField(max_digits=10, decimal_places=2)
    status = models.CharField(max_length=10, choices=STATUS_CHOICES, default='disponivel')
    foto = models.ImageField(upload_to='veiculos/', blank=True, null=True)

    def __str__(self):
        return f"{self.marca} {self.modelo} ({self.ano})"

class Venda(models.Model):
    veiculo = models.ForeignKey(Veiculo, on_delete=models.PROTECT, related_name='vendas')
    cliente = models.ForeignKey(Cliente, on_delete=models.PROTECT, related_name='compras')
    funcionario = models.ForeignKey(User, on_delete=models.PROTECT, related_name='vendas_realizadas')
    preco_final = models.DecimalField(max_digits=10, decimal_places=2)
    data_venda = models.DateTimeField(auto_now_add=True)

    FORMA_PAGAMENTO = [
        ('vista', 'À vista / PIX'),
        ('financiado', 'Financiamento'),
        ('cartao', 'Cartão de crédito'),
    ]

    tipo_pagamento = models.CharField(max_length=20, choices=FORMA_PAGAMENTO, default='vista')
    observacao = models.TextField(blank=True, verbose_name="Obs (Banco/Detalhes)")
    comissao_valor = models.DecimalField(max_digits=10, decimal_places=2, editable=False, default=0)

    def __str__(self):
        return f"Venda de {self.veiculo} para {self.cliente}"

    def save(self, *args, **kwargs):
        if not self.pk:
            self.veiculo.status = 'vendido'
            self.veiculo.save()
            self.comissao_valor = self.preco_final * Decimal('0.01')
            
        super().save(*args, **kwargs)