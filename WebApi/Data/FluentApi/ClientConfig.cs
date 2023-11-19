using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data.FluentApi
{
    public static class ClientConfig
    {
        public static ModelBuilder AddClientEntity(this ModelBuilder modelBuilder)
        {
            var clientEntity = modelBuilder.Entity<Client>();

            clientEntity.HasMany(c => c.RequestedTests)
                .WithOne(t => t.Owner)
                .HasForeignKey(t => t.OwnerId);

            return modelBuilder;
        }
    }
}
