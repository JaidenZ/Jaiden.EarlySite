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

                if(addinfo == null)
                {
                    throw new ArgumentNullException("新增食谱信息,参数不能为空");
                }

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
            Result result = new Result()
            {
                Status = true,
                Message = "删除食谱成功"
            };

            try
            {
                if(recipesId == 0)
                {
                    throw new ArgumentNullException("删除食谱,参数非法");
                }
                //删除食谱绑定关系


            }
            catch (Exception ex)
            {

                DBConnectionManager.Instance.Writer.Rollback();
                result.Status = false;
                result.Message = "删除食谱出错:" + ex.Message;

            }

            return result;
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
