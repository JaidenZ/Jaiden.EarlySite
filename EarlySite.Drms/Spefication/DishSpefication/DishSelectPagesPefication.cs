namespace EarlySite.Drms.Spefication
{
    using EarlySite.Model.Common;

    /// <summary>
    /// 单品分页查询规约
    /// </summary>
    public class DishSelectPagesPefication:SpeficationBase
    {
        private PageSearchParam _param = null;

        /// <summary>
        /// 单品分页查询规约
        /// </summary>
        /// <param name="param"></param>
        /// <remarks>
        /// 查询类型
        /// 0:直接分页查询
        /// 1:根据名字模糊查询
        /// 2:根据用餐时间查询
        /// 3:根据食物类型查询
        /// 
        /// </remarks>
        public DishSelectPagesPefication(PageSearchParam param)
        {
            _param = param;
        }

        public override string Satifasy()
        {
            string sql = "";

            if (_param.SearchType == 0)
            {
                sql = string.Format("select DishId,Name,UpdateDate,Type,TypeName,MealTime,ShopId,ShopName,Image,Description from which_dish where Enable = '0' " +
                    "  ORDER BY UpdateDate ASC LIMIT {0},{1}", (_param.PageIndex - 1) * _param.PageNumer, _param.PageNumer);
            }
            else if (_param.SearchType == 1)
            {
                sql = string.Format("select DishId,Name,UpdateDate,Type,TypeName,MealTime,ShopId,ShopName,Image,Description from which_dish where Enable = '0' and " +
                    " Name like '%{0}%' ORDER BY UpdateDate ASC LIMIT {1},{2}", _param.SearchCode, (_param.PageIndex - 1) * _param.PageNumer, _param.PageNumer);
            }
            else if (_param.SearchType == 2)
            {
                sql = string.Format("select DishId,Name,UpdateDate,Type,TypeName,MealTime,ShopId,ShopName,Image,Description from which_dish where Enable = '0' and " +
                    " MealTime = '{0}' ORDER BY UpdateDate ASC LIMIT {1},{2}", _param.SearchCode, (_param.PageIndex - 1) * _param.PageNumer, _param.PageNumer);
            }
            else if (_param.SearchType == 3)
            {
                sql = string.Format("select DishId,Name,UpdateDate,Type,TypeName,MealTime,ShopId,ShopName,Image,Description from which_dish where Enable = '0' and " +
                    " Type = '{0}' ORDER BY UpdateDate ASC LIMIT {1},{2}", _param.SearchCode, (_param.PageIndex - 1) * _param.PageNumer, _param.PageNumer);
            }


            return sql;
        }
    }
}
