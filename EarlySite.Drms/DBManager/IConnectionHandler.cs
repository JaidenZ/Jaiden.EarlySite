namespace EarlySite.Drms.DBManager
{
    /// <summary>
    /// 连接控制器
    /// </summary>
    public interface IConnectionHandler
    {

        /// <summary>
        /// 连接字符串
        /// </summary>
        string connectionStr { get; set; }

        /// <summary>
        /// 是否连接到数据库
        /// </summary>
        bool Connected { get; set; }

        /// <summary>
        /// 开启连接
        /// </summary>
        void Start();

        /// <summary>
        /// 断开连接
        /// </summary>
        void Stop();

    }
}
