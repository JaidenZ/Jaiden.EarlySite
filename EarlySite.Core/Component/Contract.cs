namespace EarlySite.Core.Component
{
    using System;
    using System.Security;

    /// <summary>
    /// 包含用于表示程序协定（如前置条件、后置条件和对象固定）的静态方法。
    /// </summary>
    public static class Contract
    {
        /// <summary>
        /// 为封闭方法或属性指定一个前置条件协定。
        /// </summary>
        //[Conditional("CONTRACTS_FULL")]
        public static void Requires<T>(bool condition) where T : Exception, new()
        {
            Contract.Requires<T>(condition, null);
        }

        /// <summary>
        /// 为封闭方法或属性指定一个前置条件协定，并在该协定的条件失败时显示一条消息。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">要测试的条件表达式。</param>
        /// <param name="message">如果条件为显示的消息 false。</param>
        //[Conditional("CONTRACTS_FULL")]
        public static void Requires<T>(bool condition, string message) where T : Exception, new()
        {
            if (!condition)
                Contract.Throw<T>(message);
        }

        [SecuritySafeCritical]
        private static void Throw<T>(string message) where T : Exception, new()
        {
            if (string.IsNullOrEmpty(message))
                throw new T();
            else
            {
                var ctor = typeof(T).GetConstructor(new Type[] { typeof(string) });
                throw ((T)ctor.Invoke(new object[] { message }));
            }
        }

        /// <summary>
        /// 参数指定应为的后置条件 true 封闭方法或属性时正常返回。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">要测试的条件表达式。</param>
        //[Conditional("CONTRACTS_FULL")]
        public static void Ensures<T>(bool condition) where T : Exception, new()
        {
            Contract.Requires<T>(condition);
        }

        /// <summary>
        /// 参数指定应为的后置条件 true 封闭方法或属性时正常返回。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">要测试的条件表达式。</param>
        /// <param name="message">如果条件为显示的消息 false。</param>
        //[Conditional("CONTRACTS_FULL")]
        public static void Ensures<T>(bool condition, string message) where T : Exception, new()
        {
            Contract.Requires<T>(condition, message);
        }

        /// <summary>
        /// 为封闭方法或属性指定一个固定的协定。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">要测试的条件表达式。</param>
        //[Conditional("CONTRACTS_FULL")]
        public static void Invariant<T>(bool condition) where T : Exception, new()
        {
            Contract.Requires<T>(condition);
        }

        /// <summary>
        /// 为封闭方法或属性指定一个固定的协定。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">要测试的条件表达式。</param>
        /// <param name="message">如果条件为显示的消息 false。</param>
        //[Conditional("CONTRACTS_FULL")]
        public static void Invariant<T>(bool condition, string message) where T : Exception, new()
        {
            Contract.Requires<T>(condition, message);
        }
    }
}
