namespace LibraryManagement.API.Datas.Models
{
   
        public class User
        {
            public Guid Id { get; private set; }
            public string Name { get; private set; }
            public string Email { get; private set; }
            public string PasswordHash { get; private set; }
            public List<Role> Roles { get; private set; }

        protected User()
        {
            
        }

        //private constructor
        private User(Guid id, string name, string email, string passwordHash, List<Role> roles)
            {
                Id = id;
                Name = name;
                Email = email;
                PasswordHash = passwordHash;
                Roles = roles;
        }

            //factory method to create a new user
            public static User Create(string name, string email, string passwordHash,List<Role> roles)
            {
                return new User(Guid.NewGuid(), name, email, passwordHash ,roles);
            }


            public void ChangePassword(string newPasswordHash)
            {
                PasswordHash = newPasswordHash;
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
