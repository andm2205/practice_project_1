using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace practice_pw_1
{
    class DBUtils
    {
        public static MySqlConnection GetDBConnection()
        {
            string host = "localhost";
            int port = 3306;
            string database = "exercise1";
            string username = "root";
            string password = "0801";
            string connectionString = "" +
                "server = " + host + "; " +
                "database = " + database + "; " +
                "port = " + port + "; " +
                "username = " + username + "; " +
                "password = " + password + "; " +
                "charset = utf8;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            return connection;
        }

    }
}
