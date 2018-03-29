namespace EarlySite.Drms.Spefication
{
    using System.Text;
    using EarlySite.Model.Database;

    public class RecipesAddSpefication : SpeficationBase
    {
        private RecipesInfo _info;
        public RecipesAddSpefication(RecipesInfo recipes)
        {
            _info = recipes;
        }

        public override string Satifasy()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" insert (RecipesId,Name,UpdateDate,Phone,Cover,Description,Tag,IsPrivate) into which_recipes values ");
            sql.AppendFormat("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", _info.RecipesId, _info.Name, _info.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"), _info.Phone, _info.Cover,
                _info.Description, _info.Tag, _info.IsPrivate);

            return sql.ToString();
        }
    }
}
