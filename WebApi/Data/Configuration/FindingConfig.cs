using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data.FluentApi
{
    public static class FindingConfig
    {
        public static ModelBuilder AddFindingEntity(this ModelBuilder modelBuilder)
        {
            var findingEntity = modelBuilder.Entity<Finding>();

            findingEntity.HasOne(f => f.Founder)
                .WithMany()
                .HasForeignKey(f => f.FounderId);

            findingEntity.HasOne(f => f.Test)
                .WithMany(t => t.Findings)
                .HasForeignKey(f => f.TestId);

            return modelBuilder;
        }
    }
}
