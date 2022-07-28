using Prism.Events;
using Rep.Controls.Enum;

namespace Rep.Module.Communication.Services.Application.Commands
{
    public class CreateDBCommand : PubSubEvent<CreateModel>
    {

    }
    public class CreateModel
    {
        public string ID { get; set; }
        public string ProductID { get; set; }
        public string ClientData { get; set; }
        public string ClientObj { get; set; }
        public MothedType Type { get; set; }
    }
}
