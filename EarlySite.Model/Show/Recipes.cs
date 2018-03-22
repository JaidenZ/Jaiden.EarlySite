namespace EarlySite.Model.Show
{
    using System;

    /// <summary>
    /// 食谱展示模型
    /// </summary>
    public class Recipes
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
    }
}
