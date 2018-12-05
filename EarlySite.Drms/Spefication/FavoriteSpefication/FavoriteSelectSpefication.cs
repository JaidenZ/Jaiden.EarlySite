namespace EarlySite.Drms.Spefication
{
    using EarlySite.Model.Enum;

    /// <summary>
    /// 收藏选择规约
    /// </summary>
    /// <param name="phone">收藏手机号码</param>
    /// <param name="type">收藏类型</param>
    public class FavoriteSelectSpefication : SpeficationBase
    {
        private long _phone;
        private FavoriteTypeEnum _type;

        /// <summary>
        /// 收藏选择规约
        /// </summary>
        /// <param name="phone">收藏手机号码</param>
        /// <param name="type">收藏类型</param>
        public FavoriteSelectSpefication(long phone,FavoriteTypeEnum type)
        {
            _phone = phone;
            _type = type;
        }

        public override string Satifasy()
        {
            string sql = " select Phone,FavoriteId,FavoriteType,UpdateDate from relation_favorite where 1=1 ";

            sql += string.Format(" and Phone = {0}", _phone);

            if(_type != FavoriteTypeEnum.所有类型)
            {
                sql += string.Format(" and FavoriteType = {0}", _type.GetHashCode());
            }
            return sql;
        }
    }
}
