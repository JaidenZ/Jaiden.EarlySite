﻿namespace EarlySite.Business.IService
{
    using Model.Show;
    using Model.Common;
    using Model.Enum;
    using System.Collections.Generic;
    using Core.DDD.Service;

    /// <summary>
    /// 单品食物服务
    /// </summary>
    public interface IDishService: IServiceBase
    {
        /// <summary>
        /// 根据编号精确获取单品食物
        /// </summary>
        /// <param name="dishId">单品食物编号</param>
        /// <returns></returns>
        Result<Dish> SearchDishInfoById(int dishId);

        /// <summary>
        /// 根据名称模糊获取单品食物
        /// </summary>
        /// <param name="searchName">查询名字</param>
        /// <returns></returns>
        Result<IList<Dish>> SearchDishInfoByName(string searchName);

        /// <summary>
        /// 根据用餐时间获取单品食物
        /// </summary>
        /// <param name="time">用餐时间</param>
        /// <returns></returns>
        Result<IList<Dish>> SearchDishInfoByMealTime(MealTime time);

        /// <summary>
        /// 根据单品食品类型获取信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        Result<IList<Dish>> SearchDishInfoByType(DishType type);

        /// <summary>
        /// 分享单品食物信息
        /// </summary>
        /// <param name="share"></param>
        /// <returns></returns>
        Result ShareDishInfo(DishShare share);

    }
}