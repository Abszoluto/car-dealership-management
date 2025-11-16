TP1 - Definição do Problema e Planejamento Inicial
1. Objetivo do Trabalho

Desenvolver um sistema de gerenciamento para concessionária de carros, que auxilie no controle de vendas, estoque e clientes, além de gerar relatórios mensais em formato de gráficos para apoiar a tomada de decisão.

2. Problema a Ser Resolvido

Concessionárias de carros frequentemente lidam com grandes volumes de dados relacionados a:
 1. Estoque de veículos disponíveis;

 2. Vendas realizadas;

 3. Clientes e preferências de compra;

 4. Despesas e receitas mensais.

 5. Atualmente, muitos desses processos são feitos de forma manual ou em planilhas pouco integradas, o que dificulta:

 6. Acompanhar indicadores de desempenho;

 7. Tomar decisões rápidas baseadas em dados;

 8. Gerar relatórios claros para gestores.

Assim, existe a necessidade de uma ferramenta centralizada e intuitiva para organizar, monitorar e gerar relatórios sobre o funcionamento da concessionária.

3. Tipo de Solução

Será desenvolvido um sistema web de gerenciamento com foco em:

1. Cadastro e controle de veículos (estoque).

2. Registro de vendas realizadas.

3. Gestão de clientes.

4. Relatórios mensais em forma de gráficos (ex.: vendas por mês, faturamento, veículos mais vendidos).

O sistema terá nível de complexidade simples, porém suficiente para contemplar as necessidades básicas de uma concessionária de pequeno a médio porte.

4. Requisitos
4.1 Requisitos Funcionais

O sistema deve permitir o cadastro, edição e exclusão de veículos.

O sistema deve permitir o cadastro, edição e exclusão de clientes.

O sistema deve registrar vendas realizadas (vinculando cliente e veículo).

O sistema deve gerar relatórios mensais de vendas.

O sistema deve apresentar os relatórios em gráficos visuais (ex.: barras, pizza, linha).

O sistema deve permitir o cadastro de funcionários que utilizam a aplicação.

O sistema deve permitir a consulta de histórico de vendas.

O sistema deve permitir exportação de relatórios em PDF.

4.2 Requisitos Não-Funcionais

O sistema deve ser desenvolvido em arquitetura web (frontend + backend).

O sistema deve ter interface intuitiva e responsiva.

O sistema deve armazenar dados em um banco de dados relacional.

O sistema deve gerar gráficos de forma rápida, com tempo de resposta inferior a 3 segundos.

O sistema deve ter controle de acesso com autenticação de usuários.

O sistema deve ter disponibilidade mínima de 95%.

O sistema deve ser modular, facilitando manutenção e evolução futura.

5. Diagrama de Caso de Uso

Atores:

Administrador: gerencia funcionários, clientes e veículos.

Funcionário: registra vendas e acessa relatórios.

Casos de uso principais:

Cadastrar veículo.

Cadastrar cliente.

Registrar venda.

Gerar relatório mensal.

Visualizar gráficos de desempenho.

Exportar relatório em PDF.

Gerenciar funcionários.



TODO → atividades da próxima sprint (TP2).

In Progress / Done → tarefas em execução ou concluídas.

* TP2 - Projeto de Software *

1. Linguagem e Framework	C# (.NET 8)
2. Interface Gráfica (UI)	WPF (Windows Presentation Foundation)
3. Banco de Dados Local	SQLite
4. ORM / Acesso a Dados	Entity Framework Core
5. GitHub Projects	Para gerenciar tarefas e acompanhar o progresso do TP3.
