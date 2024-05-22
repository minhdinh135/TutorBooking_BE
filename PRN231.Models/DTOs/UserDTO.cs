namespace EXE101.Models.DTOs
{
    public class UserDTO
    {
        public Guid? Id { get; set; } 

        public required string UserName { get; set; }

        public required string Email { get; set; }

        public required string PhoneNumber { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string Address { get; set; }

        public string? HashPassword { get; set; }

        public Guid? CartId { get; set; }

    }
}
