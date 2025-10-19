-- =========================
-- TABELA DE FILIAIS
-- =========================
CREATE TABLE filial (
    id_filial       INTEGER PRIMARY KEY AUTOINCREMENT,
    nome            TEXT NOT NULL,
    cnpj            TEXT UNIQUE NOT NULL,
    estado          TEXT NOT NULL,
    cidade          TEXT NOT NULL,
    data_criacao    TEXT NOT NULL
);

-- =========================
-- FUNCIONÁRIOS E CONTRATOS
-- =========================
CREATE TABLE funcionario (
    id_funcionario  INTEGER PRIMARY KEY AUTOINCREMENT,
    id_filial       INTEGER NOT NULL,
    nome_completo   TEXT NOT NULL,
    cargo           TEXT NOT NULL,
    data_admissao   TEXT NOT NULL,
    data_demissao   TEXT,
    ativo           INTEGER NOT NULL DEFAULT 1,
    FOREIGN KEY (id_filial) REFERENCES filial(id_filial)
);

CREATE TABLE contrato_funcionario (
    id_contrato         INTEGER PRIMARY KEY AUTOINCREMENT,
    id_funcionario      INTEGER NOT NULL,
    data_inicio         TEXT NOT NULL,
    data_fim            TEXT,
    salario_base        REAL NOT NULL,
    id_plano_comissao   INTEGER,
    FOREIGN KEY (id_funcionario) REFERENCES funcionario(id_funcionario),
    FOREIGN KEY (id_plano_comissao) REFERENCES plano_comissao(id_plano_comissao)
);

-- =========================
-- CLIENTES
-- =========================
CREATE TABLE cliente (
    id_cliente      INTEGER PRIMARY KEY AUTOINCREMENT,
    tipo_pessoa     TEXT NOT NULL CHECK (tipo_pessoa IN ('fisica','juridica')),
    nome_razao      TEXT NOT NULL,
    documento       TEXT UNIQUE NOT NULL,
    email           TEXT,
    telefone        TEXT,
    data_cadastro   TEXT NOT NULL
);

-- =========================
-- VEÍCULOS E AQUISIÇÃO
-- =========================
CREATE TABLE veiculo (
    id_veiculo      INTEGER PRIMARY KEY AUTOINCREMENT,
    id_filial       INTEGER NOT NULL,
    chassi          TEXT UNIQUE NOT NULL,
    marca           TEXT NOT NULL,
    modelo          TEXT NOT NULL,
    ano_modelo      INTEGER NOT NULL,
    cor             TEXT,
    quilometragem   INTEGER,
    combustivel     TEXT,
    status          TEXT NOT NULL CHECK (status IN ('estoque','reservado','vendido','baixado')),
    data_cadastro   TEXT NOT NULL,
    FOREIGN KEY (id_filial) REFERENCES filial(id_filial)
);

CREATE TABLE aquisicao_veiculo (
    id_aquisicao    INTEGER PRIMARY KEY AUTOINCREMENT,
    id_veiculo      INTEGER NOT NULL,
    tipo_origem     TEXT NOT NULL,
    fornecedor_nome TEXT NOT NULL,
    data_aquisicao  TEXT NOT NULL,
    custo_aquisicao REAL NOT NULL,
    documento       TEXT,
    FOREIGN KEY (id_veiculo) REFERENCES veiculo(id_veiculo)
);

-- =========================
-- DESPESAS (GENÉRICO)
-- =========================
CREATE TABLE despesa (
    id_despesa      INTEGER PRIMARY KEY AUTOINCREMENT,
    id_filial       INTEGER NOT NULL,
    id_veiculo      INTEGER,
    id_funcionario  INTEGER,
    tipo_despesa    TEXT NOT NULL CHECK (tipo_despesa IN (
        'preparacao', 'imposto', 'salario', 'aluguel', 'marketing', 'outros'
    )),
    fornecedor      TEXT,
    descricao       TEXT,
    valor           REAL NOT NULL,
    mes_referencia  TEXT,
    data_despesa    TEXT NOT NULL,
    documento       TEXT,
    FOREIGN KEY (id_filial) REFERENCES filial(id_filial),
    FOREIGN KEY (id_veiculo) REFERENCES veiculo(id_veiculo),
    FOREIGN KEY (id_funcionario) REFERENCES funcionario(id_funcionario)
);

-- =========================
-- VENDAS
-- =========================
CREATE TABLE pedido_venda (
    id_venda        INTEGER PRIMARY KEY AUTOINCREMENT,
    id_filial       INTEGER NOT NULL,
    id_cliente      INTEGER NOT NULL,
    id_vendedor     INTEGER NOT NULL,
    status          TEXT NOT NULL CHECK (status IN ('aberto','faturado','cancelado')),
    data_venda      TEXT,
    data_criacao    TEXT NOT NULL,
    data_cancelamento TEXT,
    FOREIGN KEY (id_filial) REFERENCES filial(id_filial),
    FOREIGN KEY (id_cliente) REFERENCES cliente(id_cliente),
    FOREIGN KEY (id_vendedor) REFERENCES funcionario(id_funcionario)
);

CREATE TABLE item_venda (
    id_item_venda   INTEGER PRIMARY KEY AUTOINCREMENT,
    id_venda        INTEGER NOT NULL,
    id_veiculo      INTEGER NOT NULL,
    preco_lista     REAL NOT NULL,
    desconto        REAL DEFAULT 0,
    preco_venda     REAL NOT NULL,
    FOREIGN KEY (id_venda) REFERENCES pedido_venda(id_venda),
    FOREIGN KEY (id_veiculo) REFERENCES veiculo(id_veiculo)
);

CREATE TABLE pagamento_venda (
    id_pagamento    INTEGER PRIMARY KEY AUTOINCREMENT,
    id_venda        INTEGER NOT NULL,
    parcela         INTEGER NOT NULL,
    forma_pagamento TEXT NOT NULL CHECK (forma_pagamento IN ('dinheiro','pix','cartao','entrada','financiamento')),
    valor           REAL NOT NULL,
    data_pagamento  TEXT,
    status          TEXT NOT NULL CHECK (status IN ('pago','pendente')),
    FOREIGN KEY (id_venda) REFERENCES pedido_venda(id_venda)
);

CREATE TABLE troca (
    id_troca        INTEGER PRIMARY KEY AUTOINCREMENT,
    id_venda        INTEGER NOT NULL,
    descricao_veiculo_recebido TEXT NOT NULL,
    valor_avaliado  REAL NOT NULL,
    estimativa_preparacao REAL,
    FOREIGN KEY (id_venda) REFERENCES pedido_venda(id_venda)
);

CREATE TABLE financiamento (
    id_financiamento INTEGER PRIMARY KEY AUTOINCREMENT,
    id_venda        INTEGER NOT NULL,
    banco           TEXT NOT NULL,
    valor_financiado REAL NOT NULL,
    taxa_juros_anual REAL,
    data_aprovacao  TEXT,
    FOREIGN KEY (id_venda) REFERENCES pedido_venda(id_venda)
);

-- =========================
-- COMISSÕES
-- =========================
CREATE TABLE plano_comissao (
    id_plano_comissao   INTEGER PRIMARY KEY AUTOINCREMENT,
    nome                TEXT NOT NULL,
    base_calculo        TEXT NOT NULL CHECK (base_calculo IN ('preco','margem')),
    percentual          REAL,
    valor_fixo          REAL,
    piso_margem         REAL,
    data_inicio         TEXT NOT NULL,
    data_fim            TEXT
);

CREATE TABLE comissao_gerada (
    id_comissao         INTEGER PRIMARY KEY AUTOINCREMENT,
    id_venda            INTEGER NOT NULL,
    id_funcionario      INTEGER NOT NULL,
    id_plano_comissao   INTEGER NOT NULL,
    valor_base          REAL NOT NULL,
    valor_comissao      REAL NOT NULL,
    data_calculo        TEXT NOT NULL,
    FOREIGN KEY (id_venda) REFERENCES pedido_venda(id_venda),
    FOREIGN KEY (id_funcionario) REFERENCES funcionario(id_funcionario),
    FOREIGN KEY (id_plano_comissao) REFERENCES plano_comissao(id_plano_comissao)
);

CREATE TABLE pagamento_comissao (
    id_pagamento_comissao INTEGER PRIMARY KEY AUTOINCREMENT,
    id_comissao          INTEGER NOT NULL,
    data_pagamento       TEXT NOT NULL,
    valor_pago           REAL NOT NULL,
    FOREIGN KEY (id_comissao) REFERENCES comissao_gerada(id_comissao)
);