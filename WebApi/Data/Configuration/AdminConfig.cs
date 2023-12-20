using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data.FluentApi
{
    public static class AdminConfig
    {
        public static ModelBuilder AddAdminEntity(this ModelBuilder modelBuilder)
        {
            var adminEntity = modelBuilder.Entity<Admin>();

            return modelBuilder;
        }
    }
}
