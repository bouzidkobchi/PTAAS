using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

namespace WebApi.Data.FluentApi
{
    public class FindingConfig : IEntityTypeConfiguration<Finding>
    {
        public void Configure(EntityTypeBuilder<Finding> builder)
        {
            builder.HasOne(f => f.Founder)
                .WithMany()
                .HasForeignKey(f => f.FounderId);

            builder.HasOne(f => f.Test)
                .WithMany(t => t.Findings)
                .HasForeignKey(f => f.TestId);
        }
    }
}
