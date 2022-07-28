using Rep.Module.Communication.Services.CommunicationAggregate;
using Rep.Module.Communication.Services.Contexts;
using Shared.Abstractions;

namespace Rep.Module.Communication.Services.Repositorys
{
    public class CommunicationRepository : Repository<CommunicationEntity,string,CommuniactionContext>, ICommunicationRepository
    {
        public CommunicationRepository(CommuniactionContext context) : base(context)
        {

        }
    }
}
