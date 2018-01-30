namespace EarlySite.Drms.DBManager
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 连接控制器
    /// </summary>
    public interface IConnectionHandler
    {

        /// <summary>
        /// 连接字符串
        /// </summary>
        string connectionStr { get; }

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
        
        /// <summary>
        /// 执行获取表数据集合
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        IList<object> ExecuteDataTableList(string command);

        /// <summary>
        /// 执行获取结果
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string command);
    }
}
