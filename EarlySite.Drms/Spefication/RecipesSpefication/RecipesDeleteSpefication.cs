namespace EarlySite.Drms.Spefication.RecipesSpefication
{
    public class RecipesDeleteSpefication : SpeficationBase
    {
        private string _deleteInfo;
        private int _type;


        /// <summary>
        /// 食谱删除规约
        /// </summary>
        /// <param name="info">删除信息</param>
        /// <param name="type">
        /// 需要删除的类型
        /// 0:根据食谱编号删除
        /// 1:根据创建人电话删除
        /// </param>
        public RecipesDeleteSpefication(string info,int type)
        {
            _deleteInfo = info;
            _type = type;
        }

        public override string Satifasy()
        {
            string sql = "";
            if(_type == 0)
            {
                sql = string.Format(" delete from which_recipes where RecipesId = '{0}'", _deleteInfo);
            }
            else if(_type == 1)
            {
                sql = string.Format(" delete from which_recipes where Phone = '{0}'", _deleteInfo);
            }
            return sql.ToString();
        }
    }
}
