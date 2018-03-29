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
    using EarlySite.Model.Database;
    using EarlySite.Core.Utils;
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
                IList<DishInfo> dish = DBConnectionManager.Instance.Reader.Select<DishInfo>(new DishSelectSpefication(dishId.ToString(), 0).Satifasy());
                if (dish != null && dish.Count > 0)
                {
                    result.Data = dish[0].Copy<Dish>();
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
                IList<DishInfo> dish = DBConnectionManager.Instance.Reader.Select<DishInfo>(new DishSelectSpefication(time.GetHashCode().ToString(), 2).Satifasy());
                result.Data = dish.CopyList<DishInfo, Dish>();
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
                IList<DishInfo> dish = DBConnectionManager.Instance.Reader.Select<DishInfo>(new DishSelectSpefication(searchName, 1).Satifasy());
                result.Data = dish.CopyList<DishInfo, Dish>();
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
                IList<DishInfo> dish = DBConnectionManager.Instance.Reader.Select<DishInfo>(new DishSelectSpefication(type.GetHashCode().ToString(), 3).Satifasy());
                result.Data = dish.CopyList<DishInfo, Dish>();
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
                StatusCode = "SSD000",
                Message = "分享单品食物成功"
            };
            try
            {
                //新增一条单品记录
                bool cannext = false;
                DishInfo dishinfo = share.DishInfo.Copy<DishInfo>();
                if (dishinfo != null)
                {
                    cannext = DBConnectionManager.Instance.Writer.Insert(new DishAddSpefication(dishinfo).Satifasy());
                }
                //新增一条单品与食谱关系记录
                if (cannext)
                {
                    cannext = false;
                    //cannext = DBConnectionManager.Instance.Writer.Insert(new )

                }
                //新增一条单品与门店关系记录
                if (cannext)
                {
                    cannext = false;
                }
                //更新食谱信息与门店信息
                if (cannext)
                {
                    cannext = false;
                }
                
                if (!cannext)
                {
                    DBConnectionManager.Instance.Rollback();
                    result.Status = false;
                    result.Message = "分享单品食物失败,请确保请求数据合法";
                }
                else
                {
                    DBConnectionManager.Instance.Commit();
                }
            }
            catch (Exception ex)
            {
                DBConnectionManager.Instance.Rollback();
                result.Status = false;
                result.Message = "分享单品食物失败" + ex.Message;
                result.StatusCode = "SSD001";
            }
            return result;

        }
    }
}
