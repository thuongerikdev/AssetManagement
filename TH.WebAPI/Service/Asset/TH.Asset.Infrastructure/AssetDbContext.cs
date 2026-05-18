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
        public DbSet<TaiSanDinhKem> taiSanDinhKems { get; set; }

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
            // STATIC SEED DATA CƠ BẢN
            // =========================================================
            modelBuilder.Entity<CauHinhHeThong>().HasData(
                new CauHinhHeThong { Id = 1, TenCongTy = "Công ty Cổ phần Công nghệ TH", MaSoThue = "0316789456", DiaChi = "Tầng 8, Tòa nhà Sailing Tower, 111A Pasteur, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh", TienToChungTu = "CT", SoBatDauChungTu = 1, TuDongKhauHao = true, DinhDangMaTaiSan = "{DANH_MUC}-{SO_THU_TU}", DoDaiMaTaiSan = 4, PhuongPhapKhauHaoMacDinh = PhuongPhapKhauHao.DuongThang }
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
                new TaiKhoanKeToan { Id = 11, MaTaiKhoan = "214", TenTaiKhoan = "Hao mòn tài sản cố định", LoaiTaiKhoan = "Nguồn vốn" },
                new TaiKhoanKeToan { Id = 12, MaTaiKhoan = "2141", TenTaiKhoan = "Hao mòn TSCĐ hữu hình", LoaiTaiKhoan = "Nguồn vốn", MaTaiKhoanCha = "214" },
                new TaiKhoanKeToan { Id = 16, MaTaiKhoan = "627", TenTaiKhoan = "Chi phí sản xuất chung", LoaiTaiKhoan = "Chi phí" },
                new TaiKhoanKeToan { Id = 17, MaTaiKhoan = "641", TenTaiKhoan = "Chi phí bán hàng", LoaiTaiKhoan = "Chi phí" },
                new TaiKhoanKeToan { Id = 18, MaTaiKhoan = "642", TenTaiKhoan = "Chi phí quản lý doanh nghiệp", LoaiTaiKhoan = "Chi phí" },
                new TaiKhoanKeToan { Id = 19, MaTaiKhoan = "811", TenTaiKhoan = "Chi phí khác (Lỗ thanh lý)", LoaiTaiKhoan = "Chi phí" },
                new TaiKhoanKeToan { Id = 20, MaTaiKhoan = "711", TenTaiKhoan = "Thu nhập khác (Lãi thanh lý)", LoaiTaiKhoan = "Doanh thu" }
            );

            modelBuilder.Entity<PhongBan>().HasData(
                new PhongBan { Id = 1, MaPhongBan = "BGD", TenPhongBan = "Ban giám đốc" },
                new PhongBan { Id = 2, MaPhongBan = "KT", TenPhongBan = "Phòng kế toán" },
                new PhongBan { Id = 3, MaPhongBan = "NS", TenPhongBan = "Phòng nhân sự" },
                new PhongBan { Id = 4, MaPhongBan = "TECH", TenPhongBan = "Phòng kỹ thuật" },
                new PhongBan { Id = 5, MaPhongBan = "PROD", TenPhongBan = "Phòng sản phẩm" },
                new PhongBan { Id = 6, MaPhongBan = "DEV", TenPhongBan = "Phòng phát triển phần mềm" },
                new PhongBan { Id = 7, MaPhongBan = "PMO", TenPhongBan = "Phòng quản lý dự án" },
                new PhongBan { Id = 8, MaPhongBan = "DESIGN", TenPhongBan = "Phòng thiết kế UI/UX" }
            );

            modelBuilder.Entity<DanhMucTaiSan>().HasData(
                new DanhMucTaiSan { Id = 1, MaDanhMuc = "LAP", TenDanhMuc = "Máy tính xách tay (Laptop)", TienTo = "LAP", ThoiGianKhauHao = 36, MaTaiKhoan = "2114" },
                new DanhMucTaiSan { Id = 2, MaDanhMuc = "SRV", TenDanhMuc = "Máy chủ & Thiết bị lưu trữ (NAS)", TienTo = "SRV", ThoiGianKhauHao = 60, MaTaiKhoan = "2112" },
                new DanhMucTaiSan { Id = 3, MaDanhMuc = "OTO", TenDanhMuc = "Phương tiện vận tải (Ô tô)", TienTo = "OTO", ThoiGianKhauHao = 120, MaTaiKhoan = "2113" },
                new DanhMucTaiSan { Id = 4, MaDanhMuc = "NET", TenDanhMuc = "Thiết bị mạng & Bảo mật", TienTo = "NET", ThoiGianKhauHao = 36, MaTaiKhoan = "2112" }
            );

            // =========================================================
            // DYNAMIC SEED DATA (Tài sản, Chứng từ ghi tăng, Phiếu cấp phát)
            // =========================================================
            var taiSans = new List<TaiSan>();
            var chungTus = new List<ChungTu>();
            var chiTiets = new List<ChiTietChungTu>();
            var dieuChuyens = new List<DieuChuyenTaiSan>();

            int tsId = 1, ctId = 1, ctctId = 1, dcId = 1;

            // Hàm tạo ngày Pseudo-random có quy luật trong khoảng 2021-2025
            DateTime GetDate(int index)
            {
                int daysToAdd = (index * 17) % 1800; // Khoảng ~5 năm từ 2021
                return new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddDays(daysToAdd);
            }

            // HÀM HELPER TẠO TÀI SẢN VÀ CÁC CHỨNG TỪ ĐI KÈM
            void CreateAssetFlow(string prefix, string name, string nhaSanXuat, int danhMucId, string tkNo, decimal gia, int? userId, int deptId)
            {
                var date = GetDate(tsId);
                var phuongThuc = (tsId % 3 == 0) ? PhuongThucThanhToan.TienMat : PhuongThucThanhToan.ChuyenKhoan;

                // 1. Tạo Tài sản
                taiSans.Add(new TaiSan
                {
                    Id = tsId,
                    MaTaiSan = $"{prefix}-{tsId:D4}",
                    TenTaiSan = name,
                    DanhMucId = danhMucId,
                    TrangThai = TrangThaiTaiSan.DangSuDung,
                    SoSeri = $"SN-{prefix}-{tsId:D6}",
                    NhaSanXuat = nhaSanXuat,
                    NgayMua = date.AddDays(-5),
                    NguyenGia = gia,
                    KhauHaoLuyKe = 0,
                    GiaTriConLai = gia,
                    KhauHaoHangThang = Math.Round(gia / (danhMucId == 3 ? 120 : (danhMucId == 2 ? 60 : 36)), 2),
                    PhuongPhapKhauHao = PhuongPhapKhauHao.DuongThang,
                    ThoiGianKhauHao = danhMucId == 3 ? 120 : (danhMucId == 2 ? 60 : 36),
                    MaTaiKhoan = tkNo,
                    PhongBanId = deptId,
                    NguoiDungId = userId,
                    NgayCapPhat = date,
                    PhuongThucThanhToan = phuongThuc,
                    NgayTao = date.AddDays(-5)
                });

                // 2. Tạo chứng từ Ghi tăng
                chungTus.Add(new ChungTu
                {
                    Id = ctId,
                    MaChungTu = $"CT-GT-{tsId:D4}",
                    NgayLap = date.AddDays(-5),
                    LoaiChungTu = LoaiChungTu.GhiTang,
                    MoTa = $"Ghi tăng {name}",
                    TongTien = gia,
                    TrangThai = "hoan_thanh",
                    NguoiLapId = 6, // User Kế toán
                    NgayTao = date.AddDays(-5)
                });

                // 3. Chi tiết chứng từ (Xử lý Tài khoản CÓ)
                string tkCo = phuongThuc == PhuongThucThanhToan.TienMat ? "111" : "112";
                chiTiets.Add(new ChiTietChungTu
                {
                    Id = ctctId++,
                    ChungTuId = ctId,
                    TaiSanId = tsId,
                    TaiKhoanNo = tkNo,
                    TaiKhoanCo = tkCo,
                    SoTien = gia,
                    MoTa = $"Thanh toán & Ghi tăng nguyên giá TSCĐ"
                });

                // 4. Phiếu cấp phát
                dieuChuyens.Add(new DieuChuyenTaiSan
                {
                    Id = dcId++,
                    TaiSanId = tsId,
                    LoaiDieuChuyen = LoaiDieuChuyen.CapPhat,
                    NgayThucHien = date,
                    DenPhongBanId = deptId,
                    DenNguoiDungId = userId,
                    TrangThai = "da_hoan_thanh",
                    GhiChu = "Cấp phát tài sản để bắt đầu sử dụng",
                    NgayTao = date
                });

                tsId++;
                ctId++;
            }

            // --- TỪ ĐIỂN TÊN TÀI SẢN THỰC TẾ (ĐẢM BẢO > 30 TRIỆU ĐỒNG) ---
            var lapNames = new[] {
                ("Apple MacBook Pro 14\" M3 (16GB/512GB)", "Apple", 55_000_000m),
                ("Dell Latitude 7430 (i7/16GB/512GB, vPro)", "Dell", 32_000_000m),
                ("HP EliteBook 845 G10 (Ryzen 9/32GB/1TB)", "HP", 35_000_000m),
                ("Lenovo ThinkPad T14s Gen 3 (Ryzen 7/16GB/512GB)", "Lenovo", 35_000_000m),
                ("Asus ProArt Studiobook 16 OLED (i9/32GB/1TB)", "Asus", 55_000_000m),
                ("Apple MacBook Air 15\" M2 (16GB/512GB)", "Apple", 38_000_000m),
                ("Dell Precision 5570 (i7/32GB/1TB, Workstation)", "Dell", 55_000_000m),
                ("Lenovo ThinkPad X1 Carbon Gen 11 (i7/16GB/1TB)", "Lenovo", 45_000_000m),
                ("HP ZBook Firefly 14 G10 (i7/32GB/1TB)", "HP", 42_000_000m),
                ("Dell XPS 13 9310 (i7/16GB/512GB)", "Dell", 35_000_000m)
            };

            var carNames = new[] {
                ("Toyota Innova Crysta 2.0G MT 2023 (7 chỗ)", "Toyota", 980_000_000m),
                ("Ford Ranger Wildtrak 2.0L Bi-Turbo AT 2023 (Pickup)", "Ford", 920_000_000m),
                ("Mercedes-Benz GLC 300 4MATIC 2023", "Mercedes-Benz", 2_299_000_000m),
                ("VinFast VF9 Plus 2023", "VinFast", 1_500_000_000m)
            };

            var netNames = new[] {
                ("Cisco Catalyst 9200L-24P-4G (Switch 24-port PoE)", "Cisco", 45_000_000m),
                ("Fortinet FortiGate 100F (NGFW, Firewall)", "Fortinet", 90_000_000m),
                ("Cisco Meraki MS120-48LP (Cloud Managed Switch)", "Cisco", 35_000_000m),
                ("Palo Alto PA-440 (Next-Gen Firewall)", "Palo Alto", 120_000_000m)
            };

            // --- 1. 99 LAPTOP CHO 99 USER ---
            for (int i = 1; i <= 99; i++)
            {
                var (name, manufacturer, price) = lapNames[i % lapNames.Length];
                int deptId = (i % 8) + 1; // Phân bổ đều vào 8 phòng ban
                CreateAssetFlow("LAP", name, manufacturer, 1, "2114", price, i, deptId);
            }

            // --- 2. 4 Ô TÔ CHO CÁC SẾP LỚN (Phòng 1, User 1->4) ---
            for (int i = 0; i < 4; i++)
            {
                var (name, manufacturer, price) = carNames[i];
                CreateAssetFlow("OTO", name, manufacturer, 3, "2113", price, i + 1, 1);
            }

            // --- 3. 1 SERVER (Phòng 4 Kỹ Thuật, Cấp phát cho Trưởng phòng - User 12) ---
            CreateAssetFlow("SRV", "Dell PowerEdge R750xs (2×Xeon Silver, 128GB RAM, 4×3.5\" HDD 4TB)", "Dell", 2, "2112", 280_000_000m, 12, 4);

            // --- 4. 4 THIẾT BỊ MẠNG (Phòng 4 Kỹ Thuật, Cấp phát cho Trưởng phòng - User 12) ---
            for (int i = 0; i < 4; i++)
            {
                var (name, manufacturer, price) = netNames[i];
                CreateAssetFlow("NET", name, manufacturer, 4, "2112", price, 12, 4);
            }

            // =========================================================
            // APPLY TO MODEL BUILDER
            // =========================================================
            modelBuilder.Entity<TaiSan>().HasData(taiSans);
            modelBuilder.Entity<ChungTu>().HasData(chungTus);
            modelBuilder.Entity<ChiTietChungTu>().HasData(chiTiets);
            modelBuilder.Entity<DieuChuyenTaiSan>().HasData(dieuChuyens);

            base.OnModelCreating(modelBuilder);
        }
    }
}