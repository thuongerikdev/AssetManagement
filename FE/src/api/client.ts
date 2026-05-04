// src/utils/apiClient.ts

// Thay đổi URL này khi bạn deploy lên server thật
const BASE_URL = 'https://9fd9-2405-4802-1caf-be30-31fc-7a4-7bf0-3fb2.ngrok-free.app/api';

const buildUrl = (endpoint: string) => {
  if (endpoint.startsWith('http')) return endpoint;
  return `${BASE_URL}${endpoint}`;
};

const getHeaders = () => {
  return {
    'Content-Type': 'application/json',
    'accept': '*/*',
    'ngrok-skip-browser-warning': 'true' 
  };
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
  return response.json();
};

export const apiClient = {
  get: async (endpoint: string) => {
    const response = await fetch(buildUrl(endpoint), {
      method: 'GET',
      headers: getHeaders(),
      credentials: 'include', // BẮT BUỘC: Để trình duyệt gửi kèm Cookie
    });
    return handleResponse(response);
  },

  post: async (endpoint: string, data: any) => {
    const response = await fetch(buildUrl(endpoint), {
      method: 'POST',
      headers: getHeaders(),
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },

  put: async (endpoint: string, data: any) => {
    const response = await fetch(buildUrl(endpoint), {
      method: 'PUT',
      headers: getHeaders(),
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },

  delete: async (endpoint: string) => {
    const response = await fetch(buildUrl(endpoint), {
      method: 'DELETE',
      headers: getHeaders(),
      credentials: 'include',
    });
    return handleResponse(response);
  }
};