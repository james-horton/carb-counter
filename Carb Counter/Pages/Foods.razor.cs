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

        // Used to restore the original values when edits are canceled. The reason a dictionary is used
        // instead of a single object is because the user can click to another row in the middle of an
        // edit, and a dictionary will keep track of the changes for all rows in the grid.
        protected Dictionary<long, FoodModel>? foodsOriginal = new();
        
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
                var userId = await GetAuthenticatedUserId();

                if (userId != null)
                {
                    await LoadData(userId);
                }
            }
        }

        protected async Task LoadData(string userId)
        {
            if (FoodData != null)
            { 
                foods = await FoodData.GetFoodsAsync(userId);
                StateHasChanged();
            }
        }

        protected async Task EditRow(FoodModel food)
        {
            if (foodsOriginal != null)
            {
                if (foodsOriginal.ContainsKey(food.Id) == false)
                {
                    var orignalFood = (FoodModel)food.Clone();
                    foodsOriginal[orignalFood.Id] = orignalFood;
                }
            }

            foodToUpdate = food;

            if (foodsGrid != null)
            {
                await foodsGrid.EditRow(food);
            }
        }

        protected async Task OnUpdateRow(FoodModel food)
        {
            if (food == foodToInsert)
            {
                foodToInsert = null;
            }

            foodToUpdate = null;

            var userId = await GetAuthenticatedUserId();

            if (userId != null && FoodData != null)
            {
                var result = await FoodData.UpdateFoodAsync(userId, food);
            }

            // reset any previous edits
            if (foodsOriginal != null && foodsOriginal.ContainsKey(food.Id))
            {
                foodsOriginal.Remove(food.Id);
            }
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

            // restore original food values before edits were made.
            if (foodsOriginal != null && foodsOriginal.ContainsKey(food.Id))
            {
                var id = food.Id;
                food.Name = foodsOriginal[id].Name;
                food.ServingSize = foodsOriginal[id].ServingSize;
                food.CarbQty = foodsOriginal[id].CarbQty;
                food.CalorieQty = foodsOriginal[id].CalorieQty;

                // reset previous edit.
                foodsOriginal.Remove(id);
            }

            foodsGrid?.CancelEditRow(food);
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
                var userId = await GetAuthenticatedUserId();
                if (userId != null && FoodData != null)
                {
                    var result = await FoodData.DeleteFoodAsync(userId, food.Id);
                                       
                    if (foodsGrid != null)
                    {
                        foods.Remove(food);
                        await foodsGrid.Reload();
                    }                    
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

        protected async Task OnCreateRow(FoodModel food)
        {
            var userId = await GetAuthenticatedUserId();

            if (userId != null && FoodData != null)
            {
                var id = await FoodData.InsertFoodAsync(userId, food);
                if (foodToInsert != null && id > 0)
                {
                    foodToInsert.Id = id;
                    foodToInsert = null;
                }
            }
        }

        protected async Task<string> GetAuthenticatedUserId()
        {
            string userId = "";
            if (AuthenticationStateProvider != null)
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

                if (authState?.User?.Identity?.IsAuthenticated == true)
                {
                    userId = authState?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                }

            }
            return userId;
        }
    }
}
