namespace EXE101.Models.DTOs
{
    public class UserInfoDTO
    {
        public Guid? Id { get; set; } 

        public required string ReceiverName { get; set; }

        public required string Email { get; set; }

        public required string Address { get; set; }

        public required string PhoneNumber { get; set; }

        public string? AddressInfo { get; set; }

        public string? AdditionalInfo { get; set; }

        public bool IsSaved { get; set; }

        public DateTime CreatedAt { get; set; }

        //public required DateTime DateOfBirth { get; set; }

        public required Guid UserId { get; set; }
    }
}
