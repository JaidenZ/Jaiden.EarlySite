namespace EarlySite.Model.Database
{
    using System;

    /// <summary>
    /// 关系分享实体
    /// </summary>
    public class RelationShareInfo : IKeyNameSpecification
    {
        /// <summary>
        /// 食谱编号
        /// </summary>
        public int RecipesId { get; set; }
        /// <summary>
        /// 食物编号
        /// </summary>
        public int DishId { get; set; }
        /// <summary>
        /// 分享者手机号
        /// </summary>
        public long Phone { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }

        public string GetKeyName()
        {
            //DB_RS_食谱编号_食物编号_手机号
            return string.Format("DB_RS_{0}_{1}_{2}", this.RecipesId, this.DishId, this.Phone);
        }
    }
}
