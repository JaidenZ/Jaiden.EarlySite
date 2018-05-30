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
    using EarlySite.Model.Show;
    using EarlySite.Cache.CacheBase;
    using EarlySite.Core.DDD.Service;

    public class RecipesService : IRecipesService
    {
        /// <summary>
        /// 创建一个食谱
        /// </summary>
        /// <param name="recipes"></param>
        /// <returns></returns>
        public Result CreatRecipes(Recipes recipes)
        {
            Result result = new Result()
            {
                Status = true,
                Message = "创建食谱成功"
            };
            
            try
            {
                //食谱缓存服务
                IRecipesCache recipesservice = ServiceObjectContainer.Get<IRecipesCache>();
                
                RecipesInfo addinfo = recipes.Copy<RecipesInfo>();
                if (addinfo == null)
                {
                    throw new ArgumentNullException("新增食谱信息,参数不能为空");
                }

                result.Status = DBConnectionManager.Instance.Writer.Insert(new RecipesAddSpefication(addinfo).Satifasy());

                if (result.Status)
                {
                    DBConnectionManager.Instance.Writer.Commit();
                    //更新缓存
                    recipesservice.SaveInfo(addinfo);
                }
                else
                {
                    DBConnectionManager.Instance.Writer.Rollback();
                }

            }
            catch (Exception ex)
            {

                DBConnectionManager.Instance.Writer.Rollback();
                result.Status = false;
                result.Message = "创建食谱出错:" + ex.Message;
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:CreatRecipes() .RecipesService"), LogType.ErrorLog);

            }

            return result;
        }

        /// <summary>
        /// 根据手机号获取喜爱的食谱集
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Result<IList<Recipes>> GetFavoriteRecipesByPhone(long phone)
        {
            Result<IList<Recipes>> result = new Result<IList<Recipes>>()
            {
                Status = true,
                Message = "查找食谱成功"
            };
            try
            {
                //食谱缓存服务
                IRecipesCache recipesservice = ServiceObjectContainer.Get<IRecipesCache>();
                if (phone == 0)
                {
                    throw new ArgumentException("获取食谱,参数非法");
                }

                IList<RecipesInfo> favoriteRecipes = recipesservice.GetFavoriteRecipesByPhone(phone);

                if(favoriteRecipes != null && favoriteRecipes.Count > 0)
                {
                    result.Data = favoriteRecipes.CopyList<RecipesInfo, Recipes>();
                    result.Status = true;
                }

            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = "查找食谱出错:" + ex.Message;
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:GetFavoriteRecipesByPhone() .RecipesService"), LogType.ErrorLog);
            }

            return result;

        }


        /// <summary>
        /// 根据手机号获取用户的食谱集
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Result<IList<Recipes>> GetRecipesByPhone(long phone)
        {
            Result<IList<Recipes>> result = new Result<IList<Recipes>>()
            {
                Status = true,
                Message = "查找食谱成功"
            };
            try
            {
                //食谱缓存服务
                IRecipesCache recipesservice = ServiceObjectContainer.Get<IRecipesCache>();
                if (phone == 0)
                {
                    throw new ArgumentException("获取食谱,参数非法");
                }

                IList<RecipesInfo> recipeslist = recipesservice.GetRecipesInfoByPhone(phone);
                if (recipeslist != null && recipeslist.Count > 0)
                {
                    result.Data = recipeslist.CopyList<RecipesInfo,Recipes>();
                }
                else
                {
                    result.Status = false;
                    result.Message = "获取食谱失败,未找到对应食谱";
                }

            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "查找食谱出错:" + ex.Message;
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:GetRecipesByPhone() .RecipesService"), LogType.ErrorLog);
            }


            return result;
        }
        /// <summary>
        /// 根据食谱编号获取单个食谱信息
        /// </summary>
        /// <param name="recipesId"></param>
        /// <returns></returns>
        public Result<Recipes> GetRecipesById(int recipesId)
        {
            Result<Recipes> result = new Result<Recipes>()
            {
                Status = true,
                Message = "查找食谱成功"
            };
            try
            {

                if (recipesId == 0)
                {
                    throw new ArgumentException("获取食谱,参数非法");
                }
                //食谱缓存服务
                IRecipesCache recipesservice = ServiceObjectContainer.Get<IRecipesCache>();

                RecipesInfo recipe = recipesservice.GetRecipesInfoById(recipesId);

                if(recipe != null)
                {
                    result.Data = recipe.Copy<Recipes>();
                }
                else
                {
                    result.Status = false;
                    result.Message = "获取食谱失败,未找到对应食谱";
                }
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = "查找食谱出错:" + ex.Message;
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:GetRecipesById() .RecipesService"), LogType.ErrorLog);
            }


            return result;
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
                if (recipesId == 0)
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
                    DBConnectionManager.Instance.Writer.Rollback();
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
                    DBConnectionManager.Instance.Writer.Rollback();
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
                    DBConnectionManager.Instance.Writer.Rollback();
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

        /// <summary>
        /// 获取分页食谱信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Result<PageList<Recipes>> GetPageRecipes(PageSearchParam param)
        {
            Result<PageList<Recipes>> pagelistresult = new Result<PageList<Recipes>>()
            {
                Status = true
            };
            pagelistresult.Data = new PageList<Recipes>();
            pagelistresult.Data.PageIndex = param.PageIndex;
            pagelistresult.Data.PageNumer = param.PageNumer;
            param.SearchCode = "";
            param.SearchType = 0;

            try
            {
                //获取总数
                pagelistresult.Data.Count = DBConnectionManager.Instance.Reader.Count(new RecipesCountForSelectPageSpefication(param).Satifasy());

                //获取分页信息
                IList<RecipesInfo> selectresult = DBConnectionManager.Instance.Reader.Select<RecipesInfo>(new RecipesSelectPageSpefication(param).Satifasy());
                pagelistresult.Data.List = selectresult.CopyList<RecipesInfo, Recipes>();

            }
            catch (Exception ex)
            {
                pagelistresult.Status = false;
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:GetPageRecipes() .RecipesService"), LogType.ErrorLog);
            }


            return pagelistresult;
        }
    }
}
