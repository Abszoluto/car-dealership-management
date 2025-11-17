** Planos de teste para o software **
* Critérios de Aceitação *

Todas as funcionalidades devem ser executadas sem falhas.
O banco de dados deve refletir corretamente as ações realizadas.
A interface deve permanecer intuitiva e responsiva.
1. Cadastro e Gerenciamento de Veículos

Casos de teste:
Criar um novo veículo no sistema com todos os campos obrigatórios preenchidos.
Editar informações de um veículo existente (ex: preço, modelo, quilometragem).
Remover um veículo e verificar se ele é excluído corretamente do banco de dados.
Validar o tratamento de erros ao inserir dados inválidos (ex: ano inexistente, preço negativo).
Confirmar se o sistema atualiza corretamente a lista de veículos em tempo real após alterações.

2. Cadastro e Gerenciamento de Clientes

Casos de teste:
Criar novo cliente com CPF/CNPJ válido e dados obrigatórios completos.
Editar dados de um cliente existente.
Testar mensagens de erro para dados duplicados (ex: CPF já cadastrado).

3. Cadastro e Gerenciamento de Vendedores

Casos de teste:
Testar restrições de login ou permissões (caso o sistema possua autenticação).

4. Processo de Venda de Veículos

Casos de teste:
Realizar uma venda completa, vinculando veículo, cliente e vendedor.
Verificar se o veículo some da lista de estoque
Calcular corretamente o valor total da venda com descontos.

5. Informações Complementares do Veículo

Casos de teste:
Adicionar fotos de um veículo.

6. Testes de Interface e Usabilidade

Casos de teste:
Verificar se os botões e menus executam as ações corretas.
Garantir clareza nas mensagens de erro e feedback ao usuário.
