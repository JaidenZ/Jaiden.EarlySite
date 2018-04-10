namespace EarlySite.Model.Show
{
    


    /// <summary>
    /// 分享单品模型
    /// </summary>
    public class DishShare
    {
        /// <summary>
        /// 单品信息
        /// </summary>
        public Dish DishInfo { get; set; }

        /// <summary>
        /// 门店信息
        /// </summary>
        public Shop ShopInfo { get; set; }

        /// <summary>
        /// 分享食谱信息
        /// </summary>
        public Recipes RecipesInfo { get; set; }
    }


    /// <summary>
    /// 收藏单品模型
    /// </summary>
    public class DishCollect
    {
        /// <summary>
        /// 收藏的食物编号
        /// </summary>
        public int DIshId { get; set; }


        /// <summary>
        /// 收藏的食谱编号
        /// </summary>
        public int RecipesId { get; set; }

        /// <summary>
        /// 收藏者的手机号
        /// </summary>
        public long Phone { get; set; }

    }

}
