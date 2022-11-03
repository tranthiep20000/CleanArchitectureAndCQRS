namespace CwkSocial.API.Contracts.Identity
{
    public class AuthenticationIdentityUser
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