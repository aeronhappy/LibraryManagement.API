using LibraryManagement.SharedKernel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Contracts.Services
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        private readonly IPublisher _publisher;

        public DomainEventPublisher(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task PublishAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken)
        {
            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }
        }
    }
}
