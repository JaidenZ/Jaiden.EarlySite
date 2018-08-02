namespace EarlySite.Model.Show
{
    using System;

    /// <summary>
    /// 门店展示模型
    /// </summary>
    public class Shop
    {
        /// <summary>
        /// 门店编号
        /// </summary>
        public int ShopId { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 门店详细地址
        /// </summary>
        public string ShopAddress { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

    }
}
