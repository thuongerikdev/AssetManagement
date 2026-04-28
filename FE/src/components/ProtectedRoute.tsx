// src/components/ProtectedRoute.tsx
import { Navigate, Outlet, useLocation } from 'react-router';
import { useAuth } from './AuthContext';

export function ProtectedRoute() {
  const { isAuthenticated } = useAuth();
  const location = useLocation();

  if (!isAuthenticated) {
    // Nếu chưa đăng nhập, đá về /login và lưu lại đường dẫn cũ (để login xong quay lại được nếu cần)
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  // Nếu đã đăng nhập, render các component con (chính là Layout chứa giao diện app)
  return <Outlet />;
}