import { apiClient } from './client';

export interface ChiTietChungTu {
  id?: number;
  taiKhoanNo: string;
  taiKhoanCo: string;
  soTien: number;
  moTa?: string; // <-- Đổi từ dienGiai thành moTa
  taiSanId?: number;
  maTaiSan?: string;
}

export interface ChungTu {
  id?: number;
  maChungTu: string;
  ngayLap: string;       // <-- Đổi ngayChungTu thành ngayLap
  loaiChungTu: number;   
  moTa: string;          // <-- Đổi dienGiai thành moTa
  tongTien: number;
  trangThai: string;     // <-- Đổi number thành string ("nhap", "hoan_thanh")
  nguoiLapId?: number | null; // <-- Đổi nguoiLap thành nguoiLapId
  ngayTao?: string;
  nguoiGhiSo?: string;
  ngayGhiSo?: string;
  chiTietChungTus?: ChiTietChungTu[];
}

export const voucherApi = {
  getAll: () => apiClient.get('/ChungTu/get-all'),
  getById: (id: number) => apiClient.get(`/ChungTu/get/${id}`),
  postVoucher: (id: number) => apiClient.put(`/ChungTu/post-voucher/${id}`, {}), 
};