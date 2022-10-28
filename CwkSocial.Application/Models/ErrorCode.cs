namespace CwkSocial.APPLICATION.Models
{
    public enum ErrorCode
    {
        NotFound = 404,
        ServerError = 500,

        // Validation errors should be in the range 100 - 199
        ValidationError = 101,

        // Infrastructure errors should be in the range 201 - 299
        IdentityUserAlreadyExists = 201,
        IdentityCreationFailed = 202,
        IdentityUserDoesNotExsist = 203,
        IncorrectPassword = 204,

        UnknowError = 999
    }
}