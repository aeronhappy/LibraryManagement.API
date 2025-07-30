
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Domain.Entities
{
    public class Role
    {
        public required RoleId Id { get; set; }
        public required string Name { get; set; }
        public List<User> Users { get; set; } = [];
    }
}

