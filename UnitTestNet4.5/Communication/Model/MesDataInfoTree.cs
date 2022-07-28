using Communication.Enum;
using System.Collections.Generic;

namespace Communication.Model
{
    public class MesDataInfoTree
    {
        public MesDataInfoTree(string productId, IEnumerable<MesDataInfoItem> mesDataInfoItems, MothedType mothedType, string methodName)
        {
            this.MesDataInfoItems = mesDataInfoItems;
            this.MethodName = methodName;
            this.MothedType = mothedType;
            this.ProductID = productId;
        }
        /// <summary>
        /// 子集合
        /// </summary>
        public IEnumerable<MesDataInfoItem> MesDataInfoItems { get; private set; }
        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { get; private set; }
        public string ProductID { get; private set; }
        public MothedType MothedType { get; private set; }
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
