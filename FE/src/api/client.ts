// src/utils/apiClient.ts

// Thay đổi URL này khi bạn deploy lên server thật
const BASE_URL = 'https://assetmanagement.fly.dev/api';

const buildUrl = (endpoint: string) => {
  if (endpoint.startsWith('http')) return endpoint;
  return `${BASE_URL}${endpoint}`;
};

// Cập nhật hàm getHeaders để lấy Token
const getHeaders = () => {
  const headers: Record<string, string> = {
    'Content-Type': 'application/json',
    'accept': '*/*',
    'ngrok-skip-browser-warning': 'true' 
  };

  // CÁCH 2: Kẹp Token lên Header
  // Giả sử bạn lưu token trong localStorage với key là 'access_token' sau khi đăng nhập
  // Bạn có thể thay đổi cách lấy token này (ví dụ từ Redux/Zustand store nếu cần)
  const token = localStorage.getItem('access_token');
  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }

  return headers;
};

// Hàm xử lý chung cho Response
const handleResponse = async (response: Response) => {
  if (response.status === 401 || response.status === 403) {
    // Bắn một event toàn cục để React (AuthContext) bắt được và hiện Modal
    window.dispatchEvent(new CustomEvent('unauthorized-access', { 
        detail: { status: response.status } 
    }));
    throw new Error('Unauthorized or Forbidden');
  }

  if (!response.ok) {
    throw new Error('API Request Failed');
  }
  
  // Xử lý trường hợp API trả về rỗng (204 No Content) để tránh lỗi JSON.parse
  if (response.status === 204) {
    return null; 
  }
  return response.json();
};

export const apiClient = {
  get: async (endpoint: string) => {
    const response = await fetch(buildUrl(endpoint), {
      method: 'GET',
      headers: getHeaders(),
      credentials: 'include', // CÁCH 1: BẮT BUỘC để trình duyệt gửi kèm Cookie
    });
    return handleResponse(response);
  },

  post: async (endpoint: string, data: any) => {
    const response = await fetch(buildUrl(endpoint), {
      method: 'POST',
      headers: getHeaders(),
      credentials: 'include', // CÁCH 1
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },

  put: async (endpoint: string, data: any) => {
    const response = await fetch(buildUrl(endpoint), {
      method: 'PUT',
      headers: getHeaders(),
      credentials: 'include', // CÁCH 1
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },

  delete: async (endpoint: string) => {
    const response = await fetch(buildUrl(endpoint), {
      method: 'DELETE',
      headers: getHeaders(),
      credentials: 'include', // CÁCH 1
    });
    return handleResponse(response);
  }
};