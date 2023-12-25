using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

//namespace WebApi.Data.Configuration
//{
    //public class ApplicationUserRolesConfig : IEntityTypeConfiguration<ApplicationUserRoles>
    //{
    //    public void Configure(EntityTypeBuilder<ApplicationUserRoles> builder)
    //    {
    //        builder.HasKey(x => new {x.RoleId, x.UserId});
    //        builder.HasOne(x => x.User)
    //            .WithMany()
    //            .HasForeignKey(x => x.UserId);

    //        builder.HasOne(x => x.Role)
    //            .WithMany()
    //            .HasForeignKey(x => x.RoleId);
    //    }
    //}
//}
