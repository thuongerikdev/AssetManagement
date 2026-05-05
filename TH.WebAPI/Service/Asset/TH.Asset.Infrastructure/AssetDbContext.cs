using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using TH.Asset.Domain.Entities;
using TH.Asset.Domain.Enums;

namespace TH.Asset.Infrastructure.Database
{
    public class AssetDbContext : DbContext
    {
        public DbSet<PhongBan> phongBans { get; set; }
        public DbSet<TaiKhoanKeToan> taiKhoanKeToans { get; set; }
        public DbSet<DanhMucTaiSan> danhMucTaiSans { get; set; }
        public DbSet<CauHinhHeThong> cauHinhHeThongs { get; set; }
        public DbSet<LoTaiSan> loTaiSans { get; set; }
        public DbSet<TaiSan> taiSans { get; set; }
        public DbSet<DieuChuyenTaiSan> dieuChuyenTaiSans { get; set; }
        public DbSet<BaoTriTaiSan> baoTriTaiSans { get; set; }
        public DbSet<ThanhLyTaiSan> thanhLyTaiSans { get; set; }
        public DbSet<LichSuKhauHao> lichSuKhauHaos { get; set; }
        public DbSet<ChungTu> chungTus { get; set; }
        public DbSet<ChiTietChungTu> chiTietChungTus { get; set; }

        public AssetDbContext(DbContextOptions<AssetDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhongBan>().HasIndex(x => x.MaPhongBan).IsUnique();
            modelBuilder.Entity<TaiKhoanKeToan>().HasIndex(x => x.MaTaiKhoan).IsUnique();
            modelBuilder.Entity<DanhMucTaiSan>().HasIndex(x => x.MaDanhMuc).IsUnique();
            modelBuilder.Entity<LoTaiSan>().HasIndex(x => x.MaLo).IsUnique();
            modelBuilder.Entity<TaiSan>().HasIndex(x => x.MaTaiSan).IsUnique();
            modelBuilder.Entity<ChungTu>().HasIndex(x => x.MaChungTu).IsUnique();

            modelBuilder.Entity<TaiKhoanKeToan>().HasOne(t => t.TaiKhoanCha).WithMany().HasForeignKey(t => t.MaTaiKhoanCha).HasPrincipalKey(t => t.MaTaiKhoan).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DanhMucTaiSan>().HasOne(d => d.TaiKhoanKeToan).WithMany().HasForeignKey(d => d.MaTaiKhoan).HasPrincipalKey(t => t.MaTaiKhoan).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TaiSan>().HasOne(t => t.TaiKhoanKeToan).WithMany().HasForeignKey(t => t.MaTaiKhoan).HasPrincipalKey(t => t.MaTaiKhoan).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ChiTietChungTu>().HasOne(c => c.TaiKhoanKeToanNo).WithMany().HasForeignKey(c => c.TaiKhoanNo).HasPrincipalKey(t => t.MaTaiKhoan).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ChiTietChungTu>().HasOne(c => c.TaiKhoanKeToanCo).WithMany().HasForeignKey(c => c.TaiKhoanCo).HasPrincipalKey(t => t.MaTaiKhoan).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaiSan>().HasOne(t => t.DanhMucTaiSan).WithMany(d => d.TaiSans).HasForeignKey(t => t.DanhMucId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TaiSan>().HasOne(t => t.LoTaiSan).WithMany(l => l.TaiSans).HasForeignKey(t => t.LoId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TaiSan>().HasOne(t => t.PhongBan).WithMany(p => p.TaiSans).HasForeignKey(t => t.PhongBanId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DieuChuyenTaiSan>().HasOne(d => d.TaiSan).WithMany(t => t.DieuChuyenTaiSans).HasForeignKey(d => d.TaiSanId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DieuChuyenTaiSan>().HasOne(d => d.TuPhongBan).WithMany().HasForeignKey(d => d.TuPhongBanId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DieuChuyenTaiSan>().HasOne(d => d.DenPhongBan).WithMany().HasForeignKey(d => d.DenPhongBanId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<BaoTriTaiSan>().HasOne(b => b.TaiSan).WithMany(t => t.BaoTriTaiSans).HasForeignKey(b => b.TaiSanId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ThanhLyTaiSan>().HasOne(th => th.TaiSan).WithMany(t => t.ThanhLyTaiSans).HasForeignKey(th => th.TaiSanId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<LichSuKhauHao>().HasOne(l => l.TaiSan).WithMany(t => t.LichSuKhauHaos).HasForeignKey(l => l.TaiSanId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<LichSuKhauHao>().HasOne(l => l.ChungTu).WithMany(c => c.LichSuKhauHaos).HasForeignKey(l => l.ChungTuId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ChiTietChungTu>().HasOne(c => c.ChungTu).WithMany(ch => ch.ChiTietChungTus).HasForeignKey(c => c.ChungTuId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ChiTietChungTu>().HasOne(c => c.TaiSan).WithMany().HasForeignKey(c => c.TaiSanId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TaiSan>().Property(x => x.TrangThai).HasConversion<string>();
            modelBuilder.Entity<TaiSan>().Property(x => x.PhuongPhapKhauHao).HasConversion<string>();
            modelBuilder.Entity<TaiSan>().Property(x => x.PhuongThucThanhToan).HasConversion<string>();
            modelBuilder.Entity<CauHinhHeThong>().Property(x => x.PhuongPhapKhauHaoMacDinh).HasConversion<string>();
            modelBuilder.Entity<DieuChuyenTaiSan>().Property(x => x.LoaiDieuChuyen).HasConversion<string>();
            modelBuilder.Entity<BaoTriTaiSan>().Property(x => x.TrangThai).HasConversion<string>();
            modelBuilder.Entity<ThanhLyTaiSan>().Property(x => x.TrangThai).HasConversion<string>();
            modelBuilder.Entity<ChungTu>().Property(x => x.LoaiChungTu).HasConversion<string>();

            var utcDT = new ValueConverter<DateTime, DateTime>(v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            var utcNDT = new ValueConverter<DateTime?, DateTime?>(v => v == null ? null : (v.Value.Kind == DateTimeKind.Utc ? v.Value : v.Value.ToUniversalTime()), v => v == null ? null : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
                foreach (var prop in entity.GetProperties())
                {
                    if (prop.ClrType == typeof(DateTime)) prop.SetValueConverter(utcDT);
                    if (prop.ClrType == typeof(DateTime?)) prop.SetValueConverter(utcNDT);
                }

            // =========================================================
            // STATIC SEED DATA
            // =========================================================

            // --- CẤU HÌNH HỆ THỐNG (thông tin công ty thực tế) ---
            modelBuilder.Entity<CauHinhHeThong>().HasData(
                new CauHinhHeThong
                {
                    Id = 1,
                    TenCongTy = "Công ty Cổ phần Công nghệ TH",
                    MaSoThue = "0316789456",
                    DiaChi = "Tầng 8, Tòa nhà Sailing Tower, 111A Pasteur, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh",
                    TienToChungTu = "CT",
                    SoBatDauChungTu = 1,
                    TuDongKhauHao = true,
                    DinhDangMaTaiSan = "{DANH_MUC}-{SO_THU_TU}",
                    DoDaiMaTaiSan = 4,
                    PhuongPhapKhauHaoMacDinh = PhuongPhapKhauHao.DuongThang
                }
            );

            // --- TÀI KHOẢN KẾ TOÁN (Chuẩn VAS) ---
            modelBuilder.Entity<TaiKhoanKeToan>().HasData(
                new TaiKhoanKeToan { Id = 1,  MaTaiKhoan = "111",  TenTaiKhoan = "Tiền mặt",                         LoaiTaiKhoan = "Tài sản" },
                new TaiKhoanKeToan { Id = 2,  MaTaiKhoan = "112",  TenTaiKhoan = "Tiền gửi ngân hàng",               LoaiTaiKhoan = "Tài sản" },
                new TaiKhoanKeToan { Id = 3,  MaTaiKhoan = "331",  TenTaiKhoan = "Phải trả cho người bán",           LoaiTaiKhoan = "Nguồn vốn" },
                new TaiKhoanKeToan { Id = 4,  MaTaiKhoan = "211",  TenTaiKhoan = "Tài sản cố định hữu hình",         LoaiTaiKhoan = "Tài sản" },
                new TaiKhoanKeToan { Id = 5,  MaTaiKhoan = "2111", TenTaiKhoan = "Nhà cửa, vật kiến trúc",           LoaiTaiKhoan = "Tài sản", MaTaiKhoanCha = "211" },
                new TaiKhoanKeToan { Id = 6,  MaTaiKhoan = "2112", TenTaiKhoan = "Máy móc, thiết bị",                LoaiTaiKhoan = "Tài sản", MaTaiKhoanCha = "211" },
                new TaiKhoanKeToan { Id = 7,  MaTaiKhoan = "2113", TenTaiKhoan = "Phương tiện vận tải, truyền dẫn",  LoaiTaiKhoan = "Tài sản", MaTaiKhoanCha = "211" },
                new TaiKhoanKeToan { Id = 8,  MaTaiKhoan = "2114", TenTaiKhoan = "Thiết bị, dụng cụ quản lý",        LoaiTaiKhoan = "Tài sản", MaTaiKhoanCha = "211" },
                new TaiKhoanKeToan { Id = 11, MaTaiKhoan = "214",  TenTaiKhoan = "Hao mòn tài sản cố định",          LoaiTaiKhoan = "Nguồn vốn" },
                new TaiKhoanKeToan { Id = 12, MaTaiKhoan = "2141", TenTaiKhoan = "Hao mòn TSCĐ hữu hình",            LoaiTaiKhoan = "Nguồn vốn", MaTaiKhoanCha = "214" },
                new TaiKhoanKeToan { Id = 16, MaTaiKhoan = "627",  TenTaiKhoan = "Chi phí sản xuất chung",           LoaiTaiKhoan = "Chi phí" },
                new TaiKhoanKeToan { Id = 17, MaTaiKhoan = "641",  TenTaiKhoan = "Chi phí bán hàng",                 LoaiTaiKhoan = "Chi phí" },
                new TaiKhoanKeToan { Id = 18, MaTaiKhoan = "642",  TenTaiKhoan = "Chi phí quản lý doanh nghiệp",     LoaiTaiKhoan = "Chi phí" },
                new TaiKhoanKeToan { Id = 19, MaTaiKhoan = "811",  TenTaiKhoan = "Chi phí khác (Lỗ thanh lý)",       LoaiTaiKhoan = "Chi phí" },
                new TaiKhoanKeToan { Id = 20, MaTaiKhoan = "711",  TenTaiKhoan = "Thu nhập khác (Lãi thanh lý)",     LoaiTaiKhoan = "Doanh thu" }
            );

            // --- PHÒNG BAN ---
            modelBuilder.Entity<PhongBan>().HasData(
                new PhongBan { Id = 1, MaPhongBan = "BGD",    TenPhongBan = "Ban giám đốc" },
                new PhongBan { Id = 2, MaPhongBan = "KT",     TenPhongBan = "Phòng kế toán" },
                new PhongBan { Id = 3, MaPhongBan = "NS",     TenPhongBan = "Phòng nhân sự" },
                new PhongBan { Id = 4, MaPhongBan = "TECH",   TenPhongBan = "Phòng kỹ thuật" },
                new PhongBan { Id = 5, MaPhongBan = "PROD",   TenPhongBan = "Phòng sản phẩm" },
                new PhongBan { Id = 6, MaPhongBan = "DEV",    TenPhongBan = "Phòng phát triển phần mềm" },
                new PhongBan { Id = 7, MaPhongBan = "PMO",    TenPhongBan = "Phòng quản lý dự án" },
                new PhongBan { Id = 8, MaPhongBan = "DESIGN", TenPhongBan = "Phòng thiết kế UI/UX" }
            );

            // --- DANH MỤC TÀI SẢN ---
            var danhMucs = new List<DanhMucTaiSan>
            {
                new DanhMucTaiSan { Id = 1, MaDanhMuc = "LAP", TenDanhMuc = "Máy tính xách tay (Laptop)",         TienTo = "LAP", ThoiGianKhauHao = 36,  MaTaiKhoan = "2114" },
                new DanhMucTaiSan { Id = 2, MaDanhMuc = "SRV", TenDanhMuc = "Máy chủ & Thiết bị lưu trữ (NAS)", TienTo = "SRV", ThoiGianKhauHao = 60,  MaTaiKhoan = "2112" },
                new DanhMucTaiSan { Id = 3, MaDanhMuc = "OTO", TenDanhMuc = "Phương tiện vận tải (Ô tô)",        TienTo = "OTO", ThoiGianKhauHao = 120, MaTaiKhoan = "2113" },
                new DanhMucTaiSan { Id = 4, MaDanhMuc = "NET", TenDanhMuc = "Thiết bị mạng & Bảo mật",          TienTo = "NET", ThoiGianKhauHao = 36,  MaTaiKhoan = "2112" }
            };
            modelBuilder.Entity<DanhMucTaiSan>().HasData(danhMucs);

            // =========================================================
            // SEED DATA TÀI SẢN (100 tài sản)
            // Phân bổ theo phòng ban - kế toán KHÔNG được dùng server
            //
            // User IDs:
            //   BGD(1):  1–4   | KT(2):  5–9   | NS(3): 10–11
            //   TECH(4): 12–14 | PROD(5): 15–19 | DEV(6): 20–29
            //   PMO(7):  30–32 | DESIGN(8): 33–35
            //
            // Tài sản:
            //   ID 1–8   : BGD Laptop (8 chiếc, 2/người × 4 người)
            //   ID 9–10  : BGD Ô tô (2 xe)
            //   ID 11–15 : KT Laptop (5 chiếc, 1/người) – KHÔNG server
            //   ID 16–17 : NS Laptop (2 chiếc, 1/người)
            //   ID 18–20 : TECH Laptop (3 chiếc, 1/người)
            //   ID 21–30 : TECH Server (10 chiếc – phòng kỹ thuật quản lý)
            //   ID 31–40 : TECH Network (10 thiết bị mạng)
            //   ID 41–45 : PROD Laptop (5 chiếc, 1/người)
            //   ID 46–65 : DEV Laptop (20 chiếc, 2/người × 10 người)
            //   ID 66–68 : PMO Laptop (3 chiếc, 1/người)
            //   ID 69–74 : DESIGN Laptop (6 chiếc, 2/người × 3 người)
            //   ID 75–84 : Đã thanh lý (10 tài sản cũ)
            //   ID 85–100: Chưa cấp phát (16 tài sản tồn kho)
            // =========================================================

            var taiSans = new List<TaiSan>();
            var chungTus = new List<ChungTu>();
            var chiTiets = new List<ChiTietChungTu>();
            var lichSuKhauHaos = new List<LichSuKhauHao>();
            var thanhLys = new List<ThanhLyTaiSan>();
            var dieuChuyens = new List<DieuChuyenTaiSan>();

            int ctId = 1, chiTietId = 1, khId = 1, dcId = 1;

            // ---- Chứng từ khấu hao tổng hợp tháng 01/2025 ----
            var ctKhauHao = new ChungTu
            {
                Id = ctId++,
                MaChungTu = "CT-KH-2025-01",
                NgayLap = new DateTime(2025, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                LoaiChungTu = LoaiChungTu.KhauHao,
                MoTa = "Trích khấu hao TSCĐ tháng 01/2025",
                TongTien = 0,
                TrangThai = "hoan_thanh",
                NguoiLapId = 6, // Lê Bảo Ngọc – KT TSCĐ
                NgayTao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            // Helper: tạo 1 tài sản laptop với thông tin đầy đủ
            TaiSan MakeLaptop(int id, string tenMay, string serial, decimal gia, int soThang,
                int? userId, int? deptId, DateTime? ngayMua, DateTime? ngayCapPhat,
                TrangThaiTaiSan trangThai = TrangThaiTaiSan.DangSuDung,
                PhuongThucThanhToan phuongThuc = PhuongThucThanhToan.ChuyenKhoan)
            {
                int thoiGian = 36;
                decimal khHangThang = Math.Round(gia / thoiGian, 2);
                decimal khLuyKe = khHangThang * soThang;
                return new TaiSan
                {
                    Id = id,
                    MaTaiSan = $"LAP-{id:D4}",
                    TenTaiSan = tenMay,
                    DanhMucId = 1,
                    TrangThai = trangThai,
                    SoSeri = serial,
                    NhaSanXuat = tenMay.Contains("Dell") ? "Dell" : tenMay.Contains("HP") ? "HP" : tenMay.Contains("MacBook") ? "Apple" : tenMay.Contains("Lenovo") ? "Lenovo" : "Asus",
                    NgayMua = ngayMua ?? new DateTime(2023, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    NguyenGia = gia,
                    KhauHaoLuyKe = khLuyKe,
                    GiaTriConLai = gia - khLuyKe,
                    KhauHaoHangThang = khHangThang,
                    PhuongPhapKhauHao = PhuongPhapKhauHao.DuongThang,
                    ThoiGianKhauHao = thoiGian,
                    MaTaiKhoan = "2114",
                    PhongBanId = deptId,
                    NguoiDungId = userId,
                    NgayCapPhat = ngayCapPhat,
                    PhuongThucThanhToan = phuongThuc,
                    NgayTao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                };
            }

            TaiSan MakeServer(int id, string tenMay, string serial, decimal gia, int soThang,
                int? userId, int deptId, DateTime? ngayMua)
            {
                int thoiGian = 60;
                decimal khHangThang = Math.Round(gia / thoiGian, 2);
                decimal khLuyKe = khHangThang * soThang;
                return new TaiSan
                {
                    Id = id,
                    MaTaiSan = $"SRV-{id:D4}",
                    TenTaiSan = tenMay,
                    DanhMucId = 2,
                    TrangThai = TrangThaiTaiSan.DangSuDung,
                    SoSeri = serial,
                    NhaSanXuat = tenMay.Contains("Dell") ? "Dell" : tenMay.Contains("HP") || tenMay.Contains("HPE") ? "HP" : "Synology",
                    NgayMua = ngayMua ?? new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    NguyenGia = gia,
                    KhauHaoLuyKe = khLuyKe,
                    GiaTriConLai = gia - khLuyKe,
                    KhauHaoHangThang = khHangThang,
                    PhuongPhapKhauHao = PhuongPhapKhauHao.DuongThang,
                    ThoiGianKhauHao = thoiGian,
                    MaTaiKhoan = "2112",
                    PhongBanId = deptId,
                    NguoiDungId = userId,
                    NgayCapPhat = ngayMua,
                    PhuongThucThanhToan = PhuongThucThanhToan.ChuyenKhoan,
                    NgayTao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                };
            }

            TaiSan MakeNetwork(int id, string tenMay, string serial, decimal gia, int soThang, int deptId, DateTime? ngayMua)
            {
                int thoiGian = 36;
                decimal khHangThang = Math.Round(gia / thoiGian, 2);
                decimal khLuyKe = khHangThang * soThang;
                return new TaiSan
                {
                    Id = id,
                    MaTaiSan = $"NET-{id:D4}",
                    TenTaiSan = tenMay,
                    DanhMucId = 4,
                    TrangThai = TrangThaiTaiSan.DangSuDung,
                    SoSeri = serial,
                    NhaSanXuat = tenMay.Contains("Cisco") ? "Cisco" : tenMay.Contains("Fortinet") ? "Fortinet" : "Ubiquiti",
                    NgayMua = ngayMua ?? new DateTime(2022, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    NguyenGia = gia,
                    KhauHaoLuyKe = khLuyKe,
                    GiaTriConLai = gia - khLuyKe,
                    KhauHaoHangThang = khHangThang,
                    PhuongPhapKhauHao = PhuongPhapKhauHao.DuongThang,
                    ThoiGianKhauHao = thoiGian,
                    MaTaiKhoan = "2112",
                    PhongBanId = deptId,
                    NguoiDungId = null,
                    NgayCapPhat = ngayMua,
                    PhuongThucThanhToan = PhuongThucThanhToan.ChuyenKhoan,
                    NgayTao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                };
            }

            // --- BGD LAPTOPS: ID 1–8 (2 laptop/người, 4 người) ---
            var buy2023H1 = new DateTime(2023, 3, 1, 0, 0, 0, DateTimeKind.Utc);
            var buy2023H2 = new DateTime(2023, 9, 1, 0, 0, 0, DateTimeKind.Utc);
            var capPhat2023 = new DateTime(2023, 3, 15, 0, 0, 0, DateTimeKind.Utc);
            // Phạm Quốc Hùng (userId=1, deptId=1)
            taiSans.Add(MakeLaptop(1, "Apple MacBook Pro 16\" M3 Pro (48GB/1TB)", "FVFX3YLHQ05M", 85_000_000, 22, 1, 1, buy2023H1, capPhat2023, phuongThuc: PhuongThucThanhToan.ChuyenKhoan));
            taiSans.Add(MakeLaptop(2, "Apple iPad Pro 12.9\" M2 (WiFi+5G)", "DMPX8GH2TYKV", 32_000_000, 22, 1, 1, buy2023H1, capPhat2023, phuongThuc: PhuongThucThanhToan.ChuyenKhoan));
            // Trần Thanh Tùng (userId=2, deptId=1)
            taiSans.Add(MakeLaptop(3, "Apple MacBook Pro 14\" M3 (16GB/512GB)", "C02ZK1YHLVDQ", 55_000_000, 22, 2, 1, buy2023H1, capPhat2023));
            taiSans.Add(MakeLaptop(4, "Dell Latitude 5430 (i7/16GB/512GB SSD)", "DLLAT5430ABCD", 28_000_000, 22, 2, 1, buy2023H1, capPhat2023));
            // Nguyễn Thị Minh Châu (userId=3, deptId=1)
            taiSans.Add(MakeLaptop(5, "Apple MacBook Air 15\" M2 (8GB/512GB)", "C02ZQ5RTLVDP", 38_000_000, 16, 3, 1, buy2023H2, new DateTime(2023, 9, 15, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeLaptop(6, "HP EliteBook 840 G9 (i5/16GB/512GB)", "HPEB840G9WXYZ", 24_000_000, 16, 3, 1, buy2023H2, new DateTime(2023, 9, 15, 0, 0, 0, DateTimeKind.Utc)));
            // Lê Quang Bình (userId=4, deptId=1) – Thư ký
            taiSans.Add(MakeLaptop(7, "Lenovo ThinkPad E14 Gen 4 (i5/8GB/256GB)", "LTPE14G4001X", 18_000_000, 22, 4, 1, buy2023H1, capPhat2023));
            taiSans.Add(MakeLaptop(8, "Asus VivoBook 15 OLED (i5/16GB/512GB)", "ASVIVO15001Y", 16_000_000, 22, 4, 1, buy2023H1, capPhat2023));

            // --- BGD Ô TÔ: ID 9–10 ---
            var buyOto = new DateTime(2023, 1, 10, 0, 0, 0, DateTimeKind.Utc);
            taiSans.Add(new TaiSan
            {
                Id = 9, MaTaiSan = "OTO-0009", TenTaiSan = "Toyota Innova Crysta 2.0G MT 2023 (7 chỗ)", DanhMucId = 3,
                TrangThai = TrangThaiTaiSan.DangSuDung, SoSeri = "MNBVCXZLKJHG1", NhaSanXuat = "Toyota",
                NgayMua = buyOto, NguyenGia = 980_000_000, KhauHaoLuyKe = Math.Round(980_000_000m / 120 * 24, 2),
                GiaTriConLai = 980_000_000 - Math.Round(980_000_000m / 120 * 24, 2),
                KhauHaoHangThang = Math.Round(980_000_000m / 120, 2),
                PhuongPhapKhauHao = PhuongPhapKhauHao.DuongThang, ThoiGianKhauHao = 120,
                MaTaiKhoan = "2113", PhongBanId = 1, NguoiDungId = 1,
                NgayCapPhat = buyOto, PhuongThucThanhToan = PhuongThucThanhToan.ChuyenKhoan,
                NgayTao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
            taiSans.Add(new TaiSan
            {
                Id = 10, MaTaiSan = "OTO-0010", TenTaiSan = "Ford Ranger Wildtrak 2.0L Bi-Turbo AT 2023 (Pickup)", DanhMucId = 3,
                TrangThai = TrangThaiTaiSan.DangSuDung, SoSeri = "MNBVCXZLKJHG2", NhaSanXuat = "Ford",
                NgayMua = buyOto, NguyenGia = 920_000_000, KhauHaoLuyKe = Math.Round(920_000_000m / 120 * 24, 2),
                GiaTriConLai = 920_000_000 - Math.Round(920_000_000m / 120 * 24, 2),
                KhauHaoHangThang = Math.Round(920_000_000m / 120, 2),
                PhuongPhapKhauHao = PhuongPhapKhauHao.DuongThang, ThoiGianKhauHao = 120,
                MaTaiKhoan = "2113", PhongBanId = 1, NguoiDungId = 2,
                NgayCapPhat = buyOto, PhuongThucThanhToan = PhuongThucThanhToan.ChuyenKhoan,
                NgayTao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });

            // --- KẾ TOÁN LAPTOPS: ID 11–15 (1/người, chỉ laptop – KHÔNG server) ---
            var buyKT = new DateTime(2023, 4, 1, 0, 0, 0, DateTimeKind.Utc);
            var capPhatKT = new DateTime(2023, 4, 10, 0, 0, 0, DateTimeKind.Utc);
            taiSans.Add(MakeLaptop(11, "Dell Latitude 7430 (i7/16GB/512GB, vPro)", "DLLAT7430KT01", 32_000_000, 21, 5, 2, buyKT, capPhatKT));
            taiSans.Add(MakeLaptop(12, "HP EliteBook 840 G9 (i5/16GB/512GB)", "HPEB840KT02", 25_000_000, 21, 6, 2, buyKT, capPhatKT));
            taiSans.Add(MakeLaptop(13, "Lenovo ThinkPad L14 Gen 3 (i5/16GB/512GB)", "LTPL14G3KT03", 22_000_000, 21, 7, 2, buyKT, capPhatKT));
            taiSans.Add(MakeLaptop(14, "Dell Vostro 5620 (i5/8GB/256GB)", "DLVO5620KT04", 18_000_000, 21, 8, 2, buyKT, capPhatKT));
            taiSans.Add(MakeLaptop(15, "HP ProBook 445 G9 (Ryzen 5/8GB/256GB)", "HPPB445G9KT05", 16_000_000, 21, 9, 2, buyKT, capPhatKT));

            // --- NHÂN SỰ LAPTOPS: ID 16–17 ---
            var buyNS = new DateTime(2023, 5, 1, 0, 0, 0, DateTimeKind.Utc);
            taiSans.Add(MakeLaptop(16, "Lenovo IdeaPad 3 Gen 7 (i5/8GB/512GB)", "LTIP3G7NS01", 16_000_000, 20, 10, 3, buyNS, new DateTime(2023, 5, 10, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeLaptop(17, "Asus VivoBook 15 (i3/8GB/512GB)", "ASVIVO15NS02", 13_000_000, 20, 11, 3, buyNS, new DateTime(2023, 5, 10, 0, 0, 0, DateTimeKind.Utc)));

            // --- KỸ THUẬT LAPTOPS: ID 18–20 (1/người) ---
            var buyTECH = new DateTime(2022, 6, 1, 0, 0, 0, DateTimeKind.Utc);
            taiSans.Add(MakeLaptop(18, "Dell Precision 5570 (i7/32GB/1TB, Workstation)", "DLPRE5570TC01", 55_000_000, 31, 12, 4, buyTECH, new DateTime(2022, 6, 15, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeLaptop(19, "Lenovo ThinkPad P15 Gen 2 (i9/32GB/1TB)", "LTPP15G2TC02", 60_000_000, 31, 13, 4, buyTECH, new DateTime(2022, 6, 15, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeLaptop(20, "HP ZBook Fury 15 G8 (i7/16GB/512GB)", "HPZBF15G8TC03", 48_000_000, 31, 14, 4, buyTECH, new DateTime(2022, 6, 15, 0, 0, 0, DateTimeKind.Utc)));

            // --- KỸ THUẬT SERVERS: ID 21–30 ---
            // Các server được quản lý bởi phòng kỹ thuật, user = trưởng phòng hoặc null (tài sản chung)
            taiSans.Add(MakeServer(21, "Dell PowerEdge R750xs (2×Xeon Silver, 128GB RAM, 4×3.5\" HDD 4TB)", "DLSVR750XS001", 280_000_000, 36, 12, 4, new DateTime(2021, 9, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeServer(22, "Dell PowerEdge R750xs (2×Xeon Silver, 128GB RAM, 4×3.5\" HDD 4TB)", "DLSVR750XS002", 280_000_000, 36, 12, 4, new DateTime(2021, 9, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeServer(23, "HPE ProLiant DL360 Gen10 Plus (Xeon Gold, 64GB, 8×SFF)", "HPESVRDL360001", 240_000_000, 36, 13, 4, new DateTime(2021, 10, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeServer(24, "HPE ProLiant DL380 Gen10 (2×Xeon Silver, 128GB, 12×LFF)", "HPESVRDL380001", 320_000_000, 30, 13, 4, new DateTime(2022, 3, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeServer(25, "Synology RackStation RS1221+ (8-Bay NAS, Ryzen)", "SYNORS1221001", 48_000_000, 24, 14, 4, new DateTime(2022, 9, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeServer(26, "Synology RackStation RS3621RPxs (12-Bay NAS, Xeon D)", "SYNORS3621001", 120_000_000, 24, 14, 4, new DateTime(2022, 9, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeServer(27, "Dell PowerEdge T350 (Xeon E-2300, 16GB, Tower Server)", "DLSVRT350001", 95_000_000, 20, 12, 4, new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeServer(28, "HPE ProLiant MicroServer Gen10 Plus v2 (i3, 16GB)", "HPEMSVR001", 42_000_000, 20, 13, 4, new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeServer(29, "Synology DiskStation DS1621+ (6-Bay, Ryzen V1500)", "SYNODS1621001", 36_000_000, 16, 14, 4, new DateTime(2023, 6, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeServer(30, "APC Smart-UPS SRT 3000VA RM (UPS nguồn dự phòng)", "APCSRT3000001", 22_000_000, 16, 12, 4, new DateTime(2023, 6, 1, 0, 0, 0, DateTimeKind.Utc)));

            // --- KỸ THUẬT NETWORK: ID 31–40 ---
            taiSans.Add(MakeNetwork(31, "Cisco Catalyst 9200L-24P-4G (Switch 24-port PoE)", "CSCAT9200001", 45_000_000, 36, 4, new DateTime(2021, 9, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeNetwork(32, "Cisco Catalyst 9200L-48P-4G (Switch 48-port PoE)", "CSCAT9200002", 65_000_000, 36, 4, new DateTime(2021, 9, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeNetwork(33, "Fortinet FortiGate 100F (NGFW, Firewall)", "FTFG100F001", 90_000_000, 36, 4, new DateTime(2021, 9, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeNetwork(34, "Fortinet FortiAP 431F (WiFi 6, Access Point)", "FTFAP431F001", 15_000_000, 30, 4, new DateTime(2022, 3, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeNetwork(35, "Fortinet FortiAP 431F (WiFi 6, Access Point)", "FTFAP431F002", 15_000_000, 30, 4, new DateTime(2022, 3, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeNetwork(36, "Cisco ISR 4321/K9 (Router VPN/WAN)", "CSISR4321001", 55_000_000, 36, 4, new DateTime(2021, 9, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeNetwork(37, "Ubiquiti UniFi AP AC Pro (WiFi AC, PoE)", "UBIUAP001", 3_500_000, 24, 4, new DateTime(2022, 9, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeNetwork(38, "Ubiquiti UniFi AP AC Pro (WiFi AC, PoE)", "UBIUAP002", 3_500_000, 24, 4, new DateTime(2022, 9, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeNetwork(39, "Cisco SG350-28P (Switch 28-port PoE+)", "CSSG350001", 20_000_000, 20, 4, new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeNetwork(40, "Palo Alto PA-440 (Next-Gen Firewall)", "PAALTO440001", 120_000_000, 20, 4, new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)));

            // --- PHÒNG SẢN PHẨM LAPTOPS: ID 41–45 ---
            var buyPROD = new DateTime(2023, 7, 1, 0, 0, 0, DateTimeKind.Utc);
            taiSans.Add(MakeLaptop(41, "Apple MacBook Pro 14\" M3 (16GB/512GB)", "C02ZK1PROD01", 55_000_000, 18, 15, 5, buyPROD, new DateTime(2023, 7, 15, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeLaptop(42, "Apple MacBook Air 13\" M2 (8GB/256GB)", "C02ZQ5PROD02", 29_000_000, 18, 16, 5, buyPROD, new DateTime(2023, 7, 15, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeLaptop(43, "Dell XPS 13 9310 (i7/16GB/512GB)", "DLXPS9310001", 35_000_000, 18, 17, 5, buyPROD, new DateTime(2023, 7, 15, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeLaptop(44, "Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", "LTPX1CG11001", 45_000_000, 18, 18, 5, buyPROD, new DateTime(2023, 7, 15, 0, 0, 0, DateTimeKind.Utc)));
            taiSans.Add(MakeLaptop(45, "Apple MacBook Air 15\" M2 (8GB/512GB)", "C02ZQ5PROD05", 38_000_000, 18, 19, 5, buyPROD, new DateTime(2023, 7, 15, 0, 0, 0, DateTimeKind.Utc)));

            // --- PHÒNG DEV LAPTOPS: ID 46–65 (2/người × 10 người) ---
            var buyDEV1 = new DateTime(2023, 2, 1, 0, 0, 0, DateTimeKind.Utc);
            var buyDEV2 = new DateTime(2023, 8, 1, 0, 0, 0, DateTimeKind.Utc);
            string[] devLaptops1 = {
                "Apple MacBook Pro 16\" M2 Pro (16GB/512GB)", "Apple MacBook Pro 14\" M2 (16GB/512GB)",
                "Dell XPS 15 9520 (i7/32GB/1TB)", "Dell XPS 15 9520 (i7/32GB/1TB)",
                "Lenovo ThinkPad X1 Carbon Gen 10 (i7/16GB/512GB)", "HP Spectre x360 14 (i7/16GB/512GB)",
                "Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)",
                "Dell Precision 5570 (i7/32GB/1TB)", "HP ZBook Studio G9 (i7/32GB/512GB)"
            };
            string[] devLaptops2 = {
                "Apple MacBook Pro 14\" M2 Pro (12GB/512GB)", "Apple MacBook Air 15\" M2 (16GB/512GB)",
                "Dell Latitude 7430 (i7/16GB/512GB)", "Lenovo ThinkPad L15 Gen 3 (i5/16GB/512GB)",
                "HP EliteBook 845 G9 (Ryzen 5/16GB/512GB)", "Asus VivoBook Pro 15 OLED (i7/16GB/512GB)",
                "Lenovo IdeaPad Gaming 3 (i7/16GB/512GB)", "Dell Vostro 5620 (i7/16GB/512GB)",
                "HP EliteBook 830 G9 (i5/8GB/256GB)", "Asus ExpertBook B9 (i7/16GB/1TB)"
            };
            // 10 dev users: 20..29, dept=6
            for (int i = 0; i < 10; i++)
            {
                int uid = 20 + i;
                int assetId1 = 46 + i * 2;
                int assetId2 = 47 + i * 2;
                string sn1 = $"DEV{uid}L1{assetId1:D4}";
                string sn2 = $"DEV{uid}L2{assetId2:D4}";
                decimal gia1 = (i < 5 ? 45_000_000m : 35_000_000m);
                decimal gia2 = (i < 5 ? 28_000_000m : 22_000_000m);
                var buyDate = i < 5 ? buyDEV1 : buyDEV2;
                int soThang = i < 5 ? 23 : 17;
                taiSans.Add(MakeLaptop(assetId1, devLaptops1[i], sn1, gia1, soThang, uid, 6, buyDate, buyDate.AddDays(10)));
                taiSans.Add(MakeLaptop(assetId2, devLaptops2[i], sn2, gia2, soThang, uid, 6, buyDate, buyDate.AddDays(10)));
            }

            // --- PMO LAPTOPS: ID 66–68 ---
            var buyPMO = new DateTime(2023, 5, 15, 0, 0, 0, DateTimeKind.Utc);
            taiSans.Add(MakeLaptop(66, "Apple MacBook Pro 14\" M3 (16GB/512GB)", "C02ZK1PMO01", 55_000_000, 19, 30, 7, buyPMO, buyPMO.AddDays(5)));
            taiSans.Add(MakeLaptop(67, "Dell Latitude 5540 (i7/16GB/512GB)", "DLLAT5540PMO02", 28_000_000, 19, 31, 7, buyPMO, buyPMO.AddDays(5)));
            taiSans.Add(MakeLaptop(68, "Lenovo ThinkPad E14 Gen 5 (i5/16GB/512GB)", "LTPE14G5PMO03", 22_000_000, 19, 32, 7, buyPMO, buyPMO.AddDays(5)));

            // --- DESIGN LAPTOPS: ID 69–74 (2/người × 3 người, laptop cao cấp) ---
            var buyDESIGN = new DateTime(2023, 4, 1, 0, 0, 0, DateTimeKind.Utc);
            taiSans.Add(MakeLaptop(69, "Apple MacBook Pro 16\" M3 Max (36GB/1TB) – Lead Designer", "FVFX3DES001", 95_000_000, 21, 33, 8, buyDESIGN, buyDESIGN.AddDays(7)));
            taiSans.Add(MakeLaptop(70, "Dell Precision 5570 (i9/64GB/2TB, Workstation)", "DLPRE5570DES01", 75_000_000, 21, 33, 8, buyDESIGN, buyDESIGN.AddDays(7)));
            taiSans.Add(MakeLaptop(71, "Apple MacBook Pro 14\" M3 Pro (18GB/512GB)", "FVFX3DES002", 65_000_000, 21, 34, 8, buyDESIGN, buyDESIGN.AddDays(7)));
            taiSans.Add(MakeLaptop(72, "Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", "ASPAS16DES01", 55_000_000, 21, 34, 8, buyDESIGN, buyDESIGN.AddDays(7)));
            taiSans.Add(MakeLaptop(73, "Apple MacBook Pro 14\" M3 (16GB/512GB)", "FVFX3DES003", 55_000_000, 21, 35, 8, buyDESIGN, buyDESIGN.AddDays(7)));
            taiSans.Add(MakeLaptop(74, "HP Spectre x360 14 OLED (i7/16GB/1TB)", "HPSPX360DES01", 42_000_000, 21, 35, 8, buyDESIGN, buyDESIGN.AddDays(7)));

            // --- ĐÃ THANH LÝ: ID 75–84 ---
            // Các tài sản cũ đã hết khấu hao hoặc hỏng, không có user/dept
            string[] oldLapNames = {
                "HP EliteBook 840 G5 (i5/8GB/256GB, cũ)",
                "Dell Latitude 5480 (i5/8GB/256GB, cũ)",
                "Lenovo ThinkPad T470 (i5/8GB/256GB, cũ)",
                "Asus VivoBook 15 (i5/8GB/512GB, cũ)",
                "HP ProBook 430 G5 (i5/8GB/256GB, cũ)",
                "Dell Vostro 3580 (i5/8GB/256GB, cũ)",
                "Cisco Catalyst 2960-X-24PS (switch cũ, hỏng)",
                "D-Link DES-1026G (switch 24-port cũ, hỏng)",
                "Synology DS218j (NAS 2-Bay cũ)",
                "APC Back-UPS 650VA (UPS cũ, hỏng pin)"
            };
            int[] oldDanhmuc = { 1, 1, 1, 1, 1, 1, 4, 4, 2, 2 };
            string[] oldMaTK = { "2114", "2114", "2114", "2114", "2114", "2114", "2112", "2112", "2112", "2112" };
            decimal[] oldGia = { 18_000_000, 16_000_000, 17_000_000, 14_000_000, 15_000_000, 13_000_000, 35_000_000, 12_000_000, 8_000_000, 5_000_000 };
            int[] oldThoiGian = { 36, 36, 36, 36, 36, 36, 36, 36, 60, 36 };

            for (int i = 0; i < 10; i++)
            {
                int tsId = 75 + i;
                decimal gia = oldGia[i];
                int thoiGian = oldThoiGian[i];
                // Đã khấu hao hết (40+ tháng)
                decimal khHangThang = Math.Round(gia / thoiGian, 2);
                decimal khLuyKe = Math.Min(gia, khHangThang * 38);
                decimal conLai = gia - khLuyKe;
                string prefix = oldDanhmuc[i] == 1 ? "LAP" : oldDanhmuc[i] == 2 ? "SRV" : "NET";

                taiSans.Add(new TaiSan
                {
                    Id = tsId,
                    MaTaiSan = $"{prefix}-{tsId:D4}",
                    TenTaiSan = oldLapNames[i],
                    DanhMucId = oldDanhmuc[i],
                    TrangThai = TrangThaiTaiSan.DaThanhLy,
                    SoSeri = $"OLD{tsId:D4}SN",
                    NgayMua = new DateTime(2019, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    NguyenGia = gia,
                    KhauHaoLuyKe = khLuyKe,
                    GiaTriConLai = conLai,
                    KhauHaoHangThang = khHangThang,
                    PhuongPhapKhauHao = PhuongPhapKhauHao.DuongThang,
                    ThoiGianKhauHao = thoiGian,
                    MaTaiKhoan = oldMaTK[i],
                    PhongBanId = null,
                    NguoiDungId = null,
                    NgayCapPhat = null,
                    PhuongThucThanhToan = PhuongThucThanhToan.TienMat,
                    NgayTao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                });

                // Chứng từ ghi tăng (mua ban đầu)
                var ctGhiTang = new ChungTu
                {
                    Id = ctId,
                    MaChungTu = $"CT-GT-{tsId:D3}",
                    NgayLap = new DateTime(2019, 6, 15, 0, 0, 0, DateTimeKind.Utc),
                    LoaiChungTu = LoaiChungTu.GhiTang,
                    MoTa = $"Ghi tăng tài sản {prefix}-{tsId:D4} khi mua",
                    TongTien = gia,
                    TrangThai = "hoan_thanh",
                    NguoiLapId = 6,
                    NgayTao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                };
                chungTus.Add(ctGhiTang);
                chiTiets.Add(new ChiTietChungTu { Id = chiTietId++, ChungTuId = ctId, TaiSanId = tsId, TaiKhoanNo = oldMaTK[i], SoTien = gia, MoTa = "Ghi tăng nguyên giá TSCĐ" });
                chiTiets.Add(new ChiTietChungTu { Id = chiTietId++, ChungTuId = ctId, TaiSanId = tsId, TaiKhoanCo = "112", SoTien = gia, MoTa = "Thanh toán qua ngân hàng" });
                ctId++;

                // Chứng từ thanh lý
                decimal giaThanhLy = Math.Max(0, conLai * 0.1m); // Thanh lý 10% giá trị còn lại
                decimal laiLo = giaThanhLy - conLai;
                var ctThanhLy = new ChungTu
                {
                    Id = ctId,
                    MaChungTu = $"CT-TL-{tsId:D3}",
                    NgayLap = new DateTime(2024, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                    LoaiChungTu = LoaiChungTu.ThanhLy,
                    MoTa = $"Thanh lý tài sản {prefix}-{tsId:D4} – hỏng, hết khấu hao",
                    TongTien = gia,
                    TrangThai = "hoan_thanh",
                    NguoiLapId = 6,
                    NgayTao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                };
                chungTus.Add(ctThanhLy);
                chiTiets.Add(new ChiTietChungTu { Id = chiTietId++, ChungTuId = ctId, TaiSanId = tsId, TaiKhoanCo = oldMaTK[i], SoTien = gia, MoTa = "Xóa sổ nguyên giá TSCĐ" });
                chiTiets.Add(new ChiTietChungTu { Id = chiTietId++, ChungTuId = ctId, TaiSanId = tsId, TaiKhoanNo = "2141", SoTien = khLuyKe, MoTa = "Xóa sổ hao mòn lũy kế" });
                if (giaThanhLy > 0)
                    chiTiets.Add(new ChiTietChungTu { Id = chiTietId++, ChungTuId = ctId, TaiSanId = tsId, TaiKhoanNo = "111", SoTien = giaThanhLy, MoTa = "Thu tiền thanh lý" });
                if (laiLo < 0)
                    chiTiets.Add(new ChiTietChungTu { Id = chiTietId++, ChungTuId = ctId, TaiSanId = tsId, TaiKhoanNo = "811", SoTien = Math.Abs(laiLo), MoTa = "Lỗ thanh lý tài sản" });
                else if (laiLo > 0)
                    chiTiets.Add(new ChiTietChungTu { Id = chiTietId++, ChungTuId = ctId, TaiSanId = tsId, TaiKhoanCo = "711", SoTien = laiLo, MoTa = "Lãi thanh lý tài sản" });
                ctId++;

                thanhLys.Add(new ThanhLyTaiSan
                {
                    Id = tsId,
                    TaiSanId = tsId,
                    NgayThanhLy = new DateTime(2024, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                    NguyenGia = gia,
                    KhauHaoLuyKe = khLuyKe,
                    GiaTriConLai = conLai,
                    GiaTriThanhLy = giaThanhLy,
                    LaiLo = laiLo,
                    LyDo = "Hư hỏng nặng, hết khấu hao, không còn giá trị sử dụng",
                    TrangThai = TrangThaiThanhLy.DaHoanThanh,
                    NgayTao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                });
            }

            // --- CHƯA CẤP PHÁT: ID 85–100 (16 laptop tồn kho) ---
            string[] newLaptops = {
                "Dell Latitude 5540 (i7/16GB/512GB)", "Dell Latitude 5540 (i7/16GB/512GB)",
                "HP EliteBook 840 G10 (i5/16GB/512GB)", "HP EliteBook 840 G10 (i5/16GB/512GB)",
                "Lenovo ThinkPad E16 Gen 1 (i5/16GB/512GB)", "Lenovo ThinkPad E16 Gen 1 (i5/16GB/512GB)",
                "Asus ExpertBook B5 Flip (i7/16GB/512GB)", "Asus ExpertBook B5 Flip (i7/16GB/512GB)",
                "Apple MacBook Air 13\" M3 (8GB/256GB)", "Apple MacBook Air 13\" M3 (8GB/256GB)",
                "Dell XPS 13 Plus 9320 (i7/32GB/1TB)", "Dell XPS 13 Plus 9320 (i7/32GB/1TB)",
                "HP Spectre x360 14 OLED (i7/16GB/512GB)", "HP Spectre x360 14 OLED (i7/16GB/512GB)",
                "Lenovo ThinkPad X1 Carbon Gen 12 (i7/16GB/1TB)", "Lenovo ThinkPad X1 Carbon Gen 12 (i7/16GB/1TB)"
            };
            decimal[] newGia = { 28_000_000, 28_000_000, 26_000_000, 26_000_000, 22_000_000, 22_000_000, 30_000_000, 30_000_000, 29_000_000, 29_000_000, 38_000_000, 38_000_000, 42_000_000, 42_000_000, 48_000_000, 48_000_000 };
            var buyNew = new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc);

            for (int i = 0; i < 16; i++)
            {
                int tsId = 85 + i;
                decimal gia = newGia[i];
                decimal khHangThang = Math.Round(gia / 36, 2);
                taiSans.Add(new TaiSan
                {
                    Id = tsId,
                    MaTaiSan = $"LAP-{tsId:D4}",
                    TenTaiSan = newLaptops[i],
                    DanhMucId = 1,
                    TrangThai = TrangThaiTaiSan.ChuaCapPhat,
                    SoSeri = $"NEW{tsId:D4}SN",
                    NhaSanXuat = newLaptops[i].Contains("Dell") ? "Dell" : newLaptops[i].Contains("HP") ? "HP" : newLaptops[i].Contains("Apple") ? "Apple" : newLaptops[i].Contains("Lenovo") ? "Lenovo" : "Asus",
                    NgayMua = buyNew,
                    NguyenGia = gia,
                    KhauHaoLuyKe = 0,
                    GiaTriConLai = gia,
                    KhauHaoHangThang = khHangThang,
                    PhuongPhapKhauHao = PhuongPhapKhauHao.DuongThang,
                    ThoiGianKhauHao = 36,
                    MaTaiKhoan = "2114",
                    PhongBanId = null,
                    NguoiDungId = null,
                    NgayCapPhat = null,
                    PhuongThucThanhToan = PhuongThucThanhToan.ChuyenKhoan,
                    NgayTao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                });

                // Chứng từ ghi tăng khi nhập kho
                var ctGhiTangNew = new ChungTu
                {
                    Id = ctId,
                    MaChungTu = $"CT-GT-{tsId:D3}",
                    NgayLap = new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                    LoaiChungTu = LoaiChungTu.GhiTang,
                    MoTa = $"Ghi tăng TSCĐ LAP-{tsId:D4} – nhập kho theo HĐ số {1000 + i}",
                    TongTien = gia,
                    TrangThai = "hoan_thanh",
                    NguoiLapId = 6,
                    NgayTao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                };
                chungTus.Add(ctGhiTangNew);
                chiTiets.Add(new ChiTietChungTu { Id = chiTietId++, ChungTuId = ctId, TaiSanId = tsId, TaiKhoanNo = "2114", SoTien = gia, MoTa = $"Ghi tăng nguyên giá {newLaptops[i]}" });
                chiTiets.Add(new ChiTietChungTu { Id = chiTietId++, ChungTuId = ctId, TaiSanId = tsId, TaiKhoanCo = "331", SoTien = gia, MoTa = "Phải trả nhà cung cấp Laptop" });
                ctId++;
            }

            // --- CHỨNG TỪ GHI TĂNG cho tài sản đang dùng (ID 1–74) ---
            // Tạo chứng từ ghi tăng nguyên giá khi mua (đơn giản)
            foreach (var ts in taiSans.Where(t => t.TrangThai == TrangThaiTaiSan.DangSuDung || t.TrangThai == TrangThaiTaiSan.ChuaCapPhat).ToList())
            {
                if (taiSans.Any(t2 => t2.Id == ts.Id && t2.TrangThai == TrangThaiTaiSan.ChuaCapPhat)) continue;
                // Chứng từ ghi tăng
                var ctGT = new ChungTu
                {
                    Id = ctId,
                    MaChungTu = $"CT-GT-{ts.Id:D3}",
                    NgayLap = ts.NgayMua ?? new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    LoaiChungTu = LoaiChungTu.GhiTang,
                    MoTa = $"Ghi tăng tài sản {ts.MaTaiSan} – {ts.TenTaiSan}",
                    TongTien = ts.NguyenGia ?? 0,
                    TrangThai = "hoan_thanh",
                    NguoiLapId = 6,
                    NgayTao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                };
                chungTus.Add(ctGT);
                chiTiets.Add(new ChiTietChungTu { Id = chiTietId++, ChungTuId = ctId, TaiSanId = ts.Id, TaiKhoanNo = ts.MaTaiKhoan, SoTien = ts.NguyenGia, MoTa = "Ghi tăng nguyên giá TSCĐ" });
                chiTiets.Add(new ChiTietChungTu { Id = chiTietId++, ChungTuId = ctId, TaiSanId = ts.Id, TaiKhoanCo = ts.PhuongThucThanhToan == PhuongThucThanhToan.TienMat ? "111" : "112", SoTien = ts.NguyenGia, MoTa = ts.PhuongThucThanhToan == PhuongThucThanhToan.TienMat ? "Thanh toán tiền mặt" : "Thanh toán chuyển khoản" });
                ctId++;

                // Điều chuyển (cấp phát ban đầu)
                if (ts.PhongBanId.HasValue || ts.NguoiDungId.HasValue)
                {
                    dieuChuyens.Add(new DieuChuyenTaiSan
                    {
                        Id = dcId++,
                        TaiSanId = ts.Id,
                        LoaiDieuChuyen = LoaiDieuChuyen.CapPhat,
                        DenPhongBanId = ts.PhongBanId,
                        DenNguoiDungId = ts.NguoiDungId,
                        NgayThucHien = ts.NgayCapPhat,
                        GhiChu = "Cấp phát tài sản theo quyết định phân công",
                        TrangThai = "da_hoan_thanh",
                        NgayTao = ts.NgayCapPhat ?? new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    });
                }

                // Trích khấu hao vào chứng từ tổng hợp tháng 01/2025
                if ((ts.KhauHaoHangThang ?? 0) > 0)
                {
                    ctKhauHao.TongTien += ts.KhauHaoHangThang ?? 0;
                    lichSuKhauHaos.Add(new LichSuKhauHao
                    {
                        Id = khId++,
                        TaiSanId = ts.Id,
                        ChungTuId = ctKhauHao.Id,
                        KyKhauHao = "2025-01",
                        SoTien = ts.KhauHaoHangThang,
                        LuyKeSauKhauHao = ts.KhauHaoLuyKe,
                        ConLaiSauKhauHao = ts.GiaTriConLai,
                        NgayTao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    });
                    chiTiets.Add(new ChiTietChungTu
                    {
                        Id = chiTietId++,
                        ChungTuId = ctKhauHao.Id,
                        TaiSanId = ts.Id,
                        TaiKhoanNo = "642",
                        TaiKhoanCo = "2141",
                        SoTien = ts.KhauHaoHangThang,
                        MoTa = $"Trích khấu hao {ts.MaTaiSan} tháng 01/2025"
                    });
                }
            }

            // Hoàn thiện chứng từ khấu hao
            chungTus.Add(ctKhauHao);

            modelBuilder.Entity<TaiSan>().HasData(taiSans);
            modelBuilder.Entity<ChungTu>().HasData(chungTus);
            modelBuilder.Entity<ChiTietChungTu>().HasData(chiTiets);
            modelBuilder.Entity<LichSuKhauHao>().HasData(lichSuKhauHaos);
            modelBuilder.Entity<ThanhLyTaiSan>().HasData(thanhLys);
            modelBuilder.Entity<DieuChuyenTaiSan>().HasData(dieuChuyens);

            base.OnModelCreating(modelBuilder);
        }
    }
}
