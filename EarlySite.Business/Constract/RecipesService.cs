namespace EarlySite.Business.Constract
{
    using System;
    using System.Collections.Generic;
    using Business.IService;
    using EarlySite.Model.Common;
    using EarlySite.Model.Enum;
    using EarlySite.Model.Show;


    public class RecipesService : IRecipesService
    {
        public Result CreatRecipes(Recipes recipes)
        {
            throw new NotImplementedException();
        }

        public Result<IList<Recipes>> GetRecipesByPhone(long phone)
        {
            throw new NotImplementedException();
        }

        public Result RemoveRecipesById(int recipesId)
        {
            throw new NotImplementedException();
        }

        public Result RemoveRecipesByPhone(long phone)
        {
            throw new NotImplementedException();
        }

        public Result UpdateRecipes(Recipes recipes)
        {
            throw new NotImplementedException();
        }
    }
}
