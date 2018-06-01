namespace EarlySite.Model.Database
{
    using System;

    /// <summary>
    /// 门店信息
    /// </summary>
    public class ShopInfo : IKeyNameSpecification
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
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }

        public string GetKeyName()
        {
            return string.Format("DB_SI_{0}_{1}",this.ShopId,this.ShopName);
        }
    }
}
