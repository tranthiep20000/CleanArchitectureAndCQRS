namespace CwkSocial.APPLICATION.Identity.Dtos
{
    public class AuthenticationIdentityUserDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrentCity { get; set; }
        public string Token { get; set; }
    }
}