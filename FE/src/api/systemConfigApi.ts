// src/api/systemConfigApi.ts
import { apiClient } from './client';

export interface CauHinhHeThong {
  id?: number;
  tenCongTy?: string;
  maSoThue?: string;
  diaChi?: string;
  tienToChungTu?: string;
  soBatDauChungTu?: number;
  phuongPhapKhauHaoMacDinh?: number; // 0: Đường thẳng, 1: Số dư giảm dần
  tuDongKhauHao?: boolean;
  dinhDangMaTaiSan?: string;
  doDaiMaTaiSan?: number;
}

export const systemConfigApi = {
  get: () => apiClient.get('/CauHinhHeThong/get'),
  update: (data: CauHinhHeThong) => apiClient.put('/CauHinhHeThong/update', data),
};