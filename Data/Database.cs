using System;
using System.Data;
using Microsoft.Data.Sqlite;
using Dapper;

namespace CarDealerApp.Data
{
    public class Database : IDisposable
    {
        private readonly SqliteConnection _conn;

        public Database(string path)
        {
            Console.WriteLine("Abrindo banco: " + path);
            _conn = new SqliteConnection($"Data Source={path}");
            _conn.Open();
        }

        public IDbConnection Connection => _conn;

        public void Dispose()
        {
            _conn?.Dispose();
        }
    }
}
