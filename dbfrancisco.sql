-- APAGANDO O BANCO DE DADOS

DROP DATABASE dbfrancisco;

-- CRIANDO O BANCO DE DADOS
CREATE DATABASE dbfrancisco
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_general_ci;

-- ENTRANDO NO BANCO DE DADOS
USE dbfrancisco;

-- ALTER DATABASE dbfrancisco 
-- CHARACTER SET utf8mb4 
-- COLLATE utf8mb4_general_ci;

-- CRIANDO A TABELA DE VOLUNTÁRIOS
CREATE TABLE tbVoluntarios(

codVol INT NOT NULL AUTO_INCREMENT, -- 0
nome VARCHAR(100) NOT NULL, -- 1
telCel VARCHAR(15), -- 2
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
-- salt VARCHAR(64) NOT NULL,
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

-- CRIANDO A TABELA DE FORNECEDORES
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
descricao VARCHAR(100) NOT NULL,
peso INT NOT NULL,
unidade VARCHAR(20) NOT NULL,
quantidade INT NOT NULL,
codUni INT NOT NULL,
PRIMARY KEY(codList),
FOREIGN KEY(codUni) REFERENCES tbUnidades(codUni)
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

-- CRIANDO A TABELA DE PRODUTOS - regsitro da entrada
CREATE TABLE tbProdutos(
  codProd INT NOT NULL AUTO_INCREMENT,  
  descricao VARCHAR(100) NOT NULL,
  quantidade INT NOT NULL,
  peso DECIMAL (10,3) NOT NULL,
  unidade VARCHAR(20) NOT NULL,
  codBar VARCHAR(13) NOT NULL,
  dataDeEntrada DATETIME NOT NULL,
  dataDeValidade DATETIME NOT NULL,
  dataLimiteDeSaida DATETIME,
  tipoMovimentacao VARCHAR(20) DEFAULT 'ENTRADA', 
  codUsu INT NOT NULL,
  codOri INT NOT NULL,
  codList INT NOT NULL,
  PRIMARY KEY(codProd, codBar),
  FOREIGN KEY(codUsu) REFERENCES tbUsuarios(codUsu),
  FOREIGN KEY(codOri) REFERENCES tbOrigemDoacao(codOri),
  FOREIGN KEY(codList) REFERENCES tbLista(codList)
);

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

create table tbEstoqueItens(
  codList INT NOT NULL,
  quantidade INT NOT NULL DEFAULT 0,
  dataMovimentacao DATE,
  horaMovimentacao TIME,
  PRIMARY KEY (codList),
  FOREIGN KEY (codList) REFERENCES tbLista(codList)
);

DELIMITER $$

CREATE TRIGGER trg_CriarRegistroEstoque
AFTER INSERT ON tbLista
FOR EACH ROW
BEGIN
    INSERT INTO tbEstoqueItens (codList, quantidade)
    VALUES (NEW.codList, 0);
END$$

DELIMITER ;

DELIMITER $$

CREATE TRIGGER trg_AtualizarEstoque_Entrada
AFTER INSERT ON tbProdutos
FOR EACH ROW
BEGIN
    UPDATE tbEstoqueItens
    SET quantidade = quantidade + NEW.quantidade, dataMovimentacao = CURRENT_DATE(), horaMovimentacao = CURRENT_TIME()
    WHERE codList = NEW.codList;
END$$

DELIMITER ;

DELIMITER $$

CREATE TRIGGER trg_AtualizarEstoque_Saida
AFTER INSERT ON tbItensCesta
FOR EACH ROW
BEGIN
    UPDATE tbEstoqueItens
    SET quantidade = quantidade - NEW.quantidade, dataMovimentacao = CURRENT_DATE(), horaMovimentacao = CURRENT_TIME()
    WHERE codList = NEW.codList;
END$$

DELIMITER ;


DELIMITER $$

CREATE TRIGGER trg_ValidarEstoque_Saida
BEFORE INSERT ON tbItensCesta
FOR EACH ROW
BEGIN
    DECLARE estoqueAtual INT;

    SELECT quantidade
    INTO estoqueAtual
    FROM tbEstoqueItens
    WHERE codList = NEW.codList;

    IF estoqueAtual < NEW.quantidade THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Estoque insuficiente para realizar saida do produto';
    END IF;
END$$

DELIMITER ;

INSERT INTO tbVoluntarios
(nome, telCel, cpf, cep, rua, numero, complemento, bairro, cidade, estado)
VALUES
('Admin','(11)90000-0000','000.000.000-00','00000-000','Grupo Francisco','000','','Jd.Francisco','São Paulo','SP');

INSERT INTO tbUsuarios
(usuario, senha, tipo, codVol)
VALUES
('admin','123','ADMIN',1); 

-- INSERT INTO tbProdutos(codProd,nome,quantidade,peso,unidade,codBar,dataDeEntrada,dataDeValidade,dataLimiteDeSaida,codUsu)VALUES(1,'Arroz Branco',10,5,'KG','1234561234561','2025-09-16','2026-09-10','2026-07-30',1);

-- SELECT nome AS nomeProduto, SUM(quantidade) AS totalQuantidadeProdutos, FROM tbProdutos GROUP BY nome ORDER BY totalQuantidadeProdutos DESC, totalQuantidadeEstoque DESC LIMIT 8;

-- SELECT nome, SUM(quantidade) FROM tbProdutos WHERE codProd = 1;

-- SELECT nome AS nomeProduto, SUM(quantidade) FROM tbProdutos GROUP BY nome;

-- SELECT nome AS nomeProduto, SUM(quantidade) AS totalQuantidadeProdutos FROM tbProdutos GROUP BY nome ORDER BY totalQuantidadeProdutos DESC LIMIT 8;

-- INSERT INTO tbProdutos(codProd,nome,quantidade,peso,unidade,codBar,dataDeEntrada,dataDeValidade,dataLimiteDeSaida,codUsu)VALUES(2,'Feijão Carioca',5,1,'KG','1234561444888','2025-09-10','2026-09-05','2026-02-15',1);

-- SELECT p.nome AS nomeProduto, SUM(p.quantidade) AS totalQuantidadeProdutos FROM tbProdutos as p GROUP BY p.nome ORDER BY totalQuantidadeProdutos DESC LIMIT 8;

-- INSERT INTO tbProdutos(codProd,nome,quantidade,peso,unidade,codBar,dataDeEntrada,dataDeValidade,dataLimiteDeSaida,codUsu)VALUES(3,'Macarrão',20,500,'G','1234561555333','2025-06-10','2025-12-25','2026-03-05',1);

-- INSERT INTO tbProdutos(codProd,nome,quantidade,peso,unidade,codBar,dataDeEntrada,dataDeValidade,dataLimiteDeSaida,codUsu)VALUES(4,'Farinha de trigo',7,1,'KG','5468761566644','2025-09-11','2025-11-30','2026-12-28',1);

-- INSERINDO DADOS TBUNIDADES
INSERT INTO tbUnidades(codUni, descricao)VALUES(1,'QUILOGRAMAS (KG)');
INSERT INTO tbUnidades(codUni, descricao)VALUES(2,'GRAMAS (G)');
INSERT INTO tbUnidades(codUni, descricao)VALUES(3,'LITROS (L)');
INSERT INTO tbUnidades(codUni, descricao)VALUES(4,'MILILITROS (ML)');
INSERT INTO tbUnidades(codUni, descricao)VALUES(5,'UNIDADES (UN)');

-- INSERINDO DADOS TBFORNECEDORES
INSERT INTO tbOrigemDoacao(codOri, nome)VALUES(1,'ROTARY');

-- INSERT INTO tbLista(descricao, peso, unidade, codUni)
-- VALUES
-- ('ARROZ BRANCO TIPO 1 5KG', 5, 'QUILOGRAMAS (KG)', 1),
-- ('FEIJAO CARIOCA 1KG', 1, 'QUILOGRAMAS (KG)', 1),
-- ('ACUCAR CRISTAL 1KG', 1, 'QUILOGRAMAS (KG)', 1),
-- ('LEITE INTEGRAL 1L', 1, 'LITROS (L)', 3),
-- ('OLEO DE SOJA 900ML', 900, 'MILILITROS (ML)', 4),
-- ('MACARRAO PENNE 500G', 500, 'GRAMAS (G)', 2);

-- INSERT INTO tbModeloCesta(descricao)VALUES('CESTA BASICA PADRAO');

-- INSERT INTO tbItensDoModeloCesta(codModelo, codList, quantidadeMinima)
-- VALUES
-- (1, 1, 1),
-- (1, 2, 1),
-- (1, 3, 4),
-- (1, 4, 4);

-- INSERT INTO tbprodutos(descricao, quantidade, peso, unidade, codBar, dataDeEntrada, dataDeValidade, codUsu, codOri, codList)
-- VALUES
-- ('ARROZ BRANCO TIPO 1 5KG', 20, 5, 'QUILOGRAMAS (KG)', '7891000100011', '2026-02-10', '2026-06-15', 1, 1, 1),
-- ('ARROZ BRANCO TIPO 1 5KG', 15, 5, 'QUILOGRAMAS (KG)', '7891000100028', '2026-02-18', '2026-07-01', 1, 1, 1),
-- ('FEIJAO CARIOCA 1KG', 50, 1, 'QUILOGRAMAS (KG)', '7892000200035', '2026-02-05', '2026-05-20', 1, 1, 2),
-- ('FEIJAO CARIOCA 1KG', 30, 1, 'QUILOGRAMAS (KG)', '7892000200042', '2026-02-22', '2026-06-10', 1, 1, 2),
-- ('ACUCAR CRISTAL 1KG', 40, 1, 'QUILOGRAMAS (KG)', '7893000300059', '2026-02-12', '2026-08-30', 1, 1, 3),
-- ('ACUCAR CRISTAL 1KG', 25, 1, 'QUILOGRAMAS (KG)', '7893000300066', '2026-02-25', '2026-09-15', 1, 1, 3),
-- ('LEITE INTEGRAL 1L', 60, 1, 'LITROS (L)', '7894000400073', '2026-02-08', '2026-04-25', 1, 1, 4),
-- ('LEITE INTEGRAL 1L', 45, 1, 'LITROS (L)', '7894000400080', '2026-02-20', '2026-05-18', 1, 1, 4),
-- ('OLEO DE SOJA 900ML', 35, 900, 'MILILITROS (ML)', '7895000500097', '2026-02-03', '2026-10-01', 1, 1, 5),
-- ('OLEO DE SOJA 900ML', 20, 900, 'MILILITROS (ML)', '7895000500103', '2026-02-27', '2026-11-12', 1, 1, 5),
-- ('MACARRAO PENNE 500G', 70, 500, 'GRAMAS (G)', '7896000600110', '2026-02-15', '2026-07-20', 1, 1, 6),
-- ('MACARRAO PENNE 500G', 55, 500, 'GRAMAS (G)', '7896000600127', '2026-02-28', '2026-08-05', 1, 1, 6);

-- INSERT INTO tbCestas(dataDeSaida, codUsu)VALUES('2026-03-02', 1);

-- INSERT INTO tbItensCesta(codCes, codList, quantidade)
-- VALUES
-- (1, 1, 1),
-- (1, 2, 2),
-- (1, 3, 4),
-- (1, 4, 4),
-- (1, 5, 2);

-- SELECT l.descricao, imc.quantidadeMinima FROM tbItensDoModeloCesta AS imc INNER JOIN tbLista AS l ON l.codList = imc.codList WHERE imc.codModelo = 1;

-- Busca a quantidade atual de itens em estoque
-- SELECT l.descricao, imc.quantidadeMinima, IFNULL(SUM(p.quantidade),0) AS estoqueAtual FROM tbItensDoModeloCesta imc INNER JOIN tbLista l ON l.codList = imc.codList LEFT JOIN tbEstoqueItens p ON p.codList = l.codList WHERE imc.codModelo = 1 GROUP BY imc.codModelo, imc.codList, l.descricao, l.unidade, imc.quantidadeMinima;

-- SELECT L.codList, L.descricao, E.quantidade AS quantidadeEmEstoque, E.dataMovimentacao FROM tbLista L INNER JOIN tbEstoqueItens E ON L.codList = E.codList WHERE E.dataMovimentacao = '2026-03-02' ORDER BY L.descricao;

-- SELECT L.codList, L.descricao, E.quantidade AS quantidadeEmEstoque, E.dataMovimentacao, E.horaMovimentacao FROM tbLista L INNER JOIN tbEstoqueItens E ON L.codList = E.codList ORDER BY L.descricao;

-- Bsusca os itens exatos que foram incluidos em uma cesta
-- SELECT ipc.codCes, ipc.codList, l.descricao, ipc.quantidade, c.dataDeSaida, c.codUsu, c.codCli FROM tbItensCesta AS ipc INNER JOIN tbCestas AS c ON ipc.codCes = c.codCes INNER JOIN tbLista AS l ON ipc.codList = l.codList WHERE ipc.codCes = 1;