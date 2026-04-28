// src/App.tsx
import { RouterProvider } from 'react-router';
import { router } from './routes'; // Thay bằng đường dẫn file router thực tế của bạn
import { Toaster } from './components/ui/sonner';
import { AuthProvider } from './components/AuthContext';

export default function App() {
  return (
    <AuthProvider>
      <RouterProvider router={router} />
      <Toaster position="top-right" richColors />
    </AuthProvider>
  );
}