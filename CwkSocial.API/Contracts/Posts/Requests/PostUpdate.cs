using System.ComponentModel.DataAnnotations;

namespace CwkSocial.API.Contracts.Posts.Requests
{
    public class PostUpdate
    {
        [Required]
        [MinLength(1)]
        public string TextContent { get; set; }
    }
}