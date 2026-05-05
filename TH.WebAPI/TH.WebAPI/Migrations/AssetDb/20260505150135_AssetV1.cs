using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TH.WebAPI.Migrations.AssetDb
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
                    PhuongThucThanhToan = table.Column<string>(type: "text", nullable: true),
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
                values: new object[] { 1, "Tầng 8, Tòa nhà Sailing Tower, 111A Pasteur, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh", "{DANH_MUC}-{SO_THU_TU}", 4, "0316789456", "DuongThang", 1, "Công ty Cổ phần Công nghệ TH", "CT", true });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "chung_tu",
                columns: new[] { "Id", "LoaiChungTu", "MaChungTu", "MoTa", "NgayLap", "NgayTao", "NguoiLapId", "TongTien", "TrangThai" },
                values: new object[,]
                {
                    { 1, "KhauHao", "CT-KH-2025-01", "Trích khấu hao TSCĐ tháng 01/2025", new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 106577777.81m, "hoan_thanh" },
                    { 2, "GhiTang", "CT-GT-075", "Ghi tăng tài sản LAP-0075 khi mua", new DateTime(2019, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 18000000m, "hoan_thanh" },
                    { 3, "ThanhLy", "CT-TL-075", "Thanh lý tài sản LAP-0075 – hỏng, hết khấu hao", new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 18000000m, "hoan_thanh" },
                    { 4, "GhiTang", "CT-GT-076", "Ghi tăng tài sản LAP-0076 khi mua", new DateTime(2019, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 16000000m, "hoan_thanh" },
                    { 5, "ThanhLy", "CT-TL-076", "Thanh lý tài sản LAP-0076 – hỏng, hết khấu hao", new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 16000000m, "hoan_thanh" },
                    { 6, "GhiTang", "CT-GT-077", "Ghi tăng tài sản LAP-0077 khi mua", new DateTime(2019, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 17000000m, "hoan_thanh" },
                    { 7, "ThanhLy", "CT-TL-077", "Thanh lý tài sản LAP-0077 – hỏng, hết khấu hao", new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 17000000m, "hoan_thanh" },
                    { 8, "GhiTang", "CT-GT-078", "Ghi tăng tài sản LAP-0078 khi mua", new DateTime(2019, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 14000000m, "hoan_thanh" },
                    { 9, "ThanhLy", "CT-TL-078", "Thanh lý tài sản LAP-0078 – hỏng, hết khấu hao", new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 14000000m, "hoan_thanh" },
                    { 10, "GhiTang", "CT-GT-079", "Ghi tăng tài sản LAP-0079 khi mua", new DateTime(2019, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 15000000m, "hoan_thanh" },
                    { 11, "ThanhLy", "CT-TL-079", "Thanh lý tài sản LAP-0079 – hỏng, hết khấu hao", new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 15000000m, "hoan_thanh" },
                    { 12, "GhiTang", "CT-GT-080", "Ghi tăng tài sản LAP-0080 khi mua", new DateTime(2019, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 13000000m, "hoan_thanh" },
                    { 13, "ThanhLy", "CT-TL-080", "Thanh lý tài sản LAP-0080 – hỏng, hết khấu hao", new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 13000000m, "hoan_thanh" },
                    { 14, "GhiTang", "CT-GT-081", "Ghi tăng tài sản NET-0081 khi mua", new DateTime(2019, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 15, "ThanhLy", "CT-TL-081", "Thanh lý tài sản NET-0081 – hỏng, hết khấu hao", new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 16, "GhiTang", "CT-GT-082", "Ghi tăng tài sản NET-0082 khi mua", new DateTime(2019, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 12000000m, "hoan_thanh" },
                    { 17, "ThanhLy", "CT-TL-082", "Thanh lý tài sản NET-0082 – hỏng, hết khấu hao", new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 12000000m, "hoan_thanh" },
                    { 18, "GhiTang", "CT-GT-083", "Ghi tăng tài sản SRV-0083 khi mua", new DateTime(2019, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 8000000m, "hoan_thanh" },
                    { 19, "ThanhLy", "CT-TL-083", "Thanh lý tài sản SRV-0083 – hỏng, hết khấu hao", new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 8000000m, "hoan_thanh" },
                    { 20, "GhiTang", "CT-GT-084", "Ghi tăng tài sản SRV-0084 khi mua", new DateTime(2019, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 5000000m, "hoan_thanh" },
                    { 21, "ThanhLy", "CT-TL-084", "Thanh lý tài sản SRV-0084 – hỏng, hết khấu hao", new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 5000000m, "hoan_thanh" },
                    { 22, "GhiTang", "CT-GT-085", "Ghi tăng TSCĐ LAP-0085 – nhập kho theo HĐ số 1000", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 28000000m, "hoan_thanh" },
                    { 23, "GhiTang", "CT-GT-086", "Ghi tăng TSCĐ LAP-0086 – nhập kho theo HĐ số 1001", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 28000000m, "hoan_thanh" },
                    { 24, "GhiTang", "CT-GT-087", "Ghi tăng TSCĐ LAP-0087 – nhập kho theo HĐ số 1002", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 26000000m, "hoan_thanh" },
                    { 25, "GhiTang", "CT-GT-088", "Ghi tăng TSCĐ LAP-0088 – nhập kho theo HĐ số 1003", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 26000000m, "hoan_thanh" },
                    { 26, "GhiTang", "CT-GT-089", "Ghi tăng TSCĐ LAP-0089 – nhập kho theo HĐ số 1004", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 22000000m, "hoan_thanh" },
                    { 27, "GhiTang", "CT-GT-090", "Ghi tăng TSCĐ LAP-0090 – nhập kho theo HĐ số 1005", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 22000000m, "hoan_thanh" },
                    { 28, "GhiTang", "CT-GT-091", "Ghi tăng TSCĐ LAP-0091 – nhập kho theo HĐ số 1006", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 30000000m, "hoan_thanh" },
                    { 29, "GhiTang", "CT-GT-092", "Ghi tăng TSCĐ LAP-0092 – nhập kho theo HĐ số 1007", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 30000000m, "hoan_thanh" },
                    { 30, "GhiTang", "CT-GT-093", "Ghi tăng TSCĐ LAP-0093 – nhập kho theo HĐ số 1008", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 29000000m, "hoan_thanh" },
                    { 31, "GhiTang", "CT-GT-094", "Ghi tăng TSCĐ LAP-0094 – nhập kho theo HĐ số 1009", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 29000000m, "hoan_thanh" },
                    { 32, "GhiTang", "CT-GT-095", "Ghi tăng TSCĐ LAP-0095 – nhập kho theo HĐ số 1010", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 33, "GhiTang", "CT-GT-096", "Ghi tăng TSCĐ LAP-0096 – nhập kho theo HĐ số 1011", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 34, "GhiTang", "CT-GT-097", "Ghi tăng TSCĐ LAP-0097 – nhập kho theo HĐ số 1012", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" },
                    { 35, "GhiTang", "CT-GT-098", "Ghi tăng TSCĐ LAP-0098 – nhập kho theo HĐ số 1013", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" },
                    { 36, "GhiTang", "CT-GT-099", "Ghi tăng TSCĐ LAP-0099 – nhập kho theo HĐ số 1014", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 48000000m, "hoan_thanh" },
                    { 37, "GhiTang", "CT-GT-100", "Ghi tăng TSCĐ LAP-0100 – nhập kho theo HĐ số 1015", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 48000000m, "hoan_thanh" },
                    { 38, "GhiTang", "CT-GT-001", "Ghi tăng tài sản LAP-0001 – Apple MacBook Pro 16\" M3 Pro (48GB/1TB)", new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 85000000m, "hoan_thanh" },
                    { 39, "GhiTang", "CT-GT-002", "Ghi tăng tài sản LAP-0002 – Apple iPad Pro 12.9\" M2 (WiFi+5G)", new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 32000000m, "hoan_thanh" },
                    { 40, "GhiTang", "CT-GT-003", "Ghi tăng tài sản LAP-0003 – Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 41, "GhiTang", "CT-GT-004", "Ghi tăng tài sản LAP-0004 – Dell Latitude 5430 (i7/16GB/512GB SSD)", new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 28000000m, "hoan_thanh" },
                    { 42, "GhiTang", "CT-GT-005", "Ghi tăng tài sản LAP-0005 – Apple MacBook Air 15\" M2 (8GB/512GB)", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 43, "GhiTang", "CT-GT-006", "Ghi tăng tài sản LAP-0006 – HP EliteBook 840 G9 (i5/16GB/512GB)", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 24000000m, "hoan_thanh" },
                    { 44, "GhiTang", "CT-GT-007", "Ghi tăng tài sản LAP-0007 – Lenovo ThinkPad E14 Gen 4 (i5/8GB/256GB)", new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 18000000m, "hoan_thanh" },
                    { 45, "GhiTang", "CT-GT-008", "Ghi tăng tài sản LAP-0008 – Asus VivoBook 15 OLED (i5/16GB/512GB)", new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 16000000m, "hoan_thanh" },
                    { 46, "GhiTang", "CT-GT-009", "Ghi tăng tài sản OTO-0009 – Toyota Innova Crysta 2.0G MT 2023 (7 chỗ)", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 980000000m, "hoan_thanh" },
                    { 47, "GhiTang", "CT-GT-010", "Ghi tăng tài sản OTO-0010 – Ford Ranger Wildtrak 2.0L Bi-Turbo AT 2023 (Pickup)", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 920000000m, "hoan_thanh" },
                    { 48, "GhiTang", "CT-GT-011", "Ghi tăng tài sản LAP-0011 – Dell Latitude 7430 (i7/16GB/512GB, vPro)", new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 32000000m, "hoan_thanh" },
                    { 49, "GhiTang", "CT-GT-012", "Ghi tăng tài sản LAP-0012 – HP EliteBook 840 G9 (i5/16GB/512GB)", new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 25000000m, "hoan_thanh" },
                    { 50, "GhiTang", "CT-GT-013", "Ghi tăng tài sản LAP-0013 – Lenovo ThinkPad L14 Gen 3 (i5/16GB/512GB)", new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 22000000m, "hoan_thanh" },
                    { 51, "GhiTang", "CT-GT-014", "Ghi tăng tài sản LAP-0014 – Dell Vostro 5620 (i5/8GB/256GB)", new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 18000000m, "hoan_thanh" },
                    { 52, "GhiTang", "CT-GT-015", "Ghi tăng tài sản LAP-0015 – HP ProBook 445 G9 (Ryzen 5/8GB/256GB)", new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 16000000m, "hoan_thanh" },
                    { 53, "GhiTang", "CT-GT-016", "Ghi tăng tài sản LAP-0016 – Lenovo IdeaPad 3 Gen 7 (i5/8GB/512GB)", new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 16000000m, "hoan_thanh" },
                    { 54, "GhiTang", "CT-GT-017", "Ghi tăng tài sản LAP-0017 – Asus VivoBook 15 (i3/8GB/512GB)", new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 13000000m, "hoan_thanh" },
                    { 55, "GhiTang", "CT-GT-018", "Ghi tăng tài sản LAP-0018 – Dell Precision 5570 (i7/32GB/1TB, Workstation)", new DateTime(2022, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 56, "GhiTang", "CT-GT-019", "Ghi tăng tài sản LAP-0019 – Lenovo ThinkPad P15 Gen 2 (i9/32GB/1TB)", new DateTime(2022, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 60000000m, "hoan_thanh" },
                    { 57, "GhiTang", "CT-GT-020", "Ghi tăng tài sản LAP-0020 – HP ZBook Fury 15 G8 (i7/16GB/512GB)", new DateTime(2022, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 48000000m, "hoan_thanh" },
                    { 58, "GhiTang", "CT-GT-021", "Ghi tăng tài sản SRV-0021 – Dell PowerEdge R750xs (2×Xeon Silver, 128GB RAM, 4×3.5\" HDD 4TB)", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 280000000m, "hoan_thanh" },
                    { 59, "GhiTang", "CT-GT-022", "Ghi tăng tài sản SRV-0022 – Dell PowerEdge R750xs (2×Xeon Silver, 128GB RAM, 4×3.5\" HDD 4TB)", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 280000000m, "hoan_thanh" },
                    { 60, "GhiTang", "CT-GT-023", "Ghi tăng tài sản SRV-0023 – HPE ProLiant DL360 Gen10 Plus (Xeon Gold, 64GB, 8×SFF)", new DateTime(2021, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 240000000m, "hoan_thanh" },
                    { 61, "GhiTang", "CT-GT-024", "Ghi tăng tài sản SRV-0024 – HPE ProLiant DL380 Gen10 (2×Xeon Silver, 128GB, 12×LFF)", new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 320000000m, "hoan_thanh" },
                    { 62, "GhiTang", "CT-GT-025", "Ghi tăng tài sản SRV-0025 – Synology RackStation RS1221+ (8-Bay NAS, Ryzen)", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 48000000m, "hoan_thanh" },
                    { 63, "GhiTang", "CT-GT-026", "Ghi tăng tài sản SRV-0026 – Synology RackStation RS3621RPxs (12-Bay NAS, Xeon D)", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 120000000m, "hoan_thanh" },
                    { 64, "GhiTang", "CT-GT-027", "Ghi tăng tài sản SRV-0027 – Dell PowerEdge T350 (Xeon E-2300, 16GB, Tower Server)", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 95000000m, "hoan_thanh" },
                    { 65, "GhiTang", "CT-GT-028", "Ghi tăng tài sản SRV-0028 – HPE ProLiant MicroServer Gen10 Plus v2 (i3, 16GB)", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" },
                    { 66, "GhiTang", "CT-GT-029", "Ghi tăng tài sản SRV-0029 – Synology DiskStation DS1621+ (6-Bay, Ryzen V1500)", new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 36000000m, "hoan_thanh" },
                    { 67, "GhiTang", "CT-GT-030", "Ghi tăng tài sản SRV-0030 – APC Smart-UPS SRT 3000VA RM (UPS nguồn dự phòng)", new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 22000000m, "hoan_thanh" },
                    { 68, "GhiTang", "CT-GT-031", "Ghi tăng tài sản NET-0031 – Cisco Catalyst 9200L-24P-4G (Switch 24-port PoE)", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 69, "GhiTang", "CT-GT-032", "Ghi tăng tài sản NET-0032 – Cisco Catalyst 9200L-48P-4G (Switch 48-port PoE)", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 65000000m, "hoan_thanh" },
                    { 70, "GhiTang", "CT-GT-033", "Ghi tăng tài sản NET-0033 – Fortinet FortiGate 100F (NGFW, Firewall)", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 90000000m, "hoan_thanh" },
                    { 71, "GhiTang", "CT-GT-034", "Ghi tăng tài sản NET-0034 – Fortinet FortiAP 431F (WiFi 6, Access Point)", new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 15000000m, "hoan_thanh" },
                    { 72, "GhiTang", "CT-GT-035", "Ghi tăng tài sản NET-0035 – Fortinet FortiAP 431F (WiFi 6, Access Point)", new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 15000000m, "hoan_thanh" },
                    { 73, "GhiTang", "CT-GT-036", "Ghi tăng tài sản NET-0036 – Cisco ISR 4321/K9 (Router VPN/WAN)", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 74, "GhiTang", "CT-GT-037", "Ghi tăng tài sản NET-0037 – Ubiquiti UniFi AP AC Pro (WiFi AC, PoE)", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 3500000m, "hoan_thanh" },
                    { 75, "GhiTang", "CT-GT-038", "Ghi tăng tài sản NET-0038 – Ubiquiti UniFi AP AC Pro (WiFi AC, PoE)", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 3500000m, "hoan_thanh" },
                    { 76, "GhiTang", "CT-GT-039", "Ghi tăng tài sản NET-0039 – Cisco SG350-28P (Switch 28-port PoE+)", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 20000000m, "hoan_thanh" },
                    { 77, "GhiTang", "CT-GT-040", "Ghi tăng tài sản NET-0040 – Palo Alto PA-440 (Next-Gen Firewall)", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 120000000m, "hoan_thanh" },
                    { 78, "GhiTang", "CT-GT-041", "Ghi tăng tài sản LAP-0041 – Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 79, "GhiTang", "CT-GT-042", "Ghi tăng tài sản LAP-0042 – Apple MacBook Air 13\" M2 (8GB/256GB)", new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 29000000m, "hoan_thanh" },
                    { 80, "GhiTang", "CT-GT-043", "Ghi tăng tài sản LAP-0043 – Dell XPS 13 9310 (i7/16GB/512GB)", new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 81, "GhiTang", "CT-GT-044", "Ghi tăng tài sản LAP-0044 – Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 82, "GhiTang", "CT-GT-045", "Ghi tăng tài sản LAP-0045 – Apple MacBook Air 15\" M2 (8GB/512GB)", new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 83, "GhiTang", "CT-GT-046", "Ghi tăng tài sản LAP-0046 – Apple MacBook Pro 16\" M2 Pro (16GB/512GB)", new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 84, "GhiTang", "CT-GT-047", "Ghi tăng tài sản LAP-0047 – Apple MacBook Pro 14\" M2 Pro (12GB/512GB)", new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 28000000m, "hoan_thanh" },
                    { 85, "GhiTang", "CT-GT-048", "Ghi tăng tài sản LAP-0048 – Apple MacBook Pro 14\" M2 (16GB/512GB)", new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 86, "GhiTang", "CT-GT-049", "Ghi tăng tài sản LAP-0049 – Apple MacBook Air 15\" M2 (16GB/512GB)", new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 28000000m, "hoan_thanh" },
                    { 87, "GhiTang", "CT-GT-050", "Ghi tăng tài sản LAP-0050 – Dell XPS 15 9520 (i7/32GB/1TB)", new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 88, "GhiTang", "CT-GT-051", "Ghi tăng tài sản LAP-0051 – Dell Latitude 7430 (i7/16GB/512GB)", new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 28000000m, "hoan_thanh" },
                    { 89, "GhiTang", "CT-GT-052", "Ghi tăng tài sản LAP-0052 – Dell XPS 15 9520 (i7/32GB/1TB)", new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 90, "GhiTang", "CT-GT-053", "Ghi tăng tài sản LAP-0053 – Lenovo ThinkPad L15 Gen 3 (i5/16GB/512GB)", new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 28000000m, "hoan_thanh" },
                    { 91, "GhiTang", "CT-GT-054", "Ghi tăng tài sản LAP-0054 – Lenovo ThinkPad X1 Carbon Gen 10 (i7/16GB/512GB)", new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 92, "GhiTang", "CT-GT-055", "Ghi tăng tài sản LAP-0055 – HP EliteBook 845 G9 (Ryzen 5/16GB/512GB)", new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 28000000m, "hoan_thanh" },
                    { 93, "GhiTang", "CT-GT-056", "Ghi tăng tài sản LAP-0056 – HP Spectre x360 14 (i7/16GB/512GB)", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 94, "GhiTang", "CT-GT-057", "Ghi tăng tài sản LAP-0057 – Asus VivoBook Pro 15 OLED (i7/16GB/512GB)", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 22000000m, "hoan_thanh" },
                    { 95, "GhiTang", "CT-GT-058", "Ghi tăng tài sản LAP-0058 – Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 96, "GhiTang", "CT-GT-059", "Ghi tăng tài sản LAP-0059 – Lenovo IdeaPad Gaming 3 (i7/16GB/512GB)", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 22000000m, "hoan_thanh" },
                    { 97, "GhiTang", "CT-GT-060", "Ghi tăng tài sản LAP-0060 – Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 98, "GhiTang", "CT-GT-061", "Ghi tăng tài sản LAP-0061 – Dell Vostro 5620 (i7/16GB/512GB)", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 22000000m, "hoan_thanh" },
                    { 99, "GhiTang", "CT-GT-062", "Ghi tăng tài sản LAP-0062 – Dell Precision 5570 (i7/32GB/1TB)", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 100, "GhiTang", "CT-GT-063", "Ghi tăng tài sản LAP-0063 – HP EliteBook 830 G9 (i5/8GB/256GB)", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 22000000m, "hoan_thanh" },
                    { 101, "GhiTang", "CT-GT-064", "Ghi tăng tài sản LAP-0064 – HP ZBook Studio G9 (i7/32GB/512GB)", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 102, "GhiTang", "CT-GT-065", "Ghi tăng tài sản LAP-0065 – Asus ExpertBook B9 (i7/16GB/1TB)", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 22000000m, "hoan_thanh" },
                    { 103, "GhiTang", "CT-GT-066", "Ghi tăng tài sản LAP-0066 – Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 104, "GhiTang", "CT-GT-067", "Ghi tăng tài sản LAP-0067 – Dell Latitude 5540 (i7/16GB/512GB)", new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 28000000m, "hoan_thanh" },
                    { 105, "GhiTang", "CT-GT-068", "Ghi tăng tài sản LAP-0068 – Lenovo ThinkPad E14 Gen 5 (i5/16GB/512GB)", new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 22000000m, "hoan_thanh" },
                    { 106, "GhiTang", "CT-GT-069", "Ghi tăng tài sản LAP-0069 – Apple MacBook Pro 16\" M3 Max (36GB/1TB) – Lead Designer", new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 95000000m, "hoan_thanh" },
                    { 107, "GhiTang", "CT-GT-070", "Ghi tăng tài sản LAP-0070 – Dell Precision 5570 (i9/64GB/2TB, Workstation)", new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 75000000m, "hoan_thanh" },
                    { 108, "GhiTang", "CT-GT-071", "Ghi tăng tài sản LAP-0071 – Apple MacBook Pro 14\" M3 Pro (18GB/512GB)", new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 65000000m, "hoan_thanh" },
                    { 109, "GhiTang", "CT-GT-072", "Ghi tăng tài sản LAP-0072 – Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 110, "GhiTang", "CT-GT-073", "Ghi tăng tài sản LAP-0073 – Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 111, "GhiTang", "CT-GT-074", "Ghi tăng tài sản LAP-0074 – HP Spectre x360 14 OLED (i7/16GB/1TB)", new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" }
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
                    { 4, "TECH", "Phòng kỹ thuật" },
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
                    { 12, "Nguồn vốn", "2141", "214", "Hao mòn TSCĐ hữu hình" }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "danh_muc_tai_san",
                columns: new[] { "Id", "MaDanhMuc", "MaTaiKhoan", "TenDanhMuc", "ThoiGianKhauHao", "TienTo" },
                values: new object[,]
                {
                    { 1, "LAP", "2114", "Máy tính xách tay (Laptop)", 36, "LAP" },
                    { 2, "SRV", "2112", "Máy chủ & Thiết bị lưu trữ (NAS)", 60, "SRV" },
                    { 3, "OTO", "2113", "Phương tiện vận tải (Ô tô)", 120, "OTO" },
                    { 4, "NET", "2112", "Thiết bị mạng & Bảo mật", 36, "NET" }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "tai_san",
                columns: new[] { "Id", "DanhMucId", "GiaTriConLai", "KhauHaoHangThang", "KhauHaoLuyKe", "LoId", "MaTaiKhoan", "MaTaiSan", "MoTa", "NgayCapNhat", "NgayCapPhat", "NgayMua", "NgayTao", "NguoiDungId", "NguyenGia", "NhaSanXuat", "PhongBanId", "PhuongPhapKhauHao", "PhuongThucThanhToan", "SoSeri", "SoThuTuTrongLo", "TenTaiSan", "ThoiGianKhauHao", "ThongSoKyThuat", "TongSoTrongLo", "TrangThai" },
                values: new object[,]
                {
                    { 1, 1, 33055555.58m, 2361111.11m, 51944444.42m, null, "2114", "LAP-0001", null, null, new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 85000000m, "Apple", 1, "DuongThang", "ChuyenKhoan", "FVFX3YLHQ05M", null, "Apple MacBook Pro 16\" M3 Pro (48GB/1TB)", 36, null, null, "DangSuDung" },
                    { 2, 1, 12444444.42m, 888888.89m, 19555555.58m, null, "2114", "LAP-0002", null, null, new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 32000000m, "Asus", 1, "DuongThang", "ChuyenKhoan", "DMPX8GH2TYKV", null, "Apple iPad Pro 12.9\" M2 (WiFi+5G)", 36, null, null, "DangSuDung" },
                    { 3, 1, 21388888.84m, 1527777.78m, 33611111.16m, null, "2114", "LAP-0003", null, null, new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 55000000m, "Apple", 1, "DuongThang", "ChuyenKhoan", "C02ZK1YHLVDQ", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 4, 1, 10888888.84m, 777777.78m, 17111111.16m, null, "2114", "LAP-0004", null, null, new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 28000000m, "Dell", 1, "DuongThang", "ChuyenKhoan", "DLLAT5430ABCD", null, "Dell Latitude 5430 (i7/16GB/512GB SSD)", 36, null, null, "DangSuDung" },
                    { 5, 1, 21111111.04m, 1055555.56m, 16888888.96m, null, "2114", "LAP-0005", null, null, new DateTime(2023, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 38000000m, "Apple", 1, "DuongThang", "ChuyenKhoan", "C02ZQ5RTLVDP", null, "Apple MacBook Air 15\" M2 (8GB/512GB)", 36, null, null, "DangSuDung" },
                    { 6, 1, 13333333.28m, 666666.67m, 10666666.72m, null, "2114", "LAP-0006", null, null, new DateTime(2023, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 24000000m, "HP", 1, "DuongThang", "ChuyenKhoan", "HPEB840G9WXYZ", null, "HP EliteBook 840 G9 (i5/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 7, 1, 7000000m, 500000m, 11000000m, null, "2114", "LAP-0007", null, null, new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, 18000000m, "Lenovo", 1, "DuongThang", "ChuyenKhoan", "LTPE14G4001X", null, "Lenovo ThinkPad E14 Gen 4 (i5/8GB/256GB)", 36, null, null, "DangSuDung" },
                    { 8, 1, 6222222.32m, 444444.44m, 9777777.68m, null, "2114", "LAP-0008", null, null, new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, 16000000m, "Asus", 1, "DuongThang", "ChuyenKhoan", "ASVIVO15001Y", null, "Asus VivoBook 15 OLED (i5/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 9, 3, 784000000.00m, 8166666.67m, 196000000.00m, null, "2113", "OTO-0009", null, null, new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 980000000m, "Toyota", 1, "DuongThang", "ChuyenKhoan", "MNBVCXZLKJHG1", null, "Toyota Innova Crysta 2.0G MT 2023 (7 chỗ)", 120, null, null, "DangSuDung" },
                    { 10, 3, 736000000.00m, 7666666.67m, 184000000.00m, null, "2113", "OTO-0010", null, null, new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 920000000m, "Ford", 1, "DuongThang", "ChuyenKhoan", "MNBVCXZLKJHG2", null, "Ford Ranger Wildtrak 2.0L Bi-Turbo AT 2023 (Pickup)", 120, null, null, "DangSuDung" },
                    { 11, 1, 13333333.31m, 888888.89m, 18666666.69m, null, "2114", "LAP-0011", null, null, new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, 32000000m, "Dell", 2, "DuongThang", "ChuyenKhoan", "DLLAT7430KT01", null, "Dell Latitude 7430 (i7/16GB/512GB, vPro)", 36, null, null, "DangSuDung" },
                    { 12, 1, 10416666.76m, 694444.44m, 14583333.24m, null, "2114", "LAP-0012", null, null, new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 25000000m, "HP", 2, "DuongThang", "ChuyenKhoan", "HPEB840KT02", null, "HP EliteBook 840 G9 (i5/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 13, 1, 9166666.69m, 611111.11m, 12833333.31m, null, "2114", "LAP-0013", null, null, new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, 22000000m, "Lenovo", 2, "DuongThang", "ChuyenKhoan", "LTPL14G3KT03", null, "Lenovo ThinkPad L14 Gen 3 (i5/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 14, 1, 7500000m, 500000m, 10500000m, null, "2114", "LAP-0014", null, null, new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8, 18000000m, "Dell", 2, "DuongThang", "ChuyenKhoan", "DLVO5620KT04", null, "Dell Vostro 5620 (i5/8GB/256GB)", 36, null, null, "DangSuDung" },
                    { 15, 1, 6666666.76m, 444444.44m, 9333333.24m, null, "2114", "LAP-0015", null, null, new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, 16000000m, "HP", 2, "DuongThang", "ChuyenKhoan", "HPPB445G9KT05", null, "HP ProBook 445 G9 (Ryzen 5/8GB/256GB)", 36, null, null, "DangSuDung" },
                    { 16, 1, 7111111.20m, 444444.44m, 8888888.80m, null, "2114", "LAP-0016", null, null, new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10, 16000000m, "Lenovo", 3, "DuongThang", "ChuyenKhoan", "LTIP3G7NS01", null, "Lenovo IdeaPad 3 Gen 7 (i5/8GB/512GB)", 36, null, null, "DangSuDung" },
                    { 17, 1, 5777777.80m, 361111.11m, 7222222.20m, null, "2114", "LAP-0017", null, null, new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, 13000000m, "Asus", 3, "DuongThang", "ChuyenKhoan", "ASVIVO15NS02", null, "Asus VivoBook 15 (i3/8GB/512GB)", 36, null, null, "DangSuDung" },
                    { 18, 1, 7638888.82m, 1527777.78m, 47361111.18m, null, "2114", "LAP-0018", null, null, new DateTime(2022, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12, 55000000m, "Dell", 4, "DuongThang", "ChuyenKhoan", "DLPRE5570TC01", null, "Dell Precision 5570 (i7/32GB/1TB, Workstation)", 36, null, null, "DangSuDung" },
                    { 19, 1, 8333333.23m, 1666666.67m, 51666666.77m, null, "2114", "LAP-0019", null, null, new DateTime(2022, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13, 60000000m, "Lenovo", 4, "DuongThang", "ChuyenKhoan", "LTPP15G2TC02", null, "Lenovo ThinkPad P15 Gen 2 (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 20, 1, 6666666.77m, 1333333.33m, 41333333.23m, null, "2114", "LAP-0020", null, null, new DateTime(2022, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 14, 48000000m, "HP", 4, "DuongThang", "ChuyenKhoan", "HPZBF15G8TC03", null, "HP ZBook Fury 15 G8 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 21, 2, 111999999.88m, 4666666.67m, 168000000.12m, null, "2112", "SRV-0021", null, null, new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12, 280000000m, "Dell", 4, "DuongThang", "ChuyenKhoan", "DLSVR750XS001", null, "Dell PowerEdge R750xs (2×Xeon Silver, 128GB RAM, 4×3.5\" HDD 4TB)", 60, null, null, "DangSuDung" },
                    { 22, 2, 111999999.88m, 4666666.67m, 168000000.12m, null, "2112", "SRV-0022", null, null, new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12, 280000000m, "Dell", 4, "DuongThang", "ChuyenKhoan", "DLSVR750XS002", null, "Dell PowerEdge R750xs (2×Xeon Silver, 128GB RAM, 4×3.5\" HDD 4TB)", 60, null, null, "DangSuDung" },
                    { 23, 2, 96000000m, 4000000m, 144000000m, null, "2112", "SRV-0023", null, null, new DateTime(2021, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13, 240000000m, "HP", 4, "DuongThang", "ChuyenKhoan", "HPESVRDL360001", null, "HPE ProLiant DL360 Gen10 Plus (Xeon Gold, 64GB, 8×SFF)", 60, null, null, "DangSuDung" },
                    { 24, 2, 160000000.10m, 5333333.33m, 159999999.90m, null, "2112", "SRV-0024", null, null, new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13, 320000000m, "HP", 4, "DuongThang", "ChuyenKhoan", "HPESVRDL380001", null, "HPE ProLiant DL380 Gen10 (2×Xeon Silver, 128GB, 12×LFF)", 60, null, null, "DangSuDung" },
                    { 25, 2, 28800000m, 800000m, 19200000m, null, "2112", "SRV-0025", null, null, new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 14, 48000000m, "Synology", 4, "DuongThang", "ChuyenKhoan", "SYNORS1221001", null, "Synology RackStation RS1221+ (8-Bay NAS, Ryzen)", 60, null, null, "DangSuDung" },
                    { 26, 2, 72000000m, 2000000m, 48000000m, null, "2112", "SRV-0026", null, null, new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 14, 120000000m, "Synology", 4, "DuongThang", "ChuyenKhoan", "SYNORS3621001", null, "Synology RackStation RS3621RPxs (12-Bay NAS, Xeon D)", 60, null, null, "DangSuDung" },
                    { 27, 2, 63333333.40m, 1583333.33m, 31666666.60m, null, "2112", "SRV-0027", null, null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12, 95000000m, "Dell", 4, "DuongThang", "ChuyenKhoan", "DLSVRT350001", null, "Dell PowerEdge T350 (Xeon E-2300, 16GB, Tower Server)", 60, null, null, "DangSuDung" },
                    { 28, 2, 28000000m, 700000m, 14000000m, null, "2112", "SRV-0028", null, null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13, 42000000m, "HP", 4, "DuongThang", "ChuyenKhoan", "HPEMSVR001", null, "HPE ProLiant MicroServer Gen10 Plus v2 (i3, 16GB)", 60, null, null, "DangSuDung" },
                    { 29, 2, 26400000m, 600000m, 9600000m, null, "2112", "SRV-0029", null, null, new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 14, 36000000m, "Synology", 4, "DuongThang", "ChuyenKhoan", "SYNODS1621001", null, "Synology DiskStation DS1621+ (6-Bay, Ryzen V1500)", 60, null, null, "DangSuDung" },
                    { 30, 2, 16133333.28m, 366666.67m, 5866666.72m, null, "2112", "SRV-0030", null, null, new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12, 22000000m, "Synology", 4, "DuongThang", "ChuyenKhoan", "APCSRT3000001", null, "APC Smart-UPS SRT 3000VA RM (UPS nguồn dự phòng)", 60, null, null, "DangSuDung" },
                    { 31, 4, 0m, 1250000m, 45000000m, null, "2112", "NET-0031", null, null, new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 45000000m, "Cisco", 4, "DuongThang", "ChuyenKhoan", "CSCAT9200001", null, "Cisco Catalyst 9200L-24P-4G (Switch 24-port PoE)", 36, null, null, "DangSuDung" },
                    { 32, 4, -0.16m, 1805555.56m, 65000000.16m, null, "2112", "NET-0032", null, null, new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 65000000m, "Cisco", 4, "DuongThang", "ChuyenKhoan", "CSCAT9200002", null, "Cisco Catalyst 9200L-48P-4G (Switch 48-port PoE)", 36, null, null, "DangSuDung" },
                    { 33, 4, 0m, 2500000m, 90000000m, null, "2112", "NET-0033", null, null, new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 90000000m, "Fortinet", 4, "DuongThang", "ChuyenKhoan", "FTFG100F001", null, "Fortinet FortiGate 100F (NGFW, Firewall)", 36, null, null, "DangSuDung" },
                    { 34, 4, 2499999.90m, 416666.67m, 12500000.10m, null, "2112", "NET-0034", null, null, new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 15000000m, "Fortinet", 4, "DuongThang", "ChuyenKhoan", "FTFAP431F001", null, "Fortinet FortiAP 431F (WiFi 6, Access Point)", 36, null, null, "DangSuDung" },
                    { 35, 4, 2499999.90m, 416666.67m, 12500000.10m, null, "2112", "NET-0035", null, null, new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 15000000m, "Fortinet", 4, "DuongThang", "ChuyenKhoan", "FTFAP431F002", null, "Fortinet FortiAP 431F (WiFi 6, Access Point)", 36, null, null, "DangSuDung" },
                    { 36, 4, -0.08m, 1527777.78m, 55000000.08m, null, "2112", "NET-0036", null, null, new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 55000000m, "Cisco", 4, "DuongThang", "ChuyenKhoan", "CSISR4321001", null, "Cisco ISR 4321/K9 (Router VPN/WAN)", 36, null, null, "DangSuDung" },
                    { 37, 4, 1166666.72m, 97222.22m, 2333333.28m, null, "2112", "NET-0037", null, null, new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 3500000m, "Ubiquiti", 4, "DuongThang", "ChuyenKhoan", "UBIUAP001", null, "Ubiquiti UniFi AP AC Pro (WiFi AC, PoE)", 36, null, null, "DangSuDung" },
                    { 38, 4, 1166666.72m, 97222.22m, 2333333.28m, null, "2112", "NET-0038", null, null, new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 3500000m, "Ubiquiti", 4, "DuongThang", "ChuyenKhoan", "UBIUAP002", null, "Ubiquiti UniFi AP AC Pro (WiFi AC, PoE)", 36, null, null, "DangSuDung" },
                    { 39, 4, 8888888.80m, 555555.56m, 11111111.20m, null, "2112", "NET-0039", null, null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 20000000m, "Cisco", 4, "DuongThang", "ChuyenKhoan", "CSSG350001", null, "Cisco SG350-28P (Switch 28-port PoE+)", 36, null, null, "DangSuDung" },
                    { 40, 4, 53333333.40m, 3333333.33m, 66666666.60m, null, "2112", "NET-0040", null, null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 120000000m, "Ubiquiti", 4, "DuongThang", "ChuyenKhoan", "PAALTO440001", null, "Palo Alto PA-440 (Next-Gen Firewall)", 36, null, null, "DangSuDung" },
                    { 41, 1, 27499999.96m, 1527777.78m, 27500000.04m, null, "2114", "LAP-0041", null, null, new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 15, 55000000m, "Apple", 5, "DuongThang", "ChuyenKhoan", "C02ZK1PROD01", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 42, 1, 14499999.92m, 805555.56m, 14500000.08m, null, "2114", "LAP-0042", null, null, new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 16, 29000000m, "Apple", 5, "DuongThang", "ChuyenKhoan", "C02ZQ5PROD02", null, "Apple MacBook Air 13\" M2 (8GB/256GB)", 36, null, null, "DangSuDung" },
                    { 43, 1, 17500000.04m, 972222.22m, 17499999.96m, null, "2114", "LAP-0043", null, null, new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 17, 35000000m, "Dell", 5, "DuongThang", "ChuyenKhoan", "DLXPS9310001", null, "Dell XPS 13 9310 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 44, 1, 22500000m, 1250000m, 22500000m, null, "2114", "LAP-0044", null, null, new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 18, 45000000m, "Lenovo", 5, "DuongThang", "ChuyenKhoan", "LTPX1CG11001", null, "Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", 36, null, null, "DangSuDung" },
                    { 45, 1, 18999999.92m, 1055555.56m, 19000000.08m, null, "2114", "LAP-0045", null, null, new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 19, 38000000m, "Apple", 5, "DuongThang", "ChuyenKhoan", "C02ZQ5PROD05", null, "Apple MacBook Air 15\" M2 (8GB/512GB)", 36, null, null, "DangSuDung" },
                    { 46, 1, 16250000m, 1250000m, 28750000m, null, "2114", "LAP-0046", null, null, new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 20, 45000000m, "Apple", 6, "DuongThang", "ChuyenKhoan", "DEV20L10046", null, "Apple MacBook Pro 16\" M2 Pro (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 47, 1, 10111111.06m, 777777.78m, 17888888.94m, null, "2114", "LAP-0047", null, null, new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 20, 28000000m, "Apple", 6, "DuongThang", "ChuyenKhoan", "DEV20L20047", null, "Apple MacBook Pro 14\" M2 Pro (12GB/512GB)", 36, null, null, "DangSuDung" },
                    { 48, 1, 16250000m, 1250000m, 28750000m, null, "2114", "LAP-0048", null, null, new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 21, 45000000m, "Apple", 6, "DuongThang", "ChuyenKhoan", "DEV21L10048", null, "Apple MacBook Pro 14\" M2 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 49, 1, 10111111.06m, 777777.78m, 17888888.94m, null, "2114", "LAP-0049", null, null, new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 21, 28000000m, "Apple", 6, "DuongThang", "ChuyenKhoan", "DEV21L20049", null, "Apple MacBook Air 15\" M2 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 50, 1, 16250000m, 1250000m, 28750000m, null, "2114", "LAP-0050", null, null, new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 22, 45000000m, "Dell", 6, "DuongThang", "ChuyenKhoan", "DEV22L10050", null, "Dell XPS 15 9520 (i7/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 51, 1, 10111111.06m, 777777.78m, 17888888.94m, null, "2114", "LAP-0051", null, null, new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 22, 28000000m, "Dell", 6, "DuongThang", "ChuyenKhoan", "DEV22L20051", null, "Dell Latitude 7430 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 52, 1, 16250000m, 1250000m, 28750000m, null, "2114", "LAP-0052", null, null, new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 23, 45000000m, "Dell", 6, "DuongThang", "ChuyenKhoan", "DEV23L10052", null, "Dell XPS 15 9520 (i7/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 53, 1, 10111111.06m, 777777.78m, 17888888.94m, null, "2114", "LAP-0053", null, null, new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 23, 28000000m, "Lenovo", 6, "DuongThang", "ChuyenKhoan", "DEV23L20053", null, "Lenovo ThinkPad L15 Gen 3 (i5/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 54, 1, 16250000m, 1250000m, 28750000m, null, "2114", "LAP-0054", null, null, new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24, 45000000m, "Lenovo", 6, "DuongThang", "ChuyenKhoan", "DEV24L10054", null, "Lenovo ThinkPad X1 Carbon Gen 10 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 55, 1, 10111111.06m, 777777.78m, 17888888.94m, null, "2114", "LAP-0055", null, null, new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24, 28000000m, "HP", 6, "DuongThang", "ChuyenKhoan", "DEV24L20055", null, "HP EliteBook 845 G9 (Ryzen 5/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 56, 1, 18472222.26m, 972222.22m, 16527777.74m, null, "2114", "LAP-0056", null, null, new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 25, 35000000m, "HP", 6, "DuongThang", "ChuyenKhoan", "DEV25L10056", null, "HP Spectre x360 14 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 57, 1, 11611111.13m, 611111.11m, 10388888.87m, null, "2114", "LAP-0057", null, null, new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 25, 22000000m, "Asus", 6, "DuongThang", "ChuyenKhoan", "DEV25L20057", null, "Asus VivoBook Pro 15 OLED (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 58, 1, 18472222.26m, 972222.22m, 16527777.74m, null, "2114", "LAP-0058", null, null, new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 26, 35000000m, "Lenovo", 6, "DuongThang", "ChuyenKhoan", "DEV26L10058", null, "Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 59, 1, 11611111.13m, 611111.11m, 10388888.87m, null, "2114", "LAP-0059", null, null, new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 26, 22000000m, "Lenovo", 6, "DuongThang", "ChuyenKhoan", "DEV26L20059", null, "Lenovo IdeaPad Gaming 3 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 60, 1, 18472222.26m, 972222.22m, 16527777.74m, null, "2114", "LAP-0060", null, null, new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 27, 35000000m, "Asus", 6, "DuongThang", "ChuyenKhoan", "DEV27L10060", null, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 61, 1, 11611111.13m, 611111.11m, 10388888.87m, null, "2114", "LAP-0061", null, null, new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 27, 22000000m, "Dell", 6, "DuongThang", "ChuyenKhoan", "DEV27L20061", null, "Dell Vostro 5620 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 62, 1, 18472222.26m, 972222.22m, 16527777.74m, null, "2114", "LAP-0062", null, null, new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 28, 35000000m, "Dell", 6, "DuongThang", "ChuyenKhoan", "DEV28L10062", null, "Dell Precision 5570 (i7/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 63, 1, 11611111.13m, 611111.11m, 10388888.87m, null, "2114", "LAP-0063", null, null, new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 28, 22000000m, "HP", 6, "DuongThang", "ChuyenKhoan", "DEV28L20063", null, "HP EliteBook 830 G9 (i5/8GB/256GB)", 36, null, null, "DangSuDung" },
                    { 64, 1, 18472222.26m, 972222.22m, 16527777.74m, null, "2114", "LAP-0064", null, null, new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 29, 35000000m, "HP", 6, "DuongThang", "ChuyenKhoan", "DEV29L10064", null, "HP ZBook Studio G9 (i7/32GB/512GB)", 36, null, null, "DangSuDung" },
                    { 65, 1, 11611111.13m, 611111.11m, 10388888.87m, null, "2114", "LAP-0065", null, null, new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 29, 22000000m, "Asus", 6, "DuongThang", "ChuyenKhoan", "DEV29L20065", null, "Asus ExpertBook B9 (i7/16GB/1TB)", 36, null, null, "DangSuDung" },
                    { 66, 1, 25972222.18m, 1527777.78m, 29027777.82m, null, "2114", "LAP-0066", null, null, new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 30, 55000000m, "Apple", 7, "DuongThang", "ChuyenKhoan", "C02ZK1PMO01", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 67, 1, 13222222.18m, 777777.78m, 14777777.82m, null, "2114", "LAP-0067", null, null, new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 31, 28000000m, "Dell", 7, "DuongThang", "ChuyenKhoan", "DLLAT5540PMO02", null, "Dell Latitude 5540 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 68, 1, 10388888.91m, 611111.11m, 11611111.09m, null, "2114", "LAP-0068", null, null, new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 32, 22000000m, "Lenovo", 7, "DuongThang", "ChuyenKhoan", "LTPE14G5PMO03", null, "Lenovo ThinkPad E14 Gen 5 (i5/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 69, 1, 39583333.31m, 2638888.89m, 55416666.69m, null, "2114", "LAP-0069", null, null, new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 33, 95000000m, "Apple", 8, "DuongThang", "ChuyenKhoan", "FVFX3DES001", null, "Apple MacBook Pro 16\" M3 Max (36GB/1TB) – Lead Designer", 36, null, null, "DangSuDung" },
                    { 70, 1, 31250000.07m, 2083333.33m, 43749999.93m, null, "2114", "LAP-0070", null, null, new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 33, 75000000m, "Dell", 8, "DuongThang", "ChuyenKhoan", "DLPRE5570DES01", null, "Dell Precision 5570 (i9/64GB/2TB, Workstation)", 36, null, null, "DangSuDung" },
                    { 71, 1, 27083333.24m, 1805555.56m, 37916666.76m, null, "2114", "LAP-0071", null, null, new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 34, 65000000m, "Apple", 8, "DuongThang", "ChuyenKhoan", "FVFX3DES002", null, "Apple MacBook Pro 14\" M3 Pro (18GB/512GB)", 36, null, null, "DangSuDung" },
                    { 72, 1, 22916666.62m, 1527777.78m, 32083333.38m, null, "2114", "LAP-0072", null, null, new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 34, 55000000m, "Asus", 8, "DuongThang", "ChuyenKhoan", "ASPAS16DES01", null, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 73, 1, 22916666.62m, 1527777.78m, 32083333.38m, null, "2114", "LAP-0073", null, null, new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 35, 55000000m, "Apple", 8, "DuongThang", "ChuyenKhoan", "FVFX3DES003", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 74, 1, 17499999.93m, 1166666.67m, 24500000.07m, null, "2114", "LAP-0074", null, null, new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 35, 42000000m, "HP", 8, "DuongThang", "ChuyenKhoan", "HPSPX360DES01", null, "HP Spectre x360 14 OLED (i7/16GB/1TB)", 36, null, null, "DangSuDung" },
                    { 75, 1, 0m, 500000m, 18000000m, null, "2114", "LAP-0075", null, null, null, new DateTime(2019, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 18000000m, null, null, "DuongThang", "TienMat", "OLD0075SN", null, "HP EliteBook 840 G5 (i5/8GB/256GB, cũ)", 36, null, null, "DaThanhLy" },
                    { 76, 1, 0m, 444444.44m, 16000000m, null, "2114", "LAP-0076", null, null, null, new DateTime(2019, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 16000000m, null, null, "DuongThang", "TienMat", "OLD0076SN", null, "Dell Latitude 5480 (i5/8GB/256GB, cũ)", 36, null, null, "DaThanhLy" },
                    { 77, 1, 0m, 472222.22m, 17000000m, null, "2114", "LAP-0077", null, null, null, new DateTime(2019, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 17000000m, null, null, "DuongThang", "TienMat", "OLD0077SN", null, "Lenovo ThinkPad T470 (i5/8GB/256GB, cũ)", 36, null, null, "DaThanhLy" },
                    { 78, 1, 0m, 388888.89m, 14000000m, null, "2114", "LAP-0078", null, null, null, new DateTime(2019, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 14000000m, null, null, "DuongThang", "TienMat", "OLD0078SN", null, "Asus VivoBook 15 (i5/8GB/512GB, cũ)", 36, null, null, "DaThanhLy" },
                    { 79, 1, 0m, 416666.67m, 15000000m, null, "2114", "LAP-0079", null, null, null, new DateTime(2019, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 15000000m, null, null, "DuongThang", "TienMat", "OLD0079SN", null, "HP ProBook 430 G5 (i5/8GB/256GB, cũ)", 36, null, null, "DaThanhLy" },
                    { 80, 1, 0m, 361111.11m, 13000000m, null, "2114", "LAP-0080", null, null, null, new DateTime(2019, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 13000000m, null, null, "DuongThang", "TienMat", "OLD0080SN", null, "Dell Vostro 3580 (i5/8GB/256GB, cũ)", 36, null, null, "DaThanhLy" },
                    { 81, 4, 0m, 972222.22m, 35000000m, null, "2112", "NET-0081", null, null, null, new DateTime(2019, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 35000000m, null, null, "DuongThang", "TienMat", "OLD0081SN", null, "Cisco Catalyst 2960-X-24PS (switch cũ, hỏng)", 36, null, null, "DaThanhLy" },
                    { 82, 4, 0m, 333333.33m, 12000000m, null, "2112", "NET-0082", null, null, null, new DateTime(2019, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 12000000m, null, null, "DuongThang", "TienMat", "OLD0082SN", null, "D-Link DES-1026G (switch 24-port cũ, hỏng)", 36, null, null, "DaThanhLy" },
                    { 83, 2, 2933333.46m, 133333.33m, 5066666.54m, null, "2112", "SRV-0083", null, null, null, new DateTime(2019, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 8000000m, null, null, "DuongThang", "TienMat", "OLD0083SN", null, "Synology DS218j (NAS 2-Bay cũ)", 60, null, null, "DaThanhLy" },
                    { 84, 2, 0m, 138888.89m, 5000000m, null, "2112", "SRV-0084", null, null, null, new DateTime(2019, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 5000000m, null, null, "DuongThang", "TienMat", "OLD0084SN", null, "APC Back-UPS 650VA (UPS cũ, hỏng pin)", 36, null, null, "DaThanhLy" },
                    { 85, 1, 28000000m, 777777.78m, 0m, null, "2114", "LAP-0085", null, null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 28000000m, "Dell", null, "DuongThang", "ChuyenKhoan", "NEW0085SN", null, "Dell Latitude 5540 (i7/16GB/512GB)", 36, null, null, "ChuaCapPhat" },
                    { 86, 1, 28000000m, 777777.78m, 0m, null, "2114", "LAP-0086", null, null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 28000000m, "Dell", null, "DuongThang", "ChuyenKhoan", "NEW0086SN", null, "Dell Latitude 5540 (i7/16GB/512GB)", 36, null, null, "ChuaCapPhat" },
                    { 87, 1, 26000000m, 722222.22m, 0m, null, "2114", "LAP-0087", null, null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 26000000m, "HP", null, "DuongThang", "ChuyenKhoan", "NEW0087SN", null, "HP EliteBook 840 G10 (i5/16GB/512GB)", 36, null, null, "ChuaCapPhat" },
                    { 88, 1, 26000000m, 722222.22m, 0m, null, "2114", "LAP-0088", null, null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 26000000m, "HP", null, "DuongThang", "ChuyenKhoan", "NEW0088SN", null, "HP EliteBook 840 G10 (i5/16GB/512GB)", 36, null, null, "ChuaCapPhat" },
                    { 89, 1, 22000000m, 611111.11m, 0m, null, "2114", "LAP-0089", null, null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 22000000m, "Lenovo", null, "DuongThang", "ChuyenKhoan", "NEW0089SN", null, "Lenovo ThinkPad E16 Gen 1 (i5/16GB/512GB)", 36, null, null, "ChuaCapPhat" },
                    { 90, 1, 22000000m, 611111.11m, 0m, null, "2114", "LAP-0090", null, null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 22000000m, "Lenovo", null, "DuongThang", "ChuyenKhoan", "NEW0090SN", null, "Lenovo ThinkPad E16 Gen 1 (i5/16GB/512GB)", 36, null, null, "ChuaCapPhat" },
                    { 91, 1, 30000000m, 833333.33m, 0m, null, "2114", "LAP-0091", null, null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 30000000m, "Asus", null, "DuongThang", "ChuyenKhoan", "NEW0091SN", null, "Asus ExpertBook B5 Flip (i7/16GB/512GB)", 36, null, null, "ChuaCapPhat" },
                    { 92, 1, 30000000m, 833333.33m, 0m, null, "2114", "LAP-0092", null, null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 30000000m, "Asus", null, "DuongThang", "ChuyenKhoan", "NEW0092SN", null, "Asus ExpertBook B5 Flip (i7/16GB/512GB)", 36, null, null, "ChuaCapPhat" },
                    { 93, 1, 29000000m, 805555.56m, 0m, null, "2114", "LAP-0093", null, null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 29000000m, "Apple", null, "DuongThang", "ChuyenKhoan", "NEW0093SN", null, "Apple MacBook Air 13\" M3 (8GB/256GB)", 36, null, null, "ChuaCapPhat" },
                    { 94, 1, 29000000m, 805555.56m, 0m, null, "2114", "LAP-0094", null, null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 29000000m, "Apple", null, "DuongThang", "ChuyenKhoan", "NEW0094SN", null, "Apple MacBook Air 13\" M3 (8GB/256GB)", 36, null, null, "ChuaCapPhat" },
                    { 95, 1, 38000000m, 1055555.56m, 0m, null, "2114", "LAP-0095", null, null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 38000000m, "Dell", null, "DuongThang", "ChuyenKhoan", "NEW0095SN", null, "Dell XPS 13 Plus 9320 (i7/32GB/1TB)", 36, null, null, "ChuaCapPhat" },
                    { 96, 1, 38000000m, 1055555.56m, 0m, null, "2114", "LAP-0096", null, null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 38000000m, "Dell", null, "DuongThang", "ChuyenKhoan", "NEW0096SN", null, "Dell XPS 13 Plus 9320 (i7/32GB/1TB)", 36, null, null, "ChuaCapPhat" },
                    { 97, 1, 42000000m, 1166666.67m, 0m, null, "2114", "LAP-0097", null, null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 42000000m, "HP", null, "DuongThang", "ChuyenKhoan", "NEW0097SN", null, "HP Spectre x360 14 OLED (i7/16GB/512GB)", 36, null, null, "ChuaCapPhat" },
                    { 98, 1, 42000000m, 1166666.67m, 0m, null, "2114", "LAP-0098", null, null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 42000000m, "HP", null, "DuongThang", "ChuyenKhoan", "NEW0098SN", null, "HP Spectre x360 14 OLED (i7/16GB/512GB)", 36, null, null, "ChuaCapPhat" },
                    { 99, 1, 48000000m, 1333333.33m, 0m, null, "2114", "LAP-0099", null, null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 48000000m, "Lenovo", null, "DuongThang", "ChuyenKhoan", "NEW0099SN", null, "Lenovo ThinkPad X1 Carbon Gen 12 (i7/16GB/1TB)", 36, null, null, "ChuaCapPhat" },
                    { 100, 1, 48000000m, 1333333.33m, 0m, null, "2114", "LAP-0100", null, null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 48000000m, "Lenovo", null, "DuongThang", "ChuyenKhoan", "NEW0100SN", null, "Lenovo ThinkPad X1 Carbon Gen 12 (i7/16GB/1TB)", 36, null, null, "ChuaCapPhat" }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "chi_tiet_chung_tu",
                columns: new[] { "Id", "ChungTuId", "MoTa", "SoTien", "TaiKhoanCo", "TaiKhoanNo", "TaiSanId" },
                values: new object[,]
                {
                    { 1, 2, "Ghi tăng nguyên giá TSCĐ", 18000000m, null, "2114", 75 },
                    { 2, 2, "Thanh toán qua ngân hàng", 18000000m, "112", null, 75 },
                    { 3, 3, "Xóa sổ nguyên giá TSCĐ", 18000000m, "2114", null, 75 },
                    { 4, 3, "Xóa sổ hao mòn lũy kế", 18000000m, null, "2141", 75 },
                    { 5, 4, "Ghi tăng nguyên giá TSCĐ", 16000000m, null, "2114", 76 },
                    { 6, 4, "Thanh toán qua ngân hàng", 16000000m, "112", null, 76 },
                    { 7, 5, "Xóa sổ nguyên giá TSCĐ", 16000000m, "2114", null, 76 },
                    { 8, 5, "Xóa sổ hao mòn lũy kế", 16000000m, null, "2141", 76 },
                    { 9, 6, "Ghi tăng nguyên giá TSCĐ", 17000000m, null, "2114", 77 },
                    { 10, 6, "Thanh toán qua ngân hàng", 17000000m, "112", null, 77 },
                    { 11, 7, "Xóa sổ nguyên giá TSCĐ", 17000000m, "2114", null, 77 },
                    { 12, 7, "Xóa sổ hao mòn lũy kế", 17000000m, null, "2141", 77 },
                    { 13, 8, "Ghi tăng nguyên giá TSCĐ", 14000000m, null, "2114", 78 },
                    { 14, 8, "Thanh toán qua ngân hàng", 14000000m, "112", null, 78 },
                    { 15, 9, "Xóa sổ nguyên giá TSCĐ", 14000000m, "2114", null, 78 },
                    { 16, 9, "Xóa sổ hao mòn lũy kế", 14000000m, null, "2141", 78 },
                    { 17, 10, "Ghi tăng nguyên giá TSCĐ", 15000000m, null, "2114", 79 },
                    { 18, 10, "Thanh toán qua ngân hàng", 15000000m, "112", null, 79 },
                    { 19, 11, "Xóa sổ nguyên giá TSCĐ", 15000000m, "2114", null, 79 },
                    { 20, 11, "Xóa sổ hao mòn lũy kế", 15000000m, null, "2141", 79 },
                    { 21, 12, "Ghi tăng nguyên giá TSCĐ", 13000000m, null, "2114", 80 },
                    { 22, 12, "Thanh toán qua ngân hàng", 13000000m, "112", null, 80 },
                    { 23, 13, "Xóa sổ nguyên giá TSCĐ", 13000000m, "2114", null, 80 },
                    { 24, 13, "Xóa sổ hao mòn lũy kế", 13000000m, null, "2141", 80 },
                    { 25, 14, "Ghi tăng nguyên giá TSCĐ", 35000000m, null, "2112", 81 },
                    { 26, 14, "Thanh toán qua ngân hàng", 35000000m, "112", null, 81 },
                    { 27, 15, "Xóa sổ nguyên giá TSCĐ", 35000000m, "2112", null, 81 },
                    { 28, 15, "Xóa sổ hao mòn lũy kế", 35000000m, null, "2141", 81 },
                    { 29, 16, "Ghi tăng nguyên giá TSCĐ", 12000000m, null, "2112", 82 },
                    { 30, 16, "Thanh toán qua ngân hàng", 12000000m, "112", null, 82 },
                    { 31, 17, "Xóa sổ nguyên giá TSCĐ", 12000000m, "2112", null, 82 },
                    { 32, 17, "Xóa sổ hao mòn lũy kế", 12000000m, null, "2141", 82 },
                    { 33, 18, "Ghi tăng nguyên giá TSCĐ", 8000000m, null, "2112", 83 },
                    { 34, 18, "Thanh toán qua ngân hàng", 8000000m, "112", null, 83 },
                    { 35, 19, "Xóa sổ nguyên giá TSCĐ", 8000000m, "2112", null, 83 },
                    { 36, 19, "Xóa sổ hao mòn lũy kế", 5066666.54m, null, "2141", 83 },
                    { 37, 19, "Thu tiền thanh lý", 293333.346m, null, "111", 83 },
                    { 38, 19, "Lỗ thanh lý tài sản", 2640000.114m, null, "811", 83 },
                    { 39, 20, "Ghi tăng nguyên giá TSCĐ", 5000000m, null, "2112", 84 },
                    { 40, 20, "Thanh toán qua ngân hàng", 5000000m, "112", null, 84 },
                    { 41, 21, "Xóa sổ nguyên giá TSCĐ", 5000000m, "2112", null, 84 },
                    { 42, 21, "Xóa sổ hao mòn lũy kế", 5000000m, null, "2141", 84 },
                    { 43, 22, "Ghi tăng nguyên giá Dell Latitude 5540 (i7/16GB/512GB)", 28000000m, null, "2114", 85 },
                    { 44, 22, "Phải trả nhà cung cấp Laptop", 28000000m, "331", null, 85 },
                    { 45, 23, "Ghi tăng nguyên giá Dell Latitude 5540 (i7/16GB/512GB)", 28000000m, null, "2114", 86 },
                    { 46, 23, "Phải trả nhà cung cấp Laptop", 28000000m, "331", null, 86 },
                    { 47, 24, "Ghi tăng nguyên giá HP EliteBook 840 G10 (i5/16GB/512GB)", 26000000m, null, "2114", 87 },
                    { 48, 24, "Phải trả nhà cung cấp Laptop", 26000000m, "331", null, 87 },
                    { 49, 25, "Ghi tăng nguyên giá HP EliteBook 840 G10 (i5/16GB/512GB)", 26000000m, null, "2114", 88 },
                    { 50, 25, "Phải trả nhà cung cấp Laptop", 26000000m, "331", null, 88 },
                    { 51, 26, "Ghi tăng nguyên giá Lenovo ThinkPad E16 Gen 1 (i5/16GB/512GB)", 22000000m, null, "2114", 89 },
                    { 52, 26, "Phải trả nhà cung cấp Laptop", 22000000m, "331", null, 89 },
                    { 53, 27, "Ghi tăng nguyên giá Lenovo ThinkPad E16 Gen 1 (i5/16GB/512GB)", 22000000m, null, "2114", 90 },
                    { 54, 27, "Phải trả nhà cung cấp Laptop", 22000000m, "331", null, 90 },
                    { 55, 28, "Ghi tăng nguyên giá Asus ExpertBook B5 Flip (i7/16GB/512GB)", 30000000m, null, "2114", 91 },
                    { 56, 28, "Phải trả nhà cung cấp Laptop", 30000000m, "331", null, 91 },
                    { 57, 29, "Ghi tăng nguyên giá Asus ExpertBook B5 Flip (i7/16GB/512GB)", 30000000m, null, "2114", 92 },
                    { 58, 29, "Phải trả nhà cung cấp Laptop", 30000000m, "331", null, 92 },
                    { 59, 30, "Ghi tăng nguyên giá Apple MacBook Air 13\" M3 (8GB/256GB)", 29000000m, null, "2114", 93 },
                    { 60, 30, "Phải trả nhà cung cấp Laptop", 29000000m, "331", null, 93 },
                    { 61, 31, "Ghi tăng nguyên giá Apple MacBook Air 13\" M3 (8GB/256GB)", 29000000m, null, "2114", 94 },
                    { 62, 31, "Phải trả nhà cung cấp Laptop", 29000000m, "331", null, 94 },
                    { 63, 32, "Ghi tăng nguyên giá Dell XPS 13 Plus 9320 (i7/32GB/1TB)", 38000000m, null, "2114", 95 },
                    { 64, 32, "Phải trả nhà cung cấp Laptop", 38000000m, "331", null, 95 },
                    { 65, 33, "Ghi tăng nguyên giá Dell XPS 13 Plus 9320 (i7/32GB/1TB)", 38000000m, null, "2114", 96 },
                    { 66, 33, "Phải trả nhà cung cấp Laptop", 38000000m, "331", null, 96 },
                    { 67, 34, "Ghi tăng nguyên giá HP Spectre x360 14 OLED (i7/16GB/512GB)", 42000000m, null, "2114", 97 },
                    { 68, 34, "Phải trả nhà cung cấp Laptop", 42000000m, "331", null, 97 },
                    { 69, 35, "Ghi tăng nguyên giá HP Spectre x360 14 OLED (i7/16GB/512GB)", 42000000m, null, "2114", 98 },
                    { 70, 35, "Phải trả nhà cung cấp Laptop", 42000000m, "331", null, 98 },
                    { 71, 36, "Ghi tăng nguyên giá Lenovo ThinkPad X1 Carbon Gen 12 (i7/16GB/1TB)", 48000000m, null, "2114", 99 },
                    { 72, 36, "Phải trả nhà cung cấp Laptop", 48000000m, "331", null, 99 },
                    { 73, 37, "Ghi tăng nguyên giá Lenovo ThinkPad X1 Carbon Gen 12 (i7/16GB/1TB)", 48000000m, null, "2114", 100 },
                    { 74, 37, "Phải trả nhà cung cấp Laptop", 48000000m, "331", null, 100 },
                    { 75, 38, "Ghi tăng nguyên giá TSCĐ", 85000000m, null, "2114", 1 },
                    { 76, 38, "Thanh toán chuyển khoản", 85000000m, "112", null, 1 },
                    { 77, 1, "Trích khấu hao LAP-0001 tháng 01/2025", 2361111.11m, "2141", "642", 1 },
                    { 78, 39, "Ghi tăng nguyên giá TSCĐ", 32000000m, null, "2114", 2 },
                    { 79, 39, "Thanh toán chuyển khoản", 32000000m, "112", null, 2 },
                    { 80, 1, "Trích khấu hao LAP-0002 tháng 01/2025", 888888.89m, "2141", "642", 2 },
                    { 81, 40, "Ghi tăng nguyên giá TSCĐ", 55000000m, null, "2114", 3 },
                    { 82, 40, "Thanh toán chuyển khoản", 55000000m, "112", null, 3 },
                    { 83, 1, "Trích khấu hao LAP-0003 tháng 01/2025", 1527777.78m, "2141", "642", 3 },
                    { 84, 41, "Ghi tăng nguyên giá TSCĐ", 28000000m, null, "2114", 4 },
                    { 85, 41, "Thanh toán chuyển khoản", 28000000m, "112", null, 4 },
                    { 86, 1, "Trích khấu hao LAP-0004 tháng 01/2025", 777777.78m, "2141", "642", 4 },
                    { 87, 42, "Ghi tăng nguyên giá TSCĐ", 38000000m, null, "2114", 5 },
                    { 88, 42, "Thanh toán chuyển khoản", 38000000m, "112", null, 5 },
                    { 89, 1, "Trích khấu hao LAP-0005 tháng 01/2025", 1055555.56m, "2141", "642", 5 },
                    { 90, 43, "Ghi tăng nguyên giá TSCĐ", 24000000m, null, "2114", 6 },
                    { 91, 43, "Thanh toán chuyển khoản", 24000000m, "112", null, 6 },
                    { 92, 1, "Trích khấu hao LAP-0006 tháng 01/2025", 666666.67m, "2141", "642", 6 },
                    { 93, 44, "Ghi tăng nguyên giá TSCĐ", 18000000m, null, "2114", 7 },
                    { 94, 44, "Thanh toán chuyển khoản", 18000000m, "112", null, 7 },
                    { 95, 1, "Trích khấu hao LAP-0007 tháng 01/2025", 500000m, "2141", "642", 7 },
                    { 96, 45, "Ghi tăng nguyên giá TSCĐ", 16000000m, null, "2114", 8 },
                    { 97, 45, "Thanh toán chuyển khoản", 16000000m, "112", null, 8 },
                    { 98, 1, "Trích khấu hao LAP-0008 tháng 01/2025", 444444.44m, "2141", "642", 8 },
                    { 99, 46, "Ghi tăng nguyên giá TSCĐ", 980000000m, null, "2113", 9 },
                    { 100, 46, "Thanh toán chuyển khoản", 980000000m, "112", null, 9 },
                    { 101, 1, "Trích khấu hao OTO-0009 tháng 01/2025", 8166666.67m, "2141", "642", 9 },
                    { 102, 47, "Ghi tăng nguyên giá TSCĐ", 920000000m, null, "2113", 10 },
                    { 103, 47, "Thanh toán chuyển khoản", 920000000m, "112", null, 10 },
                    { 104, 1, "Trích khấu hao OTO-0010 tháng 01/2025", 7666666.67m, "2141", "642", 10 },
                    { 105, 48, "Ghi tăng nguyên giá TSCĐ", 32000000m, null, "2114", 11 },
                    { 106, 48, "Thanh toán chuyển khoản", 32000000m, "112", null, 11 },
                    { 107, 1, "Trích khấu hao LAP-0011 tháng 01/2025", 888888.89m, "2141", "642", 11 },
                    { 108, 49, "Ghi tăng nguyên giá TSCĐ", 25000000m, null, "2114", 12 },
                    { 109, 49, "Thanh toán chuyển khoản", 25000000m, "112", null, 12 },
                    { 110, 1, "Trích khấu hao LAP-0012 tháng 01/2025", 694444.44m, "2141", "642", 12 },
                    { 111, 50, "Ghi tăng nguyên giá TSCĐ", 22000000m, null, "2114", 13 },
                    { 112, 50, "Thanh toán chuyển khoản", 22000000m, "112", null, 13 },
                    { 113, 1, "Trích khấu hao LAP-0013 tháng 01/2025", 611111.11m, "2141", "642", 13 },
                    { 114, 51, "Ghi tăng nguyên giá TSCĐ", 18000000m, null, "2114", 14 },
                    { 115, 51, "Thanh toán chuyển khoản", 18000000m, "112", null, 14 },
                    { 116, 1, "Trích khấu hao LAP-0014 tháng 01/2025", 500000m, "2141", "642", 14 },
                    { 117, 52, "Ghi tăng nguyên giá TSCĐ", 16000000m, null, "2114", 15 },
                    { 118, 52, "Thanh toán chuyển khoản", 16000000m, "112", null, 15 },
                    { 119, 1, "Trích khấu hao LAP-0015 tháng 01/2025", 444444.44m, "2141", "642", 15 },
                    { 120, 53, "Ghi tăng nguyên giá TSCĐ", 16000000m, null, "2114", 16 },
                    { 121, 53, "Thanh toán chuyển khoản", 16000000m, "112", null, 16 },
                    { 122, 1, "Trích khấu hao LAP-0016 tháng 01/2025", 444444.44m, "2141", "642", 16 },
                    { 123, 54, "Ghi tăng nguyên giá TSCĐ", 13000000m, null, "2114", 17 },
                    { 124, 54, "Thanh toán chuyển khoản", 13000000m, "112", null, 17 },
                    { 125, 1, "Trích khấu hao LAP-0017 tháng 01/2025", 361111.11m, "2141", "642", 17 },
                    { 126, 55, "Ghi tăng nguyên giá TSCĐ", 55000000m, null, "2114", 18 },
                    { 127, 55, "Thanh toán chuyển khoản", 55000000m, "112", null, 18 },
                    { 128, 1, "Trích khấu hao LAP-0018 tháng 01/2025", 1527777.78m, "2141", "642", 18 },
                    { 129, 56, "Ghi tăng nguyên giá TSCĐ", 60000000m, null, "2114", 19 },
                    { 130, 56, "Thanh toán chuyển khoản", 60000000m, "112", null, 19 },
                    { 131, 1, "Trích khấu hao LAP-0019 tháng 01/2025", 1666666.67m, "2141", "642", 19 },
                    { 132, 57, "Ghi tăng nguyên giá TSCĐ", 48000000m, null, "2114", 20 },
                    { 133, 57, "Thanh toán chuyển khoản", 48000000m, "112", null, 20 },
                    { 134, 1, "Trích khấu hao LAP-0020 tháng 01/2025", 1333333.33m, "2141", "642", 20 },
                    { 135, 58, "Ghi tăng nguyên giá TSCĐ", 280000000m, null, "2112", 21 },
                    { 136, 58, "Thanh toán chuyển khoản", 280000000m, "112", null, 21 },
                    { 137, 1, "Trích khấu hao SRV-0021 tháng 01/2025", 4666666.67m, "2141", "642", 21 },
                    { 138, 59, "Ghi tăng nguyên giá TSCĐ", 280000000m, null, "2112", 22 },
                    { 139, 59, "Thanh toán chuyển khoản", 280000000m, "112", null, 22 },
                    { 140, 1, "Trích khấu hao SRV-0022 tháng 01/2025", 4666666.67m, "2141", "642", 22 },
                    { 141, 60, "Ghi tăng nguyên giá TSCĐ", 240000000m, null, "2112", 23 },
                    { 142, 60, "Thanh toán chuyển khoản", 240000000m, "112", null, 23 },
                    { 143, 1, "Trích khấu hao SRV-0023 tháng 01/2025", 4000000m, "2141", "642", 23 },
                    { 144, 61, "Ghi tăng nguyên giá TSCĐ", 320000000m, null, "2112", 24 },
                    { 145, 61, "Thanh toán chuyển khoản", 320000000m, "112", null, 24 },
                    { 146, 1, "Trích khấu hao SRV-0024 tháng 01/2025", 5333333.33m, "2141", "642", 24 },
                    { 147, 62, "Ghi tăng nguyên giá TSCĐ", 48000000m, null, "2112", 25 },
                    { 148, 62, "Thanh toán chuyển khoản", 48000000m, "112", null, 25 },
                    { 149, 1, "Trích khấu hao SRV-0025 tháng 01/2025", 800000m, "2141", "642", 25 },
                    { 150, 63, "Ghi tăng nguyên giá TSCĐ", 120000000m, null, "2112", 26 },
                    { 151, 63, "Thanh toán chuyển khoản", 120000000m, "112", null, 26 },
                    { 152, 1, "Trích khấu hao SRV-0026 tháng 01/2025", 2000000m, "2141", "642", 26 },
                    { 153, 64, "Ghi tăng nguyên giá TSCĐ", 95000000m, null, "2112", 27 },
                    { 154, 64, "Thanh toán chuyển khoản", 95000000m, "112", null, 27 },
                    { 155, 1, "Trích khấu hao SRV-0027 tháng 01/2025", 1583333.33m, "2141", "642", 27 },
                    { 156, 65, "Ghi tăng nguyên giá TSCĐ", 42000000m, null, "2112", 28 },
                    { 157, 65, "Thanh toán chuyển khoản", 42000000m, "112", null, 28 },
                    { 158, 1, "Trích khấu hao SRV-0028 tháng 01/2025", 700000m, "2141", "642", 28 },
                    { 159, 66, "Ghi tăng nguyên giá TSCĐ", 36000000m, null, "2112", 29 },
                    { 160, 66, "Thanh toán chuyển khoản", 36000000m, "112", null, 29 },
                    { 161, 1, "Trích khấu hao SRV-0029 tháng 01/2025", 600000m, "2141", "642", 29 },
                    { 162, 67, "Ghi tăng nguyên giá TSCĐ", 22000000m, null, "2112", 30 },
                    { 163, 67, "Thanh toán chuyển khoản", 22000000m, "112", null, 30 },
                    { 164, 1, "Trích khấu hao SRV-0030 tháng 01/2025", 366666.67m, "2141", "642", 30 },
                    { 165, 68, "Ghi tăng nguyên giá TSCĐ", 45000000m, null, "2112", 31 },
                    { 166, 68, "Thanh toán chuyển khoản", 45000000m, "112", null, 31 },
                    { 167, 1, "Trích khấu hao NET-0031 tháng 01/2025", 1250000m, "2141", "642", 31 },
                    { 168, 69, "Ghi tăng nguyên giá TSCĐ", 65000000m, null, "2112", 32 },
                    { 169, 69, "Thanh toán chuyển khoản", 65000000m, "112", null, 32 },
                    { 170, 1, "Trích khấu hao NET-0032 tháng 01/2025", 1805555.56m, "2141", "642", 32 },
                    { 171, 70, "Ghi tăng nguyên giá TSCĐ", 90000000m, null, "2112", 33 },
                    { 172, 70, "Thanh toán chuyển khoản", 90000000m, "112", null, 33 },
                    { 173, 1, "Trích khấu hao NET-0033 tháng 01/2025", 2500000m, "2141", "642", 33 },
                    { 174, 71, "Ghi tăng nguyên giá TSCĐ", 15000000m, null, "2112", 34 },
                    { 175, 71, "Thanh toán chuyển khoản", 15000000m, "112", null, 34 },
                    { 176, 1, "Trích khấu hao NET-0034 tháng 01/2025", 416666.67m, "2141", "642", 34 },
                    { 177, 72, "Ghi tăng nguyên giá TSCĐ", 15000000m, null, "2112", 35 },
                    { 178, 72, "Thanh toán chuyển khoản", 15000000m, "112", null, 35 },
                    { 179, 1, "Trích khấu hao NET-0035 tháng 01/2025", 416666.67m, "2141", "642", 35 },
                    { 180, 73, "Ghi tăng nguyên giá TSCĐ", 55000000m, null, "2112", 36 },
                    { 181, 73, "Thanh toán chuyển khoản", 55000000m, "112", null, 36 },
                    { 182, 1, "Trích khấu hao NET-0036 tháng 01/2025", 1527777.78m, "2141", "642", 36 },
                    { 183, 74, "Ghi tăng nguyên giá TSCĐ", 3500000m, null, "2112", 37 },
                    { 184, 74, "Thanh toán chuyển khoản", 3500000m, "112", null, 37 },
                    { 185, 1, "Trích khấu hao NET-0037 tháng 01/2025", 97222.22m, "2141", "642", 37 },
                    { 186, 75, "Ghi tăng nguyên giá TSCĐ", 3500000m, null, "2112", 38 },
                    { 187, 75, "Thanh toán chuyển khoản", 3500000m, "112", null, 38 },
                    { 188, 1, "Trích khấu hao NET-0038 tháng 01/2025", 97222.22m, "2141", "642", 38 },
                    { 189, 76, "Ghi tăng nguyên giá TSCĐ", 20000000m, null, "2112", 39 },
                    { 190, 76, "Thanh toán chuyển khoản", 20000000m, "112", null, 39 },
                    { 191, 1, "Trích khấu hao NET-0039 tháng 01/2025", 555555.56m, "2141", "642", 39 },
                    { 192, 77, "Ghi tăng nguyên giá TSCĐ", 120000000m, null, "2112", 40 },
                    { 193, 77, "Thanh toán chuyển khoản", 120000000m, "112", null, 40 },
                    { 194, 1, "Trích khấu hao NET-0040 tháng 01/2025", 3333333.33m, "2141", "642", 40 },
                    { 195, 78, "Ghi tăng nguyên giá TSCĐ", 55000000m, null, "2114", 41 },
                    { 196, 78, "Thanh toán chuyển khoản", 55000000m, "112", null, 41 },
                    { 197, 1, "Trích khấu hao LAP-0041 tháng 01/2025", 1527777.78m, "2141", "642", 41 },
                    { 198, 79, "Ghi tăng nguyên giá TSCĐ", 29000000m, null, "2114", 42 },
                    { 199, 79, "Thanh toán chuyển khoản", 29000000m, "112", null, 42 },
                    { 200, 1, "Trích khấu hao LAP-0042 tháng 01/2025", 805555.56m, "2141", "642", 42 },
                    { 201, 80, "Ghi tăng nguyên giá TSCĐ", 35000000m, null, "2114", 43 },
                    { 202, 80, "Thanh toán chuyển khoản", 35000000m, "112", null, 43 },
                    { 203, 1, "Trích khấu hao LAP-0043 tháng 01/2025", 972222.22m, "2141", "642", 43 },
                    { 204, 81, "Ghi tăng nguyên giá TSCĐ", 45000000m, null, "2114", 44 },
                    { 205, 81, "Thanh toán chuyển khoản", 45000000m, "112", null, 44 },
                    { 206, 1, "Trích khấu hao LAP-0044 tháng 01/2025", 1250000m, "2141", "642", 44 },
                    { 207, 82, "Ghi tăng nguyên giá TSCĐ", 38000000m, null, "2114", 45 },
                    { 208, 82, "Thanh toán chuyển khoản", 38000000m, "112", null, 45 },
                    { 209, 1, "Trích khấu hao LAP-0045 tháng 01/2025", 1055555.56m, "2141", "642", 45 },
                    { 210, 83, "Ghi tăng nguyên giá TSCĐ", 45000000m, null, "2114", 46 },
                    { 211, 83, "Thanh toán chuyển khoản", 45000000m, "112", null, 46 },
                    { 212, 1, "Trích khấu hao LAP-0046 tháng 01/2025", 1250000m, "2141", "642", 46 },
                    { 213, 84, "Ghi tăng nguyên giá TSCĐ", 28000000m, null, "2114", 47 },
                    { 214, 84, "Thanh toán chuyển khoản", 28000000m, "112", null, 47 },
                    { 215, 1, "Trích khấu hao LAP-0047 tháng 01/2025", 777777.78m, "2141", "642", 47 },
                    { 216, 85, "Ghi tăng nguyên giá TSCĐ", 45000000m, null, "2114", 48 },
                    { 217, 85, "Thanh toán chuyển khoản", 45000000m, "112", null, 48 },
                    { 218, 1, "Trích khấu hao LAP-0048 tháng 01/2025", 1250000m, "2141", "642", 48 },
                    { 219, 86, "Ghi tăng nguyên giá TSCĐ", 28000000m, null, "2114", 49 },
                    { 220, 86, "Thanh toán chuyển khoản", 28000000m, "112", null, 49 },
                    { 221, 1, "Trích khấu hao LAP-0049 tháng 01/2025", 777777.78m, "2141", "642", 49 },
                    { 222, 87, "Ghi tăng nguyên giá TSCĐ", 45000000m, null, "2114", 50 },
                    { 223, 87, "Thanh toán chuyển khoản", 45000000m, "112", null, 50 },
                    { 224, 1, "Trích khấu hao LAP-0050 tháng 01/2025", 1250000m, "2141", "642", 50 },
                    { 225, 88, "Ghi tăng nguyên giá TSCĐ", 28000000m, null, "2114", 51 },
                    { 226, 88, "Thanh toán chuyển khoản", 28000000m, "112", null, 51 },
                    { 227, 1, "Trích khấu hao LAP-0051 tháng 01/2025", 777777.78m, "2141", "642", 51 },
                    { 228, 89, "Ghi tăng nguyên giá TSCĐ", 45000000m, null, "2114", 52 },
                    { 229, 89, "Thanh toán chuyển khoản", 45000000m, "112", null, 52 },
                    { 230, 1, "Trích khấu hao LAP-0052 tháng 01/2025", 1250000m, "2141", "642", 52 },
                    { 231, 90, "Ghi tăng nguyên giá TSCĐ", 28000000m, null, "2114", 53 },
                    { 232, 90, "Thanh toán chuyển khoản", 28000000m, "112", null, 53 },
                    { 233, 1, "Trích khấu hao LAP-0053 tháng 01/2025", 777777.78m, "2141", "642", 53 },
                    { 234, 91, "Ghi tăng nguyên giá TSCĐ", 45000000m, null, "2114", 54 },
                    { 235, 91, "Thanh toán chuyển khoản", 45000000m, "112", null, 54 },
                    { 236, 1, "Trích khấu hao LAP-0054 tháng 01/2025", 1250000m, "2141", "642", 54 },
                    { 237, 92, "Ghi tăng nguyên giá TSCĐ", 28000000m, null, "2114", 55 },
                    { 238, 92, "Thanh toán chuyển khoản", 28000000m, "112", null, 55 },
                    { 239, 1, "Trích khấu hao LAP-0055 tháng 01/2025", 777777.78m, "2141", "642", 55 },
                    { 240, 93, "Ghi tăng nguyên giá TSCĐ", 35000000m, null, "2114", 56 },
                    { 241, 93, "Thanh toán chuyển khoản", 35000000m, "112", null, 56 },
                    { 242, 1, "Trích khấu hao LAP-0056 tháng 01/2025", 972222.22m, "2141", "642", 56 },
                    { 243, 94, "Ghi tăng nguyên giá TSCĐ", 22000000m, null, "2114", 57 },
                    { 244, 94, "Thanh toán chuyển khoản", 22000000m, "112", null, 57 },
                    { 245, 1, "Trích khấu hao LAP-0057 tháng 01/2025", 611111.11m, "2141", "642", 57 },
                    { 246, 95, "Ghi tăng nguyên giá TSCĐ", 35000000m, null, "2114", 58 },
                    { 247, 95, "Thanh toán chuyển khoản", 35000000m, "112", null, 58 },
                    { 248, 1, "Trích khấu hao LAP-0058 tháng 01/2025", 972222.22m, "2141", "642", 58 },
                    { 249, 96, "Ghi tăng nguyên giá TSCĐ", 22000000m, null, "2114", 59 },
                    { 250, 96, "Thanh toán chuyển khoản", 22000000m, "112", null, 59 },
                    { 251, 1, "Trích khấu hao LAP-0059 tháng 01/2025", 611111.11m, "2141", "642", 59 },
                    { 252, 97, "Ghi tăng nguyên giá TSCĐ", 35000000m, null, "2114", 60 },
                    { 253, 97, "Thanh toán chuyển khoản", 35000000m, "112", null, 60 },
                    { 254, 1, "Trích khấu hao LAP-0060 tháng 01/2025", 972222.22m, "2141", "642", 60 },
                    { 255, 98, "Ghi tăng nguyên giá TSCĐ", 22000000m, null, "2114", 61 },
                    { 256, 98, "Thanh toán chuyển khoản", 22000000m, "112", null, 61 },
                    { 257, 1, "Trích khấu hao LAP-0061 tháng 01/2025", 611111.11m, "2141", "642", 61 },
                    { 258, 99, "Ghi tăng nguyên giá TSCĐ", 35000000m, null, "2114", 62 },
                    { 259, 99, "Thanh toán chuyển khoản", 35000000m, "112", null, 62 },
                    { 260, 1, "Trích khấu hao LAP-0062 tháng 01/2025", 972222.22m, "2141", "642", 62 },
                    { 261, 100, "Ghi tăng nguyên giá TSCĐ", 22000000m, null, "2114", 63 },
                    { 262, 100, "Thanh toán chuyển khoản", 22000000m, "112", null, 63 },
                    { 263, 1, "Trích khấu hao LAP-0063 tháng 01/2025", 611111.11m, "2141", "642", 63 },
                    { 264, 101, "Ghi tăng nguyên giá TSCĐ", 35000000m, null, "2114", 64 },
                    { 265, 101, "Thanh toán chuyển khoản", 35000000m, "112", null, 64 },
                    { 266, 1, "Trích khấu hao LAP-0064 tháng 01/2025", 972222.22m, "2141", "642", 64 },
                    { 267, 102, "Ghi tăng nguyên giá TSCĐ", 22000000m, null, "2114", 65 },
                    { 268, 102, "Thanh toán chuyển khoản", 22000000m, "112", null, 65 },
                    { 269, 1, "Trích khấu hao LAP-0065 tháng 01/2025", 611111.11m, "2141", "642", 65 },
                    { 270, 103, "Ghi tăng nguyên giá TSCĐ", 55000000m, null, "2114", 66 },
                    { 271, 103, "Thanh toán chuyển khoản", 55000000m, "112", null, 66 },
                    { 272, 1, "Trích khấu hao LAP-0066 tháng 01/2025", 1527777.78m, "2141", "642", 66 },
                    { 273, 104, "Ghi tăng nguyên giá TSCĐ", 28000000m, null, "2114", 67 },
                    { 274, 104, "Thanh toán chuyển khoản", 28000000m, "112", null, 67 },
                    { 275, 1, "Trích khấu hao LAP-0067 tháng 01/2025", 777777.78m, "2141", "642", 67 },
                    { 276, 105, "Ghi tăng nguyên giá TSCĐ", 22000000m, null, "2114", 68 },
                    { 277, 105, "Thanh toán chuyển khoản", 22000000m, "112", null, 68 },
                    { 278, 1, "Trích khấu hao LAP-0068 tháng 01/2025", 611111.11m, "2141", "642", 68 },
                    { 279, 106, "Ghi tăng nguyên giá TSCĐ", 95000000m, null, "2114", 69 },
                    { 280, 106, "Thanh toán chuyển khoản", 95000000m, "112", null, 69 },
                    { 281, 1, "Trích khấu hao LAP-0069 tháng 01/2025", 2638888.89m, "2141", "642", 69 },
                    { 282, 107, "Ghi tăng nguyên giá TSCĐ", 75000000m, null, "2114", 70 },
                    { 283, 107, "Thanh toán chuyển khoản", 75000000m, "112", null, 70 },
                    { 284, 1, "Trích khấu hao LAP-0070 tháng 01/2025", 2083333.33m, "2141", "642", 70 },
                    { 285, 108, "Ghi tăng nguyên giá TSCĐ", 65000000m, null, "2114", 71 },
                    { 286, 108, "Thanh toán chuyển khoản", 65000000m, "112", null, 71 },
                    { 287, 1, "Trích khấu hao LAP-0071 tháng 01/2025", 1805555.56m, "2141", "642", 71 },
                    { 288, 109, "Ghi tăng nguyên giá TSCĐ", 55000000m, null, "2114", 72 },
                    { 289, 109, "Thanh toán chuyển khoản", 55000000m, "112", null, 72 },
                    { 290, 1, "Trích khấu hao LAP-0072 tháng 01/2025", 1527777.78m, "2141", "642", 72 },
                    { 291, 110, "Ghi tăng nguyên giá TSCĐ", 55000000m, null, "2114", 73 },
                    { 292, 110, "Thanh toán chuyển khoản", 55000000m, "112", null, 73 },
                    { 293, 1, "Trích khấu hao LAP-0073 tháng 01/2025", 1527777.78m, "2141", "642", 73 },
                    { 294, 111, "Ghi tăng nguyên giá TSCĐ", 42000000m, null, "2114", 74 },
                    { 295, 111, "Thanh toán chuyển khoản", 42000000m, "112", null, 74 },
                    { 296, 1, "Trích khấu hao LAP-0074 tháng 01/2025", 1166666.67m, "2141", "642", 74 }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "dieu_chuyen_tai_san",
                columns: new[] { "Id", "DenNguoiDungId", "DenPhongBanId", "GhiChu", "LoaiDieuChuyen", "NgayTao", "NgayThucHien", "TaiSanId", "TrangThai", "TuNguoiDungId", "TuPhongBanId" },
                values: new object[,]
                {
                    { 1, 1, 1, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, "da_hoan_thanh", null, null },
                    { 2, 1, 1, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 2, "da_hoan_thanh", null, null },
                    { 3, 2, 1, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "da_hoan_thanh", null, null },
                    { 4, 2, 1, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 4, "da_hoan_thanh", null, null },
                    { 5, 3, 1, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), 5, "da_hoan_thanh", null, null },
                    { 6, 3, 1, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), 6, "da_hoan_thanh", null, null },
                    { 7, 4, 1, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 7, "da_hoan_thanh", null, null },
                    { 8, 4, 1, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 8, "da_hoan_thanh", null, null },
                    { 9, 1, 1, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 9, "da_hoan_thanh", null, null },
                    { 10, 2, 1, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 10, "da_hoan_thanh", null, null },
                    { 11, 5, 2, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), 11, "da_hoan_thanh", null, null },
                    { 12, 6, 2, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), 12, "da_hoan_thanh", null, null },
                    { 13, 7, 2, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), 13, "da_hoan_thanh", null, null },
                    { 14, 8, 2, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), 14, "da_hoan_thanh", null, null },
                    { 15, 9, 2, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), 15, "da_hoan_thanh", null, null },
                    { 16, 10, 3, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), 16, "da_hoan_thanh", null, null },
                    { 17, 11, 3, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), 17, "da_hoan_thanh", null, null },
                    { 18, 12, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2022, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 18, "da_hoan_thanh", null, null },
                    { 19, 13, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2022, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 19, "da_hoan_thanh", null, null },
                    { 20, 14, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2022, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 20, "da_hoan_thanh", null, null },
                    { 21, 12, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 21, "da_hoan_thanh", null, null },
                    { 22, 12, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 22, "da_hoan_thanh", null, null },
                    { 23, 13, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2021, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 23, "da_hoan_thanh", null, null },
                    { 24, 13, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24, "da_hoan_thanh", null, null },
                    { 25, 14, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 25, "da_hoan_thanh", null, null },
                    { 26, 14, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 26, "da_hoan_thanh", null, null },
                    { 27, 12, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 27, "da_hoan_thanh", null, null },
                    { 28, 13, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 28, "da_hoan_thanh", null, null },
                    { 29, 14, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 29, "da_hoan_thanh", null, null },
                    { 30, 12, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 30, "da_hoan_thanh", null, null },
                    { 31, null, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 31, "da_hoan_thanh", null, null },
                    { 32, null, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 32, "da_hoan_thanh", null, null },
                    { 33, null, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 33, "da_hoan_thanh", null, null },
                    { 34, null, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 34, "da_hoan_thanh", null, null },
                    { 35, null, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 35, "da_hoan_thanh", null, null },
                    { 36, null, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 36, "da_hoan_thanh", null, null },
                    { 37, null, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 37, "da_hoan_thanh", null, null },
                    { 38, null, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 38, "da_hoan_thanh", null, null },
                    { 39, null, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 39, "da_hoan_thanh", null, null },
                    { 40, null, 4, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 40, "da_hoan_thanh", null, null },
                    { 41, 15, 5, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), 41, "da_hoan_thanh", null, null },
                    { 42, 16, 5, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), 42, "da_hoan_thanh", null, null },
                    { 43, 17, 5, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), 43, "da_hoan_thanh", null, null },
                    { 44, 18, 5, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), 44, "da_hoan_thanh", null, null },
                    { 45, 19, 5, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), 45, "da_hoan_thanh", null, null },
                    { 46, 20, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 46, "da_hoan_thanh", null, null },
                    { 47, 20, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 47, "da_hoan_thanh", null, null },
                    { 48, 21, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 48, "da_hoan_thanh", null, null },
                    { 49, 21, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 49, "da_hoan_thanh", null, null },
                    { 50, 22, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 50, "da_hoan_thanh", null, null },
                    { 51, 22, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 51, "da_hoan_thanh", null, null },
                    { 52, 23, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 52, "da_hoan_thanh", null, null },
                    { 53, 23, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 53, "da_hoan_thanh", null, null },
                    { 54, 24, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 54, "da_hoan_thanh", null, null },
                    { 55, 24, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 55, "da_hoan_thanh", null, null },
                    { 56, 25, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), 56, "da_hoan_thanh", null, null },
                    { 57, 25, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), 57, "da_hoan_thanh", null, null },
                    { 58, 26, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), 58, "da_hoan_thanh", null, null },
                    { 59, 26, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), 59, "da_hoan_thanh", null, null },
                    { 60, 27, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), 60, "da_hoan_thanh", null, null },
                    { 61, 27, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), 61, "da_hoan_thanh", null, null },
                    { 62, 28, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), 62, "da_hoan_thanh", null, null },
                    { 63, 28, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), 63, "da_hoan_thanh", null, null },
                    { 64, 29, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), 64, "da_hoan_thanh", null, null },
                    { 65, 29, 6, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), 65, "da_hoan_thanh", null, null },
                    { 66, 30, 7, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 66, "da_hoan_thanh", null, null },
                    { 67, 31, 7, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 67, "da_hoan_thanh", null, null },
                    { 68, 32, 7, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 68, "da_hoan_thanh", null, null },
                    { 69, 33, 8, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), 69, "da_hoan_thanh", null, null },
                    { 70, 33, 8, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), 70, "da_hoan_thanh", null, null },
                    { 71, 34, 8, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), 71, "da_hoan_thanh", null, null },
                    { 72, 34, 8, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), 72, "da_hoan_thanh", null, null },
                    { 73, 35, 8, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), 73, "da_hoan_thanh", null, null },
                    { 74, 35, 8, "Cấp phát tài sản theo quyết định phân công", "CapPhat", new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), 74, "da_hoan_thanh", null, null }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "lich_su_khau_hao",
                columns: new[] { "Id", "ChungTuId", "ConLaiSauKhauHao", "KyKhauHao", "LuyKeSauKhauHao", "NgayTao", "SoTien", "TaiSanId" },
                values: new object[,]
                {
                    { 1, 1, 33055555.58m, "2025-01", 51944444.42m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2361111.11m, 1 },
                    { 2, 1, 12444444.42m, "2025-01", 19555555.58m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 888888.89m, 2 },
                    { 3, 1, 21388888.84m, "2025-01", 33611111.16m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1527777.78m, 3 },
                    { 4, 1, 10888888.84m, "2025-01", 17111111.16m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 777777.78m, 4 },
                    { 5, 1, 21111111.04m, "2025-01", 16888888.96m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1055555.56m, 5 },
                    { 6, 1, 13333333.28m, "2025-01", 10666666.72m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 666666.67m, 6 },
                    { 7, 1, 7000000m, "2025-01", 11000000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 500000m, 7 },
                    { 8, 1, 6222222.32m, "2025-01", 9777777.68m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 444444.44m, 8 },
                    { 9, 1, 784000000.00m, "2025-01", 196000000.00m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8166666.67m, 9 },
                    { 10, 1, 736000000.00m, "2025-01", 184000000.00m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7666666.67m, 10 },
                    { 11, 1, 13333333.31m, "2025-01", 18666666.69m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 888888.89m, 11 },
                    { 12, 1, 10416666.76m, "2025-01", 14583333.24m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 694444.44m, 12 },
                    { 13, 1, 9166666.69m, "2025-01", 12833333.31m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 611111.11m, 13 },
                    { 14, 1, 7500000m, "2025-01", 10500000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 500000m, 14 },
                    { 15, 1, 6666666.76m, "2025-01", 9333333.24m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 444444.44m, 15 },
                    { 16, 1, 7111111.20m, "2025-01", 8888888.80m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 444444.44m, 16 },
                    { 17, 1, 5777777.80m, "2025-01", 7222222.20m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 361111.11m, 17 },
                    { 18, 1, 7638888.82m, "2025-01", 47361111.18m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1527777.78m, 18 },
                    { 19, 1, 8333333.23m, "2025-01", 51666666.77m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1666666.67m, 19 },
                    { 20, 1, 6666666.77m, "2025-01", 41333333.23m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1333333.33m, 20 },
                    { 21, 1, 111999999.88m, "2025-01", 168000000.12m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4666666.67m, 21 },
                    { 22, 1, 111999999.88m, "2025-01", 168000000.12m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4666666.67m, 22 },
                    { 23, 1, 96000000m, "2025-01", 144000000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4000000m, 23 },
                    { 24, 1, 160000000.10m, "2025-01", 159999999.90m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5333333.33m, 24 },
                    { 25, 1, 28800000m, "2025-01", 19200000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 800000m, 25 },
                    { 26, 1, 72000000m, "2025-01", 48000000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2000000m, 26 },
                    { 27, 1, 63333333.40m, "2025-01", 31666666.60m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1583333.33m, 27 },
                    { 28, 1, 28000000m, "2025-01", 14000000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 700000m, 28 },
                    { 29, 1, 26400000m, "2025-01", 9600000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 600000m, 29 },
                    { 30, 1, 16133333.28m, "2025-01", 5866666.72m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 366666.67m, 30 },
                    { 31, 1, 0m, "2025-01", 45000000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1250000m, 31 },
                    { 32, 1, -0.16m, "2025-01", 65000000.16m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1805555.56m, 32 },
                    { 33, 1, 0m, "2025-01", 90000000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2500000m, 33 },
                    { 34, 1, 2499999.90m, "2025-01", 12500000.10m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 416666.67m, 34 },
                    { 35, 1, 2499999.90m, "2025-01", 12500000.10m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 416666.67m, 35 },
                    { 36, 1, -0.08m, "2025-01", 55000000.08m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1527777.78m, 36 },
                    { 37, 1, 1166666.72m, "2025-01", 2333333.28m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 97222.22m, 37 },
                    { 38, 1, 1166666.72m, "2025-01", 2333333.28m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 97222.22m, 38 },
                    { 39, 1, 8888888.80m, "2025-01", 11111111.20m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 555555.56m, 39 },
                    { 40, 1, 53333333.40m, "2025-01", 66666666.60m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3333333.33m, 40 },
                    { 41, 1, 27499999.96m, "2025-01", 27500000.04m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1527777.78m, 41 },
                    { 42, 1, 14499999.92m, "2025-01", 14500000.08m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 805555.56m, 42 },
                    { 43, 1, 17500000.04m, "2025-01", 17499999.96m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 972222.22m, 43 },
                    { 44, 1, 22500000m, "2025-01", 22500000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1250000m, 44 },
                    { 45, 1, 18999999.92m, "2025-01", 19000000.08m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1055555.56m, 45 },
                    { 46, 1, 16250000m, "2025-01", 28750000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1250000m, 46 },
                    { 47, 1, 10111111.06m, "2025-01", 17888888.94m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 777777.78m, 47 },
                    { 48, 1, 16250000m, "2025-01", 28750000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1250000m, 48 },
                    { 49, 1, 10111111.06m, "2025-01", 17888888.94m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 777777.78m, 49 },
                    { 50, 1, 16250000m, "2025-01", 28750000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1250000m, 50 },
                    { 51, 1, 10111111.06m, "2025-01", 17888888.94m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 777777.78m, 51 },
                    { 52, 1, 16250000m, "2025-01", 28750000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1250000m, 52 },
                    { 53, 1, 10111111.06m, "2025-01", 17888888.94m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 777777.78m, 53 },
                    { 54, 1, 16250000m, "2025-01", 28750000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1250000m, 54 },
                    { 55, 1, 10111111.06m, "2025-01", 17888888.94m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 777777.78m, 55 },
                    { 56, 1, 18472222.26m, "2025-01", 16527777.74m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 972222.22m, 56 },
                    { 57, 1, 11611111.13m, "2025-01", 10388888.87m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 611111.11m, 57 },
                    { 58, 1, 18472222.26m, "2025-01", 16527777.74m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 972222.22m, 58 },
                    { 59, 1, 11611111.13m, "2025-01", 10388888.87m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 611111.11m, 59 },
                    { 60, 1, 18472222.26m, "2025-01", 16527777.74m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 972222.22m, 60 },
                    { 61, 1, 11611111.13m, "2025-01", 10388888.87m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 611111.11m, 61 },
                    { 62, 1, 18472222.26m, "2025-01", 16527777.74m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 972222.22m, 62 },
                    { 63, 1, 11611111.13m, "2025-01", 10388888.87m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 611111.11m, 63 },
                    { 64, 1, 18472222.26m, "2025-01", 16527777.74m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 972222.22m, 64 },
                    { 65, 1, 11611111.13m, "2025-01", 10388888.87m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 611111.11m, 65 },
                    { 66, 1, 25972222.18m, "2025-01", 29027777.82m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1527777.78m, 66 },
                    { 67, 1, 13222222.18m, "2025-01", 14777777.82m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 777777.78m, 67 },
                    { 68, 1, 10388888.91m, "2025-01", 11611111.09m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 611111.11m, 68 },
                    { 69, 1, 39583333.31m, "2025-01", 55416666.69m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2638888.89m, 69 },
                    { 70, 1, 31250000.07m, "2025-01", 43749999.93m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2083333.33m, 70 },
                    { 71, 1, 27083333.24m, "2025-01", 37916666.76m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1805555.56m, 71 },
                    { 72, 1, 22916666.62m, "2025-01", 32083333.38m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1527777.78m, 72 },
                    { 73, 1, 22916666.62m, "2025-01", 32083333.38m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1527777.78m, 73 },
                    { 74, 1, 17499999.93m, "2025-01", 24500000.07m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1166666.67m, 74 }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "thanh_ly_tai_san",
                columns: new[] { "Id", "GhiChu", "GiaTriConLai", "GiaTriThanhLy", "KhauHaoLuyKe", "LaiLo", "LyDo", "NgayTao", "NgayThanhLy", "NguyenGia", "TaiSanId", "TrangThai" },
                values: new object[,]
                {
                    { 75, null, 0m, 0m, 18000000m, 0m, "Hư hỏng nặng, hết khấu hao, không còn giá trị sử dụng", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 18000000m, 75, "DaHoanThanh" },
                    { 76, null, 0m, 0m, 16000000m, 0m, "Hư hỏng nặng, hết khấu hao, không còn giá trị sử dụng", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 16000000m, 76, "DaHoanThanh" },
                    { 77, null, 0m, 0m, 17000000m, 0m, "Hư hỏng nặng, hết khấu hao, không còn giá trị sử dụng", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 17000000m, 77, "DaHoanThanh" },
                    { 78, null, 0m, 0m, 14000000m, 0m, "Hư hỏng nặng, hết khấu hao, không còn giá trị sử dụng", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 14000000m, 78, "DaHoanThanh" },
                    { 79, null, 0m, 0m, 15000000m, 0m, "Hư hỏng nặng, hết khấu hao, không còn giá trị sử dụng", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 15000000m, 79, "DaHoanThanh" },
                    { 80, null, 0m, 0m, 13000000m, 0m, "Hư hỏng nặng, hết khấu hao, không còn giá trị sử dụng", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 13000000m, 80, "DaHoanThanh" },
                    { 81, null, 0m, 0m, 35000000m, 0m, "Hư hỏng nặng, hết khấu hao, không còn giá trị sử dụng", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 35000000m, 81, "DaHoanThanh" },
                    { 82, null, 0m, 0m, 12000000m, 0m, "Hư hỏng nặng, hết khấu hao, không còn giá trị sử dụng", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 12000000m, 82, "DaHoanThanh" },
                    { 83, null, 2933333.46m, 293333.346m, 5066666.54m, -2640000.114m, "Hư hỏng nặng, hết khấu hao, không còn giá trị sử dụng", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 8000000m, 83, "DaHoanThanh" },
                    { 84, null, 0m, 0m, 5000000m, 0m, "Hư hỏng nặng, hết khấu hao, không còn giá trị sử dụng", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 5000000m, 84, "DaHoanThanh" }
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
