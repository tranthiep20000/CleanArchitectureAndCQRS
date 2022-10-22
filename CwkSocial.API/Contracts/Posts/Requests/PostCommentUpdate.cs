using System.ComponentModel.DataAnnotations;

namespace CwkSocial.API.Contracts.Posts.Requests
{
    public class PostCommentUpdate
    {
        [Required]
        [MinLength(1)]
        public string TextComment { get; set; }
    }
}
