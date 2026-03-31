// src/api/accountApi.ts
import { apiClient } from './client';

export interface TaiKhoanKeToan {
  id?: number;
  maTaiKhoan: string;
  tenTaiKhoan?: string;
  loaiTaiKhoan?: string;
  maTaiKhoanCha?: string;
}

export const accountApi = {
  getAll: () => apiClient.get('/TaiKhoanKeToan/get-all'),
  getById: (id: number) => apiClient.get(`/TaiKhoanKeToan/get/${id}`),
  create: (data: TaiKhoanKeToan) => apiClient.post('/TaiKhoanKeToan/create', data),
  update: (data: TaiKhoanKeToan) => apiClient.put('/TaiKhoanKeToan/update', data),
  delete: (id: number) => apiClient.delete(`/TaiKhoanKeToan/delete/${id}`),
};