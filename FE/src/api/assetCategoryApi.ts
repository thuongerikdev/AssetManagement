import { apiClient } from './client';

export interface AssetCategory {
  id?: number;
  maDanhMuc: string;
  tenDanhMuc: string;
  tienTo: string;
  thoiGianKhauHao: number | '';
  maTaiKhoan: string;
}

export const assetCategoryApi = {
  getAll: () => apiClient.get('/DanhMucTaiSan/get-all'),
  getById: (id: number) => apiClient.get(`/DanhMucTaiSan/get/${id}`),
  create: (data: AssetCategory) => apiClient.post('/DanhMucTaiSan/create', data),
  update: (data: AssetCategory) => apiClient.put('/DanhMucTaiSan/update', data),
  delete: (id: number) => apiClient.delete(`/DanhMucTaiSan/delete/${id}`),
};