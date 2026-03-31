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
            // ====== ASSET CORE CONFIGURATION ======
            modelBuilder.Entity<PhongBan>().HasIndex(x => x.MaPhongBan).IsUnique();
            modelBuilder.Entity<TaiKhoanKeToan>().HasIndex(x => x.MaTaiKhoan).IsUnique();
            modelBuilder.Entity<DanhMucTaiSan>().HasIndex(x => x.MaDanhMuc).IsUnique();
            modelBuilder.Entity<LoTaiSan>().HasIndex(x => x.MaLo).IsUnique();
            modelBuilder.Entity<TaiSan>().HasIndex(x => x.MaTaiSan).IsUnique();
            modelBuilder.Entity<ChungTu>().HasIndex(x => x.MaChungTu).IsUnique();

            // Xử lý các liên kết bằng MÃ TÀI KHOẢN (string)
            modelBuilder.Entity<TaiKhoanKeToan>().HasOne(t => t.TaiKhoanCha).WithMany().HasForeignKey(t => t.MaTaiKhoanCha).HasPrincipalKey(t => t.MaTaiKhoan).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DanhMucTaiSan>().HasOne(d => d.TaiKhoanKeToan).WithMany().HasForeignKey(d => d.MaTaiKhoan).HasPrincipalKey(t => t.MaTaiKhoan).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TaiSan>().HasOne(t => t.TaiKhoanKeToan).WithMany().HasForeignKey(t => t.MaTaiKhoan).HasPrincipalKey(t => t.MaTaiKhoan).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ChiTietChungTu>().HasOne(c => c.TaiKhoanKeToanNo).WithMany().HasForeignKey(c => c.TaiKhoanNo).HasPrincipalKey(t => t.MaTaiKhoan).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ChiTietChungTu>().HasOne(c => c.TaiKhoanKeToanCo).WithMany().HasForeignKey(c => c.TaiKhoanCo).HasPrincipalKey(t => t.MaTaiKhoan).OnDelete(DeleteBehavior.Restrict);

            // Các liên kết bằng Id bình thường
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

            // Convert Enums
            modelBuilder.Entity<TaiSan>().Property(x => x.TrangThai).HasConversion<string>();
            modelBuilder.Entity<TaiSan>().Property(x => x.PhuongPhapKhauHao).HasConversion<string>();
            modelBuilder.Entity<CauHinhHeThong>().Property(x => x.PhuongPhapKhauHaoMacDinh).HasConversion<string>();
            modelBuilder.Entity<DieuChuyenTaiSan>().Property(x => x.LoaiDieuChuyen).HasConversion<string>();
            modelBuilder.Entity<BaoTriTaiSan>().Property(x => x.TrangThai).HasConversion<string>();
            modelBuilder.Entity<ThanhLyTaiSan>().Property(x => x.TrangThai).HasConversion<string>();
            modelBuilder.Entity<ChungTu>().Property(x => x.LoaiChungTu).HasConversion<string>();

            var utcDateTimeConverter = new ValueConverter<DateTime, DateTime>(v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            var utcNullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(v => v == null ? (DateTime?)null : (v.Value.Kind == DateTimeKind.Utc ? v.Value : v.Value.ToUniversalTime()), v => v == null ? null : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var prop in entity.GetProperties())
                {
                    if (prop.ClrType == typeof(DateTime)) prop.SetValueConverter(utcDateTimeConverter);
                    if (prop.ClrType == typeof(DateTime?)) prop.SetValueConverter(utcNullableDateTimeConverter);
                }
            }

            // =========================================================
            // ====== STATIC SEED DATA ======
            // =========================================================

            modelBuilder.Entity<CauHinhHeThong>().HasData(
                new CauHinhHeThong { Id = 1, TenCongTy = "Công ty TNHH ABC", TienToChungTu = "CT", SoBatDauChungTu = 1, TuDongKhauHao = true, DinhDangMaTaiSan = "{DANH_MUC}-{SO_THU_TU}", DoDaiMaTaiSan = 4, PhuongPhapKhauHaoMacDinh = PhuongPhapKhauHao.DuongThang }
            );

            modelBuilder.Entity<TaiKhoanKeToan>().HasData(
                new TaiKhoanKeToan { Id = 1, MaTaiKhoan = "111", TenTaiKhoan = "Tiền mặt", LoaiTaiKhoan = "Tài sản" },
                new TaiKhoanKeToan { Id = 2, MaTaiKhoan = "112", TenTaiKhoan = "Tiền gửi ngân hàng", LoaiTaiKhoan = "Tài sản" },
                new TaiKhoanKeToan { Id = 3, MaTaiKhoan = "331", TenTaiKhoan = "Phải trả cho người bán", LoaiTaiKhoan = "Nguồn vốn" },
                new TaiKhoanKeToan { Id = 4, MaTaiKhoan = "211", TenTaiKhoan = "Tài sản cố định hữu hình", LoaiTaiKhoan = "Tài sản" },
                new TaiKhoanKeToan { Id = 5, MaTaiKhoan = "2111", TenTaiKhoan = "Nhà cửa, vật kiến trúc", LoaiTaiKhoan = "Tài sản", MaTaiKhoanCha = "211" },
                new TaiKhoanKeToan { Id = 6, MaTaiKhoan = "2112", TenTaiKhoan = "Máy móc, thiết bị", LoaiTaiKhoan = "Tài sản", MaTaiKhoanCha = "211" },
                new TaiKhoanKeToan { Id = 7, MaTaiKhoan = "2113", TenTaiKhoan = "Phương tiện vận tải, truyền dẫn", LoaiTaiKhoan = "Tài sản", MaTaiKhoanCha = "211" },
                new TaiKhoanKeToan { Id = 8, MaTaiKhoan = "2114", TenTaiKhoan = "Thiết bị, dụng cụ quản lý", LoaiTaiKhoan = "Tài sản", MaTaiKhoanCha = "211" },
                new TaiKhoanKeToan { Id = 9, MaTaiKhoan = "2115", TenTaiKhoan = "Cây lâu năm, súc vật", LoaiTaiKhoan = "Tài sản", MaTaiKhoanCha = "211" },
                new TaiKhoanKeToan { Id = 11, MaTaiKhoan = "214", TenTaiKhoan = "Hao mòn tài sản cố định", LoaiTaiKhoan = "Nguồn vốn" },
                new TaiKhoanKeToan { Id = 12, MaTaiKhoan = "2141", TenTaiKhoan = "Hao mòn TSCĐ hữu hình", LoaiTaiKhoan = "Nguồn vốn", MaTaiKhoanCha = "214" },
                new TaiKhoanKeToan { Id = 16, MaTaiKhoan = "627", TenTaiKhoan = "Chi phí sản xuất chung", LoaiTaiKhoan = "Chi phí" },
                new TaiKhoanKeToan { Id = 17, MaTaiKhoan = "641", TenTaiKhoan = "Chi phí bán hàng", LoaiTaiKhoan = "Chi phí" },
                new TaiKhoanKeToan { Id = 18, MaTaiKhoan = "642", TenTaiKhoan = "Chi phí quản lý doanh nghiệp", LoaiTaiKhoan = "Chi phí" },
                new TaiKhoanKeToan { Id = 19, MaTaiKhoan = "811", TenTaiKhoan = "Chi phí khác (Lỗ thanh lý)", LoaiTaiKhoan = "Chi phí" },
                new TaiKhoanKeToan { Id = 20, MaTaiKhoan = "711", TenTaiKhoan = "Thu nhập khác (Lãi thanh lý)", LoaiTaiKhoan = "Doanh thu" }
            );

            var phongBans = new List<PhongBan>
            {
                new PhongBan { Id = 1, MaPhongBan = "BGD", TenPhongBan = "Ban giám đốc" },
                new PhongBan { Id = 2, MaPhongBan = "KT", TenPhongBan = "Phòng kế toán" },
                new PhongBan { Id = 3, MaPhongBan = "NS", TenPhongBan = "Phòng nhân sự" },
                new PhongBan { Id = 4, MaPhongBan = "TECH", TenPhongBan = "Phòng kĩ thuật" },
                new PhongBan { Id = 5, MaPhongBan = "PROD", TenPhongBan = "Phòng sản phẩm" },
                new PhongBan { Id = 6, MaPhongBan = "DEV", TenPhongBan = "Phòng phát triển phần mềm" },
                new PhongBan { Id = 7, MaPhongBan = "PMO", TenPhongBan = "Phòng quản lý dự án" },
                new PhongBan { Id = 8, MaPhongBan = "DESIGN", TenPhongBan = "Phòng thiết kế UI/UX" }
            };
            modelBuilder.Entity<PhongBan>().HasData(phongBans);

            var danhMucs = new List<DanhMucTaiSan>
            {
                new DanhMucTaiSan { Id = 1, MaDanhMuc = "LAP", TenDanhMuc = "Máy tính xách tay (Laptop)", TienTo = "LAP", ThoiGianKhauHao = 36, MaTaiKhoan = "2114" },
                new DanhMucTaiSan { Id = 2, MaDanhMuc = "SRV", TenDanhMuc = "Hệ thống Server & NAS", TienTo = "SRV", ThoiGianKhauHao = 60, MaTaiKhoan = "2112" },
                new DanhMucTaiSan { Id = 3, MaDanhMuc = "OTO", TenDanhMuc = "Phương tiện vận tải (Ô tô)", TienTo = "OTO", ThoiGianKhauHao = 120, MaTaiKhoan = "2113" },
                new DanhMucTaiSan { Id = 4, MaDanhMuc = "NET", TenDanhMuc = "Thiết bị mạng (Switch, Router)", TienTo = "NET", ThoiGianKhauHao = 36, MaTaiKhoan = "2112" }
            };
            modelBuilder.Entity<DanhMucTaiSan>().HasData(danhMucs);

            // =========================================================
            // ====== AUTO GENERATED DATA (Liên kết Logic Toàn diện) ======
            // =========================================================

            var random = new Random(12345);

            var taiSans = new List<TaiSan>();
            var chungTus = new List<ChungTu>();
            var chiTietChungTus = new List<ChiTietChungTu>();
            var lichSuKhauHaos = new List<LichSuKhauHao>();
            var thanhLys = new List<ThanhLyTaiSan>();
            var dieuChuyens = new List<DieuChuyenTaiSan>(); // Đã thêm List Điều Chuyển

            string[] laptopNames = { "MacBook Pro 16", "Dell Precision 5570", "Lenovo ThinkPad P1", "Asus ROG" };
            string[] serverNames = { "Dell PowerEdge R740", "HP ProLiant DL380", "Synology NAS 8-Bay", "Cisco Catalyst" };

            int chungTuIdCounter = 1;
            int chiTietIdCounter = 1;
            int khauHaoIdCounter = 1;
            int dieuChuyenIdCounter = 1;

            var ctKhauHaoTong = new ChungTu
            {
                Id = chungTuIdCounter++,
                MaChungTu = "KH-2025-01",
                NgayLap = new DateTime(2025, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                LoaiChungTu = LoaiChungTu.KhauHao,
                MoTa = "Khấu hao tài sản cố định tháng 01/2025",
                TongTien = 0,
                TrangThai = "hoan_thanh",
                NgayTao = DateTime.UtcNow
            };

            for (int i = 1; i <= 300; i++)
            {
                int danhMucId = random.Next(1, 5);
                var danhMuc = danhMucs.First(d => d.Id == danhMucId);
                string tenTaiSan = (danhMucId == 1) ? laptopNames[random.Next(laptopNames.Length)] : serverNames[random.Next(serverNames.Length)];
                if (danhMucId == 3) tenTaiSan = "Xe Ô tô VinFast VF8";

                decimal nguyenGia = random.Next(30, 251) * 1000000m;
                int thoiGianKhauHao = danhMuc.ThoiGianKhauHao ?? 36;
                decimal khauHaoHangThang = Math.Round(nguyenGia / thoiGianKhauHao, 2);
                int soThangDaSuDung = random.Next(5, 20);
                decimal khauHaoLuyKe = khauHaoHangThang * soThangDaSuDung;
                decimal giaTriConLai = nguyenGia - khauHaoLuyKe;

                TrangThaiTaiSan trangThai;
                int? nguoiDungId = random.Next(1, 50);
                int? phongBanId = random.Next(1, 9);
                DateTime? ngayCapPhat = new DateTime(2024, random.Next(1, 12), random.Next(1, 28), 0, 0, 0, DateTimeKind.Utc);

                if (i <= 30)
                {
                    trangThai = TrangThaiTaiSan.DaThanhLy;
                    nguoiDungId = null; // Hiện tại không ai giữ
                    phongBanId = null;
                }
                else if (i <= 250)
                {
                    trangThai = TrangThaiTaiSan.DangSuDung;
                }
                else
                {
                    trangThai = TrangThaiTaiSan.ChuaCapPhat;
                    nguoiDungId = null;
                    phongBanId = null;
                    ngayCapPhat = null;
                }

                var ts = new TaiSan
                {
                    Id = i,
                    MaTaiSan = $"{danhMuc.TienTo}-{i:D4}",
                    TenTaiSan = $"{tenTaiSan} (No.{i})",
                    DanhMucId = danhMucId,
                    TrangThai = trangThai,
                    SoSeri = $"SN{random.Next(10000, 99999)}",
                    NgayMua = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    NguyenGia = nguyenGia,
                    GiaTriConLai = giaTriConLai,
                    KhauHaoLuyKe = khauHaoLuyKe,
                    KhauHaoHangThang = khauHaoHangThang,
                    PhuongPhapKhauHao = PhuongPhapKhauHao.DuongThang,
                    ThoiGianKhauHao = thoiGianKhauHao,
                    MaTaiKhoan = danhMuc.MaTaiKhoan,
                    PhongBanId = phongBanId,
                    NguoiDungId = nguoiDungId,
                    NgayCapPhat = ngayCapPhat,
                    NgayTao = DateTime.UtcNow
                };
                taiSans.Add(ts);

                // --- XỬ LÝ LOGIC CHỨNG TỪ & LỊCH SỬ DỰA TRÊN TRẠNG THÁI ---

                if (trangThai == TrangThaiTaiSan.DaThanhLy)
                {
                    int nguoiDungCu = random.Next(1, 50);
                    int phongBanCu = random.Next(1, 9);
                    DateTime ngayThuHoi = new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc);

                    // Sinh 1 phiếu Cấp phát trong quá khứ
                    dieuChuyens.Add(new DieuChuyenTaiSan
                    {
                        Id = dieuChuyenIdCounter++,
                        TaiSanId = i,
                        LoaiDieuChuyen = LoaiDieuChuyen.CapPhat,
                        DenPhongBanId = phongBanCu,
                        DenNguoiDungId = nguoiDungCu,
                        NgayThucHien = ngayCapPhat.Value,
                        GhiChu = "Cấp phát ban đầu",
                        TrangThai = "da_hoan_thanh",
                        NgayTao = ngayCapPhat.Value
                    });

                    // Sinh 1 phiếu Thu hồi trước khi thanh lý
                    dieuChuyens.Add(new DieuChuyenTaiSan
                    {
                        Id = dieuChuyenIdCounter++,
                        TaiSanId = i,
                        LoaiDieuChuyen = LoaiDieuChuyen.ThuHoi,
                        TuPhongBanId = phongBanCu,
                        TuNguoiDungId = nguoiDungCu,
                        NgayThucHien = ngayThuHoi,
                        GhiChu = "Thu hồi tài sản hỏng chờ thanh lý",
                        TrangThai = "da_hoan_thanh",
                        NgayTao = ngayThuHoi
                    });

                    // Sinh Phiếu Thanh Lý & Chứng từ
                    decimal giaTriThanhLy = random.Next(2, 15) * 1000000m;
                    decimal laiLo = giaTriThanhLy - giaTriConLai;

                    thanhLys.Add(new ThanhLyTaiSan
                    {
                        Id = i,
                        TaiSanId = i,
                        NgayThanhLy = new DateTime(2025, 2, 15, 0, 0, 0, DateTimeKind.Utc),
                        NguyenGia = nguyenGia,
                        KhauHaoLuyKe = khauHaoLuyKe,
                        GiaTriConLai = giaTriConLai,
                        GiaTriThanhLy = giaTriThanhLy,
                        LaiLo = laiLo,
                        LyDo = "Hư hỏng không thể sửa chữa",
                        TrangThai = TrangThaiThanhLy.DaHoanThanh,
                        NgayTao = DateTime.UtcNow
                    });

                    var ctThanhLy = new ChungTu
                    {
                        Id = chungTuIdCounter++,
                        MaChungTu = $"TL-2025-{i:D3}",
                        NgayLap = new DateTime(2025, 2, 15, 0, 0, 0, DateTimeKind.Utc),
                        LoaiChungTu = LoaiChungTu.ThanhLy,
                        MoTa = $"Thanh lý tài sản {ts.MaTaiSan}",
                        TongTien = nguyenGia,
                        TrangThai = "hoan_thanh",
                        NgayTao = DateTime.UtcNow
                    };
                    chungTus.Add(ctThanhLy);

                    chiTietChungTus.Add(new ChiTietChungTu { Id = chiTietIdCounter++, ChungTuId = ctThanhLy.Id, TaiSanId = ts.Id, TaiKhoanCo = ts.MaTaiKhoan, SoTien = nguyenGia, MoTa = "Giảm nguyên giá TSCĐ" });
                    chiTietChungTus.Add(new ChiTietChungTu { Id = chiTietIdCounter++, ChungTuId = ctThanhLy.Id, TaiSanId = ts.Id, TaiKhoanNo = "2141", SoTien = khauHaoLuyKe, MoTa = "Xóa sổ hao mòn lũy kế" });
                    chiTietChungTus.Add(new ChiTietChungTu { Id = chiTietIdCounter++, ChungTuId = ctThanhLy.Id, TaiSanId = ts.Id, TaiKhoanNo = "111", SoTien = giaTriThanhLy, MoTa = "Thu tiền thanh lý bằng tiền mặt" });

                    if (laiLo >= 0) chiTietChungTus.Add(new ChiTietChungTu { Id = chiTietIdCounter++, ChungTuId = ctThanhLy.Id, TaiSanId = ts.Id, TaiKhoanCo = "711", SoTien = laiLo, MoTa = "Lãi thanh lý" });
                    else chiTietChungTus.Add(new ChiTietChungTu { Id = chiTietIdCounter++, ChungTuId = ctThanhLy.Id, TaiSanId = ts.Id, TaiKhoanNo = "811", SoTien = Math.Abs(laiLo), MoTa = "Lỗ thanh lý" });
                }
                else if (trangThai == TrangThaiTaiSan.DangSuDung)
                {
                    // 1. Sinh Phiếu Cấp phát Khớp chính xác với người dùng/phòng ban hiện tại
                    dieuChuyens.Add(new DieuChuyenTaiSan
                    {
                        Id = dieuChuyenIdCounter++,
                        TaiSanId = i,
                        LoaiDieuChuyen = LoaiDieuChuyen.CapPhat,
                        DenPhongBanId = phongBanId,
                        DenNguoiDungId = nguoiDungId,
                        NgayThucHien = ngayCapPhat.Value,
                        GhiChu = "Cấp phát tài sản cho nhân sự",
                        TrangThai = "da_hoan_thanh",
                        NgayTao = ngayCapPhat.Value
                    });

                    // Giả lập thỉnh thoảng có Luân chuyển để data đẹp hơn
                    if (i % 5 == 0)
                    {
                        int tuPhongBanId = random.Next(1, 9);
                        int tuNguoiDungId = random.Next(1, 50);
                        DateTime ngayLuanChuyen = ngayCapPhat.Value.AddDays(40);

                        // Sửa lại phiếu cấp phát ban đầu thành của người cũ
                        var capPhatCu = dieuChuyens.Last();
                        capPhatCu.DenPhongBanId = tuPhongBanId;
                        capPhatCu.DenNguoiDungId = tuNguoiDungId;

                        // Tạo phiếu luân chuyển đến người hiện tại (khớp với bảng TaiSan)
                        dieuChuyens.Add(new DieuChuyenTaiSan
                        {
                            Id = dieuChuyenIdCounter++,
                            TaiSanId = i,
                            LoaiDieuChuyen = LoaiDieuChuyen.LuanChuyen,
                            TuPhongBanId = tuPhongBanId,
                            TuNguoiDungId = tuNguoiDungId,
                            DenPhongBanId = phongBanId,
                            DenNguoiDungId = nguoiDungId,
                            NgayThucHien = ngayLuanChuyen,
                            GhiChu = "Luân chuyển do đổi dự án",
                            TrangThai = "da_hoan_thanh",
                            NgayTao = ngayLuanChuyen
                        });
                        ts.NgayCapPhat = ngayLuanChuyen; // Update lại tài sản
                    }

                    // Khấu hao ...
                    ctKhauHaoTong.TongTien += khauHaoHangThang;
                    lichSuKhauHaos.Add(new LichSuKhauHao
                    {
                        Id = khauHaoIdCounter++,
                        TaiSanId = ts.Id,
                        ChungTuId = ctKhauHaoTong.Id,
                        KyKhauHao = "2025-01",
                        SoTien = khauHaoHangThang,
                        LuyKeSauKhauHao = khauHaoLuyKe,
                        ConLaiSauKhauHao = giaTriConLai,
                        NgayTao = DateTime.UtcNow
                    });
                    chiTietChungTus.Add(new ChiTietChungTu
                    {
                        Id = chiTietIdCounter++,
                        ChungTuId = ctKhauHaoTong.Id,
                        TaiSanId = ts.Id,
                        TaiKhoanNo = "642",
                        TaiKhoanCo = "2141",
                        SoTien = khauHaoHangThang,
                        MoTa = $"Trích khấu hao {ts.MaTaiSan}"
                    });
                }
            }

            chungTus.Add(ctKhauHaoTong);

            modelBuilder.Entity<TaiSan>().HasData(taiSans);
            modelBuilder.Entity<ThanhLyTaiSan>().HasData(thanhLys);
            modelBuilder.Entity<ChungTu>().HasData(chungTus);
            modelBuilder.Entity<ChiTietChungTu>().HasData(chiTietChungTus);
            modelBuilder.Entity<LichSuKhauHao>().HasData(lichSuKhauHaos);
            modelBuilder.Entity<DieuChuyenTaiSan>().HasData(dieuChuyens); // <--- ĐÃ ADD ĐIỀU CHUYỂN

            base.OnModelCreating(modelBuilder);
        }
    }
}