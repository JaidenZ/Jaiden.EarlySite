namespace EarlySite.Drms.Spefication
{
    using System.Text;
    using EarlySite.Model.Database;

    /// <summary>
    /// 增加单品规约
    /// </summary>
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
            sql.AppendFormat(" insert into which_dish (DishId,Name,UpdateDate,Type,TypeName,MealTime,ShopId,ShopName,Price,Image,Description) ");
            sql.Append(" values ");
            sql.AppendFormat(" ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", _dishInfo.DIshId, _dishInfo.Name, _dishInfo.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"), _dishInfo.Type.GetHashCode(), _dishInfo.TypeName, _dishInfo.MealTime.GetHashCode(), _dishInfo.ShopId, _dishInfo.ShopName,_dishInfo.Price ,_dishInfo.Image, _dishInfo.Description);
            
            return sql.ToString();
        }
    }
}
