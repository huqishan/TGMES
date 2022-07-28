using Shared.Abstractions.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Abstractions.Model
{
    public class LogMessageModel
    {
        public DateTime LogTime { get; set; }= DateTime.Now;
        public string Message { get; set; }
        public LogType Type { get; set; }
    }
}
