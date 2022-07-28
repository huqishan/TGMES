using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Enum
{
    public enum EquipmentState
    {
        [Description("运行状态")]
        OPERATING,
        [Description("待机状态")]
        STANDBY,
        [Description("故障状态")]
        FAULT,
        [Description("关机状态")]
        SHUTDOWN
    }
    public enum StandyReason
    {
        NULL,
        [Description("来料问题")]
        ABNORMAL_INCOMING,
        [Description("换料")]
        REFUELLING,
        [Description("后工序堵料")]
        ABNORMAL_BLOCKING,
        [Description("过程检")]
        PROCESS_INSPECTION,
        [Description("质量异常待判定")]
        ABNORMAL_QUALITY,
        [Description("信息化系统故障")]
        ABNORMAL_INFO,
        [Description("环境异常")]
        ABNORMAL_ENVIRON,
        [Description("计划停机")]
        PLAN_STANDBY
    }
}
