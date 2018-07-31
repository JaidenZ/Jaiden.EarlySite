namespace EarlySite.Business.Constract
{
    using System.Collections.Generic;
    using EarlySite.Model.Show;
    using IService;

    /// <summary>
    /// 筛选服务
    /// </summary>
    public class ShakeService : IShakeService
    {
        Dish IShakeService.ShakeDish(ShakeParam param)
        {
            throw new System.NotImplementedException();
        }

        IList<Shop> IShakeService.ShakeNearShops(ShakeParam param)
        {
            throw new System.NotImplementedException();
        }
    }
}
