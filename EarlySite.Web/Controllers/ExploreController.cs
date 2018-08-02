namespace EarlySite.Web.Controllers
{

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using EarlySite.Business.IService;
    using EarlySite.Core.DDD.Service;
    using EarlySite.Core.Utils;
    using EarlySite.Model.Common;
    using EarlySite.Model.Enum;
    using EarlySite.Model.Show;

    /// <summary>
    /// 发现视图控制器
    /// </summary>
    public class ExploreController : BaseController
    {
        
        /// <summary>
        /// 发现主页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            //获取用户数据
            ViewBag.Account = base.CurrentAccount;
            //获取当前定位
            Position position = base.CurrentPosition;
            //筛选附近店铺
            ShakeParam param = new ShakeParam()
            {
                Longitude = position.Longitude,
                Latitude = position.Latitude,
                NearDistance = base.SearchDistance
            };
            Result<IList<Shop>> shoplist = ServiceObjectContainer.Get<IShakeService>().ShakeNearShops(param);
            ViewBag.NearShop = shoplist.Data;



            return View();
        }

        /// <summary>
        /// 附近商家分布式图
        /// </summary>
        /// <param name="nearshops"></param>
        /// <returns></returns>
        public PartialViewResult NearShopPartialView(IList<Shop> nearshops)
        {
            return PartialView(nearshops);
        }


    }
}