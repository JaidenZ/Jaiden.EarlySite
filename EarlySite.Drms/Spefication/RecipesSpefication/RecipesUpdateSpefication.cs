namespace EarlySite.Drms.Spefication
{
    using System.Text;
    using EarlySite.Model.Database;

    public class RecipesUpdateSpefication : SpeficationBase
    {
        private RecipesInfo _info;
        public RecipesUpdateSpefication(RecipesInfo recipes)
        {
            _info = recipes;
        }

        public override string Satifasy()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("update which_recipes set Name = '{1}',UpdateDate = '{2}',Phone = '{3}',Covert = '{4}',Description = '{5}',Tag = '{6}',IsPrivate = '{7}' where RecipesId = '{0}'", _info.RecipesId,
                _info.Name, _info.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"), _info.Phone, _info.Cover, _info.Description, _info.Tag, _info.IsPrivate);

            return sql.ToString();
        }
    }
}
