using System.Collections.Generic;
using System.Web.Http;
using EarlySite.Business.IService;
using EarlySite.Core;
using EarlySite.Core.DDD.Service;
using EarlySite.Model.Common;
using EarlySite.Model.Show;
using EarlySite.WebAPI.Models;

namespace EarlySite.WebAPI.Controllers
{
    public class AccountController : ApiController
    {
        /// <summary>
        /// 根据手机号获取账户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost]
        public Result<Account> GetAccountInfoByPhone(long phone)
        {
            return ServiceObjectContainer.Get<IAccountService>().GetAccountInfo(phone);
        }



        [HttpPost]
        public Result<ConsoleModel> ConsoleModel()
        {

            ConsoleModel model = new Models.ConsoleModel();

            model.Age = 1;
            model.Name = "张三";
            model.Sex = 3;

            Result<ConsoleModel> result = new Result<Models.ConsoleModel>();
            result.Status = true;
            result.Data = model;

            return result;

        }

    }
}
