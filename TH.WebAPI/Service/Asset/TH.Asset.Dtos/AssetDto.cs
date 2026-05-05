using System;
using System.Collections.Generic;
using TH.Asset.Domain.Enums;

namespace TH.Asset.Dtos
{
    // ==========================================
    // 1. PHÒNG BAN DTOs
    // ==========================================
    public class CreatePhongBanRequestDto
    {
        public required string maPhongBan { get; set; }
        public string? tenPhongBan { get; set; }
    }

    public class UpdatePhongBanRequestDto : CreatePhongBanRequestDto
    {
        public int id { get; set; }
    }

    public class PhongBanResponse
    {
        public int id { get; set; }
        public string maPhongBan { get; set; }
        public string? tenPhongBan { get; set; }
    }

    // ==========================================
    // 2. TÀI KHOẢN KẾ TOÁN DTOs
    // ==========================================
    public class CreateTaiKhoanKeToanRequestDto
    {
        public required string maTaiKhoan { get; set; }
        public string? tenTaiKhoan { get; set; }
        public string? loaiTaiKhoan { get; set; }
        public string? maTaiKhoanCha { get; set; }
    }

    public class UpdateTaiKhoanKeToanRequestDto : CreateTaiKhoanKeToanRequestDto
    {
        public int id { get; set; }
    }

    public class TaiKhoanKeToanResponse
    {
        public int id { get; set; }
        public string maTaiKhoan { get; set; }
        public string? tenTaiKhoan { get; set; }
        public string? loaiTaiKhoan { get; set; }
        public string? maTaiKhoanCha { get; set; }
    }

    // ==========================================
    // 3. DANH MỤC TÀI SẢN DTOs
    // ==========================================
    public class CreateDanhMucTaiSanRequestDto
    {
        public required string maDanhMuc { get; set; }
        public string? tenDanhMuc { get; set; }
        public string? tienTo { get; set; }
        public int? thoiGianKhauHao { get; set; }
        public string? maTaiKhoan { get; set; }
    }

    public class UpdateDanhMucTaiSanRequestDto : CreateDanhMucTaiSanRequestDto
    {
        public int id { get; set; }
    }

    public class DanhMucTaiSanResponse
    {
        public int id { get; set; }
        public string maDanhMuc { get; set; }
        public string? tenDanhMuc { get; set; }
        public string? tienTo { get; set; }
        public int? thoiGianKhauHao { get; set; }
        public string? maTaiKhoan { get; set; }
    }

    // ==========================================
    // 4. LÔ TÀI SẢN DTOs
    // ==========================================
    public class CreateLoTaiSanRequestDto
    {
        public required string maLo { get; set; }
        public int? soLuong { get; set; }
        public decimal? tongGiaTri { get; set; }
    }

    public class UpdateLoTaiSanRequestDto : CreateLoTaiSanRequestDto
    {
        public int id { get; set; }
    }

    public class LoTaiSanResponse
    {
        public int id { get; set; }
        public string maLo { get; set; }
        public int? soLuong { get; set; }
        public decimal? tongGiaTri { get; set; }
        public DateTime? ngayTao { get; set; }
    }

    // ==========================================
    // 5. TÀI SẢN DTOs
    // ==========================================
    public class CreateTaiSanRequestDto
    {
        public required string maTaiSan { get; set; }
        public string? tenTaiSan { get; set; }
        public int? danhMucId { get; set; }
        public TrangThaiTaiSan trangThai { get; set; } = TrangThaiTaiSan.DangSuDung;
        public string? soSeri { get; set; }
        public string? nhaSanXuat { get; set; }
        public string? moTa { get; set; }
        public string? thongSoKyThuat { get; set; }
        public DateTime? ngayMua { get; set; }
        public decimal? nguyenGia { get; set; }
        public PhuongPhapKhauHao? phuongPhapKhauHao { get; set; }
        public int? thoiGianKhauHao { get; set; }
        public string? maTaiKhoan { get; set; }
        public int? phongBanId { get; set; }
        public int? nguoiDungId { get; set; }
        public DateTime? ngayCapPhat { get; set; }

        public PhuongThucThanhToan? phuongThucThanhToan { get; set; }  // <-- THÊM
    }

    public class UpdateTaiSanRequestDto : CreateTaiSanRequestDto
    {
        public int id { get; set; }
        public decimal? giaTriConLai { get; set; }
        public decimal? khauHaoLuyKe { get; set; }
        public decimal? khauHaoHangThang { get; set; }
        // phuongThucThanhToan kế thừa từ Create
    }

    public class TaiSanResponse
    {
        public int id { get; set; }
        public string maTaiSan { get; set; }
        public string? tenTaiSan { get; set; }
        public int? danhMucId { get; set; }
        public int? loId { get; set; }
        public int? soThuTuTrongLo { get; set; }
        public int? tongSoTrongLo { get; set; }
        public TrangThaiTaiSan trangThai { get; set; }
        public string? soSeri { get; set; }
        public string? nhaSanXuat { get; set; }
        public string? moTa { get; set; }
        public string? thongSoKyThuat { get; set; }
        public DateTime? ngayMua { get; set; }
        public decimal? nguyenGia { get; set; }
        public decimal? giaTriConLai { get; set; }
        public decimal? khauHaoLuyKe { get; set; }
        public decimal? khauHaoHangThang { get; set; }
        public PhuongPhapKhauHao? phuongPhapKhauHao { get; set; }
        public int? thoiGianKhauHao { get; set; }
        public string? maTaiKhoan { get; set; }
        public int? phongBanId { get; set; }
        public int? nguoiDungId { get; set; }

        public DateTime? ngayCapPhat { get; set; }
        public DateTime? ngayTao { get; set; }
        public DateTime? ngayCapNhat { get; set; }

        public PhuongThucThanhToan? phuongThucThanhToan { get; set; }  // <-- THÊM
    }

    // ==========================================
    // 6. ĐIỀU CHUYỂN TÀI SẢN DTOs
    // ==========================================
    public class CreateDieuChuyenTaiSanRequestDto
    {
        public required int taiSanId { get; set; }
        public LoaiDieuChuyen loaiDieuChuyen { get; set; }
        public DateTime? ngayThucHien { get; set; }
        public int? tuPhongBanId { get; set; }
        public int? denPhongBanId { get; set; }
        public int? tuNguoiDungId { get; set; }
        public int? denNguoiDungId { get; set; }
        public string? ghiChu { get; set; }
    }

    public class UpdateDieuChuyenTaiSanRequestDto : CreateDieuChuyenTaiSanRequestDto
    {
        public int id { get; set; }
        public string? trangThai { get; set; }
    }

    public class DieuChuyenTaiSanResponse
    {
        public int id { get; set; }
        public int? taiSanId { get; set; }
        public LoaiDieuChuyen? loaiDieuChuyen { get; set; }
        public DateTime? ngayThucHien { get; set; }
        public int? tuPhongBanId { get; set; }
        public int? denPhongBanId { get; set; }
        public int? tuNguoiDungId { get; set; }
        public int? denNguoiDungId { get; set; }
        public string? trangThai { get; set; }
        public string? ghiChu { get; set; }
        public DateTime? ngayTao { get; set; }
    }

    // ==========================================
    // 7. BẢO TRÌ TÀI SẢN DTOs
    // ==========================================
    public class CreateBaoTriTaiSanRequestDto
    {
        public required int taiSanId { get; set; }
        public DateTime? ngayThucHien { get; set; }
        public string? loaiBaoTri { get; set; }
        public string? moTa { get; set; }
        public bool? coChiPhi { get; set; }
        public decimal? chiPhi { get; set; }
        public string? loaiChiPhi { get; set; }
        public string? nhaCungCap { get; set; }
        public TrangThaiBaoTri? trangThai { get; set; }
        public string? ghiChu { get; set; }
    }

    public class UpdateBaoTriTaiSanRequestDto : CreateBaoTriTaiSanRequestDto
    {
        public int id { get; set; }
    }

    public class BaoTriTaiSanResponse
    {
        public int id { get; set; }
        public int? taiSanId { get; set; }
        public DateTime? ngayThucHien { get; set; }
        public string? loaiBaoTri { get; set; }
        public string? moTa { get; set; }
        public bool? coChiPhi { get; set; }
        public decimal? chiPhi { get; set; }
        public string? loaiChiPhi { get; set; }
        public string? nhaCungCap { get; set; }
        public TrangThaiBaoTri? trangThai { get; set; }
        public string? ghiChu { get; set; }
        public DateTime? ngayTao { get; set; }
    }

    // ==========================================
    // 8. CHỨNG TỪ DTOs
    // ==========================================
    public class CreateChungTuRequestDto
    {
        public required string maChungTu { get; set; }
        public DateTime? ngayLap { get; set; }
        public LoaiChungTu? loaiChungTu { get; set; }
        public string? moTa { get; set; }
        public decimal? tongTien { get; set; }
        public string? trangThai { get; set; }
        public int? nguoiLapId { get; set; }

        public List<CreateChiTietChungTuRequestDto>? chiTietChungTus { get; set; }
    }

    public class UpdateChungTuRequestDto : CreateChungTuRequestDto
    {
        public int id { get; set; }
    }

    public class ChungTuResponse
    {
        public int id { get; set; }
        public string maChungTu { get; set; }
        public DateTime? ngayLap { get; set; }
        public LoaiChungTu? loaiChungTu { get; set; }
        public string? moTa { get; set; }
        public decimal? tongTien { get; set; }
        public string? trangThai { get; set; }
        public int? nguoiLapId { get; set; }
        public DateTime? ngayTao { get; set; }

        public List<ChiTietChungTuResponse>? chiTietChungTus { get; set; }
    }

    // --- CHI TIẾT CHỨNG TỪ ---
    public class CreateChiTietChungTuRequestDto
    {
        public string? taiKhoanNo { get; set; }
        public string? taiKhoanCo { get; set; }
        public decimal? soTien { get; set; }
        public string? moTa { get; set; }
        public int? taiSanId { get; set; }
    }

    public class ChiTietChungTuResponse
    {
        public int id { get; set; }
        public int? chungTuId { get; set; }
        public string? taiKhoanNo { get; set; }
        public string? taiKhoanCo { get; set; }
        public decimal? soTien { get; set; }
        public string? moTa { get; set; }
        public int? taiSanId { get; set; }
    }


    public class CauHinhHeThongRequestDto
    {
        public string? tenCongTy { get; set; }
        public string? maSoThue { get; set; }
        public string? diaChi { get; set; }
        public string? tienToChungTu { get; set; }
        public int? soBatDauChungTu { get; set; }
        public PhuongPhapKhauHao? phuongPhapKhauHaoMacDinh { get; set; }
        public bool? tuDongKhauHao { get; set; }
        public string? dinhDangMaTaiSan { get; set; }
        public int? doDaiMaTaiSan { get; set; }
    }

    public class CauHinhHeThongResponse : CauHinhHeThongRequestDto
    {
        public int id { get; set; }
    }


    public class CreateThanhLyTaiSanRequestDto
    {
        public required int taiSanId { get; set; }
        public DateTime? ngayThanhLy { get; set; }
        public decimal? giaTriThanhLy { get; set; }
        public string? lyDo { get; set; }
        public string? ghiChu { get; set; }
        public TrangThaiThanhLy? trangThai { get; set; }
    }

    public class UpdateThanhLyTaiSanRequestDto : CreateThanhLyTaiSanRequestDto
    {
        public int id { get; set; }
    }

    public class ThanhLyTaiSanResponse
    {
        public int id { get; set; }
        public int? taiSanId { get; set; }
        public DateTime? ngayThanhLy { get; set; }
        public decimal? nguyenGia { get; set; }
        public decimal? khauHaoLuyKe { get; set; }
        public decimal? giaTriConLai { get; set; }
        public decimal? giaTriThanhLy { get; set; }
        public decimal? laiLo { get; set; }
        public string? lyDo { get; set; }
        public string? ghiChu { get; set; }
        public TrangThaiThanhLy? trangThai { get; set; }
        public DateTime? ngayTao { get; set; }
    }



    public class CreateLichSuKhauHaoRequestDto
    {
        public required int taiSanId { get; set; }
        public int? chungTuId { get; set; }
        public required string kyKhauHao { get; set; } // VD: "2026-03"
        public decimal soTien { get; set; }
    }

    public class UpdateLichSuKhauHaoRequestDto : CreateLichSuKhauHaoRequestDto
    {
        public int id { get; set; }
    }

    public class LichSuKhauHaoResponse
    {
        public int id { get; set; }
        public int? taiSanId { get; set; }
        public int? chungTuId { get; set; }
        public string? kyKhauHao { get; set; }
        public decimal? soTien { get; set; }
        public decimal? luyKeSauKhauHao { get; set; }
        public decimal? conLaiSauKhauHao { get; set; }
        public DateTime? ngayTao { get; set; }
    }


    public class KhauHaoHangLoatRequestDto
    {
        public string KyKhauHao { get; set; }
        public List<ChiTietKhauHaoDto> DanhSachTaiSan { get; set; }
    }

    public class ChiTietKhauHaoDto
    {
        public int TaiSanId { get; set; }
        public decimal SoTien { get; set; }
    }

    // ==========================================
    // SỔ CÁI DTOs
    // ==========================================

    public class SoCaiTomTatResponse
    {
        public string maTaiKhoan { get; set; } = "";
        public string? tenTaiKhoan { get; set; }
        public string? loaiTaiKhoan { get; set; }
        public decimal soDuDauKy { get; set; }
        public decimal phatSinhNo { get; set; }
        public decimal phatSinhCo { get; set; }
        public decimal soDuCuoiKy { get; set; }
        public int soLuongButToan { get; set; }
    }

    public class SoCaiChiTietResponse
    {
        public string maTaiKhoan { get; set; } = "";
        public string? tenTaiKhoan { get; set; }
        public string? loaiTaiKhoan { get; set; }
        public decimal soDuDauKy { get; set; }
        public decimal phatSinhNo { get; set; }
        public decimal phatSinhCo { get; set; }
        public decimal soDuCuoiKy { get; set; }
        public List<SoCaiButToanResponse> butToans { get; set; } = new();
    }

    public class SoCaiButToanResponse
    {
        public int chungTuId { get; set; }
        public string maChungTu { get; set; } = "";
        public DateTime? ngayHachToan { get; set; }
        public string dienGiai { get; set; } = "";
        public decimal phatSinhNo { get; set; }
        public decimal phatSinhCo { get; set; }
        public decimal soDuLuyKe { get; set; }
        public string? loaiChungTu { get; set; }
    }

    // ==========================================
    // SINH MÃ TỰ ĐỘNG DTOs
    // ==========================================

    public class GenerateMaTaiSanResponse
    {
        public string maTaiSan { get; set; } = "";
        public string dinhDangApDung { get; set; } = "";
        public int soThuTuTiepTheo { get; set; }
    }

    public class GenerateMaChungTuResponse
    {
        public string maChungTu { get; set; } = "";
        public string tienToApDung { get; set; } = "";
        public int soThuTuTiepTheo { get; set; }
    }

    public class TaiSanDinhKemResponse
    {
        public int id { get; set; }
        public int taiSanId { get; set; }
        public string tenFile { get; set; } = "";
        public string? loaiFile { get; set; }
        public string? duongDan { get; set; }
        public long? kichThuoc { get; set; }
        public DateTime ngayTai { get; set; }
        public string? moTa { get; set; }
    }
}