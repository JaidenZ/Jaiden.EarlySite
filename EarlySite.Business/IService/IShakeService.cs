namespace EarlySite.Business.IService
{
    using System;
    using System.Collections;
    using Model.Show;
    using Model.Common;
    using Core.DDD.Service;
    using EarlySite.Business.Filter;
    using System.Collections.Generic;


    /// <summary>
    /// 筛选服务
    /// </summary>
    [ServiceObject(ServiceName = "筛选操作服务", ServiceFilter = typeof(ShakeServiceFilter))]
    public interface IShakeService:IServiceBase
    {

        /// <summary>
        /// 筛选出附近的商店
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        IList<Shop> ShakeNearShops(ShakeParam param);


        /// <summary>
        /// 筛选符合条件的美食
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Dish ShakeDish(ShakeParam param);



    }
}
