using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarlySite.Model.Common
{
    public class PageSearchParam
    {

        /// <summary>
        /// 查询码
        /// </summary>
        public string SearchCode { get; set; }

        /// <summary>
        /// 查询类型,根据不同环境匹配
        /// </summary>
        public int SearchType { get; set; }

        /// <summary>
        /// 索引页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示
        /// </summary>
        public int PageNumer { get; set; }

    }
}
