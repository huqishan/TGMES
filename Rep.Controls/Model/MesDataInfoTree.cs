using Rep.Controls.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rep.Controls.Model
{
    public class MesDataInfoTree
    {
        public MesDataInfoTree(IEnumerable<MesDataInfoItem> mesDataInfoItems,MothedType mothedType, string methodName)
        {
            this.MesDataInfoItems = mesDataInfoItems;
            this.MethodName = methodName;
            this.MothedType = mothedType;
        }
        /// <summary>
        /// 子集合
        /// </summary>
        public IEnumerable<MesDataInfoItem> MesDataInfoItems { get; private set; }
        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { get; private set; }
        public MothedType MothedType { get; private set; }
        public string Url { get; set; }
        public string Xsi { get; set; }
        public string Xsd { get; set; }
        public string Soap { get; set; }
        public string Xmlns { get; set; }
    }
    public class MesDataInfoItem
    {
        public MesDataInfoItem(string code, object value)
        {
            this.Code = code;
            this.Value = value;
        }
        /// <summary>
        /// 上传代码
        /// </summary>
        public string Code { get; private set; }
        /// <summary>
        /// 上传的数据
        /// </summary>
        public object Value { get; private set; }
    }
    
}
