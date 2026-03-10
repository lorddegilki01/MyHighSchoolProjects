using System.Configuration;
using MySqlConnector;

namespace MiniE_TicaretUygulaması.Data;

public static class Database
{
    public static string ConnectionString
    {
        get
        {
            var conn = ConfigurationManager.ConnectionStrings["MySqlConnection"];
            if (conn is null || string.IsNullOrWhiteSpace(conn.ConnectionString))
            {
                throw new InvalidOperationException("App.config içinde MySqlConnection tanımı bulunamadı.");
            }

            return conn.ConnectionString;
        }
    }

    public static MySqlConnection CreateConnection() => new(ConnectionString);
}

