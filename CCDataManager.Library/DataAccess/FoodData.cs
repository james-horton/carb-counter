using CCDataManager.Library.Models;
using System;
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

        public async Task<List<FoodModel>> GetFoodsAsync(string userId)
        {
            var foods = await _sql.LoadDataSPAsync<FoodModel, dynamic>("dbo.spFood_GetAll", new { UserId = userId }, "CCData");

            return foods;
        }

        public async Task<long> InsertFoodAsync(string userId, FoodModel food)
        {         
            food.UserId = userId;
            food.DateAdded = DateTime.UtcNow;            

            var id = (long) await _sql.ExecuteScalarSPAsync("dbo.spFood_Insert", food, "CCData");
            
            return id;
        }

        public async Task<int> UpdateFoodAsync(string userId, FoodModel food)
        {
            var parms = new
            {
                food.Id,
                UserId = userId,
                food.Name,
                food.ServingSize,
                food.CarbQty,
                food.CalorieQty
            };

            var output = await _sql.SaveDataSPAsync("dbo.spFood_Update", parms, "CCData");

            return output;
        }

        public async Task<int> DeleteFoodAsync(string userId, long id)
        {
            var parms = new
            {
                Id = id,
                UserId = userId
            };

            var output = await _sql.SaveDataSPAsync("dbo.spFood_Delete", parms, "CCData");

            return output;
        }
    }
}
