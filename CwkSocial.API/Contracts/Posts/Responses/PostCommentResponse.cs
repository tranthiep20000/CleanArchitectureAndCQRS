namespace CwkSocial.API.Contracts.Posts.Responses
{
    public class PostCommentResponse
    {
        public Guid PostId { get; set; }
        public Guid CommentId { get; set; }
        public string Text { get; set; }
        public Guid UserProfileId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
    }
}
