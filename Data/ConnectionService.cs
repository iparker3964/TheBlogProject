using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogProject.Data
{
    public class ConnectionService
    {
        public static string GetConnectionString(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }
        private static string BuildConnectionString(string databaseUrl)
        {
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            return new NpgsqlConnectionStringBuilder()
            {
                Host = "containers-us-west-164.railway.app",
                Port = 7070,
                Username = "postgres",
                Password = "ry3HdoTd2q4InR9BWXF5",
                Database = "railway",
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            }.ToString();
            //return new NpgsqlConnectionStringBuilder()
            //{
            //    Host = databaseUri.Host,
            //    Port = databaseUri.Port,
            //    Username = userInfo[0],
            //    Password = userInfo[1],
            //    Database = databaseUri.LocalPath.TrimStart('/'),
            //    SslMode = SslMode.Require,
            //    TrustServerCertificate = true
            //}.ToString();
        }
    }
}
