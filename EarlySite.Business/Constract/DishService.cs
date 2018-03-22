namespace EarlySite.Business.Constract
{
    using System;
    using System.Collections.Generic;
    using Business.IService;
    using EarlySite.Model.Common;
    using EarlySite.Model.Enum;
    using EarlySite.Model.Show;

    public class DishService : IDishService
    {
        public Result<Dish> SearchDishInfoById(int dishId)
        {
            Result<Dish> result = new Result<Dish>()
            {
                Data = null,
                Status = true
            };
            try
            {
                Dish dish = new Dish();
                result.Data = dish;

            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "查询单品食物出错:" + ex.Message;
                result.StatusCode = "SD001";
            }
            
            return result;
        }

        public Result<IList<Dish>> SearchDishInfoByMealTime(MealTime time)
        {
            throw new NotImplementedException();
        }

        public Result<IList<Dish>> SearchDishInfoByName(string searchName)
        {
            throw new NotImplementedException();
        }

        public Result<IList<Dish>> SearchDishInfoByType(DishType type)
        {
            throw new NotImplementedException();
        }

        public Result ShareDishInfo(DishShare share)
        {
            throw new NotImplementedException();
        }
    }
}
