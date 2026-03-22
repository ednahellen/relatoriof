using MySql.Data.MySqlClient;
using System.Data;

namespace GPSFA_WinForms
{
    public class ProductRepository
    {
        public static DataTable BuscarTodosProdutos()
        {
            DataTable dt = new DataTable();
            using (MySqlCommand comm = new MySqlCommand())
            {
                comm.CommandText = "SELECT descricao, quantidade, peso, unidade, codBar, dataDeEntrada, dataDeValidade FROM tbprodutos ORDER BY dataDeEntrada DESC;";
                comm.Connection = DataBaseConnection.OpenConnection();

                MySqlDataAdapter da = new MySqlDataAdapter(comm);
                da.Fill(dt);

                DataBaseConnection.CloseConnection();
            }

            return dt;
        }
    }
}
