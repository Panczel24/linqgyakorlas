using MySql.Data.MySqlClient;
using Mysqlx.Connection;
using System;
using System.Collections.Generic;
using System.Text;

namespace linqgyakorlas
{
    internal class Database
    {
        private string connectionString = "server=localhost; database=diakok;uid=root,pwd=;";
        public List<Diak> DiakokBetoltese()
        {
            List<Diak> diakok = new List<Diak>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM diakok";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    using(MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Diak diak = new Diak(
                               reader["nev"].ToString(),
                               Convert.ToInt32(reader["eletkor"]),
                               reader["osztaly"].ToString(),
                               Convert.ToDouble(reader["atlag"])
                            );
                            diakok.Add(diak);
                        }
                    }
                }
            }
            return diakok;
        }
    }
}
