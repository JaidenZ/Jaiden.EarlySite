namespace EarlySite.Business.Constract
{
    using System;
    using System.Collections.Generic;
    using Business.IService;
    using EarlySite.Drms.DBManager;
    using EarlySite.Drms.Spefication;
    using EarlySite.Model.Common;
    using EarlySite.Model.Enum;
    using EarlySite.Model.Show;
    using EarlySite.Model.Database;
    using EarlySite.Core.Utils;
    using EarlySite.Cache.CacheBase;
    using EarlySite.Core.DDD.Service;
    using System.Linq;

    public class DishService : IDishService
    {

        /// <summary>
        /// 根据编号精确获取单品食物
        /// </summary>
        /// <param name="dishId">单品食物编号</param>
        /// <returns></returns>
        public Result<Dish> SearchDishInfoById(int dishId)
        {
            Result<Dish> result = new Result<Dish>()
            {
                Data = null,
                Status = true
            };
            try
            {

                IDishCache service = ServiceObjectContainer.Get<IDishCache>();
                DishInfo dishinfo = service.GetDishInfoById(dishId);
                if(dishinfo != null)
                {
                    result.Data = dishinfo.Copy<Dish>();
                    result.Status = true;
                }
                else
                {
                    result.Status = false;
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "查询单品食物出错:" + ex.Message;
                result.StatusCode = "SD001";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:SearchDishInfoById() .DishService"), LogType.ErrorLog);
            }

            return result;
        }
        
        /// <summary>
        /// 根据名称模糊获取单品食物
        /// </summary>
        /// <param name="searchName">查询名字</param>
        /// <returns></returns>
        public Result<PageList<Dish>> SearchDishInfoByName(string searchName,PageSearchParam param)
        {
            Result<PageList<Dish>> result = new Result<PageList<Dish>>()
            {
                Data = null,
                Status = true
            };

            param.SearchCode = searchName;
            param.SearchType = 1;

            result.Data = new PageList<Dish>();
            result.Data.PageIndex = param.PageIndex;
            result.Data.PageNumer = param.PageNumer;

            try
            {
                //查询符合条件的数据总条数
                result.Data.Count = DBConnectionManager.Instance.Reader.Count(new DishCountForSelectPageSpefication(param).Satifasy());
                //查询数据集合
                IList<DishInfo> dish = DBConnectionManager.Instance.Reader.Select<DishInfo>(new DishSelectPagesPefication(param).Satifasy());
                result.Data.List = dish.CopyList<DishInfo, Dish>();
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "查询单品食物出错:" + ex.Message;
                result.StatusCode = "SD001";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:SearchDishInfoByName() .DishService"), LogType.ErrorLog);
            }

            return result;
        }

        /// <summary>
        /// 根据用餐时间获取单品食物
        /// </summary>
        /// <param name="time">用餐时间</param>
        /// <returns></returns>
        public Result<PageList<Dish>> SearchDishInfoByMealTime(MealTime time, PageSearchParam param)
        {
            Result<PageList<Dish>> result = new Result<PageList<Dish>>()
            {
                Data = null,
                Status = true
            };

            param.SearchCode = time.GetHashCode().ToString();
            param.SearchType = 2;

            result.Data = new PageList<Dish>();
            result.Data.PageIndex = param.PageIndex;
            result.Data.PageNumer = param.PageNumer;

            try
            {
                //查询符合条件的数据总条数
                result.Data.Count = DBConnectionManager.Instance.Reader.Count(new DishCountForSelectPageSpefication(param).Satifasy());
                //查询数据集合
                IList<DishInfo> dish = DBConnectionManager.Instance.Reader.Select<DishInfo>(new DishSelectPagesPefication(param).Satifasy());
                result.Data.List = dish.CopyList<DishInfo, Dish>();
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "查询单品食物出错:" + ex.Message;
                result.StatusCode = "SD001";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:SearchDishInfoByMealTime() .DishService"), LogType.ErrorLog);
            }

            return result;
        }

        /// <summary>
        /// 根据单品食品类型获取信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public Result<PageList<Dish>> SearchDishInfoByType(DishType type,PageSearchParam param)
        {
            Result<PageList<Dish>> result = new Result<PageList<Dish>>()
            {
                Data = null,
                Status = true
            };

            param.SearchCode = type.GetHashCode().ToString();
            param.SearchType = 3;

            result.Data = new PageList<Dish>();
            result.Data.PageIndex = param.PageIndex;
            result.Data.PageNumer = param.PageNumer;

            try
            {
                //查询符合条件的数据总条数
                result.Data.Count = DBConnectionManager.Instance.Reader.Count(new DishCountForSelectPageSpefication(param).Satifasy());
                //查询数据集合
                IList<DishInfo> dish = DBConnectionManager.Instance.Reader.Select<DishInfo>(new DishSelectPagesPefication(param).Satifasy());
                result.Data.List = dish.CopyList<DishInfo, Dish>();
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "查询单品食物出错:" + ex.Message;
                result.StatusCode = "SD001";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:SearchDishInfoByType() .DishService"), LogType.ErrorLog);
            }

            return result;

        }

        /// <summary>
        /// 分享单品食物信息
        /// </summary>
        /// <param name="share"></param>
        /// <returns></returns>
        public Result ShareDishInfo(DishShare share)
        {
            Result result = new Result()
            {
                Status = true,
                StatusCode = "SSD000",
                Message = "分享单品食物成功"
            };
            try
            {
                //新增一条单品记录
                bool cannext = false;
                DishInfo dishinfo = share.DishInfo.Copy<DishInfo>();
                ShopInfo updateshop = share.ShopInfo.Copy<ShopInfo>();
                RecipesInfo updaterecipes = share.RecipesInfo.Copy<RecipesInfo>();
                IList<RelationShareInfo> shareinfo = null;
                if (dishinfo == null)
                {
                    throw new ArgumentNullException("创建食物,单品食物参数不能为空");
                }
                if(updateshop == null)
                {
                    throw new ArgumentNullException("创建食物,门店信息参数不能为空");
                }
                if (updaterecipes == null)
                {
                    throw new ArgumentNullException("创建食物,食谱参数不能为空");
                }

                cannext = DBConnectionManager.Instance.Writer.Insert(new DishAddSpefication(dishinfo).Satifasy());

                //新增一条单品与食谱关系记录
                if (cannext)
                {
                    cannext = false;
                    shareinfo = new List<RelationShareInfo>();
                    RelationShareInfo sharerelation = new RelationShareInfo()
                    {
                        DishId = dishinfo.DIshId,
                        Phone = share.RecipesInfo.Phone,
                        RecipesId = share.RecipesInfo.RecipesId,
                        UpdateDate = DateTime.Now
                    };
                    shareinfo.Add(sharerelation);
                    cannext = DBConnectionManager.Instance.Writer.Insert(new RelationShareAddSpefication(shareinfo).Satifasy());

                }
                //更新门店信息(更新操作时间)
                if (cannext)
                {
                    updateshop.UpdateDate = DateTime.Now;
                    cannext = DBConnectionManager.Instance.Writer.Update(new ShopUpdateSpefication(updateshop).Satifasy());
                }
                //更新食谱信息(更新操作时间)
                if (cannext)
                {
                    updaterecipes.UpdateDate = DateTime.Now;
                    cannext = DBConnectionManager.Instance.Writer.Update(new RecipesUpdateSpefication(updaterecipes).Satifasy());
                }

                if (!cannext)
                {
                    DBConnectionManager.Instance.Writer.Rollback();
                    result.Status = false;
                    result.Message = "分享单品食物失败,请确保请求数据合法";
                }
                else
                {
                    DBConnectionManager.Instance.Writer.Commit();

                    //更新缓存
                    IRelationShareInfoCache shareservice = ServiceObjectContainer.Get<IRelationShareInfoCache>();
                    foreach (RelationShareInfo item in shareinfo)
                    {
                        shareservice.SaveInfo(item);
                    }
                    

                    IDishCache dishservice = ServiceObjectContainer.Get<IDishCache>();
                    dishservice.SaveInfo(dishinfo);

                    IShopCache shopservice = ServiceObjectContainer.Get<IShopCache>();
                    shopservice.SaveInfo(updateshop);

                    IRecipesCache recipesservice = ServiceObjectContainer.Get<IRecipesCache>();
                    recipesservice.SaveInfo(updaterecipes);
                    
                }
            }
            catch (Exception ex)
            {
                DBConnectionManager.Instance.Writer.Rollback();
                result.Status = false;
                result.Message = "分享单品食物失败:" + ex.Message;
                result.StatusCode = "SSD001";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:ShareDishInfo() .DishService"), LogType.ErrorLog);
            }
            return result;

        }

        /// <summary>
        /// 收藏单品食物到食谱
        /// </summary>
        /// <param name="collect"></param>
        /// <returns></returns>
        public Result CollectDishInfo(DishCollect collect)
        {
            Result result = new Result()
            {
                Status = true,
                StatusCode = "CD000",
                Message = "收藏单品食物成功"
            };
            try
            {
                IRecipesCache recipesservice = ServiceObjectContainer.Get<IRecipesCache>();
                IRelationShareInfoCache relationservice = ServiceObjectContainer.Get<IRelationShareInfoCache>();
                RecipesInfo updaterecipes = null;
                IList<RelationShareInfo> shareinfo = null;
                //新增一条单品记录
                bool cannext = false;
                
                //新增一条单品与食谱关系记录
                if (cannext)
                {
                    cannext = false;
                    shareinfo = new List<RelationShareInfo>();
                    RelationShareInfo sharerelation = new RelationShareInfo()
                    {
                        DishId = collect.DIshId,
                        Phone = collect.Phone,
                        RecipesId = collect.RecipesId,
                        UpdateDate = DateTime.Now
                    };
                    shareinfo.Add(sharerelation);
                    cannext = DBConnectionManager.Instance.Writer.Insert(new RelationShareAddSpefication(shareinfo).Satifasy());

                }
                //更新食谱信息(更新操作时间)
                if (cannext)
                {
                    cannext = false;
                    updaterecipes = recipesservice.GetRecipesInfoById(collect.RecipesId);
                    
                    if (updaterecipes == null)
                    {
                        throw new ArgumentNullException("收藏食物,食谱参数不能为空");
                    }
                    updaterecipes.UpdateDate = DateTime.Now;
                    cannext = DBConnectionManager.Instance.Writer.Update(new RecipesUpdateSpefication(updaterecipes).Satifasy());
                }

                if (!cannext)
                {
                    DBConnectionManager.Instance.Writer.Rollback();
                    result.Status = false;
                    result.Message = "收藏单品食物失败,请确保请求数据合法";
                }
                else
                {
                    DBConnectionManager.Instance.Writer.Commit();
                    //更新缓存
                    recipesservice.SaveInfo(updaterecipes);

                    foreach (RelationShareInfo item in shareinfo)
                    {
                        relationservice.SaveInfo(item);
                    }

                }
            }
            catch (Exception ex)
            {
                DBConnectionManager.Instance.Writer.Rollback();
                result.Status = false;
                result.Message = "收藏单品食物失败" + ex.Message;
                result.StatusCode = "CD001";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:CollectDishInfo() .DishService"), LogType.ErrorLog);
            }
            return result;
        }

        /// <summary>
        /// 获取单品分页信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Result<PageList<Dish>> GetPageDishInfo(PageSearchParam param)
        {
            Result<PageList<Dish>> result = new Result<PageList<Dish>>()
            {
                Data = null,
                Status = true
            };

            param.SearchType = 0;

            result.Data = new PageList<Dish>();
            result.Data.PageIndex = param.PageIndex;
            result.Data.PageNumer = param.PageNumer;

            try
            {
                //查询符合条件的数据总条数
                result.Data.Count = DBConnectionManager.Instance.Reader.Count(new DishCountForSelectPageSpefication(param).Satifasy());
                //查询数据集合
                IList<DishInfo> dish = DBConnectionManager.Instance.Reader.Select<DishInfo>(new DishSelectPagesPefication(param).Satifasy());
                result.Data.List = dish.CopyList<DishInfo, Dish>();
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "查询单品食物出错:" + ex.Message;
                result.StatusCode = "GPD001";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:GetPageDishInfo() .DishService"), LogType.ErrorLog);
            }

            return result;
        }

        /// <summary>
        /// 根据当前手机号分享的单品信息集合
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Result<IList<Dish>> GetShareDishList(long phone)
        {
            Result<IList<Dish>> result = new Result<IList<Dish>>()
            {
                Data = new List<Dish>(),
                Message = "查询单品信息集合成功",
                Status = true
            };

            try
            {
                //获取分享关系集合
                IRelationShareInfoCache relationcache = ServiceObjectContainer.Get<IRelationShareInfoCache>();
               
                IList<RelationShareInfo> relationshares = relationcache.GetRelationShareByPhone(phone);

                //查询单品信息
                if(relationshares != null && relationshares.Count > 0)
                {
                    IDishCache dishcache = ServiceObjectContainer.Get<IDishCache>();
                    IList<int> dishIds = new List<int>();
                    foreach (var share in relationshares)
                    {
                        DishInfo dish = dishcache.GetDishInfoById(share.DishId);
                        result.Data.Add(dish.Copy<Dish>());
                    }
                }
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = "查询分享单品食物出错:" + ex.Message;
                result.StatusCode = "GSD001";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:GetShareDishList() .DishService"), LogType.ErrorLog);
            }


            return result;
        }

        /// <summary>
        /// 根据食谱编号获取收藏的单品集合
        /// </summary>
        /// <param name="recipesId"></param>
        /// <returns></returns>
        public Result<IList<Dish>> GetCollectDishList(int recipesId)
        {
            Result<IList<Dish>> result = new Result<IList<Dish>>()
            {
                Data = new List<Dish>(),
                Message = "查询单品信息集合成功",
                Status = true
            };

            try
            {
                //获取分享关系集合
                IRelationShareInfoCache relationcache = ServiceObjectContainer.Get<IRelationShareInfoCache>();

                IList<RelationShareInfo> relationshares = relationcache.GetRelationShareByReceipId(recipesId);

                //查询单品信息
                if (relationshares != null && relationshares.Count > 0)
                {
                    IDishCache dishcache = ServiceObjectContainer.Get<IDishCache>();
                    IList<int> dishIds = new List<int>();
                    foreach (var share in relationshares)
                    {
                        DishInfo dish = dishcache.GetDishInfoById(share.DishId);
                        result.Data.Add(dish.Copy<Dish>());
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "查询分享单品食物出错:" + ex.Message;
                result.StatusCode = "GSD001";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:GetCollectDishList() .DishService"), LogType.ErrorLog);
            }


            return result;
        }

        public Result<IList<Dish>> GetFavoriteDishByPhone(long phone)
        {
            Result<IList<Dish>> result = new Result<IList<Dish>>()
            {
                Status = true,
                Message = "查找单品集合成功"
            };
            try
            {
                //收藏关系缓存
                IRelationFavoriteCache favoritecache = ServiceObjectContainer.Get<IRelationFavoriteCache>();
                //食谱缓存服务
                IDishCache dishcache = ServiceObjectContainer.Get<IDishCache>();
                if (phone == 0)
                {
                    throw new ArgumentException("获取食谱,参数非法");
                }

                //获取收藏关系
                IList<FavoriteInfo> favoritelist = favoritecache.GetFavoriteByPhone(phone, Model.Enum.FavoriteTypeEnum.收藏单品);
                IList<int> favoriterecipesId = favoritelist.Select(s => s.FavoriteId).ToList();

                //获取单品信息
                IList<DishInfo> favoritedishs = dishcache.GetDishInfoById(favoriterecipesId);

                if (favoritedishs != null && favoritedishs.Count > 0)
                {
                    result.Data = favoritedishs.CopyList<DishInfo, Dish>();
                    result.Status = true;
                }
                else
                {
                    result.Status = false;
                    result.Data = new List<Dish>();
                }

            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Data = new List<Dish>();
                result.Message = "查找单品出错:" + ex.Message;
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:GetFavoriteDishByPhone() .DishService"), LogType.ErrorLog);
            }

            return result;



        }
    }
}
