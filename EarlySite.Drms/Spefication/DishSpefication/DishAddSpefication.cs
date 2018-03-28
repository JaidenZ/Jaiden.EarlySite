namespace EarlySite.Drms.Spefication
{
    using System.Text;
    using EarlySite.Model.Database;

    public class DishAddSpefication : SpeficationBase
    {
        private DishInfo _dishInfo;

        public DishAddSpefication(DishInfo info)
        {
            _dishInfo = info;
        }

        public override string Satifasy()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" insert into which_dish DishId,Name,UpdateDate,Type,TypeName,MealTime,ShopId,ShopName,Image,Description ");
            sql.Append(" values ");
            sql.AppendFormat(" ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", _dishInfo.DIshId, _dishInfo.Name, _dishInfo.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"), _dishInfo.Type.GetHashCode(), _dishInfo.TypeName, _dishInfo.MealTime, _dishInfo.ShopId, _dishInfo.ShopName, _dishInfo.Image, _dishInfo.Description);
            
            return sql.ToString();
        }
    }
}
