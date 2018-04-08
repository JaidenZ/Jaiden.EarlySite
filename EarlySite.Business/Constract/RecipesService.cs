namespace EarlySite.Business.Constract
{
    using System;
    using System.Collections.Generic;
    using Business.IService;
    using EarlySite.Core.Utils;
    using EarlySite.Drms.DBManager;
    using EarlySite.Drms.Spefication;
    using EarlySite.Model.Common;
    using EarlySite.Model.Database;
    using EarlySite.Model.Enum;
    using EarlySite.Model.Show;


    public class RecipesService : IRecipesService
    {
        public Result CreatRecipes(Recipes recipes)
        {
            Result result = new Result()
            {
                Status = true,
                Message = "创建食谱成功"
            };

            try
            {
                RecipesInfo addinfo = recipes.Copy<RecipesInfo>();
                result.Status = DBConnectionManager.Instance.Writer.Insert(new RecipesAddSpefication(addinfo).Satifasy());

                if (result.Status)
                {
                    DBConnectionManager.Instance.Writer.Commit();
                }
                else
                {
                    DBConnectionManager.Instance.Writer.Rollback();
                }

            }
            catch(Exception ex)
            {

                DBConnectionManager.Instance.Writer.Rollback();
                result.Status = false;
                result.Message = "创建食谱出错:" + ex.Message;

            }

            return result;
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
