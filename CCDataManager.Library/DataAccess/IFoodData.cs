using CCDataManager.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCDataManager.Library.DataAccess
{
    public interface IFoodData
    {
        Task<List<FoodModel>> GetFoodsAsync(string userId);
        Task<long> InsertFoodAsync(string userId, FoodModel food);
        Task<int> UpdateFoodAsync(string userId, FoodModel food);
        Task<int> DeleteFoodAsync(string userId, long id);
    }
}