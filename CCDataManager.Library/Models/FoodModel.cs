using System;

namespace CCDataManager.Library.Models
{
    public class FoodModel
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string ServingSize { get; set; }
        public decimal CarbQty { get; set; }
        public decimal CalorieQty { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
