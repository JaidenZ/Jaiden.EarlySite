namespace EarlySite.Drms.DBManager.Connection
{
    /// <summary>
    /// 连接处理器
    /// </summary>
    public interface IConnection
    {

        /// <summary>
        /// 连接字符串
        /// </summary>
        string connectionStr { get; }

        /// <summary>
        /// 是否连接到数据库
        /// </summary>
        bool Connected { get; }

    }
}
