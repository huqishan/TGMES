using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Model
{
    public class MesResult
    {
        /// <summary>
        /// mes反馈信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 结果状态
        /// </summary>
        public bool State { get; set; }
    }
}
