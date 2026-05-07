using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TH.Auth.Domain.Role;
using TH.Auth.Domain.User;
using TH.Auth.Infrastructure.Repository.User;

namespace TH.Auth.Infrastructure.SeedData
{
    // ── Static seed (Roles only) — dùng trong OnModelCreating / migrations ──
    public static class AuthSeedData
    {
        public static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthRole>().HasData(
                new AuthRole { roleID = 1,  roleName = "admin_he_thong",       roleDescription = "Quản trị viên hệ thống",    scope = "staff", isDefault = false },
                new AuthRole { roleID = 2,  roleName = "ke_toan_tscd",         roleDescription = "Kế toán tài sản cố định",   scope = "staff", isDefault = false },
                new AuthRole { roleID = 3,  roleName = "truong_phong_ke_toan", roleDescription = "Trưởng phòng kế toán",      scope = "staff", isDefault = false },
                new AuthRole { roleID = 4,  roleName = "ky_thuat_vien",        roleDescription = "Kỹ thuật viên",             scope = "staff", isDefault = false },
                new AuthRole { roleID = 5,  roleName = "truong_phong_ky_thuat",roleDescription = "Trưởng phòng kỹ thuật",    scope = "staff", isDefault = false },
                new AuthRole { roleID = 6,  roleName = "giam_doc",             roleDescription = "Giám đốc",                  scope = "staff", isDefault = false },
                new AuthRole { roleID = 7,  roleName = "pho_giam_doc",         roleDescription = "Phó giám đốc",              scope = "staff", isDefault = false },
                new AuthRole { roleID = 8,  roleName = "truong_phong_ban",     roleDescription = "Trưởng phòng ban",          scope = "staff", isDefault = false },
                new AuthRole { roleID = 9,  roleName = "nhan_vien",            roleDescription = "Nhân viên",                 scope = "staff", isDefault = true  },
                new AuthRole { roleID = 10, roleName = "nhan_vien_nhan_su",    roleDescription = "Nhân viên nhân sự",         scope = "staff", isDefault = false }
            );
        }
    }

    // ── Runtime seeder — chạy lúc app khởi động, hash password tự động ──
    public static class AuthDataSeeder
    {
        public static async Task SeedAsync(AuthDbContext db, IPasswordHasher hasher, ILogger logger)
        {
            if (await db.authUsers.AnyAsync()) return;

            logger.LogInformation("[Seed] Bắt đầu seed users...");

            var now = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            const string defaultPassword = "Password@123";

            var roles = await db.authRoles.ToDictionaryAsync(r => r.roleName, r => r.roleID);

            // ── ADMIN ──────────────────────────────────────────────────────────
            await AddUserAsync(db, roles, logger, now,
                userName: "admin",
                email: "admin@thtech.vn",
                phone: "0900000000",
                dept: "1",
                password: "Admin@123456",
                roleName: "admin_he_thong",
                hasher: hasher);

            // ── BAN GIÁM ĐỐC ──────────────────────────────────────────────────
            await AddUserAsync(db, roles, logger, now, "pham.quoc.hung",     "pqhung@thtech.vn",       "0901234001", "1", defaultPassword, "giam_doc",              hasher);
            await AddUserAsync(db, roles, logger, now, "tran.thanh.tung",    "tttung@thtech.vn",       "0901234002", "1", defaultPassword, "pho_giam_doc",          hasher);
            await AddUserAsync(db, roles, logger, now, "nguyen.minh.chau",   "nmchau@thtech.vn",       "0901234003", "1", defaultPassword, "pho_giam_doc",          hasher);
            await AddUserAsync(db, roles, logger, now, "le.quang.binh",      "lqbinh@thtech.vn",       "0901234004", "1", defaultPassword, "nhan_vien",             hasher);

            // ── PHÒNG KẾ TOÁN ─────────────────────────────────────────────────
            await AddUserAsync(db, roles, logger, now, "nguyen.thi.huong",   "nthuong.kt@thtech.vn",   "0902234001", "2", defaultPassword, "truong_phong_ke_toan",  hasher);
            await AddUserAsync(db, roles, logger, now, "le.bao.ngoc",        "lbngoc.kt@thtech.vn",    "0902234002", "2", defaultPassword, "ke_toan_tscd",          hasher);
            await AddUserAsync(db, roles, logger, now, "tran.van.hai",       "tvhai.kt@thtech.vn",     "0902234003", "2", defaultPassword, "nhan_vien",             hasher);
            await AddUserAsync(db, roles, logger, now, "pham.thi.mai",       "ptmai.kt@thtech.vn",     "0902234004", "2", defaultPassword, "nhan_vien",             hasher);
            await AddUserAsync(db, roles, logger, now, "vo.xuan.truong",     "vxtruong.kt@thtech.vn",  "0902234005", "2", defaultPassword, "nhan_vien",             hasher);

            // ── PHÒNG NHÂN SỰ ─────────────────────────────────────────────────
            await AddUserAsync(db, roles, logger, now, "hoang.thu.ha",       "htha.ns@thtech.vn",      "0903234001", "3", defaultPassword, "truong_phong_ban",      hasher);
            await AddUserAsync(db, roles, logger, now, "bui.thi.linh",       "btlinh.ns@thtech.vn",    "0903234002", "3", defaultPassword, "nhan_vien_nhan_su",     hasher);

            // ── PHÒNG KỸ THUẬT ────────────────────────────────────────────────
            await AddUserAsync(db, roles, logger, now, "do.van.tai",         "dvtai.tech@thtech.vn",   "0904234001", "4", defaultPassword, "truong_phong_ky_thuat", hasher);
            await AddUserAsync(db, roles, logger, now, "nguyen.cong.minh",   "ncminh.tech@thtech.vn",  "0904234002", "4", defaultPassword, "ky_thuat_vien",         hasher);
            await AddUserAsync(db, roles, logger, now, "ha.thi.loan",        "htloan.tech@thtech.vn",  "0904234003", "4", defaultPassword, "ky_thuat_vien",         hasher);

            // ── PHÒNG SẢN PHẨM ────────────────────────────────────────────────
            await AddUserAsync(db, roles, logger, now, "ly.minh.quan",       "lmquan.prod@thtech.vn",  "0905234001", "5", defaultPassword, "truong_phong_ban",      hasher);
            await AddUserAsync(db, roles, logger, now, "dinh.thi.bich.ngoc", "dtbngoc.prod@thtech.vn", "0905234002", "5", defaultPassword, "nhan_vien",             hasher);
            await AddUserAsync(db, roles, logger, now, "phan.huu.phuoc",     "phphuoc.prod@thtech.vn", "0905234003", "5", defaultPassword, "nhan_vien",             hasher);
            await AddUserAsync(db, roles, logger, now, "vu.thi.thuy.dung",   "vttdung.prod@thtech.vn", "0905234004", "5", defaultPassword, "nhan_vien",             hasher);
            await AddUserAsync(db, roles, logger, now, "cao.ngoc.anh",       "cnanh.prod@thtech.vn",   "0905234005", "5", defaultPassword, "nhan_vien",             hasher);

            // ── PHÒNG PHÁT TRIỂN PHẦN MỀM ─────────────────────────────────────
            await AddUserAsync(db, roles, logger, now, "dang.huu.thanh",     "dhthanh.dev@thtech.vn",  "0906234001", "6", defaultPassword, "truong_phong_ban",      hasher);
            await AddUserAsync(db, roles, logger, now, "nguyen.van.dung",    "nvdung.dev@thtech.vn",   "0906234002", "6", defaultPassword, "nhan_vien",             hasher);
            await AddUserAsync(db, roles, logger, now, "tran.thi.kim.oanh",  "ttkoanh.dev@thtech.vn",  "0906234003", "6", defaultPassword, "nhan_vien",             hasher);
            await AddUserAsync(db, roles, logger, now, "le.hoang.nam",       "lhnam.dev@thtech.vn",    "0906234004", "6", defaultPassword, "nhan_vien",             hasher);
            await AddUserAsync(db, roles, logger, now, "pham.tuan.anh",      "ptanh.dev@thtech.vn",    "0906234005", "6", defaultPassword, "nhan_vien",             hasher);
            await AddUserAsync(db, roles, logger, now, "vu.duc.manh",        "vdmanh.dev@thtech.vn",   "0906234006", "6", defaultPassword, "nhan_vien",             hasher);
            await AddUserAsync(db, roles, logger, now, "hoang.thi.hong",     "hthong.dev@thtech.vn",   "0906234007", "6", defaultPassword, "nhan_vien",             hasher);
            await AddUserAsync(db, roles, logger, now, "bui.thanh.phong",    "btphong.dev@thtech.vn",  "0906234008", "6", defaultPassword, "nhan_vien",             hasher);
            await AddUserAsync(db, roles, logger, now, "do.thanh.dat",       "dtdat.dev@thtech.vn",    "0906234009", "6", defaultPassword, "nhan_vien",             hasher);
            await AddUserAsync(db, roles, logger, now, "luu.thi.bao.chau",   "ltbchau.dev@thtech.vn",  "0906234010", "6", defaultPassword, "nhan_vien",             hasher);

            // ── PHÒNG QUẢN LÝ DỰ ÁN ──────────────────────────────────────────
            await AddUserAsync(db, roles, logger, now, "vu.thanh.long",      "vtlong.pmo@thtech.vn",   "0907234001", "7", defaultPassword, "truong_phong_ban",      hasher);
            await AddUserAsync(db, roles, logger, now, "nguyen.ba.kien",     "nbkien.pmo@thtech.vn",   "0907234002", "7", defaultPassword, "nhan_vien",             hasher);
            await AddUserAsync(db, roles, logger, now, "tran.thi.phuong",    "ttphuong.pmo@thtech.vn", "0907234003", "7", defaultPassword, "nhan_vien",             hasher);

            // ── PHÒNG THIẾT KẾ ────────────────────────────────────────────────
            await AddUserAsync(db, roles, logger, now, "phan.ngoc.yen",      "pnyen.design@thtech.vn", "0908234001", "8", defaultPassword, "truong_phong_ban",      hasher);
            await AddUserAsync(db, roles, logger, now, "cao.thi.bich",       "ctbich.design@thtech.vn","0908234002", "8", defaultPassword, "nhan_vien",             hasher);
            await AddUserAsync(db, roles, logger, now, "truong.van.khai",    "tvkhai.design@thtech.vn","0908234003", "8", defaultPassword, "nhan_vien",             hasher);

            logger.LogInformation("[Seed] Hoàn thành seed users.");
        }

        private static async Task AddUserAsync(
            AuthDbContext db,
            Dictionary<string, int> roles,
            ILogger logger,
            DateTime now,
            string userName, string email, string phone,
            string dept, string password, string roleName,
            IPasswordHasher hasher)
        {
            var user = new AuthUser
            {
                userName        = userName,
                email           = email,
                phoneNumber     = phone,
                departmentID    = dept,
                passwordHash    = hasher.Hash(password),
                isEmailVerified = true,
                status          = "Active",
                tokenVersion    = 1,
                scope           = "staff",
                createdAt       = now,
                updatedAt       = now
            };

            await db.authUsers.AddAsync(user);
            await db.SaveChangesAsync();

            if (roles.TryGetValue(roleName, out var roleId))
            {
                db.authUserRoles.Add(new AuthUserRole
                {
                    userID     = user.userID,
                    roleID     = roleId,
                    assignedAt = now
                });
                await db.SaveChangesAsync();
            }
            else
            {
                logger.LogWarning("[Seed] Role '{Role}' không tìm thấy cho user '{User}'", roleName, userName);
            }
        }
    }
}
