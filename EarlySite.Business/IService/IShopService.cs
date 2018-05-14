namespace EarlySite.Business.IService
{
    using Model.Show;
    using Model.Common;
    using Model.Enum;
    using System.Collections.Generic;
    using Core.DDD.Service;
    using EarlySite.Business.Filter;

    /// <summary>
    /// 门店操作服务
    /// </summary>
    [ServiceObject(ServiceName = "门店操作服务", ServiceFilter = typeof(ShopServiceFilter))]
    public interface IShopService : IServiceBase
    {
        /// <summary>
        /// 根据店铺昵称模糊搜索
        /// </summary>
        /// <param name="shopName"></param>
        /// <returns></returns>
        Result<PageList<Shop>> SearchShopInfoByName(string shopName ,PageSearchParam param);

        /// <summary>
        /// 获取店铺分页信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Result<PageList<Shop>> GetShopPageList(PageSearchParam param);

        /// <summary>
        /// 根据店铺编号获取信息
        /// </summary>
        /// <param name="shopId">店铺编号</param>
        /// <returns></returns>
        Result<Shop> GetShopInfoById(int shopId);

        /// <summary>
        /// 创建门店信息
        /// </summary>
        /// <param name="shopInfo"></param>
        /// <returns></returns>
        Result CreatShopInfo(Shop shopInfo);

        /// <summary>
        /// 更新门店信息
        /// </summary>
        /// <param name="shopInfo"></param>
        /// <returns></returns>
        Result UpdateShopInfo(Shop shopInfo);

        /// <summary>
        /// 根据店铺编号移除店铺信息
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        Result RemoveShopInfoById(int shopId);

    }
}
