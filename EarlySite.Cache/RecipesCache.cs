namespace EarlySite.Cache
{
    using System;
    using Core.Collection;
    using Model.Show;
    using Model.Database;
    using System.Collections.Generic;
    using EarlySite.Drms.DBManager;
    using EarlySite.Drms.Spefication;

    /// <summary>
    /// 食谱信息缓存
    /// </summary>
    public class RecipesCache
    {

        public bool SaveRecipes(RecipesInfo recipes)
        {
            bool result = false;
            string key = string.Format(recipes.GetKeyName());
            result = Session.Current.Set(key,recipes);

            return result;
        }


        public bool SaveRecipes(IList<RecipesInfo> recipes)
        {
            if(recipes == null)
            {
                return false;
            }

            foreach (RecipesInfo item in recipes)
            {
                SaveRecipes(item);
            }

            return true;
        }
    }
}
