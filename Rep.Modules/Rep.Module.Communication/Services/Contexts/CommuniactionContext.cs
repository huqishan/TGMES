using Rep.Module.Communication.Services.CommunicationAggregate;
using Shared.Abstractions;
using SqlSugar;

namespace Rep.Module.Communication.Services.Contexts
{
    public class CommuniactionContext : DBContext<CommunicationEntity>
    {
        public CommuniactionContext(ConnectionConfig config) : base(config)
        {

        }
    }
}
