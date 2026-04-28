import { apiClient } from './client';

export interface LoginRequest {
  userName: string;
  password: string;
}

export interface UserData {
  userID: number;
  userName: string;
  email: string;
  isEmailVerified: boolean;
  token: string;
  refreshToken: string;
  sessionId: number;
  mfaTicket: string | null;
  requiresMfa: boolean | null;
  deviceId: string;
  permissions: string[];
  roles: string[];
  tokenExpiration: string;
  refreshTokenExpiration: string;
}

export interface LoginResponse {
  errorCode: number;
  errorMessage: string;
  data?: UserData;
}

export const authApi = {
  login: async (credentials: { userName: string; password: string }) => {
    // Gọi thẳng vào API mới của bạn
    return await apiClient.post('/Login/userLogin', credentials);
  },
  getAllUsers: () => apiClient.get('/user/admin/getAllUsers'),
  deleteUser: (id: number) => apiClient.delete(`/user/deleteUser?userId=${id}`),
  
  // --- ROLES ---
  getAllRoles: () => apiClient.get('/Role/getall'),
  addRole: (data: any) => apiClient.post('/Role/addRole', data),
  updateRole: (data: any) => apiClient.put('/Role/updateRole', data),
  deleteRole: (id: number) => apiClient.delete(`/Role/deleteRole/${id}`),
  cloneRole: (data: any) => apiClient.post('/Role/clonerole', data),
  
  // --- PERMISSIONS ---
  getAllPermissions: () => apiClient.get('/permission/getall'),
  addPermission: (data: any) => apiClient.post('/permission/BulkCreate', [data]),
  updatePermission: (data: any) => apiClient.put('/permission/updatePermission', data),
  deletePermission: (id: number) => apiClient.delete(`/permission/delete?permissionId=${id}`),

};