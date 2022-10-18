using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using Microsoft.VisualBasic;
using System.Xml.Linq;

namespace CwkSocial.API.Contracts.Posts.Responses
{
    public class PostResponse
    {
        public Guid PostId { get; set; }
        public Guid UserProfileId { get; set; }
        public string TextContent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
    }
}
