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
                table: "AuthRole",
                columns: new[] { "roleID", "isDefault", "roleDescription", "roleName", "scope" },
                values: new object[,]
                {
                    { 1, false, "Quản trị viên hệ thống", "admin_he_thong", "staff" },
                    { 2, false, "Kế toán tài sản cố định", "ke_toan_tscd", "staff" },
                    { 3, false, "Trưởng phòng kế toán", "truong_phong_ke_toan", "staff" },
                    { 4, false, "Kỹ thuật viên", "ky_thuat_vien", "staff" },
                    { 5, false, "Trưởng phòng kỹ thuật", "truong_phong_ky_thuat", "staff" },
                    { 6, false, "Giám đốc", "giam_doc", "staff" },
                    { 7, false, "Phó giám đốc", "pho_giam_doc", "staff" },
                    { 8, false, "Trưởng phòng ban", "truong_phong_ban", "staff" },
                    { 9, true, "Nhân viên", "nhan_vien", "staff" },
                    { 10, false, "Nhân viên nhân sự", "nhan_vien_nhan_su", "staff" }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "AuthUser",
                columns: new[] { "userID", "createdAt", "departmentID", "email", "googleSub", "isEmailVerified", "passwordHash", "phoneNumber", "scope", "status", "tokenVersion", "updatedAt", "userName" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1", "pqhung@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0901234001", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pham.quoc.hung" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1", "tttung@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0901234002", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "tran.thanh.tung" },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1", "nmchau@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0901234003", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "nguyen.minh.chau" },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1", "lqbinh@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0901234004", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "le.quang.binh" },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2", "nthuong.kt@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0902234001", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "nguyen.thi.huong" },
                    { 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2", "lbngoc.kt@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0902234002", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "le.bao.ngoc" },
                    { 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2", "tvhai.kt@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0902234003", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "tran.van.hai" },
                    { 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2", "ptmai.kt@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0902234004", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pham.thi.mai" },
                    { 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2", "vxtruong.kt@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0902234005", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "vo.xuan.truong" },
                    { 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "3", "htha.ns@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0903234001", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "hoang.thu.ha" },
                    { 11, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "3", "btlinh.ns@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0903234002", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "bui.thi.linh" },
                    { 12, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "4", "dvtai.tech@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0904234001", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "do.van.tai" },
                    { 13, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "4", "ncminh.tech@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0904234002", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "nguyen.cong.minh" },
                    { 14, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "4", "htloan.tech@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0904234003", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ha.thi.loan" },
                    { 15, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "5", "lmquan.prod@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0905234001", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ly.minh.quan" },
                    { 16, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "5", "dtbngoc.prod@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0905234002", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "dinh.thi.bich.ngoc" },
                    { 17, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "5", "phphuoc.prod@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0905234003", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "phan.huu.phuoc" },
                    { 18, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "5", "vttdung.prod@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0905234004", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "vu.thi.thuy.dung" },
                    { 19, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "5", "cnanh.prod@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0905234005", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "cao.ngoc.anh" },
                    { 20, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "dhthanh.dev@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0906234001", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "dang.huu.thanh" },
                    { 21, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "nvdung.dev@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0906234002", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "nguyen.van.dung" },
                    { 22, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "ttkoanh.dev@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0906234003", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "tran.thi.kim.oanh" },
                    { 23, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "lhnam.dev@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0906234004", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "le.hoang.nam" },
                    { 24, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "ptanh.dev@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0906234005", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pham.tuan.anh" },
                    { 25, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "vdmanh.dev@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0906234006", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "vu.duc.manh" },
                    { 26, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "hthong.dev@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0906234007", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "hoang.thi.hong" },
                    { 27, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "btphong.dev@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0906234008", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "bui.thanh.phong" },
                    { 28, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "dtdat.dev@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0906234009", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "do.thanh.dat" },
                    { 29, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6", "ltbchau.dev@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0906234010", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "luu.thi.bao.chau" },
                    { 30, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "7", "vtlong.pmo@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0907234001", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "vu.thanh.long" },
                    { 31, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "7", "nbkien.pmo@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0907234002", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "nguyen.ba.kien" },
                    { 32, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "7", "ttphuong.pmo@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0907234003", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "tran.thi.phuong" },
                    { 33, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "8", "pnyen.design@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0908234001", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "phan.ngoc.yen" },
                    { 34, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "8", "ctbich.design@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0908234002", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "cao.thi.bich" },
                    { 35, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "8", "tvkhai.design@thtech.vn", null, true, "$2a$11$hLyVHfCp0KoZ3XU5Qf8C7OeGUv8RqKLNz2wYA1mS3nXbP4dJ6iG6", "0908234003", "staff", "Active", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "truong.van.khai" }
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
                    { 35, null, null, "Trương Văn", "Nam", "Khải", 35 }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "AuthUserRole",
                columns: new[] { "roleID", "userID", "assignedAt" },
                values: new object[,]
                {
                    { 6, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, 11, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 12, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 13, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 14, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, 15, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 16, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 17, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 18, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 19, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, 20, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 21, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 22, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 23, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 24, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 25, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 26, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 27, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 28, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 29, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, 30, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 31, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 32, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, 33, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 34, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 35, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
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
