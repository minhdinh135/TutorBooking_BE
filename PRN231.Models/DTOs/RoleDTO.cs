namespace EXE101.Models.DTOs
{
    public class RoleDTO
    {
        public Guid? Id { get; set; } 

        public required string Name { get; set; }

        public required Guid UserId { get; set; }

    }
}
