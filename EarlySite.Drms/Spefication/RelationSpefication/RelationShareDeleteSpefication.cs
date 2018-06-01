namespace EarlySite.Drms.Spefication
{
    using System.Text;
    using System.Collections.Generic;
    using Model.Database;

    public class RelationShareDeleteSpefication : SpeficationBase
    {
        private string _deleteInfo;
        private long _phone = 0;
        private int _type = 0;

        /// <summary>
        /// 分享关系解除 规约
        /// </summary>
        /// <param name="deleteInfo">解除信息</param>
        /// <param name="type">
        /// 解除类型
        /// 0:根据食谱编号 手机号解除
        /// 1:根据食物编号 手机号解除
        /// 2:根据分享者手机号解除
        /// </param>
        public RelationShareDeleteSpefication(string deleteInfo,long phone,int type)
        {
            _deleteInfo = deleteInfo;
            _type = type;
            _phone = phone;
        }

        public override string Satifasy()
        {
            string sql = "";

            if(_type == 0)
            {
                sql = string.Format(" delete from relation_share where RecipesId = '{0}' and phone = '{1}'", _deleteInfo, _phone);
            }
            else if(_type == 1)
            {
                sql = string.Format(" delete from relation_share where DishId = '{0}' and phone = '{1}'", _deleteInfo,_phone);
            }
            else if(_type == 2)
            {
                sql = string.Format(" delete from relation_share where Phone = '{0}'", _deleteInfo);
            }

            return sql;
        }
    }
}
