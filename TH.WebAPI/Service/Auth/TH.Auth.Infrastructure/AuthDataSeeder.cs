using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TH.Auth.Domain.Role;
using TH.Auth.Domain.User;
using TH.Constant;

namespace TH.Auth.Infrastructure.SeedData
{
    public static class AuthSeedData
    {
        private static readonly List<string> _permKeys =
            PermissionConstants.Permissions.Keys.OrderBy(k => k).ToList();

        private static int Pid(string key)
        {
            var idx = _permKeys.IndexOf(key);
            if (idx < 0) throw new Exception($"Permission key '{key}' not found in PermissionConstants");
            return idx + 1;
        }

        private const string DEFAULT_PASSWORD_HASH = "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e";

        // ============================================================
        // ROLES – 5 role
        // ============================================================
        public static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthRole>().HasData(
                new AuthRole { roleID = 1, roleName = "admin", roleDescription = "Quản trị viên hệ thống", scope = "staff", isDefault = false },
                new AuthRole { roleID = 2, roleName = "ke_toan", roleDescription = "Kế toán", scope = "staff", isDefault = false },
                new AuthRole { roleID = 3, roleName = "giam_doc", roleDescription = "Giám đốc", scope = "staff", isDefault = false },
                new AuthRole { roleID = 4, roleName = "truong_phong", roleDescription = "Trưởng phòng", scope = "staff", isDefault = false },
                new AuthRole { roleID = 5, roleName = "nhan_vien", roleDescription = "Nhân viên", scope = "staff", isDefault = true }
            );
        }

        // ============================================================
        // USERS
        // ============================================================
        public static void SeedUsers(ModelBuilder modelBuilder)
        {
            var now = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var users = new List<AuthUser>
            {
                // ===== ADMIN HỆ THỐNG =====
                new AuthUser { userID = 99, userName = "admin",               email = "admin@thtech.vn",          phoneNumber = "0999999999", departmentID = "4", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },

                // ===== BAN GIÁM ĐỐC (Dept 1) =====
                new AuthUser { userID = 1,  userName = "pham.quoc.hung",      email = "pqhung@thtech.vn",         phoneNumber = "0901234001", departmentID = "1", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 2,  userName = "tran.thanh.tung",     email = "tttung@thtech.vn",         phoneNumber = "0901234002", departmentID = "1", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 3,  userName = "nguyen.minh.chau",    email = "nmchau@thtech.vn",         phoneNumber = "0901234003", departmentID = "1", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 4,  userName = "le.quang.binh",       email = "lqbinh@thtech.vn",         phoneNumber = "0901234004", departmentID = "1", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },

                // ===== PHÒNG KẾ TOÁN (Dept 2) =====
                new AuthUser { userID = 5,  userName = "nguyen.thi.huong",    email = "nthuong.kt@thtech.vn",     phoneNumber = "0902234001", departmentID = "2", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 6,  userName = "le.bao.ngoc",         email = "lbngoc.kt@thtech.vn",      phoneNumber = "0902234002", departmentID = "2", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 7,  userName = "tran.van.hai",        email = "tvhai.kt@thtech.vn",       phoneNumber = "0902234003", departmentID = "2", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 8,  userName = "pham.thi.mai",        email = "ptmai.kt@thtech.vn",       phoneNumber = "0902234004", departmentID = "2", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 9,  userName = "vo.xuan.truong",      email = "vxtruong.kt@thtech.vn",    phoneNumber = "0902234005", departmentID = "2", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },

                // ===== PHÒNG NHÂN SỰ (Dept 3) =====
                new AuthUser { userID = 10, userName = "hoang.thu.ha",        email = "htha.ns@thtech.vn",        phoneNumber = "0903234001", departmentID = "3", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 11, userName = "bui.thi.linh",        email = "btlinh.ns@thtech.vn",      phoneNumber = "0903234002", departmentID = "3", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },

                // ===== PHÒNG KỸ THUẬT (Dept 4) =====
                new AuthUser { userID = 12, userName = "do.van.tai",          email = "dvtai.tech@thtech.vn",     phoneNumber = "0904234001", departmentID = "4", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 13, userName = "nguyen.cong.minh",    email = "ncminh.tech@thtech.vn",    phoneNumber = "0904234002", departmentID = "4", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 14, userName = "ha.thi.loan",         email = "htloan.tech@thtech.vn",    phoneNumber = "0904234003", departmentID = "4", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },

                // ===== PHÒNG SẢN PHẨM (Dept 5) =====
                new AuthUser { userID = 15, userName = "ly.minh.quan",        email = "lmquan.prod@thtech.vn",    phoneNumber = "0905234001", departmentID = "5", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 16, userName = "dinh.thi.bich.ngoc",  email = "dtbngoc.prod@thtech.vn",   phoneNumber = "0905234002", departmentID = "5", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 17, userName = "phan.huu.phuoc",      email = "phphuoc.prod@thtech.vn",   phoneNumber = "0905234003", departmentID = "5", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 18, userName = "vu.thi.thuy.dung",    email = "vttdung.prod@thtech.vn",   phoneNumber = "0905234004", departmentID = "5", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 19, userName = "cao.ngoc.anh",        email = "cnanh.prod@thtech.vn",     phoneNumber = "0905234005", departmentID = "5", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },

                // ===== PHÒNG PHÁT TRIỂN PHẦN MỀM (Dept 6) =====
                new AuthUser { userID = 20, userName = "dang.huu.thanh",      email = "dhthanh.dev@thtech.vn",    phoneNumber = "0906234001", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 21, userName = "nguyen.van.dung",     email = "nvdung.dev@thtech.vn",     phoneNumber = "0906234002", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 22, userName = "tran.thi.kim.oanh",   email = "ttkoanh.dev@thtech.vn",    phoneNumber = "0906234003", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 23, userName = "le.hoang.nam",        email = "lhnam.dev@thtech.vn",      phoneNumber = "0906234004", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 24, userName = "pham.tuan.anh",       email = "ptanh.dev@thtech.vn",      phoneNumber = "0906234005", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 25, userName = "vu.duc.manh",         email = "vdmanh.dev@thtech.vn",     phoneNumber = "0906234006", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 26, userName = "hoang.thi.hong",      email = "hthong.dev@thtech.vn",     phoneNumber = "0906234007", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 27, userName = "bui.thanh.phong",     email = "btphong.dev@thtech.vn",    phoneNumber = "0906234008", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 28, userName = "do.thanh.dat",        email = "dtdat.dev@thtech.vn",      phoneNumber = "0906234009", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 29, userName = "luu.thi.bao.chau",    email = "ltbchau.dev@thtech.vn",    phoneNumber = "0906234010", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },

                // ===== PHÒNG QUẢN LÝ DỰ ÁN (Dept 7) =====
                new AuthUser { userID = 30, userName = "vu.thanh.long",       email = "vtlong.pmo@thtech.vn",     phoneNumber = "0907234001", departmentID = "7", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 31, userName = "nguyen.ba.kien",      email = "nbkien.pmo@thtech.vn",     phoneNumber = "0907234002", departmentID = "7", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 32, userName = "tran.thi.phuong",     email = "ttphuong.pmo@thtech.vn",   phoneNumber = "0907234003", departmentID = "7", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },

                // ===== PHÒNG THIẾT KẾ UI/UX (Dept 8) =====
                new AuthUser { userID = 33, userName = "phan.ngoc.yen",       email = "pnyen.design@thtech.vn",   phoneNumber = "0908234001", departmentID = "8", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 34, userName = "cao.thi.bich",        email = "ctbich.design@thtech.vn",  phoneNumber = "0908234002", departmentID = "8", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                new AuthUser { userID = 35, userName = "truong.van.khai",     email = "tvkhai.design@thtech.vn",  phoneNumber = "0908234003", departmentID = "8", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
            };

            modelBuilder.Entity<AuthUser>().HasData(users);
        }

        // ============================================================
        // PROFILES
        // ============================================================
        public static void SeedProfiles(ModelBuilder modelBuilder)
        {
            var profiles = new List<AuthProfile>
            {
                new AuthProfile { profileID = 99, userID = 99, firstName = "System",              lastName = "Admin",   gender = "Nam" },

                new AuthProfile { profileID = 1,  userID = 1,  firstName = "Phạm Quốc",           lastName = "Hùng",    gender = "Nam" },
                new AuthProfile { profileID = 2,  userID = 2,  firstName = "Trần Thanh",           lastName = "Tùng",    gender = "Nam" },
                new AuthProfile { profileID = 3,  userID = 3,  firstName = "Nguyễn Thị Minh",     lastName = "Châu",    gender = "Nữ"  },
                new AuthProfile { profileID = 4,  userID = 4,  firstName = "Lê Quang",             lastName = "Bình",    gender = "Nam" },

                new AuthProfile { profileID = 5,  userID = 5,  firstName = "Nguyễn Thị",          lastName = "Hương",   gender = "Nữ"  },
                new AuthProfile { profileID = 6,  userID = 6,  firstName = "Lê Bảo",              lastName = "Ngọc",    gender = "Nữ"  },
                new AuthProfile { profileID = 7,  userID = 7,  firstName = "Trần Văn",            lastName = "Hải",     gender = "Nam" },
                new AuthProfile { profileID = 8,  userID = 8,  firstName = "Phạm Thị",            lastName = "Mai",     gender = "Nữ"  },
                new AuthProfile { profileID = 9,  userID = 9,  firstName = "Võ Xuân",             lastName = "Trường",  gender = "Nam" },

                new AuthProfile { profileID = 10, userID = 10, firstName = "Hoàng Thu",           lastName = "Hà",      gender = "Nữ"  },
                new AuthProfile { profileID = 11, userID = 11, firstName = "Bùi Thị",             lastName = "Linh",    gender = "Nữ"  },

                new AuthProfile { profileID = 12, userID = 12, firstName = "Đỗ Văn",              lastName = "Tài",     gender = "Nam" },
                new AuthProfile { profileID = 13, userID = 13, firstName = "Nguyễn Công",         lastName = "Minh",    gender = "Nam" },
                new AuthProfile { profileID = 14, userID = 14, firstName = "Hà Thị",              lastName = "Loan",    gender = "Nữ"  },

                new AuthProfile { profileID = 15, userID = 15, firstName = "Lý Minh",             lastName = "Quân",    gender = "Nam" },
                new AuthProfile { profileID = 16, userID = 16, firstName = "Đinh Thị Bích",       lastName = "Ngọc",    gender = "Nữ"  },
                new AuthProfile { profileID = 17, userID = 17, firstName = "Phan Hữu",            lastName = "Phước",   gender = "Nam" },
                new AuthProfile { profileID = 18, userID = 18, firstName = "Vũ Thị Thùy",         lastName = "Dung",    gender = "Nữ"  },
                new AuthProfile { profileID = 19, userID = 19, firstName = "Cao Ngọc",            lastName = "Anh",     gender = "Nữ"  },

                new AuthProfile { profileID = 20, userID = 20, firstName = "Đặng Hữu",            lastName = "Thành",   gender = "Nam" },
                new AuthProfile { profileID = 21, userID = 21, firstName = "Nguyễn Văn",          lastName = "Dũng",    gender = "Nam" },
                new AuthProfile { profileID = 22, userID = 22, firstName = "Trần Thị Kim",        lastName = "Oanh",    gender = "Nữ"  },
                new AuthProfile { profileID = 23, userID = 23, firstName = "Lê Hoàng",            lastName = "Nam",     gender = "Nam" },
                new AuthProfile { profileID = 24, userID = 24, firstName = "Phạm Tuấn",           lastName = "Anh",     gender = "Nam" },
                new AuthProfile { profileID = 25, userID = 25, firstName = "Vũ Đức",              lastName = "Mạnh",    gender = "Nam" },
                new AuthProfile { profileID = 26, userID = 26, firstName = "Hoàng Thị",           lastName = "Hồng",    gender = "Nữ"  },
                new AuthProfile { profileID = 27, userID = 27, firstName = "Bùi Thanh",           lastName = "Phong",   gender = "Nam" },
                new AuthProfile { profileID = 28, userID = 28, firstName = "Đỗ Thành",            lastName = "Đạt",     gender = "Nam" },
                new AuthProfile { profileID = 29, userID = 29, firstName = "Lưu Thị Bảo",         lastName = "Châu",    gender = "Nữ"  },

                new AuthProfile { profileID = 30, userID = 30, firstName = "Vũ Thành",            lastName = "Long",    gender = "Nam" },
                new AuthProfile { profileID = 31, userID = 31, firstName = "Nguyễn Bá",           lastName = "Kiên",    gender = "Nam" },
                new AuthProfile { profileID = 32, userID = 32, firstName = "Trần Thị",            lastName = "Phương",  gender = "Nữ"  },

                new AuthProfile { profileID = 33, userID = 33, firstName = "Phan Ngọc",           lastName = "Yến",     gender = "Nữ"  },
                new AuthProfile { profileID = 34, userID = 34, firstName = "Cao Thị",             lastName = "Bích",    gender = "Nữ"  },
                new AuthProfile { profileID = 35, userID = 35, firstName = "Trương Văn",          lastName = "Khải",    gender = "Nam" },
            };

            modelBuilder.Entity<AuthProfile>().HasData(profiles);
        }

        // ============================================================
        // USER-ROLES
        // roleID: 1=admin | 2=ke_toan | 3=giam_doc | 4=truong_phong | 5=nhan_vien
        // ============================================================
        public static void SeedUserRoles(ModelBuilder modelBuilder)
        {
            var now = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var userRoles = new List<AuthUserRole>
            {
                // ===== ADMIN =====
                new AuthUserRole { userID = 99, roleID = 1, assignedAt = now },

                // ===== BAN GIÁM ĐỐC =====
                new AuthUserRole { userID = 1,  roleID = 3, assignedAt = now }, // Giám đốc
                new AuthUserRole { userID = 2,  roleID = 3, assignedAt = now }, // Phó giám đốc
                new AuthUserRole { userID = 3,  roleID = 3, assignedAt = now }, // Phó giám đốc
                new AuthUserRole { userID = 4,  roleID = 5, assignedAt = now }, // Thư ký → nhân viên

                // ===== PHÒNG KẾ TOÁN =====
                new AuthUserRole { userID = 5,  roleID = 4, assignedAt = now }, // Trưởng phòng
                new AuthUserRole { userID = 5,  roleID = 2, assignedAt = now }, // Kiêm kế toán TSCĐ
                new AuthUserRole { userID = 6,  roleID = 2, assignedAt = now },
                new AuthUserRole { userID = 7,  roleID = 2, assignedAt = now },
                new AuthUserRole { userID = 8,  roleID = 2, assignedAt = now },
                new AuthUserRole { userID = 9,  roleID = 2, assignedAt = now },

                // ===== PHÒNG NHÂN SỰ =====
                new AuthUserRole { userID = 10, roleID = 4, assignedAt = now }, // Trưởng phòng
                new AuthUserRole { userID = 11, roleID = 5, assignedAt = now },

                // ===== PHÒNG KỸ THUẬT =====
                new AuthUserRole { userID = 12, roleID = 4, assignedAt = now }, // Trưởng phòng
                new AuthUserRole { userID = 13, roleID = 5, assignedAt = now },
                new AuthUserRole { userID = 14, roleID = 5, assignedAt = now },

                // ===== PHÒNG SẢN PHẨM =====
                new AuthUserRole { userID = 15, roleID = 4, assignedAt = now }, // Trưởng phòng
                new AuthUserRole { userID = 16, roleID = 5, assignedAt = now },
                new AuthUserRole { userID = 17, roleID = 5, assignedAt = now },
                new AuthUserRole { userID = 18, roleID = 5, assignedAt = now },
                new AuthUserRole { userID = 19, roleID = 5, assignedAt = now },

                // ===== PHÒNG PHÁT TRIỂN PHẦN MỀM =====
                new AuthUserRole { userID = 20, roleID = 4, assignedAt = now }, // Trưởng phòng
                new AuthUserRole { userID = 21, roleID = 5, assignedAt = now },
                new AuthUserRole { userID = 22, roleID = 5, assignedAt = now },
                new AuthUserRole { userID = 23, roleID = 5, assignedAt = now },
                new AuthUserRole { userID = 24, roleID = 5, assignedAt = now },
                new AuthUserRole { userID = 25, roleID = 5, assignedAt = now },
                new AuthUserRole { userID = 26, roleID = 5, assignedAt = now },
                new AuthUserRole { userID = 27, roleID = 5, assignedAt = now },
                new AuthUserRole { userID = 28, roleID = 5, assignedAt = now },
                new AuthUserRole { userID = 29, roleID = 5, assignedAt = now },

                // ===== PHÒNG QUẢN LÝ DỰ ÁN =====
                new AuthUserRole { userID = 30, roleID = 4, assignedAt = now }, // Trưởng phòng
                new AuthUserRole { userID = 31, roleID = 5, assignedAt = now },
                new AuthUserRole { userID = 32, roleID = 5, assignedAt = now },

                // ===== PHÒNG THIẾT KẾ UI/UX =====
                new AuthUserRole { userID = 33, roleID = 4, assignedAt = now }, // Trưởng phòng
                new AuthUserRole { userID = 34, roleID = 5, assignedAt = now },
                new AuthUserRole { userID = 35, roleID = 5, assignedAt = now },
            };

            modelBuilder.Entity<AuthUserRole>().HasData(userRoles);
        }

        // ============================================================
        // PERMISSIONS – tự động từ PermissionConstants
        // ============================================================
        public static void SeedPermissions(ModelBuilder modelBuilder)
        {
            var permissions = _permKeys
                .Select((key, idx) => new AuthPermission
                {
                    permissionID = idx + 1,
                    permissionName = key,
                    permissionDescription = PermissionConstants.Permissions[key],
                    code = PermissionConstants.Permissions[key],
                    scope = "staff"
                })
                .ToList();

            modelBuilder.Entity<AuthPermission>().HasData(permissions);
        }

        // ============================================================
        // ROLE-PERMISSIONS
        // ============================================================
        public static void SeedRolePermissions(ModelBuilder modelBuilder)
        {
            // Quyền chung – tất cả 5 role đều có
            // Đã bỏ toàn bộ Auth* (Login/Register) và Account* (MFA/password/forgot)
            // vì các key này không còn tồn tại trong PermissionConstants
            var commonPerms = new[]
            {
                "UserGetMe", "UserUpdateProfile", "UserUpdateUsername",
                "CauHinhHeThongGet",
            };

            // ── Role 1: Admin – toàn quyền ──────────────────────────────────────
            var adminPerms = _permKeys.ToArray();

            // ── Role 2: Kế toán ─────────────────────────────────────────────────
            var keToantscPerms = commonPerms.Concat(new[]
            {
                "AuthCreateUser",
                "UserGetAll", "UserGetAllSlim", "UserGetSlimById", "UserGetByDepartmentId",
                "RoleGetAll", "RoleGetAllScopeUser", "RoleGetByUserId",
                "PermissionGetAll", "PermissionGetById", "PermissionGetByUserId", "PermissionGetByRoleId",
                "TaiSanGetAll", "TaiSanGetById", "TaiSanGetMine", "TaiSanGenerateCode",
                "TaiSanCreate", "TaiSanConfirm", "TaiSanReject", "TaiSanUpdate",
                "DanhMucTaiSanGetAll", "DanhMucTaiSanGetById", "DanhMucTaiSanCreate", "DanhMucTaiSanUpdate",
                "PhongBanGetAll", "PhongBanGetById",
                "TaiKhoanKeToanGetAll", "TaiKhoanKeToanGetById", "TaiKhoanKeToanCreate", "TaiKhoanKeToanUpdate",
                "LichSuKhauHaoGetAll", "LichSuKhauHaoGetById", "LichSuKhauHaoGetByAsset",
                "LichSuKhauHaoCreate", "LichSuKhauHaoCreateBulk", "LichSuKhauHaoUpdate", "LichSuKhauHaoDelete",
                "ChungTuGetAll", "ChungTuGetById", "ChungTuGetByAsset", "ChungTuGenerateCode",
                "ChungTuCreate", "ChungTuUpdate", "ChungTuDelete",
                "SoCaiGetTomTat", "SoCaiGetChiTiet",
                "TaiSanDinhKemGetByAsset", "TaiSanDinhKemUpload",
                "ThanhLyTaiSanGetAll", "ThanhLyTaiSanGetById", "ThanhLyTaiSanGetByAsset",
                "ThanhLyTaiSanCreate", "ThanhLyTaiSanUpdate",
                "BaoTriTaiSanGetAll", "BaoTriTaiSanGetById", "BaoTriTaiSanGetByAsset",
                "DieuChuyenTaiSanGetAll", "DieuChuyenTaiSanGetById", "DieuChuyenTaiSanGetByAsset",
                "DieuChuyenTaiSanCreate",
            }).Distinct().ToArray();

            // ── Role 3: Giám đốc – xem toàn bộ + duyệt tài sản ────────────────
            var giamdocPerms = commonPerms.Concat(new[]
            {
                "UserGetAll", "UserGetAllSlim", "UserGetSlimById", "UserGetByDepartmentId",
                "RoleGetAll", "RoleGetAllScopeUser", "RoleGetByUserId",
                "PermissionGetAll", "PermissionGetById", "PermissionGetByUserId", "PermissionGetByRoleId",
                "TaiSanGetAll", "TaiSanGetById", "TaiSanGetMine", "TaiSanConfirm", "TaiSanReject",
                "DanhMucTaiSanGetAll", "DanhMucTaiSanGetById",
                "PhongBanGetAll", "PhongBanGetById",
                "TaiKhoanKeToanGetAll", "TaiKhoanKeToanGetById",
                "LichSuKhauHaoGetAll", "LichSuKhauHaoGetById", "LichSuKhauHaoGetByAsset",
                "ChungTuGetAll", "ChungTuGetById", "ChungTuGetByAsset",
                "SoCaiGetTomTat", "SoCaiGetChiTiet",
                "TaiSanDinhKemGetByAsset",
                "ThanhLyTaiSanGetAll", "ThanhLyTaiSanGetById", "ThanhLyTaiSanGetByAsset",
                "BaoTriTaiSanGetAll", "BaoTriTaiSanGetById", "BaoTriTaiSanGetByAsset",
                "DieuChuyenTaiSanGetAll", "DieuChuyenTaiSanGetById", "DieuChuyenTaiSanGetByAsset",
                "UserSessionGetAll", "UserSessionGetByUserId", "UserSessionGetBySessionId",
                "AuditLogGetAll", "AuditLogGetById", "AuditLogGetByUserId",
            }).Distinct().ToArray();

            // ── Role 4: Trưởng phòng ────────────────────────────────────────────
            var truongPhongPerms = commonPerms.Concat(new[]
            {
                "UserGetAllSlim", "UserGetSlimById", "UserGetByDepartmentId",
                "TaiSanGetAll", "TaiSanGetById", "TaiSanGetMine", "TaiSanCreate",
                "TaiSanConfirm", "TaiSanReject",
                "DanhMucTaiSanGetAll", "DanhMucTaiSanGetById",
                "PhongBanGetAll", "PhongBanGetById",
                "TaiSanDinhKemGetByAsset", "TaiSanDinhKemUpload",
                "LichSuKhauHaoGetAll", "LichSuKhauHaoGetById", "LichSuKhauHaoGetByAsset",
                "ChungTuGetAll", "ChungTuGetById", "ChungTuGetByAsset",
                "ThanhLyTaiSanGetAll", "ThanhLyTaiSanGetById", "ThanhLyTaiSanGetByAsset",
                "BaoTriTaiSanGetAll", "BaoTriTaiSanGetById", "BaoTriTaiSanGetByAsset",
                "BaoTriTaiSanCreate", "BaoTriTaiSanUpdate",
                "DieuChuyenTaiSanGetAll", "DieuChuyenTaiSanGetById", "DieuChuyenTaiSanGetByAsset",
                "DieuChuyenTaiSanCreate",
            }).Distinct().ToArray();

            // ── Role 5: Nhân viên ───────────────────────────────────────────────
            var nhanVienPerms = commonPerms.Concat(new[]
            {
                "TaiSanGetMine", "TaiSanGetById",
                "DanhMucTaiSanGetAll", "DanhMucTaiSanGetById",
                "PhongBanGetAll", "PhongBanGetById",
                "TaiSanDinhKemGetByAsset",
                "LichSuKhauHaoGetById", "LichSuKhauHaoGetByAsset",
                "ChungTuGetById", "ChungTuGetByAsset",
                "BaoTriTaiSanGetById", "BaoTriTaiSanGetByAsset", "BaoTriTaiSanCreate",
                "DieuChuyenTaiSanGetById", "DieuChuyenTaiSanGetByAsset", "DieuChuyenTaiSanCreate",
            }).Distinct().ToArray();

            var rolePerms = new Dictionary<int, string[]>
            {
                [1] = adminPerms,
                [2] = keToantscPerms,
                [3] = giamdocPerms,
                [4] = truongPhongPerms,
                [5] = nhanVienPerms,
            };

            var entries = new List<AuthRolePermission>();

            foreach (var (roleId, keys) in rolePerms.OrderBy(x => x.Key))
            {
                foreach (var key in keys)
                {
                    int permId = Pid(key);
                    entries.Add(new AuthRolePermission
                    {
                        rolePermissionID = (roleId * 10000) + permId,
                        roleID = roleId,
                        permissionID = permId,
                    });
                }
            }

            modelBuilder.Entity<AuthRolePermission>().HasData(entries);
        }
    }
}