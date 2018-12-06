namespace EarlySite.Drms.Spefication.FavoriteSpefication
{
    using System.Collections.Generic;
    using System.Text;
    using EarlySite.Model.Database;
    using EarlySite.Model.Enum;

    /// <summary>
    /// 收藏新增规约
    /// </summary>
    public class FavoriteAddSpefication : SpeficationBase
    {

        private IList<FavoriteInfo> _info;

        /// <summary>
        /// 收藏新增规约
        /// </summary>
        public FavoriteAddSpefication(IList<FavoriteInfo> info)
        {
            _info = info;
        }

        public override string Satifasy()
        {

            StringBuilder sql = new StringBuilder();

            if (_info != null && _info.Count > 0)
            {
                sql.Append("insert into relation_favorite (Phone,FavoriteId,FavoriteType,UpdateDate) values");

                for (int i = 0; i < _info.Count; i++)
                {
                    sql.AppendFormat("('{0}','{1}','{2}','{3}')", _info[i].Phone, _info[i].FavoriteId, _info[i].FavoriteType, _info[i].UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (i != _info.Count - 1)
                    {
                        sql.Append(" , ");
                    }
                }
            }
            return sql.ToString();
        }
    }
}
