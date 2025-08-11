using LibraryManagement.SharedKernel;

namespace Identity.IntegrationEvent
{
    public record UserCreatedIntegrationEvent(Guid Id,string Email,string Name) : IIntegrationEvent;
   
}
