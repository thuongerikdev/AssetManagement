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
  login: async (data: LoginRequest): Promise<LoginResponse> => {
    const rootUrl = (import.meta as any).env.VITE_API_BASE_URL?.replace(/\/api\/?$/, '') || 'https://localhost:7029';
    
    // apiClient bây giờ sẽ dùng nguyên link này vì thấy có chữ https ở đầu
    return await apiClient.post(`${rootUrl}/login/userLogin`, data);
  }
};