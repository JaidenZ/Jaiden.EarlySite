namespace EarlySite.Model.Common
{
    using System.Collections.Generic;

    public class PageList<T>
    {
        /// <summary>
        /// 索引页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        public int PageNumber
        {
            get
            {
                int number = this.Count / this.PageCount;

                if ((this.Count % this.PageCount) > 0)
                {
                    return number + 1;
                }
                else
                {
                    return number;
                }

            }
        }

        /// <summary>
        /// 数据集合
        /// </summary>
        public IList<T> List { get; set; }
    }

}
