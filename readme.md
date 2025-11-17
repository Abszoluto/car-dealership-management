# TP1 -- Definição do Problema e Planejamento Inicial

## 1. Objetivo do Trabalho
Desenvolver um sistema de gerenciamento para concessionária de carros,
voltado para o controle de vendas, estoque e clientes. O sistema também
deve gerar relatórios mensais em formato de gráficos, apoiando a tomada
de decisão.

## 2. Problema a Ser Resolvido
Concessionárias lidam diariamente com um volume significativo de
informações, como:
1.  Estoque de veículos disponíveis
2.  Vendas realizadas
3.  Dados de clientes e suas preferências
4.  Despesas e receitas mensais

Grande parte desses processos ainda é executada de maneira manual ou em
planilhas isoladas, o que dificulta:
-   Acompanhar indicadores de desempenho
-   Tomar decisões baseadas em dados
-   Gerar relatórios consistentes para gestores

Nesse cenário, torna-se necessária uma ferramenta centralizada, organizada e simples de operar, reunindo e processando essas informações de forma eficiente.

## 3. Tipo de Solução

Será desenvolvido um sistema web que permita:
1.  Cadastro e controle de veículos
2.  Registro de vendas
3.  Gestão de clientes
4.  Relatórios mensais em formato gráfico (ex.: vendas por mês,
    faturamento, modelos mais vendidos)

O sistema terá nível de complexidade simples, adequado para
concessionárias de pequeno e médio porte.

------------------------------------------------------------------------

# 4. Requisitos
## 4.1 Requisitos Funcionais
-   Cadastro, edição e exclusão de veículos
-   Cadastro, edição e exclusão de clientes
-   Registro de vendas associando cliente e veículo
-   Geração de relatórios mensais de vendas
-   Visualização dos relatórios em gráficos
-   Cadastro de funcionários
-   Consulta de histórico de vendas
-   Exportação de relatórios em PDF
## 4.2 Requisitos Não Funcionais
-   Arquitetura web (frontend + backend)
-   Interface intuitiva e responsiva
-   Banco de dados relacional
-   Geração de gráficos com tempo de resposta rápido
-   Autenticação de usuários
-   Estrutura modular para manutenção futura

------------------------------------------------------------------------

# 5. Diagrama de Caso de Uso
## Atores
-   Administrador (Gerente): gerencia funcionários, clientes e veículos
-   Funcionário: registra vendas e acessa relatórios
  
## Casos de Uso Principais
1.  Cadastrar veículo
2.  Cadastrar cliente
3.  Registrar venda
4.  Gerar relatório mensal
5.  Visualizar gráficos de desempenho
6.  Exportar relatório em PDF
7.  Gerenciar funcionários

------------------------------------------------------------------------

# TP2 -- Projeto de Software
-   Linguagem: Python 3
-   Framework: Django 5
-   Frontend: HTML5, CSS3 (Bootstrap 5), JavaScript
-   Banco de Dados: SQLite
-   Gerenciamento de tarefas: GitHub Projects

------------------------------------------------------------------------

# Como Usar

## 1. Instalar dependências
    pip install -r requirements.txt

## 2. Criar a estrutura do banco de dados
    python manage.py makemigrations
    python manage.py migrate

## 3. Iniciar o servidor local
    python manage.py runserver

## 4. Acessar o sistema
Abra no navegador:\
http://127.0.0.1:8000/

Observação: será necessário criar um usuário para acessar o sistema,
pois há autenticação configurada.
