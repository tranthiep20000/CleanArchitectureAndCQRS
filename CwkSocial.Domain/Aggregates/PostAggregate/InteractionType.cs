using System.ComponentModel;

namespace CwkSocial.DOMAIN.Aggregates.PostAggregate
{
    public enum InteractionType
    {
        [Description("Like")] Like,
        [Description("DisLike")] DisLike,
        [Description("Haha")] Haha,
        [Description("Wow")] Wow,
        [Description("Love")] Love,
        [Description("Angry")] Angry
    }
}