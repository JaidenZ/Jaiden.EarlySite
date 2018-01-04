namespace EarlySite.Model.Common
{
    
    /// <summary>
    /// 返回结果
    /// </summary>
    public class Result
    {

        /// <summary>
        /// 结果状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 结果代码
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// 结果文本
        /// </summary>
        public string Message { get; set; }


        /// <summary>
        /// 失败时候调用
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Result Fail(string message)
        {
            return new Result { Status = false, Message = message };
        }

        /// <summary>
        /// 失败时调用
        /// </summary>
        /// <param name="statusCode">错误异常时的代码</param>
        /// <param name="message">结果文本</param>
        /// <returns></returns>
        public static Result Fail(string statusCode, string message)
        {
            var result = Fail(message);
            result.StatusCode = statusCode;
            return result;
        }

        /// <summary>
        /// 成功时候调用
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Result Success(string message)
        {
            return new Result { Status = true, Message = message };
        }
    }

    public class Result<T> : Result
    {
        /// <summary>
        /// 返回数据集
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 失败时候调用
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Result<T> Fail(string message)
        {
            return new Result<T> { Status = false, Message = message };
        }

        /// <summary>
        /// 失败时调用
        /// </summary>
        /// <param name="statusCode">错误异常时的代码</param>
        /// <param name="message">结果文本</param>
        /// <returns></returns>
        public static Result<T> Fail(string statusCode, string message)
        {
            var result = Fail(message);
            result.StatusCode = statusCode;
            return result;
        }

        /// <summary>
        /// 成功时候调用
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Result<T> Success(string message)
        {
            return new Result<T> { Status = true, Message = message };
        }
    }
}
