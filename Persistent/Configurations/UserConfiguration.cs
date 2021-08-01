using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Model;

namespace Persistent.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> template)
        {
            template.ToTable("User");
            
            template.HasKey(e => e.Id);
            template.Property(e => e.Email).HasMaxLength(20);

            
        }
    }
}
