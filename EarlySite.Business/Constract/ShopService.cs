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

        public Result<Shop> GetShopInfoById(int shopId)
        {
            Result<Shop> result = new Result<Shop>()
            {
                Status = true,
                Message = "获取门店信息成功"
            };
            if(shopId == 0)
            {
                throw new ArgumentException("查询门店信息,参数非法");
            }

            try
            {
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
            }

            return result;
        }

        public Result RemoveShopInfoById(int shopId)
        {
            throw new NotImplementedException();
        }

        public Result<IList<Shop>> SearchShopInfoByName(string shopName)
        {
            throw new NotImplementedException();
        }

        public Result UpdateShopInfo(Shop shopInfo)
        {
            Result result = new Result()
            {
                Status = true,
                Message = "更新门店成功"
            };

            try
            {
                ShopInfo info = shopInfo.Copy<ShopInfo>();
                if (info == null)
                {
                    throw new ArgumentNullException("更新门店,参数不能为空");
                }
                info.UpdateDate = DateTime.Now;
                result.Status = DBConnectionManager.Instance.Writer.Insert(new ShopUpdateSpefication(info).Satifasy());

                if (result.Status)
                {
                    DBConnectionManager.Instance.Writer.Commit();
                }
                else
                {
                    DBConnectionManager.Instance.Writer.Rollback();
                    result.Message = "更新门店失败,请确认参数合法";
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
    }
}
