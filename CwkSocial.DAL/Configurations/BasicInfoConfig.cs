using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CwkSocial.DAL.Configurations
{
    internal class BasicInfoConfig : IEntityTypeConfiguration<BasicInfo>
    {
        public void Configure(EntityTypeBuilder<BasicInfo> builder)
        {
        }
    }
}