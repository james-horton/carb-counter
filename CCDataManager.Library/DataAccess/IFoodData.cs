using CCDataManager.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCDataManager.Library.DataAccess
{
    public interface IFoodData
    {
        Task<List<FoodModel>> GetFoods(string userId);
        Task<int> SaveFoodRecord(FoodModel food);
    }
}