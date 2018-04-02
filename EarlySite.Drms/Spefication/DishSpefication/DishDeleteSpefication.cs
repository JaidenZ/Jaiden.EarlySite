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
            return string.Format(" delete from which_dish where DishId = '{0}' ", _id);
        }
    }
}
