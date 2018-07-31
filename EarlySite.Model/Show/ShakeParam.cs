namespace EarlySite.Model.Show
{
    using Model.Enum;

    /// <summary>
    /// 摇一摇参数
    /// </summary>
    public class ShakeParam
    {
        /// <summary>
        /// 用餐时间段
        /// </summary>
        public int MealTime { get; set; }

        /// <summary>
        /// 口味类型
        /// </summary>
        public int DishType { get; set; }

        /// <summary>
        /// 用餐人数
        /// </summary>
        public int MealNumber { get; set; }

        /// <summary>
        /// 花费
        /// </summary>
        public int SpendGrade { get; set; }

        /// <summary>
        /// 当前定位经度
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// 当前定位纬度
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// 筛选附近的距离
        /// </summary>
        public int NearDistance { get; set; }
    }
}
