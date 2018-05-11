namespace EarlySite.Business.Constract
{
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Security;
    using EarlySite.Core.Component;
    using EarlySite.Core.Serialization;
    using EarlySite.Drms.DBManager.Provider;
    using EarlySite.Drms.Spefication;
    using IService;

    [TypeLibType(TypeLibTypeFlags.FRestricted | TypeLibTypeFlags.FLicensed)]
    public sealed class LoggerService : ILoggerService
    {
        
        private const string EXCATEGORY_NAME = "bs.exception";

        public sealed class Context
        {
            /// <summary>
            /// 堆栈跟踪信息
            /// </summary>
            public string StackTrace
            {
                get;
                set;
            }
            /// <summary>
            /// 异常提示信息
            /// </summary>
            public string Message
            {
                get;
                set;
            }
            /// <summary>
            /// 输入的数据
            /// </summary>
            public object[] Data
            {
                get;
                set;
            }
        }



        [SecuritySafeCritical]
        public void AddExceptionLog(Exception exception, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(exception != null);
            Context context = new Context
            {
                Message = exception.Message,
                Data = args,
                StackTrace = exception.StackTrace
            };
            AddRunningLog(EXCATEGORY_NAME, context);
        }

        [SecuritySafeCritical]
        public void AddRunningLog(string category, object message)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(category));
            using (MySqlDBWriter writer = new MySqlDBWriter())
            {
                try
                {
                    XmlSerializer xs = new XmlSerializer(); // 使用基础设施框架层面重写提供的序列化工具（不可第三方）
                    /*
                     * 先注明本算法支持深层次嵌套类型序列化 你需要值得注意与有趣之处在于
                     * 1、它是对于.NET/XML序列化的扩充（支持.NET/XML序列化的特性）
                     * 2、它的设计目的是架设于XML/DI技术层面（与Spring相类似)
                     * 3、它可以序列化对象型数组并加以反向序列化
                     */
                    string messagestr = string.Empty; // var lgr = xs.Deserialize<Logger>(message);
                    if (message != null)
                    {
                        messagestr = xs.Serializable(message);
                    }
                    AddSystemLoggerSpeficaiton logger = new AddSystemLoggerSpeficaiton();
                    writer.Insert(logger.Satifasy(),
                        writer.CreateParameter("@category", category, DbType.String),
                        writer.CreateParameter("@message", message, DbType.String),
                        writer.CreateParameter("@createdate", DateTime.Now, DbType.DateTime));
                    {
                        writer.Commit(); // 提交更改
                    }
                }
                catch (Exception) // 
                {
                    writer.Rollback(); // 回滚更改
                }
            }
        }
    }
}
