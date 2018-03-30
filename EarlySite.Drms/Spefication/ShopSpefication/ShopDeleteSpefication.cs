namespace EarlySite.Drms.Spefication
{
    public class ShopDeleteSpefication : SpeficationBase
    {
        private int _shopId = 0;

        public ShopDeleteSpefication(int shopId)
        {
            _shopId = shopId;
        }

        public override string Satifasy()
        {
            string sql = string.Format(" delete from which_shop where ShopId = '{0}'", _shopId);
            return sql;
        }
    }
}
