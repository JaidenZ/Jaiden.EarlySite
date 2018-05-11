namespace EarlySite.Business.IService
{
    using Core.DDD.Service;
    using System;


    /// <summary>
    /// 日志记录服务
    /// </summary>
    public interface ILoggerService : IServiceBase
    {
        /// <summary>
        /// 添加运行日志
        /// </summary>
        /// <param name="category">类别</param>
        /// <param name="message">信息</param>
        void AddRunningLog(string category, object message);

        /// <summary>
        /// 添加异常错误日志
        /// </summary>
        /// <param name="exception">异常</param>
        /// <param name="args">发生异常时的仿真数据</param>
        void AddExceptionLog(Exception exception, params object[] args);

    }
}
