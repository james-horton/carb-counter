using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCDataManager.Library.DataAccess
{
    public interface ISqlDataAccess
    {
        string GetConnectionString(string name);
        Task<List<T>> LoadData<T>(string sql, object parms, string connectionStringName);
        Task<List<T>> LoadData<T>(string sql, string connectionStringName);
        Task<List<T>> LoadDataSP<T, U>(string storedProcedure, U parameters, string connectionStringName);
        Task<int> SaveData(string sql, object parms, string connectionStringName);
        Task<int> SaveDataSP<T>(string storedProcedure, T parameters, string connectionStringName);
    }
}