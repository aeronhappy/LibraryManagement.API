using LibraryManagement.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Contracts.Services
{
    public interface IEventBus
    {
        Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);
    }
}
