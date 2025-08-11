using Identity.Domain.DomainEvents;
using Identity.Domain.ValueObjects;
using LibraryManagement.SharedKernel.Entitites;

namespace Identity.Domain.Entities
{

    public class User : Entity
    {
        public UserId Id { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public List<Role> Roles { get; private set; } = [];
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastLogin { get; private set; }

        protected User()
        {

        }

        //private constructor
        private User(UserId id, string name, string email, string passwordHash, List<Role> roles)
        {
            Id = id;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Roles = roles;
            CreatedAt = DateTime.UtcNow;
            LastLogin = DateTime.UtcNow;

        }

        //factory method to create a new user
        public static User Create(string name, string email, string passwordHash, List<Role> roles)
        {
            var user = new User(new UserId(Guid.NewGuid()), name, email, passwordHash, roles);
            user.RaiseDomainEvent(new UserCreatedEvent(user));
            return user;
        }


        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
        }

        public void UpdateLastLogin(DateTime lastLoginDate)
        {
            LastLogin = lastLoginDate;
        }

        public void Update(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Name cannot be empty", nameof(newName));
            }
            Name = newName;
        }




    }
}
