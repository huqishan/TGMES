using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rep.Controls.Model
{
    public class UpLoadMessageModel
    {
        public IEnumerable<CommunicationDataModel> CommunicationDatas { get;set; }
        public bool IsResult { get; set; }
    }
}
