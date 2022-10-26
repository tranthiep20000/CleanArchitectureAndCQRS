using System.ComponentModel.DataAnnotations;

namespace CwkSocial.API.Contracts.Identity
{
    public class UserRegistration
    {
        [EmailAddress]
        [Required]
        public string Username { get; set; }
        
        [Required]        
        public string Password { get; set; }
        
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string LastName { get; set; }
        
        [Required]        
        public DateTime DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }
        public string CurrentCity { get; set; }
    }
}
