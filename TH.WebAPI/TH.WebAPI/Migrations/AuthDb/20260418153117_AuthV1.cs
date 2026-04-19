using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TH.WebAPI.Migrations.AuthDb
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
                    { 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "1", "gd.nguyen@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0901000001", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "gd.nguyen" },
                    { 2, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "1", "pgd.tran@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0901000002", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "pgd.tran" },
                    { 3, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "1", "la.bgd@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0901000003", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "la.bgd" },
                    { 4, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "2", "tp.le.kt@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0902000001", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "tp.le.kt" },
                    { 5, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "2", "kt.tscd1@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0902000002", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "kt.tscd1" },
                    { 6, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "2", "kt.tscd2@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0902000003", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "kt.tscd2" },
                    { 7, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "2", "kt.tscd3@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0902000004", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "kt.tscd3" },
                    { 8, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "2", "nv.kt1@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0902000011", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.kt1" },
                    { 9, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "2", "nv.kt2@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0902000012", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.kt2" },
                    { 10, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "2", "nv.kt3@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0902000013", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.kt3" },
                    { 11, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "2", "nv.kt4@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0902000014", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.kt4" },
                    { 12, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "2", "nv.kt5@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0902000015", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.kt5" },
                    { 13, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "2", "nv.kt6@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0902000016", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.kt6" },
                    { 14, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "2", "nv.kt7@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0902000017", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.kt7" },
                    { 15, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "3", "tp.pham.ns@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0903000001", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "tp.pham.ns" },
                    { 16, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "3", "nv.ns1@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0903000002", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.ns1" },
                    { 17, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "3", "nv.ns2@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0903000003", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.ns2" },
                    { 18, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "3", "nv.ns3@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0903000004", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.ns3" },
                    { 19, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "3", "nv.ns4@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0903000005", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.ns4" },
                    { 20, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "3", "nv.ns5@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0903000006", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.ns5" },
                    { 21, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "3", "nv.ns6@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0903000007", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.ns6" },
                    { 22, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "3", "nv.ns7@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0903000008", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.ns7" },
                    { 23, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "3", "nv.ns8@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0903000009", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.ns8" },
                    { 24, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "3", "nv.ns9@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "09030000010", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.ns9" },
                    { 25, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "3", "nv.ns10@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "09030000011", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "nv.ns10" },
                    { 26, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "4", "tp.vu.kt@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0904000001", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "tp.vu.kt" },
                    { 27, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "4", "ktv1@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0904000002", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "ktv1" },
                    { 28, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "4", "ktv2@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0904000003", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "ktv2" },
                    { 29, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "4", "ktv3@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0904000004", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "ktv3" },
                    { 30, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "4", "ktv4@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0904000005", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "ktv4" },
                    { 31, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "4", "ktv5@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0904000006", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "ktv5" },
                    { 32, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "4", "ktv6@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0904000007", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "ktv6" },
                    { 33, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "4", "ktv7@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0904000008", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "ktv7" },
                    { 34, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "4", "ktv8@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0904000009", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "ktv8" },
                    { 35, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "4", "ktv9@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "09040000010", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "ktv9" },
                    { 36, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "4", "ktv10@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "09040000011", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "ktv10" },
                    { 37, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "5", "tp.hoang.sp@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0905000001", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "tp.hoang.sp" },
                    { 38, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "5", "pm1@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0905000002", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "pm1" },
                    { 39, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "5", "pm2@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0905000003", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "pm2" },
                    { 40, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "5", "pm3@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0905000004", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "pm3" },
                    { 41, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "5", "pm4@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0905000005", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "pm4" },
                    { 42, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "5", "pm5@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0905000006", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "pm5" },
                    { 43, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "5", "pm6@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0905000007", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "pm6" },
                    { 44, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "5", "pm7@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0905000008", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "pm7" },
                    { 45, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "5", "pm8@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0905000009", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "pm8" },
                    { 46, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "5", "pm9@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "09050000010", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "pm9" },
                    { 47, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "5", "pm10@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "09050000011", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "pm10" },
                    { 48, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "6", "tp.minh.dev@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0906000001", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "tp.minh.dev" },
                    { 49, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "6", "dev1@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0906000002", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "dev1" },
                    { 50, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "6", "dev2@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0906000003", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "dev2" },
                    { 51, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "6", "dev3@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0906000004", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "dev3" },
                    { 52, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "6", "dev4@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0906000005", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "dev4" },
                    { 53, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "6", "dev5@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0906000006", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "dev5" },
                    { 54, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "6", "dev6@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0906000007", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "dev6" },
                    { 55, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "6", "dev7@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0906000008", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "dev7" },
                    { 56, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "6", "dev8@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0906000009", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "dev8" },
                    { 57, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "6", "dev9@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "09060000010", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "dev9" },
                    { 58, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "6", "dev10@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "09060000011", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "dev10" },
                    { 59, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "7", "tp.quan.pm@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0907000001", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "tp.quan.pm" },
                    { 60, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "7", "prjm1@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0907000002", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "prjm1" },
                    { 61, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "7", "prjm2@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0907000003", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "prjm2" },
                    { 62, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "7", "prjm3@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0907000004", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "prjm3" },
                    { 63, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "7", "prjm4@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0907000005", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "prjm4" },
                    { 64, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "7", "prjm5@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0907000006", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "prjm5" },
                    { 65, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "7", "prjm6@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0907000007", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "prjm6" },
                    { 66, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "7", "prjm7@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0907000008", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "prjm7" },
                    { 67, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "7", "prjm8@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0907000009", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "prjm8" },
                    { 68, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "7", "prjm9@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "09070000010", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "prjm9" },
                    { 69, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "7", "prjm10@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "09070000011", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "prjm10" },
                    { 70, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "8", "tp.linh.ux@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0908000001", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "tp.linh.ux" },
                    { 71, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "8", "designer1@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0908000002", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "designer1" },
                    { 72, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "8", "designer2@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0908000003", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "designer2" },
                    { 73, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "8", "designer3@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0908000004", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "designer3" },
                    { 74, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "8", "designer4@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0908000005", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "designer4" },
                    { 75, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "8", "designer5@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0908000006", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "designer5" },
                    { 76, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "8", "designer6@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0908000007", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "designer6" },
                    { 77, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "8", "designer7@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0908000008", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "designer7" },
                    { 78, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "8", "designer8@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "0908000009", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "designer8" },
                    { 79, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "8", "designer9@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "09080000010", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "designer9" },
                    { 80, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "8", "designer10@thtech.vn", null, true, "$2a$11$X7vK5Q8Z9Z8Z9Z8Z9Z8Z9OqJ8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z8Z9Z", "09080000011", "staff", "Active", 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(1674), "designer10" }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "AuthProfile",
                columns: new[] { "profileID", "avatar", "dateOfBirth", "firstName", "gender", "lastName", "userID" },
                values: new object[,]
                {
                    { 1, null, null, "Nguyễn Văn", "Nam", "A", 1 },
                    { 2, null, null, "Trần Thị", "Nữ", "B", 2 },
                    { 3, null, null, "Lê Văn", "Nam", "C", 3 },
                    { 4, null, null, "Lê Thị", "Nữ", "D", 4 },
                    { 5, null, null, "Hoàng Văn", "Nam", "E", 5 },
                    { 6, null, null, "Phạm Thị", "Nữ", "F", 6 },
                    { 7, null, null, "Vũ Văn", "Nam", "G", 7 },
                    { 8, null, null, "Nhân viên", "Nam", "KT1", 8 },
                    { 9, null, null, "Nhân viên", "Nữ", "KT2", 9 },
                    { 10, null, null, "Nhân viên", "Nam", "KT3", 10 },
                    { 11, null, null, "Nhân viên", "Nữ", "KT4", 11 },
                    { 12, null, null, "Nhân viên", "Nam", "KT5", 12 },
                    { 13, null, null, "Nhân viên", "Nữ", "KT6", 13 },
                    { 14, null, null, "Nhân viên", "Nam", "KT7", 14 },
                    { 15, null, null, "Phạm Văn", "Nam", "H", 15 },
                    { 16, null, null, "Nhân viên", "Nam", "NS1", 16 },
                    { 17, null, null, "Nhân viên", "Nữ", "NS2", 17 },
                    { 18, null, null, "Nhân viên", "Nam", "NS3", 18 },
                    { 19, null, null, "Nhân viên", "Nữ", "NS4", 19 },
                    { 20, null, null, "Nhân viên", "Nam", "NS5", 20 },
                    { 21, null, null, "Nhân viên", "Nữ", "NS6", 21 },
                    { 22, null, null, "Nhân viên", "Nam", "NS7", 22 },
                    { 23, null, null, "Nhân viên", "Nữ", "NS8", 23 },
                    { 24, null, null, "Nhân viên", "Nam", "NS9", 24 },
                    { 25, null, null, "Nhân viên", "Nữ", "NS10", 25 },
                    { 26, null, null, "Vũ Thị", "Nữ", "I", 26 },
                    { 27, null, null, "Kỹ thuật viên", "Nữ", "1", 27 },
                    { 28, null, null, "Kỹ thuật viên", "Nam", "2", 28 },
                    { 29, null, null, "Kỹ thuật viên", "Nữ", "3", 29 },
                    { 30, null, null, "Kỹ thuật viên", "Nam", "4", 30 },
                    { 31, null, null, "Kỹ thuật viên", "Nữ", "5", 31 },
                    { 32, null, null, "Kỹ thuật viên", "Nam", "6", 32 },
                    { 33, null, null, "Kỹ thuật viên", "Nữ", "7", 33 },
                    { 34, null, null, "Kỹ thuật viên", "Nam", "8", 34 },
                    { 35, null, null, "Kỹ thuật viên", "Nữ", "9", 35 },
                    { 36, null, null, "Kỹ thuật viên", "Nam", "10", 36 },
                    { 37, null, null, "Hoàng Thị", "Nữ", "J", 37 },
                    { 38, null, null, "PM", "Nam", "1", 38 },
                    { 39, null, null, "PM", "Nữ", "2", 39 },
                    { 40, null, null, "PM", "Nam", "3", 40 },
                    { 41, null, null, "PM", "Nữ", "4", 41 },
                    { 42, null, null, "PM", "Nam", "5", 42 },
                    { 43, null, null, "PM", "Nữ", "6", 43 },
                    { 44, null, null, "PM", "Nam", "7", 44 },
                    { 45, null, null, "PM", "Nữ", "8", 45 },
                    { 46, null, null, "PM", "Nam", "9", 46 },
                    { 47, null, null, "PM", "Nữ", "10", 47 },
                    { 48, null, null, "Minh Văn", "Nam", "K", 48 },
                    { 49, null, null, "Developer", "Nữ", "1", 49 },
                    { 50, null, null, "Developer", "Nam", "2", 50 },
                    { 51, null, null, "Developer", "Nữ", "3", 51 },
                    { 52, null, null, "Developer", "Nam", "4", 52 },
                    { 53, null, null, "Developer", "Nữ", "5", 53 },
                    { 54, null, null, "Developer", "Nam", "6", 54 },
                    { 55, null, null, "Developer", "Nữ", "7", 55 },
                    { 56, null, null, "Developer", "Nam", "8", 56 },
                    { 57, null, null, "Developer", "Nữ", "9", 57 },
                    { 58, null, null, "Developer", "Nam", "10", 58 },
                    { 59, null, null, "Quân Văn", "Nam", "L", 59 },
                    { 60, null, null, "Project Manager", "Nam", "1", 60 },
                    { 61, null, null, "Project Manager", "Nữ", "2", 61 },
                    { 62, null, null, "Project Manager", "Nam", "3", 62 },
                    { 63, null, null, "Project Manager", "Nữ", "4", 63 },
                    { 64, null, null, "Project Manager", "Nam", "5", 64 },
                    { 65, null, null, "Project Manager", "Nữ", "6", 65 },
                    { 66, null, null, "Project Manager", "Nam", "7", 66 },
                    { 67, null, null, "Project Manager", "Nữ", "8", 67 },
                    { 68, null, null, "Project Manager", "Nam", "9", 68 },
                    { 69, null, null, "Project Manager", "Nữ", "10", 69 },
                    { 70, null, null, "Linh Thị", "Nữ", "M", 70 },
                    { 71, null, null, "Designer", "Nữ", "1", 71 },
                    { 72, null, null, "Designer", "Nam", "2", 72 },
                    { 73, null, null, "Designer", "Nữ", "3", 73 },
                    { 74, null, null, "Designer", "Nam", "4", 74 },
                    { 75, null, null, "Designer", "Nữ", "5", 75 },
                    { 76, null, null, "Designer", "Nam", "6", 76 },
                    { 77, null, null, "Designer", "Nữ", "7", 77 },
                    { 78, null, null, "Designer", "Nam", "8", 78 },
                    { 79, null, null, "Designer", "Nữ", "9", 79 },
                    { 80, null, null, "Designer", "Nam", "10", 80 }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "AuthUserRole",
                columns: new[] { "roleID", "userID", "assignedAt" },
                values: new object[,]
                {
                    { 6, 1, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 7, 2, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 7, 3, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 3, 4, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 2, 5, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 2, 6, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 2, 7, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 8, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 9, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 10, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 11, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 12, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 13, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 14, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 8, 15, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 10, 15, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 10, 16, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 10, 17, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 10, 18, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 10, 19, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 10, 20, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 10, 21, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 10, 22, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 10, 23, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 10, 24, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 10, 25, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 5, 26, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 4, 27, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 4, 28, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 4, 29, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 4, 30, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 4, 31, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 4, 32, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 4, 33, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 4, 34, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 4, 35, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 4, 36, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 8, 37, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 38, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 39, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 40, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 41, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 42, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 43, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 44, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 45, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 46, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 47, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 8, 48, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 49, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 50, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 51, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 52, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 53, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 54, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 55, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 56, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 57, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 58, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 8, 59, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 60, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 61, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 62, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 63, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 64, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 65, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 66, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 67, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 68, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 69, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 8, 70, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 71, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 72, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 73, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 74, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 75, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 76, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 77, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 78, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 79, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) },
                    { 9, 80, new DateTime(2026, 4, 18, 15, 31, 16, 986, DateTimeKind.Utc).AddTicks(2641) }
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
