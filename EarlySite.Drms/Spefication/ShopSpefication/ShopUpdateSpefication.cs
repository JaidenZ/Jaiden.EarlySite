namespace EarlySite.Drms.Spefication
{
    using Model.Database;
    public class ShopUpdateSpefication : SpeficationBase
    {
        private ShopInfo _info;

        public ShopUpdateSpefication(ShopInfo update)
        {
            _info = update;
        }

        public override string Satifasy()
        {
            string sql = string.Format(" update which_shop set ShopName = '{1}',Longitude = '{2}',Latitude = '{3}',UpdateDate = '{4}',Description = '{5}' where ShopId = '{0}'",
                _info.ShopId, _info.ShopName, _info.Longitude, _info.Latitude, _info.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"), _info.Description);

            return sql;
        }
    }
}
