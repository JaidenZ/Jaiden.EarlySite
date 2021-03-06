﻿namespace EarlySite.Business.IService
{
    using Model.Show;
    using Model.Common;
    using Model.Enum;
    using Core.DDD.Service;
    using EarlySite.Business.Filter;
    using System.Collections.Generic;

    /// <summary>
    /// 单品食物服务
    /// </summary>
    [ServiceObject(ServiceName = "单品食物服务", ServiceFilter = typeof(DishServiceFilter))]
    public interface IDishService: IServiceBase
    {
        /// <summary>
        /// 根据编号精确获取单品食物
        /// </summary>
        /// <param name="dishId">单品食物编号</param>
        /// <returns></returns>
        Result<Dish> SearchDishInfoById(int dishId);


        /// <summary>
        /// 获取单品分页信息
        /// </summary>
        /// <returns></returns>
        Result<PageList<Dish>> GetPageDishInfo(PageSearchParam param);

        /// <summary>
        /// 根据名称模糊获取单品食物
        /// </summary>
        /// <param name="searchName">查询名字</param>
        /// <returns></returns>
        Result<PageList<Dish>> SearchDishInfoByName(string searchName, PageSearchParam param);

        /// <summary>
        /// 根据用餐时间获取单品食物
        /// </summary>
        /// <param name="time">用餐时间</param>
        /// <returns></returns>
        Result<PageList<Dish>> SearchDishInfoByMealTime(MealTime time, PageSearchParam param);

        /// <summary>
        /// 根据单品食品类型获取信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        Result<PageList<Dish>> SearchDishInfoByType(DishType type, PageSearchParam param);

        /// <summary>
        /// 分享单品食物信息
        /// </summary>
        /// <param name="share"></param>
        /// <returns></returns>
        Result ShareDishInfo(DishShare share);

        /// <summary>
        /// 收藏单品食物信息
        /// </summary>
        /// <param name="share"></param>
        /// <returns></returns>
        Result CollectDishInfo(DishCollect collect);

        /// <summary>
        /// 根据手机号获取分享的单品集合
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        Result<IList<Dish>> GetShareDishList(long phone);

        /// <summary>
        /// 根据食谱编号获取收藏的单品集合
        /// </summary>
        /// <param name="recipesId"></param>
        /// <returns></returns>
        Result<IList<Dish>> GetCollectDishList(int recipesId);

        /// <summary>
        /// 根据手机号获取喜爱的单品集
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Result<IList<Dish>> GetFavoriteDishByPhone(long phone);

    }
}
