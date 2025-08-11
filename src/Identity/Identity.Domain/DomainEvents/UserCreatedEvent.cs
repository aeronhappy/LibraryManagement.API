using Identity.Domain.Entities;
using LibraryManagement.SharedKernel;

namespace Identity.Domain.DomainEvents
{
    public record UserCreatedEvent(User User) : IDomainEvent;
  
}
