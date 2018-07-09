namespace EarlySite.Web.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// 组件控制器
    /// </summary>
    public class ComponentController : BaseController
    {

        /// <summary>
        /// 生成单品分享组件界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenerateDishShareView()
        {

            ViewBag.Meal = 1;
            return View();
        }




    }
}