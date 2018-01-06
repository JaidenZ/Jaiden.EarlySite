using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EarlySite.Core.Utils
{
    /// <summary>
    /// Description:The common tool to save system log
    /// Author:Haojun.Zhao
    /// </summary>
    public class LoggerUtils
    {
        public LogType type;

        /// <summary>
        /// To build a Log
        /// </summary>
        public static void LogIn(string content,LogType type)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Log\\";
            string filename = "";
            switch (type)
            {
                case LogType.ErrorLog:
                    path += "ErrorLog";
                    filename = path + "\\Error" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    break;
                case LogType.WorkingLog:
                    path += "WorkingLog";
                    filename = path + "\\Working" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    break;
                case LogType.SqlLog:
                    path += "SqlErrorLog";
                    filename = path + "\\Sql" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    break;
                default:
                    path += "Log";
                    filename = path + "\\Log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    break;
            }
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.AppendAllText(filename, content);
        }


        /// <summary>
        /// Get SQL Exception info
        /// </summary>
        /// <param name="ex">exception</param>
        /// <param name="backStr">The location of exception</param>
        /// <returns></returns>
        public static string ColectSqlExceptionMessage(string sql,string backStr,string param = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("*************************SQL异常文本*************************");
            sb.AppendLine("【出现时间】：" + DateTime.Now.ToString());
            sb.AppendLine("【出现地点】：" + backStr);
            sb.AppendLine("【SQL语句】：" + sql);
            sb.AppendLine("【SQL参数】：\r\n" + param);
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }

        /// <summary>
        /// Get Exception message info
        /// </summary>
        /// <param name="ex">exception</param>
        /// <param name="backStr">The location of exception</param>
        /// <returns></returns>
        public static string ColectExceptionMessage(Exception ex,string backStr)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("****************************异常文本****************************");
            sb.AppendLine("【出现时间】：" + DateTime.Now.ToString());
            sb.AppendLine("【出现地点】：" + backStr);
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
            }
            else
            {
                sb.AppendLine("【未处理异常】：" + backStr);
            }
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }

    }


    /// <summary>
    /// The enum with log
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 错误日志
        /// </summary>
        ErrorLog = 0x00,
        /// <summary>
        /// 运行日志
        /// </summary>
        WorkingLog = 0x01,
        /// <summary>
        /// SQL错误日志
        /// </summary>
        SqlLog = 0x02

    }
}
