namespace CwkSocial.APPLICATION.Posts
{
    public class PostErrorMessage
    {
        public const string PostNotFound = "No Post with ID {0}";
        public const string UserProfileNotFound = "No UserProfile with ID {0}";
        public const string PostCommentNotFound = "No PostComment with ID {0}";
        public const string PostInteractionNotFound = "No PostInteraction with ID {0}";
        public const string PostDeleteNotPossible = "Only the owner of a post can delete it";
        public const string PostUpdateNotPossible = "Post update not possible because it's not the post owner that initiates the update";
        public const string CommentDeleteNotPossible = "Only the owner of a comment can delete it";
        public const string InteractionDeleteNotPossible = "Only the owner of a interaction can delete it";
    }
}