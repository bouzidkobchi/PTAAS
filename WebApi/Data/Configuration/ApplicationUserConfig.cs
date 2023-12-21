using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

namespace WebApi.Data.FluentApi
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            builder.HasMany(u => u.TokenTests)
                .WithMany(t => t.Pentesters);

            builder.HasMany(u => u.RequestedTests)
                .WithOne(t => t.Owner);
        }
    }
}
