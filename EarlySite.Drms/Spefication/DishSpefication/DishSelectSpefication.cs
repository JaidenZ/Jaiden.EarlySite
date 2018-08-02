namespace EarlySite.Drms.Spefication
{
    using System;
    using System.Text;


    public class DishSelectSpefication : SpeficationBase
    {
        private string _searchCode;
        private int _type = 0;

        /// <summary>
        /// 单品食物查询规约
        /// </summary>
        /// <param name="searchText">查询内容</param>
        /// <param name="searchType">
        /// 查询方式
        /// 0:根据单品食物编号查询
        /// 1:根据名字模糊查询
        /// 2:根据用餐时间查询
        /// 3:根据食物类型查询
        /// 4:根据门店编号查询
        /// </param>
        public DishSelectSpefication(string searchCode,int searchType)
        {
            _searchCode = searchCode;
            _type = searchType;
        }

        public override string Satifasy()
        {
            string sql = "";

            if(_type == 0)
            {
                sql = string.Format("select DishId,Name,UpdateDate,Type,TypeName,MealTime,ShopId,ShopName,Price,Image,Description from which_dish where " +
                    " DishId = '{0}'", _searchCode);
            }
            else if(_type == 1)
            {
                sql = string.Format("select DishId,Name,UpdateDate,Type,TypeName,MealTime,ShopId,ShopName,Price,Image,Description from which_dish where Enable = '0' and " +
                    " Name like '%{0}%'", _searchCode);
            }
            else if(_type == 2)
            {
                sql = string.Format("select DishId,Name,UpdateDate,Type,TypeName,MealTime,ShopId,ShopName,Price,Image,Description from which_dish where Enable = '0' and " +
                    " MealTime = '{0}'", _searchCode);
            }
            else if (_type == 3)
            {
                sql = string.Format("select DishId,Name,UpdateDate,Type,TypeName,MealTime,ShopId,ShopName,Price,Image,Description from which_dish where Enable = '0' and " +
                    " Type = '{0}'", _searchCode);
            }
            else if (_type == 4)
            {
                sql = string.Format("select DishId,Name,UpdateDate,Type,TypeName,MealTime,ShopId,ShopName,Price,Image,Description from which_dish where Enable = '0' and " +
                    " ShopId = '{0}'", _searchCode);
            }
            return sql;
        }
    }
}
