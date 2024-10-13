using Microsoft.Data.Sqlite;
using System.Data;

namespace ShoppingCart.Api.Shared.Data
{
	internal sealed class SqlConnectionFactory : ISqlConnectionFactory
	{
		private readonly string _connectionString;

		public SqlConnectionFactory(string connectionString)
		{
			_connectionString = connectionString;
		}

		public IDbConnection CreateConnection()
		{
			var connection = new SqliteConnection(_connectionString);
			connection.Open();
			return connection;
		}
	}
}
