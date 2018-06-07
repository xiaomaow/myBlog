using MySql.Data.MySqlClient;
using System.Configuration;

namespace MyBlog.Web.Service
{
    public class DBHelper
    {
        public static MySqlConnection MySqlConnection()
        {
            string mysqlconnectionString = ConfigurationManager.ConnectionStrings["mysqlconnectionString"].ToString();
            var connection = new MySqlConnection(mysqlconnectionString);
            connection.Open();
            return connection;
        }
    }
}