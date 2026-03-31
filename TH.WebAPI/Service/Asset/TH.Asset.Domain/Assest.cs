using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TH.Asset.Domain.Enums;

namespace TH.Asset.Domain.Entities
{
    [Table("phong_ban", Schema = Constant.Database.DbSchema.Asset)]
    public class PhongBan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string MaPhongBan { get; set; }

        public string? TenPhongBan { get; set; }

        public virtual ICollection<TaiSan>? TaiSans { get; set; }
    }

    [Table("tai_khoan_ke_toan", Schema = Constant.Database.DbSchema.Asset)]
    public class TaiKhoanKeToan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string MaTaiKhoan { get; set; }

        public string? TenTaiKhoan { get; set; }
        public string? LoaiTaiKhoan { get; set; }
        public string? MaTaiKhoanCha { get; set; }

        [ForeignKey(nameof(MaTaiKhoanCha))]
        public virtual TaiKhoanKeToan? TaiKhoanCha { get; set; }
    }

    [Table("danh_muc_tai_san", Schema = Constant.Database.DbSchema.Asset)]
    public class DanhMucTaiSan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string MaDanhMuc { get; set; }

        public string? TenDanhMuc { get; set; }
        public string? TienTo { get; set; }
        public int? ThoiGianKhauHao { get; set; }
        public string? MaTaiKhoan { get; set; }

        [ForeignKey(nameof(MaTaiKhoan))]
        public virtual TaiKhoanKeToan? TaiKhoanKeToan { get; set; }

        public virtual ICollection<TaiSan>? TaiSans { get; set; }
    }

    [Table("cau_hinh_he_thong", Schema = Constant.Database.DbSchema.Asset)]
    public class CauHinhHeThong
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? TenCongTy { get; set; }
        public string? MaSoThue { get; set; }
        public string? DiaChi { get; set; }
        public string? TienToChungTu { get; set; }
        public int? SoBatDauChungTu { get; set; }
        public PhuongPhapKhauHao? PhuongPhapKhauHaoMacDinh { get; set; }
        public bool? TuDongKhauHao { get; set; }
        public string? DinhDangMaTaiSan { get; set; }
        public int? DoDaiMaTaiSan { get; set; }
    }

    [Table("lo_tai_san", Schema = Constant.Database.DbSchema.Asset)]
    public class LoTaiSan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string MaLo { get; set; }

        public int? SoLuong { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TongGiaTri { get; set; }

        public DateTime? NgayTao { get; set; }

        public virtual ICollection<TaiSan>? TaiSans { get; set; }
    }

    [Table("tai_san", Schema = Constant.Database.DbSchema.Asset)]
    public class TaiSan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string MaTaiSan { get; set; }

        public string? TenTaiSan { get; set; }
        public int? DanhMucId { get; set; }
        public int? LoId { get; set; }
        public int? SoThuTuTrongLo { get; set; }
        public int? TongSoTrongLo { get; set; }

        public TrangThaiTaiSan TrangThai { get; set; } = TrangThaiTaiSan.DangSuDung;

        public string? SoSeri { get; set; }
        public string? NhaSanXuat { get; set; }
        public string? MoTa { get; set; }

        [Column(TypeName = "jsonb")]
        public string? ThongSoKyThuat { get; set; }

        public DateTime? NgayMua { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? NguyenGia { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? GiaTriConLai { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? KhauHaoLuyKe { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? KhauHaoHangThang { get; set; }

        public PhuongPhapKhauHao? PhuongPhapKhauHao { get; set; }
        public int? ThoiGianKhauHao { get; set; }

        public string? MaTaiKhoan { get; set; }
        public int? PhongBanId { get; set; }

        // FK tham chiếu tới module Auth (Không cần virtual NguoiDung)
        public int? NguoiDungId { get; set; }

        public DateTime? NgayCapPhat { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }

        [ForeignKey(nameof(DanhMucId))]
        public virtual DanhMucTaiSan? DanhMucTaiSan { get; set; }

        [ForeignKey(nameof(LoId))]
        public virtual LoTaiSan? LoTaiSan { get; set; }

        [ForeignKey(nameof(PhongBanId))]
        public virtual PhongBan? PhongBan { get; set; }

        [ForeignKey(nameof(MaTaiKhoan))]
        public virtual TaiKhoanKeToan? TaiKhoanKeToan { get; set; }

        public virtual ICollection<DieuChuyenTaiSan>? DieuChuyenTaiSans { get; set; }
        public virtual ICollection<BaoTriTaiSan>? BaoTriTaiSans { get; set; }
        public virtual ICollection<ThanhLyTaiSan>? ThanhLyTaiSans { get; set; }
        public virtual ICollection<LichSuKhauHao>? LichSuKhauHaos { get; set; }
    }

    [Table("dieu_chuyen_tai_san", Schema = Constant.Database.DbSchema.Asset)]
    public class DieuChuyenTaiSan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? TaiSanId { get; set; }
        public LoaiDieuChuyen? LoaiDieuChuyen { get; set; }
        public DateTime? NgayThucHien { get; set; }

        public int? TuPhongBanId { get; set; }
        public int? DenPhongBanId { get; set; }

        public int? TuNguoiDungId { get; set; }
        public int? DenNguoiDungId { get; set; }

        public string? TrangThai { get; set; } = "da_hoan_thanh";
        public string? GhiChu { get; set; }
        public DateTime? NgayTao { get; set; }

        [ForeignKey(nameof(TaiSanId))]
        public virtual TaiSan? TaiSan { get; set; }

        [ForeignKey(nameof(TuPhongBanId))]
        public virtual PhongBan? TuPhongBan { get; set; }

        [ForeignKey(nameof(DenPhongBanId))]
        public virtual PhongBan? DenPhongBan { get; set; }
    }

    [Table("bao_tri_tai_san", Schema = Constant.Database.DbSchema.Asset)]
    public class BaoTriTaiSan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? TaiSanId { get; set; }
        public DateTime? NgayThucHien { get; set; }
        public string? LoaiBaoTri { get; set; }
        public string? MoTa { get; set; }
        public bool? CoChiPhi { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ChiPhi { get; set; } = 0;

        public string? LoaiChiPhi { get; set; }
        public string? NhaCungCap { get; set; }

        public TrangThaiBaoTri? TrangThai { get; set; }
        public string? GhiChu { get; set; }
        public DateTime? NgayTao { get; set; }

        [ForeignKey(nameof(TaiSanId))]
        public virtual TaiSan? TaiSan { get; set; }
    }

    [Table("thanh_ly_tai_san", Schema = Constant.Database.DbSchema.Asset)]
    public class ThanhLyTaiSan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? TaiSanId { get; set; }
        public DateTime? NgayThanhLy { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? NguyenGia { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? KhauHaoLuyKe { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? GiaTriConLai { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? GiaTriThanhLy { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? LaiLo { get; set; }

        public string? LyDo { get; set; }
        public string? GhiChu { get; set; }

        public TrangThaiThanhLy? TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }

        [ForeignKey(nameof(TaiSanId))]
        public virtual TaiSan? TaiSan { get; set; }
    }

    [Table("lich_su_khau_hao", Schema = Constant.Database.DbSchema.Asset)]
    public class LichSuKhauHao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? TaiSanId { get; set; }
        public int? ChungTuId { get; set; }
        public string? KyKhauHao { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SoTien { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? LuyKeSauKhauHao { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ConLaiSauKhauHao { get; set; }

        public DateTime? NgayTao { get; set; }

        [ForeignKey(nameof(TaiSanId))]
        public virtual TaiSan? TaiSan { get; set; }

        [ForeignKey(nameof(ChungTuId))]
        public virtual ChungTu? ChungTu { get; set; }
    }

    [Table("chung_tu", Schema = Constant.Database.DbSchema.Asset)]
    public class ChungTu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string MaChungTu { get; set; }

        public DateTime? NgayLap { get; set; }
        public LoaiChungTu? LoaiChungTu { get; set; }
        public string? MoTa { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TongTien { get; set; }

        public string? TrangThai { get; set; }
        public int? NguoiLapId { get; set; }
        public DateTime? NgayTao { get; set; }

        public virtual ICollection<ChiTietChungTu>? ChiTietChungTus { get; set; }
        public virtual ICollection<LichSuKhauHao>? LichSuKhauHaos { get; set; }
    }

    [Table("chi_tiet_chung_tu", Schema = Constant.Database.DbSchema.Asset)]
    public class ChiTietChungTu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ChungTuId { get; set; }

        public string? TaiKhoanNo { get; set; }
        public string? TaiKhoanCo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SoTien { get; set; }

        public string? MoTa { get; set; }
        public int? TaiSanId { get; set; }

        [ForeignKey(nameof(ChungTuId))]
        public virtual ChungTu? ChungTu { get; set; }

        [ForeignKey(nameof(TaiSanId))]
        public virtual TaiSan? TaiSan { get; set; }

        [ForeignKey(nameof(TaiKhoanNo))]
        public virtual TaiKhoanKeToan? TaiKhoanKeToanNo { get; set; }

        [ForeignKey(nameof(TaiKhoanCo))]
        public virtual TaiKhoanKeToan? TaiKhoanKeToanCo { get; set; }
    }
}