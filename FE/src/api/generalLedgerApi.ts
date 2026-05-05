import { apiClient } from './client';

export interface SoCaiTomTatResponse {
  maTaiKhoan: string;
  tenTaiKhoan?: string;
  loaiTaiKhoan?: string;
  soDuDauKy: number;
  phatSinhNo: number;
  phatSinhCo: number;
  soDuCuoiKy: number;
  soLuongButToan: number;
}

export interface SoCaiButToanResponse {
  chungTuId?: number;
  maChungTu?: string;
  ngayHachToan?: string;
  dienGiai?: string;
  phatSinhNo: number;
  phatSinhCo: number;
  soDuLuyKe: number;
  loaiChungTu?: string;
}

export interface SoCaiChiTietResponse {
  maTaiKhoan: string;
  tenTaiKhoan?: string;
  loaiTaiKhoan?: string;
  soDuDauKy: number;
  phatSinhNo: number;
  phatSinhCo: number;
  soDuCuoiKy: number;
  butToans: SoCaiButToanResponse[];
}

export const generalLedgerApi = {
  getTomTat: (fromDate?: string, toDate?: string) => {
    const params = new URLSearchParams();
    if (fromDate) params.append('fromDate', fromDate);
    if (toDate) params.append('toDate', toDate);
    const qs = params.toString();
    return apiClient.get(`/SoCai/tom-tat${qs ? '?' + qs : ''}`);
  },
  getChiTiet: (maTaiKhoan: string, fromDate?: string, toDate?: string) => {
    const params = new URLSearchParams();
    if (fromDate) params.append('fromDate', fromDate);
    if (toDate) params.append('toDate', toDate);
    const qs = params.toString();
    return apiClient.get(`/SoCai/chi-tiet/${encodeURIComponent(maTaiKhoan)}${qs ? '?' + qs : ''}`);
  },
};
