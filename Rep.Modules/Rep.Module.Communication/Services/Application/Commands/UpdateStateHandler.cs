using Prism.Events;
using Rep.Module.Communication.Services.Repositorys;
using System;

namespace Rep.Module.Communication.Services.Application.Commands
{
    public class UpdateStateHandler
    {
        ICommunicationRepository _repository;
        IEventAggregator _aggregator;
        public UpdateStateHandler(ICommunicationRepository repository, IEventAggregator aggregator)
        {
            _repository = repository;
            _aggregator = aggregator;
            _aggregator.GetEvent<UpdateStateCommand>().Subscribe(Handle);
        }
        public void Handle(UpdateDBStateMessage message)
        {
            var model=_repository.Get(message.ID);
            model.ChangeState(Convert.ToInt32(message.State), message.MESResult);
            _repository.Update(model);
        }
    }
}
