namespace EarlySite.Drms.Spefication
{
    using EarlySite.Model.Common;

    public class RecipesSelectPageSpefication : SpeficationBase
    {
        private PageSearchParam _param = null;
        public RecipesSelectPageSpefication(PageSearchParam param)
        {
            _param = param;
        }

        public override string Satifasy()
        {
            string sql = "";

            if (_param.SearchType == 0)
            {
                sql = string.Format(" select RecipesId,Name,UpdateDate,Phone,Cover,Description,Tag,IsPrivate from which_recipes where IsPrivate = '0' ORDER BY UpdateDate ASC LIMIT {0},{1}", (_param.PageIndex - 1) * _param.PageNumer, _param.PageNumer);
            }
            else if (_param.SearchType == 1)
            {
                sql = string.Format(" select RecipesId,Name,UpdateDate,Phone,Cover,Description,Tag,IsPrivate from which_recipes where IsPrivate = '0' and Name like '%{0}%' ORDER BY UpdateDate ASC LIMIT {1},{2}", _param.SearchCode, (_param.PageIndex - 1) * _param.PageNumer, _param.PageNumer);
            }

            return sql;
        }
    }
}
