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
        protected FoodModel? foodOriginal;
        
        protected void Reset()
        {
            foodToInsert = null;
            foodToUpdate = null;
            foodOriginal = null;
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
            foodOriginal = (FoodModel) food.Clone();
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

            // restore original food values before edits were made
            if (foodOriginal != null)
            {
                food.Name = foodOriginal.Name;
                food.ServingSize = foodOriginal.ServingSize;
                food.CarbQty = foodOriginal.CarbQty;
                food.CalorieQty = foodOriginal.CalorieQty;
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
                if (id > 0)
                {
                    foodToInsert?.Id = id;
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
