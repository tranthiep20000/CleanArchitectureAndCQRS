using System.ComponentModel.DataAnnotations;

namespace CwkSocial.API.Contracts.Posts.Requests
{
    public class PostCommentCreate
    {
        [Required]
        public Guid UserProfileId { get; set; }

        [Required]
        [MinLength(1)]
        public string TextComment { get; set; }
    }
}
