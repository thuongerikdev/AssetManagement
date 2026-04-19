// Thay đổi URL này khi bạn deploy lên server thật
const BASE_URL = 'https://68f1-2405-4802-1c99-5780-a882-b9cc-61cf-bc35.ngrok-free.app/api';

// 1. Hàm xử lý URL thông minh: Nếu endpoint bắt đầu bằng http thì giữ nguyên, ngược lại mới nối BASE_URL
const buildUrl = (endpoint: string) => {
  if (endpoint.startsWith('http')) {
    return endpoint;
  }
  return `${BASE_URL}${endpoint}`;
};

// 2. Hàm tự động lấy Token nhét vào Header (Thay cho interceptor của Axios)
const getHeaders = () => {
  const headers: Record<string, string> = {
    'Content-Type': 'application/json',
    'accept': '*/*',
    // Bỏ qua màn hình cảnh báo của ngrok bản miễn phí để không bị lỗi CORS
    'ngrok-skip-browser-warning': 'true' 
  };
  
  const token = localStorage.getItem('access_token');
  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }
  
  return headers;
};

// Hàm hỗ trợ gọi API cơ bản
export const apiClient = {
  get: async (endpoint: string) => {
    const response = await fetch(buildUrl(endpoint), {
      method: 'GET',
      headers: getHeaders(), // Nhét token và ngrok header vào đây
    });
    return response.json();
  },

  post: async (endpoint: string, data: any) => {
    const response = await fetch(buildUrl(endpoint), {
      method: 'POST',
      headers: getHeaders(), // Nhét token và ngrok header vào đây
      body: JSON.stringify(data),
    });
    return response.json();
  },

  put: async (endpoint: string, data: any) => {
    const response = await fetch(buildUrl(endpoint), {
      method: 'PUT',
      headers: getHeaders(), // Nhét token và ngrok header vào đây
      body: JSON.stringify(data),
    });
    return response.json();
  },

  delete: async (endpoint: string) => {
    const response = await fetch(buildUrl(endpoint), {
      method: 'DELETE',
      headers: getHeaders(), // Nhét token và ngrok header vào đây
    });
    return response.json();
  }
};