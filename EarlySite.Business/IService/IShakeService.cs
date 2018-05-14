namespace EarlySite.Business.IService
{
    using Model.Show;
    using Model.Common;
    using Core.DDD.Service;
    using EarlySite.Business.Filter;


    /// <summary>
    /// 筛选服务
    /// </summary>
    [ServiceObject(ServiceName = "筛选操作服务", ServiceFilter = typeof(ShakeServiceFilter))]
    public interface IShakeService:IServiceBase
    {
    }
}
