using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

namespace WebApi.Data.FluentApi
{
    public class PentrationTestConfig : IEntityTypeConfiguration<PentrationTest>
    {
        public void Configure(EntityTypeBuilder<PentrationTest> builder)
        {
            builder.HasOne(t => t.System)
                .WithMany(s => s.Tests)
                .HasForeignKey(t => t.SystemId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Owner)
                .WithMany(t => t.RequestedTests)
                .HasForeignKey(t => t.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(t => t.Status);
        }
    }
}
