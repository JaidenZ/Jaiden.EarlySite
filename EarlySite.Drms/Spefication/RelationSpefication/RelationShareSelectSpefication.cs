namespace EarlySite.Drms.Spefication.RelationSpefication
{
    using System.Text;
    using System.Collections.Generic;
    using Model.Database;

    /// <summary>
    /// 分享关系搜索规约
    /// </summary>
    public class RelationShareSelectSpefication : SpeficationBase
    {
        private string _searchtext = "";
        private int _type = 0;

        /// <summary>
        /// 分享关系搜索规约
        /// </summary>
        /// <param name="search">搜索字段</param>
        /// <param name="type">
        /// 搜索类型
        /// 0:根据手机号搜索
        /// 1:根据食谱编号搜索
        /// 2:根据单品编号搜索
        /// </param>
        public RelationShareSelectSpefication(string search,int type)
        {
            _searchtext = search;
            _type = type;
        }

        public override string Satifasy()
        {
            string sql = "";

            if (_type == 0)
            {
                sql = string.Format(" select RecipesId,DishId,Phone,UpdateDate from relation_share where Phone = '{0}' ", _searchtext);
            }
            else if (_type == 1)
            {
                sql = string.Format(" select RecipesId,DishId,Phone,UpdateDate from relation_share where RecipesId = '{0}' ", _searchtext);
            }
            else if (_type == 2)
            {
                sql = string.Format(" select RecipesId,DishId,Phone,UpdateDate from relation_share where DishId = '{0}' ", _searchtext);
            }

            return sql;
        }
    }
}
