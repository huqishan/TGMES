using Prism.Events;
using Rep.Controls.Enum;
using Rep.Module.Communication.Services.CommunicationAggregate;
using Rep.Module.Communication.Services.Repositorys;
using System;

namespace Rep.Module.Communication.Services.Application.Commands
{
    public class CreateHandler
    {
        ICommunicationRepository _repository;
        IEventAggregator _aggregator;
        public CreateHandler(ICommunicationRepository repository, IEventAggregator aggregator)
        {
            _repository = repository;
            _aggregator = aggregator;
            _aggregator.GetEvent<CreateDBCommand>().Subscribe(Handle);
        }
        public void Handle(CreateModel message)
        {
            CommunicationEntity communication = new CommunicationEntity(message.ID,message.ProductID, message.ClientData, null, Convert.ToInt32(DataState.UnUpLoad), Convert.ToInt32(message.Type), message.ClientObj);
            _repository.Add(communication);
        }
    }
}
