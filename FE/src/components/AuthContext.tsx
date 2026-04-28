// src/context/AuthContext.tsx
import { createContext, useContext, useEffect, useState, ReactNode } from 'react';
import { ShieldAlert } from 'lucide-react';

interface AuthContextType {
  isAuthenticated: boolean;
  login: () => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  // Trạng thái đăng nhập (Có thể lưu 1 flag nhỏ ở localStorage để biết user đã từng login)
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(() => {
  // Thay đổi: Kiểm tra thẳng xem có access_token không
  return !!localStorage.getItem('access_token'); 
  });

  // Trạng thái hiển thị Modal 401
  const [showUnauthorizedModal, setShowUnauthorizedModal] = useState(false);

  const login = () => {
  setIsAuthenticated(true);
  // Bỏ dòng localStorage.setItem('is_logged_in', 'true') đi vì không cần thiết nữa
};

const logout = () => {
  setIsAuthenticated(false);
  // Xoá token ở đây luôn cho an toàn
  localStorage.removeItem('access_token');
  localStorage.removeItem('user_info');
  setShowUnauthorizedModal(false);
};

  useEffect(() => {
    // Lắng nghe sự kiện 401 từ apiClient
    const handleUnauthorized = () => {
      setShowUnauthorizedModal(true);
      // Bạn có thể chọn tự động logout tại đây:
      // logout(); 
    };

    window.addEventListener('unauthorized-access', handleUnauthorized);
    return () => {
      window.removeEventListener('unauthorized-access', handleUnauthorized);
    };
  }, []);

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
      {children}

      {/* Modal Cảnh báo 401 */}
      {showUnauthorizedModal && (
        <div className="fixed inset-0 z-[9999] flex items-center justify-center bg-black/50 backdrop-blur-sm px-4">
          <div className="bg-white rounded-xl shadow-2xl w-full max-w-md p-6 animate-in zoom-in-95">
            <div className="flex flex-col items-center text-center">
              <div className="w-16 h-16 bg-red-100 text-red-600 rounded-full flex items-center justify-center mb-4">
                <ShieldAlert className="w-8 h-8" />
              </div>
              <h3 className="text-xl font-bold text-gray-900 mb-2">
                Phiên đăng nhập hết hạn hoặc không có quyền!
              </h3>
              <p className="text-gray-600 mb-6">
                Bạn không có quyền truy cập dữ liệu này hoặc phiên làm việc đã kết thúc. Vui lòng đăng nhập lại.
              </p>
              <button
                onClick={() => {
                  logout(); // Xóa state login
                  window.location.href = '/login'; // Force load lại về trang login
                }}
                className="w-full bg-blue-600 hover:bg-blue-700 text-white font-medium py-2.5 rounded-lg transition-colors"
              >
                Đến trang Đăng nhập
              </button>
            </div>
          </div>
        </div>
      )}
    </AuthContext.Provider>
  );
}

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error('useAuth must be used within an AuthProvider');
  return context;
};