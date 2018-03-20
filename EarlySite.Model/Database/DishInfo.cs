namespace EarlySite.Model.Database
{
    using EarlySite.Model.Enum;
    using System;

    public class DishInfo
    {
        /// <summary>
        /// 食物编号
        /// </summary>
        public int DIshId { get; set; }

        /// <summary>
        /// 食物名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }

        /// <summary>
        /// 商店编号
        /// </summary>
        public int ShopId { get; set; }

        /// <summary>
        /// 商店名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 配图
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 食物描述
        /// </summary>
        public string Description { get; set; }

    }
}
