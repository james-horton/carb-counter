using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CCDataManager.Library.DataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;
        private readonly ILogger<SqlDataAccess> _logger;

        public SqlDataAccess(IConfiguration config, ILogger<SqlDataAccess> logger)
        {
            _config = config;
            _logger = logger;
        }

        public string GetConnectionString(string name)
        {
            return _config.GetConnectionString(name);
        }

        public async Task<List<T>> LoadDataAsync<T>(string sql, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                IEnumerable<T> results = await db.QueryAsync<T>(sql);
                return results.ToList();
            }
        }

        public async Task<List<T>> LoadDataAsync<T>(string sql, object parms, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                IEnumerable<T> results = await db.QueryAsync<T>(sql, parms);
                return results.ToList();
            }
        }

        public async Task<int> SaveDataAsync(string sql, object parms, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                int result = await db.ExecuteAsync(sql, parms);
                return result;
            }
        }

        public async Task<List<T>> LoadDataSPAsync<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                IEnumerable<T> rows = await connection.QueryAsync<T>(storedProcedure, parameters,
                        commandType: CommandType.StoredProcedure);

                return rows.ToList();
            }
        }

        public async Task<int> SaveDataSPAsync<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                int result = await connection.ExecuteAsync(storedProcedure, parameters,
                   commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<object> ExecuteScalarSPAsync<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var result = await connection.ExecuteScalarAsync(storedProcedure, parameters,
                   commandType: CommandType.StoredProcedure);

                return result;
            }
        }
    }
}
