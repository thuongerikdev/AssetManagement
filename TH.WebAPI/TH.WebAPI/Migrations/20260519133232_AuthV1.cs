using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TH.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AuthV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.CreateTable(
                name: "AuthPermission",
                schema: "auth",
                columns: table => new
                {
                    permissionID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    permissionName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    permissionDescription = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    scope = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthPermission", x => x.permissionID);
                });

            migrationBuilder.CreateTable(
                name: "AuthRole",
                schema: "auth",
                columns: table => new
                {
                    roleID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    roleDescription = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    isDefault = table.Column<bool>(type: "boolean", nullable: false),
                    scope = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthRole", x => x.roleID);
                });

            migrationBuilder.CreateTable(
                name: "AuthUser",
                schema: "auth",
                columns: table => new
                {
                    userID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phoneNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    departmentID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    passwordHash = table.Column<string>(type: "text", nullable: false),
                    googleSub = table.Column<string>(type: "text", nullable: true),
                    scope = table.Column<string>(type: "text", nullable: true),
                    isEmailVerified = table.Column<bool>(type: "boolean", nullable: false),
                    status = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    tokenVersion = table.Column<int>(type: "integer", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUser", x => x.userID);
                });

            migrationBuilder.CreateTable(
                name: "AuthRolePermission",
                schema: "auth",
                columns: table => new
                {
                    rolePermissionID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleID = table.Column<int>(type: "integer", nullable: false),
                    permissionID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthRolePermission", x => x.rolePermissionID);
                    table.ForeignKey(
                        name: "FK_AuthRolePermission_AuthPermission_permissionID",
                        column: x => x.permissionID,
                        principalSchema: "auth",
                        principalTable: "AuthPermission",
                        principalColumn: "permissionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthRolePermission_AuthRole_roleID",
                        column: x => x.roleID,
                        principalSchema: "auth",
                        principalTable: "AuthRole",
                        principalColumn: "roleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthAuditLog",
                schema: "auth",
                columns: table => new
                {
                    auditID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userID = table.Column<int>(type: "integer", nullable: true),
                    action = table.Column<string>(type: "text", nullable: true),
                    result = table.Column<string>(type: "text", nullable: true),
                    detail = table.Column<string>(type: "text", nullable: true),
                    ip = table.Column<string>(type: "text", nullable: true),
                    userAgent = table.Column<string>(type: "text", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthAuditLog", x => x.auditID);
                    table.ForeignKey(
                        name: "FK_AuthAuditLog_AuthUser_userID",
                        column: x => x.userID,
                        principalSchema: "auth",
                        principalTable: "AuthUser",
                        principalColumn: "userID");
                });

            migrationBuilder.CreateTable(
                name: "AuthEmailVerification",
                schema: "auth",
                columns: table => new
                {
                    emailVerificationID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userID = table.Column<int>(type: "integer", nullable: false),
                    codeHash = table.Column<string>(type: "text", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    consumedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthEmailVerification", x => x.emailVerificationID);
                    table.ForeignKey(
                        name: "FK_AuthEmailVerification_AuthUser_userID",
                        column: x => x.userID,
                        principalSchema: "auth",
                        principalTable: "AuthUser",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthMfaSecret",
                schema: "auth",
                columns: table => new
                {
                    mfaID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userID = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    status = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    secret = table.Column<string>(type: "text", nullable: true),
                    isEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    label = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    recoveryCodes = table.Column<string>(type: "text", nullable: true),
                    enrollmentStartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    enabledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastVerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthMfaSecret", x => x.mfaID);
                    table.ForeignKey(
                        name: "FK_AuthMfaSecret_AuthUser_userID",
                        column: x => x.userID,
                        principalSchema: "auth",
                        principalTable: "AuthUser",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthPasswordReset",
                schema: "auth",
                columns: table => new
                {
                    passwordResetID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userID = table.Column<int>(type: "integer", nullable: false),
                    codeHash = table.Column<string>(type: "text", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    consumedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    purpose = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthPasswordReset", x => x.passwordResetID);
                    table.ForeignKey(
                        name: "FK_AuthPasswordReset_AuthUser_userID",
                        column: x => x.userID,
                        principalSchema: "auth",
                        principalTable: "AuthUser",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthProfile",
                schema: "auth",
                columns: table => new
                {
                    profileID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userID = table.Column<int>(type: "integer", nullable: false),
                    firstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    lastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    avatar = table.Column<string>(type: "text", nullable: true),
                    gender = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    dateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthProfile", x => x.profileID);
                    table.ForeignKey(
                        name: "FK_AuthProfile_AuthUser_userID",
                        column: x => x.userID,
                        principalSchema: "auth",
                        principalTable: "AuthUser",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthUserRole",
                schema: "auth",
                columns: table => new
                {
                    userID = table.Column<int>(type: "integer", nullable: false),
                    roleID = table.Column<int>(type: "integer", nullable: false),
                    assignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUserRole", x => new { x.userID, x.roleID });
                    table.ForeignKey(
                        name: "FK_AuthUserRole_AuthRole_roleID",
                        column: x => x.roleID,
                        principalSchema: "auth",
                        principalTable: "AuthRole",
                        principalColumn: "roleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthUserRole_AuthUser_userID",
                        column: x => x.userID,
                        principalSchema: "auth",
                        principalTable: "AuthUser",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthUserSession",
                schema: "auth",
                columns: table => new
                {
                    sessionID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userID = table.Column<int>(type: "integer", nullable: false),
                    deviceId = table.Column<string>(type: "text", nullable: false),
                    ip = table.Column<string>(type: "text", nullable: true),
                    userAgent = table.Column<string>(type: "text", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lastSeenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isRevoked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUserSession", x => x.sessionID);
                    table.ForeignKey(
                        name: "FK_AuthUserSession_AuthUser_userID",
                        column: x => x.userID,
                        principalSchema: "auth",
                        principalTable: "AuthUser",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthRefreshToken",
                schema: "auth",
                columns: table => new
                {
                    refreshTokenID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userID = table.Column<int>(type: "integer", nullable: false),
                    sessionID = table.Column<int>(type: "integer", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByIp = table.Column<string>(type: "text", nullable: true),
                    Revoked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RevokedByIp = table.Column<string>(type: "text", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthRefreshToken", x => x.refreshTokenID);
                    table.ForeignKey(
                        name: "FK_AuthRefreshToken_AuthUserSession_sessionID",
                        column: x => x.sessionID,
                        principalSchema: "auth",
                        principalTable: "AuthUserSession",
                        principalColumn: "sessionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthRefreshToken_AuthUser_userID",
                        column: x => x.userID,
                        principalSchema: "auth",
                        principalTable: "AuthUser",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "AuthPermission",
                columns: new[] { "permissionID", "code", "permissionDescription", "permissionName", "scope" },
                values: new object[,]
                {
                    { 1, "audit_log.get_all", "audit_log.get_all", "AuditLogGetAll", "staff" },
                    { 2, "audit_log.get_by_id", "audit_log.get_by_id", "AuditLogGetById", "staff" },
                    { 3, "audit_log.get_by_user_id", "audit_log.get_by_user_id", "AuditLogGetByUserId", "staff" },
                    { 4, "auth.create_user", "auth.create_user", "AuthCreateUser", "staff" },
                    { 5, "bao_tri_tai_san.create", "bao_tri_tai_san.create", "BaoTriTaiSanCreate", "staff" },
                    { 6, "bao_tri_tai_san.delete", "bao_tri_tai_san.delete", "BaoTriTaiSanDelete", "staff" },
                    { 7, "bao_tri_tai_san.get_all", "bao_tri_tai_san.get_all", "BaoTriTaiSanGetAll", "staff" },
                    { 8, "bao_tri_tai_san.get_by_asset", "bao_tri_tai_san.get_by_asset", "BaoTriTaiSanGetByAsset", "staff" },
                    { 9, "bao_tri_tai_san.get_by_id", "bao_tri_tai_san.get_by_id", "BaoTriTaiSanGetById", "staff" },
                    { 10, "bao_tri_tai_san.update", "bao_tri_tai_san.update", "BaoTriTaiSanUpdate", "staff" },
                    { 11, "cau_hinh_he_thong.get", "cau_hinh_he_thong.get", "CauHinhHeThongGet", "staff" },
                    { 12, "cau_hinh_he_thong.update", "cau_hinh_he_thong.update", "CauHinhHeThongUpdate", "staff" },
                    { 13, "chung_tu.create", "chung_tu.create", "ChungTuCreate", "staff" },
                    { 14, "chung_tu.delete", "chung_tu.delete", "ChungTuDelete", "staff" },
                    { 15, "chung_tu.generate_code", "chung_tu.generate_code", "ChungTuGenerateCode", "staff" },
                    { 16, "chung_tu.get_all", "chung_tu.get_all", "ChungTuGetAll", "staff" },
                    { 17, "chung_tu.get_by_asset", "chung_tu.get_by_asset", "ChungTuGetByAsset", "staff" },
                    { 18, "chung_tu.get_by_id", "chung_tu.get_by_id", "ChungTuGetById", "staff" },
                    { 19, "chung_tu.update", "chung_tu.update", "ChungTuUpdate", "staff" },
                    { 20, "danh_muc_tai_san.create", "danh_muc_tai_san.create", "DanhMucTaiSanCreate", "staff" },
                    { 21, "danh_muc_tai_san.delete", "danh_muc_tai_san.delete", "DanhMucTaiSanDelete", "staff" },
                    { 22, "danh_muc_tai_san.get_all", "danh_muc_tai_san.get_all", "DanhMucTaiSanGetAll", "staff" },
                    { 23, "danh_muc_tai_san.get_by_id", "danh_muc_tai_san.get_by_id", "DanhMucTaiSanGetById", "staff" },
                    { 24, "danh_muc_tai_san.update", "danh_muc_tai_san.update", "DanhMucTaiSanUpdate", "staff" },
                    { 25, "dieu_chuyen_tai_san.create", "dieu_chuyen_tai_san.create", "DieuChuyenTaiSanCreate", "staff" },
                    { 26, "dieu_chuyen_tai_san.delete", "dieu_chuyen_tai_san.delete", "DieuChuyenTaiSanDelete", "staff" },
                    { 27, "dieu_chuyen_tai_san.get_all", "dieu_chuyen_tai_san.get_all", "DieuChuyenTaiSanGetAll", "staff" },
                    { 28, "dieu_chuyen_tai_san.get_by_asset", "dieu_chuyen_tai_san.get_by_asset", "DieuChuyenTaiSanGetByAsset", "staff" },
                    { 29, "dieu_chuyen_tai_san.get_by_id", "dieu_chuyen_tai_san.get_by_id", "DieuChuyenTaiSanGetById", "staff" },
                    { 30, "dieu_chuyen_tai_san.update", "dieu_chuyen_tai_san.update", "DieuChuyenTaiSanUpdate", "staff" },
                    { 31, "lich_su_khau_hao.create", "lich_su_khau_hao.create", "LichSuKhauHaoCreate", "staff" },
                    { 32, "lich_su_khau_hao.create_bulk", "lich_su_khau_hao.create_bulk", "LichSuKhauHaoCreateBulk", "staff" },
                    { 33, "lich_su_khau_hao.delete", "lich_su_khau_hao.delete", "LichSuKhauHaoDelete", "staff" },
                    { 34, "lich_su_khau_hao.get_all", "lich_su_khau_hao.get_all", "LichSuKhauHaoGetAll", "staff" },
                    { 35, "lich_su_khau_hao.get_by_asset", "lich_su_khau_hao.get_by_asset", "LichSuKhauHaoGetByAsset", "staff" },
                    { 36, "lich_su_khau_hao.get_by_id", "lich_su_khau_hao.get_by_id", "LichSuKhauHaoGetById", "staff" },
                    { 37, "lich_su_khau_hao.update", "lich_su_khau_hao.update", "LichSuKhauHaoUpdate", "staff" },
                    { 38, "permission.admin_add", "permission.admin_add", "PermissionAdminAdd", "staff" },
                    { 39, "permission.admin_bulk_create", "permission.admin_bulk_create", "PermissionAdminBulkCreate", "staff" },
                    { 40, "permission.admin_delete", "permission.admin_delete", "PermissionAdminDelete", "staff" },
                    { 41, "permission.admin_get_all", "permission.admin_get_all", "PermissionAdminGetAll", "staff" },
                    { 42, "permission.admin_get_by_id", "permission.admin_get_by_id", "PermissionAdminGetById", "staff" },
                    { 43, "permission.admin_get_by_role_id", "permission.admin_get_by_role_id", "PermissionAdminGetByRoleId", "staff" },
                    { 44, "permission.admin_get_by_user_id", "permission.admin_get_by_user_id", "PermissionAdminGetByUserId", "staff" },
                    { 45, "permission.admin_update", "permission.admin_update", "PermissionAdminUpdate", "staff" },
                    { 46, "permission.bulk_create", "permission.bulk_create", "PermissionBulkCreate", "staff" },
                    { 47, "permission.delete", "permission.delete", "PermissionDelete", "staff" },
                    { 48, "permission.get_all", "permission.get_all", "PermissionGetAll", "staff" },
                    { 49, "permission.get_by_id", "permission.get_by_id", "PermissionGetById", "staff" },
                    { 50, "permission.get_by_role_id", "permission.get_by_role_id", "PermissionGetByRoleId", "staff" },
                    { 51, "permission.get_by_user_id", "permission.get_by_user_id", "PermissionGetByUserId", "staff" },
                    { 52, "permission.update", "permission.update", "PermissionUpdate", "staff" },
                    { 53, "phong_ban.create", "phong_ban.create", "PhongBanCreate", "staff" },
                    { 54, "phong_ban.delete", "phong_ban.delete", "PhongBanDelete", "staff" },
                    { 55, "phong_ban.get_all", "phong_ban.get_all", "PhongBanGetAll", "staff" },
                    { 56, "phong_ban.get_by_id", "phong_ban.get_by_id", "PhongBanGetById", "staff" },
                    { 57, "phong_ban.update", "phong_ban.update", "PhongBanUpdate", "staff" },
                    { 58, "role.add", "role.add", "RoleAdd", "staff" },
                    { 59, "role.admin_add", "role.admin_add", "RoleAdminAdd", "staff" },
                    { 60, "role.admin_clone", "role.admin_clone", "RoleAdminClone", "staff" },
                    { 61, "role.admin_delete", "role.admin_delete", "RoleAdminDelete", "staff" },
                    { 62, "role.admin_update", "role.admin_update", "RoleAdminUpdate", "staff" },
                    { 63, "role.clone", "role.clone", "RoleClone", "staff" },
                    { 64, "role.delete", "role.delete", "RoleDelete", "staff" },
                    { 65, "role.get_all", "role.get_all", "RoleGetAll", "staff" },
                    { 66, "role.get_all_scope_user", "role.get_all_scope_user", "RoleGetAllScopeUser", "staff" },
                    { 67, "role.get_by_user_id", "role.get_by_user_id", "RoleGetByUserId", "staff" },
                    { 68, "role_permission.admin_assign", "role_permission.admin_assign", "RolePermissionAdminAssign", "staff" },
                    { 69, "role_permission.assign", "role_permission.assign", "RolePermissionAssign", "staff" },
                    { 70, "role.update", "role.update", "RoleUpdate", "staff" },
                    { 71, "so_cai.get_chi_tiet", "so_cai.get_chi_tiet", "SoCaiGetChiTiet", "staff" },
                    { 72, "so_cai.get_tom_tat", "so_cai.get_tom_tat", "SoCaiGetTomTat", "staff" },
                    { 73, "tai_khoan_ke_toan.create", "tai_khoan_ke_toan.create", "TaiKhoanKeToanCreate", "staff" },
                    { 74, "tai_khoan_ke_toan.delete", "tai_khoan_ke_toan.delete", "TaiKhoanKeToanDelete", "staff" },
                    { 75, "tai_khoan_ke_toan.get_all", "tai_khoan_ke_toan.get_all", "TaiKhoanKeToanGetAll", "staff" },
                    { 76, "tai_khoan_ke_toan.get_by_id", "tai_khoan_ke_toan.get_by_id", "TaiKhoanKeToanGetById", "staff" },
                    { 77, "tai_khoan_ke_toan.update", "tai_khoan_ke_toan.update", "TaiKhoanKeToanUpdate", "staff" },
                    { 78, "tai_san.confirm", "tai_san.confirm", "TaiSanConfirm", "staff" },
                    { 79, "tai_san.create", "tai_san.create", "TaiSanCreate", "staff" },
                    { 80, "tai_san.delete", "tai_san.delete", "TaiSanDelete", "staff" },
                    { 81, "tai_san_dinh_kem.delete", "tai_san_dinh_kem.delete", "TaiSanDinhKemDelete", "staff" },
                    { 82, "tai_san_dinh_kem.get_by_asset", "tai_san_dinh_kem.get_by_asset", "TaiSanDinhKemGetByAsset", "staff" },
                    { 83, "tai_san_dinh_kem.upload", "tai_san_dinh_kem.upload", "TaiSanDinhKemUpload", "staff" },
                    { 84, "tai_san.generate_code", "tai_san.generate_code", "TaiSanGenerateCode", "staff" },
                    { 85, "tai_san.get_all", "tai_san.get_all", "TaiSanGetAll", "staff" },
                    { 86, "tai_san.get_by_id", "tai_san.get_by_id", "TaiSanGetById", "staff" },
                    { 87, "tai_san.get_mine", "tai_san.get_mine", "TaiSanGetMine", "staff" },
                    { 88, "tai_san.reject", "tai_san.reject", "TaiSanReject", "staff" },
                    { 89, "tai_san.update", "tai_san.update", "TaiSanUpdate", "staff" },
                    { 90, "thanh_ly_tai_san.create", "thanh_ly_tai_san.create", "ThanhLyTaiSanCreate", "staff" },
                    { 91, "thanh_ly_tai_san.delete", "thanh_ly_tai_san.delete", "ThanhLyTaiSanDelete", "staff" },
                    { 92, "thanh_ly_tai_san.get_all", "thanh_ly_tai_san.get_all", "ThanhLyTaiSanGetAll", "staff" },
                    { 93, "thanh_ly_tai_san.get_by_asset", "thanh_ly_tai_san.get_by_asset", "ThanhLyTaiSanGetByAsset", "staff" },
                    { 94, "thanh_ly_tai_san.get_by_id", "thanh_ly_tai_san.get_by_id", "ThanhLyTaiSanGetById", "staff" },
                    { 95, "thanh_ly_tai_san.update", "thanh_ly_tai_san.update", "ThanhLyTaiSanUpdate", "staff" },
                    { 96, "user.admin_get_all", "user.admin_get_all", "UserAdminGetAll", "staff" },
                    { 97, "user.admin_get_by_id", "user.admin_get_by_id", "UserAdminGetById", "staff" },
                    { 98, "user.admin_get_slim_by_id", "user.admin_get_slim_by_id", "UserAdminGetSlimById", "staff" },
                    { 99, "user.delete", "user.delete", "UserDelete", "staff" },
                    { 100, "user.get_all", "user.get_all", "UserGetAll", "staff" },
                    { 101, "user.get_all_slim", "user.get_all_slim", "UserGetAllSlim", "staff" },
                    { 102, "user.get_by_department_id", "user.get_by_department_id", "UserGetByDepartmentId", "staff" },
                    { 103, "user.get_me", "user.get_me", "UserGetMe", "staff" },
                    { 104, "user.get_slim_by_id", "user.get_slim_by_id", "UserGetSlimById", "staff" },
                    { 105, "user_role.admin_assign", "user_role.admin_assign", "UserRoleAdminAssign", "staff" },
                    { 106, "user_role.assign", "user_role.assign", "UserRoleAssign", "staff" },
                    { 107, "user_session.get_all", "user_session.get_all", "UserSessionGetAll", "staff" },
                    { 108, "user_session.get_by_session_id", "user_session.get_by_session_id", "UserSessionGetBySessionId", "staff" },
                    { 109, "user_session.get_by_user_id", "user_session.get_by_user_id", "UserSessionGetByUserId", "staff" },
                    { 110, "user.update_profile", "user.update_profile", "UserUpdateProfile", "staff" },
                    { 111, "user.update_username", "user.update_username", "UserUpdateUsername", "staff" }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "AuthRole",
                columns: new[] { "roleID", "isDefault", "roleDescription", "roleName", "scope" },
                values: new object[,]
                {
                    { 1, false, "Quản trị viên hệ thống", "admin", "staff" },
                    { 2, false, "Kế toán", "ke_toan", "staff" },
                    { 3, false, "Giám đốc", "giam_doc", "staff" },
                    { 4, false, "Trưởng phòng", "truong_phong", "staff" },
                    { 5, true, "Nhân viên", "nhan_vien", "staff" }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "AuthUser",
                columns: new[] { "userID", "createdAt", "departmentID", "email", "googleSub", "isEmailVerified", "passwordHash", "phoneNumber", "scope", "status", "tokenVersion", "updatedAt", "userName" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1", "pqhung@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0901234001", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pham.quoc.hung" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1", "tttung@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0901234002", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "tran.thanh.tung" },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1", "nmchau@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0901234003", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "nguyen.minh.chau" },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1", "lqbinh@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0901234004", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "le.quang.binh" },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2", "nthuong.kt@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0902234001", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "nguyen.thi.huong" },
                    { 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2", "lbngoc.kt@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0902234002", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "le.bao.ngoc" },
                    { 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2", "tvhai.kt@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0902234003", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "tran.van.hai" },
                    { 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2", "ptmai.kt@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0902234004", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pham.thi.mai" },
                    { 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2", "vxtruong.kt@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0902234005", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "vo.xuan.truong" },
                    { 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "3", "htha.ns@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0903234001", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "hoang.thu.ha" },
                    { 11, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "3", "btlinh.ns@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0903234002", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "bui.thi.linh" },
                    { 12, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "4", "dvtai.tech@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0904234001", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "do.van.tai" },
                    { 13, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "4", "ncminh.tech@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0904234002", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "nguyen.cong.minh" },
                    { 14, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "4", "htloan.tech@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0904234003", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ha.thi.loan" },
                    { 15, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "5", "lmquan.prod@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0905234001", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ly.minh.quan" },
                    { 16, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "5", "dtbngoc.prod@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0905234002", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "dinh.thi.bich.ngoc" },
                    { 17, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "5", "phphuoc.prod@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0905234003", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "phan.huu.phuoc" },
                    { 18, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "5", "vttdung.prod@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0905234004", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "vu.thi.thuy.dung" },
                    { 19, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "5", "cnanh.prod@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0905234005", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "cao.ngoc.anh" },
                    { 20, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "dhthanh.dev@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0906234001", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "dang.huu.thanh" },
                    { 21, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "nvdung.dev@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0906234002", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "nguyen.van.dung" },
                    { 22, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "ttkoanh.dev@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0906234003", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "tran.thi.kim.oanh" },
                    { 23, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "lhnam.dev@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0906234004", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "le.hoang.nam" },
                    { 24, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "ptanh.dev@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0906234005", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pham.tuan.anh" },
                    { 25, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "vdmanh.dev@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0906234006", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "vu.duc.manh" },
                    { 26, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "hthong.dev@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0906234007", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "hoang.thi.hong" },
                    { 27, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "btphong.dev@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0906234008", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "bui.thanh.phong" },
                    { 28, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "dtdat.dev@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0906234009", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "do.thanh.dat" },
                    { 29, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "ltbchau.dev@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0906234010", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "luu.thi.bao.chau" },
                    { 30, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "7", "vtlong.pmo@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0907234001", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "vu.thanh.long" },
                    { 31, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "7", "nbkien.pmo@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0907234002", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "nguyen.ba.kien" },
                    { 32, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "7", "ttphuong.pmo@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0907234003", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "tran.thi.phuong" },
                    { 33, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "8", "pnyen.design@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0908234001", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "phan.ngoc.yen" },
                    { 34, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "8", "ctbich.design@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0908234002", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "cao.thi.bich" },
                    { 35, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "8", "tvkhai.design@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0908234003", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "truong.van.khai" },
                    { 99, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "4", "admin@thtech.vn", null, true, "$2a$11$48frMocIZOKO42patU1Uze9dR.44pvg.vd1yxtc3XnUdJMwzcld.e", "0999999999", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin" }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "AuthProfile",
                columns: new[] { "profileID", "avatar", "dateOfBirth", "firstName", "gender", "lastName", "userID" },
                values: new object[,]
                {
                    { 1, null, null, "Phạm Quốc", "Nam", "Hùng", 1 },
                    { 2, null, null, "Trần Thanh", "Nam", "Tùng", 2 },
                    { 3, null, null, "Nguyễn Thị Minh", "Nữ", "Châu", 3 },
                    { 4, null, null, "Lê Quang", "Nam", "Bình", 4 },
                    { 5, null, null, "Nguyễn Thị", "Nữ", "Hương", 5 },
                    { 6, null, null, "Lê Bảo", "Nữ", "Ngọc", 6 },
                    { 7, null, null, "Trần Văn", "Nam", "Hải", 7 },
                    { 8, null, null, "Phạm Thị", "Nữ", "Mai", 8 },
                    { 9, null, null, "Võ Xuân", "Nam", "Trường", 9 },
                    { 10, null, null, "Hoàng Thu", "Nữ", "Hà", 10 },
                    { 11, null, null, "Bùi Thị", "Nữ", "Linh", 11 },
                    { 12, null, null, "Đỗ Văn", "Nam", "Tài", 12 },
                    { 13, null, null, "Nguyễn Công", "Nam", "Minh", 13 },
                    { 14, null, null, "Hà Thị", "Nữ", "Loan", 14 },
                    { 15, null, null, "Lý Minh", "Nam", "Quân", 15 },
                    { 16, null, null, "Đinh Thị Bích", "Nữ", "Ngọc", 16 },
                    { 17, null, null, "Phan Hữu", "Nam", "Phước", 17 },
                    { 18, null, null, "Vũ Thị Thùy", "Nữ", "Dung", 18 },
                    { 19, null, null, "Cao Ngọc", "Nữ", "Anh", 19 },
                    { 20, null, null, "Đặng Hữu", "Nam", "Thành", 20 },
                    { 21, null, null, "Nguyễn Văn", "Nam", "Dũng", 21 },
                    { 22, null, null, "Trần Thị Kim", "Nữ", "Oanh", 22 },
                    { 23, null, null, "Lê Hoàng", "Nam", "Nam", 23 },
                    { 24, null, null, "Phạm Tuấn", "Nam", "Anh", 24 },
                    { 25, null, null, "Vũ Đức", "Nam", "Mạnh", 25 },
                    { 26, null, null, "Hoàng Thị", "Nữ", "Hồng", 26 },
                    { 27, null, null, "Bùi Thanh", "Nam", "Phong", 27 },
                    { 28, null, null, "Đỗ Thành", "Nam", "Đạt", 28 },
                    { 29, null, null, "Lưu Thị Bảo", "Nữ", "Châu", 29 },
                    { 30, null, null, "Vũ Thành", "Nam", "Long", 30 },
                    { 31, null, null, "Nguyễn Bá", "Nam", "Kiên", 31 },
                    { 32, null, null, "Trần Thị", "Nữ", "Phương", 32 },
                    { 33, null, null, "Phan Ngọc", "Nữ", "Yến", 33 },
                    { 34, null, null, "Cao Thị", "Nữ", "Bích", 34 },
                    { 35, null, null, "Trương Văn", "Nam", "Khải", 35 },
                    { 99, null, null, "System", "Nam", "Admin", 99 }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "AuthRolePermission",
                columns: new[] { "rolePermissionID", "permissionID", "roleID" },
                values: new object[,]
                {
                    { 10001, 1, 1 },
                    { 10002, 2, 1 },
                    { 10003, 3, 1 },
                    { 10004, 4, 1 },
                    { 10005, 5, 1 },
                    { 10006, 6, 1 },
                    { 10007, 7, 1 },
                    { 10008, 8, 1 },
                    { 10009, 9, 1 },
                    { 10010, 10, 1 },
                    { 10011, 11, 1 },
                    { 10012, 12, 1 },
                    { 10013, 13, 1 },
                    { 10014, 14, 1 },
                    { 10015, 15, 1 },
                    { 10016, 16, 1 },
                    { 10017, 17, 1 },
                    { 10018, 18, 1 },
                    { 10019, 19, 1 },
                    { 10020, 20, 1 },
                    { 10021, 21, 1 },
                    { 10022, 22, 1 },
                    { 10023, 23, 1 },
                    { 10024, 24, 1 },
                    { 10025, 25, 1 },
                    { 10026, 26, 1 },
                    { 10027, 27, 1 },
                    { 10028, 28, 1 },
                    { 10029, 29, 1 },
                    { 10030, 30, 1 },
                    { 10031, 31, 1 },
                    { 10032, 32, 1 },
                    { 10033, 33, 1 },
                    { 10034, 34, 1 },
                    { 10035, 35, 1 },
                    { 10036, 36, 1 },
                    { 10037, 37, 1 },
                    { 10038, 38, 1 },
                    { 10039, 39, 1 },
                    { 10040, 40, 1 },
                    { 10041, 41, 1 },
                    { 10042, 42, 1 },
                    { 10043, 43, 1 },
                    { 10044, 44, 1 },
                    { 10045, 45, 1 },
                    { 10046, 46, 1 },
                    { 10047, 47, 1 },
                    { 10048, 48, 1 },
                    { 10049, 49, 1 },
                    { 10050, 50, 1 },
                    { 10051, 51, 1 },
                    { 10052, 52, 1 },
                    { 10053, 53, 1 },
                    { 10054, 54, 1 },
                    { 10055, 55, 1 },
                    { 10056, 56, 1 },
                    { 10057, 57, 1 },
                    { 10058, 58, 1 },
                    { 10059, 59, 1 },
                    { 10060, 60, 1 },
                    { 10061, 61, 1 },
                    { 10062, 62, 1 },
                    { 10063, 63, 1 },
                    { 10064, 64, 1 },
                    { 10065, 65, 1 },
                    { 10066, 66, 1 },
                    { 10067, 67, 1 },
                    { 10068, 68, 1 },
                    { 10069, 69, 1 },
                    { 10070, 70, 1 },
                    { 10071, 71, 1 },
                    { 10072, 72, 1 },
                    { 10073, 73, 1 },
                    { 10074, 74, 1 },
                    { 10075, 75, 1 },
                    { 10076, 76, 1 },
                    { 10077, 77, 1 },
                    { 10078, 78, 1 },
                    { 10079, 79, 1 },
                    { 10080, 80, 1 },
                    { 10081, 81, 1 },
                    { 10082, 82, 1 },
                    { 10083, 83, 1 },
                    { 10084, 84, 1 },
                    { 10085, 85, 1 },
                    { 10086, 86, 1 },
                    { 10087, 87, 1 },
                    { 10088, 88, 1 },
                    { 10089, 89, 1 },
                    { 10090, 90, 1 },
                    { 10091, 91, 1 },
                    { 10092, 92, 1 },
                    { 10093, 93, 1 },
                    { 10094, 94, 1 },
                    { 10095, 95, 1 },
                    { 10096, 96, 1 },
                    { 10097, 97, 1 },
                    { 10098, 98, 1 },
                    { 10099, 99, 1 },
                    { 10100, 100, 1 },
                    { 10101, 101, 1 },
                    { 10102, 102, 1 },
                    { 10103, 103, 1 },
                    { 10104, 104, 1 },
                    { 10105, 105, 1 },
                    { 10106, 106, 1 },
                    { 10107, 107, 1 },
                    { 10108, 108, 1 },
                    { 10109, 109, 1 },
                    { 10110, 110, 1 },
                    { 10111, 111, 1 },
                    { 20004, 4, 2 },
                    { 20007, 7, 2 },
                    { 20008, 8, 2 },
                    { 20009, 9, 2 },
                    { 20011, 11, 2 },
                    { 20013, 13, 2 },
                    { 20014, 14, 2 },
                    { 20015, 15, 2 },
                    { 20016, 16, 2 },
                    { 20017, 17, 2 },
                    { 20018, 18, 2 },
                    { 20019, 19, 2 },
                    { 20020, 20, 2 },
                    { 20022, 22, 2 },
                    { 20023, 23, 2 },
                    { 20024, 24, 2 },
                    { 20025, 25, 2 },
                    { 20027, 27, 2 },
                    { 20028, 28, 2 },
                    { 20029, 29, 2 },
                    { 20031, 31, 2 },
                    { 20032, 32, 2 },
                    { 20033, 33, 2 },
                    { 20034, 34, 2 },
                    { 20035, 35, 2 },
                    { 20036, 36, 2 },
                    { 20037, 37, 2 },
                    { 20048, 48, 2 },
                    { 20049, 49, 2 },
                    { 20050, 50, 2 },
                    { 20051, 51, 2 },
                    { 20055, 55, 2 },
                    { 20056, 56, 2 },
                    { 20065, 65, 2 },
                    { 20066, 66, 2 },
                    { 20067, 67, 2 },
                    { 20071, 71, 2 },
                    { 20072, 72, 2 },
                    { 20073, 73, 2 },
                    { 20075, 75, 2 },
                    { 20076, 76, 2 },
                    { 20077, 77, 2 },
                    { 20078, 78, 2 },
                    { 20079, 79, 2 },
                    { 20082, 82, 2 },
                    { 20083, 83, 2 },
                    { 20084, 84, 2 },
                    { 20085, 85, 2 },
                    { 20086, 86, 2 },
                    { 20087, 87, 2 },
                    { 20088, 88, 2 },
                    { 20089, 89, 2 },
                    { 20090, 90, 2 },
                    { 20092, 92, 2 },
                    { 20093, 93, 2 },
                    { 20094, 94, 2 },
                    { 20095, 95, 2 },
                    { 20100, 100, 2 },
                    { 20101, 101, 2 },
                    { 20102, 102, 2 },
                    { 20103, 103, 2 },
                    { 20104, 104, 2 },
                    { 20110, 110, 2 },
                    { 20111, 111, 2 },
                    { 30001, 1, 3 },
                    { 30002, 2, 3 },
                    { 30003, 3, 3 },
                    { 30007, 7, 3 },
                    { 30008, 8, 3 },
                    { 30009, 9, 3 },
                    { 30011, 11, 3 },
                    { 30016, 16, 3 },
                    { 30017, 17, 3 },
                    { 30018, 18, 3 },
                    { 30022, 22, 3 },
                    { 30023, 23, 3 },
                    { 30027, 27, 3 },
                    { 30028, 28, 3 },
                    { 30029, 29, 3 },
                    { 30034, 34, 3 },
                    { 30035, 35, 3 },
                    { 30036, 36, 3 },
                    { 30048, 48, 3 },
                    { 30049, 49, 3 },
                    { 30050, 50, 3 },
                    { 30051, 51, 3 },
                    { 30055, 55, 3 },
                    { 30056, 56, 3 },
                    { 30065, 65, 3 },
                    { 30066, 66, 3 },
                    { 30067, 67, 3 },
                    { 30071, 71, 3 },
                    { 30072, 72, 3 },
                    { 30075, 75, 3 },
                    { 30076, 76, 3 },
                    { 30078, 78, 3 },
                    { 30082, 82, 3 },
                    { 30085, 85, 3 },
                    { 30086, 86, 3 },
                    { 30087, 87, 3 },
                    { 30088, 88, 3 },
                    { 30092, 92, 3 },
                    { 30093, 93, 3 },
                    { 30094, 94, 3 },
                    { 30100, 100, 3 },
                    { 30101, 101, 3 },
                    { 30102, 102, 3 },
                    { 30103, 103, 3 },
                    { 30104, 104, 3 },
                    { 30107, 107, 3 },
                    { 30108, 108, 3 },
                    { 30109, 109, 3 },
                    { 30110, 110, 3 },
                    { 30111, 111, 3 },
                    { 40005, 5, 4 },
                    { 40007, 7, 4 },
                    { 40008, 8, 4 },
                    { 40009, 9, 4 },
                    { 40010, 10, 4 },
                    { 40011, 11, 4 },
                    { 40016, 16, 4 },
                    { 40017, 17, 4 },
                    { 40018, 18, 4 },
                    { 40022, 22, 4 },
                    { 40023, 23, 4 },
                    { 40025, 25, 4 },
                    { 40027, 27, 4 },
                    { 40028, 28, 4 },
                    { 40029, 29, 4 },
                    { 40034, 34, 4 },
                    { 40035, 35, 4 },
                    { 40036, 36, 4 },
                    { 40055, 55, 4 },
                    { 40056, 56, 4 },
                    { 40078, 78, 4 },
                    { 40079, 79, 4 },
                    { 40082, 82, 4 },
                    { 40083, 83, 4 },
                    { 40085, 85, 4 },
                    { 40086, 86, 4 },
                    { 40087, 87, 4 },
                    { 40088, 88, 4 },
                    { 40092, 92, 4 },
                    { 40093, 93, 4 },
                    { 40094, 94, 4 },
                    { 40101, 101, 4 },
                    { 40102, 102, 4 },
                    { 40103, 103, 4 },
                    { 40104, 104, 4 },
                    { 40110, 110, 4 },
                    { 40111, 111, 4 },
                    { 50005, 5, 5 },
                    { 50008, 8, 5 },
                    { 50009, 9, 5 },
                    { 50011, 11, 5 },
                    { 50017, 17, 5 },
                    { 50018, 18, 5 },
                    { 50022, 22, 5 },
                    { 50023, 23, 5 },
                    { 50025, 25, 5 },
                    { 50028, 28, 5 },
                    { 50029, 29, 5 },
                    { 50035, 35, 5 },
                    { 50036, 36, 5 },
                    { 50055, 55, 5 },
                    { 50056, 56, 5 },
                    { 50082, 82, 5 },
                    { 50086, 86, 5 },
                    { 50087, 87, 5 },
                    { 50103, 103, 5 },
                    { 50110, 110, 5 },
                    { 50111, 111, 5 }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "AuthUserRole",
                columns: new[] { "roleID", "userID", "assignedAt" },
                values: new object[,]
                {
                    { 3, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 11, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 12, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 13, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 14, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 15, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 16, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 17, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 18, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 19, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 20, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 21, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 22, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 23, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 24, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 25, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 26, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 27, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 28, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 29, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 30, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 31, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 32, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 33, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 34, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 35, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1, 99, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthAuditLog_userID",
                schema: "auth",
                table: "AuthAuditLog",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthEmailVerification_userID",
                schema: "auth",
                table: "AuthEmailVerification",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthMfaSecret_userID",
                schema: "auth",
                table: "AuthMfaSecret",
                column: "userID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthMfaSecret_userID_type",
                schema: "auth",
                table: "AuthMfaSecret",
                columns: new[] { "userID", "type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthPasswordReset_userID",
                schema: "auth",
                table: "AuthPasswordReset",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthProfile_userID",
                schema: "auth",
                table: "AuthProfile",
                column: "userID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthRefreshToken_sessionID",
                schema: "auth",
                table: "AuthRefreshToken",
                column: "sessionID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthRefreshToken_userID",
                schema: "auth",
                table: "AuthRefreshToken",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthRole_roleName",
                schema: "auth",
                table: "AuthRole",
                column: "roleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthRolePermission_permissionID",
                schema: "auth",
                table: "AuthRolePermission",
                column: "permissionID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthRolePermission_roleID",
                schema: "auth",
                table: "AuthRolePermission",
                column: "roleID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthUser_email",
                schema: "auth",
                table: "AuthUser",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthUser_userName",
                schema: "auth",
                table: "AuthUser",
                column: "userName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthUserRole_roleID",
                schema: "auth",
                table: "AuthUserRole",
                column: "roleID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthUserSession_userID",
                schema: "auth",
                table: "AuthUserSession",
                column: "userID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthAuditLog",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthEmailVerification",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthMfaSecret",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthPasswordReset",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthProfile",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthRefreshToken",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthRolePermission",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthUserRole",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthUserSession",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthPermission",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthRole",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthUser",
                schema: "auth");
        }
    }
}
