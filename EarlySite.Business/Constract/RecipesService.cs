namespace EarlySite.Business.Constract
{
    using System;
    using System.Linq;
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
                //收藏关系缓存
                IRelationFavoriteCache favoritecache = ServiceObjectContainer.Get<IRelationFavoriteCache>();
                //食谱缓存服务
                IRecipesCache recipesservice = ServiceObjectContainer.Get<IRecipesCache>();
                if (phone == 0)
                {
                    throw new ArgumentException("获取食谱,参数非法");
                }

                //获取收藏关系
                IList<FavoriteInfo> favoritelist = favoritecache.GetFavoriteByPhone(phone, Model.Enum.FavoriteTypeEnum.收藏食谱);
                IList<int> favoriterecipesId = favoritelist.Select(s => s.FavoriteId).ToList();
                
                //获取食谱集信息
                IList<RecipesInfo> favoriteRecipes = recipesservice.GetRecipesInfoById(favoriterecipesId);

                if(favoriteRecipes != null && favoriteRecipes.Count > 0)
                {
                    result.Data = favoriteRecipes.CopyList<RecipesInfo, Recipes>();
                    result.Status = true;
                }
                else
                {
                    result.Status = false;
                    result.Data = new List<Recipes>();
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
                    result.Data = new List<Recipes>();
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

        /// <summary>
        /// 根据食谱编号移除整个食谱信息
        /// </summary>
        /// <param name="recipesId"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Result RemoveRecipesById(int recipesId,long phone)
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

                //食谱缓存服务
                IRecipesCache recipesservice = ServiceObjectContainer.Get<IRecipesCache>();
                IRelationShareInfoCache relationservice = ServiceObjectContainer.Get<IRelationShareInfoCache>();
                //删除食谱绑定关系
                bool cannext = false;
                cannext = DBConnectionManager.Instance.Writer.Update(new RelationShareDeleteSpefication(recipesId.ToString(),phone, 0).Satifasy());

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

                    relationservice.RemoveRelationShareByRecipes(recipesId, phone);
                    recipesservice.SetRecipesEnable(recipesId, false);

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

        /// <summary>
        /// 移除用户的所有食谱信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
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
                //食谱缓存服务
                IRecipesCache recipesservice = ServiceObjectContainer.Get<IRecipesCache>();
                IRelationShareInfoCache relationservice = ServiceObjectContainer.Get<IRelationShareInfoCache>();

                //删除食谱绑定关系
                bool cannext = false;
                cannext = DBConnectionManager.Instance.Writer.Update(new RelationShareDeleteSpefication("",phone, 2).Satifasy());

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

                    relationservice.RemoveRelationShareByPhone(phone);
                    recipesservice.SetRecipesEnable(phone, false);
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
                
                //食谱缓存服务
                IRecipesCache recipesservice = ServiceObjectContainer.Get<IRecipesCache>();

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
                    //更新缓存
                    recipesservice.SaveInfo(info);
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
            pagelistresult.Data = new PageList<Recipes>
            {
                PageIndex = param.PageIndex,
                PageNumer = param.PageNumer
            };
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

        /// <summary>
        /// 根据单品编号获取包含此单品的食谱集
        /// </summary>
        /// <param name="dishId">单品编号</param>
        /// <param name="num">获取食谱集数量</param>
        /// <returns></returns>
        public Result<IList<Recipes>> GetSomeRecpiesByDishId(int dishId, int num)
        {
            Result<IList<Recipes>> result = new Result<IList<Recipes>>()
            {
                Status = true,
                Data = new List<Recipes>(),
                Message = "成功获取包含此单品的食谱集"
            };
            if(dishId == 0 || num == 0)
            {
                result.Status = false;
                result.Message = "获取食谱失败,请检查参数合法性";
                return result;
            }
            try
            {
                //获取包含单品编号的关系集合
                IList<RelationShareInfo> shareinfo = ServiceObjectContainer.Get<IRelationShareInfoCache>().GetRelationShareByDishId(dishId).OrderByDescending(t =>t.UpdateDate).Take(num).ToList();
                
                if(shareinfo != null && shareinfo.Count > 0)
                {
                    //获取食谱集合信息
                    IRecipesCache cacheservice = ServiceObjectContainer.Get<IRecipesCache>();

                    foreach (var item in shareinfo)
                    {
                        Recipes recipe = cacheservice.GetRecipesInfoById(item.RecipesId).Copy<Recipes>();
                        result.Data.Add(recipe);
                    }
                }
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = "获取食谱集异常:" + ex.Message;
                result.StatusCode = "6000001";
            }

            return result;
        }

        /// <summary>
        /// 根据食谱编号获取包含此食谱单品的相似食谱集
        /// </summary>
        /// <param name="recipeId">获取食谱编号</param>
        /// <param name="num">获取食谱集数量</param>
        /// <returns></returns>
        public Result<IList<Recipes>> GetSomeRecpiesByRecipeId(int recipeId, int num)
        {

            Result<IList<Recipes>> result = new Result<IList<Recipes>>()
            {
                Status = true,
                Data = new List<Recipes>(),
                Message = "成功获取相似的食谱集"
            };
            if (recipeId == 0 || num == 0)
            {
                result.Status = false;
                result.Message = "获取食谱失败,请检查参数合法性";
                return result;
            }
            try
            {
                //获取食谱的单品
                IList<RelationShareInfo> recipesshare = ServiceObjectContainer.Get<IRelationShareInfoCache>().GetRelationShareByReceipId(recipeId).ToList();
                if(recipesshare != null && recipesshare.Count > 0)
                {
                    IList<int> dishids = new List<int>();
                    foreach (RelationShareInfo shareitem in recipesshare)
                    {
                        dishids.Add(shareitem.DishId);
                    }
                    //获取包含单品编号的其他的关系集合
                    IList<RelationShareInfo> shareinfo = ServiceObjectContainer.Get<IRelationShareInfoCache>().GetRelationShareByDishId(dishids).Where(i => i.RecipesId != recipeId).OrderByDescending(t => t.UpdateDate).Take(num).ToList();

                    if (shareinfo != null && shareinfo.Count > 0)
                    {
                        //获取食谱集合信息
                        foreach (var item in shareinfo)
                        {
                            Recipes recipe = ServiceObjectContainer.Get<IRecipesCache>().GetRecipesInfoById(item.RecipesId).Copy<Recipes>();
                            result.Data.Add(recipe);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "获取食谱集异常:" + ex.Message;
                result.StatusCode = "6000001";
            }

            return result;

        }
    }
}
