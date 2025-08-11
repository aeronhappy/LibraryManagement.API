namespace Identity.Application.Response
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public List<RoleResponse> Roles { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }

    }
}
