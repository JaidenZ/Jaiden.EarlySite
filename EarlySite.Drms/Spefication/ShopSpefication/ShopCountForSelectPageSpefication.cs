namespace EarlySite.Drms.Spefication
{
    using EarlySite.Model.Common;

    public class ShopCountForSelectPageSpefication : SpeficationBase
    {
        PageSearchParam _param = null;

        /// <summary>
        /// 店铺分页查询规约
        /// </summary>
        /// <param name="param">查询条件</param>
        public ShopCountForSelectPageSpefication(PageSearchParam param)
        {
            _param = param;
        }


        public override string Satifasy()
        {
            string sql = "";

            if (_param.SearchType == 0)
            {
                sql = string.Format(" select count(1) from which_shop where Enable = '0' ORDER BY UpdateDate ASC");
            }
            else if (_param.SearchType == 1)
            {
                sql = string.Format(" select count(1) from which_shop where Enable = '0' and ShopName like '%{0}%' ORDER BY UpdateDate ASC", _param.SearchCode);
            }

            return sql;
        }
    }
}
