using System.ComponentModel.DataAnnotations;

namespace CwkSocial.API.Contracts.Posts.Requests
{
    public class PostCreate
    {

        [Required]
        [MinLength(1)]
        public string TextContent { get; set; }
    }
}
