using System.ComponentModel.DataAnnotations;

namespace CwkSocial.API.Contracts.Posts.Requests
{
    public class PostCommentCreate
    {
        [Required]
        [MinLength(1)]
        public string TextComment { get; set; }
    }
}
