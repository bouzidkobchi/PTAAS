using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

namespace WebApi.Data.FluentApi
{
    public class TargetSystemConfig : IEntityTypeConfiguration<TargetSystem>
    {
        public void Configure(EntityTypeBuilder<TargetSystem> builder)
        {
            builder.HasMany(ts => ts.Tests)
                .WithOne(t => t.System)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
