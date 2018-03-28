namespace EarlySite.Business.Constract
{
    using System;
    using System.Collections.Generic;
    using Business.IService;
    using EarlySite.Drms.DBManager;
    using EarlySite.Drms.Spefication;
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
                IList<Dish> dish = DBConnectionManager.Instance.Reader.Select<Dish>(new DishSelectSpefication(dishId.ToString(), 0).Satifasy());
                if(dish != null && dish.Count > 0)
                {
                    result.Data = dish[0];
                }
                else
                {
                    result.Status = false;
                }
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
            Result<IList<Dish>> result = new Result<IList<Dish>>()
            {
                Data = null,
                Status = true
            };
            try
            {
                IList<Dish> dish = DBConnectionManager.Instance.Reader.Select<Dish>(new DishSelectSpefication(time.GetHashCode().ToString(),2).Satifasy());
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

        public Result<IList<Dish>> SearchDishInfoByName(string searchName)
        {
            Result<IList<Dish>> result = new Result<IList<Dish>>()
            {
                Data = null,
                Status = true
            };
            try
            {
                IList<Dish> dish = DBConnectionManager.Instance.Reader.Select<Dish>(new DishSelectSpefication(searchName, 1).Satifasy());
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
