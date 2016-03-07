using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ServerIP
{
    class Query
    {

        public string name { get; set; }

        private Dictionary<string, string> parameters { get; set; }
        private MySqlConnection conn { get; }

        /// <summary>
        /// constructor
        /// </summary>
        public Query()
        {
            name = string.Empty;
            parameters = new Dictionary<string, string>();
            string myConnectionString = "server=localhost;Port=8192;uid=root;Database=ingineriaprogramarii;Password=ingineriaProgramarii";

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public MySqlDataReader ExecuteSelect()
        {
            if (this.name == null)
                throw new Exception("invalid query name");
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = this.conn;
            cmd.CommandText = QueryHelper.getQuery(this.name, this.parameters);
            MySqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        public void ExecuteNonSelect()
        {
            if (this.name == null)
                throw new Exception("invalid query name");
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = this.conn;
            cmd.CommandText = QueryHelper.getQuery(this.name, this.parameters);
            cmd.ExecuteNonQuery();
        }

        public void setParameter(string name, string value)
        {
            parameters.Add(name, value);
        }
    }
}
