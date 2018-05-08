namespace EarlySite.Business.IService
{
    using Model.Show;
    using Model.Common;
    using Model.Enum;
    using System.Collections.Generic;
    using Core.DDD.Service;

    /// <summary>
    /// 食谱操作服务
    /// </summary>
    public interface IRecipesService: IServiceBase
    {
        /// <summary>
        /// 获取分页食谱信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Result<PageList<Recipes>> GetPageRecipes(PageSearchParam param);

        /// <summary>
        /// 根据手机号获取食谱集
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Result<IList<Recipes>> GetRecipesByPhone(long phone);

        /// <summary>
        /// 根据食谱编号获取食谱集
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Result<Recipes> GetRecipesById(int recipesId);

        /// <summary>
        /// 创建食谱
        /// </summary>
        /// <param name="recipes">食谱信息</param>
        /// <returns></returns>
        Result CreatRecipes(Recipes recipes);

        /// <summary>
        /// 更新食谱信息
        /// </summary>
        /// <param name="recipes">食谱信息</param>
        /// <returns></returns>
        Result UpdateRecipes(Recipes recipes);

        /// <summary>
        /// 根据食谱编号删除食谱信息
        /// </summary>
        /// <param name="recipesId">食谱编号</param>
        /// <returns></returns>
        Result RemoveRecipesById(int recipesId);

        /// <summary>
        /// 根据手机号删除用户所有食谱信息
        /// </summary>
        /// <param name="phone">手机编号</param>
        /// <returns></returns>
        Result RemoveRecipesByPhone(long phone);
    }
}
