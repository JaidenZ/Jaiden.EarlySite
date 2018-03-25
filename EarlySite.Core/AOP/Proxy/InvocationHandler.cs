namespace EarlySite.Core.AOP.Proxy
{
    public interface InvocationHandler
    {
        object InvokeMember(object obj, int rid, string name, params object[] args);
    }
}
