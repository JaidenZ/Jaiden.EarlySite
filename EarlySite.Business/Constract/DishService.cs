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

        /// <summary>
        /// 根据编号精确获取单品食物
        /// </summary>
        /// <param name="dishId">单品食物编号</param>
        /// <returns></returns>
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

        /// <summary>
        /// 根据用餐时间获取单品食物
        /// </summary>
        /// <param name="time">用餐时间</param>
        /// <returns></returns>
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
        /// <summary>
        /// 根据名称模糊获取单品食物
        /// </summary>
        /// <param name="searchName">查询名字</param>
        /// <returns></returns>
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

        /// <summary>
        /// 根据单品食品类型获取信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public Result<IList<Dish>> SearchDishInfoByType(DishType type)
        {
            Result<IList<Dish>> result = new Result<IList<Dish>>()
            {
                Data = null,
                Status = true
            };
            try
            {
                IList<Dish> dish = DBConnectionManager.Instance.Reader.Select<Dish>(new DishSelectSpefication(type.GetHashCode().ToString(), 3).Satifasy());
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

        /// <summary>
        /// 分享单品食物信息
        /// </summary>
        /// <param name="share"></param>
        /// <returns></returns>
        public Result ShareDishInfo(DishShare share)
        {
            Result result = new Result()
            {
                Status = true,
                StatusCode = "SSD000"
            };
            try
            {


            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = "分享单品食物失败";
                result.StatusCode = "SSD001";
            }
            return result;

        }
    }
}
