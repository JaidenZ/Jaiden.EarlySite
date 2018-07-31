namespace EarlySite.Business.Constract
{
    using System.Collections.Generic;
    using EarlySite.Model.Common;
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
        

        Result<IList<Shop>> IShakeService.ShakeNearShops(ShakeParam param)
        {
            throw new System.NotImplementedException();
        }
    }
}
