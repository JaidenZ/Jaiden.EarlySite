namespace EarlySite.Drms.Spefication.ShakeSpefication
{
    using Core.Utils;
    using EarlySite.Model.Show;

    /// <summary>
    /// 筛选附近的店铺信息
    /// </summary>
    public class ShakeNearShopSpefication : SpeficationBase
    {
        Position _southwest = null;
        Position _northeast = null;

        
        /// <summary>
        /// 筛选附近店铺初始化
        /// </summary>
        /// <param name="left">查询范围西南坐标</param>
        /// <param name="right">查询范围东北坐标</param>
        /// <param name="param">筛选参数</param>
        public ShakeNearShopSpefication(Position left,Position right)
        {
            _southwest = left;
            _northeast = right;

        }

        public override string Satifasy()
        {
            string sql = string.Format("select ShopId,ShopName,Longitude,Latitude,ShopAddress,UpdateDate,Description from which_shop where Latitude >= {0} and Latitude <= {1} and Longitude >= {2} and Longitude <= {3} ", _southwest.Latitude, _northeast.Latitude, _southwest.Longitude, _northeast.Longitude);

            return sql.ToString();
        }
    }
}
