-- APAGANDO O BANCO DE DADOS
DROP DATABASE IF EXISTS dbfrancisco;

-- CRIANDO O BANCO DE DADOS
CREATE DATABASE dbfrancisco
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_general_ci;

-- ENTRANDO NO BANCO DE DADOS
USE dbfrancisco;

-- CRIANDO A TABELA DE VOLUNTÁRIOS
CREATE TABLE tbVoluntarios(
    codVol INT NOT NULL AUTO_INCREMENT,
    nome VARCHAR(100) NOT NULL,
    telCel VARCHAR(15),
    cpf VARCHAR(14) NULL UNIQUE,
    cep VARCHAR(9),
    rua VARCHAR(100),
    numero VARCHAR(5),
    complemento VARCHAR(100),
    bairro VARCHAR(100),
    cidade VARCHAR(100),
    estado VARCHAR(2),
    ativo BOOLEAN DEFAULT TRUE,
    foto LONGBLOB,
    PRIMARY KEY(codVol)
);

-- CRIANDO A TABELA DE USUÁRIOS
CREATE TABLE tbUsuarios(
    codUsu INT NOT NULL AUTO_INCREMENT,
    usuario VARCHAR(100) NOT NULL UNIQUE,
    senha VARCHAR(100) NOT NULL,
    tipo ENUM('ADMIN','USER') DEFAULT 'USER',
    ativo BOOLEAN DEFAULT TRUE,
    codVol INT NOT NULL,
    PRIMARY KEY(codUsu),
    FOREIGN KEY(codVol) REFERENCES tbVoluntarios(codVol)
);

-- CRIANDO A TABELA DE CLIENTES
CREATE TABLE tbClientes(
    codCli INT NOT NULL AUTO_INCREMENT,  
    nome VARCHAR(100) NOT NULL,
    cpf VARCHAR(14) UNIQUE,
    cnpj VARCHAR(18) UNIQUE,
    cep VARCHAR(9),
    rua VARCHAR(100),
    numero VARCHAR(5),
    complemento VARCHAR(100),
    bairro VARCHAR(100),
    cidade VARCHAR(100),
    estado VARCHAR(2),
    telCel VARCHAR(15),
    referencia VARCHAR(200) NOT NULL,
    PRIMARY KEY(codCli)
);

-- CRIANDO A TABELA DE ORIGEM DAS DOAÇÕES
CREATE TABLE tbOrigemDoacao(
    codOri INT NOT NULL AUTO_INCREMENT,  
    nome VARCHAR(100) NOT NULL,
    cpf VARCHAR(14) UNIQUE,
    cnpj VARCHAR(18) UNIQUE,
    cep VARCHAR(9),
    rua VARCHAR(100),
    numero VARCHAR(5),
    complemento VARCHAR(100),
    bairro VARCHAR(100),
    cidade VARCHAR(100),
    estado VARCHAR(2),
    telCel VARCHAR(15),
    referencia VARCHAR(200),
    dataCadastro DATETIME DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY(codOri)
);

-- CRIANDO A TABELA DE UNIDADES
CREATE TABLE tbUnidades(
    codUni INT NOT NULL AUTO_INCREMENT,  
    descricao VARCHAR(20) NOT NULL UNIQUE,
    PRIMARY KEY(codUni)
);

-- CRIANDO A TABELA DE LISTA PRODUTOS
CREATE TABLE tbLista(
    codList INT NOT NULL AUTO_INCREMENT,  
    descricao VARCHAR(100) NOT NULL UNIQUE,
    peso INT NOT NULL,
    unidade VARCHAR(20) NOT NULL,
    quantidade INT NOT NULL DEFAULT 0,
    codUni INT NOT NULL,
    PRIMARY KEY(codList),
    FOREIGN KEY(codUni) REFERENCES tbUnidades(codUni)
);

-- CRIANDO A TABELA DE ESTOQUE DE ITENS
CREATE TABLE tbEstoqueItens(
    codList INT NOT NULL,
    quantidade INT NOT NULL DEFAULT 0,
    dataMovimentacao DATE,
    horaMovimentacao TIME,
    PRIMARY KEY (codList),
    FOREIGN KEY (codList) REFERENCES tbLista(codList)
);

-- CRIANDO A TABELA DE PRODUTOS - registro da entrada
CREATE TABLE tbProdutos(
    codProd INT NOT NULL AUTO_INCREMENT,  
    descricao VARCHAR(100) NOT NULL,
    quantidade INT NOT NULL,
    peso DECIMAL (10,3) NOT NULL,
    unidade VARCHAR(20) NOT NULL,
    codBar VARCHAR(13),
    dataDeEntrada DATETIME NOT NULL,
    dataDeValidade DATE NOT NULL,
    dataLimiteDeSaida DATE NOT NULL,
    tipoMovimentacao VARCHAR(20) DEFAULT 'ENTRADA', 
    codUsu INT NOT NULL,
    codOri INT NOT NULL,
    codList INT NOT NULL,
    PRIMARY KEY(codProd),
    FOREIGN KEY(codUsu) REFERENCES tbUsuarios(codUsu),
    FOREIGN KEY(codOri) REFERENCES tbOrigemDoacao(codOri),
    FOREIGN KEY(codList) REFERENCES tbLista(codList)
);

-- Adicionar observacao (se não existir)
ALTER TABLE tbProdutos 
ADD COLUMN observacao VARCHAR(500) NULL;

-- Adicionar destino
ALTER TABLE tbProdutos 
ADD COLUMN destino VARCHAR(200) NULL;

-- CRIANDO A TABELA DE MODELO DE CESTAS
CREATE TABLE tbModeloCesta(
    codModelo INT AUTO_INCREMENT PRIMARY KEY,
    descricao VARCHAR(100) NOT NULL
);

-- CRIANDO A TABELA DE RELAÇÃO ENTRE PRODUTO E MODELO DE CESTA
CREATE TABLE tbItensDoModeloCesta(
    codModelo INT NOT NULL,
    codList INT NOT NULL,
    quantidadeMinima INT NOT NULL,
    PRIMARY KEY (codModelo, codList),
    FOREIGN KEY (codModelo) REFERENCES tbModeloCesta(codModelo),
    FOREIGN KEY (codList) REFERENCES tbLista(codList)
);

-- CRIANDO A TABELA DE CESTAS
CREATE TABLE tbCestas(
    codCes INT NOT NULL AUTO_INCREMENT,
    dataDeSaida DATETIME DEFAULT CURRENT_TIMESTAMP,
    codUsu INT NOT NULL,
    codCli INT NULL,
    PRIMARY KEY(codCes),
    FOREIGN KEY(codUsu) REFERENCES tbUsuarios(codUsu),
    FOREIGN KEY(codCli) REFERENCES tbClientes(codCli)
);

-- CRIANDO A TABELA QUE LIGA UM PRODUTO A UMA CESTA
CREATE TABLE tbItensCesta(
    codCes INT NOT NULL,
    codList INT NOT NULL,
    quantidade INT NOT NULL,
    PRIMARY KEY (codCes, codList),
    FOREIGN KEY (codCes) REFERENCES tbCestas(codCes),
    FOREIGN KEY (codList) REFERENCES tbLista(codList)
);

-- CRIANDO A TABELA DE JORNAL
CREATE TABLE tbJornal(
    codJor INT NOT NULL AUTO_INCREMENT,
    titulo VARCHAR(100) NOT NULL,
    dataDePublicacao DATETIME NOT NULL,
    descricao VARCHAR(10000) NOT NULL, 
    foto LONGBLOB NOT NULL,
    tema VARCHAR(100) NOT NULL,
    email VARCHAR(100),
    nome VARCHAR(100),
    codUsu INT NOT NULL,
    PRIMARY KEY(codJor),
    FOREIGN KEY(codUsu) REFERENCES tbUsuarios(codUsu)
);

-- CRIANDO A TABELA DE FALE CONOSCO
CREATE TABLE tbFaleConosco(
    codFaleConosco INT NOT NULL AUTO_INCREMENT,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL,
    assunto VARCHAR(100),
    mensagem VARCHAR(200) NOT NULL,
    codUsu INT NOT NULL,
    PRIMARY KEY(codFaleConosco),
    FOREIGN KEY(codUsu) REFERENCES tbUsuarios(codUsu)
);

-- MODIFICAR COLUNAS
ALTER TABLE tbProdutos 
MODIFY COLUMN dataDeValidade DATE NOT NULL,
MODIFY COLUMN dataLimiteDeSaida DATE NOT NULL;

-- =============================================
-- INSERÇÃO DE DADOS INICIAIS (ORDEM CORRETA)
-- =============================================

-- 1. Primeiro: Inserir unidades (tabela base)
INSERT INTO tbUnidades (descricao) VALUES
('QUILOGRAMAS (KG)'),  -- codUni = 1
('GRAMAS (G)'),        -- codUni = 2
('LITROS (L)'),        -- codUni = 3
('MILILITROS (ML)'),   -- codUni = 4
('UNIDADES (UN)');     -- codUni = 5

-- 2. Inserir voluntário
INSERT INTO tbVoluntarios (nome, telCel, cpf, cep, rua, numero, complemento, bairro, cidade, estado)
VALUES ('Admin', '(11)90000-0000', '000.000.000-00', '00000-000', 'Grupo Francisco', '000', '', 'Jd.Francisco', 'São Paulo', 'SP');

-- 3. Inserir usuário
INSERT INTO tbUsuarios (usuario, senha, tipo, codVol)
VALUES ('admin', '123', 'ADMIN', 1);

-- 4. Inserir origem de doação
INSERT INTO tbOrigemDoacao (nome) VALUES ('ROTARY');
INSERT INTO tbOrigemDoacao (nome) VALUES ('DRIVE THRU');
INSERT INTO tbOrigemDoacao (nome) VALUES ('AVULSO');
INSERT INTO tbOrigemDoacao (nome) VALUES ('ESTOQUE');

-- 5. Inserir lista completa de produtos (usando codUni=5 para UNIDADES)
INSERT INTO tbLista (descricao, peso, unidade, quantidade, codUni) VALUES
('AÇUCAR 1KG',1000,'QUILOGRAMAS (KG)',0,5),
('ABSORVENTE',0,'UNIDADES (UN)',0,5),
('ACHOCOLATADO 250G',250,'UNIDADES (UN)',0,5),
('ADOÇANTE',200,'MILILITROS (ML)',0,5),
('AGUA 1,5L',1500,'LITROS (L)',0,5),
('AGUA SANITARIA 1L',1000,'LITROS (L)',0,5),
('ALCOOL 1L',1000,'LITROS (L)',0,5),
('ALCOOL 70 1L',1000,'LITROS (L)',0,5),
('ALCOOL GEL',60,'LITROS (L)',0,5),
('AMACIANTE 2L',2000,'LITROS (L)',0,5),
('AMIDO DE MILHO 200G',200,'GRAMAS (G)',0,5),
('AREIA DE GATO 4KG',4000,'GRAMAS (G)',0,5),
('ARROZ 1KG',1000,'QUILOGRAMAS (KG)',0,5),
('ARROZ 2KG',2000,'QUILOGRAMAS (KG)',0,5),
('ARROZ 5KG',5000,'QUILOGRAMAS (KG)',0,5),
('ATUM',170,'GRAMAS (G)',0,5),
('AVEIA 300G',300,'GRAMAS (G)',0,5),
('AZEITONA',100,'GRAMAS (G)',0,5),
('BISCOITO 140G',140,'GRAMAS (G)',0,5),
('CAFE 1KG',1000,'QUILOGRAMAS (KG)',0,5),
('CAFE 250G',250,'GRAMAS (G)',0,5),
('CANJICA 500G',500,'GRAMAS (G)',0,5),
('CESTA BASICA',10300,'QUILOGRAMAS (KG)',0,5),
('CHA',200,'GRAMAS (G)',0,5),
('CHOCOLATE 200G',200,'GRAMAS (G)',0,5),
('COCO RALADO 50G',50,'GRAMAS (G)',0,5),
('CREME DE LEITE 200G',200,'GRAMAS (G)',0,5),
('CREME DENTAL',180,'GRAMAS (G)',0,5),
('DESINFETANTE 2L',2000,'LITROS (L)',0,5),
('DETERGENTE',500,'MILILITROS (ML)',0,5),
('ERVILHA',170,'GRAMAS (G)',0,5),
('FARINHA DE TRIGO 1KG',1000,'GRAMAS (G)',0,5),
('FEIJAO 1KG',1000,'QUILOGRAMAS (KG)',0,5),
('FUBA 500G',500,'GRAMAS (G)',0,5),
('FUBA 400G',400,'GRAMAS (G)',0,5),
('GELATINA',20,'GRAMAS (G)',0,5),
('LEITE 1L',1000,'LITROS (L)',0,5),
('LEITE CONDENSADO',395,'MILILITROS (ML)',0,5),
('LEITE EM PO 400G',400,'GRAMAS (G)',0,5),
('MACARRAO 500G',500,'GRAMAS (G)',0,5),
('MILHO',170,'GRAMAS (G)',0,5),
('MIOJO',85,'GRAMAS (G)',0,5),
('MOLHO DE TOMATE',300,'GRAMAS (G)',0,5),
('MUCILON 400G',400,'GRAMAS (G)',0,5),
('NESTON 400G',400,'GRAMAS (G)',0,5),
('OLEO 900ML',900,'GRAMAS (G)',0,5),
('SAL 1KG',1000,'QUILOGRAMAS (KG)',0,5),
('SARDINHA',250,'GRAMAS (G)',0,5),
('SUCO',25,'GRAMAS (G)',0,5),
('VINAGRE',750,'MILILITROS (ML)',0,5),
('TEMPERO',300,'MILILITROS (ML)',0,5);

-- 6. Inserir modelo de cesta
INSERT INTO tbModeloCesta(descricao) VALUES('CESTA BASICA PADRAO');

-- 7. Inserir itens do modelo de cesta (usando os códigos gerados)

INSERT INTO tbItensDoModeloCesta(codModelo, codList, quantidadeMinima)
VALUES
(1, (SELECT codList FROM tbLista WHERE descricao = 'ARROZ 5KG'), 1),
(1, (SELECT codList FROM tbLista WHERE descricao = 'ARROZ 2KG'), 1),
(1, (SELECT codList FROM tbLista WHERE descricao = 'ARROZ 1KG'), 1),
(1, (SELECT codList FROM tbLista WHERE descricao = 'FEIJAO 1KG'), 1),
(1, (SELECT codList FROM tbLista WHERE descricao = 'LEITE 1L'), 1),
(1, (SELECT codList FROM tbLista WHERE descricao = 'FUBA 400G'), 1),
(1, (SELECT codList FROM tbLista WHERE descricao = 'FUBA 500G'), 1),
(1, (SELECT codList FROM tbLista WHERE descricao = 'MOLHO DE TOMATE'), 1),
(1, (SELECT codList FROM tbLista WHERE descricao = 'AÇUCAR 1KG'), 1),
(1, (SELECT codList FROM tbLista WHERE descricao = 'SAL 1KG'), 1),
(1, (SELECT codList FROM tbLista WHERE descricao = 'OLEO 900ML'), 1);

INSERT INTO tbEstoqueItens (codList, quantidade)
SELECT codList, 0 FROM tbLista
WHERE codList NOT IN (SELECT codList FROM tbEstoqueItens);

UPDATE tbEstoqueItens
SET quantidade = 10
WHERE codList IN (1,2,3,4,5);


INSERT INTO tbEstoqueItens (codList, quantidade)
SELECT codList, 0 FROM tbLista
WHERE codList NOT IN (SELECT codList FROM tbEstoqueItens);

-- Inserir registros faltantes em tbEstoqueItens
INSERT INTO tbEstoqueItens (codList, quantidade, dataMovimentacao, horaMovimentacao)
SELECT l.codList, 0, CURDATE(), CURTIME()
FROM tbLista l
LEFT JOIN tbEstoqueItens ei ON ei.codList = l.codList
WHERE ei.codList IS NULL;


-- =============================================
-- TRIGGERS
-- =============================================
-- =============================================
-- TRIGGERS (VERSÃO FINAL CORRIGIDA)
-- =============================================
DELIMITER $$

-- 1. Trigger para criar registro em tbEstoqueItens quando um produto é criado
DROP TRIGGER IF EXISTS trg_CriarRegistroEstoque$$
CREATE TRIGGER trg_CriarRegistroEstoque
AFTER INSERT ON tbLista
FOR EACH ROW
BEGIN
    INSERT INTO tbEstoqueItens (codList, quantidade, dataMovimentacao, horaMovimentacao)
    VALUES (NEW.codList, 0, CURDATE(), CURTIME());
END$$

-- 2. Trigger para entrada em tbProdutos (quantidade positiva)
DROP TRIGGER IF EXISTS trg_AtualizarEstoque_Entrada$$
CREATE TRIGGER trg_AtualizarEstoque_Entrada
AFTER INSERT ON tbProdutos
FOR EACH ROW
BEGIN
    IF NEW.quantidade > 0 AND NEW.tipoMovimentacao = 'ENTRADA' THEN
        UPDATE tbEstoqueItens
        SET quantidade = quantidade + NEW.quantidade, 
            dataMovimentacao = CURDATE(), 
            horaMovimentacao = CURTIME()
        WHERE codList = NEW.codList;
    END IF;
END$$

-- 3. Trigger para saída em tbProdutos (quantidade negativa)
DROP TRIGGER IF EXISTS trg_AtualizarEstoque_Saida$$
CREATE TRIGGER trg_AtualizarEstoque_Saida
AFTER INSERT ON tbProdutos
FOR EACH ROW
BEGIN
    IF NEW.quantidade < 0 AND NEW.tipoMovimentacao = 'SAIDA' THEN
        UPDATE tbEstoqueItens
        SET quantidade = quantidade + NEW.quantidade, -- NEW.quantidade é negativo
            dataMovimentacao = CURDATE(), 
            horaMovimentacao = CURTIME()
        WHERE codList = NEW.codList;
    END IF;
END$$

-- 4. Trigger para validar saída em tbProdutos
DROP TRIGGER IF EXISTS trg_ValidarEstoque_Saida$$
CREATE TRIGGER trg_ValidarEstoque_Saida
BEFORE INSERT ON tbProdutos
FOR EACH ROW
BEGIN
    DECLARE estoqueAtual INT;
    
    IF NEW.quantidade < 0 AND NEW.tipoMovimentacao = 'SAIDA' THEN
        SELECT COALESCE(quantidade, 0)
        INTO estoqueAtual
        FROM tbEstoqueItens
        WHERE codList = NEW.codList;
        
        IF estoqueAtual < ABS(NEW.quantidade) THEN
            SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Estoque insuficiente para a saída';
        END IF;
    END IF;
END$$

-- 5. Trigger para saída em tbItensCesta (cestas)
DROP TRIGGER IF EXISTS trg_AtualizarEstoque_SaidaCesta$$
CREATE TRIGGER trg_AtualizarEstoque_SaidaCesta
AFTER INSERT ON tbItensCesta
FOR EACH ROW
BEGIN
    UPDATE tbEstoqueItens
    SET quantidade = quantidade - NEW.quantidade, 
        dataMovimentacao = CURDATE(), 
        horaMovimentacao = CURTIME()
    WHERE codList = NEW.codList;
END$$

-- 6. Trigger para validar saída em tbItensCesta
DROP TRIGGER IF EXISTS trg_ValidarEstoque_SaidaCesta$$
CREATE TRIGGER trg_ValidarEstoque_SaidaCesta
BEFORE INSERT ON tbItensCesta
FOR EACH ROW
BEGIN
    DECLARE estoqueAtual INT;

    SELECT COALESCE(quantidade, 0)
    INTO estoqueAtual
    FROM tbEstoqueItens
    WHERE codList = NEW.codList;

    IF estoqueAtual < NEW.quantidade THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Estoque insuficiente para a cesta';
    END IF;
END$$

DELIMITER ;


-- =============================================
-- MELHORIAS ADICIONAIS
-- =============================================

-- Padronizar produtos sem acentos
UPDATE tbLista 
SET descricao = UPPER(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
    REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
        REPLACE(REPLACE(descricao,
        'Á', 'A'), 'À', 'A'), 'Ã', 'A'), 'Â', 'A'), 'Ä', 'A'),
        'É', 'E'), 'Ê', 'E'), 'Í', 'I'), 'Ó', 'O'), 'Ô', 'O'),
        'Ú', 'U'), 'Ç', 'C'));

-- Padronizar origens sem acentos
UPDATE tbOrigemDoacao 
SET nome = UPPER(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
    REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
        REPLACE(REPLACE(nome,
        'Á', 'A'), 'À', 'A'), 'Ã', 'A'), 'Â', 'A'), 'Ä', 'A'),
        'É', 'E'), 'Ê', 'E'), 'Í', 'I'), 'Ó', 'O'), 'Ô', 'O'),
        'Ú', 'U'), 'Ç', 'C'));

-- Adicionar índices para performance
CREATE INDEX idx_produtos_codList ON tbProdutos(codList);
CREATE INDEX idx_produtos_dataEntrada ON tbProdutos(dataDeEntrada);
CREATE INDEX idx_produtos_tipoMov ON tbProdutos(tipoMovimentacao);
CREATE INDEX idx_estoque_codList ON tbEstoqueItens(codList);
CREATE INDEX idx_lista_descricao ON tbLista(descricao);

-- Verificar integridade dos dados
SELECT '=== VERIFICAÇÃO DE INTEGRIDADE ===' as status;
SELECT COUNT(*) as total_produtos FROM tbLista;
SELECT COUNT(*) as total_estoque FROM tbEstoqueItens;
SELECT COUNT(*) as total_movimentacoes FROM tbProdutos;
SELECT COUNT(*) as total_usuarios FROM tbUsuarios;

-- Verificar se todos os produtos têm registro em tbEstoqueItens
SELECT l.descricao as produtos_sem_estoque
FROM tbLista l
LEFT JOIN tbEstoqueItens ei ON ei.codList = l.codList
WHERE ei.codList IS NULL;

-- Se houver produtos sem registro, corrigir
INSERT INTO tbEstoqueItens (codList, quantidade, dataMovimentacao, horaMovimentacao)
SELECT l.codList, 0, CURDATE(), CURTIME()
FROM tbLista l
LEFT JOIN tbEstoqueItens ei ON ei.codList = l.codList
WHERE ei.codList IS NULL
ON DUPLICATE KEY UPDATE quantidade = quantidade;