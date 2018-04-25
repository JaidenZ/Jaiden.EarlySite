namespace EarlySite.Drms.Spefication
{
    using EarlySite.Model.Common;
    using EarlySite.Model.Enum;
    /// <summary>
    /// 单品总数量分页查询规约
    /// </summary>
    public class DishCountForSelectPageSpefication : SpeficationBase
    {
        private PageSearchParam _param = null;


        public DishCountForSelectPageSpefication(PageSearchParam param)
        {
            _param = param;
        }

        public override string Satifasy()
        {
            string sql = "";

            if (_param.SearchType == 0)
            {
                sql = string.Format("select count(1) where Enable = '0'");
            }
            else if (_param.SearchType == 1)
            {
                sql = string.Format("select count(1) from which_dish where Enable = '0' and " +
                    " Name like '%{0}%' ORDER BY UpdateDate ASC ", _param.SearchCode);
            }
            else if (_param.SearchType == 2)
            {
                if(_param.SearchCode == MealTime.所有时间段.GetHashCode().ToString())
                {
                    sql = string.Format("select count(1) from which_dish where Enable = '0' " +
                        "  ORDER BY UpdateDate ASC ");
                }
                else
                {
                    sql = string.Format("select count(1) from which_dish where Enable = '0' and MealTime = '0' or " +
                        " MealTime = '{0}'  ORDER BY UpdateDate ASC ", _param.SearchCode);
                }

            }
            else if (_param.SearchType == 3)
            {
                sql = string.Format("select count(1) from which_dish where Enable = '0' and " +
                    " Type = '{0}' ORDER BY UpdateDate ASC ", _param.SearchCode);
            }


            return sql;
        }
    }
}
