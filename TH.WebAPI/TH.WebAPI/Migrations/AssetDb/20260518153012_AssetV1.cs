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
                    { 1, "GhiTang", "CT-GT-0001", "Ghi tăng Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2021, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 2, "GhiTang", "CT-GT-0002", "Ghi tăng Dell Latitude 7430 (i7/16GB/512GB, vPro)", new DateTime(2021, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc), 6, 32000000m, "hoan_thanh" },
                    { 3, "GhiTang", "CT-GT-0003", "Ghi tăng HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", new DateTime(2021, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 4, "GhiTang", "CT-GT-0004", "Ghi tăng Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", new DateTime(2021, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 5, "GhiTang", "CT-GT-0005", "Ghi tăng Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", new DateTime(2021, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 6, "GhiTang", "CT-GT-0006", "Ghi tăng Apple MacBook Air 15\" M2 (16GB/512GB)", new DateTime(2021, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 7, "GhiTang", "CT-GT-0007", "Ghi tăng Dell Precision 5570 (i7/32GB/1TB, Workstation)", new DateTime(2021, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 8, "GhiTang", "CT-GT-0008", "Ghi tăng Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", new DateTime(2021, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 9, "GhiTang", "CT-GT-0009", "Ghi tăng HP ZBook Firefly 14 G10 (i7/32GB/1TB)", new DateTime(2021, 5, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 29, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" },
                    { 10, "GhiTang", "CT-GT-0010", "Ghi tăng Dell XPS 13 9310 (i7/16GB/512GB)", new DateTime(2021, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 11, "GhiTang", "CT-GT-0011", "Ghi tăng Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2021, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 12, "GhiTang", "CT-GT-0012", "Ghi tăng Dell Latitude 7430 (i7/16GB/512GB, vPro)", new DateTime(2021, 7, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 7, 19, 0, 0, 0, 0, DateTimeKind.Utc), 6, 32000000m, "hoan_thanh" },
                    { 13, "GhiTang", "CT-GT-0013", "Ghi tăng HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 14, "GhiTang", "CT-GT-0014", "Ghi tăng Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", new DateTime(2021, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 15, "GhiTang", "CT-GT-0015", "Ghi tăng Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", new DateTime(2021, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 16, "GhiTang", "CT-GT-0016", "Ghi tăng Apple MacBook Air 15\" M2 (16GB/512GB)", new DateTime(2021, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 17, "GhiTang", "CT-GT-0017", "Ghi tăng Dell Precision 5570 (i7/32GB/1TB, Workstation)", new DateTime(2021, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 18, "GhiTang", "CT-GT-0018", "Ghi tăng Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", new DateTime(2021, 10, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 10, 29, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 19, "GhiTang", "CT-GT-0019", "Ghi tăng HP ZBook Firefly 14 G10 (i7/32GB/1TB)", new DateTime(2021, 11, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 11, 15, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" },
                    { 20, "GhiTang", "CT-GT-0020", "Ghi tăng Dell XPS 13 9310 (i7/16GB/512GB)", new DateTime(2021, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 21, "GhiTang", "CT-GT-0021", "Ghi tăng Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2021, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 22, "GhiTang", "CT-GT-0022", "Ghi tăng Dell Latitude 7430 (i7/16GB/512GB, vPro)", new DateTime(2022, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 6, 32000000m, "hoan_thanh" },
                    { 23, "GhiTang", "CT-GT-0023", "Ghi tăng HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", new DateTime(2022, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 24, "GhiTang", "CT-GT-0024", "Ghi tăng Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", new DateTime(2022, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 25, "GhiTang", "CT-GT-0025", "Ghi tăng Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", new DateTime(2022, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 26, "GhiTang", "CT-GT-0026", "Ghi tăng Apple MacBook Air 15\" M2 (16GB/512GB)", new DateTime(2022, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "hoan_thanh" },
                    { 27, "GhiTang", "CT-GT-0027", "Ghi tăng Dell Precision 5570 (i7/32GB/1TB, Workstation)", new DateTime(2022, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 28, "GhiTang", "CT-GT-0028", "Ghi tăng Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", new DateTime(2022, 4, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 4, 17, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 29, "GhiTang", "CT-GT-0029", "Ghi tăng HP ZBook Firefly 14 G10 (i7/32GB/1TB)", new DateTime(2022, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), 6, 42000000m, "hoan_thanh" },
                    { 30, "GhiTang", "CT-GT-0030", "Ghi tăng Dell XPS 13 9310 (i7/16GB/512GB)", new DateTime(2022, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 31, "GhiTang", "CT-GT-0031", "Ghi tăng Apple MacBook Pro 14\" M3 (16GB/512GB)", new DateTime(2022, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 32, "GhiTang", "CT-GT-0032", "Ghi tăng Dell Latitude 7430 (i7/16GB/512GB, vPro)", new DateTime(2022, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), 6, 32000000m, "hoan_thanh" },
                    { 33, "GhiTang", "CT-GT-0033", "Ghi tăng HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", new DateTime(2022, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 34, "GhiTang", "CT-GT-0034", "Ghi tăng Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", new DateTime(2022, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 35, "GhiTang", "CT-GT-0035", "Ghi tăng Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", new DateTime(2022, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), 6, 55000000m, "hoan_thanh" },
                    { 36, "GhiTang", "CT-GT-0036", "Ghi tăng Toyota Innova Crysta 2.0G MT 2023 (7 chỗ)", new DateTime(2022, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), 6, 980000000m, "hoan_thanh" },
                    { 37, "GhiTang", "CT-GT-0037", "Ghi tăng Ford Ranger Wildtrak 2.0L Bi-Turbo AT 2023 (Pickup)", new DateTime(2022, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), 6, 920000000m, "hoan_thanh" },
                    { 38, "GhiTang", "CT-GT-0038", "Ghi tăng Mercedes-Benz GLC 300 4MATIC 2023", new DateTime(2022, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), 6, 2299000000m, "hoan_thanh" },
                    { 39, "GhiTang", "CT-GT-0039", "Ghi tăng VinFast VF9 Plus 2023", new DateTime(2022, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), 6, 1500000000m, "hoan_thanh" },
                    { 40, "GhiTang", "CT-GT-0040", "Ghi tăng Dell PowerEdge R750xs (2×Xeon Silver, 128GB RAM, 4×3.5\" HDD 4TB)", new DateTime(2022, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), 6, 280000000m, "hoan_thanh" },
                    { 41, "GhiTang", "CT-GT-0041", "Ghi tăng Cisco Catalyst 9200L-24P-4G (Switch 24-port PoE)", new DateTime(2022, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 6, 45000000m, "hoan_thanh" },
                    { 42, "GhiTang", "CT-GT-0042", "Ghi tăng Fortinet FortiGate 100F (NGFW, Firewall)", new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 6, 90000000m, "hoan_thanh" },
                    { 43, "GhiTang", "CT-GT-0043", "Ghi tăng Cisco Meraki MS120-48LP (Cloud Managed Switch)", new DateTime(2022, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 6, 35000000m, "hoan_thanh" },
                    { 44, "GhiTang", "CT-GT-0044", "Ghi tăng Palo Alto PA-440 (Next-Gen Firewall)", new DateTime(2023, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), 6, 120000000m, "hoan_thanh" }
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
                    { 1, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0001", null, null, new DateTime(2021, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), 1, 55000000m, "Apple", 1, "DuongThang", "ChuyenKhoan", "SN-LAP-000001", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 2, 1, 32000000m, 888888.89m, 0m, null, "2114", "LAP-0002", null, null, new DateTime(2021, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc), 2, 32000000m, "Dell", 1, "DuongThang", "ChuyenKhoan", "SN-LAP-000002", null, "Dell Latitude 7430 (i7/16GB/512GB, vPro)", 36, null, null, "DangSuDung" },
                    { 3, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0003", null, null, new DateTime(2021, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, 35000000m, "HP", 1, "DuongThang", "TienMat", "SN-LAP-000003", null, "HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 4, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0004", null, null, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 4, 35000000m, "Lenovo", 1, "DuongThang", "ChuyenKhoan", "SN-LAP-000004", null, "Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 5, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0005", null, null, new DateTime(2021, 3, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), 5, 55000000m, "Asus", 2, "DuongThang", "ChuyenKhoan", "SN-LAP-000005", null, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 6, 1, 38000000m, 1055555.56m, 0m, null, "2114", "LAP-0006", null, null, new DateTime(2021, 4, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), 6, 38000000m, "Apple", 2, "DuongThang", "TienMat", "SN-LAP-000006", null, "Apple MacBook Air 15\" M2 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 7, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0007", null, null, new DateTime(2021, 4, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), 7, 55000000m, "Dell", 2, "DuongThang", "ChuyenKhoan", "SN-LAP-000007", null, "Dell Precision 5570 (i7/32GB/1TB, Workstation)", 36, null, null, "DangSuDung" },
                    { 8, 1, 45000000m, 1250000m, 0m, null, "2114", "LAP-0008", null, null, new DateTime(2021, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), 8, 45000000m, "Lenovo", 2, "DuongThang", "ChuyenKhoan", "SN-LAP-000008", null, "Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", 36, null, null, "DangSuDung" },
                    { 9, 1, 42000000m, 1166666.67m, 0m, null, "2114", "LAP-0009", null, null, new DateTime(2021, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 29, 0, 0, 0, 0, DateTimeKind.Utc), 9, 42000000m, "HP", 2, "DuongThang", "TienMat", "SN-LAP-000009", null, "HP ZBook Firefly 14 G10 (i7/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 10, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0010", null, null, new DateTime(2021, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 10, 35000000m, "Dell", 3, "DuongThang", "ChuyenKhoan", "SN-LAP-000010", null, "Dell XPS 13 9310 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 11, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0011", null, null, new DateTime(2021, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), 11, 55000000m, "Apple", 3, "DuongThang", "ChuyenKhoan", "SN-LAP-000011", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 12, 1, 32000000m, 888888.89m, 0m, null, "2114", "LAP-0012", null, null, new DateTime(2021, 7, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 7, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 7, 19, 0, 0, 0, 0, DateTimeKind.Utc), 12, 32000000m, "Dell", 4, "DuongThang", "TienMat", "SN-LAP-000012", null, "Dell Latitude 7430 (i7/16GB/512GB, vPro)", 36, null, null, "DangSuDung" },
                    { 13, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0013", null, null, new DateTime(2021, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 13, 35000000m, "HP", 4, "DuongThang", "ChuyenKhoan", "SN-LAP-000013", null, "HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 14, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0014", null, null, new DateTime(2021, 8, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), 14, 35000000m, "Lenovo", 4, "DuongThang", "ChuyenKhoan", "SN-LAP-000014", null, "Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 15, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0015", null, null, new DateTime(2021, 9, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 15, 55000000m, "Asus", 5, "DuongThang", "TienMat", "SN-LAP-000015", null, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 16, 1, 38000000m, 1055555.56m, 0m, null, "2114", "LAP-0016", null, null, new DateTime(2021, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), 16, 38000000m, "Apple", 5, "DuongThang", "ChuyenKhoan", "SN-LAP-000016", null, "Apple MacBook Air 15\" M2 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 17, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0017", null, null, new DateTime(2021, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), 17, 55000000m, "Dell", 5, "DuongThang", "ChuyenKhoan", "SN-LAP-000017", null, "Dell Precision 5570 (i7/32GB/1TB, Workstation)", 36, null, null, "DangSuDung" },
                    { 18, 1, 45000000m, 1250000m, 0m, null, "2114", "LAP-0018", null, null, new DateTime(2021, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 10, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 10, 29, 0, 0, 0, 0, DateTimeKind.Utc), 18, 45000000m, "Lenovo", 5, "DuongThang", "TienMat", "SN-LAP-000018", null, "Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", 36, null, null, "DangSuDung" },
                    { 19, 1, 42000000m, 1166666.67m, 0m, null, "2114", "LAP-0019", null, null, new DateTime(2021, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 11, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 11, 15, 0, 0, 0, 0, DateTimeKind.Utc), 19, 42000000m, "HP", 5, "DuongThang", "ChuyenKhoan", "SN-LAP-000019", null, "HP ZBook Firefly 14 G10 (i7/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 20, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0020", null, null, new DateTime(2021, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 20, 35000000m, "Dell", 6, "DuongThang", "ChuyenKhoan", "SN-LAP-000020", null, "Dell XPS 13 9310 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 21, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0021", null, null, new DateTime(2021, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 21, 55000000m, "Apple", 6, "DuongThang", "TienMat", "SN-LAP-000021", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 22, 1, 32000000m, 888888.89m, 0m, null, "2114", "LAP-0022", null, null, new DateTime(2022, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 22, 32000000m, "Dell", 6, "DuongThang", "ChuyenKhoan", "SN-LAP-000022", null, "Dell Latitude 7430 (i7/16GB/512GB, vPro)", 36, null, null, "DangSuDung" },
                    { 23, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0023", null, null, new DateTime(2022, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), 23, 35000000m, "HP", 6, "DuongThang", "ChuyenKhoan", "SN-LAP-000023", null, "HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 24, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0024", null, null, new DateTime(2022, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), 24, 35000000m, "Lenovo", 6, "DuongThang", "TienMat", "SN-LAP-000024", null, "Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 25, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0025", null, null, new DateTime(2022, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), 25, 55000000m, "Asus", 6, "DuongThang", "ChuyenKhoan", "SN-LAP-000025", null, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 26, 1, 38000000m, 1055555.56m, 0m, null, "2114", "LAP-0026", null, null, new DateTime(2022, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), 26, 38000000m, "Apple", 6, "DuongThang", "ChuyenKhoan", "SN-LAP-000026", null, "Apple MacBook Air 15\" M2 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 27, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0027", null, null, new DateTime(2022, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), 27, 55000000m, "Dell", 6, "DuongThang", "TienMat", "SN-LAP-000027", null, "Dell Precision 5570 (i7/32GB/1TB, Workstation)", 36, null, null, "DangSuDung" },
                    { 28, 1, 45000000m, 1250000m, 0m, null, "2114", "LAP-0028", null, null, new DateTime(2022, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 4, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 4, 17, 0, 0, 0, 0, DateTimeKind.Utc), 28, 45000000m, "Lenovo", 6, "DuongThang", "ChuyenKhoan", "SN-LAP-000028", null, "Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", 36, null, null, "DangSuDung" },
                    { 29, 1, 42000000m, 1166666.67m, 0m, null, "2114", "LAP-0029", null, null, new DateTime(2022, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), 29, 42000000m, "HP", 6, "DuongThang", "ChuyenKhoan", "SN-LAP-000029", null, "HP ZBook Firefly 14 G10 (i7/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 30, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0030", null, null, new DateTime(2022, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), 30, 35000000m, "Dell", 7, "DuongThang", "TienMat", "SN-LAP-000030", null, "Dell XPS 13 9310 (i7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 31, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0031", null, null, new DateTime(2022, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), 31, 55000000m, "Apple", 7, "DuongThang", "ChuyenKhoan", "SN-LAP-000031", null, "Apple MacBook Pro 14\" M3 (16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 32, 1, 32000000m, 888888.89m, 0m, null, "2114", "LAP-0032", null, null, new DateTime(2022, 6, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), 32, 32000000m, "Dell", 7, "DuongThang", "ChuyenKhoan", "SN-LAP-000032", null, "Dell Latitude 7430 (i7/16GB/512GB, vPro)", 36, null, null, "DangSuDung" },
                    { 33, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0033", null, null, new DateTime(2022, 7, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 33, 35000000m, "HP", 8, "DuongThang", "TienMat", "SN-LAP-000033", null, "HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 34, 1, 35000000m, 972222.22m, 0m, null, "2114", "LAP-0034", null, null, new DateTime(2022, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), 34, 35000000m, "Lenovo", 8, "DuongThang", "ChuyenKhoan", "SN-LAP-000034", null, "Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", 36, null, null, "DangSuDung" },
                    { 35, 1, 55000000m, 1527777.78m, 0m, null, "2114", "LAP-0035", null, null, new DateTime(2022, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), 35, 55000000m, "Asus", 8, "DuongThang", "ChuyenKhoan", "SN-LAP-000035", null, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", 36, null, null, "DangSuDung" },
                    { 36, 3, 980000000m, 8166666.67m, 0m, null, "2113", "OTO-0036", null, null, new DateTime(2022, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), 1, 980000000m, "Toyota", 1, "DuongThang", "TienMat", "SN-OTO-000036", null, "Toyota Innova Crysta 2.0G MT 2023 (7 chỗ)", 120, null, null, "DangSuDung" },
                    { 37, 3, 920000000m, 7666666.67m, 0m, null, "2113", "OTO-0037", null, null, new DateTime(2022, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), 2, 920000000m, "Ford", 1, "DuongThang", "ChuyenKhoan", "SN-OTO-000037", null, "Ford Ranger Wildtrak 2.0L Bi-Turbo AT 2023 (Pickup)", 120, null, null, "DangSuDung" },
                    { 38, 3, 2299000000m, 19158333.33m, 0m, null, "2113", "OTO-0038", null, null, new DateTime(2022, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), 3, 2299000000m, "Mercedes-Benz", 1, "DuongThang", "ChuyenKhoan", "SN-OTO-000038", null, "Mercedes-Benz GLC 300 4MATIC 2023", 120, null, null, "DangSuDung" },
                    { 39, 3, 1500000000m, 12500000m, 0m, null, "2113", "OTO-0039", null, null, new DateTime(2022, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), 4, 1500000000m, "VinFast", 1, "DuongThang", "TienMat", "SN-OTO-000039", null, "VinFast VF9 Plus 2023", 120, null, null, "DangSuDung" },
                    { 40, 2, 280000000m, 4666666.67m, 0m, null, "2112", "SRV-0040", null, null, new DateTime(2022, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), 12, 280000000m, "Dell", 4, "DuongThang", "ChuyenKhoan", "SN-SRV-000040", null, "Dell PowerEdge R750xs (2×Xeon Silver, 128GB RAM, 4×3.5\" HDD 4TB)", 60, null, null, "DangSuDung" },
                    { 41, 4, 45000000m, 1250000m, 0m, null, "2112", "NET-0041", null, null, new DateTime(2022, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 12, 45000000m, "Cisco", 4, "DuongThang", "ChuyenKhoan", "SN-NET-000041", null, "Cisco Catalyst 9200L-24P-4G (Switch 24-port PoE)", 36, null, null, "DangSuDung" },
                    { 42, 4, 90000000m, 2500000m, 0m, null, "2112", "NET-0042", null, null, new DateTime(2022, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 12, 90000000m, "Fortinet", 4, "DuongThang", "TienMat", "SN-NET-000042", null, "Fortinet FortiGate 100F (NGFW, Firewall)", 36, null, null, "DangSuDung" },
                    { 43, 4, 35000000m, 972222.22m, 0m, null, "2112", "NET-0043", null, null, new DateTime(2023, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 12, 35000000m, "Cisco", 4, "DuongThang", "ChuyenKhoan", "SN-NET-000043", null, "Cisco Meraki MS120-48LP (Cloud Managed Switch)", 36, null, null, "DangSuDung" },
                    { 44, 4, 120000000m, 3333333.33m, 0m, null, "2112", "NET-0044", null, null, new DateTime(2023, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), 12, 120000000m, "Palo Alto", 4, "DuongThang", "ChuyenKhoan", "SN-NET-000044", null, "Palo Alto PA-440 (Next-Gen Firewall)", 36, null, null, "DangSuDung" }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "chi_tiet_chung_tu",
                columns: new[] { "Id", "ChungTuId", "MoTa", "SoTien", "TaiKhoanCo", "TaiKhoanNo", "TaiSanId" },
                values: new object[,]
                {
                    { 1, 1, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 1 },
                    { 2, 2, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 32000000m, "112", "2114", 2 },
                    { 3, 3, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "111", "2114", 3 },
                    { 4, 4, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 4 },
                    { 5, 5, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 5 },
                    { 6, 6, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 38000000m, "111", "2114", 6 },
                    { 7, 7, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 7 },
                    { 8, 8, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 45000000m, "112", "2114", 8 },
                    { 9, 9, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 42000000m, "111", "2114", 9 },
                    { 10, 10, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 10 },
                    { 11, 11, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 11 },
                    { 12, 12, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 32000000m, "111", "2114", 12 },
                    { 13, 13, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 13 },
                    { 14, 14, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 14 },
                    { 15, 15, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "111", "2114", 15 },
                    { 16, 16, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 38000000m, "112", "2114", 16 },
                    { 17, 17, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 17 },
                    { 18, 18, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 45000000m, "111", "2114", 18 },
                    { 19, 19, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 42000000m, "112", "2114", 19 },
                    { 20, 20, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 20 },
                    { 21, 21, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "111", "2114", 21 },
                    { 22, 22, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 32000000m, "112", "2114", 22 },
                    { 23, 23, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 23 },
                    { 24, 24, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "111", "2114", 24 },
                    { 25, 25, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 25 },
                    { 26, 26, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 38000000m, "112", "2114", 26 },
                    { 27, 27, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "111", "2114", 27 },
                    { 28, 28, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 45000000m, "112", "2114", 28 },
                    { 29, 29, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 42000000m, "112", "2114", 29 },
                    { 30, 30, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "111", "2114", 30 },
                    { 31, 31, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 31 },
                    { 32, 32, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 32000000m, "112", "2114", 32 },
                    { 33, 33, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "111", "2114", 33 },
                    { 34, 34, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2114", 34 },
                    { 35, 35, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 55000000m, "112", "2114", 35 },
                    { 36, 36, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 980000000m, "111", "2113", 36 },
                    { 37, 37, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 920000000m, "112", "2113", 37 },
                    { 38, 38, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 2299000000m, "112", "2113", 38 },
                    { 39, 39, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 1500000000m, "111", "2113", 39 },
                    { 40, 40, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 280000000m, "112", "2112", 40 },
                    { 41, 41, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 45000000m, "112", "2112", 41 },
                    { 42, 42, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 90000000m, "111", "2112", 42 },
                    { 43, 43, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 35000000m, "112", "2112", 43 },
                    { 44, 44, "Thanh toán & Ghi tăng nguyên giá TSCĐ", 120000000m, "112", "2112", 44 }
                });

            migrationBuilder.InsertData(
                schema: "asset",
                table: "dieu_chuyen_tai_san",
                columns: new[] { "Id", "DenNguoiDungId", "DenPhongBanId", "GhiChu", "LoaiDieuChuyen", "NgayTao", "NgayThucHien", "TaiSanId", "TrangThai", "TuNguoiDungId", "TuPhongBanId" },
                values: new object[,]
                {
                    { 1, 1, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), 1, "da_hoan_thanh", null, null },
                    { 2, 2, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2, "da_hoan_thanh", null, null },
                    { 3, 3, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "da_hoan_thanh", null, null },
                    { 4, 4, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 4, "da_hoan_thanh", null, null },
                    { 5, 5, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 3, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 3, 27, 0, 0, 0, 0, DateTimeKind.Utc), 5, "da_hoan_thanh", null, null },
                    { 6, 6, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 4, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 4, 13, 0, 0, 0, 0, DateTimeKind.Utc), 6, "da_hoan_thanh", null, null },
                    { 7, 7, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 4, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 4, 30, 0, 0, 0, 0, DateTimeKind.Utc), 7, "da_hoan_thanh", null, null },
                    { 8, 8, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), 8, "da_hoan_thanh", null, null },
                    { 9, 9, 2, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), 9, "da_hoan_thanh", null, null },
                    { 10, 10, 3, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), 10, "da_hoan_thanh", null, null },
                    { 11, 11, 3, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), 11, "da_hoan_thanh", null, null },
                    { 12, 12, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 7, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 7, 24, 0, 0, 0, 0, DateTimeKind.Utc), 12, "da_hoan_thanh", null, null },
                    { 13, 13, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), 13, "da_hoan_thanh", null, null },
                    { 14, 14, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 8, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 27, 0, 0, 0, 0, DateTimeKind.Utc), 14, "da_hoan_thanh", null, null },
                    { 15, 15, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 9, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 13, 0, 0, 0, 0, DateTimeKind.Utc), 15, "da_hoan_thanh", null, null },
                    { 16, 16, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), 16, "da_hoan_thanh", null, null },
                    { 17, 17, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), 17, "da_hoan_thanh", null, null },
                    { 18, 18, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), 18, "da_hoan_thanh", null, null },
                    { 19, 19, 5, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 19, "da_hoan_thanh", null, null },
                    { 20, 20, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), 20, "da_hoan_thanh", null, null },
                    { 21, 21, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2021, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 21, "da_hoan_thanh", null, null },
                    { 22, 22, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 22, "da_hoan_thanh", null, null },
                    { 23, 23, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), 23, "da_hoan_thanh", null, null },
                    { 24, 24, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), 24, "da_hoan_thanh", null, null },
                    { 25, 25, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), 25, "da_hoan_thanh", null, null },
                    { 26, 26, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 26, "da_hoan_thanh", null, null },
                    { 27, 27, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), 27, "da_hoan_thanh", null, null },
                    { 28, 28, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), 28, "da_hoan_thanh", null, null },
                    { 29, 29, 6, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), 29, "da_hoan_thanh", null, null },
                    { 30, 30, 7, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), 30, "da_hoan_thanh", null, null },
                    { 31, 31, 7, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), 31, "da_hoan_thanh", null, null },
                    { 32, 32, 7, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 6, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 29, 0, 0, 0, 0, DateTimeKind.Utc), 32, "da_hoan_thanh", null, null },
                    { 33, 33, 8, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 7, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 7, 16, 0, 0, 0, 0, DateTimeKind.Utc), 33, "da_hoan_thanh", null, null },
                    { 34, 34, 8, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), 34, "da_hoan_thanh", null, null },
                    { 35, 35, 8, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), 35, "da_hoan_thanh", null, null },
                    { 36, 1, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), 36, "da_hoan_thanh", null, null },
                    { 37, 2, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), 37, "da_hoan_thanh", null, null },
                    { 38, 3, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), 38, "da_hoan_thanh", null, null },
                    { 39, 4, 1, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), 39, "da_hoan_thanh", null, null },
                    { 40, 12, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), 40, "da_hoan_thanh", null, null },
                    { 41, 12, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 41, "da_hoan_thanh", null, null },
                    { 42, 12, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2022, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 42, "da_hoan_thanh", null, null },
                    { 43, 12, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 43, "da_hoan_thanh", null, null },
                    { 44, 12, 4, "Cấp phát tài sản để bắt đầu sử dụng", "CapPhat", new DateTime(2023, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), 44, "da_hoan_thanh", null, null }
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
