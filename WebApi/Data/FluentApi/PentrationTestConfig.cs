﻿using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data.FluentApi
{
    public static class PentrationTestConfig
    {
        public static ModelBuilder AddPentrationTestEntity(this ModelBuilder modelBuilder)
        {
            var pentrationTestEntity = modelBuilder.Entity<PentrationTest>();

            pentrationTestEntity.HasOne(t => t.System)
                .WithMany(s => s.Tests)
                .HasForeignKey(t => t.SystemId)
                .OnDelete(DeleteBehavior.NoAction);

            pentrationTestEntity.HasOne(p => p.Owner)
                .WithMany(t => t.RequestedTests)
                .HasForeignKey(t => t.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);

            pentrationTestEntity.HasIndex(t => t.Status);

            return modelBuilder;
        }
    }
}
