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
                name: "tai_san_dinh_kem",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaiSanId = table.Column<int>(type: "integer", nullable: false),
                    TenFile = table.Column<string>(type: "text", nullable: false),
                    LoaiFile = table.Column<string>(type: "text", nullable: true),
                    DuongDan = table.Column<string>(type: "text", nullable: true),
                    KichThuoc = table.Column<long>(type: "bigint", nullable: true),
                    NgayTai = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MoTa = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tai_san_dinh_kem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tai_san_dinh_kem_tai_san_TaiSanId",
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
                    { 1, "GhiTang", "CT-GT-0001", "Ghi tăng Dell Latitude 7430 (i7/16GB/512GB, vPro)", new DateTime(2021, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), 6, 32000000m, "hoan_thanh" },
                    { 2, "GhiTang", "CT-GT-0002", "Ghi tăng HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", new DateTime(2021, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 3, "GhiTang", "CT-GT-0003", "Ghi tăng Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", new DateTime(2021, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 4, "GhiTang", "CT-GT-0004", "Ghi tăng Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", new DateTime(2021, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 5, "GhiTang", "CT-GT-0005", "Ghi tăng Apple MacBook Air 15\" M2 (16GB/512GB)", new DateTime(2021, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 6, "GhiTang", "CT-GT-0006", "Ghi tăng Dell Precision 5570 (i7/32GB/1TB, Workstation)", new DateTime(2021, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 7, "GhiTang", "CT-GT-0007", "Ghi tăng Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", new DateTime(2021, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 8, "GhiTang", "CT-GT-0008", "Ghi tăng HP ZBook Firefly 14 G10 (i7/32GB/1TB)", new DateTime(2021, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" },
                    { 9, "GhiTang", "CT-GT-0009", "Ghi tăng Dell XPS 13 9310 (i7/16GB/512GB)", new DateTime(2021, 5, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 29, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 10, "GhiTang", "CT-GT-0010", "Ghi tăng Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2021, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 11, "GhiTang", "CT-GT-0011", "Ghi tăng Dell Latitude 7430 (i7/16GB/512GB, vPro)", new DateTime(2021, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), 6, 32000000m, "hoan_thanh" },
                    { 12, "GhiTang", "CT-GT-0012", "Ghi tăng HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", new DateTime(2021, 7, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 7, 19, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 13, "GhiTang", "CT-GT-0013", "Ghi tăng Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 14, "GhiTang", "CT-GT-0014", "Ghi tăng Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", new DateTime(2021, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 15, "GhiTang", "CT-GT-0015", "Ghi tăng Apple MacBook Air 15\" M2 (16GB/512GB)", new DateTime(2021, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 16, "GhiTang", "CT-GT-0016", "Ghi tăng Dell Precision 5570 (i7/32GB/1TB, Workstation)", new DateTime(2021, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 17, "GhiTang", "CT-GT-0017", "Ghi tăng Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", new DateTime(2021, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 18, "GhiTang", "CT-GT-0018", "Ghi tăng HP ZBook Firefly 14 G10 (i7/32GB/1TB)", new DateTime(2021, 10, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 10, 29, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" },
                    { 19, "GhiTang", "CT-GT-0019", "Ghi tăng Dell XPS 13 9310 (i7/16GB/512GB)", new DateTime(2021, 11, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 11, 15, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 20, "GhiTang", "CT-GT-0020", "Ghi tăng Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2021, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 21, "GhiTang", "CT-GT-0021", "Ghi tăng Dell Latitude 7430 (i7/16GB/512GB, vPro)", new DateTime(2021, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 6, 32000000m, "hoan_thanh" },
                    { 22, "GhiTang", "CT-GT-0022", "Ghi tăng HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", new DateTime(2022, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 23, "GhiTang", "CT-GT-0023", "Ghi tăng Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", new DateTime(2022, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 24, "GhiTang", "CT-GT-0024", "Ghi tăng Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", new DateTime(2022, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 25, "GhiTang", "CT-GT-0025", "Ghi tăng Apple MacBook Air 15\" M2 (16GB/512GB)", new DateTime(2022, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 26, "GhiTang", "CT-GT-0026", "Ghi tăng Dell Precision 5570 (i7/32GB/1TB, Workstation)", new DateTime(2022, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 27, "GhiTang", "CT-GT-0027", "Ghi tăng Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", new DateTime(2022, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 28, "GhiTang", "CT-GT-0028", "Ghi tăng HP ZBook Firefly 14 G10 (i7/32GB/1TB)", new DateTime(2022, 4, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 4, 17, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" },
                    { 29, "GhiTang", "CT-GT-0029", "Ghi tăng Dell XPS 13 9310 (i7/16GB/512GB)", new DateTime(2022, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 30, "GhiTang", "CT-GT-0030", "Ghi tăng Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2022, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 31, "GhiTang", "CT-GT-0031", "Ghi tăng Dell Latitude 7430 (i7/16GB/512GB, vPro)", new DateTime(2022, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), 6, 32000000m, "hoan_thanh" },
                    { 32, "GhiTang", "CT-GT-0032", "Ghi tăng HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", new DateTime(2022, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 33, "GhiTang", "CT-GT-0033", "Ghi tăng Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", new DateTime(2022, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 34, "GhiTang", "CT-GT-0034", "Ghi tăng Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", new DateTime(2022, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 35, "GhiTang", "CT-GT-0035", "Ghi tăng Apple MacBook Air 15\" M2 (16GB/512GB)", new DateTime(2022, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 36, "GhiTang", "CT-GT-0036", "Ghi tăng Dell Precision 5570 (i7/32GB/1TB, Workstation)", new DateTime(2022, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 37, "GhiTang", "CT-GT-0037", "Ghi tăng Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", new DateTime(2022, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 38, "GhiTang", "CT-GT-0038", "Ghi tăng HP ZBook Firefly 14 G10 (i7/32GB/1TB)", new DateTime(2022, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" },
                    { 39, "GhiTang", "CT-GT-0039", "Ghi tăng Dell XPS 13 9310 (i7/16GB/512GB)", new DateTime(2022, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 40, "GhiTang", "CT-GT-0040", "Ghi tăng Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2022, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 41, "GhiTang", "CT-GT-0041", "Ghi tăng Dell Latitude 7430 (i7/16GB/512GB, vPro)", new DateTime(2022, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 6, 32000000m, "hoan_thanh" },
                    { 42, "GhiTang", "CT-GT-0042", "Ghi tăng HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 43, "GhiTang", "CT-GT-0043", "Ghi tăng Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", new DateTime(2022, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 44, "GhiTang", "CT-GT-0044", "Ghi tăng Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", new DateTime(2023, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 45, "GhiTang", "CT-GT-0045", "Ghi tăng Apple MacBook Air 15\" M2 (16GB/512GB)", new DateTime(2023, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 46, "GhiTang", "CT-GT-0046", "Ghi tăng Dell Precision 5570 (i7/32GB/1TB, Workstation)", new DateTime(2023, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 47, "GhiTang", "CT-GT-0047", "Ghi tăng Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", new DateTime(2023, 3, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 6, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 48, "GhiTang", "CT-GT-0048", "Ghi tăng HP ZBook Firefly 14 G10 (i7/32GB/1TB)", new DateTime(2023, 3, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 23, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" },
                    { 49, "GhiTang", "CT-GT-0049", "Ghi tăng Dell XPS 13 9310 (i7/16GB/512GB)", new DateTime(2023, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 50, "GhiTang", "CT-GT-0050", "Ghi tăng Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2023, 4, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 26, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 51, "GhiTang", "CT-GT-0051", "Ghi tăng Dell Latitude 7430 (i7/16GB/512GB, vPro)", new DateTime(2023, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), 6, 32000000m, "hoan_thanh" },
                    { 52, "GhiTang", "CT-GT-0052", "Ghi tăng HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", new DateTime(2023, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 53, "GhiTang", "CT-GT-0053", "Ghi tăng Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", new DateTime(2023, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 54, "GhiTang", "CT-GT-0054", "Ghi tăng Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", new DateTime(2023, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 55, "GhiTang", "CT-GT-0055", "Ghi tăng Apple MacBook Air 15\" M2 (16GB/512GB)", new DateTime(2023, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 56, "GhiTang", "CT-GT-0056", "Ghi tăng Dell Precision 5570 (i7/32GB/1TB, Workstation)", new DateTime(2023, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 57, "GhiTang", "CT-GT-0057", "Ghi tăng Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", new DateTime(2023, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 58, "GhiTang", "CT-GT-0058", "Ghi tăng HP ZBook Firefly 14 G10 (i7/32GB/1TB)", new DateTime(2023, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" },
                    { 59, "GhiTang", "CT-GT-0059", "Ghi tăng Dell XPS 13 9310 (i7/16GB/512GB)", new DateTime(2023, 9, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 26, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 60, "GhiTang", "CT-GT-0060", "Ghi tăng Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2023, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 61, "GhiTang", "CT-GT-0061", "Ghi tăng Dell Latitude 7430 (i7/16GB/512GB, vPro)", new DateTime(2023, 10, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 10, 30, 0, 0, 0, 0, DateTimeKind.Utc), 6, 32000000m, "hoan_thanh" },
                    { 62, "GhiTang", "CT-GT-0062", "Ghi tăng HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", new DateTime(2023, 11, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 11, 16, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 63, "GhiTang", "CT-GT-0063", "Ghi tăng Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 64, "GhiTang", "CT-GT-0064", "Ghi tăng Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 65, "GhiTang", "CT-GT-0065", "Ghi tăng Apple MacBook Air 15\" M2 (16GB/512GB)", new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 66, "GhiTang", "CT-GT-0066", "Ghi tăng Dell Precision 5570 (i7/32GB/1TB, Workstation)", new DateTime(2024, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 67, "GhiTang", "CT-GT-0067", "Ghi tăng Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", new DateTime(2024, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 68, "GhiTang", "CT-GT-0068", "Ghi tăng HP ZBook Firefly 14 G10 (i7/32GB/1TB)", new DateTime(2024, 2, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 26, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" },
                    { 69, "GhiTang", "CT-GT-0069", "Ghi tăng Dell XPS 13 9310 (i7/16GB/512GB)", new DateTime(2024, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 70, "GhiTang", "CT-GT-0070", "Ghi tăng Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 71, "GhiTang", "CT-GT-0071", "Ghi tăng Dell Latitude 7430 (i7/16GB/512GB, vPro)", new DateTime(2024, 4, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 4, 17, 0, 0, 0, 0, DateTimeKind.Utc), 6, 32000000m, "hoan_thanh" },
                    { 72, "GhiTang", "CT-GT-0072", "Ghi tăng HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", new DateTime(2024, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 73, "GhiTang", "CT-GT-0073", "Ghi tăng Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", new DateTime(2024, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 74, "GhiTang", "CT-GT-0074", "Ghi tăng Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", new DateTime(2024, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 75, "GhiTang", "CT-GT-0075", "Ghi tăng Apple MacBook Air 15\" M2 (16GB/512GB)", new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 76, "GhiTang", "CT-GT-0076", "Ghi tăng Dell Precision 5570 (i7/32GB/1TB, Workstation)", new DateTime(2024, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 77, "GhiTang", "CT-GT-0077", "Ghi tăng Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", new DateTime(2024, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 78, "GhiTang", "CT-GT-0078", "Ghi tăng HP ZBook Firefly 14 G10 (i7/32GB/1TB)", new DateTime(2024, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" },
                    { 79, "GhiTang", "CT-GT-0079", "Ghi tăng Dell XPS 13 9310 (i7/16GB/512GB)", new DateTime(2024, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 80, "GhiTang", "CT-GT-0080", "Ghi tăng Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2024, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 81, "GhiTang", "CT-GT-0081", "Ghi tăng Dell Latitude 7430 (i7/16GB/512GB, vPro)", new DateTime(2024, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), 6, 32000000m, "hoan_thanh" },
                    { 82, "GhiTang", "CT-GT-0082", "Ghi tăng HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", new DateTime(2024, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 83, "GhiTang", "CT-GT-0083", "Ghi tăng Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", new DateTime(2024, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 84, "GhiTang", "CT-GT-0084", "Ghi tăng Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", new DateTime(2024, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 85, "GhiTang", "CT-GT-0085", "Ghi tăng Apple MacBook Air 15\" M2 (16GB/512GB)", new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 86, "GhiTang", "CT-GT-0086", "Ghi tăng Dell Precision 5570 (i7/32GB/1TB, Workstation)", new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 87, "GhiTang", "CT-GT-0087", "Ghi tăng Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 88, "GhiTang", "CT-GT-0088", "Ghi tăng HP ZBook Firefly 14 G10 (i7/32GB/1TB)", new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" },
                    { 89, "GhiTang", "CT-GT-0089", "Ghi tăng Dell XPS 13 9310 (i7/16GB/512GB)", new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 90, "GhiTang", "CT-GT-0090", "Ghi tăng Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 91, "GhiTang", "CT-GT-0091", "Ghi tăng Dell Latitude 7430 (i7/16GB/512GB, vPro)", new DateTime(2025, 3, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 3, 23, 0, 0, 0, 0, DateTimeKind.Utc), 6, 32000000m, "hoan_thanh" },
                    { 92, "GhiTang", "CT-GT-0092", "Ghi tăng HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", new DateTime(2025, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 93, "GhiTang", "CT-GT-0093", "Ghi tăng Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", new DateTime(2025, 4, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 26, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 94, "GhiTang", "CT-GT-0094", "Ghi tăng Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", new DateTime(2025, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 95, "GhiTang", "CT-GT-0095", "Ghi tăng Apple MacBook Air 15\" M2 (16GB/512GB)", new DateTime(2025, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 96, "GhiTang", "CT-GT-0096", "Ghi tăng Dell Precision 5570 (i7/32GB/1TB, Workstation)", new DateTime(2025, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 97, "GhiTang", "CT-GT-0097", "Ghi tăng Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", new DateTime(2025, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 98, "GhiTang", "CT-GT-0098", "Ghi tăng HP ZBook Firefly 14 G10 (i7/32GB/1TB)", new DateTime(2025, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" },
                    { 99, "GhiTang", "CT-GT-0099", "Ghi tăng Dell XPS 13 9310 (i7/16GB/512GB)", new DateTime(2025, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 100, "GhiTang", "CT-GT-0100", "Ghi tăng Toyota Innova Crysta 2.0G MT 2023 (7 chỗ)", new DateTime(2025, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), 6, 980000000m, "hoan_thanh" },
                    { 101, "GhiTang", "CT-GT-0101", "Ghi tăng Ford Ranger Wildtrak 2.0L Bi-Turbo AT 2023 (Pickup)", new DateTime(2025, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), 6, 920000000m, "hoan_thanh" },
                    { 102, "GhiTang", "CT-GT-0102", "Ghi tăng Mercedes-Benz GLC 300 4MATIC 2023", new DateTime(2025, 9, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 26, 0, 0, 0, 0, DateTimeKind.Utc), 6, 2299000000m, "hoan_thanh" },
                    { 103, "GhiTang", "CT-GT-0103", "Ghi tăng VinFast VF9 Plus 2023", new DateTime(2025, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), 6, 1500000000m, "hoan_thanh" },
                    { 104, "GhiTang", "CT-GT-0104", "Ghi tăng Dell PowerEdge R750xs (2×Xeon Silver, 128GB RAM, 4×3.5\" HDD 4TB)", new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Utc), 6, 280000000m, "hoan_thanh" },
                    { 105, "GhiTang", "CT-GT-0105", "Ghi tăng Cisco Catalyst 9200L-24P-4G (Switch 24-port PoE)", new DateTime(2025, 11, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 16, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 106, "GhiTang", "CT-GT-0106", "Ghi tăng Fortinet FortiGate 100F (NGFW, Firewall)", new DateTime(2020, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 6, 90000000m, "hoan_thanh" },
                    { 107, "GhiTang", "CT-GT-0107", "Ghi tăng Cisco Meraki MS120-48LP (Cloud Managed Switch)", new DateTime(2021, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 108, "GhiTang", "CT-GT-0108", "Ghi tăng Palo Alto PA-440 (Next-Gen Firewall)", new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 120000000m, "hoan_thanh" }
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
                    { 1, 1, 32000000m, 888888.89m, 0m, null, "2114", "LAP-0001", null, null, new DateTime(2021, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), 1, 32000000m, "Dell", 2, "DuongThang", "ChuyenKhoan", "SN-LAP-000001", null, "Dell Latitude 7430 (i7/16GB/512GB, vPro)", 36, null, null, "DangSuDung" },
                    { 2, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0002", null, null, new DateTime(2021, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc), 2, 35000000m, "HP", 3, "DuongThang", "ChuyenKhoan", "SN-LAP-000002", null, "HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 3, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0003", null, null, new DateTime(2021, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, 35000000m, "Lenovo", 4, "DuongThang", "TienMat", "SN-LAP-000003", null, "Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 4, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0004", null, null, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 4, 55000000m, "Asus", 5, "DuongThang", "ChuyenKhoan", "SN-LAP-000004", null, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 5, 1, 38000000m, 1055555.56m, 0m, null, "2114", "LAP-0005", null, null, new DateTime(2021, 3, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), 5, 38000000m, "Apple", 6, "DuongThang", "ChuyenKhoan", "SN-LAP-000005", null, "Apple MacBook Air 15\" M2 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 6, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0006", null, null, new DateTime(2021, 4, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "Dell", 7, "DuongThang", "TienMat", "SN-LAP-000006", null, "Dell Precision 5570 (i7/32GB/1TB, Workstation)", 36, null, null, "DangSuDung" },
                    { 7, 1, 45000000m, 1250000m, 0m, null, "2114", "LAP-0007", null, null, new DateTime(2021, 4, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), 7, 45000000m, "Lenovo", 8, "DuongThang", "ChuyenKhoan", "SN-LAP-000007", null, "Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", 36, null, null, "DangSuDung" },
                    { 8, 1, 42000000m, 1166666.67m, 0m, null, "2114", "LAP-0008", null, null, new DateTime(2021, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), 8, 42000000m, "HP", 1, "DuongThang", "ChuyenKhoan", "SN-LAP-000008", null, "HP ZBook Firefly 14 G10 (i7/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 9, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0009", null, null, new DateTime(2021, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 29, 0, 0, 0, 0, DateTimeKind.Utc), 9, 35000000m, "Dell", 2, "DuongThang", "TienMat", "SN-LAP-000009", null, "Dell XPS 13 9310 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 10, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0010", null, null, new DateTime(2021, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 10, 55000000m, "Apple", 3, "DuongThang", "ChuyenKhoan", "SN-LAP-000010", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 11, 1, 32000000m, 888888.89m, 0m, null, "2114", "LAP-0011", null, null, new DateTime(2021, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), 11, 32000000m, "Dell", 4, "DuongThang", "ChuyenKhoan", "SN-LAP-000011", null, "Dell Latitude 7430 (i7/16GB/512GB, vPro)", 36, null, null, "DangSuDung" },
                    { 12, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0012", null, null, new DateTime(2021, 7, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 7, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 7, 19, 0, 0, 0, 0, DateTimeKind.Utc), 12, 35000000m, "HP", 5, "DuongThang", "TienMat", "SN-LAP-000012", null, "HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 13, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0013", null, null, new DateTime(2021, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 13, 35000000m, "Lenovo", 6, "DuongThang", "ChuyenKhoan", "SN-LAP-000013", null, "Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 14, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0014", null, null, new DateTime(2021, 8, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), 14, 55000000m, "Asus", 7, "DuongThang", "ChuyenKhoan", "SN-LAP-000014", null, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 15, 1, 38000000m, 1055555.56m, 0m, null, "2114", "LAP-0015", null, null, new DateTime(2021, 9, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 15, 38000000m, "Apple", 8, "DuongThang", "TienMat", "SN-LAP-000015", null, "Apple MacBook Air 15\" M2 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 16, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0016", null, null, new DateTime(2021, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), 16, 55000000m, "Dell", 1, "DuongThang", "ChuyenKhoan", "SN-LAP-000016", null, "Dell Precision 5570 (i7/32GB/1TB, Workstation)", 36, null, null, "DangSuDung" },
                    { 17, 1, 45000000m, 1250000m, 0m, null, "2114", "LAP-0017", null, null, new DateTime(2021, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), 17, 45000000m, "Lenovo", 2, "DuongThang", "ChuyenKhoan", "SN-LAP-000017", null, "Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", 36, null, null, "DangSuDung" },
                    { 18, 1, 42000000m, 1166666.67m, 0m, null, "2114", "LAP-0018", null, null, new DateTime(2021, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 10, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 10, 29, 0, 0, 0, 0, DateTimeKind.Utc), 18, 42000000m, "HP", 3, "DuongThang", "TienMat", "SN-LAP-000018", null, "HP ZBook Firefly 14 G10 (i7/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 19, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0019", null, null, new DateTime(2021, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 11, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 11, 15, 0, 0, 0, 0, DateTimeKind.Utc), 19, 35000000m, "Dell", 4, "DuongThang", "ChuyenKhoan", "SN-LAP-000019", null, "Dell XPS 13 9310 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 20, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0020", null, null, new DateTime(2021, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 20, 55000000m, "Apple", 5, "DuongThang", "ChuyenKhoan", "SN-LAP-000020", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 21, 1, 32000000m, 888888.89m, 0m, null, "2114", "LAP-0021", null, null, new DateTime(2021, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 21, 32000000m, "Dell", 6, "DuongThang", "TienMat", "SN-LAP-000021", null, "Dell Latitude 7430 (i7/16GB/512GB, vPro)", 36, null, null, "DangSuDung" },
                    { 22, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0022", null, null, new DateTime(2022, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 22, 35000000m, "HP", 7, "DuongThang", "ChuyenKhoan", "SN-LAP-000022", null, "HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 23, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0023", null, null, new DateTime(2022, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), 23, 35000000m, "Lenovo", 8, "DuongThang", "ChuyenKhoan", "SN-LAP-000023", null, "Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 24, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0024", null, null, new DateTime(2022, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), 24, 55000000m, "Asus", 1, "DuongThang", "TienMat", "SN-LAP-000024", null, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 25, 1, 38000000m, 1055555.56m, 0m, null, "2114", "LAP-0025", null, null, new DateTime(2022, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), 25, 38000000m, "Apple", 2, "DuongThang", "ChuyenKhoan", "SN-LAP-000025", null, "Apple MacBook Air 15\" M2 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 26, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0026", null, null, new DateTime(2022, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), 26, 55000000m, "Dell", 3, "DuongThang", "ChuyenKhoan", "SN-LAP-000026", null, "Dell Precision 5570 (i7/32GB/1TB, Workstation)", 36, null, null, "DangSuDung" },
                    { 27, 1, 45000000m, 1250000m, 0m, null, "2114", "LAP-0027", null, null, new DateTime(2022, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), 27, 45000000m, "Lenovo", 4, "DuongThang", "TienMat", "SN-LAP-000027", null, "Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", 36, null, null, "DangSuDung" },
                    { 28, 1, 42000000m, 1166666.67m, 0m, null, "2114", "LAP-0028", null, null, new DateTime(2022, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 4, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 4, 17, 0, 0, 0, 0, DateTimeKind.Utc), 28, 42000000m, "HP", 5, "DuongThang", "ChuyenKhoan", "SN-LAP-000028", null, "HP ZBook Firefly 14 G10 (i7/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 29, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0029", null, null, new DateTime(2022, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), 29, 35000000m, "Dell", 6, "DuongThang", "ChuyenKhoan", "SN-LAP-000029", null, "Dell XPS 13 9310 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 30, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0030", null, null, new DateTime(2022, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), 30, 55000000m, "Apple", 7, "DuongThang", "TienMat", "SN-LAP-000030", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 31, 1, 32000000m, 888888.89m, 0m, null, "2114", "LAP-0031", null, null, new DateTime(2022, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), 31, 32000000m, "Dell", 8, "DuongThang", "ChuyenKhoan", "SN-LAP-000031", null, "Dell Latitude 7430 (i7/16GB/512GB, vPro)", 36, null, null, "DangSuDung" },
                    { 32, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0032", null, null, new DateTime(2022, 6, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), 32, 35000000m, "HP", 1, "DuongThang", "ChuyenKhoan", "SN-LAP-000032", null, "HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 33, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0033", null, null, new DateTime(2022, 7, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 33, 35000000m, "Lenovo", 2, "DuongThang", "TienMat", "SN-LAP-000033", null, "Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 34, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0034", null, null, new DateTime(2022, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), 34, 55000000m, "Asus", 3, "DuongThang", "ChuyenKhoan", "SN-LAP-000034", null, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 35, 1, 38000000m, 1055555.56m, 0m, null, "2114", "LAP-0035", null, null, new DateTime(2022, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), 35, 38000000m, "Apple", 4, "DuongThang", "ChuyenKhoan", "SN-LAP-000035", null, "Apple MacBook Air 15\" M2 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 36, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0036", null, null, new DateTime(2022, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), 36, 55000000m, "Dell", 5, "DuongThang", "TienMat", "SN-LAP-000036", null, "Dell Precision 5570 (i7/32GB/1TB, Workstation)", 36, null, null, "DangSuDung" },
                    { 37, 1, 45000000m, 1250000m, 0m, null, "2114", "LAP-0037", null, null, new DateTime(2022, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), 37, 45000000m, "Lenovo", 6, "DuongThang", "ChuyenKhoan", "SN-LAP-000037", null, "Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", 36, null, null, "DangSuDung" },
                    { 38, 1, 42000000m, 1166666.67m, 0m, null, "2114", "LAP-0038", null, null, new DateTime(2022, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), 38, 42000000m, "HP", 7, "DuongThang", "ChuyenKhoan", "SN-LAP-000038", null, "HP ZBook Firefly 14 G10 (i7/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 39, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0039", null, null, new DateTime(2022, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), 39, 35000000m, "Dell", 8, "DuongThang", "TienMat", "SN-LAP-000039", null, "Dell XPS 13 9310 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 40, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0040", null, null, new DateTime(2022, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), 40, 55000000m, "Apple", 1, "DuongThang", "ChuyenKhoan", "SN-LAP-000040", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 41, 1, 32000000m, 888888.89m, 0m, null, "2114", "LAP-0041", null, null, new DateTime(2022, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 41, 32000000m, "Dell", 2, "DuongThang", "ChuyenKhoan", "SN-LAP-000041", null, "Dell Latitude 7430 (i7/16GB/512GB, vPro)", 36, null, null, "DangSuDung" },
                    { 42, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0042", null, null, new DateTime(2022, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 42, 35000000m, "HP", 3, "DuongThang", "TienMat", "SN-LAP-000042", null, "HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 43, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0043", null, null, new DateTime(2023, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 43, 35000000m, "Lenovo", 4, "DuongThang", "ChuyenKhoan", "SN-LAP-000043", null, "Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 44, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0044", null, null, new DateTime(2023, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), 44, 55000000m, "Asus", 5, "DuongThang", "ChuyenKhoan", "SN-LAP-000044", null, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 45, 1, 38000000m, 1055555.56m, 0m, null, "2114", "LAP-0045", null, null, new DateTime(2023, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), 45, 38000000m, "Apple", 6, "DuongThang", "TienMat", "SN-LAP-000045", null, "Apple MacBook Air 15\" M2 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 46, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0046", null, null, new DateTime(2023, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 46, 55000000m, "Dell", 7, "DuongThang", "ChuyenKhoan", "SN-LAP-000046", null, "Dell Precision 5570 (i7/32GB/1TB, Workstation)", 36, null, null, "DangSuDung" },
                    { 47, 1, 45000000m, 1250000m, 0m, null, "2114", "LAP-0047", null, null, new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 6, 0, 0, 0, 0, DateTimeKind.Utc), 47, 45000000m, "Lenovo", 8, "DuongThang", "ChuyenKhoan", "SN-LAP-000047", null, "Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", 36, null, null, "DangSuDung" },
                    { 48, 1, 42000000m, 1166666.67m, 0m, null, "2114", "LAP-0048", null, null, new DateTime(2023, 3, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 23, 0, 0, 0, 0, DateTimeKind.Utc), 48, 42000000m, "HP", 1, "DuongThang", "TienMat", "SN-LAP-000048", null, "HP ZBook Firefly 14 G10 (i7/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 49, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0049", null, null, new DateTime(2023, 4, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), 49, 35000000m, "Dell", 2, "DuongThang", "ChuyenKhoan", "SN-LAP-000049", null, "Dell XPS 13 9310 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 50, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0050", null, null, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 26, 0, 0, 0, 0, DateTimeKind.Utc), 50, 55000000m, "Apple", 3, "DuongThang", "ChuyenKhoan", "SN-LAP-000050", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 51, 1, 32000000m, 888888.89m, 0m, null, "2114", "LAP-0051", null, null, new DateTime(2023, 5, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), 51, 32000000m, "Dell", 4, "DuongThang", "TienMat", "SN-LAP-000051", null, "Dell Latitude 7430 (i7/16GB/512GB, vPro)", 36, null, null, "DangSuDung" },
                    { 52, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0052", null, null, new DateTime(2023, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), 52, 35000000m, "HP", 5, "DuongThang", "ChuyenKhoan", "SN-LAP-000052", null, "HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 53, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0053", null, null, new DateTime(2023, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), 53, 35000000m, "Lenovo", 6, "DuongThang", "ChuyenKhoan", "SN-LAP-000053", null, "Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 54, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0054", null, null, new DateTime(2023, 7, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), 54, 55000000m, "Asus", 7, "DuongThang", "TienMat", "SN-LAP-000054", null, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 55, 1, 38000000m, 1055555.56m, 0m, null, "2114", "LAP-0055", null, null, new DateTime(2023, 7, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), 55, 38000000m, "Apple", 8, "DuongThang", "ChuyenKhoan", "SN-LAP-000055", null, "Apple MacBook Air 15\" M2 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 56, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0056", null, null, new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), 56, 55000000m, "Dell", 1, "DuongThang", "ChuyenKhoan", "SN-LAP-000056", null, "Dell Precision 5570 (i7/32GB/1TB, Workstation)", 36, null, null, "DangSuDung" },
                    { 57, 1, 45000000m, 1250000m, 0m, null, "2114", "LAP-0057", null, null, new DateTime(2023, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), 57, 45000000m, "Lenovo", 2, "DuongThang", "TienMat", "SN-LAP-000057", null, "Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", 36, null, null, "DangSuDung" },
                    { 58, 1, 42000000m, 1166666.67m, 0m, null, "2114", "LAP-0058", null, null, new DateTime(2023, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), 58, 42000000m, "HP", 3, "DuongThang", "ChuyenKhoan", "SN-LAP-000058", null, "HP ZBook Firefly 14 G10 (i7/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 59, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0059", null, null, new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 26, 0, 0, 0, 0, DateTimeKind.Utc), 59, 35000000m, "Dell", 4, "DuongThang", "ChuyenKhoan", "SN-LAP-000059", null, "Dell XPS 13 9310 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 60, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0060", null, null, new DateTime(2023, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), 60, 55000000m, "Apple", 5, "DuongThang", "TienMat", "SN-LAP-000060", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 61, 1, 32000000m, 888888.89m, 0m, null, "2114", "LAP-0061", null, null, new DateTime(2023, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 10, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 10, 30, 0, 0, 0, 0, DateTimeKind.Utc), 61, 32000000m, "Dell", 6, "DuongThang", "ChuyenKhoan", "SN-LAP-000061", null, "Dell Latitude 7430 (i7/16GB/512GB, vPro)", 36, null, null, "DangSuDung" },
                    { 62, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0062", null, null, new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 11, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 11, 16, 0, 0, 0, 0, DateTimeKind.Utc), 62, 35000000m, "HP", 7, "DuongThang", "ChuyenKhoan", "SN-LAP-000062", null, "HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 63, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0063", null, null, new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), 63, 35000000m, "Lenovo", 8, "DuongThang", "TienMat", "SN-LAP-000063", null, "Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 64, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0064", null, null, new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 64, 55000000m, "Asus", 1, "DuongThang", "ChuyenKhoan", "SN-LAP-000064", null, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 65, 1, 38000000m, 1055555.56m, 0m, null, "2114", "LAP-0065", null, null, new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 65, 38000000m, "Apple", 2, "DuongThang", "ChuyenKhoan", "SN-LAP-000065", null, "Apple MacBook Air 15\" M2 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 66, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0066", null, null, new DateTime(2024, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), 66, 55000000m, "Dell", 3, "DuongThang", "TienMat", "SN-LAP-000066", null, "Dell Precision 5570 (i7/32GB/1TB, Workstation)", 36, null, null, "DangSuDung" },
                    { 67, 1, 45000000m, 1250000m, 0m, null, "2114", "LAP-0067", null, null, new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), 67, 45000000m, "Lenovo", 4, "DuongThang", "ChuyenKhoan", "SN-LAP-000067", null, "Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", 36, null, null, "DangSuDung" },
                    { 68, 1, 42000000m, 1166666.67m, 0m, null, "2114", "LAP-0068", null, null, new DateTime(2024, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 26, 0, 0, 0, 0, DateTimeKind.Utc), 68, 42000000m, "HP", 5, "DuongThang", "ChuyenKhoan", "SN-LAP-000068", null, "HP ZBook Firefly 14 G10 (i7/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 69, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0069", null, null, new DateTime(2024, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), 69, 35000000m, "Dell", 6, "DuongThang", "TienMat", "SN-LAP-000069", null, "Dell XPS 13 9310 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 70, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0070", null, null, new DateTime(2024, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), 70, 55000000m, "Apple", 7, "DuongThang", "ChuyenKhoan", "SN-LAP-000070", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 71, 1, 32000000m, 888888.89m, 0m, null, "2114", "LAP-0071", null, null, new DateTime(2024, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 4, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 4, 17, 0, 0, 0, 0, DateTimeKind.Utc), 71, 32000000m, "Dell", 8, "DuongThang", "ChuyenKhoan", "SN-LAP-000071", null, "Dell Latitude 7430 (i7/16GB/512GB, vPro)", 36, null, null, "DangSuDung" },
                    { 72, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0072", null, null, new DateTime(2024, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), 72, 35000000m, "HP", 1, "DuongThang", "TienMat", "SN-LAP-000072", null, "HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 73, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0073", null, null, new DateTime(2024, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), 73, 35000000m, "Lenovo", 2, "DuongThang", "ChuyenKhoan", "SN-LAP-000073", null, "Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 74, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0074", null, null, new DateTime(2024, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), 74, 55000000m, "Asus", 3, "DuongThang", "ChuyenKhoan", "SN-LAP-000074", null, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 75, 1, 38000000m, 1055555.56m, 0m, null, "2114", "LAP-0075", null, null, new DateTime(2024, 6, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), 75, 38000000m, "Apple", 4, "DuongThang", "TienMat", "SN-LAP-000075", null, "Apple MacBook Air 15\" M2 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 76, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0076", null, null, new DateTime(2024, 7, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 76, 55000000m, "Dell", 5, "DuongThang", "ChuyenKhoan", "SN-LAP-000076", null, "Dell Precision 5570 (i7/32GB/1TB, Workstation)", 36, null, null, "DangSuDung" },
                    { 77, 1, 45000000m, 1250000m, 0m, null, "2114", "LAP-0077", null, null, new DateTime(2024, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), 77, 45000000m, "Lenovo", 6, "DuongThang", "ChuyenKhoan", "SN-LAP-000077", null, "Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", 36, null, null, "DangSuDung" },
                    { 78, 1, 42000000m, 1166666.67m, 0m, null, "2114", "LAP-0078", null, null, new DateTime(2024, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), 78, 42000000m, "HP", 7, "DuongThang", "TienMat", "SN-LAP-000078", null, "HP ZBook Firefly 14 G10 (i7/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 79, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0079", null, null, new DateTime(2024, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), 79, 35000000m, "Dell", 8, "DuongThang", "ChuyenKhoan", "SN-LAP-000079", null, "Dell XPS 13 9310 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 80, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0080", null, null, new DateTime(2024, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), 80, 55000000m, "Apple", 1, "DuongThang", "ChuyenKhoan", "SN-LAP-000080", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 81, 1, 32000000m, 888888.89m, 0m, null, "2114", "LAP-0081", null, null, new DateTime(2024, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), 81, 32000000m, "Dell", 2, "DuongThang", "TienMat", "SN-LAP-000081", null, "Dell Latitude 7430 (i7/16GB/512GB, vPro)", 36, null, null, "DangSuDung" },
                    { 82, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0082", null, null, new DateTime(2024, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), 82, 35000000m, "HP", 3, "DuongThang", "ChuyenKhoan", "SN-LAP-000082", null, "HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 83, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0083", null, null, new DateTime(2024, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), 83, 35000000m, "Lenovo", 4, "DuongThang", "ChuyenKhoan", "SN-LAP-000083", null, "Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 84, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0084", null, null, new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 84, 55000000m, "Asus", 5, "DuongThang", "TienMat", "SN-LAP-000084", null, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 85, 1, 38000000m, 1055555.56m, 0m, null, "2114", "LAP-0085", null, null, new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 85, 38000000m, "Apple", 6, "DuongThang", "ChuyenKhoan", "SN-LAP-000085", null, "Apple MacBook Air 15\" M2 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 86, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0086", null, null, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 86, 55000000m, "Dell", 7, "DuongThang", "ChuyenKhoan", "SN-LAP-000086", null, "Dell Precision 5570 (i7/32GB/1TB, Workstation)", 36, null, null, "DangSuDung" },
                    { 87, 1, 45000000m, 1250000m, 0m, null, "2114", "LAP-0087", null, null, new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), 87, 45000000m, "Lenovo", 8, "DuongThang", "TienMat", "SN-LAP-000087", null, "Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", 36, null, null, "DangSuDung" },
                    { 88, 1, 42000000m, 1166666.67m, 0m, null, "2114", "LAP-0088", null, null, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), 88, 42000000m, "HP", 1, "DuongThang", "ChuyenKhoan", "SN-LAP-000088", null, "HP ZBook Firefly 14 G10 (i7/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 89, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0089", null, null, new DateTime(2025, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 89, 35000000m, "Dell", 2, "DuongThang", "ChuyenKhoan", "SN-LAP-000089", null, "Dell XPS 13 9310 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 90, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0090", null, null, new DateTime(2025, 3, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Utc), 90, 55000000m, "Apple", 3, "DuongThang", "TienMat", "SN-LAP-000090", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 91, 1, 32000000m, 888888.89m, 0m, null, "2114", "LAP-0091", null, null, new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 3, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 3, 23, 0, 0, 0, 0, DateTimeKind.Utc), 91, 32000000m, "Dell", 4, "DuongThang", "ChuyenKhoan", "SN-LAP-000091", null, "Dell Latitude 7430 (i7/16GB/512GB, vPro)", 36, null, null, "DangSuDung" },
                    { 92, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0092", null, null, new DateTime(2025, 4, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), 92, 35000000m, "HP", 5, "DuongThang", "ChuyenKhoan", "SN-LAP-000092", null, "HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 93, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0093", null, null, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 26, 0, 0, 0, 0, DateTimeKind.Utc), 93, 35000000m, "Lenovo", 6, "DuongThang", "TienMat", "SN-LAP-000093", null, "Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 94, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0094", null, null, new DateTime(2025, 5, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), 94, 55000000m, "Asus", 7, "DuongThang", "ChuyenKhoan", "SN-LAP-000094", null, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 95, 1, 38000000m, 1055555.56m, 0m, null, "2114", "LAP-0095", null, null, new DateTime(2025, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), 95, 38000000m, "Apple", 8, "DuongThang", "ChuyenKhoan", "SN-LAP-000095", null, "Apple MacBook Air 15\" M2 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 96, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0096", null, null, new DateTime(2025, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), 96, 55000000m, "Dell", 1, "DuongThang", "TienMat", "SN-LAP-000096", null, "Dell Precision 5570 (i7/32GB/1TB, Workstation)", 36, null, null, "DangSuDung" },
                    { 97, 1, 45000000m, 1250000m, 0m, null, "2114", "LAP-0097", null, null, new DateTime(2025, 7, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), 97, 45000000m, "Lenovo", 2, "DuongThang", "ChuyenKhoan", "SN-LAP-000097", null, "Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", 36, null, null, "DangSuDung" },
                    { 98, 1, 42000000m, 1166666.67m, 0m, null, "2114", "LAP-0098", null, null, new DateTime(2025, 7, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), 98, 42000000m, "HP", 3, "DuongThang", "ChuyenKhoan", "SN-LAP-000098", null, "HP ZBook Firefly 14 G10 (i7/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 99, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0099", null, null, new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), 99, 35000000m, "Dell", 4, "DuongThang", "TienMat", "SN-LAP-000099", null, "Dell XPS 13 9310 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 100, 3, 980000000m, 8166666.67m, 0m, null, "2113", "OTO-0100", null, null, new DateTime(2025, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), 1, 980000000m, "Toyota", 1, "DuongThang", "ChuyenKhoan", "SN-OTO-000100", null, "Toyota Innova Crysta 2.0G MT 2023 (7 chỗ)", 120, null, null, "DangSuDung" },
                    { 101, 3, 920000000m, 7666666.67m, 0m, null, "2113", "OTO-0101", null, null, new DateTime(2025, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), 2, 920000000m, "Ford", 1, "DuongThang", "ChuyenKhoan", "SN-OTO-000101", null, "Ford Ranger Wildtrak 2.0L Bi-Turbo AT 2023 (Pickup)", 120, null, null, "DangSuDung" },
                    { 102, 3, 2299000000m, 19158333.33m, 0m, null, "2113", "OTO-0102", null, null, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, 2299000000m, "Mercedes-Benz", 1, "DuongThang", "TienMat", "SN-OTO-000102", null, "Mercedes-Benz GLC 300 4MATIC 2023", 120, null, null, "DangSuDung" },
                    { 103, 3, 1500000000m, 12500000m, 0m, null, "2113", "OTO-0103", null, null, new DateTime(2025, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), 4, 1500000000m, "VinFast", 1, "DuongThang", "ChuyenKhoan", "SN-OTO-000103", null, "VinFast VF9 Plus 2023", 120, null, null, "DangSuDung" },
                    { 104, 2, 280000000m, 4666666.67m, 0m, null, "2112", "SRV-0104", null, null, new DateTime(2025, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, 280000000m, "Dell", 4, "DuongThang", "ChuyenKhoan", "SN-SRV-000104", null, "Dell PowerEdge R750xs (2×Xeon Silver, 128GB RAM, 4×3.5\" HDD 4TB)", 60, null, null, "DangSuDung" },
                    { 105, 4, 45000000m, 1250000m, 0m, null, "2112", "NET-0105", null, null, new DateTime(2025, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, 45000000m, "Cisco", 4, "DuongThang", "TienMat", "SN-NET-000105", null, "Cisco Catalyst 9200L-24P-4G (Switch 24-port PoE)", 36, null, null, "DangSuDung" },
                    { 106, 4, 90000000m, 2500000m, 0m, null, "2112", "NET-0106", null, null, new DateTime(2021, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), null, 90000000m, "Fortinet", 4, "DuongThang", "ChuyenKhoan", "SN-NET-000106", null, "Fortinet FortiGate 100F (NGFW, Firewall)", 36, null, null, "DangSuDung" },
                    { 107, 4, 35000000m, 972222.22m, 0m, null, "2112", "NET-0107", null, null, new DateTime(2021, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, 35000000m, "Cisco", 4, "DuongThang", "ChuyenKhoan", "SN-NET-000107", null, "Cisco Meraki MS120-48LP (Cloud Managed Switch)", 36, null, null, "DangSuDung" },
                    { 108, 4, 120000000m, 3333333.33m, 0m, null, "2112", "NET-0108", null, null, new DateTime(2021, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 120000000m, "Palo Alto", 4, "DuongThang", "TienMat", "SN-NET-000108", null, "Palo Alto PA-440 (Next-Gen Firewall)", 36, null, null, "DangSuDung" }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "chi_tiet_chung_tu",
                columns: new[] { "Id", "ChungTuId", "MoTa", "SoTien", "TaiKhoanCo", "TaiKhoanNo", "TaiSanId" },
                values: new object[,]
                {
                    { 1, 1, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 32000000m, "112", "2114", 1 },
                    { 2, 2, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 2 },
                    { 3, 3, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "111", "2114", 3 },
                    { 4, 4, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 4 },
                    { 5, 5, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 38000000m, "112", "2114", 5 },
                    { 6, 6, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "111", "2114", 6 },
                    { 7, 7, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 45000000m, "112", "2114", 7 },
                    { 8, 8, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 42000000m, "112", "2114", 8 },
                    { 9, 9, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "111", "2114", 9 },
                    { 10, 10, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 10 },
                    { 11, 11, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 32000000m, "112", "2114", 11 },
                    { 12, 12, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "111", "2114", 12 },
                    { 13, 13, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 13 },
                    { 14, 14, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 14 },
                    { 15, 15, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 38000000m, "111", "2114", 15 },
                    { 16, 16, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 16 },
                    { 17, 17, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 45000000m, "112", "2114", 17 },
                    { 18, 18, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 42000000m, "111", "2114", 18 },
                    { 19, 19, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 19 },
                    { 20, 20, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 20 },
                    { 21, 21, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 32000000m, "111", "2114", 21 },
                    { 22, 22, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 22 },
                    { 23, 23, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 23 },
                    { 24, 24, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "111", "2114", 24 },
                    { 25, 25, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 38000000m, "112", "2114", 25 },
                    { 26, 26, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 26 },
                    { 27, 27, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 45000000m, "111", "2114", 27 },
                    { 28, 28, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 42000000m, "112", "2114", 28 },
                    { 29, 29, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 29 },
                    { 30, 30, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "111", "2114", 30 },
                    { 31, 31, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 32000000m, "112", "2114", 31 },
                    { 32, 32, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 32 },
                    { 33, 33, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "111", "2114", 33 },
                    { 34, 34, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 34 },
                    { 35, 35, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 38000000m, "112", "2114", 35 },
                    { 36, 36, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "111", "2114", 36 },
                    { 37, 37, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 45000000m, "112", "2114", 37 },
                    { 38, 38, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 42000000m, "112", "2114", 38 },
                    { 39, 39, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "111", "2114", 39 },
                    { 40, 40, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 40 },
                    { 41, 41, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 32000000m, "112", "2114", 41 },
                    { 42, 42, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "111", "2114", 42 },
                    { 43, 43, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 43 },
                    { 44, 44, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 44 },
                    { 45, 45, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 38000000m, "111", "2114", 45 },
                    { 46, 46, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 46 },
                    { 47, 47, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 45000000m, "112", "2114", 47 },
                    { 48, 48, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 42000000m, "111", "2114", 48 },
                    { 49, 49, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 49 },
                    { 50, 50, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 50 },
                    { 51, 51, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 32000000m, "111", "2114", 51 },
                    { 52, 52, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 52 },
                    { 53, 53, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 53 },
                    { 54, 54, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "111", "2114", 54 },
                    { 55, 55, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 38000000m, "112", "2114", 55 },
                    { 56, 56, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 56 },
                    { 57, 57, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 45000000m, "111", "2114", 57 },
                    { 58, 58, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 42000000m, "112", "2114", 58 },
                    { 59, 59, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 59 },
                    { 60, 60, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "111", "2114", 60 },
                    { 61, 61, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 32000000m, "112", "2114", 61 },
                    { 62, 62, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 62 },
                    { 63, 63, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "111", "2114", 63 },
                    { 64, 64, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 64 },
                    { 65, 65, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 38000000m, "112", "2114", 65 },
                    { 66, 66, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "111", "2114", 66 },
                    { 67, 67, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 45000000m, "112", "2114", 67 },
                    { 68, 68, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 42000000m, "112", "2114", 68 },
                    { 69, 69, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "111", "2114", 69 },
                    { 70, 70, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 70 },
                    { 71, 71, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 32000000m, "112", "2114", 71 },
                    { 72, 72, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "111", "2114", 72 },
                    { 73, 73, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 73 },
                    { 74, 74, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 74 },
                    { 75, 75, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 38000000m, "111", "2114", 75 },
                    { 76, 76, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 76 },
                    { 77, 77, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 45000000m, "112", "2114", 77 },
                    { 78, 78, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 42000000m, "111", "2114", 78 },
                    { 79, 79, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 79 },
                    { 80, 80, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 80 },
                    { 81, 81, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 32000000m, "111", "2114", 81 },
                    { 82, 82, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 82 },
                    { 83, 83, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 83 },
                    { 84, 84, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "111", "2114", 84 },
                    { 85, 85, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 38000000m, "112", "2114", 85 },
                    { 86, 86, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 86 },
                    { 87, 87, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 45000000m, "111", "2114", 87 },
                    { 88, 88, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 42000000m, "112", "2114", 88 },
                    { 89, 89, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 89 },
                    { 90, 90, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "111", "2114", 90 },
                    { 91, 91, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 32000000m, "112", "2114", 91 },
                    { 92, 92, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 92 },
                    { 93, 93, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "111", "2114", 93 },
                    { 94, 94, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 94 },
                    { 95, 95, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 38000000m, "112", "2114", 95 },
                    { 96, 96, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "111", "2114", 96 },
                    { 97, 97, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 45000000m, "112", "2114", 97 },
                    { 98, 98, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 42000000m, "112", "2114", 98 },
                    { 99, 99, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "111", "2114", 99 },
                    { 100, 100, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 980000000m, "112", "2113", 100 },
                    { 101, 101, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 920000000m, "112", "2113", 101 },
                    { 102, 102, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 2299000000m, "111", "2113", 102 },
                    { 103, 103, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 1500000000m, "112", "2113", 103 },
                    { 104, 104, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 280000000m, "112", "2112", 104 },
                    { 105, 105, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 45000000m, "111", "2112", 105 },
                    { 106, 106, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 90000000m, "112", "2112", 106 },
                    { 107, 107, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2112", 107 },
                    { 108, 108, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 120000000m, "111", "2112", 108 }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "dieu_chuyen_tai_san",
                columns: new[] { "Id", "DenNguoiDungId", "DenPhongBanId", "GhiChu", "LoaiDieuChuyen", "NgayTao", "NgayThucHien", "TaiSanId", "TrangThai", "TuNguoiDungId", "TuPhongBanId" },
                values: new object[,]
                {
                    { 1, 1, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), 1, "da_hoan_thanh", null, null },
                    { 2, 2, 3, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2, "da_hoan_thanh", null, null },
                    { 3, 3, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "da_hoan_thanh", null, null },
                    { 4, 4, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 4, "da_hoan_thanh", null, null },
                    { 5, 5, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 3, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 3, 27, 0, 0, 0, 0, DateTimeKind.Utc), 5, "da_hoan_thanh", null, null },
                    { 6, 6, 7, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 4, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 4, 13, 0, 0, 0, 0, DateTimeKind.Utc), 6, "da_hoan_thanh", null, null },
                    { 7, 7, 8, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 4, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 4, 30, 0, 0, 0, 0, DateTimeKind.Utc), 7, "da_hoan_thanh", null, null },
                    { 8, 8, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), 8, "da_hoan_thanh", null, null },
                    { 9, 9, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), 9, "da_hoan_thanh", null, null },
                    { 10, 10, 3, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), 10, "da_hoan_thanh", null, null },
                    { 11, 11, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), 11, "da_hoan_thanh", null, null },
                    { 12, 12, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 7, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 7, 24, 0, 0, 0, 0, DateTimeKind.Utc), 12, "da_hoan_thanh", null, null },
                    { 13, 13, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), 13, "da_hoan_thanh", null, null },
                    { 14, 14, 7, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 8, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 27, 0, 0, 0, 0, DateTimeKind.Utc), 14, "da_hoan_thanh", null, null },
                    { 15, 15, 8, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 9, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 13, 0, 0, 0, 0, DateTimeKind.Utc), 15, "da_hoan_thanh", null, null },
                    { 16, 16, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), 16, "da_hoan_thanh", null, null },
                    { 17, 17, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), 17, "da_hoan_thanh", null, null },
                    { 18, 18, 3, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), 18, "da_hoan_thanh", null, null },
                    { 19, 19, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 19, "da_hoan_thanh", null, null },
                    { 20, 20, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), 20, "da_hoan_thanh", null, null },
                    { 21, 21, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 21, "da_hoan_thanh", null, null },
                    { 22, 22, 7, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 22, "da_hoan_thanh", null, null },
                    { 23, 23, 8, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), 23, "da_hoan_thanh", null, null },
                    { 24, 24, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), 24, "da_hoan_thanh", null, null },
                    { 25, 25, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), 25, "da_hoan_thanh", null, null },
                    { 26, 26, 3, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 26, "da_hoan_thanh", null, null },
                    { 27, 27, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), 27, "da_hoan_thanh", null, null },
                    { 28, 28, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), 28, "da_hoan_thanh", null, null },
                    { 29, 29, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), 29, "da_hoan_thanh", null, null },
                    { 30, 30, 7, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), 30, "da_hoan_thanh", null, null },
                    { 31, 31, 8, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), 31, "da_hoan_thanh", null, null },
                    { 32, 32, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 6, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 29, 0, 0, 0, 0, DateTimeKind.Utc), 32, "da_hoan_thanh", null, null },
                    { 33, 33, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 7, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 7, 16, 0, 0, 0, 0, DateTimeKind.Utc), 33, "da_hoan_thanh", null, null },
                    { 34, 34, 3, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), 34, "da_hoan_thanh", null, null },
                    { 35, 35, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), 35, "da_hoan_thanh", null, null },
                    { 36, 36, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), 36, "da_hoan_thanh", null, null },
                    { 37, 37, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), 37, "da_hoan_thanh", null, null },
                    { 38, 38, 7, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), 38, "da_hoan_thanh", null, null },
                    { 39, 39, 8, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), 39, "da_hoan_thanh", null, null },
                    { 40, 40, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), 40, "da_hoan_thanh", null, null },
                    { 41, 41, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 41, "da_hoan_thanh", null, null },
                    { 42, 42, 3, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 42, "da_hoan_thanh", null, null },
                    { 43, 43, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 43, "da_hoan_thanh", null, null },
                    { 44, 44, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), 44, "da_hoan_thanh", null, null },
                    { 45, 45, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), 45, "da_hoan_thanh", null, null },
                    { 46, 46, 7, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), 46, "da_hoan_thanh", null, null },
                    { 47, 47, 8, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Utc), 47, "da_hoan_thanh", null, null },
                    { 48, 48, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 3, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 28, 0, 0, 0, 0, DateTimeKind.Utc), 48, "da_hoan_thanh", null, null },
                    { 49, 49, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 4, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 14, 0, 0, 0, 0, DateTimeKind.Utc), 49, "da_hoan_thanh", null, null },
                    { 50, 50, 3, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 50, "da_hoan_thanh", null, null },
                    { 51, 51, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 5, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 5, 18, 0, 0, 0, 0, DateTimeKind.Utc), 51, "da_hoan_thanh", null, null },
                    { 52, 52, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), 52, "da_hoan_thanh", null, null },
                    { 53, 53, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), 53, "da_hoan_thanh", null, null },
                    { 54, 54, 7, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 7, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 8, 0, 0, 0, 0, DateTimeKind.Utc), 54, "da_hoan_thanh", null, null },
                    { 55, 55, 8, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 7, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 25, 0, 0, 0, 0, DateTimeKind.Utc), 55, "da_hoan_thanh", null, null },
                    { 56, 56, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), 56, "da_hoan_thanh", null, null },
                    { 57, 57, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), 57, "da_hoan_thanh", null, null },
                    { 58, 58, 3, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), 58, "da_hoan_thanh", null, null },
                    { 59, 59, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 59, "da_hoan_thanh", null, null },
                    { 60, 60, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), 60, "da_hoan_thanh", null, null },
                    { 61, 61, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), 61, "da_hoan_thanh", null, null },
                    { 62, 62, 7, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 62, "da_hoan_thanh", null, null },
                    { 63, 63, 8, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), 63, "da_hoan_thanh", null, null },
                    { 64, 64, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 64, "da_hoan_thanh", null, null },
                    { 65, 65, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 65, "da_hoan_thanh", null, null },
                    { 66, 66, 3, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), 66, "da_hoan_thanh", null, null },
                    { 67, 67, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), 67, "da_hoan_thanh", null, null },
                    { 68, 68, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), 68, "da_hoan_thanh", null, null },
                    { 69, 69, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 69, "da_hoan_thanh", null, null },
                    { 70, 70, 7, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), 70, "da_hoan_thanh", null, null },
                    { 71, 71, 8, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), 71, "da_hoan_thanh", null, null },
                    { 72, 72, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), 72, "da_hoan_thanh", null, null },
                    { 73, 73, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), 73, "da_hoan_thanh", null, null },
                    { 74, 74, 3, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), 74, "da_hoan_thanh", null, null },
                    { 75, 75, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 6, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 6, 29, 0, 0, 0, 0, DateTimeKind.Utc), 75, "da_hoan_thanh", null, null },
                    { 76, 76, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 7, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 7, 16, 0, 0, 0, 0, DateTimeKind.Utc), 76, "da_hoan_thanh", null, null },
                    { 77, 77, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), 77, "da_hoan_thanh", null, null },
                    { 78, 78, 7, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), 78, "da_hoan_thanh", null, null },
                    { 79, 79, 8, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), 79, "da_hoan_thanh", null, null },
                    { 80, 80, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), 80, "da_hoan_thanh", null, null },
                    { 81, 81, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), 81, "da_hoan_thanh", null, null },
                    { 82, 82, 3, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), 82, "da_hoan_thanh", null, null },
                    { 83, 83, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), 83, "da_hoan_thanh", null, null },
                    { 84, 84, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 84, "da_hoan_thanh", null, null },
                    { 85, 85, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 85, "da_hoan_thanh", null, null },
                    { 86, 86, 7, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 86, "da_hoan_thanh", null, null },
                    { 87, 87, 8, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), 87, "da_hoan_thanh", null, null },
                    { 88, 88, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), 88, "da_hoan_thanh", null, null },
                    { 89, 89, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), 89, "da_hoan_thanh", null, null },
                    { 90, 90, 3, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 3, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 3, 11, 0, 0, 0, 0, DateTimeKind.Utc), 90, "da_hoan_thanh", null, null },
                    { 91, 91, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Utc), 91, "da_hoan_thanh", null, null },
                    { 92, 92, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 4, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 14, 0, 0, 0, 0, DateTimeKind.Utc), 92, "da_hoan_thanh", null, null },
                    { 93, 93, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 93, "da_hoan_thanh", null, null },
                    { 94, 94, 7, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 5, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 18, 0, 0, 0, 0, DateTimeKind.Utc), 94, "da_hoan_thanh", null, null },
                    { 95, 95, 8, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), 95, "da_hoan_thanh", null, null },
                    { 96, 96, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), 96, "da_hoan_thanh", null, null },
                    { 97, 97, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 7, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 8, 0, 0, 0, 0, DateTimeKind.Utc), 97, "da_hoan_thanh", null, null },
                    { 98, 98, 3, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 7, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 25, 0, 0, 0, 0, DateTimeKind.Utc), 98, "da_hoan_thanh", null, null },
                    { 99, 99, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), 99, "da_hoan_thanh", null, null },
                    { 100, 1, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), 100, "da_hoan_thanh", null, null },
                    { 101, 2, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), 101, "da_hoan_thanh", null, null },
                    { 102, 3, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 102, "da_hoan_thanh", null, null },
                    { 103, 4, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), 103, "da_hoan_thanh", null, null },
                    { 104, null, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), 104, "da_hoan_thanh", null, null },
                    { 105, null, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2025, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 105, "da_hoan_thanh", null, null },
                    { 106, null, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 106, "da_hoan_thanh", null, null },
                    { 107, null, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), 107, "da_hoan_thanh", null, null },
                    { 108, null, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), 108, "da_hoan_thanh", null, null }
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
                name: "IX_tai_san_dinh_kem_TaiSanId",
                schema: "asset",
                table: "tai_san_dinh_kem",
                column: "TaiSanId");

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
                name: "tai_san_dinh_kem",
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
