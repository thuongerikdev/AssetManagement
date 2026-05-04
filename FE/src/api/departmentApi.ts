// src/api/departmentApi.ts
import { apiClient } from './client';

// Định nghĩa kiểu dữ liệu để tái sử dụng
export interface Department {
  id?: string | number; // id có thể không có khi tạo mới
  maPhongBan: string;
  tenPhongBan: string;
}

export const departmentApi = {
  getAll: () => apiClient.get('/PhongBan/get-all'),

  getById: (id: string | number) => apiClient.get(`/PhongBan/get/${id}`),
  
  create: (data: Department) => apiClient.post('/PhongBan/create', data),
  
  update: (data: Department) => apiClient.put('/PhongBan/update', data),
  
  delete: (id: string | number) => apiClient.delete(`/PhongBan/delete/${id}`),
};