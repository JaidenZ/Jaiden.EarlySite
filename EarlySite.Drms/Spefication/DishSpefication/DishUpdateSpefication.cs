namespace EarlySite.Drms.Spefication
{
    using System.Text;
    using EarlySite.Model.Database;

    public class DishUpdateSpefication : SpeficationBase
    {
        private DishInfo _info;

        public DishUpdateSpefication(DishInfo info)
        {
            _info = info;
        }

        public override string Satifasy()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("update which_dish set Name = '{0}',UpdateDate = '{1}',Type = '{2}',TypeName = '{3}',MealTime = '{4}',ShopId = '{5}',ShopName = '{6}',Price = '{10}', Image = '{7}',Description = '{8}' where DishId = '{9}'",
                _info.Name, _info.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"), _info.Type.GetHashCode(), _info.TypeName, _info.MealTime.GetHashCode(), _info.ShopId, _info.ShopName, _info.Image, _info.Description, _info.DIshId, _info.Price);

            return sql.ToString();
        }
    }
}
