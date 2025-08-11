using Borrowing.Application.Commands;
using Identity.IntegrationEvent;
using MediatR;

namespace Borrowing.Application.EventHandler
{


    public class UserCreatedIntegrationEventHandler : INotificationHandler<UserCreatedIntegrationEvent>
    {
        private readonly IMemberCommands _memberCommands;

        public UserCreatedIntegrationEventHandler(IMemberCommands memberCommands)
        {
            _memberCommands = memberCommands;
        }

        public async Task Handle(UserCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            await _memberCommands.CreateMemberAsync(notification.Name, notification.Email ,cancellationToken);
        }
    }
}
