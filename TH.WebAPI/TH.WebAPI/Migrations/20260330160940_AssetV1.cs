using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TH.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AssetV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "asset");

            migrationBuilder.CreateTable(
                name: "cau_hinh_he_thong",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenCongTy = table.Column<string>(type: "text", nullable: true),
                    MaSoThue = table.Column<string>(type: "text", nullable: true),
                    DiaChi = table.Column<string>(type: "text", nullable: true),
                    TienToChungTu = table.Column<string>(type: "text", nullable: true),
                    SoBatDauChungTu = table.Column<int>(type: "integer", nullable: true),
                    PhuongPhapKhauHaoMacDinh = table.Column<string>(type: "text", nullable: true),
                    TuDongKhauHao = table.Column<bool>(type: "boolean", nullable: true),
                    DinhDangMaTaiSan = table.Column<string>(type: "text", nullable: true),
                    DoDaiMaTaiSan = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cau_hinh_he_thong", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "chung_tu",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaChungTu = table.Column<string>(type: "text", nullable: false),
                    NgayLap = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LoaiChungTu = table.Column<string>(type: "text", nullable: true),
                    MoTa = table.Column<string>(type: "text", nullable: true),
                    TongTien = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    TrangThai = table.Column<string>(type: "text", nullable: true),
                    NguoiLapId = table.Column<int>(type: "integer", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chung_tu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lo_tai_san",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaLo = table.Column<string>(type: "text", nullable: false),
                    SoLuong = table.Column<int>(type: "integer", nullable: true),
                    TongGiaTri = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lo_tai_san", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "phong_ban",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaPhongBan = table.Column<string>(type: "text", nullable: false),
                    TenPhongBan = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phong_ban", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tai_khoan_ke_toan",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaTaiKhoan = table.Column<string>(type: "text", nullable: false),
                    TenTaiKhoan = table.Column<string>(type: "text", nullable: true),
                    LoaiTaiKhoan = table.Column<string>(type: "text", nullable: true),
                    MaTaiKhoanCha = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tai_khoan_ke_toan", x => x.Id);
                    table.UniqueConstraint("AK_tai_khoan_ke_toan_MaTaiKhoan", x => x.MaTaiKhoan);
                    table.ForeignKey(
                        name: "FK_tai_khoan_ke_toan_tai_khoan_ke_toan_MaTaiKhoanCha",
                        column: x => x.MaTaiKhoanCha,
                        principalSchema: "asset",
                        principalTable: "tai_khoan_ke_toan",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "danh_muc_tai_san",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaDanhMuc = table.Column<string>(type: "text", nullable: false),
                    TenDanhMuc = table.Column<string>(type: "text", nullable: true),
                    TienTo = table.Column<string>(type: "text", nullable: true),
                    ThoiGianKhauHao = table.Column<int>(type: "integer", nullable: true),
                    MaTaiKhoan = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_danh_muc_tai_san", x => x.Id);
                    table.ForeignKey(
                        name: "FK_danh_muc_tai_san_tai_khoan_ke_toan_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalSchema: "asset",
                        principalTable: "tai_khoan_ke_toan",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tai_san",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaTaiSan = table.Column<string>(type: "text", nullable: false),
                    TenTaiSan = table.Column<string>(type: "text", nullable: true),
                    DanhMucId = table.Column<int>(type: "integer", nullable: true),
                    LoId = table.Column<int>(type: "integer", nullable: true),
                    SoThuTuTrongLo = table.Column<int>(type: "integer", nullable: true),
                    TongSoTrongLo = table.Column<int>(type: "integer", nullable: true),
                    TrangThai = table.Column<string>(type: "text", nullable: false),
                    SoSeri = table.Column<string>(type: "text", nullable: true),
                    NhaSanXuat = table.Column<string>(type: "text", nullable: true),
                    MoTa = table.Column<string>(type: "text", nullable: true),
                    ThongSoKyThuat = table.Column<string>(type: "jsonb", nullable: true),
                    NgayMua = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NguyenGia = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GiaTriConLai = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    KhauHaoLuyKe = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    KhauHaoHangThang = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    PhuongPhapKhauHao = table.Column<string>(type: "text", nullable: true),
                    ThoiGianKhauHao = table.Column<int>(type: "integer", nullable: true),
                    MaTaiKhoan = table.Column<string>(type: "text", nullable: true),
                    PhongBanId = table.Column<int>(type: "integer", nullable: true),
                    NguoiDungId = table.Column<int>(type: "integer", nullable: true),
                    NgayCapPhat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NgayCapNhat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tai_san", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tai_san_danh_muc_tai_san_DanhMucId",
                        column: x => x.DanhMucId,
                        principalSchema: "asset",
                        principalTable: "danh_muc_tai_san",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tai_san_lo_tai_san_LoId",
                        column: x => x.LoId,
                        principalSchema: "asset",
                        principalTable: "lo_tai_san",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tai_san_phong_ban_PhongBanId",
                        column: x => x.PhongBanId,
                        principalSchema: "asset",
                        principalTable: "phong_ban",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tai_san_tai_khoan_ke_toan_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalSchema: "asset",
                        principalTable: "tai_khoan_ke_toan",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bao_tri_tai_san",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaiSanId = table.Column<int>(type: "integer", nullable: true),
                    NgayThucHien = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LoaiBaoTri = table.Column<string>(type: "text", nullable: true),
                    MoTa = table.Column<string>(type: "text", nullable: true),
                    CoChiPhi = table.Column<bool>(type: "boolean", nullable: true),
                    ChiPhi = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    LoaiChiPhi = table.Column<string>(type: "text", nullable: true),
                    NhaCungCap = table.Column<string>(type: "text", nullable: true),
                    TrangThai = table.Column<string>(type: "text", nullable: true),
                    GhiChu = table.Column<string>(type: "text", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bao_tri_tai_san", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bao_tri_tai_san_tai_san_TaiSanId",
                        column: x => x.TaiSanId,
                        principalSchema: "asset",
                        principalTable: "tai_san",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chi_tiet_chung_tu",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChungTuId = table.Column<int>(type: "integer", nullable: true),
                    TaiKhoanNo = table.Column<string>(type: "text", nullable: true),
                    TaiKhoanCo = table.Column<string>(type: "text", nullable: true),
                    SoTien = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    MoTa = table.Column<string>(type: "text", nullable: true),
                    TaiSanId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chi_tiet_chung_tu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_chi_tiet_chung_tu_chung_tu_ChungTuId",
                        column: x => x.ChungTuId,
                        principalSchema: "asset",
                        principalTable: "chung_tu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chi_tiet_chung_tu_tai_khoan_ke_toan_TaiKhoanCo",
                        column: x => x.TaiKhoanCo,
                        principalSchema: "asset",
                        principalTable: "tai_khoan_ke_toan",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_chi_tiet_chung_tu_tai_khoan_ke_toan_TaiKhoanNo",
                        column: x => x.TaiKhoanNo,
                        principalSchema: "asset",
                        principalTable: "tai_khoan_ke_toan",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_chi_tiet_chung_tu_tai_san_TaiSanId",
                        column: x => x.TaiSanId,
                        principalSchema: "asset",
                        principalTable: "tai_san",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "dieu_chuyen_tai_san",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaiSanId = table.Column<int>(type: "integer", nullable: true),
                    LoaiDieuChuyen = table.Column<string>(type: "text", nullable: true),
                    NgayThucHien = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TuPhongBanId = table.Column<int>(type: "integer", nullable: true),
                    DenPhongBanId = table.Column<int>(type: "integer", nullable: true),
                    TuNguoiDungId = table.Column<int>(type: "integer", nullable: true),
                    DenNguoiDungId = table.Column<int>(type: "integer", nullable: true),
                    TrangThai = table.Column<string>(type: "text", nullable: true),
                    GhiChu = table.Column<string>(type: "text", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dieu_chuyen_tai_san", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dieu_chuyen_tai_san_phong_ban_DenPhongBanId",
                        column: x => x.DenPhongBanId,
                        principalSchema: "asset",
                        principalTable: "phong_ban",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dieu_chuyen_tai_san_phong_ban_TuPhongBanId",
                        column: x => x.TuPhongBanId,
                        principalSchema: "asset",
                        principalTable: "phong_ban",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dieu_chuyen_tai_san_tai_san_TaiSanId",
                        column: x => x.TaiSanId,
                        principalSchema: "asset",
                        principalTable: "tai_san",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lich_su_khau_hao",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaiSanId = table.Column<int>(type: "integer", nullable: true),
                    ChungTuId = table.Column<int>(type: "integer", nullable: true),
                    KyKhauHao = table.Column<string>(type: "text", nullable: true),
                    SoTien = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    LuyKeSauKhauHao = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    ConLaiSauKhauHao = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lich_su_khau_hao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lich_su_khau_hao_chung_tu_ChungTuId",
                        column: x => x.ChungTuId,
                        principalSchema: "asset",
                        principalTable: "chung_tu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lich_su_khau_hao_tai_san_TaiSanId",
                        column: x => x.TaiSanId,
                        principalSchema: "asset",
                        principalTable: "tai_san",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "thanh_ly_tai_san",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaiSanId = table.Column<int>(type: "integer", nullable: true),
                    NgayThanhLy = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NguyenGia = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    KhauHaoLuyKe = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GiaTriConLai = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GiaTriThanhLy = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    LaiLo = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    LyDo = table.Column<string>(type: "text", nullable: true),
                    GhiChu = table.Column<string>(type: "text", nullable: true),
                    TrangThai = table.Column<string>(type: "text", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thanh_ly_tai_san", x => x.Id);
                    table.ForeignKey(
                        name: "FK_thanh_ly_tai_san_tai_san_TaiSanId",
                        column: x => x.TaiSanId,
                        principalSchema: "asset",
                        principalTable: "tai_san",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "cau_hinh_he_thong",
                columns: new[] { "Id", "DiaChi", "DinhDangMaTaiSan", "DoDaiMaTaiSan", "MaSoThue", "PhuongPhapKhauHaoMacDinh", "SoBatDauChungTu", "TenCongTy", "TienToChungTu", "TuDongKhauHao" },
                values: new object[] { 1, null, "{DANH_MUC}-{SO_THU_TU}", 4, null, "DuongThang", 1, "Công ty TNHH ABC", "CT", true });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "chung_tu",
                columns: new[] { "Id", "LoaiChungTu", "MaChungTu", "MoTa", "NgayLap", "NgayTao", "NguoiLapId", "TongTien", "TrangThai" },
                values: new object[,]
                {
                    { 1, "KhauHao", "KH-2025-01", "Khấu hao tài sản cố định tháng 01/2025", new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6143), null, 628094444.37m, "hoan_thanh" },
                    { 2, "ThanhLy", "TL-2025-001", "Thanh lý tài sản LAP-0001", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6304), null, 201000000m, "hoan_thanh" },
                    { 3, "ThanhLy", "TL-2025-002", "Thanh lý tài sản LAP-0002", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6345), null, 77000000m, "hoan_thanh" },
                    { 4, "ThanhLy", "TL-2025-003", "Thanh lý tài sản LAP-0003", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6366), null, 109000000m, "hoan_thanh" },
                    { 5, "ThanhLy", "TL-2025-004", "Thanh lý tài sản OTO-0004", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6438), null, 144000000m, "hoan_thanh" },
                    { 6, "ThanhLy", "TL-2025-005", "Thanh lý tài sản OTO-0005", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6460), null, 89000000m, "hoan_thanh" },
                    { 7, "ThanhLy", "TL-2025-006", "Thanh lý tài sản OTO-0006", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6481), null, 127000000m, "hoan_thanh" },
                    { 8, "ThanhLy", "TL-2025-007", "Thanh lý tài sản LAP-0007", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6498), null, 105000000m, "hoan_thanh" },
                    { 9, "ThanhLy", "TL-2025-008", "Thanh lý tài sản OTO-0008", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6516), null, 127000000m, "hoan_thanh" },
                    { 10, "ThanhLy", "TL-2025-009", "Thanh lý tài sản SRV-0009", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6583), null, 156000000m, "hoan_thanh" },
                    { 11, "ThanhLy", "TL-2025-010", "Thanh lý tài sản LAP-0010", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6605), null, 152000000m, "hoan_thanh" },
                    { 12, "ThanhLy", "TL-2025-011", "Thanh lý tài sản SRV-0011", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6622), null, 48000000m, "hoan_thanh" },
                    { 13, "ThanhLy", "TL-2025-012", "Thanh lý tài sản SRV-0012", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6639), null, 249000000m, "hoan_thanh" },
                    { 14, "ThanhLy", "TL-2025-013", "Thanh lý tài sản SRV-0013", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6683), null, 90000000m, "hoan_thanh" },
                    { 15, "ThanhLy", "TL-2025-014", "Thanh lý tài sản OTO-0014", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6701), null, 165000000m, "hoan_thanh" },
                    { 16, "ThanhLy", "TL-2025-015", "Thanh lý tài sản NET-0015", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6722), null, 67000000m, "hoan_thanh" },
                    { 17, "ThanhLy", "TL-2025-016", "Thanh lý tài sản SRV-0016", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6741), null, 215000000m, "hoan_thanh" },
                    { 18, "ThanhLy", "TL-2025-017", "Thanh lý tài sản NET-0017", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6760), null, 48000000m, "hoan_thanh" },
                    { 19, "ThanhLy", "TL-2025-018", "Thanh lý tài sản SRV-0018", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6870), null, 110000000m, "hoan_thanh" },
                    { 20, "ThanhLy", "TL-2025-019", "Thanh lý tài sản NET-0019", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6888), null, 211000000m, "hoan_thanh" },
                    { 21, "ThanhLy", "TL-2025-020", "Thanh lý tài sản LAP-0020", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6906), null, 86000000m, "hoan_thanh" },
                    { 22, "ThanhLy", "TL-2025-021", "Thanh lý tài sản SRV-0021", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6923), null, 57000000m, "hoan_thanh" },
                    { 23, "ThanhLy", "TL-2025-022", "Thanh lý tài sản NET-0022", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7022), null, 122000000m, "hoan_thanh" },
                    { 24, "ThanhLy", "TL-2025-023", "Thanh lý tài sản LAP-0023", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7054), null, 128000000m, "hoan_thanh" },
                    { 25, "ThanhLy", "TL-2025-024", "Thanh lý tài sản NET-0024", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7132), null, 241000000m, "hoan_thanh" },
                    { 26, "ThanhLy", "TL-2025-025", "Thanh lý tài sản SRV-0025", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7153), null, 38000000m, "hoan_thanh" },
                    { 27, "ThanhLy", "TL-2025-026", "Thanh lý tài sản OTO-0026", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7171), null, 49000000m, "hoan_thanh" },
                    { 28, "ThanhLy", "TL-2025-027", "Thanh lý tài sản OTO-0027", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7238), null, 83000000m, "hoan_thanh" },
                    { 29, "ThanhLy", "TL-2025-028", "Thanh lý tài sản SRV-0028", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7257), null, 80000000m, "hoan_thanh" },
                    { 30, "ThanhLy", "TL-2025-029", "Thanh lý tài sản SRV-0029", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7275), null, 157000000m, "hoan_thanh" },
                    { 31, "ThanhLy", "TL-2025-030", "Thanh lý tài sản OTO-0030", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7292), null, 212000000m, "hoan_thanh" }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "phong_ban",
                columns: new[] { "Id", "MaPhongBan", "TenPhongBan" },
                values: new object[,]
                {
                    { 1, "BGD", "Ban giám đốc" },
                    { 2, "KT", "Phòng kế toán" },
                    { 3, "NS", "Phòng nhân sự" },
                    { 4, "TECH", "Phòng kĩ thuật" },
                    { 5, "PROD", "Phòng sản phẩm" },
                    { 6, "DEV", "Phòng phát triển phần mềm" },
                    { 7, "PMO", "Phòng quản lý dự án" },
                    { 8, "DESIGN", "Phòng thiết kế UI/UX" }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "tai_khoan_ke_toan",
                columns: new[] { "Id", "LoaiTaiKhoan", "MaTaiKhoan", "MaTaiKhoanCha", "TenTaiKhoan" },
                values: new object[,]
                {
                    { 1, "Tài sản", "111", null, "Tiền mặt" },
                    { 2, "Tài sản", "112", null, "Tiền gửi ngân hàng" },
                    { 3, "Nguồn vốn", "331", null, "Phải trả cho người bán" },
                    { 4, "Tài sản", "211", null, "Tài sản cố định hữu hình" },
                    { 11, "Nguồn vốn", "214", null, "Hao mòn tài sản cố định" },
                    { 16, "Chi phí", "627", null, "Chi phí sản xuất chung" },
                    { 17, "Chi phí", "641", null, "Chi phí bán hàng" },
                    { 18, "Chi phí", "642", null, "Chi phí quản lý doanh nghiệp" },
                    { 19, "Chi phí", "811", null, "Chi phí khác (Lỗ thanh lý)" },
                    { 20, "Doanh thu", "711", null, "Thu nhập khác (Lãi thanh lý)" },
                    { 5, "Tài sản", "2111", "211", "Nhà cửa, vật kiến trúc" },
                    { 6, "Tài sản", "2112", "211", "Máy móc, thiết bị" },
                    { 7, "Tài sản", "2113", "211", "Phương tiện vận tải, truyền dẫn" },
                    { 8, "Tài sản", "2114", "211", "Thiết bị, dụng cụ quản lý" },
                    { 9, "Tài sản", "2115", "211", "Cây lâu năm, súc vật" },
                    { 12, "Nguồn vốn", "2141", "214", "Hao mòn TSCĐ hữu hình" }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "danh_muc_tai_san",
                columns: new[] { "Id", "MaDanhMuc", "MaTaiKhoan", "TenDanhMuc", "ThoiGianKhauHao", "TienTo" },
                values: new object[,]
                {
                    { 1, "LAP", "2114", "Máy tính xách tay (Laptop)", 36, "LAP" },
                    { 2, "SRV", "2112", "Hệ thống Server & NAS", 60, "SRV" },
                    { 3, "OTO", "2113", "Phương tiện vận tải (Ô tô)", 120, "OTO" },
                    { 4, "NET", "2112", "Thiết bị mạng (Switch, Router)", 36, "NET" }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "tai_san",
                columns: new[] { "Id", "DanhMucId", "GiaTriConLai", "KhauHaoHangThang", "KhauHaoLuyKe", "LoId", "MaTaiKhoan", "MaTaiSan", "MoTa", "NgayCapNhat", "NgayCapPhat", "NgayMua", "NgayTao", "NguoiDungId", "NguyenGia", "NhaSanXuat", "PhongBanId", "PhuongPhapKhauHao", "SoSeri", "SoThuTuTrongLo", "TenTaiSan", "ThoiGianKhauHao", "ThongSoKyThuat", "TongSoTrongLo", "TrangThai" },
                values: new object[,]
                {
                    { 1, 1, 134000000.04m, 5583333.33m, 66999999.96m, null, "2114", "LAP-0001", null, null, new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6276), null, 201000000m, null, 7, "DuongThang", "SN33419", null, "MacBook Pro 16 (No.1)", 36, null, null, "DaThanhLy" },
                    { 2, 1, 66305555.55m, 2138888.89m, 10694444.45m, null, "2114", "LAP-0002", null, null, new DateTime(2024, 3, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6339), null, 77000000m, null, 6, "DuongThang", "SN87498", null, "Dell Precision 5570 (No.2)", 36, null, null, "DaThanhLy" },
                    { 3, 1, 63583333.30m, 3027777.78m, 45416666.70m, null, "2114", "LAP-0003", null, null, new DateTime(2024, 9, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6361), null, 109000000m, null, 5, "DuongThang", "SN45662", null, "Lenovo ThinkPad P1 (No.3)", 36, null, null, "DaThanhLy" },
                    { 4, 3, 129600000m, 1200000m, 14400000m, null, "2113", "OTO-0004", null, null, new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6433), null, 144000000m, null, 7, "DuongThang", "SN50315", null, "Xe Ô tô VinFast VF8 (No.4)", 120, null, null, "DaThanhLy" },
                    { 5, 3, 82324999.97m, 741666.67m, 6675000.03m, null, "2113", "OTO-0005", null, null, new DateTime(2024, 5, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6452), null, 89000000m, null, 3, "DuongThang", "SN25809", null, "Xe Ô tô VinFast VF8 (No.5)", 120, null, null, "DaThanhLy" },
                    { 6, 3, 114300000.04m, 1058333.33m, 12699999.96m, null, "2113", "OTO-0006", null, null, new DateTime(2024, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6475), null, 127000000m, null, 5, "DuongThang", "SN93758", null, "Xe Ô tô VinFast VF8 (No.6)", 120, null, null, "DaThanhLy" },
                    { 7, 1, 90416666.65m, 2916666.67m, 14583333.35m, null, "2114", "LAP-0007", null, null, new DateTime(2024, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6493), null, 105000000m, null, 1, "DuongThang", "SN79119", null, "Dell Precision 5570 (No.7)", 36, null, null, "DaThanhLy" },
                    { 8, 3, 116416666.70m, 1058333.33m, 10583333.30m, null, "2113", "OTO-0008", null, null, new DateTime(2024, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6511), null, 127000000m, null, 6, "DuongThang", "SN16503", null, "Xe Ô tô VinFast VF8 (No.8)", 120, null, null, "DaThanhLy" },
                    { 9, 2, 122200000m, 2600000m, 33800000m, null, "2112", "SRV-0009", null, null, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6576), null, 156000000m, null, 1, "DuongThang", "SN50975", null, "Cisco Catalyst (No.9)", 60, null, null, "DaThanhLy" },
                    { 10, 1, 114000000.02m, 4222222.22m, 37999999.98m, null, "2114", "LAP-0010", null, null, new DateTime(2024, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6600), null, 152000000m, null, 3, "DuongThang", "SN51703", null, "Asus ROG (No.10)", 36, null, null, "DaThanhLy" },
                    { 11, 2, 34400000m, 800000m, 13600000m, null, "2112", "SRV-0011", null, null, new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6617), null, 48000000m, null, 8, "DuongThang", "SN86445", null, "Synology NAS 8-Bay (No.11)", 60, null, null, "DaThanhLy" },
                    { 12, 2, 190900000m, 4150000m, 58100000m, null, "2112", "SRV-0012", null, null, new DateTime(2024, 10, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6634), null, 249000000m, null, 2, "DuongThang", "SN47264", null, "Synology NAS 8-Bay (No.12)", 60, null, null, "DaThanhLy" },
                    { 13, 2, 82500000m, 1500000m, 7500000m, null, "2112", "SRV-0013", null, null, new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6679), null, 90000000m, null, 7, "DuongThang", "SN98468", null, "Cisco Catalyst (No.13)", 60, null, null, "DaThanhLy" },
                    { 14, 3, 149875000m, 1375000m, 15125000m, null, "2113", "OTO-0014", null, null, new DateTime(2024, 9, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6696), null, 165000000m, null, 5, "DuongThang", "SN48708", null, "Xe Ô tô VinFast VF8 (No.14)", 120, null, null, "DaThanhLy" },
                    { 15, 4, 55833333.34m, 1861111.11m, 11166666.66m, null, "2112", "NET-0015", null, null, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6718), null, 67000000m, null, 4, "DuongThang", "SN41057", null, "Synology NAS 8-Bay (No.15)", 36, null, null, "DaThanhLy" },
                    { 16, 2, 161250000.05m, 3583333.33m, 53749999.95m, null, "2112", "SRV-0016", null, null, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6735), null, 215000000m, null, 5, "DuongThang", "SN55623", null, "HP ProLiant DL380 (No.16)", 60, null, null, "DaThanhLy" },
                    { 17, 4, 32000000.04m, 1333333.33m, 15999999.96m, null, "2112", "NET-0017", null, null, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6753), null, 48000000m, null, 3, "DuongThang", "SN80599", null, "Dell PowerEdge R740 (No.17)", 36, null, null, "DaThanhLy" },
                    { 18, 2, 80666666.72m, 1833333.33m, 29333333.28m, null, "2112", "SRV-0018", null, null, new DateTime(2024, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6864), null, 110000000m, null, 5, "DuongThang", "SN65282", null, "Dell PowerEdge R740 (No.18)", 60, null, null, "DaThanhLy" },
                    { 19, 4, 111361111.13m, 5861111.11m, 99638888.87m, null, "2112", "NET-0019", null, null, new DateTime(2024, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6883), null, 211000000m, null, 2, "DuongThang", "SN18274", null, "Dell PowerEdge R740 (No.19)", 36, null, null, "DaThanhLy" },
                    { 20, 1, 71666666.66m, 2388888.89m, 14333333.34m, null, "2114", "LAP-0020", null, null, new DateTime(2024, 7, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6901), null, 86000000m, null, 8, "DuongThang", "SN31696", null, "Asus ROG (No.20)", 36, null, null, "DaThanhLy" },
                    { 21, 2, 52250000m, 950000m, 4750000m, null, "2112", "SRV-0021", null, null, new DateTime(2024, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6918), null, 57000000m, null, 3, "DuongThang", "SN87295", null, "HP ProLiant DL380 (No.21)", 60, null, null, "DaThanhLy" },
                    { 22, 4, 74555555.54m, 3388888.89m, 47444444.46m, null, "2112", "NET-0022", null, null, new DateTime(2024, 3, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7014), null, 122000000m, null, 5, "DuongThang", "SN63052", null, "Dell PowerEdge R740 (No.22)", 36, null, null, "DaThanhLy" },
                    { 23, 1, 85333333.28m, 3555555.56m, 42666666.72m, null, "2114", "LAP-0023", null, null, new DateTime(2024, 10, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7046), null, 128000000m, null, 6, "DuongThang", "SN86845", null, "MacBook Pro 16 (No.23)", 36, null, null, "DaThanhLy" },
                    { 24, 4, 167361111.16m, 6694444.44m, 73638888.84m, null, "2112", "NET-0024", null, null, new DateTime(2024, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7123), null, 241000000m, null, 4, "DuongThang", "SN14458", null, "Dell PowerEdge R740 (No.24)", 36, null, null, "DaThanhLy" },
                    { 25, 2, 32300000.03m, 633333.33m, 5699999.97m, null, "2112", "SRV-0025", null, null, new DateTime(2024, 4, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7148), null, 38000000m, null, 1, "DuongThang", "SN70360", null, "Dell PowerEdge R740 (No.25)", 60, null, null, "DaThanhLy" },
                    { 26, 3, 43283333.38m, 408333.33m, 5716666.62m, null, "2113", "OTO-0026", null, null, new DateTime(2024, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7166), null, 49000000m, null, 3, "DuongThang", "SN96921", null, "Xe Ô tô VinFast VF8 (No.26)", 120, null, null, "DaThanhLy" },
                    { 27, 3, 70549999.94m, 691666.67m, 12450000.06m, null, "2113", "OTO-0027", null, null, new DateTime(2024, 3, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7233), null, 83000000m, null, 8, "DuongThang", "SN23073", null, "Xe Ô tô VinFast VF8 (No.27)", 120, null, null, "DaThanhLy" },
                    { 28, 2, 73333333.35m, 1333333.33m, 6666666.65m, null, "2112", "SRV-0028", null, null, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7252), null, 80000000m, null, 1, "DuongThang", "SN20378", null, "HP ProLiant DL380 (No.28)", 60, null, null, "DaThanhLy" },
                    { 29, 2, 125599999.96m, 2616666.67m, 31400000.04m, null, "2112", "SRV-0029", null, null, new DateTime(2024, 11, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7270), null, 157000000m, null, 5, "DuongThang", "SN45371", null, "Dell PowerEdge R740 (No.29)", 60, null, null, "DaThanhLy" },
                    { 30, 3, 178433333.27m, 1766666.67m, 33566666.73m, null, "2113", "OTO-0030", null, null, new DateTime(2024, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7288), null, 212000000m, null, 5, "DuongThang", "SN45075", null, "Xe Ô tô VinFast VF8 (No.30)", 120, null, null, "DaThanhLy" },
                    { 31, 4, 79333333.30m, 3777777.78m, 56666666.70m, null, "2112", "NET-0031", null, null, new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7306), 48, 136000000m, null, 3, "DuongThang", "SN36529", null, "HP ProLiant DL380 (No.31)", 36, null, null, "DangSuDung" },
                    { 32, 1, 135333333.32m, 5638888.89m, 67666666.68m, null, "2114", "LAP-0032", null, null, new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7329), 49, 203000000m, null, 7, "DuongThang", "SN77841", null, "MacBook Pro 16 (No.32)", 36, null, null, "DangSuDung" },
                    { 33, 3, 209950000.06m, 2058333.33m, 37049999.94m, null, "2113", "OTO-0033", null, null, new DateTime(2024, 6, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7388), 29, 247000000m, null, 8, "DuongThang", "SN61121", null, "Xe Ô tô VinFast VF8 (No.33)", 120, null, null, "DangSuDung" },
                    { 34, 2, 171600000m, 3300000m, 26400000m, null, "2112", "SRV-0034", null, null, new DateTime(2024, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7403), 46, 198000000m, null, 7, "DuongThang", "SN94283", null, "Dell PowerEdge R740 (No.34)", 60, null, null, "DangSuDung" },
                    { 35, 3, 121200000m, 1200000m, 22800000m, null, "2113", "OTO-0035", null, null, new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7416), 24, 144000000m, null, 7, "DuongThang", "SN11486", null, "Xe Ô tô VinFast VF8 (No.35)", 120, null, null, "DangSuDung" },
                    { 36, 3, 203400000m, 1800000m, 12600000m, null, "2113", "OTO-0036", null, null, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7429), 34, 216000000m, null, 4, "DuongThang", "SN18403", null, "Xe Ô tô VinFast VF8 (No.36)", 120, null, null, "DangSuDung" },
                    { 37, 3, 183683333.29m, 1716666.67m, 22316666.71m, null, "2113", "OTO-0037", null, null, new DateTime(2024, 9, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7443), 10, 206000000m, null, 1, "DuongThang", "SN10703", null, "Xe Ô tô VinFast VF8 (No.37)", 120, null, null, "DangSuDung" },
                    { 38, 3, 82416666.65m, 716666.67m, 3583333.35m, null, "2113", "OTO-0038", null, null, new DateTime(2024, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7455), 44, 86000000m, null, 7, "DuongThang", "SN31066", null, "Xe Ô tô VinFast VF8 (No.38)", 120, null, null, "DangSuDung" },
                    { 39, 1, 175833333.34m, 5861111.11m, 35166666.66m, null, "2114", "LAP-0039", null, null, new DateTime(2024, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7468), 42, 211000000m, null, 2, "DuongThang", "SN91294", null, "Lenovo ThinkPad P1 (No.39)", 36, null, null, "DangSuDung" },
                    { 40, 3, 98050000.03m, 883333.33m, 7949999.97m, null, "2113", "OTO-0040", null, null, new DateTime(2024, 8, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7530), 7, 106000000m, null, 4, "DuongThang", "SN20774", null, "Xe Ô tô VinFast VF8 (No.40)", 120, null, null, "DangSuDung" },
                    { 41, 1, 127083333.37m, 5083333.33m, 55916666.63m, null, "2114", "LAP-0041", null, null, new DateTime(2024, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7544), 12, 183000000m, null, 6, "DuongThang", "SN61566", null, "MacBook Pro 16 (No.41)", 36, null, null, "DangSuDung" },
                    { 42, 4, 25972222.18m, 1527777.78m, 29027777.82m, null, "2112", "NET-0042", null, null, new DateTime(2024, 3, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7557), 16, 55000000m, null, 1, "DuongThang", "SN89298", null, "Synology NAS 8-Bay (No.42)", 36, null, null, "DangSuDung" },
                    { 43, 2, 46000000m, 1000000m, 14000000m, null, "2112", "SRV-0043", null, null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7569), 31, 60000000m, null, 4, "DuongThang", "SN74660", null, "HP ProLiant DL380 (No.43)", 60, null, null, "DangSuDung" },
                    { 44, 3, 151925000m, 1475000m, 25075000m, null, "2113", "OTO-0044", null, null, new DateTime(2024, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7581), 16, 177000000m, null, 7, "DuongThang", "SN48435", null, "Xe Ô tô VinFast VF8 (No.44)", 120, null, null, "DangSuDung" },
                    { 45, 3, 115825000m, 1025000m, 7175000m, null, "2113", "OTO-0045", null, null, new DateTime(2024, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7593), 12, 123000000m, null, 3, "DuongThang", "SN14333", null, "Xe Ô tô VinFast VF8 (No.45)", 120, null, null, "DangSuDung" },
                    { 46, 4, 161000000m, 5750000m, 46000000m, null, "2112", "NET-0046", null, null, new DateTime(2024, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7606), 6, 207000000m, null, 3, "DuongThang", "SN75289", null, "Cisco Catalyst (No.46)", 36, null, null, "DangSuDung" },
                    { 47, 4, 119000000m, 4250000m, 34000000m, null, "2112", "NET-0047", null, null, new DateTime(2024, 11, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7664), 14, 153000000m, null, 2, "DuongThang", "SN35651", null, "Synology NAS 8-Bay (No.47)", 36, null, null, "DangSuDung" },
                    { 48, 4, 85222222.20m, 3277777.78m, 32777777.80m, null, "2112", "NET-0048", null, null, new DateTime(2024, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7679), 11, 118000000m, null, 5, "DuongThang", "SN15108", null, "Cisco Catalyst (No.48)", 36, null, null, "DangSuDung" },
                    { 49, 2, 46933333.28m, 1066666.67m, 17066666.72m, null, "2112", "SRV-0049", null, null, new DateTime(2024, 5, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7692), 48, 64000000m, null, 4, "DuongThang", "SN33200", null, "Dell PowerEdge R740 (No.49)", 60, null, null, "DangSuDung" },
                    { 50, 4, 38277777.80m, 1472222.22m, 14722222.20m, null, "2112", "NET-0050", null, null, new DateTime(2024, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7705), 17, 53000000m, null, 8, "DuongThang", "SN26063", null, "HP ProLiant DL380 (No.50)", 36, null, null, "DangSuDung" },
                    { 51, 4, 108888888.88m, 3888888.89m, 31111111.12m, null, "2112", "NET-0051", null, null, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7718), 13, 140000000m, null, 1, "DuongThang", "SN32445", null, "HP ProLiant DL380 (No.51)", 36, null, null, "DangSuDung" },
                    { 52, 3, 138133333.36m, 1233333.33m, 9866666.64m, null, "2113", "OTO-0052", null, null, new DateTime(2024, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7731), 31, 148000000m, null, 4, "DuongThang", "SN38499", null, "Xe Ô tô VinFast VF8 (No.52)", 120, null, null, "DangSuDung" },
                    { 53, 3, 145350000m, 1275000m, 7650000m, null, "2113", "OTO-0053", null, null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7743), 43, 153000000m, null, 8, "DuongThang", "SN83011", null, "Xe Ô tô VinFast VF8 (No.53)", 120, null, null, "DangSuDung" },
                    { 54, 1, 105333333.32m, 4388888.89m, 52666666.68m, null, "2114", "LAP-0054", null, null, new DateTime(2024, 5, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7755), 16, 158000000m, null, 4, "DuongThang", "SN44851", null, "Lenovo ThinkPad P1 (No.54)", 36, null, null, "DangSuDung" },
                    { 55, 3, 205200000m, 1800000m, 10800000m, null, "2113", "OTO-0055", null, null, new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7813), 36, 216000000m, null, 3, "DuongThang", "SN94939", null, "Xe Ô tô VinFast VF8 (No.55)", 120, null, null, "DangSuDung" },
                    { 56, 3, 176750000.05m, 1683333.33m, 25249999.95m, null, "2113", "OTO-0056", null, null, new DateTime(2024, 7, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7826), 7, 202000000m, null, 3, "DuongThang", "SN62162", null, "Xe Ô tô VinFast VF8 (No.56)", 120, null, null, "DangSuDung" },
                    { 57, 3, 107891666.71m, 1008333.33m, 13108333.29m, null, "2113", "OTO-0057", null, null, new DateTime(2024, 3, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7838), 13, 121000000m, null, 2, "DuongThang", "SN26562", null, "Xe Ô tô VinFast VF8 (No.57)", 120, null, null, "DangSuDung" },
                    { 58, 3, 51300000m, 450000m, 2700000m, null, "2113", "OTO-0058", null, null, new DateTime(2024, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7850), 39, 54000000m, null, 7, "DuongThang", "SN83329", null, "Xe Ô tô VinFast VF8 (No.58)", 120, null, null, "DangSuDung" },
                    { 59, 2, 113900000.03m, 2233333.33m, 20099999.97m, null, "2112", "SRV-0059", null, null, new DateTime(2024, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7862), 44, 134000000m, null, 2, "DuongThang", "SN37404", null, "HP ProLiant DL380 (No.59)", 60, null, null, "DangSuDung" },
                    { 60, 1, 89249999.96m, 3305555.56m, 29750000.04m, null, "2114", "LAP-0060", null, null, new DateTime(2024, 10, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7876), 45, 119000000m, null, 7, "DuongThang", "SN11499", null, "Lenovo ThinkPad P1 (No.60)", 36, null, null, "DangSuDung" },
                    { 61, 4, 152777777.84m, 6944444.44m, 97222222.16m, null, "2112", "NET-0061", null, null, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7889), 44, 250000000m, null, 7, "DuongThang", "SN62595", null, "Cisco Catalyst (No.61)", 36, null, null, "DangSuDung" },
                    { 62, 4, 109555555.64m, 6444444.44m, 122444444.36m, null, "2112", "NET-0062", null, null, new DateTime(2024, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7901), 31, 232000000m, null, 4, "DuongThang", "SN11335", null, "Synology NAS 8-Bay (No.62)", 36, null, null, "DangSuDung" },
                    { 63, 1, 168000000.02m, 6222222.22m, 55999999.98m, null, "2114", "LAP-0063", null, null, new DateTime(2024, 11, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7940), 31, 224000000m, null, 1, "DuongThang", "SN44788", null, "Dell Precision 5570 (No.63)", 36, null, null, "DangSuDung" },
                    { 64, 4, 65333333.36m, 2333333.33m, 18666666.64m, null, "2112", "NET-0064", null, null, new DateTime(2024, 9, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7955), 12, 84000000m, null, 8, "DuongThang", "SN74768", null, "HP ProLiant DL380 (No.64)", 36, null, null, "DangSuDung" },
                    { 65, 4, 92277777.84m, 4194444.44m, 58722222.16m, null, "2112", "NET-0065", null, null, new DateTime(2024, 7, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7968), 12, 151000000m, null, 3, "DuongThang", "SN64985", null, "Cisco Catalyst (No.65)", 36, null, null, "DangSuDung" },
                    { 66, 2, 153066666.73m, 3733333.33m, 70933333.27m, null, "2112", "SRV-0066", null, null, new DateTime(2024, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7982), 43, 224000000m, null, 7, "DuongThang", "SN53999", null, "Dell PowerEdge R740 (No.66)", 60, null, null, "DangSuDung" },
                    { 67, 1, 48638888.91m, 2861111.11m, 54361111.09m, null, "2114", "LAP-0067", null, null, new DateTime(2024, 5, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7994), 33, 103000000m, null, 6, "DuongThang", "SN26682", null, "Lenovo ThinkPad P1 (No.67)", 36, null, null, "DangSuDung" },
                    { 68, 2, 82599999.94m, 1966666.67m, 35400000.06m, null, "2112", "SRV-0068", null, null, new DateTime(2024, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8006), 7, 118000000m, null, 5, "DuongThang", "SN31802", null, "Synology NAS 8-Bay (No.68)", 60, null, null, "DangSuDung" },
                    { 69, 2, 37600000.04m, 783333.33m, 9399999.96m, null, "2112", "SRV-0069", null, null, new DateTime(2024, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8105), 37, 47000000m, null, 5, "DuongThang", "SN20565", null, "HP ProLiant DL380 (No.69)", 60, null, null, "DangSuDung" },
                    { 70, 3, 126258333.37m, 1158333.33m, 12741666.63m, null, "2113", "OTO-0070", null, null, new DateTime(2024, 6, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8119), 20, 139000000m, null, 4, "DuongThang", "SN44701", null, "Xe Ô tô VinFast VF8 (No.70)", 120, null, null, "DangSuDung" },
                    { 71, 2, 54466666.61m, 1266666.67m, 21533333.39m, null, "2112", "SRV-0071", null, null, new DateTime(2024, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8132), 10, 76000000m, null, 8, "DuongThang", "SN58706", null, "Cisco Catalyst (No.71)", 60, null, null, "DangSuDung" },
                    { 72, 1, 176999999.96m, 6555555.56m, 59000000.04m, null, "2114", "LAP-0072", null, null, new DateTime(2024, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8144), 12, 236000000m, null, 2, "DuongThang", "SN36058", null, "Dell Precision 5570 (No.72)", 36, null, null, "DangSuDung" },
                    { 73, 2, 172000000m, 4000000m, 68000000m, null, "2112", "SRV-0073", null, null, new DateTime(2024, 7, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8155), 29, 240000000m, null, 1, "DuongThang", "SN52518", null, "Synology NAS 8-Bay (No.73)", 60, null, null, "DangSuDung" },
                    { 74, 3, 60349999.94m, 591666.67m, 10650000.06m, null, "2113", "OTO-0074", null, null, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8168), 21, 71000000m, null, 4, "DuongThang", "SN82546", null, "Xe Ô tô VinFast VF8 (No.74)", 120, null, null, "DangSuDung" },
                    { 75, 1, 72500000m, 2500000m, 17500000m, null, "2114", "LAP-0075", null, null, new DateTime(2024, 11, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8179), 18, 90000000m, null, 5, "DuongThang", "SN18071", null, "Lenovo ThinkPad P1 (No.75)", 36, null, null, "DangSuDung" },
                    { 76, 1, 120000000.04m, 4444444.44m, 39999999.96m, null, "2114", "LAP-0076", null, null, new DateTime(2024, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8192), 44, 160000000m, null, 1, "DuongThang", "SN15839", null, "MacBook Pro 16 (No.76)", 36, null, null, "DangSuDung" },
                    { 77, 2, 75600000m, 1400000m, 8400000m, null, "2112", "SRV-0077", null, null, new DateTime(2024, 6, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8249), 13, 84000000m, null, 3, "DuongThang", "SN38596", null, "Synology NAS 8-Bay (No.77)", 60, null, null, "DangSuDung" },
                    { 78, 2, 30216666.63m, 616666.67m, 6783333.37m, null, "2112", "SRV-0078", null, null, new DateTime(2024, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8262), 9, 37000000m, null, 8, "DuongThang", "SN79019", null, "HP ProLiant DL380 (No.78)", 60, null, null, "DangSuDung" },
                    { 79, 1, 57638888.84m, 2305555.56m, 25361111.16m, null, "2114", "LAP-0079", null, null, new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8274), 43, 83000000m, null, 4, "DuongThang", "SN78204", null, "Dell Precision 5570 (No.79)", 36, null, null, "DangSuDung" },
                    { 80, 4, 200583333.31m, 6916666.67m, 48416666.69m, null, "2112", "NET-0080", null, null, new DateTime(2024, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8286), 37, 249000000m, null, 3, "DuongThang", "SN80772", null, "HP ProLiant DL380 (No.80)", 36, null, null, "DangSuDung" },
                    { 81, 3, 103500000m, 900000m, 4500000m, null, "2113", "OTO-0081", null, null, new DateTime(2024, 3, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8298), 33, 108000000m, null, 8, "DuongThang", "SN91324", null, "Xe Ô tô VinFast VF8 (No.81)", 120, null, null, "DangSuDung" },
                    { 82, 1, 19249999.95m, 916666.67m, 13750000.05m, null, "2114", "LAP-0082", null, null, new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8311), 42, 33000000m, null, 4, "DuongThang", "SN83050", null, "Lenovo ThinkPad P1 (No.82)", 36, null, null, "DangSuDung" },
                    { 83, 4, 95527777.74m, 5027777.78m, 85472222.26m, null, "2112", "NET-0083", null, null, new DateTime(2024, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8323), 4, 181000000m, null, 7, "DuongThang", "SN14447", null, "Dell PowerEdge R740 (No.83)", 36, null, null, "DangSuDung" },
                    { 84, 2, 111750000.05m, 2483333.33m, 37249999.95m, null, "2112", "SRV-0084", null, null, new DateTime(2024, 2, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8335), 4, 149000000m, null, 4, "DuongThang", "SN62275", null, "Dell PowerEdge R740 (No.84)", 60, null, null, "DangSuDung" },
                    { 85, 2, 137500000m, 2750000m, 27500000m, null, "2112", "SRV-0085", null, null, new DateTime(2024, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8396), 11, 165000000m, null, 3, "DuongThang", "SN71815", null, "Dell PowerEdge R740 (No.85)", 60, null, null, "DangSuDung" },
                    { 86, 3, 137800000m, 1300000m, 18200000m, null, "2113", "OTO-0086", null, null, new DateTime(2024, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8408), 45, 156000000m, null, 5, "DuongThang", "SN74805", null, "Xe Ô tô VinFast VF8 (No.86)", 120, null, null, "DangSuDung" },
                    { 87, 1, 147333333.32m, 6138888.89m, 73666666.68m, null, "2114", "LAP-0087", null, null, new DateTime(2024, 10, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8421), 26, 221000000m, null, 1, "DuongThang", "SN47500", null, "Lenovo ThinkPad P1 (No.87)", 36, null, null, "DangSuDung" },
                    { 88, 2, 136300000m, 2900000m, 37700000m, null, "2112", "SRV-0088", null, null, new DateTime(2024, 9, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8432), 30, 174000000m, null, 8, "DuongThang", "SN24334", null, "HP ProLiant DL380 (No.88)", 60, null, null, "DangSuDung" },
                    { 89, 1, 143055555.58m, 5722222.22m, 62944444.42m, null, "2114", "LAP-0089", null, null, new DateTime(2024, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8445), 18, 206000000m, null, 1, "DuongThang", "SN46240", null, "Dell Precision 5570 (No.89)", 36, null, null, "DangSuDung" },
                    { 90, 2, 61500000m, 1500000m, 28500000m, null, "2112", "SRV-0090", null, null, new DateTime(2024, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8456), 9, 90000000m, null, 4, "DuongThang", "SN23277", null, "Synology NAS 8-Bay (No.90)", 60, null, null, "DangSuDung" },
                    { 91, 3, 124200000m, 1150000m, 13800000m, null, "2113", "OTO-0091", null, null, new DateTime(2024, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8468), 25, 138000000m, null, 3, "DuongThang", "SN64120", null, "Xe Ô tô VinFast VF8 (No.91)", 120, null, null, "DangSuDung" },
                    { 92, 1, 101583333.29m, 4416666.67m, 57416666.71m, null, "2114", "LAP-0092", null, null, new DateTime(2024, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8480), 41, 159000000m, null, 1, "DuongThang", "SN19720", null, "MacBook Pro 16 (No.92)", 36, null, null, "DangSuDung" },
                    { 93, 4, 60000000m, 3000000m, 48000000m, null, "2112", "NET-0093", null, null, new DateTime(2024, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8493), 16, 108000000m, null, 7, "DuongThang", "SN25833", null, "Synology NAS 8-Bay (No.93)", 36, null, null, "DangSuDung" },
                    { 94, 1, 59611111.08m, 2055555.56m, 14388888.92m, null, "2114", "LAP-0094", null, null, new DateTime(2024, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8550), 43, 74000000m, null, 5, "DuongThang", "SN92644", null, "Dell Precision 5570 (No.94)", 36, null, null, "DangSuDung" },
                    { 95, 3, 219600000.04m, 2033333.33m, 24399999.96m, null, "2113", "OTO-0095", null, null, new DateTime(2024, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8563), 28, 244000000m, null, 2, "DuongThang", "SN63406", null, "Xe Ô tô VinFast VF8 (No.95)", 120, null, null, "DangSuDung" },
                    { 96, 4, 182861111.08m, 6305555.56m, 44138888.92m, null, "2112", "NET-0096", null, null, new DateTime(2024, 5, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8578), 27, 227000000m, null, 7, "DuongThang", "SN38559", null, "Cisco Catalyst (No.96)", 36, null, null, "DangSuDung" },
                    { 97, 4, 90000000m, 4500000m, 72000000m, null, "2112", "NET-0097", null, null, new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8590), 42, 162000000m, null, 2, "DuongThang", "SN75195", null, "Cisco Catalyst (No.97)", 36, null, null, "DangSuDung" },
                    { 98, 2, 64916666.73m, 1583333.33m, 30083333.27m, null, "2112", "SRV-0098", null, null, new DateTime(2024, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8603), 37, 95000000m, null, 6, "DuongThang", "SN69264", null, "Cisco Catalyst (No.98)", 60, null, null, "DangSuDung" },
                    { 99, 1, 45833333.32m, 1527777.78m, 9166666.68m, null, "2114", "LAP-0099", null, null, new DateTime(2024, 2, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8615), 13, 55000000m, null, 2, "DuongThang", "SN52870", null, "Dell Precision 5570 (No.99)", 36, null, null, "DangSuDung" },
                    { 100, 2, 184166666.70m, 3683333.33m, 36833333.30m, null, "2112", "SRV-0100", null, null, new DateTime(2024, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8629), 40, 221000000m, null, 1, "DuongThang", "SN21473", null, "Cisco Catalyst (No.100)", 60, null, null, "DangSuDung" },
                    { 101, 2, 149783333.39m, 3483333.33m, 59216666.61m, null, "2112", "SRV-0101", null, null, new DateTime(2024, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8689), 17, 209000000m, null, 7, "DuongThang", "SN34986", null, "Cisco Catalyst (No.101)", 60, null, null, "DangSuDung" },
                    { 102, 2, 55650000m, 1050000m, 7350000m, null, "2112", "SRV-0102", null, null, new DateTime(2024, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8701), 2, 63000000m, null, 7, "DuongThang", "SN41814", null, "Cisco Catalyst (No.102)", 60, null, null, "DangSuDung" },
                    { 103, 1, 127777777.79m, 5111111.11m, 56222222.21m, null, "2114", "LAP-0103", null, null, new DateTime(2024, 8, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8714), 29, 184000000m, null, 8, "DuongThang", "SN78306", null, "Lenovo ThinkPad P1 (No.103)", 36, null, null, "DangSuDung" },
                    { 104, 4, 33444444.48m, 1194444.44m, 9555555.52m, null, "2112", "NET-0104", null, null, new DateTime(2024, 7, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8726), 36, 43000000m, null, 6, "DuongThang", "SN84457", null, "Dell PowerEdge R740 (No.104)", 36, null, null, "DangSuDung" },
                    { 105, 4, 42000000m, 1500000m, 12000000m, null, "2112", "NET-0105", null, null, new DateTime(2024, 10, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8738), 31, 54000000m, null, 2, "DuongThang", "SN13506", null, "HP ProLiant DL380 (No.105)", 36, null, null, "DangSuDung" },
                    { 106, 2, 207000000.02m, 3833333.33m, 22999999.98m, null, "2112", "SRV-0106", null, null, new DateTime(2024, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8750), 10, 230000000m, null, 5, "DuongThang", "SN44084", null, "Dell PowerEdge R740 (No.106)", 60, null, null, "DangSuDung" },
                    { 107, 2, 76683333.39m, 1783333.33m, 30316666.61m, null, "2112", "SRV-0107", null, null, new DateTime(2024, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8762), 9, 107000000m, null, 4, "DuongThang", "SN15936", null, "Cisco Catalyst (No.107)", 60, null, null, "DangSuDung" },
                    { 108, 1, 36805555.58m, 1472222.22m, 16194444.42m, null, "2114", "LAP-0108", null, null, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8775), 27, 53000000m, null, 2, "DuongThang", "SN93392", null, "MacBook Pro 16 (No.108)", 36, null, null, "DangSuDung" },
                    { 109, 1, 26194444.43m, 1138888.89m, 14805555.57m, null, "2114", "LAP-0109", null, null, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8814), 26, 41000000m, null, 6, "DuongThang", "SN76666", null, "Asus ROG (No.109)", 36, null, null, "DangSuDung" },
                    { 110, 4, 145333333.28m, 6055555.56m, 72666666.72m, null, "2112", "NET-0110", null, null, new DateTime(2024, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8828), 25, 218000000m, null, 6, "DuongThang", "SN94267", null, "Cisco Catalyst (No.110)", 36, null, null, "DangSuDung" },
                    { 111, 2, 45233333.38m, 983333.33m, 13766666.62m, null, "2112", "SRV-0111", null, null, new DateTime(2024, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8840), 8, 59000000m, null, 8, "DuongThang", "SN81684", null, "Dell PowerEdge R740 (No.111)", 60, null, null, "DangSuDung" },
                    { 112, 4, 91000000.05m, 4333333.33m, 64999999.95m, null, "2112", "NET-0112", null, null, new DateTime(2024, 8, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8853), 7, 156000000m, null, 7, "DuongThang", "SN20902", null, "Cisco Catalyst (No.112)", 36, null, null, "DangSuDung" },
                    { 113, 2, 88933333.38m, 1933333.33m, 27066666.62m, null, "2112", "SRV-0113", null, null, new DateTime(2024, 8, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8865), 43, 116000000m, null, 7, "DuongThang", "SN88713", null, "Cisco Catalyst (No.113)", 60, null, null, "DangSuDung" },
                    { 114, 1, 102000000m, 4250000m, 51000000m, null, "2114", "LAP-0114", null, null, new DateTime(2024, 3, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8876), 8, 153000000m, null, 3, "DuongThang", "SN37721", null, "Lenovo ThinkPad P1 (No.114)", 36, null, null, "DangSuDung" },
                    { 115, 4, 173333333.32m, 5777777.78m, 34666666.68m, null, "2112", "NET-0115", null, null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8889), 23, 208000000m, null, 7, "DuongThang", "SN52378", null, "Synology NAS 8-Bay (No.115)", 36, null, null, "DangSuDung" },
                    { 116, 1, 147250000m, 4750000m, 23750000m, null, "2114", "LAP-0116", null, null, new DateTime(2024, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8901), 43, 171000000m, null, 1, "DuongThang", "SN37024", null, "MacBook Pro 16 (No.116)", 36, null, null, "DangSuDung" },
                    { 117, 3, 60124999.97m, 541666.67m, 4875000.03m, null, "2113", "OTO-0117", null, null, new DateTime(2024, 2, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8988), 46, 65000000m, null, 8, "DuongThang", "SN97652", null, "Xe Ô tô VinFast VF8 (No.117)", 120, null, null, "DangSuDung" },
                    { 118, 1, 146222222.24m, 5222222.22m, 41777777.76m, null, "2114", "LAP-0118", null, null, new DateTime(2024, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9001), 22, 188000000m, null, 6, "DuongThang", "SN88846", null, "Dell Precision 5570 (No.118)", 36, null, null, "DangSuDung" },
                    { 119, 2, 95566666.71m, 2033333.33m, 26433333.29m, null, "2112", "SRV-0119", null, null, new DateTime(2024, 5, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9013), 36, 122000000m, null, 7, "DuongThang", "SN20302", null, "Dell PowerEdge R740 (No.119)", 60, null, null, "DangSuDung" },
                    { 120, 1, 145333333.28m, 6055555.56m, 72666666.72m, null, "2114", "LAP-0120", null, null, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9026), 47, 218000000m, null, 2, "DuongThang", "SN67814", null, "MacBook Pro 16 (No.120)", 36, null, null, "DangSuDung" },
                    { 121, 1, 90000000m, 5000000m, 90000000m, null, "2114", "LAP-0121", null, null, new DateTime(2024, 10, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9037), 49, 180000000m, null, 5, "DuongThang", "SN60327", null, "Dell Precision 5570 (No.121)", 36, null, null, "DangSuDung" },
                    { 122, 4, 86416666.73m, 5083333.33m, 96583333.27m, null, "2112", "NET-0122", null, null, new DateTime(2024, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9051), 23, 183000000m, null, 1, "DuongThang", "SN76154", null, "Cisco Catalyst (No.122)", 36, null, null, "DangSuDung" },
                    { 123, 4, 40500000m, 1500000m, 13500000m, null, "2112", "NET-0123", null, null, new DateTime(2024, 9, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9062), 46, 54000000m, null, 3, "DuongThang", "SN69505", null, "Cisco Catalyst (No.123)", 36, null, null, "DangSuDung" },
                    { 124, 1, 92000000m, 4000000m, 52000000m, null, "2114", "LAP-0124", null, null, new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9073), 7, 144000000m, null, 6, "DuongThang", "SN41756", null, "Lenovo ThinkPad P1 (No.124)", 36, null, null, "DangSuDung" },
                    { 125, 1, 46972222.26m, 2472222.22m, 42027777.74m, null, "2114", "LAP-0125", null, null, new DateTime(2024, 4, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9161), 36, 89000000m, null, 1, "DuongThang", "SN63341", null, "Dell Precision 5570 (No.125)", 36, null, null, "DangSuDung" },
                    { 126, 2, 140466666.61m, 3266666.67m, 55533333.39m, null, "2112", "SRV-0126", null, null, new DateTime(2024, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9174), 19, 196000000m, null, 6, "DuongThang", "SN76448", null, "Dell PowerEdge R740 (No.126)", 60, null, null, "DangSuDung" },
                    { 127, 3, 142450000.03m, 1283333.33m, 11549999.97m, null, "2113", "OTO-0127", null, null, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9186), 7, 154000000m, null, 1, "DuongThang", "SN97197", null, "Xe Ô tô VinFast VF8 (No.127)", 120, null, null, "DangSuDung" },
                    { 128, 1, 101055555.64m, 5944444.44m, 112944444.36m, null, "2114", "LAP-0128", null, null, new DateTime(2024, 8, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9198), 49, 214000000m, null, 1, "DuongThang", "SN83092", null, "MacBook Pro 16 (No.128)", 36, null, null, "DangSuDung" },
                    { 129, 2, 79750000m, 1450000m, 7250000m, null, "2112", "SRV-0129", null, null, new DateTime(2024, 9, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9210), 2, 87000000m, null, 3, "DuongThang", "SN66069", null, "Dell PowerEdge R740 (No.129)", 60, null, null, "DangSuDung" },
                    { 130, 4, 58333333.28m, 2916666.67m, 46666666.72m, null, "2112", "NET-0130", null, null, new DateTime(2024, 11, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9225), 7, 105000000m, null, 8, "DuongThang", "SN61253", null, "HP ProLiant DL380 (No.130)", 36, null, null, "DangSuDung" },
                    { 131, 4, 68472222.23m, 2361111.11m, 16527777.77m, null, "2112", "NET-0131", null, null, new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9284), 44, 85000000m, null, 8, "DuongThang", "SN47267", null, "Cisco Catalyst (No.131)", 36, null, null, "DangSuDung" },
                    { 132, 4, 50749999.95m, 2416666.67m, 36250000.05m, null, "2112", "NET-0132", null, null, new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9297), 9, 87000000m, null, 4, "DuongThang", "SN21685", null, "HP ProLiant DL380 (No.132)", 36, null, null, "DangSuDung" },
                    { 133, 2, 176800000m, 3400000m, 27200000m, null, "2112", "SRV-0133", null, null, new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9309), 27, 204000000m, null, 7, "DuongThang", "SN50673", null, "HP ProLiant DL380 (No.133)", 60, null, null, "DangSuDung" },
                    { 134, 3, 38250000m, 375000m, 6750000m, null, "2113", "OTO-0134", null, null, new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9321), 48, 45000000m, null, 5, "DuongThang", "SN17521", null, "Xe Ô tô VinFast VF8 (No.134)", 120, null, null, "DangSuDung" },
                    { 135, 3, 60124999.97m, 541666.67m, 4875000.03m, null, "2113", "OTO-0135", null, null, new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9334), 49, 65000000m, null, 7, "DuongThang", "SN90542", null, "Xe Ô tô VinFast VF8 (No.135)", 120, null, null, "DangSuDung" },
                    { 136, 2, 34850000m, 850000m, 16150000m, null, "2112", "SRV-0136", null, null, new DateTime(2024, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9345), 42, 51000000m, null, 2, "DuongThang", "SN10978", null, "Dell PowerEdge R740 (No.136)", 60, null, null, "DangSuDung" },
                    { 137, 4, 70833333.37m, 2833333.33m, 31166666.63m, null, "2112", "NET-0137", null, null, new DateTime(2024, 6, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9358), 33, 102000000m, null, 8, "DuongThang", "SN97494", null, "Synology NAS 8-Bay (No.137)", 36, null, null, "DangSuDung" },
                    { 138, 2, 101200000m, 2300000m, 36800000m, null, "2112", "SRV-0138", null, null, new DateTime(2024, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9370), 10, 138000000m, null, 3, "DuongThang", "SN68378", null, "Cisco Catalyst (No.138)", 60, null, null, "DangSuDung" },
                    { 139, 4, 64222222.18m, 3777777.78m, 71777777.82m, null, "2112", "NET-0139", null, null, new DateTime(2024, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9383), 17, 136000000m, null, 5, "DuongThang", "SN82276", null, "Dell PowerEdge R740 (No.139)", 36, null, null, "DangSuDung" },
                    { 140, 2, 85400000.06m, 2033333.33m, 36599999.94m, null, "2112", "SRV-0140", null, null, new DateTime(2024, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9442), 4, 122000000m, null, 4, "DuongThang", "SN38879", null, "Synology NAS 8-Bay (No.140)", 60, null, null, "DangSuDung" },
                    { 141, 4, 130500000.03m, 4833333.33m, 43499999.97m, null, "2112", "NET-0141", null, null, new DateTime(2024, 10, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9454), 36, 174000000m, null, 5, "DuongThang", "SN22468", null, "Dell PowerEdge R740 (No.141)", 36, null, null, "DangSuDung" },
                    { 142, 2, 24933333.28m, 566666.67m, 9066666.72m, null, "2112", "SRV-0142", null, null, new DateTime(2024, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9466), 30, 34000000m, null, 4, "DuongThang", "SN37367", null, "Synology NAS 8-Bay (No.142)", 60, null, null, "DangSuDung" },
                    { 143, 3, 65866666.72m, 633333.33m, 10133333.28m, null, "2113", "OTO-0143", null, null, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9479), 28, 76000000m, null, 5, "DuongThang", "SN78151", null, "Xe Ô tô VinFast VF8 (No.143)", 120, null, null, "DangSuDung" },
                    { 144, 3, 197416666.65m, 1716666.67m, 8583333.35m, null, "2113", "OTO-0144", null, null, new DateTime(2024, 11, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9493), 14, 206000000m, null, 4, "DuongThang", "SN34398", null, "Xe Ô tô VinFast VF8 (No.144)", 120, null, null, "DangSuDung" },
                    { 145, 4, 63750000m, 3750000m, 71250000m, null, "2112", "NET-0145", null, null, new DateTime(2024, 5, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9504), 8, 135000000m, null, 7, "DuongThang", "SN57546", null, "Synology NAS 8-Bay (No.145)", 36, null, null, "DangSuDung" },
                    { 146, 4, 22666666.73m, 1333333.33m, 25333333.27m, null, "2112", "NET-0146", null, null, new DateTime(2024, 2, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9517), 19, 48000000m, null, 2, "DuongThang", "SN92700", null, "Synology NAS 8-Bay (No.146)", 36, null, null, "DangSuDung" },
                    { 147, 1, 121666666.64m, 4055555.56m, 24333333.36m, null, "2114", "LAP-0147", null, null, new DateTime(2024, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9529), 11, 146000000m, null, 5, "DuongThang", "SN21011", null, "Asus ROG (No.147)", 36, null, null, "DangSuDung" },
                    { 148, 2, 83200000.04m, 1733333.33m, 20799999.96m, null, "2112", "SRV-0148", null, null, new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9588), 40, 104000000m, null, 1, "DuongThang", "SN24320", null, "HP ProLiant DL380 (No.148)", 60, null, null, "DangSuDung" },
                    { 149, 4, 117361111.16m, 4694444.44m, 51638888.84m, null, "2112", "NET-0149", null, null, new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9602), 19, 169000000m, null, 1, "DuongThang", "SN65839", null, "Synology NAS 8-Bay (No.149)", 36, null, null, "DangSuDung" },
                    { 150, 3, 36558333.29m, 341666.67m, 4441666.71m, null, "2113", "OTO-0150", null, null, new DateTime(2024, 7, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9615), 44, 41000000m, null, 3, "DuongThang", "SN63913", null, "Xe Ô tô VinFast VF8 (No.150)", 120, null, null, "DangSuDung" },
                    { 151, 3, 114750000m, 1125000m, 20250000m, null, "2113", "OTO-0151", null, null, new DateTime(2024, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9627), 9, 135000000m, null, 8, "DuongThang", "SN20407", null, "Xe Ô tô VinFast VF8 (No.151)", 120, null, null, "DangSuDung" },
                    { 152, 1, 170500000m, 5500000m, 27500000m, null, "2114", "LAP-0152", null, null, new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9639), 37, 198000000m, null, 7, "DuongThang", "SN43766", null, "MacBook Pro 16 (No.152)", 36, null, null, "DangSuDung" },
                    { 153, 2, 129850000m, 2650000m, 29150000m, null, "2112", "SRV-0153", null, null, new DateTime(2024, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9650), 6, 159000000m, null, 8, "DuongThang", "SN69946", null, "Synology NAS 8-Bay (No.153)", 60, null, null, "DangSuDung" },
                    { 154, 1, 118833333.35m, 3833333.33m, 19166666.65m, null, "2114", "LAP-0154", null, null, new DateTime(2024, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9663), 48, 138000000m, null, 8, "DuongThang", "SN68103", null, "Asus ROG (No.154)", 36, null, null, "DangSuDung" },
                    { 155, 3, 106600000m, 1025000m, 16400000m, null, "2113", "OTO-0155", null, null, new DateTime(2024, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9675), 39, 123000000m, null, 3, "DuongThang", "SN13601", null, "Xe Ô tô VinFast VF8 (No.155)", 120, null, null, "DangSuDung" },
                    { 156, 4, 179638888.92m, 6194444.44m, 43361111.08m, null, "2112", "NET-0156", null, null, new DateTime(2024, 11, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9732), 25, 223000000m, null, 5, "DuongThang", "SN28828", null, "Synology NAS 8-Bay (No.156)", 36, null, null, "DangSuDung" },
                    { 157, 1, 125222222.28m, 5444444.44m, 70777777.72m, null, "2114", "LAP-0157", null, null, new DateTime(2024, 3, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9744), 49, 196000000m, null, 1, "DuongThang", "SN35834", null, "Dell Precision 5570 (No.157)", 36, null, null, "DangSuDung" },
                    { 158, 3, 78966666.61m, 766666.67m, 13033333.39m, null, "2113", "OTO-0158", null, null, new DateTime(2024, 3, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9757), 30, 92000000m, null, 3, "DuongThang", "SN48235", null, "Xe Ô tô VinFast VF8 (No.158)", 120, null, null, "DangSuDung" },
                    { 159, 4, 175000000m, 6250000m, 50000000m, null, "2112", "NET-0159", null, null, new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9769), 24, 225000000m, null, 3, "DuongThang", "SN51965", null, "Synology NAS 8-Bay (No.159)", 36, null, null, "DangSuDung" },
                    { 160, 4, 42500000m, 2500000m, 47500000m, null, "2112", "NET-0160", null, null, new DateTime(2024, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9783), 5, 90000000m, null, 6, "DuongThang", "SN25100", null, "Cisco Catalyst (No.160)", 36, null, null, "DangSuDung" },
                    { 161, 4, 165388888.90m, 6361111.11m, 63611111.10m, null, "2112", "NET-0161", null, null, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9796), 26, 229000000m, null, 2, "DuongThang", "SN68201", null, "Dell PowerEdge R740 (No.161)", 36, null, null, "DangSuDung" },
                    { 162, 4, 23833333.38m, 1083333.33m, 15166666.62m, null, "2112", "NET-0162", null, null, new DateTime(2024, 2, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9855), 48, 39000000m, null, 1, "DuongThang", "SN76314", null, "Dell PowerEdge R740 (No.162)", 36, null, null, "DangSuDung" },
                    { 163, 2, 171500000m, 3500000m, 38500000m, null, "2112", "SRV-0163", null, null, new DateTime(2024, 7, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9867), 34, 210000000m, null, 8, "DuongThang", "SN73303", null, "Cisco Catalyst (No.163)", 60, null, null, "DangSuDung" },
                    { 164, 1, 27472222.28m, 1194444.44m, 15527777.72m, null, "2114", "LAP-0164", null, null, new DateTime(2024, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9879), 7, 43000000m, null, 2, "DuongThang", "SN33140", null, "Lenovo ThinkPad P1 (No.164)", 36, null, null, "DangSuDung" },
                    { 165, 3, 28875000m, 275000m, 4125000m, null, "2113", "OTO-0165", null, null, new DateTime(2024, 8, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9891), 11, 33000000m, null, 2, "DuongThang", "SN14702", null, "Xe Ô tô VinFast VF8 (No.165)", 120, null, null, "DangSuDung" },
                    { 166, 2, 204050000m, 3850000m, 26950000m, null, "2112", "SRV-0166", null, null, new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9903), 40, 231000000m, null, 1, "DuongThang", "SN70645", null, "Dell PowerEdge R740 (No.166)", 60, null, null, "DangSuDung" },
                    { 167, 2, 88833333.27m, 2166666.67m, 41166666.73m, null, "2112", "SRV-0167", null, null, new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9915), 38, 130000000m, null, 1, "DuongThang", "SN68910", null, "HP ProLiant DL380 (No.167)", 60, null, null, "DangSuDung" },
                    { 168, 3, 192100000.06m, 1883333.33m, 33899999.94m, null, "2113", "OTO-0168", null, null, new DateTime(2024, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9975), 25, 226000000m, null, 6, "DuongThang", "SN44459", null, "Xe Ô tô VinFast VF8 (No.168)", 120, null, null, "DangSuDung" },
                    { 169, 3, 128625000m, 1225000m, 18375000m, null, "2113", "OTO-0169", null, null, new DateTime(2024, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9988), 39, 147000000m, null, 5, "DuongThang", "SN31449", null, "Xe Ô tô VinFast VF8 (No.169)", 120, null, null, "DangSuDung" },
                    { 170, 4, 105777777.82m, 6222222.22m, 118222222.18m, null, "2112", "NET-0170", null, null, new DateTime(2024, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1), 32, 224000000m, null, 6, "DuongThang", "SN89455", null, "Cisco Catalyst (No.170)", 36, null, null, "DangSuDung" },
                    { 171, 4, 106499999.94m, 5916666.67m, 106500000.06m, null, "2112", "NET-0171", null, null, new DateTime(2024, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(14), 10, 213000000m, null, 2, "DuongThang", "SN97060", null, "Synology NAS 8-Bay (No.171)", 36, null, null, "DangSuDung" },
                    { 172, 1, 136499999.96m, 5055555.56m, 45500000.04m, null, "2114", "LAP-0172", null, null, new DateTime(2024, 9, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(25), 16, 182000000m, null, 2, "DuongThang", "SN77402", null, "Lenovo ThinkPad P1 (No.172)", 36, null, null, "DangSuDung" },
                    { 173, 1, 88499999.94m, 4916666.67m, 88500000.06m, null, "2114", "LAP-0173", null, null, new DateTime(2024, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(38), 5, 177000000m, null, 5, "DuongThang", "SN25771", null, "Lenovo ThinkPad P1 (No.173)", 36, null, null, "DangSuDung" },
                    { 174, 4, 114388888.92m, 3944444.44m, 27611111.08m, null, "2112", "NET-0174", null, null, new DateTime(2024, 9, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(119), 20, 142000000m, null, 6, "DuongThang", "SN14786", null, "Synology NAS 8-Bay (No.174)", 36, null, null, "DangSuDung" },
                    { 175, 3, 119166666.70m, 1083333.33m, 10833333.30m, null, "2113", "OTO-0175", null, null, new DateTime(2024, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(133), 10, 130000000m, null, 6, "DuongThang", "SN66660", null, "Xe Ô tô VinFast VF8 (No.175)", 120, null, null, "DangSuDung" },
                    { 176, 3, 131250000m, 1250000m, 18750000m, null, "2113", "OTO-0176", null, null, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(145), 10, 150000000m, null, 4, "DuongThang", "SN16560", null, "Xe Ô tô VinFast VF8 (No.176)", 120, null, null, "DangSuDung" },
                    { 177, 1, 101888888.88m, 3638888.89m, 29111111.12m, null, "2114", "LAP-0177", null, null, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(158), 4, 131000000m, null, 4, "DuongThang", "SN49154", null, "Asus ROG (No.177)", 36, null, null, "DangSuDung" },
                    { 178, 2, 43050000m, 1050000m, 19950000m, null, "2112", "SRV-0178", null, null, new DateTime(2024, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(169), 26, 63000000m, null, 3, "DuongThang", "SN48838", null, "Dell PowerEdge R740 (No.178)", 60, null, null, "DangSuDung" },
                    { 179, 2, 93616666.73m, 2283333.33m, 43383333.27m, null, "2112", "SRV-0179", null, null, new DateTime(2024, 11, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(182), 16, 137000000m, null, 4, "DuongThang", "SN26654", null, "Cisco Catalyst (No.179)", 60, null, null, "DangSuDung" },
                    { 180, 3, 121599999.98m, 1066666.67m, 6400000.02m, null, "2113", "OTO-0180", null, null, new DateTime(2024, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(195), 8, 128000000m, null, 1, "DuongThang", "SN23975", null, "Xe Ô tô VinFast VF8 (No.180)", 120, null, null, "DangSuDung" },
                    { 181, 3, 161600000m, 1600000m, 30400000m, null, "2113", "OTO-0181", null, null, new DateTime(2024, 3, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(207), 16, 192000000m, null, 4, "DuongThang", "SN73158", null, "Xe Ô tô VinFast VF8 (No.181)", 120, null, null, "DangSuDung" },
                    { 182, 2, 26349999.97m, 516666.67m, 4650000.03m, null, "2112", "SRV-0182", null, null, new DateTime(2024, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(285), 27, 31000000m, null, 1, "DuongThang", "SN58927", null, "Synology NAS 8-Bay (No.182)", 60, null, null, "DangSuDung" },
                    { 183, 3, 149150000.02m, 1308333.33m, 7849999.98m, null, "2113", "OTO-0183", null, null, new DateTime(2024, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(298), 14, 157000000m, null, 7, "DuongThang", "SN66070", null, "Xe Ô tô VinFast VF8 (No.183)", 120, null, null, "DangSuDung" },
                    { 184, 4, 181666666.64m, 6055555.56m, 36333333.36m, null, "2112", "NET-0184", null, null, new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(311), 14, 218000000m, null, 8, "DuongThang", "SN88038", null, "HP ProLiant DL380 (No.184)", 36, null, null, "DangSuDung" },
                    { 185, 3, 132250000m, 1150000m, 5750000m, null, "2113", "OTO-0185", null, null, new DateTime(2024, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(322), 35, 138000000m, null, 2, "DuongThang", "SN73557", null, "Xe Ô tô VinFast VF8 (No.185)", 120, null, null, "DangSuDung" },
                    { 186, 1, 104861111.16m, 4194444.44m, 46138888.84m, null, "2114", "LAP-0186", null, null, new DateTime(2024, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(334), 5, 151000000m, null, 4, "DuongThang", "SN36203", null, "Dell Precision 5570 (No.186)", 36, null, null, "DangSuDung" },
                    { 187, 3, 33666666.73m, 333333.33m, 6333333.27m, null, "2113", "OTO-0187", null, null, new DateTime(2024, 10, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(346), 34, 40000000m, null, 8, "DuongThang", "SN77795", null, "Xe Ô tô VinFast VF8 (No.187)", 120, null, null, "DangSuDung" },
                    { 188, 4, 89555555.55m, 2888888.89m, 14444444.45m, null, "2112", "NET-0188", null, null, new DateTime(2024, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(359), 10, 104000000m, null, 8, "DuongThang", "SN43676", null, "HP ProLiant DL380 (No.188)", 36, null, null, "DangSuDung" },
                    { 189, 1, 108333333.30m, 4166666.67m, 41666666.70m, null, "2114", "LAP-0189", null, null, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(371), 20, 150000000m, null, 7, "DuongThang", "SN14233", null, "Lenovo ThinkPad P1 (No.189)", 36, null, null, "DangSuDung" },
                    { 190, 1, 64277777.80m, 2472222.22m, 24722222.20m, null, "2114", "LAP-0190", null, null, new DateTime(2024, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(383), 31, 89000000m, null, 2, "DuongThang", "SN13481", null, "Lenovo ThinkPad P1 (No.190)", 36, null, null, "DangSuDung" },
                    { 191, 4, 109250000m, 4750000m, 61750000m, null, "2112", "NET-0191", null, null, new DateTime(2024, 5, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(441), 8, 171000000m, null, 4, "DuongThang", "SN21287", null, "Synology NAS 8-Bay (No.191)", 36, null, null, "DangSuDung" },
                    { 192, 2, 49350000m, 1050000m, 13650000m, null, "2112", "SRV-0192", null, null, new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(453), 15, 63000000m, null, 7, "DuongThang", "SN56231", null, "Dell PowerEdge R740 (No.192)", 60, null, null, "DangSuDung" },
                    { 193, 3, 50400000m, 450000m, 3600000m, null, "2113", "OTO-0193", null, null, new DateTime(2024, 10, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(465), 47, 54000000m, null, 5, "DuongThang", "SN74888", null, "Xe Ô tô VinFast VF8 (No.193)", 120, null, null, "DangSuDung" },
                    { 194, 2, 171366666.69m, 3233333.33m, 22633333.31m, null, "2112", "SRV-0194", null, null, new DateTime(2024, 11, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(477), 18, 194000000m, null, 6, "DuongThang", "SN62628", null, "HP ProLiant DL380 (No.194)", 60, null, null, "DangSuDung" },
                    { 195, 3, 116850000m, 1025000m, 6150000m, null, "2113", "OTO-0195", null, null, new DateTime(2024, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(489), 46, 123000000m, null, 5, "DuongThang", "SN97467", null, "Xe Ô tô VinFast VF8 (No.195)", 120, null, null, "DangSuDung" },
                    { 196, 4, 61111111.16m, 2444444.44m, 26888888.84m, null, "2112", "NET-0196", null, null, new DateTime(2024, 8, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(501), 15, 88000000m, null, 2, "DuongThang", "SN38130", null, "Synology NAS 8-Bay (No.196)", 36, null, null, "DangSuDung" },
                    { 197, 4, 165833333.32m, 5527777.78m, 33166666.68m, null, "2112", "NET-0197", null, null, new DateTime(2024, 8, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(515), 35, 199000000m, null, 3, "DuongThang", "SN43330", null, "Synology NAS 8-Bay (No.197)", 36, null, null, "DangSuDung" },
                    { 198, 2, 163999999.96m, 3416666.67m, 41000000.04m, null, "2112", "SRV-0198", null, null, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(527), 31, 205000000m, null, 8, "DuongThang", "SN96843", null, "Dell PowerEdge R740 (No.198)", 60, null, null, "DangSuDung" },
                    { 199, 1, 58138888.86m, 2527777.78m, 32861111.14m, null, "2114", "LAP-0199", null, null, new DateTime(2024, 7, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(584), 30, 91000000m, null, 3, "DuongThang", "SN35065", null, "Lenovo ThinkPad P1 (No.199)", 36, null, null, "DangSuDung" },
                    { 200, 2, 50916666.71m, 1083333.33m, 14083333.29m, null, "2112", "SRV-0200", null, null, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(596), 25, 65000000m, null, 4, "DuongThang", "SN78908", null, "Dell PowerEdge R740 (No.200)", 60, null, null, "DangSuDung" },
                    { 201, 4, 174805555.55m, 5638888.89m, 28194444.45m, null, "2112", "NET-0201", null, null, new DateTime(2024, 3, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(609), 21, 203000000m, null, 8, "DuongThang", "SN52088", null, "Synology NAS 8-Bay (No.201)", 36, null, null, "DangSuDung" },
                    { 202, 1, 168750000m, 6250000m, 56250000m, null, "2114", "LAP-0202", null, null, new DateTime(2024, 3, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(621), 12, 225000000m, null, 8, "DuongThang", "SN38397", null, "Dell Precision 5570 (No.202)", 36, null, null, "DangSuDung" },
                    { 203, 1, 160666666.72m, 6694444.44m, 80333333.28m, null, "2114", "LAP-0203", null, null, new DateTime(2024, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(634), 25, 241000000m, null, 6, "DuongThang", "SN94190", null, "Dell Precision 5570 (No.203)", 36, null, null, "DangSuDung" },
                    { 204, 1, 135055555.60m, 5194444.44m, 51944444.40m, null, "2114", "LAP-0204", null, null, new DateTime(2024, 3, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(646), 6, 187000000m, null, 5, "DuongThang", "SN73565", null, "Asus ROG (No.204)", 36, null, null, "DangSuDung" },
                    { 205, 1, 56250000.03m, 2083333.33m, 18749999.97m, null, "2114", "LAP-0205", null, null, new DateTime(2024, 9, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(658), 17, 75000000m, null, 3, "DuongThang", "SN60993", null, "MacBook Pro 16 (No.205)", 36, null, null, "DangSuDung" },
                    { 206, 2, 176800000m, 3400000m, 27200000m, null, "2112", "SRV-0206", null, null, new DateTime(2024, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(670), 18, 204000000m, null, 8, "DuongThang", "SN66481", null, "Dell PowerEdge R740 (No.206)", 60, null, null, "DangSuDung" },
                    { 207, 4, 54166666.64m, 1805555.56m, 10833333.36m, null, "2112", "NET-0207", null, null, new DateTime(2024, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(728), 47, 65000000m, null, 2, "DuongThang", "SN52199", null, "HP ProLiant DL380 (No.207)", 36, null, null, "DangSuDung" },
                    { 208, 4, 71249999.99m, 2638888.89m, 23750000.01m, null, "2112", "NET-0208", null, null, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(741), 6, 95000000m, null, 7, "DuongThang", "SN57782", null, "Cisco Catalyst (No.208)", 36, null, null, "DangSuDung" },
                    { 209, 1, 25666666.62m, 1166666.67m, 16333333.38m, null, "2114", "LAP-0209", null, null, new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(753), 35, 42000000m, null, 1, "DuongThang", "SN19064", null, "Lenovo ThinkPad P1 (No.209)", 36, null, null, "DangSuDung" },
                    { 210, 1, 71111111.04m, 3555555.56m, 56888888.96m, null, "2114", "LAP-0210", null, null, new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(766), 14, 128000000m, null, 7, "DuongThang", "SN17649", null, "Asus ROG (No.210)", 36, null, null, "DangSuDung" },
                    { 211, 3, 46000000m, 400000m, 2000000m, null, "2113", "OTO-0211", null, null, new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(778), 14, 48000000m, null, 3, "DuongThang", "SN65294", null, "Xe Ô tô VinFast VF8 (No.211)", 120, null, null, "DangSuDung" },
                    { 212, 2, 26650000m, 650000m, 12350000m, null, "2112", "SRV-0212", null, null, new DateTime(2024, 5, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(789), 49, 39000000m, null, 3, "DuongThang", "SN63856", null, "Dell PowerEdge R740 (No.212)", 60, null, null, "DangSuDung" },
                    { 213, 3, 158649999.98m, 1391666.67m, 8350000.02m, null, "2113", "OTO-0213", null, null, new DateTime(2024, 1, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(802), 46, 167000000m, null, 3, "DuongThang", "SN99756", null, "Xe Ô tô VinFast VF8 (No.213)", 120, null, null, "DangSuDung" },
                    { 214, 1, 95333333.36m, 3972222.22m, 47666666.64m, null, "2114", "LAP-0214", null, null, new DateTime(2024, 9, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(814), 16, 143000000m, null, 6, "DuongThang", "SN92952", null, "Asus ROG (No.214)", 36, null, null, "DangSuDung" },
                    { 215, 2, 96750000m, 2150000m, 32250000m, null, "2112", "SRV-0215", null, null, new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(871), 39, 129000000m, null, 6, "DuongThang", "SN63361", null, "Dell PowerEdge R740 (No.215)", 60, null, null, "DangSuDung" },
                    { 216, 1, 26666666.66m, 888888.89m, 5333333.34m, null, "2114", "LAP-0216", null, null, new DateTime(2024, 3, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(884), 20, 32000000m, null, 3, "DuongThang", "SN29392", null, "MacBook Pro 16 (No.216)", 36, null, null, "DangSuDung" },
                    { 217, 2, 50166666.61m, 1166666.67m, 19833333.39m, null, "2112", "SRV-0217", null, null, new DateTime(2024, 4, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(897), 48, 70000000m, null, 4, "DuongThang", "SN16643", null, "HP ProLiant DL380 (No.217)", 60, null, null, "DangSuDung" },
                    { 218, 1, 153750000.04m, 5694444.44m, 51249999.96m, null, "2114", "LAP-0218", null, null, new DateTime(2024, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(909), 11, 205000000m, null, 3, "DuongThang", "SN93113", null, "Asus ROG (No.218)", 36, null, null, "DangSuDung" },
                    { 219, 1, 55250000m, 3250000m, 61750000m, null, "2114", "LAP-0219", null, null, new DateTime(2024, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(921), 31, 117000000m, null, 1, "DuongThang", "SN73202", null, "Asus ROG (No.219)", 36, null, null, "DangSuDung" },
                    { 220, 4, 122111111.12m, 4361111.11m, 34888888.88m, null, "2112", "NET-0220", null, null, new DateTime(2024, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(935), 15, 157000000m, null, 8, "DuongThang", "SN30770", null, "Cisco Catalyst (No.220)", 36, null, null, "DangSuDung" },
                    { 221, 3, 46958333.35m, 408333.33m, 2041666.65m, null, "2113", "OTO-0221", null, null, new DateTime(2024, 7, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(948), 19, 49000000m, null, 8, "DuongThang", "SN19131", null, "Xe Ô tô VinFast VF8 (No.221)", 120, null, null, "DangSuDung" },
                    { 222, 4, 142333333.30m, 6777777.78m, 101666666.70m, null, "2112", "NET-0222", null, null, new DateTime(2024, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(960), 35, 244000000m, null, 4, "DuongThang", "SN59366", null, "Synology NAS 8-Bay (No.222)", 36, null, null, "DangSuDung" },
                    { 223, 4, 179166666.68m, 5972222.22m, 35833333.32m, null, "2112", "NET-0223", null, null, new DateTime(2024, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(975), 8, 215000000m, null, 8, "DuongThang", "SN92435", null, "Synology NAS 8-Bay (No.223)", 36, null, null, "DangSuDung" },
                    { 224, 2, 93800000.06m, 2233333.33m, 40199999.94m, null, "2112", "SRV-0224", null, null, new DateTime(2024, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1034), 41, 134000000m, null, 8, "DuongThang", "SN38689", null, "Cisco Catalyst (No.224)", 60, null, null, "DangSuDung" },
                    { 225, 4, 94444444.42m, 3777777.78m, 41555555.58m, null, "2112", "NET-0225", null, null, new DateTime(2024, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1046), 19, 136000000m, null, 3, "DuongThang", "SN35048", null, "Dell PowerEdge R740 (No.225)", 36, null, null, "DangSuDung" },
                    { 226, 3, 74100000m, 650000m, 3900000m, null, "2113", "OTO-0226", null, null, new DateTime(2024, 4, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1058), 42, 78000000m, null, 3, "DuongThang", "SN80010", null, "Xe Ô tô VinFast VF8 (No.226)", 120, null, null, "DangSuDung" },
                    { 227, 2, 119699999.98m, 2216666.67m, 13300000.02m, null, "2112", "SRV-0227", null, null, new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1071), 19, 133000000m, null, 1, "DuongThang", "SN53491", null, "Synology NAS 8-Bay (No.227)", 60, null, null, "DangSuDung" },
                    { 228, 3, 219458333.35m, 1908333.33m, 9541666.65m, null, "2113", "OTO-0228", null, null, new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1083), 33, 229000000m, null, 1, "DuongThang", "SN95806", null, "Xe Ô tô VinFast VF8 (No.228)", 120, null, null, "DangSuDung" },
                    { 229, 1, 36416666.71m, 1583333.33m, 20583333.29m, null, "2114", "LAP-0229", null, null, new DateTime(2024, 8, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1095), 9, 57000000m, null, 3, "DuongThang", "SN19002", null, "Dell Precision 5570 (No.229)", 36, null, null, "DangSuDung" },
                    { 230, 3, 33600000m, 300000m, 2400000m, null, "2113", "OTO-0230", null, null, new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1107), 26, 36000000m, null, 2, "DuongThang", "SN15711", null, "Xe Ô tô VinFast VF8 (No.230)", 120, null, null, "DangSuDung" },
                    { 231, 4, 126666666.64m, 5277777.78m, 63333333.36m, null, "2112", "NET-0231", null, null, new DateTime(2024, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1119), 21, 190000000m, null, 3, "DuongThang", "SN95687", null, "Synology NAS 8-Bay (No.231)", 36, null, null, "DangSuDung" },
                    { 232, 2, 142216666.69m, 2683333.33m, 18783333.31m, null, "2112", "SRV-0232", null, null, new DateTime(2024, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1159), 49, 161000000m, null, 3, "DuongThang", "SN37355", null, "Synology NAS 8-Bay (No.232)", 60, null, null, "DangSuDung" },
                    { 233, 3, 116766666.69m, 1033333.33m, 7233333.31m, null, "2113", "OTO-0233", null, null, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1171), 21, 124000000m, null, 4, "DuongThang", "SN74346", null, "Xe Ô tô VinFast VF8 (No.233)", 120, null, null, "DangSuDung" },
                    { 234, 2, 85066666.72m, 1933333.33m, 30933333.28m, null, "2112", "SRV-0234", null, null, new DateTime(2024, 11, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1183), 44, 116000000m, null, 8, "DuongThang", "SN83024", null, "HP ProLiant DL380 (No.234)", 60, null, null, "DangSuDung" },
                    { 235, 1, 128250000m, 6750000m, 114750000m, null, "2114", "LAP-0235", null, null, new DateTime(2024, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1194), 40, 243000000m, null, 7, "DuongThang", "SN33262", null, "Lenovo ThinkPad P1 (No.235)", 36, null, null, "DangSuDung" },
                    { 236, 3, 98000000.05m, 933333.33m, 13999999.95m, null, "2113", "OTO-0236", null, null, new DateTime(2024, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1207), 45, 112000000m, null, 2, "DuongThang", "SN87359", null, "Xe Ô tô VinFast VF8 (No.236)", 120, null, null, "DangSuDung" },
                    { 237, 3, 88800000m, 800000m, 7200000m, null, "2113", "OTO-0237", null, null, new DateTime(2024, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1219), 28, 96000000m, null, 1, "DuongThang", "SN91633", null, "Xe Ô tô VinFast VF8 (No.237)", 120, null, null, "DangSuDung" },
                    { 238, 3, 186383333.38m, 1758333.33m, 24616666.62m, null, "2113", "OTO-0238", null, null, new DateTime(2024, 6, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1231), 12, 211000000m, null, 6, "DuongThang", "SN73252", null, "Xe Ô tô VinFast VF8 (No.238)", 120, null, null, "DangSuDung" },
                    { 239, 2, 23500000m, 500000m, 6500000m, null, "2112", "SRV-0239", null, null, new DateTime(2024, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1242), 42, 30000000m, null, 1, "DuongThang", "SN85599", null, "Cisco Catalyst (No.239)", 60, null, null, "DangSuDung" },
                    { 240, 2, 122783333.31m, 2316666.67m, 16216666.69m, null, "2112", "SRV-0240", null, null, new DateTime(2024, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1324), 45, 139000000m, null, 4, "DuongThang", "SN65022", null, "Synology NAS 8-Bay (No.240)", 60, null, null, "DangSuDung" },
                    { 241, 3, 30600000m, 300000m, 5400000m, null, "2113", "OTO-0241", null, null, new DateTime(2024, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1336), 13, 36000000m, null, 4, "DuongThang", "SN72912", null, "Xe Ô tô VinFast VF8 (No.241)", 120, null, null, "DangSuDung" },
                    { 242, 2, 65933333.38m, 1433333.33m, 20066666.62m, null, "2112", "SRV-0242", null, null, new DateTime(2024, 6, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1350), 11, 86000000m, null, 7, "DuongThang", "SN70809", null, "Cisco Catalyst (No.242)", 60, null, null, "DangSuDung" },
                    { 243, 4, 170777777.77m, 5888888.89m, 41222222.23m, null, "2112", "NET-0243", null, null, new DateTime(2024, 3, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1363), 8, 212000000m, null, 2, "DuongThang", "SN62625", null, "Dell PowerEdge R740 (No.243)", 36, null, null, "DangSuDung" },
                    { 244, 2, 151083333.37m, 3083333.33m, 33916666.63m, null, "2112", "SRV-0244", null, null, new DateTime(2024, 2, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1375), 23, 185000000m, null, 8, "DuongThang", "SN24235", null, "HP ProLiant DL380 (No.244)", 60, null, null, "DangSuDung" },
                    { 245, 3, 141300000.04m, 1308333.33m, 15699999.96m, null, "2113", "OTO-0245", null, null, new DateTime(2024, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1387), 31, 157000000m, null, 1, "DuongThang", "SN55292", null, "Xe Ô tô VinFast VF8 (No.245)", 120, null, null, "DangSuDung" },
                    { 246, 3, 128966666.62m, 1216666.67m, 17033333.38m, null, "2113", "OTO-0246", null, null, new DateTime(2024, 8, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1400), 32, 146000000m, null, 4, "DuongThang", "SN33131", null, "Xe Ô tô VinFast VF8 (No.246)", 120, null, null, "DangSuDung" },
                    { 247, 4, 98000000.08m, 5444444.44m, 97999999.92m, null, "2112", "NET-0247", null, null, new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1413), 33, 196000000m, null, 1, "DuongThang", "SN44705", null, "HP ProLiant DL380 (No.247)", 36, null, null, "DangSuDung" },
                    { 248, 4, 91722222.20m, 3527777.78m, 35277777.80m, null, "2112", "NET-0248", null, null, new DateTime(2024, 8, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1492), 27, 127000000m, null, 2, "DuongThang", "SN14603", null, "Cisco Catalyst (No.248)", 36, null, null, "DangSuDung" },
                    { 249, 1, 72222222.24m, 3611111.11m, 57777777.76m, null, "2114", "LAP-0249", null, null, new DateTime(2024, 8, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1504), 39, 130000000m, null, 4, "DuongThang", "SN59429", null, "Dell Precision 5570 (No.249)", 36, null, null, "DangSuDung" },
                    { 250, 1, 146249999.97m, 5416666.67m, 48750000.03m, null, "2114", "LAP-0250", null, null, new DateTime(2024, 8, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1516), 37, 195000000m, null, 2, "DuongThang", "SN71342", null, "Asus ROG (No.250)", 36, null, null, "DangSuDung" },
                    { 251, 2, 161366666.71m, 3433333.33m, 44633333.29m, null, "2112", "SRV-0251", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1528), null, 206000000m, null, null, "DuongThang", "SN74737", null, "Cisco Catalyst (No.251)", 60, null, null, "ChuaCapPhat" },
                    { 252, 1, 44999999.97m, 1666666.67m, 15000000.03m, null, "2114", "LAP-0252", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1537), null, 60000000m, null, null, "DuongThang", "SN93870", null, "MacBook Pro 16 (No.252)", 36, null, null, "ChuaCapPhat" },
                    { 253, 4, 108000000m, 4000000m, 36000000m, null, "2112", "NET-0253", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1546), null, 144000000m, null, null, "DuongThang", "SN80884", null, "Dell PowerEdge R740 (No.253)", 36, null, null, "ChuaCapPhat" },
                    { 254, 3, 79333333.36m, 708333.33m, 5666666.64m, null, "2113", "OTO-0254", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1557), null, 85000000m, null, null, "DuongThang", "SN15209", null, "Xe Ô tô VinFast VF8 (No.254)", 120, null, null, "ChuaCapPhat" },
                    { 255, 2, 160000000m, 3200000m, 32000000m, null, "2112", "SRV-0255", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1565), null, 192000000m, null, null, "DuongThang", "SN46370", null, "Synology NAS 8-Bay (No.255)", 60, null, null, "ChuaCapPhat" },
                    { 256, 2, 138716666.73m, 3383333.33m, 64283333.27m, null, "2112", "SRV-0256", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1575), null, 203000000m, null, null, "DuongThang", "SN36641", null, "Cisco Catalyst (No.256)", 60, null, null, "ChuaCapPhat" },
                    { 257, 1, 86111111.04m, 4305555.56m, 68888888.96m, null, "2114", "LAP-0257", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1585), null, 155000000m, null, null, "DuongThang", "SN73640", null, "MacBook Pro 16 (No.257)", 36, null, null, "ChuaCapPhat" },
                    { 258, 2, 108500000.06m, 2583333.33m, 46499999.94m, null, "2112", "SRV-0258", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1643), null, 155000000m, null, null, "DuongThang", "SN15872", null, "Cisco Catalyst (No.258)", 60, null, null, "ChuaCapPhat" },
                    { 259, 1, 191722222.23m, 6611111.11m, 46277777.77m, null, "2114", "LAP-0259", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1653), null, 238000000m, null, null, "DuongThang", "SN18267", null, "Dell Precision 5570 (No.259)", 36, null, null, "ChuaCapPhat" },
                    { 260, 1, 166666666.64m, 5555555.56m, 33333333.36m, null, "2114", "LAP-0260", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1663), null, 200000000m, null, null, "DuongThang", "SN26996", null, "Lenovo ThinkPad P1 (No.260)", 36, null, null, "ChuaCapPhat" },
                    { 261, 2, 86100000m, 2100000m, 39900000m, null, "2112", "SRV-0261", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1673), null, 126000000m, null, null, "DuongThang", "SN80102", null, "HP ProLiant DL380 (No.261)", 60, null, null, "ChuaCapPhat" },
                    { 262, 4, 205000000.02m, 6833333.33m, 40999999.98m, null, "2112", "NET-0262", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1684), null, 246000000m, null, null, "DuongThang", "SN49222", null, "Synology NAS 8-Bay (No.262)", 36, null, null, "ChuaCapPhat" },
                    { 263, 2, 99399999.94m, 2366666.67m, 42600000.06m, null, "2112", "SRV-0263", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1694), null, 142000000m, null, null, "DuongThang", "SN42762", null, "Dell PowerEdge R740 (No.263)", 60, null, null, "ChuaCapPhat" },
                    { 264, 4, 115555555.52m, 5777777.78m, 92444444.48m, null, "2112", "NET-0264", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1705), null, 208000000m, null, null, "DuongThang", "SN68246", null, "HP ProLiant DL380 (No.264)", 36, null, null, "ChuaCapPhat" },
                    { 265, 3, 85000000.06m, 833333.33m, 14999999.94m, null, "2113", "OTO-0265", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1762), null, 100000000m, null, null, "DuongThang", "SN59772", null, "Xe Ô tô VinFast VF8 (No.265)", 120, null, null, "ChuaCapPhat" },
                    { 266, 1, 61388888.91m, 3611111.11m, 68611111.09m, null, "2114", "LAP-0266", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1773), null, 130000000m, null, null, "DuongThang", "SN40980", null, "Asus ROG (No.266)", 36, null, null, "ChuaCapPhat" },
                    { 267, 3, 80666666.70m, 733333.33m, 7333333.30m, null, "2113", "OTO-0267", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1783), null, 88000000m, null, null, "DuongThang", "SN91265", null, "Xe Ô tô VinFast VF8 (No.267)", 120, null, null, "ChuaCapPhat" },
                    { 268, 4, 92638888.86m, 4027777.78m, 52361111.14m, null, "2112", "NET-0268", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1793), null, 145000000m, null, null, "DuongThang", "SN83937", null, "Dell PowerEdge R740 (No.268)", 36, null, null, "ChuaCapPhat" },
                    { 269, 1, 125666666.69m, 4333333.33m, 30333333.31m, null, "2114", "LAP-0269", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1803), null, 156000000m, null, null, "DuongThang", "SN44209", null, "Dell Precision 5570 (No.269)", 36, null, null, "ChuaCapPhat" },
                    { 270, 4, 53305555.48m, 2805555.56m, 47694444.52m, null, "2112", "NET-0270", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1814), null, 101000000m, null, null, "DuongThang", "SN71819", null, "Cisco Catalyst (No.270)", 36, null, null, "ChuaCapPhat" },
                    { 271, 2, 191200000.04m, 3983333.33m, 47799999.96m, null, "2112", "SRV-0271", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1824), null, 239000000m, null, null, "DuongThang", "SN71255", null, "Dell PowerEdge R740 (No.271)", 60, null, null, "ChuaCapPhat" },
                    { 272, 1, 63999999.96m, 2666666.67m, 32000000.04m, null, "2114", "LAP-0272", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1834), null, 96000000m, null, null, "DuongThang", "SN21847", null, "Lenovo ThinkPad P1 (No.272)", 36, null, null, "ChuaCapPhat" },
                    { 273, 4, 71777777.74m, 3777777.78m, 64222222.26m, null, "2112", "NET-0273", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1844), null, 136000000m, null, null, "DuongThang", "SN77791", null, "HP ProLiant DL380 (No.273)", 36, null, null, "ChuaCapPhat" },
                    { 274, 3, 178599999.98m, 1566666.67m, 9400000.02m, null, "2113", "OTO-0274", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1854), null, 188000000m, null, null, "DuongThang", "SN28878", null, "Xe Ô tô VinFast VF8 (No.274)", 120, null, null, "ChuaCapPhat" },
                    { 275, 4, 41166666.61m, 2166666.67m, 36833333.39m, null, "2112", "NET-0275", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1864), null, 78000000m, null, null, "DuongThang", "SN12697", null, "Synology NAS 8-Bay (No.275)", 36, null, null, "ChuaCapPhat" },
                    { 276, 1, 42777777.76m, 2138888.89m, 34222222.24m, null, "2114", "LAP-0276", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1874), null, 77000000m, null, null, "DuongThang", "SN38833", null, "Dell Precision 5570 (No.276)", 36, null, null, "ChuaCapPhat" },
                    { 277, 1, 81666666.64m, 2916666.67m, 23333333.36m, null, "2114", "LAP-0277", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1884), null, 105000000m, null, null, "DuongThang", "SN80759", null, "Lenovo ThinkPad P1 (No.277)", 36, null, null, "ChuaCapPhat" },
                    { 278, 4, 21333333.32m, 888888.89m, 10666666.68m, null, "2112", "NET-0278", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1940), null, 32000000m, null, null, "DuongThang", "SN68626", null, "Cisco Catalyst (No.278)", 36, null, null, "ChuaCapPhat" },
                    { 279, 4, 144750000.01m, 5361111.11m, 48249999.99m, null, "2112", "NET-0279", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1951), null, 193000000m, null, null, "DuongThang", "SN39619", null, "Cisco Catalyst (No.279)", 36, null, null, "ChuaCapPhat" },
                    { 280, 3, 96283333.38m, 908333.33m, 12716666.62m, null, "2113", "OTO-0280", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1961), null, 109000000m, null, null, "DuongThang", "SN32231", null, "Xe Ô tô VinFast VF8 (No.280)", 120, null, null, "ChuaCapPhat" },
                    { 281, 4, 85000000m, 4250000m, 68000000m, null, "2112", "NET-0281", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1971), null, 153000000m, null, null, "DuongThang", "SN59856", null, "Synology NAS 8-Bay (No.281)", 36, null, null, "ChuaCapPhat" },
                    { 282, 1, 66888888.88m, 2388888.89m, 19111111.12m, null, "2114", "LAP-0282", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1981), null, 86000000m, null, null, "DuongThang", "SN42778", null, "Lenovo ThinkPad P1 (No.282)", 36, null, null, "ChuaCapPhat" },
                    { 283, 3, 112200000m, 1100000m, 19800000m, null, "2113", "OTO-0283", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1990), null, 132000000m, null, null, "DuongThang", "SN99476", null, "Xe Ô tô VinFast VF8 (No.283)", 120, null, null, "ChuaCapPhat" },
                    { 284, 2, 30066666.72m, 683333.33m, 10933333.28m, null, "2112", "SRV-0284", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2000), null, 41000000m, null, null, "DuongThang", "SN63916", null, "HP ProLiant DL380 (No.284)", 60, null, null, "ChuaCapPhat" },
                    { 285, 1, 18472222.26m, 972222.22m, 16527777.74m, null, "2114", "LAP-0285", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2010), null, 35000000m, null, null, "DuongThang", "SN81986", null, "MacBook Pro 16 (No.285)", 36, null, null, "ChuaCapPhat" },
                    { 286, 1, 72833333.39m, 3833333.33m, 65166666.61m, null, "2114", "LAP-0286", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2019), null, 138000000m, null, null, "DuongThang", "SN26845", null, "Dell Precision 5570 (No.286)", 36, null, null, "ChuaCapPhat" },
                    { 287, 4, 29027777.74m, 1527777.78m, 25972222.26m, null, "2112", "NET-0287", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2030), null, 55000000m, null, null, "DuongThang", "SN50087", null, "Cisco Catalyst (No.287)", 36, null, null, "ChuaCapPhat" },
                    { 288, 3, 132775000m, 1175000m, 8225000m, null, "2113", "OTO-0288", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2040), null, 141000000m, null, null, "DuongThang", "SN27133", null, "Xe Ô tô VinFast VF8 (No.288)", 120, null, null, "ChuaCapPhat" },
                    { 289, 2, 26500000m, 500000m, 3500000m, null, "2112", "SRV-0289", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2049), null, 30000000m, null, null, "DuongThang", "SN21826", null, "Cisco Catalyst (No.289)", 60, null, null, "ChuaCapPhat" },
                    { 290, 3, 59741666.71m, 558333.33m, 7258333.29m, null, "2113", "OTO-0290", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2104), null, 67000000m, null, null, "DuongThang", "SN58908", null, "Xe Ô tô VinFast VF8 (No.290)", 120, null, null, "ChuaCapPhat" },
                    { 291, 2, 117500000m, 2500000m, 32500000m, null, "2112", "SRV-0291", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2114), null, 150000000m, null, null, "DuongThang", "SN47949", null, "Dell PowerEdge R740 (No.291)", 60, null, null, "ChuaCapPhat" },
                    { 292, 2, 155633333.38m, 3383333.33m, 47366666.62m, null, "2112", "SRV-0292", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2124), null, 203000000m, null, null, "DuongThang", "SN20431", null, "Cisco Catalyst (No.292)", 60, null, null, "ChuaCapPhat" },
                    { 293, 1, 184333333.36m, 6583333.33m, 52666666.64m, null, "2114", "LAP-0293", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2134), null, 237000000m, null, null, "DuongThang", "SN46573", null, "Lenovo ThinkPad P1 (No.293)", 36, null, null, "ChuaCapPhat" },
                    { 294, 2, 92649999.97m, 1816666.67m, 16350000.03m, null, "2112", "SRV-0294", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2144), null, 109000000m, null, null, "DuongThang", "SN20325", null, "Cisco Catalyst (No.294)", 60, null, null, "ChuaCapPhat" },
                    { 295, 3, 210900000m, 1850000m, 11100000m, null, "2113", "OTO-0295", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2153), null, 222000000m, null, null, "DuongThang", "SN23724", null, "Xe Ô tô VinFast VF8 (No.295)", 120, null, null, "ChuaCapPhat" },
                    { 296, 2, 135000000m, 2700000m, 27000000m, null, "2112", "SRV-0296", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2162), null, 162000000m, null, null, "DuongThang", "SN39156", null, "Cisco Catalyst (No.296)", 60, null, null, "ChuaCapPhat" },
                    { 297, 3, 76591666.73m, 758333.33m, 14408333.27m, null, "2113", "OTO-0297", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2172), null, 91000000m, null, null, "DuongThang", "SN48427", null, "Xe Ô tô VinFast VF8 (No.297)", 120, null, null, "ChuaCapPhat" },
                    { 298, 1, 134555555.52m, 4805555.56m, 38444444.48m, null, "2114", "LAP-0298", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2183), null, 173000000m, null, null, "DuongThang", "SN78477", null, "Asus ROG (No.298)", 36, null, null, "ChuaCapPhat" },
                    { 299, 1, 73500000.02m, 2722222.22m, 24499999.98m, null, "2114", "LAP-0299", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2193), null, 98000000m, null, null, "DuongThang", "SN59861", null, "MacBook Pro 16 (No.299)", 36, null, null, "ChuaCapPhat" },
                    { 300, 4, 161888888.90m, 5222222.22m, 26111111.10m, null, "2112", "NET-0300", null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(2204), null, 188000000m, null, null, "DuongThang", "SN27074", null, "Cisco Catalyst (No.300)", 36, null, null, "ChuaCapPhat" }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "chi_tiet_chung_tu",
                columns: new[] { "Id", "ChungTuId", "MoTa", "SoTien", "TaiKhoanCo", "TaiKhoanNo", "TaiSanId" },
                values: new object[,]
                {
                    { 1, 2, "Giảm nguyên giá TSCĐ", 201000000m, "2114", null, 1 },
                    { 2, 2, "Xóa sổ hao mòn lũy kế", 66999999.96m, null, "2141", 1 },
                    { 3, 2, "Thu tiền thanh lý bằng tiền mặt", 8000000m, null, "111", 1 },
                    { 4, 2, "Lỗ thanh lý", 126000000.04m, null, "811", 1 },
                    { 5, 3, "Giảm nguyên giá TSCĐ", 77000000m, "2114", null, 2 },
                    { 6, 3, "Xóa sổ hao mòn lũy kế", 10694444.45m, null, "2141", 2 },
                    { 7, 3, "Thu tiền thanh lý bằng tiền mặt", 6000000m, null, "111", 2 },
                    { 8, 3, "Lỗ thanh lý", 60305555.55m, null, "811", 2 },
                    { 9, 4, "Giảm nguyên giá TSCĐ", 109000000m, "2114", null, 3 },
                    { 10, 4, "Xóa sổ hao mòn lũy kế", 45416666.70m, null, "2141", 3 },
                    { 11, 4, "Thu tiền thanh lý bằng tiền mặt", 5000000m, null, "111", 3 },
                    { 12, 4, "Lỗ thanh lý", 58583333.30m, null, "811", 3 },
                    { 13, 5, "Giảm nguyên giá TSCĐ", 144000000m, "2113", null, 4 },
                    { 14, 5, "Xóa sổ hao mòn lũy kế", 14400000m, null, "2141", 4 },
                    { 15, 5, "Thu tiền thanh lý bằng tiền mặt", 8000000m, null, "111", 4 },
                    { 16, 5, "Lỗ thanh lý", 121600000m, null, "811", 4 },
                    { 17, 6, "Giảm nguyên giá TSCĐ", 89000000m, "2113", null, 5 },
                    { 18, 6, "Xóa sổ hao mòn lũy kế", 6675000.03m, null, "2141", 5 },
                    { 19, 6, "Thu tiền thanh lý bằng tiền mặt", 13000000m, null, "111", 5 },
                    { 20, 6, "Lỗ thanh lý", 69324999.97m, null, "811", 5 },
                    { 21, 7, "Giảm nguyên giá TSCĐ", 127000000m, "2113", null, 6 },
                    { 22, 7, "Xóa sổ hao mòn lũy kế", 12699999.96m, null, "2141", 6 },
                    { 23, 7, "Thu tiền thanh lý bằng tiền mặt", 4000000m, null, "111", 6 },
                    { 24, 7, "Lỗ thanh lý", 110300000.04m, null, "811", 6 },
                    { 25, 8, "Giảm nguyên giá TSCĐ", 105000000m, "2114", null, 7 },
                    { 26, 8, "Xóa sổ hao mòn lũy kế", 14583333.35m, null, "2141", 7 },
                    { 27, 8, "Thu tiền thanh lý bằng tiền mặt", 6000000m, null, "111", 7 },
                    { 28, 8, "Lỗ thanh lý", 84416666.65m, null, "811", 7 },
                    { 29, 9, "Giảm nguyên giá TSCĐ", 127000000m, "2113", null, 8 },
                    { 30, 9, "Xóa sổ hao mòn lũy kế", 10583333.30m, null, "2141", 8 },
                    { 31, 9, "Thu tiền thanh lý bằng tiền mặt", 5000000m, null, "111", 8 },
                    { 32, 9, "Lỗ thanh lý", 111416666.70m, null, "811", 8 },
                    { 33, 10, "Giảm nguyên giá TSCĐ", 156000000m, "2112", null, 9 },
                    { 34, 10, "Xóa sổ hao mòn lũy kế", 33800000m, null, "2141", 9 },
                    { 35, 10, "Thu tiền thanh lý bằng tiền mặt", 11000000m, null, "111", 9 },
                    { 36, 10, "Lỗ thanh lý", 111200000m, null, "811", 9 },
                    { 37, 11, "Giảm nguyên giá TSCĐ", 152000000m, "2114", null, 10 },
                    { 38, 11, "Xóa sổ hao mòn lũy kế", 37999999.98m, null, "2141", 10 },
                    { 39, 11, "Thu tiền thanh lý bằng tiền mặt", 10000000m, null, "111", 10 },
                    { 40, 11, "Lỗ thanh lý", 104000000.02m, null, "811", 10 },
                    { 41, 12, "Giảm nguyên giá TSCĐ", 48000000m, "2112", null, 11 },
                    { 42, 12, "Xóa sổ hao mòn lũy kế", 13600000m, null, "2141", 11 },
                    { 43, 12, "Thu tiền thanh lý bằng tiền mặt", 6000000m, null, "111", 11 },
                    { 44, 12, "Lỗ thanh lý", 28400000m, null, "811", 11 },
                    { 45, 13, "Giảm nguyên giá TSCĐ", 249000000m, "2112", null, 12 },
                    { 46, 13, "Xóa sổ hao mòn lũy kế", 58100000m, null, "2141", 12 },
                    { 47, 13, "Thu tiền thanh lý bằng tiền mặt", 11000000m, null, "111", 12 },
                    { 48, 13, "Lỗ thanh lý", 179900000m, null, "811", 12 },
                    { 49, 14, "Giảm nguyên giá TSCĐ", 90000000m, "2112", null, 13 },
                    { 50, 14, "Xóa sổ hao mòn lũy kế", 7500000m, null, "2141", 13 },
                    { 51, 14, "Thu tiền thanh lý bằng tiền mặt", 6000000m, null, "111", 13 },
                    { 52, 14, "Lỗ thanh lý", 76500000m, null, "811", 13 },
                    { 53, 15, "Giảm nguyên giá TSCĐ", 165000000m, "2113", null, 14 },
                    { 54, 15, "Xóa sổ hao mòn lũy kế", 15125000m, null, "2141", 14 },
                    { 55, 15, "Thu tiền thanh lý bằng tiền mặt", 10000000m, null, "111", 14 },
                    { 56, 15, "Lỗ thanh lý", 139875000m, null, "811", 14 },
                    { 57, 16, "Giảm nguyên giá TSCĐ", 67000000m, "2112", null, 15 },
                    { 58, 16, "Xóa sổ hao mòn lũy kế", 11166666.66m, null, "2141", 15 },
                    { 59, 16, "Thu tiền thanh lý bằng tiền mặt", 4000000m, null, "111", 15 },
                    { 60, 16, "Lỗ thanh lý", 51833333.34m, null, "811", 15 },
                    { 61, 17, "Giảm nguyên giá TSCĐ", 215000000m, "2112", null, 16 },
                    { 62, 17, "Xóa sổ hao mòn lũy kế", 53749999.95m, null, "2141", 16 },
                    { 63, 17, "Thu tiền thanh lý bằng tiền mặt", 6000000m, null, "111", 16 },
                    { 64, 17, "Lỗ thanh lý", 155250000.05m, null, "811", 16 },
                    { 65, 18, "Giảm nguyên giá TSCĐ", 48000000m, "2112", null, 17 },
                    { 66, 18, "Xóa sổ hao mòn lũy kế", 15999999.96m, null, "2141", 17 },
                    { 67, 18, "Thu tiền thanh lý bằng tiền mặt", 6000000m, null, "111", 17 },
                    { 68, 18, "Lỗ thanh lý", 26000000.04m, null, "811", 17 },
                    { 69, 19, "Giảm nguyên giá TSCĐ", 110000000m, "2112", null, 18 },
                    { 70, 19, "Xóa sổ hao mòn lũy kế", 29333333.28m, null, "2141", 18 },
                    { 71, 19, "Thu tiền thanh lý bằng tiền mặt", 13000000m, null, "111", 18 },
                    { 72, 19, "Lỗ thanh lý", 67666666.72m, null, "811", 18 },
                    { 73, 20, "Giảm nguyên giá TSCĐ", 211000000m, "2112", null, 19 },
                    { 74, 20, "Xóa sổ hao mòn lũy kế", 99638888.87m, null, "2141", 19 },
                    { 75, 20, "Thu tiền thanh lý bằng tiền mặt", 5000000m, null, "111", 19 },
                    { 76, 20, "Lỗ thanh lý", 106361111.13m, null, "811", 19 },
                    { 77, 21, "Giảm nguyên giá TSCĐ", 86000000m, "2114", null, 20 },
                    { 78, 21, "Xóa sổ hao mòn lũy kế", 14333333.34m, null, "2141", 20 },
                    { 79, 21, "Thu tiền thanh lý bằng tiền mặt", 4000000m, null, "111", 20 },
                    { 80, 21, "Lỗ thanh lý", 67666666.66m, null, "811", 20 },
                    { 81, 22, "Giảm nguyên giá TSCĐ", 57000000m, "2112", null, 21 },
                    { 82, 22, "Xóa sổ hao mòn lũy kế", 4750000m, null, "2141", 21 },
                    { 83, 22, "Thu tiền thanh lý bằng tiền mặt", 12000000m, null, "111", 21 },
                    { 84, 22, "Lỗ thanh lý", 40250000m, null, "811", 21 },
                    { 85, 23, "Giảm nguyên giá TSCĐ", 122000000m, "2112", null, 22 },
                    { 86, 23, "Xóa sổ hao mòn lũy kế", 47444444.46m, null, "2141", 22 },
                    { 87, 23, "Thu tiền thanh lý bằng tiền mặt", 9000000m, null, "111", 22 },
                    { 88, 23, "Lỗ thanh lý", 65555555.54m, null, "811", 22 },
                    { 89, 24, "Giảm nguyên giá TSCĐ", 128000000m, "2114", null, 23 },
                    { 90, 24, "Xóa sổ hao mòn lũy kế", 42666666.72m, null, "2141", 23 },
                    { 91, 24, "Thu tiền thanh lý bằng tiền mặt", 14000000m, null, "111", 23 },
                    { 92, 24, "Lỗ thanh lý", 71333333.28m, null, "811", 23 },
                    { 93, 25, "Giảm nguyên giá TSCĐ", 241000000m, "2112", null, 24 },
                    { 94, 25, "Xóa sổ hao mòn lũy kế", 73638888.84m, null, "2141", 24 },
                    { 95, 25, "Thu tiền thanh lý bằng tiền mặt", 9000000m, null, "111", 24 },
                    { 96, 25, "Lỗ thanh lý", 158361111.16m, null, "811", 24 },
                    { 97, 26, "Giảm nguyên giá TSCĐ", 38000000m, "2112", null, 25 },
                    { 98, 26, "Xóa sổ hao mòn lũy kế", 5699999.97m, null, "2141", 25 },
                    { 99, 26, "Thu tiền thanh lý bằng tiền mặt", 6000000m, null, "111", 25 },
                    { 100, 26, "Lỗ thanh lý", 26300000.03m, null, "811", 25 },
                    { 101, 27, "Giảm nguyên giá TSCĐ", 49000000m, "2113", null, 26 },
                    { 102, 27, "Xóa sổ hao mòn lũy kế", 5716666.62m, null, "2141", 26 },
                    { 103, 27, "Thu tiền thanh lý bằng tiền mặt", 3000000m, null, "111", 26 },
                    { 104, 27, "Lỗ thanh lý", 40283333.38m, null, "811", 26 },
                    { 105, 28, "Giảm nguyên giá TSCĐ", 83000000m, "2113", null, 27 },
                    { 106, 28, "Xóa sổ hao mòn lũy kế", 12450000.06m, null, "2141", 27 },
                    { 107, 28, "Thu tiền thanh lý bằng tiền mặt", 14000000m, null, "111", 27 },
                    { 108, 28, "Lỗ thanh lý", 56549999.94m, null, "811", 27 },
                    { 109, 29, "Giảm nguyên giá TSCĐ", 80000000m, "2112", null, 28 },
                    { 110, 29, "Xóa sổ hao mòn lũy kế", 6666666.65m, null, "2141", 28 },
                    { 111, 29, "Thu tiền thanh lý bằng tiền mặt", 14000000m, null, "111", 28 },
                    { 112, 29, "Lỗ thanh lý", 59333333.35m, null, "811", 28 },
                    { 113, 30, "Giảm nguyên giá TSCĐ", 157000000m, "2112", null, 29 },
                    { 114, 30, "Xóa sổ hao mòn lũy kế", 31400000.04m, null, "2141", 29 },
                    { 115, 30, "Thu tiền thanh lý bằng tiền mặt", 4000000m, null, "111", 29 },
                    { 116, 30, "Lỗ thanh lý", 121599999.96m, null, "811", 29 },
                    { 117, 31, "Giảm nguyên giá TSCĐ", 212000000m, "2113", null, 30 },
                    { 118, 31, "Xóa sổ hao mòn lũy kế", 33566666.73m, null, "2141", 30 },
                    { 119, 31, "Thu tiền thanh lý bằng tiền mặt", 8000000m, null, "111", 30 },
                    { 120, 31, "Lỗ thanh lý", 170433333.27m, null, "811", 30 },
                    { 121, 1, "Trích khấu hao NET-0031", 3777777.78m, "2141", "642", 31 },
                    { 122, 1, "Trích khấu hao LAP-0032", 5638888.89m, "2141", "642", 32 },
                    { 123, 1, "Trích khấu hao OTO-0033", 2058333.33m, "2141", "642", 33 },
                    { 124, 1, "Trích khấu hao SRV-0034", 3300000m, "2141", "642", 34 },
                    { 125, 1, "Trích khấu hao OTO-0035", 1200000m, "2141", "642", 35 },
                    { 126, 1, "Trích khấu hao OTO-0036", 1800000m, "2141", "642", 36 },
                    { 127, 1, "Trích khấu hao OTO-0037", 1716666.67m, "2141", "642", 37 },
                    { 128, 1, "Trích khấu hao OTO-0038", 716666.67m, "2141", "642", 38 },
                    { 129, 1, "Trích khấu hao LAP-0039", 5861111.11m, "2141", "642", 39 },
                    { 130, 1, "Trích khấu hao OTO-0040", 883333.33m, "2141", "642", 40 },
                    { 131, 1, "Trích khấu hao LAP-0041", 5083333.33m, "2141", "642", 41 },
                    { 132, 1, "Trích khấu hao NET-0042", 1527777.78m, "2141", "642", 42 },
                    { 133, 1, "Trích khấu hao SRV-0043", 1000000m, "2141", "642", 43 },
                    { 134, 1, "Trích khấu hao OTO-0044", 1475000m, "2141", "642", 44 },
                    { 135, 1, "Trích khấu hao OTO-0045", 1025000m, "2141", "642", 45 },
                    { 136, 1, "Trích khấu hao NET-0046", 5750000m, "2141", "642", 46 },
                    { 137, 1, "Trích khấu hao NET-0047", 4250000m, "2141", "642", 47 },
                    { 138, 1, "Trích khấu hao NET-0048", 3277777.78m, "2141", "642", 48 },
                    { 139, 1, "Trích khấu hao SRV-0049", 1066666.67m, "2141", "642", 49 },
                    { 140, 1, "Trích khấu hao NET-0050", 1472222.22m, "2141", "642", 50 },
                    { 141, 1, "Trích khấu hao NET-0051", 3888888.89m, "2141", "642", 51 },
                    { 142, 1, "Trích khấu hao OTO-0052", 1233333.33m, "2141", "642", 52 },
                    { 143, 1, "Trích khấu hao OTO-0053", 1275000m, "2141", "642", 53 },
                    { 144, 1, "Trích khấu hao LAP-0054", 4388888.89m, "2141", "642", 54 },
                    { 145, 1, "Trích khấu hao OTO-0055", 1800000m, "2141", "642", 55 },
                    { 146, 1, "Trích khấu hao OTO-0056", 1683333.33m, "2141", "642", 56 },
                    { 147, 1, "Trích khấu hao OTO-0057", 1008333.33m, "2141", "642", 57 },
                    { 148, 1, "Trích khấu hao OTO-0058", 450000m, "2141", "642", 58 },
                    { 149, 1, "Trích khấu hao SRV-0059", 2233333.33m, "2141", "642", 59 },
                    { 150, 1, "Trích khấu hao LAP-0060", 3305555.56m, "2141", "642", 60 },
                    { 151, 1, "Trích khấu hao NET-0061", 6944444.44m, "2141", "642", 61 },
                    { 152, 1, "Trích khấu hao NET-0062", 6444444.44m, "2141", "642", 62 },
                    { 153, 1, "Trích khấu hao LAP-0063", 6222222.22m, "2141", "642", 63 },
                    { 154, 1, "Trích khấu hao NET-0064", 2333333.33m, "2141", "642", 64 },
                    { 155, 1, "Trích khấu hao NET-0065", 4194444.44m, "2141", "642", 65 },
                    { 156, 1, "Trích khấu hao SRV-0066", 3733333.33m, "2141", "642", 66 },
                    { 157, 1, "Trích khấu hao LAP-0067", 2861111.11m, "2141", "642", 67 },
                    { 158, 1, "Trích khấu hao SRV-0068", 1966666.67m, "2141", "642", 68 },
                    { 159, 1, "Trích khấu hao SRV-0069", 783333.33m, "2141", "642", 69 },
                    { 160, 1, "Trích khấu hao OTO-0070", 1158333.33m, "2141", "642", 70 },
                    { 161, 1, "Trích khấu hao SRV-0071", 1266666.67m, "2141", "642", 71 },
                    { 162, 1, "Trích khấu hao LAP-0072", 6555555.56m, "2141", "642", 72 },
                    { 163, 1, "Trích khấu hao SRV-0073", 4000000m, "2141", "642", 73 },
                    { 164, 1, "Trích khấu hao OTO-0074", 591666.67m, "2141", "642", 74 },
                    { 165, 1, "Trích khấu hao LAP-0075", 2500000m, "2141", "642", 75 },
                    { 166, 1, "Trích khấu hao LAP-0076", 4444444.44m, "2141", "642", 76 },
                    { 167, 1, "Trích khấu hao SRV-0077", 1400000m, "2141", "642", 77 },
                    { 168, 1, "Trích khấu hao SRV-0078", 616666.67m, "2141", "642", 78 },
                    { 169, 1, "Trích khấu hao LAP-0079", 2305555.56m, "2141", "642", 79 },
                    { 170, 1, "Trích khấu hao NET-0080", 6916666.67m, "2141", "642", 80 },
                    { 171, 1, "Trích khấu hao OTO-0081", 900000m, "2141", "642", 81 },
                    { 172, 1, "Trích khấu hao LAP-0082", 916666.67m, "2141", "642", 82 },
                    { 173, 1, "Trích khấu hao NET-0083", 5027777.78m, "2141", "642", 83 },
                    { 174, 1, "Trích khấu hao SRV-0084", 2483333.33m, "2141", "642", 84 },
                    { 175, 1, "Trích khấu hao SRV-0085", 2750000m, "2141", "642", 85 },
                    { 176, 1, "Trích khấu hao OTO-0086", 1300000m, "2141", "642", 86 },
                    { 177, 1, "Trích khấu hao LAP-0087", 6138888.89m, "2141", "642", 87 },
                    { 178, 1, "Trích khấu hao SRV-0088", 2900000m, "2141", "642", 88 },
                    { 179, 1, "Trích khấu hao LAP-0089", 5722222.22m, "2141", "642", 89 },
                    { 180, 1, "Trích khấu hao SRV-0090", 1500000m, "2141", "642", 90 },
                    { 181, 1, "Trích khấu hao OTO-0091", 1150000m, "2141", "642", 91 },
                    { 182, 1, "Trích khấu hao LAP-0092", 4416666.67m, "2141", "642", 92 },
                    { 183, 1, "Trích khấu hao NET-0093", 3000000m, "2141", "642", 93 },
                    { 184, 1, "Trích khấu hao LAP-0094", 2055555.56m, "2141", "642", 94 },
                    { 185, 1, "Trích khấu hao OTO-0095", 2033333.33m, "2141", "642", 95 },
                    { 186, 1, "Trích khấu hao NET-0096", 6305555.56m, "2141", "642", 96 },
                    { 187, 1, "Trích khấu hao NET-0097", 4500000m, "2141", "642", 97 },
                    { 188, 1, "Trích khấu hao SRV-0098", 1583333.33m, "2141", "642", 98 },
                    { 189, 1, "Trích khấu hao LAP-0099", 1527777.78m, "2141", "642", 99 },
                    { 190, 1, "Trích khấu hao SRV-0100", 3683333.33m, "2141", "642", 100 },
                    { 191, 1, "Trích khấu hao SRV-0101", 3483333.33m, "2141", "642", 101 },
                    { 192, 1, "Trích khấu hao SRV-0102", 1050000m, "2141", "642", 102 },
                    { 193, 1, "Trích khấu hao LAP-0103", 5111111.11m, "2141", "642", 103 },
                    { 194, 1, "Trích khấu hao NET-0104", 1194444.44m, "2141", "642", 104 },
                    { 195, 1, "Trích khấu hao NET-0105", 1500000m, "2141", "642", 105 },
                    { 196, 1, "Trích khấu hao SRV-0106", 3833333.33m, "2141", "642", 106 },
                    { 197, 1, "Trích khấu hao SRV-0107", 1783333.33m, "2141", "642", 107 },
                    { 198, 1, "Trích khấu hao LAP-0108", 1472222.22m, "2141", "642", 108 },
                    { 199, 1, "Trích khấu hao LAP-0109", 1138888.89m, "2141", "642", 109 },
                    { 200, 1, "Trích khấu hao NET-0110", 6055555.56m, "2141", "642", 110 },
                    { 201, 1, "Trích khấu hao SRV-0111", 983333.33m, "2141", "642", 111 },
                    { 202, 1, "Trích khấu hao NET-0112", 4333333.33m, "2141", "642", 112 },
                    { 203, 1, "Trích khấu hao SRV-0113", 1933333.33m, "2141", "642", 113 },
                    { 204, 1, "Trích khấu hao LAP-0114", 4250000m, "2141", "642", 114 },
                    { 205, 1, "Trích khấu hao NET-0115", 5777777.78m, "2141", "642", 115 },
                    { 206, 1, "Trích khấu hao LAP-0116", 4750000m, "2141", "642", 116 },
                    { 207, 1, "Trích khấu hao OTO-0117", 541666.67m, "2141", "642", 117 },
                    { 208, 1, "Trích khấu hao LAP-0118", 5222222.22m, "2141", "642", 118 },
                    { 209, 1, "Trích khấu hao SRV-0119", 2033333.33m, "2141", "642", 119 },
                    { 210, 1, "Trích khấu hao LAP-0120", 6055555.56m, "2141", "642", 120 },
                    { 211, 1, "Trích khấu hao LAP-0121", 5000000m, "2141", "642", 121 },
                    { 212, 1, "Trích khấu hao NET-0122", 5083333.33m, "2141", "642", 122 },
                    { 213, 1, "Trích khấu hao NET-0123", 1500000m, "2141", "642", 123 },
                    { 214, 1, "Trích khấu hao LAP-0124", 4000000m, "2141", "642", 124 },
                    { 215, 1, "Trích khấu hao LAP-0125", 2472222.22m, "2141", "642", 125 },
                    { 216, 1, "Trích khấu hao SRV-0126", 3266666.67m, "2141", "642", 126 },
                    { 217, 1, "Trích khấu hao OTO-0127", 1283333.33m, "2141", "642", 127 },
                    { 218, 1, "Trích khấu hao LAP-0128", 5944444.44m, "2141", "642", 128 },
                    { 219, 1, "Trích khấu hao SRV-0129", 1450000m, "2141", "642", 129 },
                    { 220, 1, "Trích khấu hao NET-0130", 2916666.67m, "2141", "642", 130 },
                    { 221, 1, "Trích khấu hao NET-0131", 2361111.11m, "2141", "642", 131 },
                    { 222, 1, "Trích khấu hao NET-0132", 2416666.67m, "2141", "642", 132 },
                    { 223, 1, "Trích khấu hao SRV-0133", 3400000m, "2141", "642", 133 },
                    { 224, 1, "Trích khấu hao OTO-0134", 375000m, "2141", "642", 134 },
                    { 225, 1, "Trích khấu hao OTO-0135", 541666.67m, "2141", "642", 135 },
                    { 226, 1, "Trích khấu hao SRV-0136", 850000m, "2141", "642", 136 },
                    { 227, 1, "Trích khấu hao NET-0137", 2833333.33m, "2141", "642", 137 },
                    { 228, 1, "Trích khấu hao SRV-0138", 2300000m, "2141", "642", 138 },
                    { 229, 1, "Trích khấu hao NET-0139", 3777777.78m, "2141", "642", 139 },
                    { 230, 1, "Trích khấu hao SRV-0140", 2033333.33m, "2141", "642", 140 },
                    { 231, 1, "Trích khấu hao NET-0141", 4833333.33m, "2141", "642", 141 },
                    { 232, 1, "Trích khấu hao SRV-0142", 566666.67m, "2141", "642", 142 },
                    { 233, 1, "Trích khấu hao OTO-0143", 633333.33m, "2141", "642", 143 },
                    { 234, 1, "Trích khấu hao OTO-0144", 1716666.67m, "2141", "642", 144 },
                    { 235, 1, "Trích khấu hao NET-0145", 3750000m, "2141", "642", 145 },
                    { 236, 1, "Trích khấu hao NET-0146", 1333333.33m, "2141", "642", 146 },
                    { 237, 1, "Trích khấu hao LAP-0147", 4055555.56m, "2141", "642", 147 },
                    { 238, 1, "Trích khấu hao SRV-0148", 1733333.33m, "2141", "642", 148 },
                    { 239, 1, "Trích khấu hao NET-0149", 4694444.44m, "2141", "642", 149 },
                    { 240, 1, "Trích khấu hao OTO-0150", 341666.67m, "2141", "642", 150 },
                    { 241, 1, "Trích khấu hao OTO-0151", 1125000m, "2141", "642", 151 },
                    { 242, 1, "Trích khấu hao LAP-0152", 5500000m, "2141", "642", 152 },
                    { 243, 1, "Trích khấu hao SRV-0153", 2650000m, "2141", "642", 153 },
                    { 244, 1, "Trích khấu hao LAP-0154", 3833333.33m, "2141", "642", 154 },
                    { 245, 1, "Trích khấu hao OTO-0155", 1025000m, "2141", "642", 155 },
                    { 246, 1, "Trích khấu hao NET-0156", 6194444.44m, "2141", "642", 156 },
                    { 247, 1, "Trích khấu hao LAP-0157", 5444444.44m, "2141", "642", 157 },
                    { 248, 1, "Trích khấu hao OTO-0158", 766666.67m, "2141", "642", 158 },
                    { 249, 1, "Trích khấu hao NET-0159", 6250000m, "2141", "642", 159 },
                    { 250, 1, "Trích khấu hao NET-0160", 2500000m, "2141", "642", 160 },
                    { 251, 1, "Trích khấu hao NET-0161", 6361111.11m, "2141", "642", 161 },
                    { 252, 1, "Trích khấu hao NET-0162", 1083333.33m, "2141", "642", 162 },
                    { 253, 1, "Trích khấu hao SRV-0163", 3500000m, "2141", "642", 163 },
                    { 254, 1, "Trích khấu hao LAP-0164", 1194444.44m, "2141", "642", 164 },
                    { 255, 1, "Trích khấu hao OTO-0165", 275000m, "2141", "642", 165 },
                    { 256, 1, "Trích khấu hao SRV-0166", 3850000m, "2141", "642", 166 },
                    { 257, 1, "Trích khấu hao SRV-0167", 2166666.67m, "2141", "642", 167 },
                    { 258, 1, "Trích khấu hao OTO-0168", 1883333.33m, "2141", "642", 168 },
                    { 259, 1, "Trích khấu hao OTO-0169", 1225000m, "2141", "642", 169 },
                    { 260, 1, "Trích khấu hao NET-0170", 6222222.22m, "2141", "642", 170 },
                    { 261, 1, "Trích khấu hao NET-0171", 5916666.67m, "2141", "642", 171 },
                    { 262, 1, "Trích khấu hao LAP-0172", 5055555.56m, "2141", "642", 172 },
                    { 263, 1, "Trích khấu hao LAP-0173", 4916666.67m, "2141", "642", 173 },
                    { 264, 1, "Trích khấu hao NET-0174", 3944444.44m, "2141", "642", 174 },
                    { 265, 1, "Trích khấu hao OTO-0175", 1083333.33m, "2141", "642", 175 },
                    { 266, 1, "Trích khấu hao OTO-0176", 1250000m, "2141", "642", 176 },
                    { 267, 1, "Trích khấu hao LAP-0177", 3638888.89m, "2141", "642", 177 },
                    { 268, 1, "Trích khấu hao SRV-0178", 1050000m, "2141", "642", 178 },
                    { 269, 1, "Trích khấu hao SRV-0179", 2283333.33m, "2141", "642", 179 },
                    { 270, 1, "Trích khấu hao OTO-0180", 1066666.67m, "2141", "642", 180 },
                    { 271, 1, "Trích khấu hao OTO-0181", 1600000m, "2141", "642", 181 },
                    { 272, 1, "Trích khấu hao SRV-0182", 516666.67m, "2141", "642", 182 },
                    { 273, 1, "Trích khấu hao OTO-0183", 1308333.33m, "2141", "642", 183 },
                    { 274, 1, "Trích khấu hao NET-0184", 6055555.56m, "2141", "642", 184 },
                    { 275, 1, "Trích khấu hao OTO-0185", 1150000m, "2141", "642", 185 },
                    { 276, 1, "Trích khấu hao LAP-0186", 4194444.44m, "2141", "642", 186 },
                    { 277, 1, "Trích khấu hao OTO-0187", 333333.33m, "2141", "642", 187 },
                    { 278, 1, "Trích khấu hao NET-0188", 2888888.89m, "2141", "642", 188 },
                    { 279, 1, "Trích khấu hao LAP-0189", 4166666.67m, "2141", "642", 189 },
                    { 280, 1, "Trích khấu hao LAP-0190", 2472222.22m, "2141", "642", 190 },
                    { 281, 1, "Trích khấu hao NET-0191", 4750000m, "2141", "642", 191 },
                    { 282, 1, "Trích khấu hao SRV-0192", 1050000m, "2141", "642", 192 },
                    { 283, 1, "Trích khấu hao OTO-0193", 450000m, "2141", "642", 193 },
                    { 284, 1, "Trích khấu hao SRV-0194", 3233333.33m, "2141", "642", 194 },
                    { 285, 1, "Trích khấu hao OTO-0195", 1025000m, "2141", "642", 195 },
                    { 286, 1, "Trích khấu hao NET-0196", 2444444.44m, "2141", "642", 196 },
                    { 287, 1, "Trích khấu hao NET-0197", 5527777.78m, "2141", "642", 197 },
                    { 288, 1, "Trích khấu hao SRV-0198", 3416666.67m, "2141", "642", 198 },
                    { 289, 1, "Trích khấu hao LAP-0199", 2527777.78m, "2141", "642", 199 },
                    { 290, 1, "Trích khấu hao SRV-0200", 1083333.33m, "2141", "642", 200 },
                    { 291, 1, "Trích khấu hao NET-0201", 5638888.89m, "2141", "642", 201 },
                    { 292, 1, "Trích khấu hao LAP-0202", 6250000m, "2141", "642", 202 },
                    { 293, 1, "Trích khấu hao LAP-0203", 6694444.44m, "2141", "642", 203 },
                    { 294, 1, "Trích khấu hao LAP-0204", 5194444.44m, "2141", "642", 204 },
                    { 295, 1, "Trích khấu hao LAP-0205", 2083333.33m, "2141", "642", 205 },
                    { 296, 1, "Trích khấu hao SRV-0206", 3400000m, "2141", "642", 206 },
                    { 297, 1, "Trích khấu hao NET-0207", 1805555.56m, "2141", "642", 207 },
                    { 298, 1, "Trích khấu hao NET-0208", 2638888.89m, "2141", "642", 208 },
                    { 299, 1, "Trích khấu hao LAP-0209", 1166666.67m, "2141", "642", 209 },
                    { 300, 1, "Trích khấu hao LAP-0210", 3555555.56m, "2141", "642", 210 },
                    { 301, 1, "Trích khấu hao OTO-0211", 400000m, "2141", "642", 211 },
                    { 302, 1, "Trích khấu hao SRV-0212", 650000m, "2141", "642", 212 },
                    { 303, 1, "Trích khấu hao OTO-0213", 1391666.67m, "2141", "642", 213 },
                    { 304, 1, "Trích khấu hao LAP-0214", 3972222.22m, "2141", "642", 214 },
                    { 305, 1, "Trích khấu hao SRV-0215", 2150000m, "2141", "642", 215 },
                    { 306, 1, "Trích khấu hao LAP-0216", 888888.89m, "2141", "642", 216 },
                    { 307, 1, "Trích khấu hao SRV-0217", 1166666.67m, "2141", "642", 217 },
                    { 308, 1, "Trích khấu hao LAP-0218", 5694444.44m, "2141", "642", 218 },
                    { 309, 1, "Trích khấu hao LAP-0219", 3250000m, "2141", "642", 219 },
                    { 310, 1, "Trích khấu hao NET-0220", 4361111.11m, "2141", "642", 220 },
                    { 311, 1, "Trích khấu hao OTO-0221", 408333.33m, "2141", "642", 221 },
                    { 312, 1, "Trích khấu hao NET-0222", 6777777.78m, "2141", "642", 222 },
                    { 313, 1, "Trích khấu hao NET-0223", 5972222.22m, "2141", "642", 223 },
                    { 314, 1, "Trích khấu hao SRV-0224", 2233333.33m, "2141", "642", 224 },
                    { 315, 1, "Trích khấu hao NET-0225", 3777777.78m, "2141", "642", 225 },
                    { 316, 1, "Trích khấu hao OTO-0226", 650000m, "2141", "642", 226 },
                    { 317, 1, "Trích khấu hao SRV-0227", 2216666.67m, "2141", "642", 227 },
                    { 318, 1, "Trích khấu hao OTO-0228", 1908333.33m, "2141", "642", 228 },
                    { 319, 1, "Trích khấu hao LAP-0229", 1583333.33m, "2141", "642", 229 },
                    { 320, 1, "Trích khấu hao OTO-0230", 300000m, "2141", "642", 230 },
                    { 321, 1, "Trích khấu hao NET-0231", 5277777.78m, "2141", "642", 231 },
                    { 322, 1, "Trích khấu hao SRV-0232", 2683333.33m, "2141", "642", 232 },
                    { 323, 1, "Trích khấu hao OTO-0233", 1033333.33m, "2141", "642", 233 },
                    { 324, 1, "Trích khấu hao SRV-0234", 1933333.33m, "2141", "642", 234 },
                    { 325, 1, "Trích khấu hao LAP-0235", 6750000m, "2141", "642", 235 },
                    { 326, 1, "Trích khấu hao OTO-0236", 933333.33m, "2141", "642", 236 },
                    { 327, 1, "Trích khấu hao OTO-0237", 800000m, "2141", "642", 237 },
                    { 328, 1, "Trích khấu hao OTO-0238", 1758333.33m, "2141", "642", 238 },
                    { 329, 1, "Trích khấu hao SRV-0239", 500000m, "2141", "642", 239 },
                    { 330, 1, "Trích khấu hao SRV-0240", 2316666.67m, "2141", "642", 240 },
                    { 331, 1, "Trích khấu hao OTO-0241", 300000m, "2141", "642", 241 },
                    { 332, 1, "Trích khấu hao SRV-0242", 1433333.33m, "2141", "642", 242 },
                    { 333, 1, "Trích khấu hao NET-0243", 5888888.89m, "2141", "642", 243 },
                    { 334, 1, "Trích khấu hao SRV-0244", 3083333.33m, "2141", "642", 244 },
                    { 335, 1, "Trích khấu hao OTO-0245", 1308333.33m, "2141", "642", 245 },
                    { 336, 1, "Trích khấu hao OTO-0246", 1216666.67m, "2141", "642", 246 },
                    { 337, 1, "Trích khấu hao NET-0247", 5444444.44m, "2141", "642", 247 },
                    { 338, 1, "Trích khấu hao NET-0248", 3527777.78m, "2141", "642", 248 },
                    { 339, 1, "Trích khấu hao LAP-0249", 3611111.11m, "2141", "642", 249 },
                    { 340, 1, "Trích khấu hao LAP-0250", 5416666.67m, "2141", "642", 250 }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "lich_su_khau_hao",
                columns: new[] { "Id", "ChungTuId", "ConLaiSauKhauHao", "KyKhauHao", "LuyKeSauKhauHao", "NgayTao", "SoTien", "TaiSanId" },
                values: new object[,]
                {
                    { 1, 1, 79333333.30m, "2025-01", 56666666.70m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7312), 3777777.78m, 31 },
                    { 2, 1, 135333333.32m, "2025-01", 67666666.68m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7331), 5638888.89m, 32 },
                    { 3, 1, 209950000.06m, "2025-01", 37049999.94m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7392), 2058333.33m, 33 },
                    { 4, 1, 171600000m, "2025-01", 26400000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7406), 3300000m, 34 },
                    { 5, 1, 121200000m, "2025-01", 22800000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7417), 1200000m, 35 },
                    { 6, 1, 203400000m, "2025-01", 12600000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7431), 1800000m, 36 },
                    { 7, 1, 183683333.29m, "2025-01", 22316666.71m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7445), 1716666.67m, 37 },
                    { 8, 1, 82416666.65m, "2025-01", 3583333.35m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7457), 716666.67m, 38 },
                    { 9, 1, 175833333.34m, "2025-01", 35166666.66m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7470), 5861111.11m, 39 },
                    { 10, 1, 98050000.03m, "2025-01", 7949999.97m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7532), 883333.33m, 40 },
                    { 11, 1, 127083333.37m, "2025-01", 55916666.63m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7546), 5083333.33m, 41 },
                    { 12, 1, 25972222.18m, "2025-01", 29027777.82m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7559), 1527777.78m, 42 },
                    { 13, 1, 46000000m, "2025-01", 14000000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7571), 1000000m, 43 },
                    { 14, 1, 151925000m, "2025-01", 25075000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7584), 1475000m, 44 },
                    { 15, 1, 115825000m, "2025-01", 7175000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7595), 1025000m, 45 },
                    { 16, 1, 161000000m, "2025-01", 46000000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7607), 5750000m, 46 },
                    { 17, 1, 119000000m, "2025-01", 34000000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7666), 4250000m, 47 },
                    { 18, 1, 85222222.20m, "2025-01", 32777777.80m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7682), 3277777.78m, 48 },
                    { 19, 1, 46933333.28m, "2025-01", 17066666.72m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7694), 1066666.67m, 49 },
                    { 20, 1, 38277777.80m, "2025-01", 14722222.20m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7707), 1472222.22m, 50 },
                    { 21, 1, 108888888.88m, "2025-01", 31111111.12m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7720), 3888888.89m, 51 },
                    { 22, 1, 138133333.36m, "2025-01", 9866666.64m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7733), 1233333.33m, 52 },
                    { 23, 1, 145350000m, "2025-01", 7650000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7745), 1275000m, 53 },
                    { 24, 1, 105333333.32m, "2025-01", 52666666.68m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7757), 4388888.89m, 54 },
                    { 25, 1, 205200000m, "2025-01", 10800000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7815), 1800000m, 55 },
                    { 26, 1, 176750000.05m, "2025-01", 25249999.95m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7828), 1683333.33m, 56 },
                    { 27, 1, 107891666.71m, "2025-01", 13108333.29m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7840), 1008333.33m, 57 },
                    { 28, 1, 51300000m, "2025-01", 2700000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7852), 450000m, 58 },
                    { 29, 1, 113900000.03m, "2025-01", 20099999.97m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7864), 2233333.33m, 59 },
                    { 30, 1, 89249999.96m, "2025-01", 29750000.04m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7877), 3305555.56m, 60 },
                    { 31, 1, 152777777.84m, "2025-01", 97222222.16m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7890), 6944444.44m, 61 },
                    { 32, 1, 109555555.64m, "2025-01", 122444444.36m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7903), 6444444.44m, 62 },
                    { 33, 1, 168000000.02m, "2025-01", 55999999.98m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7942), 6222222.22m, 63 },
                    { 34, 1, 65333333.36m, "2025-01", 18666666.64m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7957), 2333333.33m, 64 },
                    { 35, 1, 92277777.84m, "2025-01", 58722222.16m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7971), 4194444.44m, 65 },
                    { 36, 1, 153066666.73m, "2025-01", 70933333.27m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7984), 3733333.33m, 66 },
                    { 37, 1, 48638888.91m, "2025-01", 54361111.09m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7996), 2861111.11m, 67 },
                    { 38, 1, 82599999.94m, "2025-01", 35400000.06m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8008), 1966666.67m, 68 },
                    { 39, 1, 37600000.04m, "2025-01", 9399999.96m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8107), 783333.33m, 69 },
                    { 40, 1, 126258333.37m, "2025-01", 12741666.63m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8121), 1158333.33m, 70 },
                    { 41, 1, 54466666.61m, "2025-01", 21533333.39m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8134), 1266666.67m, 71 },
                    { 42, 1, 176999999.96m, "2025-01", 59000000.04m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8146), 6555555.56m, 72 },
                    { 43, 1, 172000000m, "2025-01", 68000000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8157), 4000000m, 73 },
                    { 44, 1, 60349999.94m, "2025-01", 10650000.06m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8170), 591666.67m, 74 },
                    { 45, 1, 72500000m, "2025-01", 17500000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8181), 2500000m, 75 },
                    { 46, 1, 120000000.04m, "2025-01", 39999999.96m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8194), 4444444.44m, 76 },
                    { 47, 1, 75600000m, "2025-01", 8400000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8251), 1400000m, 77 },
                    { 48, 1, 30216666.63m, "2025-01", 6783333.37m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8263), 616666.67m, 78 },
                    { 49, 1, 57638888.84m, "2025-01", 25361111.16m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8276), 2305555.56m, 79 },
                    { 50, 1, 200583333.31m, "2025-01", 48416666.69m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8288), 6916666.67m, 80 },
                    { 51, 1, 103500000m, "2025-01", 4500000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8300), 900000m, 81 },
                    { 52, 1, 19249999.95m, "2025-01", 13750000.05m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8313), 916666.67m, 82 },
                    { 53, 1, 95527777.74m, "2025-01", 85472222.26m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8325), 5027777.78m, 83 },
                    { 54, 1, 111750000.05m, "2025-01", 37249999.95m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8337), 2483333.33m, 84 },
                    { 55, 1, 137500000m, "2025-01", 27500000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8398), 2750000m, 85 },
                    { 56, 1, 137800000m, "2025-01", 18200000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8411), 1300000m, 86 },
                    { 57, 1, 147333333.32m, "2025-01", 73666666.68m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8423), 6138888.89m, 87 },
                    { 58, 1, 136300000m, "2025-01", 37700000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8434), 2900000m, 88 },
                    { 59, 1, 143055555.58m, "2025-01", 62944444.42m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8447), 5722222.22m, 89 },
                    { 60, 1, 61500000m, "2025-01", 28500000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8458), 1500000m, 90 },
                    { 61, 1, 124200000m, "2025-01", 13800000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8470), 1150000m, 91 },
                    { 62, 1, 101583333.29m, "2025-01", 57416666.71m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8482), 4416666.67m, 92 },
                    { 63, 1, 60000000m, "2025-01", 48000000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8539), 3000000m, 93 },
                    { 64, 1, 59611111.08m, "2025-01", 14388888.92m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8552), 2055555.56m, 94 },
                    { 65, 1, 219600000.04m, "2025-01", 24399999.96m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8565), 2033333.33m, 95 },
                    { 66, 1, 182861111.08m, "2025-01", 44138888.92m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8580), 6305555.56m, 96 },
                    { 67, 1, 90000000m, "2025-01", 72000000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8592), 4500000m, 97 },
                    { 68, 1, 64916666.73m, "2025-01", 30083333.27m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8605), 1583333.33m, 98 },
                    { 69, 1, 45833333.32m, "2025-01", 9166666.68m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8617), 1527777.78m, 99 },
                    { 70, 1, 184166666.70m, "2025-01", 36833333.30m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8631), 3683333.33m, 100 },
                    { 71, 1, 149783333.39m, "2025-01", 59216666.61m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8691), 3483333.33m, 101 },
                    { 72, 1, 55650000m, "2025-01", 7350000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8703), 1050000m, 102 },
                    { 73, 1, 127777777.79m, "2025-01", 56222222.21m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8715), 5111111.11m, 103 },
                    { 74, 1, 33444444.48m, "2025-01", 9555555.52m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8728), 1194444.44m, 104 },
                    { 75, 1, 42000000m, "2025-01", 12000000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8740), 1500000m, 105 },
                    { 76, 1, 207000000.02m, "2025-01", 22999999.98m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8752), 3833333.33m, 106 },
                    { 77, 1, 76683333.39m, "2025-01", 30316666.61m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8765), 1783333.33m, 107 },
                    { 78, 1, 36805555.58m, "2025-01", 16194444.42m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8777), 1472222.22m, 108 },
                    { 79, 1, 26194444.43m, "2025-01", 14805555.57m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8816), 1138888.89m, 109 },
                    { 80, 1, 145333333.28m, "2025-01", 72666666.72m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8830), 6055555.56m, 110 },
                    { 81, 1, 45233333.38m, "2025-01", 13766666.62m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8842), 983333.33m, 111 },
                    { 82, 1, 91000000.05m, "2025-01", 64999999.95m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8855), 4333333.33m, 112 },
                    { 83, 1, 88933333.38m, "2025-01", 27066666.62m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8866), 1933333.33m, 113 },
                    { 84, 1, 102000000m, "2025-01", 51000000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8878), 4250000m, 114 },
                    { 85, 1, 173333333.32m, "2025-01", 34666666.68m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8891), 5777777.78m, 115 },
                    { 86, 1, 147250000m, "2025-01", 23750000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8903), 4750000m, 116 },
                    { 87, 1, 60124999.97m, "2025-01", 4875000.03m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(8990), 541666.67m, 117 },
                    { 88, 1, 146222222.24m, "2025-01", 41777777.76m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9003), 5222222.22m, 118 },
                    { 89, 1, 95566666.71m, "2025-01", 26433333.29m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9015), 2033333.33m, 119 },
                    { 90, 1, 145333333.28m, "2025-01", 72666666.72m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9028), 6055555.56m, 120 },
                    { 91, 1, 90000000m, "2025-01", 90000000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9039), 5000000m, 121 },
                    { 92, 1, 86416666.73m, "2025-01", 96583333.27m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9052), 5083333.33m, 122 },
                    { 93, 1, 40500000m, "2025-01", 13500000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9064), 1500000m, 123 },
                    { 94, 1, 92000000m, "2025-01", 52000000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9075), 4000000m, 124 },
                    { 95, 1, 46972222.26m, "2025-01", 42027777.74m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9163), 2472222.22m, 125 },
                    { 96, 1, 140466666.61m, "2025-01", 55533333.39m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9176), 3266666.67m, 126 },
                    { 97, 1, 142450000.03m, "2025-01", 11549999.97m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9188), 1283333.33m, 127 },
                    { 98, 1, 101055555.64m, "2025-01", 112944444.36m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9200), 5944444.44m, 128 },
                    { 99, 1, 79750000m, "2025-01", 7250000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9214), 1450000m, 129 },
                    { 100, 1, 58333333.28m, "2025-01", 46666666.72m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9227), 2916666.67m, 130 },
                    { 101, 1, 68472222.23m, "2025-01", 16527777.77m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9285), 2361111.11m, 131 },
                    { 102, 1, 50749999.95m, "2025-01", 36250000.05m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9299), 2416666.67m, 132 },
                    { 103, 1, 176800000m, "2025-01", 27200000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9311), 3400000m, 133 },
                    { 104, 1, 38250000m, "2025-01", 6750000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9323), 375000m, 134 },
                    { 105, 1, 60124999.97m, "2025-01", 4875000.03m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9335), 541666.67m, 135 },
                    { 106, 1, 34850000m, "2025-01", 16150000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9347), 850000m, 136 },
                    { 107, 1, 70833333.37m, "2025-01", 31166666.63m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9359), 2833333.33m, 137 },
                    { 108, 1, 101200000m, "2025-01", 36800000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9372), 2300000m, 138 },
                    { 109, 1, 64222222.18m, "2025-01", 71777777.82m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9430), 3777777.78m, 139 },
                    { 110, 1, 85400000.06m, "2025-01", 36599999.94m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9444), 2033333.33m, 140 },
                    { 111, 1, 130500000.03m, "2025-01", 43499999.97m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9456), 4833333.33m, 141 },
                    { 112, 1, 24933333.28m, "2025-01", 9066666.72m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9468), 566666.67m, 142 },
                    { 113, 1, 65866666.72m, "2025-01", 10133333.28m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9481), 633333.33m, 143 },
                    { 114, 1, 197416666.65m, "2025-01", 8583333.35m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9494), 1716666.67m, 144 },
                    { 115, 1, 63750000m, "2025-01", 71250000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9506), 3750000m, 145 },
                    { 116, 1, 22666666.73m, "2025-01", 25333333.27m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9519), 1333333.33m, 146 },
                    { 117, 1, 121666666.64m, "2025-01", 24333333.36m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9531), 4055555.56m, 147 },
                    { 118, 1, 83200000.04m, "2025-01", 20799999.96m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9590), 1733333.33m, 148 },
                    { 119, 1, 117361111.16m, "2025-01", 51638888.84m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9604), 4694444.44m, 149 },
                    { 120, 1, 36558333.29m, "2025-01", 4441666.71m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9617), 341666.67m, 150 },
                    { 121, 1, 114750000m, "2025-01", 20250000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9628), 1125000m, 151 },
                    { 122, 1, 170500000m, "2025-01", 27500000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9641), 5500000m, 152 },
                    { 123, 1, 129850000m, "2025-01", 29150000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9652), 2650000m, 153 },
                    { 124, 1, 118833333.35m, "2025-01", 19166666.65m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9665), 3833333.33m, 154 },
                    { 125, 1, 106600000m, "2025-01", 16400000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9676), 1025000m, 155 },
                    { 126, 1, 179638888.92m, "2025-01", 43361111.08m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9733), 6194444.44m, 156 },
                    { 127, 1, 125222222.28m, "2025-01", 70777777.72m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9746), 5444444.44m, 157 },
                    { 128, 1, 78966666.61m, "2025-01", 13033333.39m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9759), 766666.67m, 158 },
                    { 129, 1, 175000000m, "2025-01", 50000000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9771), 6250000m, 159 },
                    { 130, 1, 42500000m, "2025-01", 47500000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9785), 2500000m, 160 },
                    { 131, 1, 165388888.90m, "2025-01", 63611111.10m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9799), 6361111.11m, 161 },
                    { 132, 1, 23833333.38m, "2025-01", 15166666.62m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9857), 1083333.33m, 162 },
                    { 133, 1, 171500000m, "2025-01", 38500000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9869), 3500000m, 163 },
                    { 134, 1, 27472222.28m, "2025-01", 15527777.72m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9881), 1194444.44m, 164 },
                    { 135, 1, 28875000m, "2025-01", 4125000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9893), 275000m, 165 },
                    { 136, 1, 204050000m, "2025-01", 26950000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9905), 3850000m, 166 },
                    { 137, 1, 88833333.27m, "2025-01", 41166666.73m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9917), 2166666.67m, 167 },
                    { 138, 1, 192100000.06m, "2025-01", 33899999.94m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9977), 1883333.33m, 168 },
                    { 139, 1, 128625000m, "2025-01", 18375000m, new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(9990), 1225000m, 169 },
                    { 140, 1, 105777777.82m, "2025-01", 118222222.18m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(3), 6222222.22m, 170 },
                    { 141, 1, 106499999.94m, "2025-01", 106500000.06m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(15), 5916666.67m, 171 },
                    { 142, 1, 136499999.96m, "2025-01", 45500000.04m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(27), 5055555.56m, 172 },
                    { 143, 1, 88499999.94m, "2025-01", 88500000.06m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(41), 4916666.67m, 173 },
                    { 144, 1, 114388888.92m, "2025-01", 27611111.08m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(121), 3944444.44m, 174 },
                    { 145, 1, 119166666.70m, "2025-01", 10833333.30m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(135), 1083333.33m, 175 },
                    { 146, 1, 131250000m, "2025-01", 18750000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(147), 1250000m, 176 },
                    { 147, 1, 101888888.88m, "2025-01", 29111111.12m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(160), 3638888.89m, 177 },
                    { 148, 1, 43050000m, "2025-01", 19950000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(171), 1050000m, 178 },
                    { 149, 1, 93616666.73m, "2025-01", 43383333.27m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(185), 2283333.33m, 179 },
                    { 150, 1, 121599999.98m, "2025-01", 6400000.02m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(197), 1066666.67m, 180 },
                    { 151, 1, 161600000m, "2025-01", 30400000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(209), 1600000m, 181 },
                    { 152, 1, 26349999.97m, "2025-01", 4650000.03m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(287), 516666.67m, 182 },
                    { 153, 1, 149150000.02m, "2025-01", 7849999.98m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(300), 1308333.33m, 183 },
                    { 154, 1, 181666666.64m, "2025-01", 36333333.36m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(312), 6055555.56m, 184 },
                    { 155, 1, 132250000m, "2025-01", 5750000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(324), 1150000m, 185 },
                    { 156, 1, 104861111.16m, "2025-01", 46138888.84m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(336), 4194444.44m, 186 },
                    { 157, 1, 33666666.73m, "2025-01", 6333333.27m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(348), 333333.33m, 187 },
                    { 158, 1, 89555555.55m, "2025-01", 14444444.45m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(361), 2888888.89m, 188 },
                    { 159, 1, 108333333.30m, "2025-01", 41666666.70m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(373), 4166666.67m, 189 },
                    { 160, 1, 64277777.80m, "2025-01", 24722222.20m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(385), 2472222.22m, 190 },
                    { 161, 1, 109250000m, "2025-01", 61750000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(443), 4750000m, 191 },
                    { 162, 1, 49350000m, "2025-01", 13650000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(455), 1050000m, 192 },
                    { 163, 1, 50400000m, "2025-01", 3600000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(467), 450000m, 193 },
                    { 164, 1, 171366666.69m, "2025-01", 22633333.31m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(479), 3233333.33m, 194 },
                    { 165, 1, 116850000m, "2025-01", 6150000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(490), 1025000m, 195 },
                    { 166, 1, 61111111.16m, "2025-01", 26888888.84m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(503), 2444444.44m, 196 },
                    { 167, 1, 165833333.32m, "2025-01", 33166666.68m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(516), 5527777.78m, 197 },
                    { 168, 1, 163999999.96m, "2025-01", 41000000.04m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(528), 3416666.67m, 198 },
                    { 169, 1, 58138888.86m, "2025-01", 32861111.14m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(586), 2527777.78m, 199 },
                    { 170, 1, 50916666.71m, "2025-01", 14083333.29m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(598), 1083333.33m, 200 },
                    { 171, 1, 174805555.55m, "2025-01", 28194444.45m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(611), 5638888.89m, 201 },
                    { 172, 1, 168750000m, "2025-01", 56250000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(623), 6250000m, 202 },
                    { 173, 1, 160666666.72m, "2025-01", 80333333.28m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(635), 6694444.44m, 203 },
                    { 174, 1, 135055555.60m, "2025-01", 51944444.40m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(647), 5194444.44m, 204 },
                    { 175, 1, 56250000.03m, "2025-01", 18749999.97m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(660), 2083333.33m, 205 },
                    { 176, 1, 176800000m, "2025-01", 27200000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(672), 3400000m, 206 },
                    { 177, 1, 54166666.64m, "2025-01", 10833333.36m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(730), 1805555.56m, 207 },
                    { 178, 1, 71249999.99m, "2025-01", 23750000.01m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(743), 2638888.89m, 208 },
                    { 179, 1, 25666666.62m, "2025-01", 16333333.38m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(755), 1166666.67m, 209 },
                    { 180, 1, 71111111.04m, "2025-01", 56888888.96m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(768), 3555555.56m, 210 },
                    { 181, 1, 46000000m, "2025-01", 2000000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(780), 400000m, 211 },
                    { 182, 1, 26650000m, "2025-01", 12350000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(791), 650000m, 212 },
                    { 183, 1, 158649999.98m, "2025-01", 8350000.02m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(804), 1391666.67m, 213 },
                    { 184, 1, 95333333.36m, "2025-01", 47666666.64m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(816), 3972222.22m, 214 },
                    { 185, 1, 96750000m, "2025-01", 32250000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(873), 2150000m, 215 },
                    { 186, 1, 26666666.66m, "2025-01", 5333333.34m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(886), 888888.89m, 216 },
                    { 187, 1, 50166666.61m, "2025-01", 19833333.39m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(899), 1166666.67m, 217 },
                    { 188, 1, 153750000.04m, "2025-01", 51249999.96m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(911), 5694444.44m, 218 },
                    { 189, 1, 55250000m, "2025-01", 61750000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(923), 3250000m, 219 },
                    { 190, 1, 122111111.12m, "2025-01", 34888888.88m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(936), 4361111.11m, 220 },
                    { 191, 1, 46958333.35m, "2025-01", 2041666.65m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(949), 408333.33m, 221 },
                    { 192, 1, 142333333.30m, "2025-01", 101666666.70m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(962), 6777777.78m, 222 },
                    { 193, 1, 179166666.68m, "2025-01", 35833333.32m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1022), 5972222.22m, 223 },
                    { 194, 1, 93800000.06m, "2025-01", 40199999.94m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1036), 2233333.33m, 224 },
                    { 195, 1, 94444444.42m, "2025-01", 41555555.58m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1048), 3777777.78m, 225 },
                    { 196, 1, 74100000m, "2025-01", 3900000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1060), 650000m, 226 },
                    { 197, 1, 119699999.98m, "2025-01", 13300000.02m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1073), 2216666.67m, 227 },
                    { 198, 1, 219458333.35m, "2025-01", 9541666.65m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1085), 1908333.33m, 228 },
                    { 199, 1, 36416666.71m, "2025-01", 20583333.29m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1097), 1583333.33m, 229 },
                    { 200, 1, 33600000m, "2025-01", 2400000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1109), 300000m, 230 },
                    { 201, 1, 126666666.64m, "2025-01", 63333333.36m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1121), 5277777.78m, 231 },
                    { 202, 1, 142216666.69m, "2025-01", 18783333.31m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1160), 2683333.33m, 232 },
                    { 203, 1, 116766666.69m, "2025-01", 7233333.31m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1173), 1033333.33m, 233 },
                    { 204, 1, 85066666.72m, "2025-01", 30933333.28m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1185), 1933333.33m, 234 },
                    { 205, 1, 128250000m, "2025-01", 114750000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1196), 6750000m, 235 },
                    { 206, 1, 98000000.05m, "2025-01", 13999999.95m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1209), 933333.33m, 236 },
                    { 207, 1, 88800000m, "2025-01", 7200000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1221), 800000m, 237 },
                    { 208, 1, 186383333.38m, "2025-01", 24616666.62m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1233), 1758333.33m, 238 },
                    { 209, 1, 23500000m, "2025-01", 6500000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1244), 500000m, 239 },
                    { 210, 1, 122783333.31m, "2025-01", 16216666.69m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1326), 2316666.67m, 240 },
                    { 211, 1, 30600000m, "2025-01", 5400000m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1338), 300000m, 241 },
                    { 212, 1, 65933333.38m, "2025-01", 20066666.62m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1351), 1433333.33m, 242 },
                    { 213, 1, 170777777.77m, "2025-01", 41222222.23m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1364), 5888888.89m, 243 },
                    { 214, 1, 151083333.37m, "2025-01", 33916666.63m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1376), 3083333.33m, 244 },
                    { 215, 1, 141300000.04m, "2025-01", 15699999.96m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1389), 1308333.33m, 245 },
                    { 216, 1, 128966666.62m, "2025-01", 17033333.38m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1402), 1216666.67m, 246 },
                    { 217, 1, 98000000.08m, "2025-01", 97999999.92m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1415), 5444444.44m, 247 },
                    { 218, 1, 91722222.20m, "2025-01", 35277777.80m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1493), 3527777.78m, 248 },
                    { 219, 1, 72222222.24m, "2025-01", 57777777.76m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1505), 3611111.11m, 249 },
                    { 220, 1, 146249999.97m, "2025-01", 48750000.03m, new DateTime(2026, 3, 30, 16, 9, 39, 648, DateTimeKind.Utc).AddTicks(1517), 5416666.67m, 250 }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "thanh_ly_tai_san",
                columns: new[] { "Id", "GhiChu", "GiaTriConLai", "GiaTriThanhLy", "KhauHaoLuyKe", "LaiLo", "LyDo", "NgayTao", "NgayThanhLy", "NguyenGia", "TaiSanId", "TrangThai" },
                values: new object[,]
                {
                    { 1, null, 134000000.04m, 8000000m, 66999999.96m, -126000000.04m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6291), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 201000000m, 1, "DaHoanThanh" },
                    { 2, null, 66305555.55m, 6000000m, 10694444.45m, -60305555.55m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6342), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 77000000m, 2, "DaHoanThanh" },
                    { 3, null, 63583333.30m, 5000000m, 45416666.70m, -58583333.30m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6364), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 109000000m, 3, "DaHoanThanh" },
                    { 4, null, 129600000m, 8000000m, 14400000m, -121600000m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6435), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 144000000m, 4, "DaHoanThanh" },
                    { 5, null, 82324999.97m, 13000000m, 6675000.03m, -69324999.97m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6456), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 89000000m, 5, "DaHoanThanh" },
                    { 6, null, 114300000.04m, 4000000m, 12699999.96m, -110300000.04m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6478), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 127000000m, 6, "DaHoanThanh" },
                    { 7, null, 90416666.65m, 6000000m, 14583333.35m, -84416666.65m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6496), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 105000000m, 7, "DaHoanThanh" },
                    { 8, null, 116416666.70m, 5000000m, 10583333.30m, -111416666.70m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6513), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 127000000m, 8, "DaHoanThanh" },
                    { 9, null, 122200000m, 11000000m, 33800000m, -111200000m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6580), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 156000000m, 9, "DaHoanThanh" },
                    { 10, null, 114000000.02m, 10000000m, 37999999.98m, -104000000.02m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6602), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 152000000m, 10, "DaHoanThanh" },
                    { 11, null, 34400000m, 6000000m, 13600000m, -28400000m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6619), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 48000000m, 11, "DaHoanThanh" },
                    { 12, null, 190900000m, 11000000m, 58100000m, -179900000m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6636), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 249000000m, 12, "DaHoanThanh" },
                    { 13, null, 82500000m, 6000000m, 7500000m, -76500000m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6681), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 90000000m, 13, "DaHoanThanh" },
                    { 14, null, 149875000m, 10000000m, 15125000m, -139875000m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6698), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 165000000m, 14, "DaHoanThanh" },
                    { 15, null, 55833333.34m, 4000000m, 11166666.66m, -51833333.34m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6720), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 67000000m, 15, "DaHoanThanh" },
                    { 16, null, 161250000.05m, 6000000m, 53749999.95m, -155250000.05m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6738), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 215000000m, 16, "DaHoanThanh" },
                    { 17, null, 32000000.04m, 6000000m, 15999999.96m, -26000000.04m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6756), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 48000000m, 17, "DaHoanThanh" },
                    { 18, null, 80666666.72m, 13000000m, 29333333.28m, -67666666.72m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6867), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 110000000m, 18, "DaHoanThanh" },
                    { 19, null, 111361111.13m, 5000000m, 99638888.87m, -106361111.13m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6885), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 211000000m, 19, "DaHoanThanh" },
                    { 20, null, 71666666.66m, 4000000m, 14333333.34m, -67666666.66m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6904), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 86000000m, 20, "DaHoanThanh" },
                    { 21, null, 52250000m, 12000000m, 4750000m, -40250000m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(6920), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 57000000m, 21, "DaHoanThanh" },
                    { 22, null, 74555555.54m, 9000000m, 47444444.46m, -65555555.54m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7018), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 122000000m, 22, "DaHoanThanh" },
                    { 23, null, 85333333.28m, 14000000m, 42666666.72m, -71333333.28m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7050), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 128000000m, 23, "DaHoanThanh" },
                    { 24, null, 167361111.16m, 9000000m, 73638888.84m, -158361111.16m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7128), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 241000000m, 24, "DaHoanThanh" },
                    { 25, null, 32300000.03m, 6000000m, 5699999.97m, -26300000.03m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7150), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 38000000m, 25, "DaHoanThanh" },
                    { 26, null, 43283333.38m, 3000000m, 5716666.62m, -40283333.38m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7169), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 49000000m, 26, "DaHoanThanh" },
                    { 27, null, 70549999.94m, 14000000m, 12450000.06m, -56549999.94m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7235), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 83000000m, 27, "DaHoanThanh" },
                    { 28, null, 73333333.35m, 14000000m, 6666666.65m, -59333333.35m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7255), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 80000000m, 28, "DaHoanThanh" },
                    { 29, null, 125599999.96m, 4000000m, 31400000.04m, -121599999.96m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7272), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 157000000m, 29, "DaHoanThanh" },
                    { 30, null, 178433333.27m, 8000000m, 33566666.73m, -170433333.27m, "Hư hỏng không thể sửa chữa", new DateTime(2026, 3, 30, 16, 9, 39, 647, DateTimeKind.Utc).AddTicks(7290), new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 212000000m, 30, "DaHoanThanh" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_bao_tri_tai_san_TaiSanId",
                schema: "asset",
                table: "bao_tri_tai_san",
                column: "TaiSanId");

            migrationBuilder.CreateIndex(
                name: "IX_chi_tiet_chung_tu_ChungTuId",
                schema: "asset",
                table: "chi_tiet_chung_tu",
                column: "ChungTuId");

            migrationBuilder.CreateIndex(
                name: "IX_chi_tiet_chung_tu_TaiKhoanCo",
                schema: "asset",
                table: "chi_tiet_chung_tu",
                column: "TaiKhoanCo");

            migrationBuilder.CreateIndex(
                name: "IX_chi_tiet_chung_tu_TaiKhoanNo",
                schema: "asset",
                table: "chi_tiet_chung_tu",
                column: "TaiKhoanNo");

            migrationBuilder.CreateIndex(
                name: "IX_chi_tiet_chung_tu_TaiSanId",
                schema: "asset",
                table: "chi_tiet_chung_tu",
                column: "TaiSanId");

            migrationBuilder.CreateIndex(
                name: "IX_chung_tu_MaChungTu",
                schema: "asset",
                table: "chung_tu",
                column: "MaChungTu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_danh_muc_tai_san_MaDanhMuc",
                schema: "asset",
                table: "danh_muc_tai_san",
                column: "MaDanhMuc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_danh_muc_tai_san_MaTaiKhoan",
                schema: "asset",
                table: "danh_muc_tai_san",
                column: "MaTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_dieu_chuyen_tai_san_DenPhongBanId",
                schema: "asset",
                table: "dieu_chuyen_tai_san",
                column: "DenPhongBanId");

            migrationBuilder.CreateIndex(
                name: "IX_dieu_chuyen_tai_san_TaiSanId",
                schema: "asset",
                table: "dieu_chuyen_tai_san",
                column: "TaiSanId");

            migrationBuilder.CreateIndex(
                name: "IX_dieu_chuyen_tai_san_TuPhongBanId",
                schema: "asset",
                table: "dieu_chuyen_tai_san",
                column: "TuPhongBanId");

            migrationBuilder.CreateIndex(
                name: "IX_lich_su_khau_hao_ChungTuId",
                schema: "asset",
                table: "lich_su_khau_hao",
                column: "ChungTuId");

            migrationBuilder.CreateIndex(
                name: "IX_lich_su_khau_hao_TaiSanId",
                schema: "asset",
                table: "lich_su_khau_hao",
                column: "TaiSanId");

            migrationBuilder.CreateIndex(
                name: "IX_lo_tai_san_MaLo",
                schema: "asset",
                table: "lo_tai_san",
                column: "MaLo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_phong_ban_MaPhongBan",
                schema: "asset",
                table: "phong_ban",
                column: "MaPhongBan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tai_khoan_ke_toan_MaTaiKhoan",
                schema: "asset",
                table: "tai_khoan_ke_toan",
                column: "MaTaiKhoan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tai_khoan_ke_toan_MaTaiKhoanCha",
                schema: "asset",
                table: "tai_khoan_ke_toan",
                column: "MaTaiKhoanCha");

            migrationBuilder.CreateIndex(
                name: "IX_tai_san_DanhMucId",
                schema: "asset",
                table: "tai_san",
                column: "DanhMucId");

            migrationBuilder.CreateIndex(
                name: "IX_tai_san_LoId",
                schema: "asset",
                table: "tai_san",
                column: "LoId");

            migrationBuilder.CreateIndex(
                name: "IX_tai_san_MaTaiKhoan",
                schema: "asset",
                table: "tai_san",
                column: "MaTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_tai_san_MaTaiSan",
                schema: "asset",
                table: "tai_san",
                column: "MaTaiSan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tai_san_PhongBanId",
                schema: "asset",
                table: "tai_san",
                column: "PhongBanId");

            migrationBuilder.CreateIndex(
                name: "IX_thanh_ly_tai_san_TaiSanId",
                schema: "asset",
                table: "thanh_ly_tai_san",
                column: "TaiSanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bao_tri_tai_san",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "cau_hinh_he_thong",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "chi_tiet_chung_tu",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "dieu_chuyen_tai_san",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "lich_su_khau_hao",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "thanh_ly_tai_san",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "chung_tu",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "tai_san",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "danh_muc_tai_san",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "lo_tai_san",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "phong_ban",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "tai_khoan_ke_toan",
                schema: "asset");
        }
    }
}
