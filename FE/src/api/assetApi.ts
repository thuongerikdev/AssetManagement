// src/api/assetApi.ts
import { apiClient } from './client';


import { DieuChuyenTaiSan } from './assetAllocationApi'; 

export interface TaiSan {
  id?: number;
  maTaiSan: string;
  tenTaiSan: string;
  danhMucId?: number;
  nhaSanXuat?: string;
  soSeri?: string;
  moTa?: string;
  ngayMua?: string;
  nguyenGia?: number;
  giaTriConLai?: number;
  khauHaoLuyKe?: number;
  khauHaoHangThang?: number;
  thoiGianKhauHao?: number;
  phongBanId?: number;
  nguoiDungId?: number;
  phuongPhapKhauHao?: number | string; 
  maTaiKhoan?: string;
  trangThai?: number | string;
  ngayCapPhat?: string;
  
  // THÊM 2 DÒNG NÀY ĐỂ FIX LỖI TYPESCRIPT
  dieuChuyenTaiSans?: DieuChuyenTaiSan[] | any[]; 
  lichSuKhauHaos?: any[];
}

export const assetApi = {
  getAll: () => apiClient.get('/TaiSan/get-all'),
  getById: (id: number) => apiClient.get(`/TaiSan/get/${id}`),
  create: (data: TaiSan) => apiClient.post('/TaiSan/create', data),
  update: (data: TaiSan) => apiClient.put('/TaiSan/update', data),
  delete: (id: number) => apiClient.delete(`/TaiSan/delete/${id}`),

  confirm: (id: number) => apiClient.post(`/TaiSan/confirm/${id}`, {}),

  getMyAssets: (userId: number) => apiClient.get(`/TaiSan/my-assets/${userId}`),
};