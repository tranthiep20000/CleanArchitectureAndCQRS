namespace CwkSocial.API
{
    public class ApiRoutes
    {
        public const string BaseRoute = "api/v{version:apiversion}/[controller]";

        public class UserProfiles
        {
            public const string IdRoute = "{id}";
        }

        public class Posts
        {
            public const string IdRoute = "{id}";
            public const string PostComments = "{postId}/comments";
            public const string CommentById = "{postId}/comments/{commentId}";
            public const string PostInteractions = "{postId}/interactions";
            public const string InteractionById = "{postId}/interactions/{interactionId}";
        }

        public class Identity
        {
            public const string Login = "login";
            public const string Registration = "registration";
            public const string RemoveAccount = "removeaccount";
        }
    }
}