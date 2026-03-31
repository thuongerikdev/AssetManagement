using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;
using TH.Asset.Domain.Entities;
using TH.Asset.Domain.Enums;

namespace TH.Asset.Infrastructure.Database
{
    public class AssetDbContext : DbContext
    {
        // Khai báo DbSet theo kiểu camelCase giống file mẫu
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

            // 1. Cấu hình Index & Ràng buộc Unique
            modelBuilder.Entity<PhongBan>().HasIndex(x => x.MaPhongBan).IsUnique();
            modelBuilder.Entity<TaiKhoanKeToan>().HasIndex(x => x.MaTaiKhoan).IsUnique();
            modelBuilder.Entity<DanhMucTaiSan>().HasIndex(x => x.MaDanhMuc).IsUnique();
            modelBuilder.Entity<LoTaiSan>().HasIndex(x => x.MaLo).IsUnique();
            modelBuilder.Entity<TaiSan>().HasIndex(x => x.MaTaiSan).IsUnique();
            modelBuilder.Entity<ChungTu>().HasIndex(x => x.MaChungTu).IsUnique();

            // 2. Cấu hình Quan hệ (Relationships)
            modelBuilder.Entity<TaiKhoanKeToan>()
                .HasOne(t => t.TaiKhoanCha).WithMany().HasForeignKey(t => t.MaTaiKhoanCha).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DanhMucTaiSan>()
                .HasOne(d => d.TaiKhoanKeToan).WithMany().HasForeignKey(d => d.MaTaiKhoan).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaiSan>()
                .HasOne(t => t.DanhMucTaiSan).WithMany(d => d.TaiSans).HasForeignKey(t => t.DanhMucId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TaiSan>()
                .HasOne(t => t.LoTaiSan).WithMany(l => l.TaiSans).HasForeignKey(t => t.LoId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TaiSan>()
                .HasOne(t => t.PhongBan).WithMany(p => p.TaiSans).HasForeignKey(t => t.PhongBanId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TaiSan>()
                .HasOne(t => t.TaiKhoanKeToan).WithMany().HasForeignKey(t => t.MaTaiKhoan).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DieuChuyenTaiSan>()
                .HasOne(d => d.TaiSan).WithMany(t => t.DieuChuyenTaiSans).HasForeignKey(d => d.TaiSanId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DieuChuyenTaiSan>()
                .HasOne(d => d.TuPhongBan).WithMany().HasForeignKey(d => d.TuPhongBanId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DieuChuyenTaiSan>()
                .HasOne(d => d.DenPhongBan).WithMany().HasForeignKey(d => d.DenPhongBanId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BaoTriTaiSan>()
                .HasOne(b => b.TaiSan).WithMany(t => t.BaoTriTaiSans).HasForeignKey(b => b.TaiSanId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ThanhLyTaiSan>()
                .HasOne(th => th.TaiSan).WithMany(t => t.ThanhLyTaiSans).HasForeignKey(th => th.TaiSanId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LichSuKhauHao>()
                .HasOne(l => l.TaiSan).WithMany(t => t.LichSuKhauHaos).HasForeignKey(l => l.TaiSanId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<LichSuKhauHao>()
                .HasOne(l => l.ChungTu).WithMany(c => c.LichSuKhauHaos).HasForeignKey(l => l.ChungTuId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChiTietChungTu>()
                .HasOne(c => c.ChungTu).WithMany(ch => ch.ChiTietChungTus).HasForeignKey(c => c.ChungTuId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ChiTietChungTu>()
                .HasOne(c => c.TaiSan).WithMany().HasForeignKey(c => c.TaiSanId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<ChiTietChungTu>()
                .HasOne(c => c.TaiKhoanKeToanNo).WithMany().HasForeignKey(c => c.TaiKhoanNo).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ChiTietChungTu>()
                .HasOne(c => c.TaiKhoanKeToanCo).WithMany().HasForeignKey(c => c.TaiKhoanCo).OnDelete(DeleteBehavior.Restrict);

            // 3. Convert Enums sang String khi lưu database
            modelBuilder.Entity<TaiSan>().Property(x => x.TrangThai).HasConversion<string>();
            modelBuilder.Entity<TaiSan>().Property(x => x.PhuongPhapKhauHao).HasConversion<string>();
            modelBuilder.Entity<CauHinhHeThong>().Property(x => x.PhuongPhapKhauHaoMacDinh).HasConversion<string>();
            modelBuilder.Entity<DieuChuyenTaiSan>().Property(x => x.LoaiDieuChuyen).HasConversion<string>();
            modelBuilder.Entity<BaoTriTaiSan>().Property(x => x.TrangThai).HasConversion<string>();
            modelBuilder.Entity<ThanhLyTaiSan>().Property(x => x.TrangThai).HasConversion<string>();
            modelBuilder.Entity<ChungTu>().Property(x => x.LoaiChungTu).HasConversion<string>();

            // ====== UTC CONVERTERS ======
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
            // ====== STATIC SEED DATA (Dữ liệu tĩnh, ít thay đổi) ======
            // =========================================================

            // Seed Hệ thống tài khoản mặc định (ví dụ)
            modelBuilder.Entity<TaiKhoanKeToan>().HasData(
                new TaiKhoanKeToan { Id = "tk_211", MaTaiKhoan = "211", TenTaiKhoan = "Tài sản cố định hữu hình", LoaiTaiKhoan = "no" },
                new TaiKhoanKeToan { Id = "tk_214", MaTaiKhoan = "214", TenTaiKhoan = "Hao mòn tài sản cố định", LoaiTaiKhoan = "co" }
            );

            // Seed Cấu hình hệ thống mặc định
            modelBuilder.Entity<CauHinhHeThong>().HasData(
                new CauHinhHeThong
                {
                    Id = 1,
                    TenCongTy = "Công ty TNHH ABC",
                    TienToChungTu = "CT",
                    SoBatDauChungTu = 1,
                    TuDongKhauHao = true,
                    DinhDangMaTaiSan = "{DANH_MUC}-{SO_THU_TU}",
                    DoDaiMaTaiSan = 4,
                    PhuongPhapKhauHaoMacDinh = PhuongPhapKhauHao.DuongThang
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}