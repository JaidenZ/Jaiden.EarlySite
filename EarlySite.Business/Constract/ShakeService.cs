namespace EarlySite.Business.Constract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EarlySite.Cache;
    using EarlySite.Cache.CacheBase;
    using EarlySite.Core.DDD.Service;
    using EarlySite.Core.Utils;
    using EarlySite.Drms.DBManager;
    using EarlySite.Drms.Spefication.ShakeSpefication;
    using EarlySite.Model.Common;
    using EarlySite.Model.Database;
    using EarlySite.Model.Show;
    using IService;

    /// <summary>
    /// 筛选服务
    /// </summary>
    public class ShakeService : IShakeService
    {

        Result<Dish> IShakeService.ShakeDish(ShakeParam param)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 筛选附近店铺
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Result<IList<Shop>> IShakeService.ShakeNearShops(ShakeParam param)
        {
            Result<IList<Shop>> result = new Result<IList<Shop>>()
            {
                Data = new List<Shop>(),
                Status = true,
                Message = "筛选附近店铺成功",
                StatusCode = "SNS000"
            };
            //1.根据搜索距离、当前点经纬度 进行拓展范围
            //  拓展范围方案:
            //  根据当前经纬度为原点,距离为远点直线距离拓展 II III  象限
            //  距离单位按米计算,获得左下 右上远点坐标,确定一个矩形
            //2.根据拓展的范围条件搜索符合条件的店铺
            try
            {
                //获取左下角坐标 第三象限 角度225度
                Position southwest = PositionUtils.CaculateFarawayPosition(new Position() { Longitude = param.Longitude, Latitude = param.Latitude }, param.NearDistance, 225);
                //获取右上角坐标 第一象限 角度45度
                Position northeast = PositionUtils.CaculateFarawayPosition(new Position() { Longitude = param.Longitude, Latitude = param.Latitude }, param.NearDistance, 45);

                IList<Shop> list = DBConnectionManager.Instance.Reader.Select<Shop>(new ShakeNearShopSpefication(southwest, northeast).Satifasy());

                result.Data = list;

            }

            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "筛选附近店铺异常," + ex.Message;
                result.StatusCode = "SNS001";
                //记录日志
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:ShakeNearShops() .ShakeService"), LogType.ErrorLog);
            }
            return result;
        }

        /// <summary>
        /// 筛选门店热门食物
        /// </summary>
        /// <param name="shopId">门店编号</param>
        /// <returns></returns>
        Result<IList<Dish>> IShakeService.ShakePopDishForShop(int shopId)
        {
            Result<IList<Dish>> result = new Result<IList<Dish>>()
            {
                Data = new List<Dish>(),
                Status = true,
                Message = "筛选门店热门单品成功",
                StatusCode = "SPD000"
            };
            try
            {
                if (shopId == 0)
                {
                    throw new ArgumentNullException("删选门店编号参数无法为0");
                }

                //食谱缓存服务
                IDishCache dishcache = ServiceObjectContainer.Get<IDishCache>();
                
                //从缓存里加载所有单品
                IList<Dish> dishlist = dishcache.GetDishInfoByShop(shopId).CopyList<DishInfo, Dish>();
                if (dishlist != null && dishlist.Count > 0)
                {
                    //降序修改时间排列 取前4
                    result.Data = dishlist.OrderByDescending(o => o.UpdateDate).Take(4).ToList();
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "筛选门店热门单品失败," + ex.Message;
                result.StatusCode = "SPD001";
                //记录日志
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:ShakePopDishForShop() .ShakeService"), LogType.ErrorLog);
            }

            return result;
        }
    }
}
