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
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:CreatRecipes() .RecipesService"), LogType.ErrorLog);

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
                bool cannext = false;
                cannext = DBConnectionManager.Instance.Writer.Update(new RelationShareDeleteSpefication(recipesId.ToString(), 0).Satifasy());

                if (cannext)
                {
                    cannext = false;
                    //修改食谱信息为禁用
                    cannext = DBConnectionManager.Instance.Writer.Update(new RecipesDeleteSpefication(recipesId.ToString(), 0).Satifasy());
                }


                if (!cannext)
                {
                    DBConnectionManager.Instance.Rollback();
                    result.Status = false;
                    result.Message = "删除食谱失败,请确保请求数据合法";
                }
                else
                {
                    DBConnectionManager.Instance.Writer.Commit();
                }


            }
            catch (Exception ex)
            {

                DBConnectionManager.Instance.Writer.Rollback();
                result.Status = false;
                result.Message = "删除食谱出错:" + ex.Message;
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:RemoveRecipesById() .RecipesService"), LogType.ErrorLog);
            }

            return result;
        }

        public Result RemoveRecipesByPhone(long phone)
        {
            Result result = new Result()
            {
                Status = true,
                Message = "删除食谱成功"
            };

            try
            {
                if (phone == 0)
                {
                    throw new ArgumentNullException("删除食谱,参数非法");
                }
                //删除食谱绑定关系
                bool cannext = false;
                cannext = DBConnectionManager.Instance.Writer.Update(new RelationShareDeleteSpefication(phone.ToString(), 2).Satifasy());

                if (cannext)
                {
                    cannext = false;
                    //修改食谱信息为禁用
                    cannext = DBConnectionManager.Instance.Writer.Update(new RecipesDeleteSpefication(phone.ToString(), 1).Satifasy());
                }


                if (!cannext)
                {
                    DBConnectionManager.Instance.Rollback();
                    result.Status = false;
                    result.Message = "删除食谱失败,请确保请求数据合法";
                }
                else
                {
                    DBConnectionManager.Instance.Writer.Commit();
                }


            }
            catch (Exception ex)
            {

                DBConnectionManager.Instance.Writer.Rollback();
                result.Status = false;
                result.Message = "删除食谱出错:" + ex.Message;
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:RemoveRecipesByPhone() .RecipesService"), LogType.ErrorLog);
            }

            return result;
        }

        /// <summary>
        /// 更新食谱信息
        /// </summary>
        /// <param name="recipes"></param>
        /// <returns></returns>
        public Result UpdateRecipes(Recipes recipes)
        {
            Result result = new Result()
            {
                Status = true,
                Message = "删除食谱成功"
            };

            try
            {
                RecipesInfo info = recipes.Copy<RecipesInfo>();
                if (info == null)
                {
                    throw new ArgumentNullException("更新食谱,参数非法");
                }
                info.UpdateDate = DateTime.Now;

                result.Status = DBConnectionManager.Instance.Writer.Update(new RecipesUpdateSpefication(info).Satifasy());
                
                if (!result.Status)
                {
                    DBConnectionManager.Instance.Rollback();
                    result.Status = false;
                    result.Message = "更新食谱失败,请确保请求数据合法";
                }
                else
                {
                    DBConnectionManager.Instance.Writer.Commit();
                }
            }
            catch (Exception ex)
            {

                DBConnectionManager.Instance.Writer.Rollback();
                result.Status = false;
                result.Message = "删除食谱出错:" + ex.Message;
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:UpdateRecipes() .RecipesService"), LogType.ErrorLog);

            }

            return result;
        }
    }
}
