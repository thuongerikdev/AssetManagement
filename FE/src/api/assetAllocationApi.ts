// src/api/assetAllocationApi.ts
import { apiClient } from './client';

export interface DieuChuyenTaiSan {
  id?: number;
  taiSanId: number;
  // Đã sửa từ number sang kiểu chuỗi (string) để khớp 100% với Backend C#
  loaiDieuChuyen: 'CapPhat' | 'ThuHoi' | 'LuanChuyen' | string; 
  ngayThucHien: string;
  tuPhongBanId?: number;
  denPhongBanId?: number;
  tuNguoiDungId?: number;
  denNguoiDungId?: number;
  trangThai?: string;
  ghiChu?: string;
  ngayTao?: string;
}

export const assetAllocationApi = {
  getAll: () => apiClient.get('/DieuChuyenTaiSan/get-all'),
  getById: (id: number) => apiClient.get(`/DieuChuyenTaiSan/get/${id}`),
  create: (data: DieuChuyenTaiSan) => apiClient.post('/DieuChuyenTaiSan/create', data),
  update: (data: DieuChuyenTaiSan) => apiClient.put('/DieuChuyenTaiSan/update', data),
  delete: (id: number) => apiClient.delete(`/DieuChuyenTaiSan/delete/${id}`),
  getByAssetId: (assetId: number) => apiClient.get(`/DieuChuyenTaiSan/get-by-asset/${assetId}`),
};