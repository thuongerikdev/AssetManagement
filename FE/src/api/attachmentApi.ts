import { apiClient } from './client';

export const ATTACHMENT_BASE_URL = 'https://assetmanagement.fly.dev';

export interface TaiSanDinhKem {
  id: number;
  taiSanId: number;
  tenFile: string;
  loaiFile?: string;
  duongDan?: string;
  kichThuoc?: number;
  ngayTai: string;
  moTa?: string;
}

export const attachmentApi = {
  getByAsset: (taiSanId: number) =>
    apiClient.get(`/TaiSanDinhKem/by-asset/${taiSanId}`),

  upload: async (taiSanId: number, file: File, moTa?: string) => {
    const form = new FormData();
    form.append('file', file);
    if (moTa) form.append('moTa', moTa);
    const response = await fetch(`${ATTACHMENT_BASE_URL}/api/TaiSanDinhKem/upload/${taiSanId}`, {
      method: 'POST',
      credentials: 'include',
      headers: { 'ngrok-skip-browser-warning': 'true' },
      body: form,
    });
    if (!response.ok) throw new Error('Upload failed');
    return response.json();
  },

  delete: (id: number) => apiClient.delete(`/TaiSanDinhKem/${id}`),
};

export function formatFileSize(bytes?: number | null): string {
  if (!bytes) return '—';
  if (bytes < 1024) return `${bytes} B`;
  if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)} KB`;
  return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
}

export function getFileIcon(loaiFile?: string | null, tenFile?: string): string {
  const ext = tenFile?.split('.').pop()?.toLowerCase() ?? '';
  const mime = loaiFile?.toLowerCase() ?? '';
  if (mime.includes('image') || ['jpg', 'jpeg', 'png', 'gif', 'webp'].includes(ext)) return '🖼️';
  if (mime.includes('pdf') || ext === 'pdf') return '📄';
  if (['xls', 'xlsx', 'csv'].includes(ext) || mime.includes('spreadsheet') || mime.includes('excel')) return '📊';
  if (['doc', 'docx'].includes(ext) || mime.includes('word')) return '📝';
  if (['zip', 'rar', '7z', 'tar', 'gz'].includes(ext)) return '🗜️';
  return '📎';
}
