namespace EarlySite.Model.Database
{
    using System;

    /// <summary>
    /// 食谱信息
    /// </summary>
    public class RecipesInfo : IKeyNameSpecification
    {
        /// <summary>
        /// 食谱编号
        /// </summary>
        public int RecipesId { get; set; }

        /// <summary>
        /// 食谱名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 创建人手机号
        /// </summary>
        public long Phone { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 是否私有
        /// </summary>
        public bool IsPrivate { get; set; }

        public string GetKeyName()
        {
            //DB_RI_食谱编号_手机号
            return string.Format("DB_RI_{0}_{1}",this.RecipesId,this.Phone);
        }
    }
}
