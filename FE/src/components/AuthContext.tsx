// src/context/AuthContext.tsx
import { createContext, useContext, useEffect, useState, ReactNode } from 'react';
import { ShieldAlert } from 'lucide-react';
import { useNavigate } from 'react-router'; // Cần thêm useNavigate nếu muốn đá về /login

interface AuthContextType {
  isAuthenticated: boolean;
  login: () => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(() => {
    return !!localStorage.getItem('access_token'); 
  });
  
  const [showUnauthorizedModal, setShowUnauthorizedModal] = useState(false);
  const [isSessionExpired, setIsSessionExpired] = useState(false); // Thêm state này

  const login = () => {
    setIsAuthenticated(true);
  };

  const logout = () => {
    setIsAuthenticated(false);
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token'); // <-- THÊM DÒNG NÀY
    localStorage.removeItem('user_info');
    setShowUnauthorizedModal(false);
    setIsSessionExpired(false);
  };

  useEffect(() => {
    const handleUnauthorized = (event: any) => {
      // Nhận event từ apiClient báo về
      if (event.detail?.isSessionExpired) {
        // Trường hợp Refresh Token cũng chết -> Ép văng ra ngoài
        setIsSessionExpired(true);
        logout(); // Xóa sạch local storage
      } else {
        // Trường hợp không có quyền (403)
        setShowUnauthorizedModal(true);
      }
    };

    window.addEventListener('unauthorized-access', handleUnauthorized);
    return () => {
      window.removeEventListener('unauthorized-access', handleUnauthorized);
    };
  }, []);

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
      {children}

      {/* Modal Cảnh báo Không có quyền */}
      {showUnauthorizedModal && (
        <div className="fixed inset-0 z-[9999] flex items-center justify-center bg-black/50 backdrop-blur-sm px-4">
          <div className="bg-white rounded-xl shadow-2xl w-full max-w-md p-6 animate-in zoom-in-95">
            <div className="flex flex-col items-center text-center">
              <div className="w-16 h-16 bg-red-100 text-red-600 rounded-full flex items-center justify-center mb-4">
                <ShieldAlert className="w-8 h-8" />
              </div>
              <h3 className="text-xl font-bold text-gray-900 mb-2">Truy cập bị từ chối!</h3>
              <p className="text-gray-600 mb-6">
                Bạn không có quyền thực hiện thao tác này hoặc truy cập dữ liệu này. Vui lòng liên hệ Quản trị viên nếu cần cấp quyền.
              </p>
              <button
                onClick={() => setShowUnauthorizedModal(false)}
                className="w-full bg-blue-600 hover:bg-blue-700 text-white font-medium py-2.5 rounded-lg transition-colors"
              >
                Đã hiểu và Đóng
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Modal Hết hạn phiên đăng nhập */}
      {isSessionExpired && (
        <div className="fixed inset-0 z-[10000] flex items-center justify-center bg-black/50 backdrop-blur-sm px-4">
          <div className="bg-white rounded-xl shadow-2xl w-full max-w-md p-6 animate-in zoom-in-95">
            <div className="flex flex-col items-center text-center">
              <div className="w-16 h-16 bg-yellow-100 text-yellow-600 rounded-full flex items-center justify-center mb-4">
                <ShieldAlert className="w-8 h-8" />
              </div>
              <h3 className="text-xl font-bold text-gray-900 mb-2">Phiên đăng nhập hết hạn</h3>
              <p className="text-gray-600 mb-6">
                Phiên làm việc của bạn đã kết thúc. Vui lòng đăng nhập lại để tiếp tục.
              </p>
              <button
                onClick={() => {
                  setIsSessionExpired(false);
                  window.location.href = '/login'; // F5 chuyển thẳng về login
                }}
                className="w-full bg-blue-600 hover:bg-blue-700 text-white font-medium py-2.5 rounded-lg transition-colors"
              >
                Đăng nhập lại
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