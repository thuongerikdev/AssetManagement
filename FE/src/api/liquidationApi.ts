// src/api/liquidationApi.ts
import { apiClient } from './client';

export interface ThanhLyTaiSan {
  id?: number;
  taiSanId: number;
  ngayThanhLy?: string;
  nguyenGia?: number;
  khauHaoLuyKe?: number;
  giaTriConLai?: number;
  giaTriThanhLy: number;
  laiLo?: number;
  lyDo: string;
  ghiChu?: string;
  // Sửa từ number sang union type (chuỗi) để khớp với giao diện và backend
  trangThai?: 'ChoDuyet' | 'DaDuyet' | 'DaHoanThanh' | string; 
  ngayTao?: string;
}

export const liquidationApi = {
  getAll: () => apiClient.get('/ThanhLyTaiSan/get-all'),
  getById: (id: number) => apiClient.get(`/ThanhLyTaiSan/get/${id}`),
  create: (data: ThanhLyTaiSan) => apiClient.post('/ThanhLyTaiSan/create', data),
  update: (data: ThanhLyTaiSan) => apiClient.put('/ThanhLyTaiSan/update', data),
  delete: (id: number) => apiClient.delete(`/ThanhLyTaiSan/delete/${id}`),
  getByAssetId: (assetId: number) => apiClient.get(`/ThanhLyTaiSan/get-by-asset/${assetId}`),
};