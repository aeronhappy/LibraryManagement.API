using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Domain.Entities
{

    public class User
    {
        public UserId Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public List<Role> Roles { get; private set; }
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
            return new User(new UserId(Guid.NewGuid()), name, email, passwordHash, roles);
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
