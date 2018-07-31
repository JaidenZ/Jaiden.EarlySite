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
        
        /// <summary>
        /// 筛选附近店铺
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Result<IList<Shop>> IShakeService.ShakeNearShops(ShakeParam param)
        {
            //1.根据搜索距离、当前点经纬度 进行拓展范围
            //  拓展范围方案:
            //  根据当前经纬度为原点,距离为横纵坐标延长,拓展I II III IV 象限
            //  距离单位按米计算,确定每个象限远点坐标
            //2.根据拓展的范围条件搜索符合条件的店铺

            /**
             *                  latitude
             *                  ^
             *                   |
             *                   |
             *                   | 原点O(lng,lat)         点P
             * ------------|--------------------*-------->lngitude
             *                   |                              p(lng,lat)
             *                   |
             *                   |
             *                   
             *                   已知点P与原点O的距离为1米,原点O的经纬已知,求点P的坐标?
             *                   
             * */

            //60 * 0.2034 = 12.204
            //60 * 0.204 = 12.24
            //经纬度100.2034 = 100度 12分 12.24秒 



            throw new System.NotImplementedException();
        }





    }
}
