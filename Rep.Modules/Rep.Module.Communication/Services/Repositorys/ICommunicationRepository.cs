using Rep.Module.Communication.Services.CommunicationAggregate;
using Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rep.Module.Communication.Services.Repositorys
{
    public interface ICommunicationRepository : IRepository<CommunicationEntity,string>
    {
    }
}
