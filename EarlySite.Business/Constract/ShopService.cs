namespace EarlySite.Business.Constract
{
    using System;
    using System.Collections.Generic;
    using Business.IService;
    using EarlySite.Model.Common;
    using EarlySite.Model.Enum;
    using EarlySite.Model.Show;

    public class ShopService : IShopService
    {
        public Result CreatShopInfo(Shop shopInfo)
        {
            throw new NotImplementedException();
        }

        public Result<Shop> GetShopInfoById(int shopId)
        {
            throw new NotImplementedException();
        }

        public Result RemoveShopInfoById(int shopId)
        {
            throw new NotImplementedException();
        }

        public Result<IList<Shop>> SearchShopInfoByName(string shopName)
        {
            throw new NotImplementedException();
        }

        public Result UpdateShopInfo(Shop shopInfo)
        {
            throw new NotImplementedException();
        }
    }
}
