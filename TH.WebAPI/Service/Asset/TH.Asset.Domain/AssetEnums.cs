using System;

namespace TH.Asset.Domain.Enums
{
    public enum TrangThaiTaiSan
    {
        ChuaCapPhat = 0, // Mặc định khi mới mua về
        ChoXacNhan = 1,  // Đã gán NguoiDungId nhưng user chưa ấn nhận
        DangSuDung = 2,  // User đã xác nhận
        DaThanhLy = 3    // Đã bỏ trạng thái Đang bảo trì
    }

    // Đã bỏ LoaiVaiTro vì liên quan đến User

    public enum LoaiDieuChuyen
    {
        CapPhat,
        ThuHoi,
        LuanChuyen
    }

    public enum TrangThaiBaoTri
    {
        ChoXuLy,
        DangThucHien,
        DaHoanThanh
    }

    public enum TrangThaiThanhLy
    {
        ChoDuyet,
        DaDuyet,
        DaHoanThanh
    }

    public enum LoaiChungTu
    {
        GhiTang,
        KhauHao,
        BaoTri,
        ThanhLy
    }

    public enum PhuongPhapKhauHao
    {
        DuongThang,
        SoDuGiamDan,
        TongSoNam
    }
}