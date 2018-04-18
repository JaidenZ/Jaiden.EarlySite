namespace EarlySite.Business.IService
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
    }
}
