namespace PRN231.Models.DTOs
{
    public class UserInfoDTO
    {
        public int Id { get; set; } 

        public required string ReceiverName { get; set; }

        public required string Email { get; set; }

        public required string Address { get; set; }

        public required string PhoneNumber { get; set; }

        public required string Gender { get; set; }
        public required string Status { get; set; }
        public required string Avatar { get; set; }
    }
}
