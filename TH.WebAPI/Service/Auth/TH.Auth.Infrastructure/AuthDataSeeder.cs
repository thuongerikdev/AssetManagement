using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TH.Auth.Domain.Role;
using TH.Auth.Domain.User;

namespace TH.Auth.Infrastructure.SeedData
{
    public static class AuthSeedData
    {
        // BCrypt hash của "Password123!" (cost=11) - dùng tool ngoài để tạo thực tế
        private const string DEFAULT_PASSWORD_HASH = "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6";

        public static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthRole>().HasData(
                new AuthRole { roleID = 1, roleName = "admin_he_thong", roleDescription = "Quản trị viên hệ thống", scope = "staff", isDefault = false },
                new AuthRole { roleID = 2, roleName = "ke_toan_tscd", roleDescription = "Kế toán tài sản cố định", scope = "staff", isDefault = false },
                new AuthRole { roleID = 3, roleName = "truong_phong_ke_toan", roleDescription = "Trưởng phòng kế toán", scope = "staff", isDefault = false },
                new AuthRole { roleID = 4, roleName = "ky_thuat_vien", roleDescription = "Kỹ thuật viên", scope = "staff", isDefault = false },
                new AuthRole { roleID = 5, roleName = "truong_phong_ky_thuat", roleDescription = "Trưởng phòng kỹ thuật", scope = "staff", isDefault = false },
                new AuthRole { roleID = 6, roleName = "giam_doc", roleDescription = "Giám đốc", scope = "staff", isDefault = false },
                new AuthRole { roleID = 7, roleName = "pho_giam_doc", roleDescription = "Phó giám đốc", scope = "staff", isDefault = false },
                new AuthRole { roleID = 8, roleName = "truong_phong_ban", roleDescription = "Trưởng phòng ban", scope = "staff", isDefault = false },
                new AuthRole { roleID = 9, roleName = "nhan_vien", roleDescription = "Nhân viên", scope = "staff", isDefault = true },
                new AuthRole { roleID = 10, roleName = "nhan_vien_nhan_su", roleDescription = "Nhân viên nhân sự", scope = "staff", isDefault = false }
            );
        }

        public static void SeedUsers(ModelBuilder modelBuilder)
        {
            var now = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // =====================================================
            // TỔNG: 35 USERS (ID 1–35)
            // BGD=1(4) | KT=2(5) | NS=3(2) | TECH=4(3)
            // PROD=5(5) | DEV=6(10) | PMO=7(3) | DESIGN=8(3)
            // =====================================================
            var users = new List<AuthUser>
            {
                // ===== BAN GIÁM ĐỐC (Dept 1) – 4 người =====
                // ID 1: Giám đốc
                new AuthUser { userID = 1, userName = "pham.quoc.hung", email = "pqhung@thtech.vn", phoneNumber = "0901234001", departmentID = "1", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 2: Phó Giám đốc 1
                new AuthUser { userID = 2, userName = "tran.thanh.tung", email = "tttung@thtech.vn", phoneNumber = "0901234002", departmentID = "1", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 3: Phó Giám đốc 2
                new AuthUser { userID = 3, userName = "nguyen.minh.chau", email = "nmchau@thtech.vn", phoneNumber = "0901234003", departmentID = "1", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 4: Thư ký Ban Giám đốc
                new AuthUser { userID = 4, userName = "le.quang.binh", email = "lqbinh@thtech.vn", phoneNumber = "0901234004", departmentID = "1", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },

                // ===== PHÒNG KẾ TOÁN (Dept 2) – 5 người =====
                // ID 5: Trưởng phòng kế toán
                new AuthUser { userID = 5, userName = "nguyen.thi.huong", email = "nthuong.kt@thtech.vn", phoneNumber = "0902234001", departmentID = "2", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 6: Kế toán TSCĐ
                new AuthUser { userID = 6, userName = "le.bao.ngoc", email = "lbngoc.kt@thtech.vn", phoneNumber = "0902234002", departmentID = "2", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 7: Kế toán tổng hợp
                new AuthUser { userID = 7, userName = "tran.van.hai", email = "tvhai.kt@thtech.vn", phoneNumber = "0902234003", departmentID = "2", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 8: Kế toán thuế
                new AuthUser { userID = 8, userName = "pham.thi.mai", email = "ptmai.kt@thtech.vn", phoneNumber = "0902234004", departmentID = "2", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 9: Kế toán công nợ
                new AuthUser { userID = 9, userName = "vo.xuan.truong", email = "vxtruong.kt@thtech.vn", phoneNumber = "0902234005", departmentID = "2", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },

                // ===== PHÒNG NHÂN SỰ (Dept 3) – 2 người =====
                // ID 10: Trưởng phòng nhân sự
                new AuthUser { userID = 10, userName = "hoang.thu.ha", email = "htha.ns@thtech.vn", phoneNumber = "0903234001", departmentID = "3", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 11: Nhân viên nhân sự
                new AuthUser { userID = 11, userName = "bui.thi.linh", email = "btlinh.ns@thtech.vn", phoneNumber = "0903234002", departmentID = "3", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },

                // ===== PHÒNG KỸ THUẬT (Dept 4) – 3 người =====
                // ID 12: Trưởng phòng kỹ thuật
                new AuthUser { userID = 12, userName = "do.van.tai", email = "dvtai.tech@thtech.vn", phoneNumber = "0904234001", departmentID = "4", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 13: Kỹ thuật viên hạ tầng
                new AuthUser { userID = 13, userName = "nguyen.cong.minh", email = "ncminh.tech@thtech.vn", phoneNumber = "0904234002", departmentID = "4", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 14: Kỹ thuật viên mạng
                new AuthUser { userID = 14, userName = "ha.thi.loan", email = "htloan.tech@thtech.vn", phoneNumber = "0904234003", departmentID = "4", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },

                // ===== PHÒNG SẢN PHẨM (Dept 5) – 5 người =====
                // ID 15: Trưởng phòng sản phẩm
                new AuthUser { userID = 15, userName = "ly.minh.quan", email = "lmquan.prod@thtech.vn", phoneNumber = "0905234001", departmentID = "5", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 16: Product Manager
                new AuthUser { userID = 16, userName = "dinh.thi.bich.ngoc", email = "dtbngoc.prod@thtech.vn", phoneNumber = "0905234002", departmentID = "5", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 17: Product Owner
                new AuthUser { userID = 17, userName = "phan.huu.phuoc", email = "phphuoc.prod@thtech.vn", phoneNumber = "0905234003", departmentID = "5", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 18: Business Analyst
                new AuthUser { userID = 18, userName = "vu.thi.thuy.dung", email = "vttdung.prod@thtech.vn", phoneNumber = "0905234004", departmentID = "5", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 19: Product Designer
                new AuthUser { userID = 19, userName = "cao.ngoc.anh", email = "cnanh.prod@thtech.vn", phoneNumber = "0905234005", departmentID = "5", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },

                // ===== PHÒNG PHÁT TRIỂN PHẦN MỀM (Dept 6) – 10 người =====
                // ID 20: Trưởng phòng phát triển
                new AuthUser { userID = 20, userName = "dang.huu.thanh", email = "dhthanh.dev@thtech.vn", phoneNumber = "0906234001", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 21: Senior Developer
                new AuthUser { userID = 21, userName = "nguyen.van.dung", email = "nvdung.dev@thtech.vn", phoneNumber = "0906234002", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 22: Backend Developer
                new AuthUser { userID = 22, userName = "tran.thi.kim.oanh", email = "ttkoanh.dev@thtech.vn", phoneNumber = "0906234003", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 23: Frontend Developer
                new AuthUser { userID = 23, userName = "le.hoang.nam", email = "lhnam.dev@thtech.vn", phoneNumber = "0906234004", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 24: Full-stack Developer
                new AuthUser { userID = 24, userName = "pham.tuan.anh", email = "ptanh.dev@thtech.vn", phoneNumber = "0906234005", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 25: Mobile Developer
                new AuthUser { userID = 25, userName = "vu.duc.manh", email = "vdmanh.dev@thtech.vn", phoneNumber = "0906234006", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 26: Backend Developer
                new AuthUser { userID = 26, userName = "hoang.thi.hong", email = "hthong.dev@thtech.vn", phoneNumber = "0906234007", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 27: DevOps Engineer
                new AuthUser { userID = 27, userName = "bui.thanh.phong", email = "btphong.dev@thtech.vn", phoneNumber = "0906234008", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 28: Junior Developer
                new AuthUser { userID = 28, userName = "do.thanh.dat", email = "dtdat.dev@thtech.vn", phoneNumber = "0906234009", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 29: Junior Developer
                new AuthUser { userID = 29, userName = "luu.thi.bao.chau", email = "ltbchau.dev@thtech.vn", phoneNumber = "0906234010", departmentID = "6", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },

                // ===== PHÒNG QUẢN LÝ DỰ ÁN (Dept 7) – 3 người =====
                // ID 30: Trưởng phòng / PMO Lead
                new AuthUser { userID = 30, userName = "vu.thanh.long", email = "vtlong.pmo@thtech.vn", phoneNumber = "0907234001", departmentID = "7", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 31: Project Manager
                new AuthUser { userID = 31, userName = "nguyen.ba.kien", email = "nbkien.pmo@thtech.vn", phoneNumber = "0907234002", departmentID = "7", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 32: Business Analyst / Scrum Master
                new AuthUser { userID = 32, userName = "tran.thi.phuong", email = "ttphuong.pmo@thtech.vn", phoneNumber = "0907234003", departmentID = "7", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },

                // ===== PHÒNG THIẾT KẾ UI/UX (Dept 8) – 3 người =====
                // ID 33: Lead Designer / Trưởng phòng
                new AuthUser { userID = 33, userName = "phan.ngoc.yen", email = "pnyen.design@thtech.vn", phoneNumber = "0908234001", departmentID = "8", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 34: UI Designer
                new AuthUser { userID = 34, userName = "cao.thi.bich", email = "ctbich.design@thtech.vn", phoneNumber = "0908234002", departmentID = "8", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
                // ID 35: UX Researcher / Motion Designer
                new AuthUser { userID = 35, userName = "truong.van.khai", email = "tvkhai.design@thtech.vn", phoneNumber = "0908234003", departmentID = "8", passwordHash = DEFAULT_PASSWORD_HASH, isEmailVerified = true, status = "Active", tokenVersion = 1, scope = "staff", createdAt = now, updatedAt = now },
            };

            modelBuilder.Entity<AuthUser>().HasData(users);
        }

        public static void SeedProfiles(ModelBuilder modelBuilder)
        {
            var profiles = new List<AuthProfile>
            {
                // ===== BAN GIÁM ĐỐC =====
                new AuthProfile { profileID = 1,  userID = 1,  firstName = "Phạm Quốc",     lastName = "Hùng",   gender = "Nam" },
                new AuthProfile { profileID = 2,  userID = 2,  firstName = "Trần Thanh",     lastName = "Tùng",   gender = "Nam" },
                new AuthProfile { profileID = 3,  userID = 3,  firstName = "Nguyễn Thị Minh",lastName = "Châu",   gender = "Nữ"  },
                new AuthProfile { profileID = 4,  userID = 4,  firstName = "Lê Quang",       lastName = "Bình",   gender = "Nam" },

                // ===== PHÒNG KẾ TOÁN =====
                new AuthProfile { profileID = 5,  userID = 5,  firstName = "Nguyễn Thị",     lastName = "Hương",  gender = "Nữ"  },
                new AuthProfile { profileID = 6,  userID = 6,  firstName = "Lê Bảo",         lastName = "Ngọc",   gender = "Nữ"  },
                new AuthProfile { profileID = 7,  userID = 7,  firstName = "Trần Văn",       lastName = "Hải",    gender = "Nam" },
                new AuthProfile { profileID = 8,  userID = 8,  firstName = "Phạm Thị",       lastName = "Mai",    gender = "Nữ"  },
                new AuthProfile { profileID = 9,  userID = 9,  firstName = "Võ Xuân",        lastName = "Trường", gender = "Nam" },

                // ===== PHÒNG NHÂN SỰ =====
                new AuthProfile { profileID = 10, userID = 10, firstName = "Hoàng Thu",      lastName = "Hà",     gender = "Nữ"  },
                new AuthProfile { profileID = 11, userID = 11, firstName = "Bùi Thị",        lastName = "Linh",   gender = "Nữ"  },

                // ===== PHÒNG KỸ THUẬT =====
                new AuthProfile { profileID = 12, userID = 12, firstName = "Đỗ Văn",         lastName = "Tài",    gender = "Nam" },
                new AuthProfile { profileID = 13, userID = 13, firstName = "Nguyễn Công",    lastName = "Minh",   gender = "Nam" },
                new AuthProfile { profileID = 14, userID = 14, firstName = "Hà Thị",         lastName = "Loan",   gender = "Nữ"  },

                // ===== PHÒNG SẢN PHẨM =====
                new AuthProfile { profileID = 15, userID = 15, firstName = "Lý Minh",        lastName = "Quân",   gender = "Nam" },
                new AuthProfile { profileID = 16, userID = 16, firstName = "Đinh Thị Bích",  lastName = "Ngọc",   gender = "Nữ"  },
                new AuthProfile { profileID = 17, userID = 17, firstName = "Phan Hữu",       lastName = "Phước",  gender = "Nam" },
                new AuthProfile { profileID = 18, userID = 18, firstName = "Vũ Thị Thùy",    lastName = "Dung",   gender = "Nữ"  },
                new AuthProfile { profileID = 19, userID = 19, firstName = "Cao Ngọc",       lastName = "Anh",    gender = "Nữ"  },

                // ===== PHÒNG PHÁT TRIỂN PHẦN MỀM =====
                new AuthProfile { profileID = 20, userID = 20, firstName = "Đặng Hữu",       lastName = "Thành",  gender = "Nam" },
                new AuthProfile { profileID = 21, userID = 21, firstName = "Nguyễn Văn",     lastName = "Dũng",   gender = "Nam" },
                new AuthProfile { profileID = 22, userID = 22, firstName = "Trần Thị Kim",   lastName = "Oanh",   gender = "Nữ"  },
                new AuthProfile { profileID = 23, userID = 23, firstName = "Lê Hoàng",       lastName = "Nam",    gender = "Nam" },
                new AuthProfile { profileID = 24, userID = 24, firstName = "Phạm Tuấn",      lastName = "Anh",    gender = "Nam" },
                new AuthProfile { profileID = 25, userID = 25, firstName = "Vũ Đức",         lastName = "Mạnh",   gender = "Nam" },
                new AuthProfile { profileID = 26, userID = 26, firstName = "Hoàng Thị",      lastName = "Hồng",   gender = "Nữ"  },
                new AuthProfile { profileID = 27, userID = 27, firstName = "Bùi Thanh",      lastName = "Phong",  gender = "Nam" },
                new AuthProfile { profileID = 28, userID = 28, firstName = "Đỗ Thành",       lastName = "Đạt",    gender = "Nam" },
                new AuthProfile { profileID = 29, userID = 29, firstName = "Lưu Thị Bảo",   lastName = "Châu",   gender = "Nữ"  },

                // ===== PHÒNG QUẢN LÝ DỰ ÁN =====
                new AuthProfile { profileID = 30, userID = 30, firstName = "Vũ Thành",       lastName = "Long",   gender = "Nam" },
                new AuthProfile { profileID = 31, userID = 31, firstName = "Nguyễn Bá",      lastName = "Kiên",   gender = "Nam" },
                new AuthProfile { profileID = 32, userID = 32, firstName = "Trần Thị",       lastName = "Phương", gender = "Nữ"  },

                // ===== PHÒNG THIẾT KẾ UI/UX =====
                new AuthProfile { profileID = 33, userID = 33, firstName = "Phan Ngọc",      lastName = "Yến",    gender = "Nữ"  },
                new AuthProfile { profileID = 34, userID = 34, firstName = "Cao Thị",        lastName = "Bích",   gender = "Nữ"  },
                new AuthProfile { profileID = 35, userID = 35, firstName = "Trương Văn",     lastName = "Khải",   gender = "Nam" },
            };

            modelBuilder.Entity<AuthProfile>().HasData(profiles);
        }

        public static void SeedUserRoles(ModelBuilder modelBuilder)
        {
            var now = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var userRoles = new List<AuthUserRole>
            {
                // ===== BAN GIÁM ĐỐC =====
                new AuthUserRole { userID = 1, roleID = 6, assignedAt = now },  // Giám đốc
                new AuthUserRole { userID = 2, roleID = 7, assignedAt = now },  // Phó GĐ
                new AuthUserRole { userID = 3, roleID = 7, assignedAt = now },  // Phó GĐ
                new AuthUserRole { userID = 4, roleID = 9, assignedAt = now },  // Thư ký → nhân viên

                // ===== PHÒNG KẾ TOÁN =====
                new AuthUserRole { userID = 5, roleID = 3, assignedAt = now },  // Trưởng phòng kế toán
                new AuthUserRole { userID = 6, roleID = 2, assignedAt = now },  // Kế toán TSCĐ
                new AuthUserRole { userID = 7, roleID = 9, assignedAt = now },  // Nhân viên kế toán
                new AuthUserRole { userID = 8, roleID = 9, assignedAt = now },  // Nhân viên kế toán
                new AuthUserRole { userID = 9, roleID = 9, assignedAt = now },  // Nhân viên kế toán

                // ===== PHÒNG NHÂN SỰ =====
                new AuthUserRole { userID = 10, roleID = 8,  assignedAt = now },  // Trưởng phòng
                new AuthUserRole { userID = 10, roleID = 10, assignedAt = now },  // Kiêm: nhân viên nhân sự
                new AuthUserRole { userID = 11, roleID = 10, assignedAt = now },  // Nhân viên nhân sự

                // ===== PHÒNG KỸ THUẬT =====
                new AuthUserRole { userID = 12, roleID = 5, assignedAt = now },  // Trưởng phòng kỹ thuật
                new AuthUserRole { userID = 13, roleID = 4, assignedAt = now },  // Kỹ thuật viên
                new AuthUserRole { userID = 14, roleID = 4, assignedAt = now },  // Kỹ thuật viên

                // ===== PHÒNG SẢN PHẨM =====
                new AuthUserRole { userID = 15, roleID = 8, assignedAt = now },  // Trưởng phòng
                new AuthUserRole { userID = 16, roleID = 9, assignedAt = now },  // Nhân viên
                new AuthUserRole { userID = 17, roleID = 9, assignedAt = now },  // Nhân viên
                new AuthUserRole { userID = 18, roleID = 9, assignedAt = now },  // Nhân viên
                new AuthUserRole { userID = 19, roleID = 9, assignedAt = now },  // Nhân viên

                // ===== PHÒNG PHÁT TRIỂN PHẦN MỀM =====
                new AuthUserRole { userID = 20, roleID = 8, assignedAt = now },  // Trưởng phòng
                new AuthUserRole { userID = 21, roleID = 9, assignedAt = now },
                new AuthUserRole { userID = 22, roleID = 9, assignedAt = now },
                new AuthUserRole { userID = 23, roleID = 9, assignedAt = now },
                new AuthUserRole { userID = 24, roleID = 9, assignedAt = now },
                new AuthUserRole { userID = 25, roleID = 9, assignedAt = now },
                new AuthUserRole { userID = 26, roleID = 9, assignedAt = now },
                new AuthUserRole { userID = 27, roleID = 9, assignedAt = now },
                new AuthUserRole { userID = 28, roleID = 9, assignedAt = now },
                new AuthUserRole { userID = 29, roleID = 9, assignedAt = now },

                // ===== PHÒNG QUẢN LÝ DỰ ÁN =====
                new AuthUserRole { userID = 30, roleID = 8, assignedAt = now },  // Trưởng phòng
                new AuthUserRole { userID = 31, roleID = 9, assignedAt = now },
                new AuthUserRole { userID = 32, roleID = 9, assignedAt = now },

                // ===== PHÒNG THIẾT KẾ UI/UX =====
                new AuthUserRole { userID = 33, roleID = 8, assignedAt = now },  // Trưởng phòng / Lead Designer
                new AuthUserRole { userID = 34, roleID = 9, assignedAt = now },
                new AuthUserRole { userID = 35, roleID = 9, assignedAt = now },
            };

            modelBuilder.Entity<AuthUserRole>().HasData(userRoles);
        }
    }
}
