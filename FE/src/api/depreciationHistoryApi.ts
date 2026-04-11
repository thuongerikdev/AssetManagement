// src/api/depreciationHistoryApi.ts
import { apiClient } from './client';

export interface LichSuKhauHao {
  id?: number;
  taiSanId: number;
  chungTuId?: number | null;
  kyKhauHao: string; // Định dạng "YYYY-MM"
  soTien: number;
  luyKeSauKhauHao?: number;
  conLaiSauKhauHao?: number;
  ngayTao?: string;
}

export const depreciationHistoryApi = {
  getAll: () => apiClient.get('/LichSuKhauHao/get-all'),
  getById: (id: number) => apiClient.get(`/LichSuKhauHao/get/${id}`),
  create: (data: LichSuKhauHao) => apiClient.post('/LichSuKhauHao/create', data),
  update: (data: LichSuKhauHao) => apiClient.put('/LichSuKhauHao/update', data),
  delete: (id: number) => apiClient.delete(`/LichSuKhauHao/delete/${id}`),
  getByAssetId: (assetId: number) => apiClient.get(`/LichSuKhauHao/get-by-asset/${assetId}`),
};