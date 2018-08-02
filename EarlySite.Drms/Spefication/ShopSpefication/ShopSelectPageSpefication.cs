namespace EarlySite.Drms.Spefication
{
    using EarlySite.Model.Common;
    using EarlySite.Model.Database;

    public class ShopSelectPageSpefication : SpeficationBase
    {
        PageSearchParam _param = null;


        /// <summary>
        /// 店铺分页查询规约
        /// </summary>
        /// <param name="param">查询条件</param>
        /// <param name="type">
        /// 条件类型
        /// 0:直接分页查询
        /// 1:根据店铺名称模糊查询
        /// </param>
        public ShopSelectPageSpefication(PageSearchParam param)
        {
            _param = param;
        }

        public override string Satifasy()
        {
            string sql = "";

            if (_param.SearchType == 0)
            {
                sql = string.Format(" select ShopId,ShopName,Longitude,Latitude,ShopAddress,UpdateDate,Description from which_shop where Enable = '0' ORDER BY UpdateDate ASC LIMIT {0},{1}", (_param.PageIndex - 1) * _param.PageNumer, _param.PageNumer);
            }
            else if (_param.SearchType == 1)
            {
                sql = string.Format(" select ShopId,ShopName,Longitude,Latitude,ShopAddress,UpdateDate,Description from which_shop where Enable = '0' and ShopName like '%{0}%' ORDER BY UpdateDate ASC LIMIT {1},{2}", _param.SearchCode, (_param.PageIndex - 1) * _param.PageNumer, _param.PageNumer);
            }

            return sql;
        }
    }
}
