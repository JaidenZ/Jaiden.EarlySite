namespace EarlySite.Drms.Spefication
{
    public class DishShopNameUpdateSpefication : SpeficationBase
    {
        private int _shopId = 0;
        private string _shopName = "";

        /// <summary>
        /// 更新食品关联门店名称
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="shopName"></param>
        public DishShopNameUpdateSpefication(int shopId,string shopName)
        {
            _shopId = shopId;
            _shopName = shopName;

        }
        public override string Satifasy()
        {
            string sql = string.Format(" update wich_dish set ShopName = '{0}' where ShopId = '{1}' ",_shopName,_shopId);

            return sql;

        }
    }
}
