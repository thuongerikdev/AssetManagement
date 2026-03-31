import { apiClient } from './client';

export interface BaoTriTaiSan {
  id?: number;
  taiSanId: number;
  ngayThucHien?: string; // <-- Đổi từ ngayBaoTri thành ngayThucHien
  loaiBaoTri: string;
  moTa: string;
  coChiPhi?: boolean;    // <-- Bổ sung thêm trường này
  chiPhi?: number;
  loaiChiPhi?: string;
  nhaCungCap?: string;
  ghiChu?: string;
  trangThai: number;
}

export const maintenanceApi = {
  getAll: () => apiClient.get('/BaoTriTaiSan/get-all'),
  getById: (id: number) => apiClient.get(`/BaoTriTaiSan/get/${id}`),
  create: (data: BaoTriTaiSan) => apiClient.post('/BaoTriTaiSan/create', data),
  update: (data: BaoTriTaiSan) => apiClient.put('/BaoTriTaiSan/update', data),
  delete: (id: number) => apiClient.delete(`/BaoTriTaiSan/delete/${id}`),
};