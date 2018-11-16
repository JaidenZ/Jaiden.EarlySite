namespace EarlySite.Model.Database
{
    using EarlySite.Model.Enum;
    using System;

    public class FavoriteInfo : IKeyNameSpecification
    {

        /// <summary>
        /// 收藏者手机号
        /// </summary>
        public long Phone { get; set; }

        /// <summary>
        /// 收藏编号
        /// </summary>
        public int FavoriteId { get; set; }

        /// <summary>
        /// 收藏类型
        /// </summary>
        public int FvoriteType { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }

        public string GetKeyName()
        {
            //DB_FA_手机号_收藏编号_类型
            return string.Format("DB_FA_{0}_{1}_{2}", this.Phone, this.FavoriteId, this.FvoriteType);
        }
    }
}
