namespace EarlySite.Drms.Spefication
{
    using System;
    using System.Collections.Generic;
    

    public class ShopSelectSpefication : SpeficationBase
    {
        private string _searchCode = "";
        private int _type = 0;

        /// <summary>
        /// 店铺查询规约
        /// </summary>
        /// <param name="searchCode">查询条件</param>
        /// <param name="type">
        /// 条件类型
        /// 0:根据店铺编号精确查询
        /// 1:根据店铺名称模糊查询
        /// </param>
        protected ShopSelectSpefication(string searchCode,int type)
        {
            _searchCode = searchCode;
            _type = type;

        }

        public override string Satifasy()
        {
            string sql = "";
            if(_type == 0)
            {
                sql = string.Format(" select ShopId,ShopName,Longitude,Latitude,UpdateDate,Description from which_shop where ShopId = '{0}'", _searchCode);
            }
            else if(_type == 1)
            {
                sql = string.Format(" select ShopId,ShopName,Longitude,Latitude,UpdateDate,Description from which_shop where ShopName like '%{0}%'", _searchCode);
            }
            return sql;
        }
    }
}
