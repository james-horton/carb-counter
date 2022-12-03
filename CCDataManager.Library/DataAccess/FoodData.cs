using CCDataManager.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCDataManager.Library.DataAccess
{
    public class FoodData : IFoodData
    {
        private readonly ISqlDataAccess _sql;

        public FoodData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public async Task<List<FoodModel>> GetFoods(string userId)
        {
            var foods = await _sql.LoadDataSP<FoodModel, dynamic>("dbo.spFood_GetAll", new { UserId = userId }, "CCData");

            return foods;
        }

        public async Task<int> SaveFoodRecord(FoodModel food)
        {
            var output = await _sql.SaveDataSP("dbo.spInventory_Insert", food, "CCData");

            return output;
        }
    }
}
