namespace EarlySite.Drms.Spefication
{
    /// <summary>
    /// 日志纪录规约
    /// </summary>
    public class AddSystemLoggerSpeficaiton : SpeficationBase
    {
        public override string Satifasy()
        {
            return "INSERT INTO logger(Category,Message,CreateDate) VALUES(@category,@message,@createdate);";

        }
    }
}
