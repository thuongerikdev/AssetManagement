// src/api/assetApi.ts
import { apiClient } from './client';


import { DieuChuyenTaiSan } from './assetAllocationApi'; 

export type PhuongThucThanhToan = 0 | 1 | 2 | 3;

export const PHUONG_THUC_THANH_TOAN_OPTIONS: {
  value: PhuongThucThanhToan;
  label: string;
}[] = [
  { value: 0, label: 'Tiền mặt' },
  { value: 1, label: 'Chuyển khoản' },
  { value: 2, label: 'Thẻ ngân hàng' },
  { value: 3, label: 'Công nợ / Ghi nợ' },
];

export interface TaiSan {
  id?: number;
  maTaiSan: string;
  tenTaiSan?: string;
  danhMucId?: number;
  trangThai?: number;
  soSeri?: string;
  nhaSanXuat?: string;
  moTa?: string;
  thongSoKyThuat?: string;
  ngayMua?: string;
  nguyenGia?: number;
  giaTriConLai?: number;
  khauHaoLuyKe?: number;
  khauHaoHangThang?: number;
  phuongPhapKhauHao?: number;
  thoiGianKhauHao?: number;
  maTaiKhoan?: string;
  phongBanId?: number;
  nguoiDungId?: number;
  ngayCapPhat?: string;
  ngayTao?: string;
  ngayCapNhat?: string;
  phuongThucThanhToan?: PhuongThucThanhToan;  // <-- THÊM
}

export const assetApi = {
  getAll: () => apiClient.get('/TaiSan/get-all'),
  getById: (id: number) => apiClient.get(`/TaiSan/get/${id}`),
  create: (data: TaiSan) => apiClient.post('/TaiSan/create', data),
  update: (data: TaiSan) => apiClient.put('/TaiSan/update', data),
  delete: (id: number) => apiClient.delete(`/TaiSan/delete/${id}`),

  confirm: (id: number) => apiClient.post(`/TaiSan/confirm/${id}`, {}),
  getMyAssets: (userId: number) => apiClient.get(`/TaiSan/my-assets/${userId}`),
  generateCode: (danhMucId: number) => apiClient.get(`/TaiSan/generate-code?danhMucId=${danhMucId}`),
};