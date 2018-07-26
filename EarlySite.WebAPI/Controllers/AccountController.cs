using System.Collections.Generic;
using System.Web.Http;
using EarlySite.Business.IService;
using EarlySite.Core;
using EarlySite.Core.DDD.Service;
using EarlySite.Model.Common;
using EarlySite.Model.Show;

namespace EarlySite.WebAPI.Controllers
{
    public class AccountController : ApiController
    {
        /// <summary>
        /// 根据手机号获取账户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Result<Account> GetAccountInfoByPhone(long phone)
        {
            return ServiceObjectContainer.Get<IAccountService>().GetAccountInfo(phone);
        }


    }
}
