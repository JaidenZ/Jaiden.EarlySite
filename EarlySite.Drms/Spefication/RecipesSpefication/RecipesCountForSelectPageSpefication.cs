namespace EarlySite.Drms.Spefication
{
    using EarlySite.Model.Common;
    using EarlySite.Model.Database;
    using EarlySite.Model.Show;

    /// <summary>
    /// 食谱数量分页查询规约
    /// </summary>
    public class RecipesCountForSelectPageSpefication : SpeficationBase
    {
        private PageSearchParam _param = null;

        public RecipesCountForSelectPageSpefication(PageSearchParam param)
        {
            _param = param;
        }

        public override string Satifasy()
        {
            string sql = "";

            if (_param.SearchType == 0)
            {
                sql = string.Format(" select count(1) from which_recipes where IsPrivate = '0' ORDER BY UpdateDate ASC");
            }
            else if (_param.SearchType == 1)
            {
                sql = string.Format(" select count(1) from which_recipes where IsPrivate = '0' and Name like '%{0}%' ORDER BY UpdateDate ASC", _param.SearchCode);
            }

            return sql;
        }
    }
}
