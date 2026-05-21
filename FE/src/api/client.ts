// src/utils/apiClient.ts

const BASE_URL = 'https://assetmanagement.fly.dev/api';

const buildUrl = (endpoint: string) => {
  if (endpoint.startsWith('http')) return endpoint;
  return `${BASE_URL}${endpoint}`;
};

const getHeaders = () => {
  const headers: Record<string, string> = {
    'Content-Type': 'application/json',
    'accept': '*/*',
    'ngrok-skip-browser-warning': 'true' 
  };

  const token = localStorage.getItem('access_token');
  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }

  return headers;
};

// --- LOGIC REFRESH TOKEN ---
let isRefreshing = false;
let failedQueue: any[] = [];

// Hàm xử lý các request đang chờ trong lúc refresh
const processQueue = (error: Error | null, token: string | null = null) => {
  failedQueue.forEach(prom => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });
  failedQueue = [];
};

// Hàm xử lý chung cho Response (Đã bỏ check 401 ở đây vì customFetch sẽ lo việc đó)
const handleResponse = async (response: Response) => {
  if (response.status === 403) {
    // 403 là không có quyền (Forbidden)
    window.dispatchEvent(new CustomEvent('unauthorized-access', { 
        detail: { status: 403 } 
    }));
    throw new Error('Forbidden');
  }

  if (!response.ok && response.status !== 401) {
    throw new Error('API Request Failed');
  }
  
  if (response.status === 204) {
    return null; 
  }
  
  // Tránh lỗi khi body rỗng
  const text = await response.text();
  return text ? JSON.parse(text) : {};
};

// Hàm gọi API Refresh
const callRefreshToken = async () => {
  const currentAccessToken = localStorage.getItem('access_token');
  const currentRefreshToken = localStorage.getItem('refresh_token');

  if (!currentRefreshToken) {
    throw new Error("Không có Refresh Token");
  }

  // Dựa theo cURL bạn cung cấp
  const response = await fetch(buildUrl('/Login/auth/refresh'), {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'accept': '*/*',
      'Authorization': `Bearer ${currentAccessToken}`
    },
    body: JSON.stringify({ refreshToken: currentRefreshToken }),
  });

  const data = await response.json();

  if (response.ok && data.errorCode === 200 && data.data?.accessToken) {
    // Lưu token mới. Lưu ý API trả về accessToken thay vì token
    localStorage.setItem('access_token', data.data.accessToken);
    localStorage.setItem('refresh_token', data.data.refreshToken);
    return data.data.accessToken;
  }
  
  throw new Error("Refresh Token Thất bại");
};

// --- CUSTOM FETCH WRAPPER ---
const customFetch = async (endpoint: string, options: RequestInit): Promise<any> => {
  const headers = getHeaders();
  // Don't override Content-Type for FormData — browser sets it with boundary
  if (options.body instanceof FormData) {
    delete headers['Content-Type'];
  }
  options.headers = headers;
  
  let response = await fetch(buildUrl(endpoint), options);

  // Kiểm tra xem token có hết hạn không (API trả 401 hoặc header x-token-expired)
  const isTokenExpired = response.status === 401 || response.headers.get('x-token-expired') === 'true';

  if (isTokenExpired) {
    // Nếu đang refresh dở, đẩy request này vào hàng đợi (queue) chờ token mới
    if (isRefreshing) {
      return new Promise(function(resolve, reject) {
        failedQueue.push({ resolve, reject });
      }).then(token => {
        // Khi refresh xong, gọi lại API với token mới
        options.headers = {
          ...options.headers,
          'Authorization': `Bearer ${token}`
        };
        return fetch(buildUrl(endpoint), options).then(handleResponse);
      }).catch(err => {
        return Promise.reject(err);
      });
    }

    // Bắt đầu khóa refresh
    isRefreshing = true;

    try {
      const newToken = await callRefreshToken();
      
      // Báo cho các request đang xếp hàng biết là đã refresh xong
      processQueue(null, newToken);
      
      // Gọi lại request ban đầu bị lỗi
      options.headers = {
        ...options.headers,
        'Authorization': `Bearer ${newToken}`
      };
      
      response = await fetch(buildUrl(endpoint), options);

    } catch (error) {
      // Nếu refresh thất bại (Refresh token cũng hết hạn)
      processQueue(error as Error, null);
      
      // Bắn event báo hiệu Session thực sự kết thúc
      window.dispatchEvent(new CustomEvent('unauthorized-access', { 
        detail: { status: 401, isSessionExpired: true } 
      }));
      
      throw new Error('Phiên đăng nhập đã hết hạn');
    } finally {
      // Mở khóa refresh
      isRefreshing = false;
    }
  }

  return handleResponse(response);
};

export const apiClient = {
  get: (endpoint: string) => customFetch(endpoint, { method: 'GET', credentials: 'omit' }), // Đổi include -> omit nếu không dùng cookie

  post: (endpoint: string, data: any) => customFetch(endpoint, {
    method: 'POST',
    credentials: 'omit',
    body: JSON.stringify(data),
  }),

  put: (endpoint: string, data: any) => customFetch(endpoint, {
    method: 'PUT',
    credentials: 'omit',
    body: JSON.stringify(data),
  }),

  delete: (endpoint: string) => customFetch(endpoint, { method: 'DELETE', credentials: 'omit' }),

  putFormData: (endpoint: string, formData: FormData) => customFetch(endpoint, {
    method: 'PUT',
    credentials: 'omit',
    body: formData,
  }),
};