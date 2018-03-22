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
}
