using GPSFA_WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Data;

public class EstoqueService
{
    private readonly EstoqueRepository _repo = new EstoqueRepository();

    public void RegistrarSaida(string produto, int quantidade, int codUsu, string destino)
    {
        using (var conn = DataBaseConnection.OpenConnection())
        using (var trans = conn.BeginTransaction())
        {
            try
            {
                int codList = _repo.ObterCodList(conn, trans, produto);

                if (codList == 0)
                    throw new Exception("Produto não encontrado");

                int saldo = _repo.ObterSaldo(conn, trans, codList);

                if (saldo < quantidade)
                    throw new Exception($"Estoque insuficiente. Disponível: {saldo}");

                _repo.InserirMovimentacao(conn, trans, codList, "SAIDA", -quantidade, codUsu, destino);
                _repo.AtualizarEstoque(conn, trans, codList);

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
        }
    }

    public int SincronizarProduto(MySqlConnection conn, MySqlTransaction trans,
        string produto, int quantidadePlanilha, int codUsu)
    {
        int codList = _repo.ObterCodList(conn, trans, produto);
        if (codList == 0) return 0;

        int saldoAtual = _repo.ObterSaldo(conn, trans, codList);
        int diferenca = quantidadePlanilha - saldoAtual;

        if (diferenca == 0) return 0;

        string tipo = diferenca > 0 ? "ENTRADA" : "SAIDA";

        _repo.InserirMovimentacao(conn, trans, codList, tipo, diferenca, codUsu, "SINCRONIZACAO PLANILHA");
        _repo.AtualizarEstoque(conn, trans, codList);

        return 1;
    }
}