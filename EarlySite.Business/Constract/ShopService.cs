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

    public class ShopService : IShopService
    {
        public Result CreatShopInfo(Shop shopInfo)
        {
            Result result = new Result()
            {
                Status = true,
                Message = "创建门店成功"
            };

            try
            {
                ShopInfo info = shopInfo.Copy<ShopInfo>();
                if(info == null)
                {
                    throw new ArgumentNullException("创建门店,参数不能为空");
                }
                info.UpdateDate = DateTime.Now;
                result.Status = DBConnectionManager.Instance.Writer.Insert(new ShopAddSpefication(info).Satifasy());

                if(result.Status)
                {
                    DBConnectionManager.Instance.Writer.Commit();
                }
                else
                {
                    DBConnectionManager.Instance.Writer.Rollback();
                    result.Message = "创建门店失败,请确认参数合法";
                }

            }
            catch (Exception ex)
            {

                DBConnectionManager.Instance.Writer.Rollback();
                result.Status = false;
                result.Message = "创建门店出错:" + ex.Message;
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:CreatShopInfo() .ShopService"), LogType.ErrorLog);

            }

            return result;
        }

        /// <summary>
        /// 根据门店编号获取信息
        /// </summary>
        /// <param name="shopId">门店编号</param>
        /// <returns>操作结果</returns>
        public Result<Shop> GetShopInfoById(int shopId)
        {
            Result<Shop> result = new Result<Shop>()
            {
                Status = true,
                Message = "获取门店信息成功"
            };
            
            try
            {
                if (shopId == 0)
                {
                    throw new ArgumentException("查询门店信息,参数非法");
                }
                //根据门店编号获取门店信息
                IList<ShopInfo> shopinfolist = DBConnectionManager.Instance.Reader.Select<ShopInfo>(new ShopSelectSpefication(shopId.ToString(), 0).Satifasy());

                if(shopinfolist != null && shopinfolist.Count > 0)
                {
                    result.Data = shopinfolist[0].Copy<Shop>();
                }
                else
                {
                    result.Data = null;
                    result.Status = false;
                    result.Message = "没有获取到对应信息";
                }

            }
            catch(Exception ex)
            {
                result.Data = null;
                result.Status = false;
                result.Message = "获取门店信息出错:" + ex.Message;
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:GetShopInfoById() .ShopService"), LogType.ErrorLog);
            }

            return result;
        }

        /// <summary>
        /// 根据门店编号删除门店信息 (禁用门店信息)
        /// </summary>
        /// <param name="shopId">门店编号</param>
        /// <returns>操作结果</returns>
        public Result RemoveShopInfoById(int shopId)
        {
            Result result = new Result()
            {
                Status = true,
                Message = "删除门店成功"
            };
            try
            {
                if (shopId == 0)
                {
                    throw new ArgumentException("查询门店信息,参数非法");
                }

                bool execstatus = DBConnectionManager.Instance.Writer.Update(new ShopDeleteSpefication(shopId).Satifasy());

                if (!execstatus)
                {
                    result.Status = false;
                    result.Message = "删除门店操作失败,请检查参数数据";
                }

            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = "获取门店信息出错:" + ex.Message;
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:RemoveShopInfoById() .ShopService"), LogType.ErrorLog);
            }

            return result;
        }

        public Result<IList<Shop>> SearchShopInfoByName(string shopName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新门店信息
        /// </summary>
        /// <param name="shopInfo">门店信息</param>
        /// <returns>操作结果</returns>
        public Result UpdateShopInfo(Shop shopInfo)
        {
            Result result = new Result()
            {
                Status = true,
                Message = "更新门店成功"
            };

            try
            {
                bool cannext = false;

                ShopInfo info = shopInfo.Copy<ShopInfo>();
                if (info == null)
                {
                    throw new ArgumentNullException("更新门店,参数不能为空");
                }
                info.UpdateDate = DateTime.Now;
                //更新门店信息
                cannext = DBConnectionManager.Instance.Writer.Update(new ShopUpdateSpefication(info).Satifasy());
                //门店信息更新成功,更新关联食物的门店名称
                if (cannext)
                {
                    cannext = false;
                    cannext = DBConnectionManager.Instance.Writer.Update(new DishShopNameUpdateSpefication(info.ShopId, info.ShopName).Satifasy());
                    
                }
                result.Status = cannext;

                if(!cannext)
                {
                    DBConnectionManager.Instance.Writer.Rollback();
                    result.Message = "更新门店失败,请确认参数合法";
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
                result.Message = "更新门店出错:" + ex.Message;
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:UpdateShopInfo() .ShopService"), LogType.ErrorLog);
            }

            return result;
        }

        /// <summary>
        /// 获取门店分页列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Result<PageList<Shop>> GetShopPageList(PageSearchParam param)
        {
            Result<PageList<Shop>> pagelistresult = new Result<PageList<Shop>>()
            {
                Status = true
            };
            pagelistresult.Data = new PageList<Shop>();
            pagelistresult.Data.PageIndex = param.PageIndex;
            pagelistresult.Data.PageNumer = param.PageNumer;
            

            try
            {
                //获取总数
                pagelistresult.Data.Count = DBConnectionManager.Instance.Reader.Count(new ShopCountForSelectPageSpefication(param).Satifasy());

                //获取分页信息
                IList<ShopInfo> selectresult = DBConnectionManager.Instance.Reader.Select<ShopInfo>(new ShopSelectPageSpefication(param).Satifasy());
                pagelistresult.Data.List = selectresult.CopyList<ShopInfo, Shop>();

            }
            catch(Exception ex)
            {
                pagelistresult.Status = false;
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:GetShopPageList() .ShopService"), LogType.ErrorLog);
            }


            return pagelistresult;
        }

    }
}
