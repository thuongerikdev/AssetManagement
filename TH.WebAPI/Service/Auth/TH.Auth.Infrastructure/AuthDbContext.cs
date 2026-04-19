using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using TH.Auth.Domain.MFA;
using TH.Auth.Domain.Role;
using TH.Auth.Domain.Token;
using TH.Auth.Domain.User;
using TH.Auth.Infrastructure.SeedData;

namespace TH.Auth.Infrastructure
{
    public class AuthDbContext : DbContext
    {
        public DbSet<AuthUser> authUsers { get; set; }
        public DbSet<AuthProfile> authProfiles { get; set; }
        public DbSet<AuthPermission> authPermissions { get; set; }
        public DbSet<AuthRole> authRoles { get; set; }
        public DbSet<AuthUserRole> authUserRoles { get; set; }
        public DbSet<AuthRolePermission> authRolePermissions { get; set; }
        public DbSet<AuthRefreshToken> authRefreshTokens { get; set; }
        public DbSet<AuthEmailVerification> authEmailVerifications { get; set; }
        public DbSet<AuthPasswordReset> authPasswordResets { get; set; }
        public DbSet<AuthMfaSecret> authMfaSecrets { get; set; }
        public DbSet<AuthAuditLog> authAuditLogs { get; set; }
        public DbSet<AuthUserSession> authUserSessions { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ====== AUTH CORE CONFIGURATION ======
            modelBuilder.Entity<AuthUser>().HasOne(u => u.profile).WithOne(p => p.user).HasForeignKey<AuthProfile>(p => p.userID);
            modelBuilder.Entity<AuthUser>().HasMany(u => u.userRoles).WithOne(ur => ur.user).HasForeignKey(ur => ur.userID);
            modelBuilder.Entity<AuthUser>().HasMany(u => u.auditLogs).WithOne(al => al.user).HasForeignKey(al => al.userID);
            modelBuilder.Entity<AuthUser>().HasOne(u => u.mfaSecret).WithOne(ms => ms.user).HasForeignKey<AuthMfaSecret>(ms => ms.userID);
            modelBuilder.Entity<AuthUser>().HasMany(u => u.sessions).WithOne(s => s.user).HasForeignKey(s => s.userID);
            modelBuilder.Entity<AuthUser>().HasMany(u => u.emailVerifications).WithOne(ev => ev.user).HasForeignKey(ev => ev.userID);
            modelBuilder.Entity<AuthUser>().HasMany(u => u.passwordResets).WithOne(pr => pr.user).HasForeignKey(pr => pr.userID);
            modelBuilder.Entity<AuthUser>().HasMany(u => u.refreshTokens).WithOne(rt => rt.user).HasForeignKey(rt => rt.userID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AuthUserSession>().HasMany(s => s.refreshTokens).WithOne(rt => rt.session).HasForeignKey(rt => rt.sessionID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AuthRole>().HasMany(r => r.userRoles).WithOne(ur => ur.role).HasForeignKey(ur => ur.roleID);
            modelBuilder.Entity<AuthRole>().HasMany(r => r.rolePermissions).WithOne(rp => rp.role).HasForeignKey(rp => rp.roleID);

            modelBuilder.Entity<AuthUserRole>(b =>
            {
                b.ToTable("AuthUserRole", "auth");
                b.HasKey(x => new { x.userID, x.roleID });
                b.Property(x => x.assignedAt).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<AuthPermission>().HasMany(p => p.rolePermissions).WithOne(rp => rp.permission).HasForeignKey(rp => rp.permissionID);

            // ====== ENTITY CONFIGURATIONS ======
            modelBuilder.Entity<AuthRole>(e =>
            {
                e.HasKey(r => r.roleID);
                e.Property(r => r.roleName).HasMaxLength(100).IsRequired();
                e.Property(r => r.roleDescription).HasMaxLength(255);
                e.HasIndex(r => r.roleName).IsUnique();
            });

            modelBuilder.Entity<AuthUser>(e =>
            {
                e.HasKey(u => u.userID);
                e.HasIndex(u => u.userName).IsUnique();
                e.HasIndex(u => u.email).IsUnique();
                e.Property(u => u.userName).HasMaxLength(100).IsRequired();
                e.Property(u => u.email).HasMaxLength(255).IsRequired();
                e.Property(u => u.phoneNumber).HasMaxLength(32);
                e.Property(u => u.status).HasMaxLength(16);
                // departmentID là string, không có FK vì module Asset tách biệt
                e.Property(u => u.departmentID).HasMaxLength(20);
            });

            // ====== UTC CONVERTERS ======
            var utcDateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );
            var utcNullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v == null ? null : (v.Value.Kind == DateTimeKind.Utc ? v.Value : v.Value.ToUniversalTime()),
                v => v == null ? null : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)
            );

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var prop in entity.GetProperties())
                {
                    if (prop.ClrType == typeof(DateTime))
                        prop.SetValueConverter(utcDateTimeConverter);
                    if (prop.ClrType == typeof(DateTime?))
                        prop.SetValueConverter(utcNullableDateTimeConverter);
                }
            }

            // =========================================================
            // ====== SEED DATA ======
            // =========================================================

            // Seed Roles
            AuthSeedData.SeedRoles(modelBuilder);

            // Seed Users
            AuthSeedData.SeedUsers(modelBuilder);

            // Seed Profiles
            AuthSeedData.SeedProfiles(modelBuilder);

            // Seed User-Role Assignments
            AuthSeedData.SeedUserRoles(modelBuilder);

            // LƯU Ý: Permissions và RolePermissions cần được seed riêng
            // vì cần query permissionID từ permission code
            // Xem RolePermissionMapping.cs để biết cách map

            base.OnModelCreating(modelBuilder);
        }
    }
}