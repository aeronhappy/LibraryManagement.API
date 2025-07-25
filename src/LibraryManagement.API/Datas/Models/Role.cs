namespace LibraryManagement.API.Datas.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public required String Name { get; set; }
        public List<User> Users { get; set; } = [];
    }
}

