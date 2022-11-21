using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Carb_Counter.Data
{
    public class DapperHelper
    {
        public async static Task<List<T>> LoadData<T>(string sql, string connectionStringName)
        {
            using (IDbConnection db = new SqlConnection(connectionStringName))
            {
                IEnumerable<T> results = await db.QueryAsync<T>(sql);
                return results.ToList();
            }
        }

        public async static Task<int> SaveData(string sql, object parms, string connectionStringName)
        {
            using (IDbConnection db = new SqlConnection(connectionStringName))
            {
                int result = await db.ExecuteAsync(sql, parms);
                return result;
            }
        }

        public async static Task<List<T>> LoadDataSP<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            using (IDbConnection connection = new SqlConnection(connectionStringName))
            {
                IEnumerable<T> rows = await connection.QueryAsync<T>(storedProcedure, parameters,
                        commandType: CommandType.StoredProcedure);

                return rows.ToList();
            }
        }

        public async static Task<int> SaveDataSP<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            using (IDbConnection connection = new SqlConnection(connectionStringName))
            {
                int result = await connection.ExecuteAsync(storedProcedure, parameters,
                   commandType: CommandType.StoredProcedure);

                return result;
            }
        }
    }
}
