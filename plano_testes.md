** Planos de teste para o software **
* Critérios de Aceitação *

Todas as funcionalidades devem ser executadas sem falhas.

O banco de dados deve refletir corretamente as ações realizadas.

O desempenho deve se manter estável mesmo em testes de carga.

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

Excluir um cliente e garantir que seus registros sejam removidos sem afetar vendas associadas.

Testar mensagens de erro para dados duplicados (ex: CPF já cadastrado).

3. Cadastro e Gerenciamento de Vendedores

Casos de teste:

Inserir um novo vendedor com dados válidos.

Editar informações (ex: comissão, telefone, e-mail).

Remover um vendedor e validar impacto em registros de vendas.

Testar restrições de login ou permissões (caso o sistema possua autenticação).

4. Processo de Venda de Veículos

Casos de teste:

Realizar uma venda completa, vinculando veículo, cliente e vendedor.

Verificar se o veículo vendido é marcado como “indisponível”.

Testar cancelamento de venda e ver se o veículo retorna ao estoque.

Calcular corretamente o valor total da venda com descontos e impostos (se aplicável).

Validar geração de comprovante ou nota de venda.

5. Informações Complementares do Veículo

Casos de teste:

Adicionar documentos (ex: CRLV, nota fiscal) e fotos de um veículo.

Verificar formatos aceitos (PDF, JPG, PNG, etc.).

Editar e substituir documentos/fotos existentes.

Remover anexos e confirmar a exclusão do armazenamento.

6. Testes de Desempenho e Escalabilidade

Casos de teste:

Inserir grande volume de veículos, clientes e vendas para avaliar tempo de resposta.

Monitorar o consumo de recursos (CPU, memória, tempo de consulta no banco).

Testar operações simultâneas de vários usuários (concorrência).

Validar se o sistema mantém desempenho estável com alto número de registros.

7. Teste de Banco de Dados com Múltiplas Filiais

Casos de teste:

Configurar bancos de dados separados para cada filial.

Validar se dados de uma filial não interferem nos de outra.

Testar sincronização ou relatórios consolidados entre filiais (caso aplicável).

Verificar conexões e autenticações entre bancos distintos.

8. Testes de Interface e Usabilidade

Casos de teste:

Verificar se os botões e menus executam as ações corretas.

Testar responsividade em diferentes tamanhos de tela (desktop, tablet, celular).

Garantir clareza nas mensagens de erro e feedback ao usuário.