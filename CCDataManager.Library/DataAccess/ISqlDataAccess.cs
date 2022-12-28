using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCDataManager.Library.DataAccess
{
    public interface ISqlDataAccess
    {
        string GetConnectionString(string name);
        Task<List<T>> LoadDataAsync<T>(string sql, object parms, string connectionStringName);
        Task<List<T>> LoadDataAsync<T>(string sql, string connectionStringName);
        Task<List<T>> LoadDataSPAsync<T, U>(string storedProcedure, U parameters, string connectionStringName);
        Task<int> SaveDataAsync(string sql, object parms, string connectionStringName);
        Task<int> SaveDataSPAsync<T>(string storedProcedure, T parameters, string connectionStringName);
        Task<object> ExecuteScalarSPAsync<T>(string storedProcedure, T parameters, string connectionStringName);
    }
}