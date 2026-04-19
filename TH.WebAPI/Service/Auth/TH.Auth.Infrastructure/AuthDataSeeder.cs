using Microsoft.EntityFrameworkCore;
using System;
using TH.Auth.Domain.Role;
using TH.Auth.Domain.User;

namespace TH.Auth.Infrastructure.SeedData
{
    public static class AuthSeedData
    {
        // Mật khẩu mặc định đã hash: "Password123!" 
        // (Trong thực tế, bạn cần hash bằng PasswordHasher của hệ thống)
        private const string DEFAULT_PASSWORD_HASH = "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z";

        public static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthRole>().HasData(
                // Admin hệ thống
                new AuthRole
                {
                    roleID = 1,
                    roleName = "admin_he_thong",
                    roleDescription = "Quản trị viên hệ thống",
                    scope = "staff",
                    isDefault = false
                },

                // Kế toán TSCĐ - Actor chính
                new AuthRole
                {
                    roleID = 2,
                    roleName = "ke_toan_tscd",
                    roleDescription = "Kế toán tài sản cố định",
                    scope = "staff",
                    isDefault = false
                },

                // Trưởng phòng kế toán
                new AuthRole
                {
                    roleID = 3,
                    roleName = "truong_phong_ke_toan",
                    roleDescription = "Trưởng phòng kế toán",
                    scope = "staff",
                    isDefault = false
                },

                // Bộ phận kỹ thuật
                new AuthRole
                {
                    roleID = 4,
                    roleName = "ky_thuat_vien",
                    roleDescription = "Kỹ thuật viên",
                    scope = "staff",
                    isDefault = false
                },

                new AuthRole
                {
                    roleID = 5,
                    roleName = "truong_phong_ky_thuat",
                    roleDescription = "Trưởng phòng kỹ thuật",
                    scope = "staff",
                    isDefault = false
                },

                // Ban giám đốc
                new AuthRole
                {
                    roleID = 6,
                    roleName = "giam_doc",
                    roleDescription = "Giám đốc",
                    scope = "staff",
                    isDefault = false
                },

                new AuthRole
                {
                    roleID = 7,
                    roleName = "pho_giam_doc",
                    roleDescription = "Phó giám đốc",
                    scope = "staff",
                    isDefault = false
                },

                // Trưởng phòng ban (chung)
                new AuthRole
                {
                    roleID = 8,
                    roleName = "truong_phong_ban",
                    roleDescription = "Trưởng phòng ban",
                    scope = "staff",
                    isDefault = false
                },

                // Nhân viên sử dụng tài sản (default)
                new AuthRole
                {
                    roleID = 9,
                    roleName = "nhan_vien",
                    roleDescription = "Nhân viên",
                    scope = "staff",
                    isDefault = true
                },

                // Nhân viên nhân sự
                new AuthRole
                {
                    roleID = 10,
                    roleName = "nhan_vien_nhan_su",
                    roleDescription = "Nhân viên nhân sự",
                    scope = "staff",
                    isDefault = false
                }
            );
        }

        public static void SeedUsers(ModelBuilder modelBuilder)
        {
            var now = DateTime.UtcNow;
            var users = new List<AuthUser>();
            int userId = 1;

            // ===== BAN GIÁM ĐỐC (Dept 1) =====
            users.Add(new AuthUser
            {
                userID = userId++,
                userName = "gd.nguyen",
                email = "gd.nguyen@thtech.vn",
                phoneNumber = "0901000001",
                departmentID = "1",
                passwordHash = DEFAULT_PASSWORD_HASH,
                isEmailVerified = true,
                status = "Active",
                tokenVersion = 1,
                scope = "staff",
                createdAt = now,
                updatedAt = now
            });

            users.Add(new AuthUser
            {
                userID = userId++,
                userName = "pgd.tran",
                email = "pgd.tran@thtech.vn",
                phoneNumber = "0901000002",
                departmentID = "1",
                passwordHash = DEFAULT_PASSWORD_HASH,
                isEmailVerified = true,
                status = "Active",
                tokenVersion = 1,
                scope = "staff",
                createdAt = now,
                updatedAt = now
            });

            users.Add(new AuthUser
            {
                userID = userId++,
                userName = "la.bgd",
                email = "la.bgd@thtech.vn",
                phoneNumber = "0901000003",
                departmentID = "1",
                passwordHash = DEFAULT_PASSWORD_HASH,
                isEmailVerified = true,
                status = "Active",
                tokenVersion = 1,
                scope = "staff",
                createdAt = now,
                updatedAt = now
            });

            // ===== PHÒNG KẾ TOÁN (Dept 2) =====
            // Trưởng phòng
            users.Add(new AuthUser
            {
                userID = userId++,
                userName = "tp.le.kt",
                email = "tp.le.kt@thtech.vn",
                phoneNumber = "0902000001",
                departmentID = "2",
                passwordHash = DEFAULT_PASSWORD_HASH,
                isEmailVerified = true,
                status = "Active",
                tokenVersion = 1,
                scope = "staff",
                createdAt = now,
                updatedAt = now
            });

            // Kế toán TSCĐ
            for (int i = 1; i <= 3; i++)
            {
                users.Add(new AuthUser
                {
                    userID = userId++,
                    userName = $"kt.tscd{i}",
                    email = $"kt.tscd{i}@thtech.vn",
                    phoneNumber = $"090200000{i + 1}",
                    departmentID = "2",
                    passwordHash = DEFAULT_PASSWORD_HASH,
                    isEmailVerified = true,
                    status = "Active",
                    tokenVersion = 1,
                    scope = "staff",
                    createdAt = now,
                    updatedAt = now
                });
            }

            // Kế toán khác
            for (int i = 1; i <= 7; i++)
            {
                users.Add(new AuthUser
                {
                    userID = userId++,
                    userName = $"nv.kt{i}",
                    email = $"nv.kt{i}@thtech.vn",
                    phoneNumber = $"090200001{i}",
                    departmentID = "2",
                    passwordHash = DEFAULT_PASSWORD_HASH,
                    isEmailVerified = true,
                    status = "Active",
                    tokenVersion = 1,
                    scope = "staff",
                    createdAt = now,
                    updatedAt = now
                });
            }

            // ===== PHÒNG NHÂN SỰ (Dept 3) =====
            users.Add(new AuthUser
            {
                userID = userId++,
                userName = "tp.pham.ns",
                email = "tp.pham.ns@thtech.vn",
                phoneNumber = "0903000001",
                departmentID = "3",
                passwordHash = DEFAULT_PASSWORD_HASH,
                isEmailVerified = true,
                status = "Active",
                tokenVersion = 1,
                scope = "staff",
                createdAt = now,
                updatedAt = now
            });

            for (int i = 1; i <= 10; i++)
            {
                users.Add(new AuthUser
                {
                    userID = userId++,
                    userName = $"nv.ns{i}",
                    email = $"nv.ns{i}@thtech.vn",
                    phoneNumber = $"090300000{i + 1}",
                    departmentID = "3",
                    passwordHash = DEFAULT_PASSWORD_HASH,
                    isEmailVerified = true,
                    status = "Active",
                    tokenVersion = 1,
                    scope = "staff",
                    createdAt = now,
                    updatedAt = now
                });
            }

            // ===== PHÒNG KỸ THUẬT (Dept 4) =====
            users.Add(new AuthUser
            {
                userID = userId++,
                userName = "tp.vu.kt",
                email = "tp.vu.kt@thtech.vn",
                phoneNumber = "0904000001",
                departmentID = "4",
                passwordHash = DEFAULT_PASSWORD_HASH,
                isEmailVerified = true,
                status = "Active",
                tokenVersion = 1,
                scope = "staff",
                createdAt = now,
                updatedAt = now
            });

            for (int i = 1; i <= 10; i++)
            {
                users.Add(new AuthUser
                {
                    userID = userId++,
                    userName = $"ktv{i}",
                    email = $"ktv{i}@thtech.vn",
                    phoneNumber = $"090400000{i + 1}",
                    departmentID = "4",
                    passwordHash = DEFAULT_PASSWORD_HASH,
                    isEmailVerified = true,
                    status = "Active",
                    tokenVersion = 1,
                    scope = "staff",
                    createdAt = now,
                    updatedAt = now
                });
            }

            // ===== PHÒNG SẢN PHẨM (Dept 5) =====
            users.Add(new AuthUser
            {
                userID = userId++,
                userName = "tp.hoang.sp",
                email = "tp.hoang.sp@thtech.vn",
                phoneNumber = "0905000001",
                departmentID = "5",
                passwordHash = DEFAULT_PASSWORD_HASH,
                isEmailVerified = true,
                status = "Active",
                tokenVersion = 1,
                scope = "staff",
                createdAt = now,
                updatedAt = now
            });

            for (int i = 1; i <= 10; i++)
            {
                users.Add(new AuthUser
                {
                    userID = userId++,
                    userName = $"pm{i}",
                    email = $"pm{i}@thtech.vn",
                    phoneNumber = $"090500000{i + 1}",
                    departmentID = "5",
                    passwordHash = DEFAULT_PASSWORD_HASH,
                    isEmailVerified = true,
                    status = "Active",
                    tokenVersion = 1,
                    scope = "staff",
                    createdAt = now,
                    updatedAt = now
                });
            }

            // ===== PHÒNG PHÁT TRIỂN PHẦN MỀM (Dept 6) =====
            users.Add(new AuthUser
            {
                userID = userId++,
                userName = "tp.minh.dev",
                email = "tp.minh.dev@thtech.vn",
                phoneNumber = "0906000001",
                departmentID = "6",
                passwordHash = DEFAULT_PASSWORD_HASH,
                isEmailVerified = true,
                status = "Active",
                tokenVersion = 1,
                scope = "staff",
                createdAt = now,
                updatedAt = now
            });

            for (int i = 1; i <= 10; i++)
            {
                users.Add(new AuthUser
                {
                    userID = userId++,
                    userName = $"dev{i}",
                    email = $"dev{i}@thtech.vn",
                    phoneNumber = $"090600000{i + 1}",
                    departmentID = "6",
                    passwordHash = DEFAULT_PASSWORD_HASH,
                    isEmailVerified = true,
                    status = "Active",
                    tokenVersion = 1,
                    scope = "staff",
                    createdAt = now,
                    updatedAt = now
                });
            }

            // ===== PHÒNG QUẢN LÝ DỰ ÁN (Dept 7) =====
            users.Add(new AuthUser
            {
                userID = userId++,
                userName = "tp.quan.pm",
                email = "tp.quan.pm@thtech.vn",
                phoneNumber = "0907000001",
                departmentID = "7",
                passwordHash = DEFAULT_PASSWORD_HASH,
                isEmailVerified = true,
                status = "Active",
                tokenVersion = 1,
                scope = "staff",
                createdAt = now,
                updatedAt = now
            });

            for (int i = 1; i <= 10; i++)
            {
                users.Add(new AuthUser
                {
                    userID = userId++,
                    userName = $"prjm{i}",
                    email = $"prjm{i}@thtech.vn",
                    phoneNumber = $"090700000{i + 1}",
                    departmentID = "7",
                    passwordHash = DEFAULT_PASSWORD_HASH,
                    isEmailVerified = true,
                    status = "Active",
                    tokenVersion = 1,
                    scope = "staff",
                    createdAt = now,
                    updatedAt = now
                });
            }

            // ===== PHÒNG THIẾT KẾ UI/UX (Dept 8) =====
            users.Add(new AuthUser
            {
                userID = userId++,
                userName = "tp.linh.ux",
                email = "tp.linh.ux@thtech.vn",
                phoneNumber = "0908000001",
                departmentID = "8",
                passwordHash = DEFAULT_PASSWORD_HASH,
                isEmailVerified = true,
                status = "Active",
                tokenVersion = 1,
                scope = "staff",
                createdAt = now,
                updatedAt = now
            });

            for (int i = 1; i <= 10; i++)
            {
                users.Add(new AuthUser
                {
                    userID = userId++,
                    userName = $"designer{i}",
                    email = $"designer{i}@thtech.vn",
                    phoneNumber = $"090800000{i + 1}",
                    departmentID = "8",
                    passwordHash = DEFAULT_PASSWORD_HASH,
                    isEmailVerified = true,
                    status = "Active",
                    tokenVersion = 1,
                    scope = "staff",
                    createdAt = now,
                    updatedAt = now
                });
            }

            modelBuilder.Entity<AuthUser>().HasData(users);
        }

        public static void SeedProfiles(ModelBuilder modelBuilder)
        {
            var profiles = new List<AuthProfile>();
            int profileId = 1;

            // BGD
            profiles.Add(new AuthProfile { profileID = profileId++, userID = 1, firstName = "Nguyễn Văn", lastName = "A", gender = "Nam" });
            profiles.Add(new AuthProfile { profileID = profileId++, userID = 2, firstName = "Trần Thị", lastName = "B", gender = "Nữ" });
            profiles.Add(new AuthProfile { profileID = profileId++, userID = 3, firstName = "Lê Văn", lastName = "C", gender = "Nam" });

            // Phòng Kế Toán
            profiles.Add(new AuthProfile { profileID = profileId++, userID = 4, firstName = "Lê Thị", lastName = "D", gender = "Nữ" });
            profiles.Add(new AuthProfile { profileID = profileId++, userID = 5, firstName = "Hoàng Văn", lastName = "E", gender = "Nam" });
            profiles.Add(new AuthProfile { profileID = profileId++, userID = 6, firstName = "Phạm Thị", lastName = "F", gender = "Nữ" });
            profiles.Add(new AuthProfile { profileID = profileId++, userID = 7, firstName = "Vũ Văn", lastName = "G", gender = "Nam" });

            for (int i = 8; i <= 14; i++)
            {
                profiles.Add(new AuthProfile
                {
                    profileID = profileId++,
                    userID = i,
                    firstName = $"Nhân viên",
                    lastName = $"KT{i - 7}",
                    gender = i % 2 == 0 ? "Nam" : "Nữ"
                });
            }

            // Phòng Nhân Sự
            profiles.Add(new AuthProfile { profileID = profileId++, userID = 15, firstName = "Phạm Văn", lastName = "H", gender = "Nam" });
            for (int i = 16; i <= 25; i++)
            {
                profiles.Add(new AuthProfile
                {
                    profileID = profileId++,
                    userID = i,
                    firstName = $"Nhân viên",
                    lastName = $"NS{i - 15}",
                    gender = i % 2 == 0 ? "Nam" : "Nữ"
                });
            }

            // Phòng Kỹ Thuật
            profiles.Add(new AuthProfile { profileID = profileId++, userID = 26, firstName = "Vũ Thị", lastName = "I", gender = "Nữ" });
            for (int i = 27; i <= 36; i++)
            {
                profiles.Add(new AuthProfile
                {
                    profileID = profileId++,
                    userID = i,
                    firstName = $"Kỹ thuật viên",
                    lastName = $"{i - 26}",
                    gender = i % 2 == 0 ? "Nam" : "Nữ"
                });
            }

            // Phòng Sản Phẩm
            profiles.Add(new AuthProfile { profileID = profileId++, userID = 37, firstName = "Hoàng Thị", lastName = "J", gender = "Nữ" });
            for (int i = 38; i <= 47; i++)
            {
                profiles.Add(new AuthProfile
                {
                    profileID = profileId++,
                    userID = i,
                    firstName = $"PM",
                    lastName = $"{i - 37}",
                    gender = i % 2 == 0 ? "Nam" : "Nữ"
                });
            }

            // Phòng Phát Triển
            profiles.Add(new AuthProfile { profileID = profileId++, userID = 48, firstName = "Minh Văn", lastName = "K", gender = "Nam" });
            for (int i = 49; i <= 58; i++)
            {
                profiles.Add(new AuthProfile
                {
                    profileID = profileId++,
                    userID = i,
                    firstName = $"Developer",
                    lastName = $"{i - 48}",
                    gender = i % 2 == 0 ? "Nam" : "Nữ"
                });
            }

            // Phòng Quản Lý Dự Án
            profiles.Add(new AuthProfile { profileID = profileId++, userID = 59, firstName = "Quân Văn", lastName = "L", gender = "Nam" });
            for (int i = 60; i <= 69; i++)
            {
                profiles.Add(new AuthProfile
                {
                    profileID = profileId++,
                    userID = i,
                    firstName = $"Project Manager",
                    lastName = $"{i - 59}",
                    gender = i % 2 == 0 ? "Nam" : "Nữ"
                });
            }

            // Phòng Thiết Kế
            profiles.Add(new AuthProfile { profileID = profileId++, userID = 70, firstName = "Linh Thị", lastName = "M", gender = "Nữ" });
            for (int i = 71; i <= 80; i++)
            {
                profiles.Add(new AuthProfile
                {
                    profileID = profileId++,
                    userID = i,
                    firstName = $"Designer",
                    lastName = $"{i - 70}",
                    gender = i % 2 == 0 ? "Nam" : "Nữ"
                });
            }

            modelBuilder.Entity<AuthProfile>().HasData(profiles);
        }

        public static void SeedUserRoles(ModelBuilder modelBuilder)
        {
            var now = DateTime.UtcNow;
            var userRoles = new List<AuthUserRole>();

            // BGD: Giám đốc (userID 1)
            userRoles.Add(new AuthUserRole { userID = 1, roleID = 6, assignedAt = now });

            // BGD: Phó giám đốc (userID 2)
            userRoles.Add(new AuthUserRole { userID = 2, roleID = 7, assignedAt = now });

            // BGD: Lãnh đạo (userID 3)
            userRoles.Add(new AuthUserRole { userID = 3, roleID = 7, assignedAt = now });

            // Phòng Kế Toán: Trưởng phòng (userID 4)
            userRoles.Add(new AuthUserRole { userID = 4, roleID = 3, assignedAt = now });

            // Phòng Kế Toán: Kế toán TSCĐ (userID 5-7)
            userRoles.Add(new AuthUserRole { userID = 5, roleID = 2, assignedAt = now });
            userRoles.Add(new AuthUserRole { userID = 6, roleID = 2, assignedAt = now });
            userRoles.Add(new AuthUserRole { userID = 7, roleID = 2, assignedAt = now });

            // Phòng Kế Toán: Nhân viên khác (userID 8-14)
            for (int i = 8; i <= 14; i++)
            {
                userRoles.Add(new AuthUserRole { userID = i, roleID = 9, assignedAt = now });
            }

            // Phòng Nhân Sự: Trưởng phòng (userID 15)
            userRoles.Add(new AuthUserRole { userID = 15, roleID = 8, assignedAt = now });
            userRoles.Add(new AuthUserRole { userID = 15, roleID = 10, assignedAt = now });

            // Phòng Nhân Sự: Nhân viên (userID 16-25)
            for (int i = 16; i <= 25; i++)
            {
                userRoles.Add(new AuthUserRole { userID = i, roleID = 10, assignedAt = now });
            }

            // Phòng Kỹ Thuật: Trưởng phòng (userID 26)
            userRoles.Add(new AuthUserRole { userID = 26, roleID = 5, assignedAt = now });

            // Phòng Kỹ Thuật: Kỹ thuật viên (userID 27-36)
            for (int i = 27; i <= 36; i++)
            {
                userRoles.Add(new AuthUserRole { userID = i, roleID = 4, assignedAt = now });
            }

            // Phòng Sản Phẩm: Trưởng phòng (userID 37)
            userRoles.Add(new AuthUserRole { userID = 37, roleID = 8, assignedAt = now });

            // Phòng Sản Phẩm: Nhân viên (userID 38-47)
            for (int i = 38; i <= 47; i++)
            {
                userRoles.Add(new AuthUserRole { userID = i, roleID = 9, assignedAt = now });
            }

            // Phòng Phát Triển: Trưởng phòng (userID 48)
            userRoles.Add(new AuthUserRole { userID = 48, roleID = 8, assignedAt = now });

            // Phòng Phát Triển: Dev (userID 49-58)
            for (int i = 49; i <= 58; i++)
            {
                userRoles.Add(new AuthUserRole { userID = i, roleID = 9, assignedAt = now });
            }

            // Phòng Quản Lý Dự Án: Trưởng phòng (userID 59)
            userRoles.Add(new AuthUserRole { userID = 59, roleID = 8, assignedAt = now });

            // Phòng Quản Lý Dự Án: PM (userID 60-69)
            for (int i = 60; i <= 69; i++)
            {
                userRoles.Add(new AuthUserRole { userID = i, roleID = 9, assignedAt = now });
            }

            // Phòng Thiết Kế: Trưởng phòng (userID 70)
            userRoles.Add(new AuthUserRole { userID = 70, roleID = 8, assignedAt = now });

            // Phòng Thiết Kế: Designer (userID 71-80)
            for (int i = 71; i <= 80; i++)
            {
                userRoles.Add(new AuthUserRole { userID = i, roleID = 9, assignedAt = now });
            }

            modelBuilder.Entity<AuthUserRole>().HasData(userRoles);
        }
    }
}