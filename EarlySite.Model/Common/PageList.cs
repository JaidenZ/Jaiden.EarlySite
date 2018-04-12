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
        public int PageNumer { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        public int PageCount
        {
            get
            {
                int count = this.Count / this.PageNumer;

                if ((this.Count % this.PageNumer) > 0)
                {
                    return count + 1;
                }
                else
                {
                    return count;
                }

            }
        }

        /// <summary>
        /// 数据集合
        /// </summary>
        public IList<T> List { get; set; }
    }

}
