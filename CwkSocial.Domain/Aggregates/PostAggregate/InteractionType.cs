using System.ComponentModel;

namespace CwkSocial.DOMAIN.Aggregates.PostAggregate
{
    public enum InteractionType
    {
        [Description("Like")] Like = 1,
        [Description("Dislike")] Dislike = 2,
        [Description("Haha")] Haha = 3,
        [Description("Wow")] Wow = 4,
        [Description("Love")] Love = 5,
        [Description("Angry")] Angry = 6
    }
}