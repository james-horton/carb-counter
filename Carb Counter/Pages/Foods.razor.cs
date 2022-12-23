using CCDataManager.Library.DataAccess;
using CCDataManager.Library.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;
using System.Security.Claims;

namespace Carb_Counter.Pages
{
    public class FoodsBase : ComponentBase
    {
        [Inject] AuthenticationStateProvider? AuthenticationStateProvider { get; set; }
        [Inject] IFoodData? FoodData { get; set; }

        protected RadzenDataGrid<FoodModel>? foodsGrid;
        protected List<FoodModel>? foods;
        protected FoodModel? foodToInsert;
        protected FoodModel? foodToUpdate;
        
        protected void Reset()
        {
            foodToInsert = null;
            foodToUpdate = null;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (AuthenticationStateProvider != null)
                {
                    var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

                    if (authState?.User?.Identity?.IsAuthenticated == true)
                    {
                        var userId = authState?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                        if (userId != null)
                        {
                            await LoadData(userId);
                        }
                    }
                }
            }
        }

        protected async Task LoadData(string userId)
        {
            foods = await FoodData.GetFoods(userId);
            StateHasChanged();
        }

        protected async Task EditRow(FoodModel food)
        {
            foodToUpdate = food;

            if (foodsGrid != null)
            {
                await foodsGrid.EditRow(food);
            }
        }

        protected void OnUpdateRow(FoodModel food)
        {
            if (food == foodToInsert)
            {
                foodToInsert = null;
            }

            foodToUpdate = null;

            //dbContext.Update(food);       

            // For production
            //dbContext.SaveChanges();
        }

        protected async Task SaveRow(FoodModel food)
        {
            if (foodsGrid != null)
            {
                await foodsGrid.UpdateRow(food);
            }
        }

        protected void CancelEdit(FoodModel food)
        {
            if (food == foodToInsert)
            {
                foodToInsert = null;
            }

            foodToUpdate = null;

            foodsGrid?.CancelEditRow(food);

            // For production
            //var foodEntry = dbContext.Entry(food);
            //if (foodEntry.State == EntityState.Modified)
            //{
            //    foodEntry.CurrentValues.SetValues(foodEntry.OriginalValues);
            //    foodEntry.State = EntityState.Unchanged;
            //}
        }
                
        protected async Task DeleteRow(FoodModel food)
        {
            if (food == foodToInsert)
            {
                foodToInsert = null;
            }

            if (food == foodToUpdate)
            {
                foodToUpdate = null;
            }

            if (foods != null && foods.Contains(food))
            {
                //dbContext.Remove<food>(food);

                // For demo purposes only
                foods.Remove(food);

                // For production
                //dbContext.SaveChanges();

                if (foodsGrid != null)
                {
                    await foodsGrid.Reload();
                }
            }
            else
            {
                foodsGrid?.CancelEditRow(food);
                if (foodsGrid != null)
                {
                    await foodsGrid.Reload();
                }
            }
        }

        protected async Task InsertRow()
        {
            foodToInsert = new FoodModel();

            if (foodsGrid != null)
            {
                await foodsGrid.InsertRow(foodToInsert);
            }
        }

        protected void OnCreateRow(FoodModel food)
        {
            //dbContext.Add(food);

            // For demo purposes only
            //food.Customer = dbContext.Customers.Find(food.CustomerID);
            //food.Employee = dbContext.Employees.Find(food.EmployeeID);

            // For production
            //dbContext.SaveChanges();
        }
    }
}
