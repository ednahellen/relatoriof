using MySql.Data.MySqlClient;
using System;
using System.Data;

public class EstoqueRepository
{
    public int ObterCodList(MySqlConnection conn, MySqlTransaction trans, string produto)
    {
        string sql = "SELECT codList FROM tbLista WHERE descricao = @produto";

        using (var cmd = new MySqlCommand(sql, conn, trans))
        {
            cmd.Parameters.AddWithValue("@produto", produto);
            var result = cmd.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
        }
    }

    public int ObterSaldo(MySqlConnection conn, MySqlTransaction trans, int codList)
    {
        string sql = @"
            SELECT COALESCE(SUM(quantidade),0)
            FROM tbMovimentacoes
            WHERE codList = @codList";

        using (var cmd = new MySqlCommand(sql, conn, trans))
        {
            cmd.Parameters.AddWithValue("@codList", codList);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }

    public void InserirMovimentacao(MySqlConnection conn, MySqlTransaction trans,
        int codList, string tipo, int quantidade, int codUsu, string destino)
    {
        string sql = @"
            INSERT INTO tbMovimentacoes
            (codList, tipoMovimentacao, quantidade, dataMovimentacao, codUsu, codOri, destino)
            VALUES
            (@codList, @tipo, @quantidade, NOW(), @codUsu, 1, @destino)";

        using (var cmd = new MySqlCommand(sql, conn, trans))
        {
            cmd.Parameters.AddWithValue("@codList", codList);
            cmd.Parameters.AddWithValue("@tipo", tipo);
            cmd.Parameters.AddWithValue("@quantidade", quantidade);
            cmd.Parameters.AddWithValue("@codUsu", codUsu);
            cmd.Parameters.AddWithValue("@destino", destino);
            cmd.ExecuteNonQuery();
        }
    }

    public void AtualizarEstoque(MySqlConnection conn, MySqlTransaction trans, int codList)
    {
        string sql = @"
        INSERT INTO tbEstoqueItens (codList, quantidade, dataAtualizacao)
        VALUES (
            @codList,
            (SELECT COALESCE(SUM(quantidade),0) FROM tbMovimentacoes WHERE codList = @codList),
            NOW()
        )
        ON DUPLICATE KEY UPDATE
            quantidade = (SELECT COALESCE(SUM(quantidade),0) FROM tbMovimentacoes WHERE codList = @codList),
            dataAtualizacao = NOW();";

        using (var cmd = new MySqlCommand(sql, conn, trans))
        {
            cmd.Parameters.AddWithValue("@codList", codList);
            cmd.ExecuteNonQuery();
        }
    }
}