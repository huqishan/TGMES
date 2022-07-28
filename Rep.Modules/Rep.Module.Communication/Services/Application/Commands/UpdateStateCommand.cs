using Prism.Events;
using Rep.Controls.Enum;

namespace Rep.Module.Communication.Services.Application.Commands
{
    public class UpdateStateCommand : PubSubEvent<UpdateDBStateMessage>
    {
    }
    public class UpdateDBStateMessage
    {
        public string ID { get; set; }
        public string MESResult { get; set; }
        public DataState State { get; set; }
    }
}
