using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Online_Shop.Models;
using Online_Shop.Models.Const;
using System;

namespace Online_Shop.Data

{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(
                b => {
                    b.Property(p => p.LastName).IsRequired().HasMaxLength(ValidConst.MaxLenghtNames);
                });

            base.OnModelCreating(builder);

            var UserId = Guid.NewGuid().ToString();
            this.SeedUsers(builder, UserId);

            var RoleId = Guid.NewGuid().ToString();
            this.SeedRoles(builder, RoleId);

            this.SeedUserRoles(builder, RoleId,UserId) ;

        }

        private void SeedUsers(ModelBuilder builder,string UserId)
        {
            var user = new ApplicationUser()
            {
                Id = UserId,
                UserName = "Admin",
                LastName="Admin",
                Email = "admin@gmail.com",
                LockoutEnabled = false,
                Address="",
                PhoneNumber=""
            };

            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            passwordHasher.HashPassword(user, "Admin*123");

            builder.Entity<ApplicationUser>().HasData(user);
        }

        private void SeedRoles(ModelBuilder builder, string RoleId)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = RoleId, Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = "user", ConcurrencyStamp = "2", NormalizedName = "Human Resource" }
                );
        }

        private void SeedUserRoles(ModelBuilder builder, String roleId ,string userId)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId= roleId, UserId= userId }
                );
        }
    }
}
