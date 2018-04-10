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
            string sql = string.Format(" update which_shop set Enable = '1' where ShopId = '{0}'", _shopId);
            return sql;
        }
    }
}
