namespace EarlySite.Drms.Spefication
{
    /// <summary>
    /// 单品删除规约
    /// </summary>
    public class DishDeleteSpefication : SpeficationBase
    {
        private int _id;

        public DishDeleteSpefication(int dishId)
        {
            _id = dishId;
        }

        public override string Satifasy()
        {
            return string.Format(" update which_shop set Enable = '1' where DishId = '{0}' ", _id);
        }
    }
}
