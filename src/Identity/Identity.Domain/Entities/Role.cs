using Identity.Domain.ValueObjects;

namespace Identity.Domain.Entities
{
    public class Role
    {
        public required RoleId Id { get; set; }
        public required string Name { get; set; }
        public List<User> Users { get; set; } = [];
    }
}

