using Chloe.Infrastructure;
using MySql.Data.MySqlClient;
using System.Data;

namespace NineBizlogistics.DB
{
    public class MysqlConnectionFactory:IDbConnectionFactory
    {

        public MysqlConnectionFactory(string host, ushort port, string username, string pwd ,string database,bool limitdatabase=false )
        {
            this.DataBase = database;
            this.Server = host;
            this.Port = port;
            this.UserID = username;
            this.Password = pwd;
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Server = host;
            sb.Port = port;
            sb.UserID = username;
            sb.Password = pwd;           
            if(limitdatabase)
            {
                sb.Database = database;
            }
            this.ConnectString = sb.ConnectionString; ;
        }
        public string DataBase { get; set; }
        public string Server { get; set; }
        public ushort  Port { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        string ConnectString { get; set; }
        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(ConnectString);
        }

    }
}
