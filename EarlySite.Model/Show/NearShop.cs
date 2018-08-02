using System.Collections.Generic;

namespace EarlySite.Model.Show
{
    /// <summary>
    /// 附近商家模型
    /// </summary>
    public class NearShop
    {
        /// <summary>
        /// 门店信息
        /// </summary>
        public Shop ShopInfo { get; set; }


        /// <summary>
        /// 最新单品信息
        /// </summary>
        public IList<Dish> NewDishs { get; set; }

    }
}
