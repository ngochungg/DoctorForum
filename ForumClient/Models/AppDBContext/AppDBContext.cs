using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumClient.Models.AppDBContext
{
    public class AppDBContext :DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<UserModel> User { get; set; }
        public DbSet<RoleModel> Role { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //UserTable
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserName)
                     .HasMaxLength(255)
                     .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e=>e.CreatedAt)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.RoleId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<UserModel>().HasData(
                new UserModel { Id = 1,Name = "Admin", UserName = "Admin", Password = "E10ADC3949BA59ABBE56E057F20F883E",RoleId = "1", Look = 1 });

            //Role table
            modelBuilder.Entity<RoleModel>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleInfo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<RoleModel>().HasData(
                new RoleModel {Id = 1,RoleInfo = "Admin"},
                new RoleModel { Id = 2, RoleInfo = "Docter" },
                new RoleModel { Id = 3, RoleInfo = "Customer" });
        }
    }
}
