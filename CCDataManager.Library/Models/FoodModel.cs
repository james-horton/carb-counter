using System;

namespace CCDataManager.Library.Models
{
    public class FoodModel : ICloneable
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string ServingSize { get; set; }
        public decimal CarbQty { get; set; }
        public decimal CalorieQty { get; set; }
        public DateTime DateAdded { get; set; }
        public FoodModel()
        {
        }

        public FoodModel(
            long Id, 
            string UserId, 
            string Name, 
            string ServingSize, 
            decimal CarbQty, 
            decimal CalorieQty, 
            DateTime DateAdded)
        {
            this.Id = Id;
            this.UserId = UserId;
            this.Name = Name;
            this.ServingSize = ServingSize;
            this.CarbQty = CarbQty;
            this.CalorieQty= CalorieQty;
            this.DateAdded = DateAdded;
        }

        public object Clone()
        {
            return new FoodModel(
                this.Id,
                this.UserId,
                this.Name,
                this.ServingSize,
                this.CarbQty,
                this.CalorieQty,
                this.DateAdded);
        }       
    }
}
