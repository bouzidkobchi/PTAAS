using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data.FluentApi
{
    public static class TargetSystemConfig
    {
        public static ModelBuilder AddTargetSystemEntity(this ModelBuilder modelBuilder)
        {
            var targetSystemEntity =  modelBuilder.Entity<TargetSystem>();

            targetSystemEntity.HasMany(ts => ts.Tests)
                .WithOne(t => t.System)
                .OnDelete(DeleteBehavior.NoAction);

            return modelBuilder;
        }
    }
}
