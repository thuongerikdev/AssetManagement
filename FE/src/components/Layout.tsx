import { Outlet, Link, useLocation } from 'react-router';
import { 
  LayoutDashboard, 
  Package, 
  TrendingDown, 
  ArrowLeftRight, 
  Wrench, 
  Trash2, 
  FileText, 
  BarChart3, 
  Settings,
  Settings2, 
  Building2, 
  ChevronDown, 
  Bell,
  User,
  LogOut,
  Menu,
  FolderTree,
  BookOpen,
  X,
  MonitorSmartphone,
  RefreshCw
} from 'lucide-react';
import { useState, useEffect } from 'react';

// BẮT BUỘC PHẢI CÓ IMPORT NÀY (Đảm bảo bạn đã tạo file src/context/GlobalContext.tsx nhé)
import { GlobalProvider, useGlobalData } from '../context/GlobalContext';

interface NavItem {
  path: string;
  label: string;
  icon: React.ElementType;
}

const navItems: NavItem[] = [
  { path: '/', label: 'Dashboard', icon: LayoutDashboard },
  { path: '/assets', label: 'Quản lý Tài sản', icon: Package },
  { path: '/depreciation', label: 'Khấu hao', icon: TrendingDown },
  { path: '/allocation', label: 'Cấp phát & Điều chuyển', icon: ArrowLeftRight },
  { path: '/maintenance', label: 'Bảo trì - Bảo dưỡng', icon: Wrench },
  { path: '/liquidation', label: 'Thanh lý', icon: Trash2 },
  { path: '/vouchers', label: 'Chứng từ Kế toán', icon: FileText },
  { path: '/reports', label: 'Báo cáo', icon: BarChart3 },
  { path: '/my-assets', label: 'Tài sản của tôi', icon: MonitorSmartphone },
];

// TÁCH PHẦN RUỘT RA ĐỂ DÙNG ĐƯỢC KHO DỮ LIỆU (useGlobalData)
function LayoutInner() {
  const location = useLocation();
  const [sidebarOpen, setSidebarOpen] = useState(true);

  const isSettingsActive = location.pathname.startsWith('/settings');
  const [isSettingsOpen, setIsSettingsOpen] = useState(isSettingsActive);

  // === MÓC DỮ LIỆU TỪ KHO CHUNG RA ĐÂY ===
  // refreshData sẽ gọi API ngầm, không làm chớp trang
  const { isLoadingGlobal, refreshData } = useGlobalData();

  useEffect(() => {
    if (location.pathname.startsWith('/settings')) {
      setIsSettingsOpen(true);
    }
  }, [location.pathname]);

  return (
    <div className="flex h-screen bg-gray-50">
      {/* Sidebar */}
      <aside 
        className={`relative bg-gradient-to-b from-blue-900 to-blue-800 text-white transition-all duration-300 ${
          sidebarOpen ? 'w-64' : 'w-0'
        } overflow-hidden shrink-0 flex flex-col`}
      >
        {/* Nút Đóng (X) được đưa vào trong Sidebar */}
        <div className="p-4 border-b border-blue-700 flex items-center justify-between">
          <div className="overflow-hidden">
            <h1 className="font-bold text-xl whitespace-nowrap">TSCĐ Manager</h1>
            <p className="text-blue-200 text-sm mt-1 whitespace-nowrap">Quản lý Tài sản Cố định</p>
          </div>
          <button
            onClick={() => setSidebarOpen(false)}
            className="text-blue-300 hover:text-white transition-colors focus:outline-none shrink-0"
            title="Đóng menu"
          >
            <X className="w-6 h-6" />
          </button>
        </div>
        
        <nav className="p-4 space-y-1 flex-1 overflow-y-auto no-scrollbar">
          {/* Các menu chính */}
          {navItems.map((item) => {
            const Icon = item.icon;
            const isActive = location.pathname === item.path || 
              (item.path !== '/' && location.pathname.startsWith(item.path));
            
            return (
              <Link
                key={item.path}
                to={item.path}
                className={`flex items-center gap-3 px-4 py-3 rounded-lg transition-all whitespace-nowrap ${
                  isActive 
                    ? 'bg-white text-blue-900 font-medium shadow-sm' 
                    : 'text-blue-100 hover:bg-blue-800'
                }`}
              >
                <Icon className="w-5 h-5 shrink-0" />
                <span>{item.label}</span>
              </Link>
            );
          })}

          {/* Menu Dropdown: Cấu hình */}
          <div className="pt-2">
            <button
              onClick={() => setIsSettingsOpen(!isSettingsOpen)}
              className={`w-full flex items-center justify-between px-4 py-3 rounded-lg transition-all ${
                isSettingsActive ? 'bg-blue-800/50 text-white font-medium' : 'text-blue-100 hover:bg-blue-800'
              }`}
            >
              <div className="flex items-center gap-3 whitespace-nowrap">
                <Settings2 className="w-5 h-5 shrink-0" />
                <span>Cấu hình</span>
              </div>
              <ChevronDown className={`w-4 h-4 shrink-0 transition-transform ${isSettingsOpen ? 'rotate-180' : ''}`} />
            </button>
            
            {/* Menu con của Cấu hình */}
            <div 
              className={`overflow-hidden transition-all duration-300 ${
                isSettingsOpen ? 'max-h-60 mt-1 opacity-100' : 'max-h-0 opacity-0'
              }`}
            >
              <div className="ml-4 pl-4 border-l border-blue-700/50 space-y-1">
                <Link
                  to="/settings/system"
                  className={`flex items-center gap-3 px-4 py-2 rounded-lg transition-all text-sm whitespace-nowrap ${
                    location.pathname === '/settings/system' 
                      ? 'bg-white text-blue-900 font-medium shadow-sm' 
                      : 'text-blue-200 hover:bg-blue-800 hover:text-white'
                  }`}
                >
                  <Settings className="w-4 h-4 shrink-0" />
                  <span>Hệ thống chung</span>
                </Link>

                <Link
                  to="/settings/departments"
                  className={`flex items-center gap-3 px-4 py-2 rounded-lg transition-all text-sm whitespace-nowrap ${
                    location.pathname === '/settings/departments' 
                      ? 'bg-white text-blue-900 font-medium shadow-sm' 
                      : 'text-blue-200 hover:bg-blue-800 hover:text-white'
                  }`}
                >
                  <Building2 className="w-4 h-4 shrink-0" />
                  <span>Phòng ban</span>
                </Link>

                <Link
                  to="/settings/categories"
                  className={`flex items-center gap-3 px-4 py-2 rounded-lg transition-all text-sm whitespace-nowrap ${
                    location.pathname === '/settings/categories' 
                      ? 'bg-white text-blue-900 font-medium shadow-sm' 
                      : 'text-blue-200 hover:bg-blue-800 hover:text-white'
                  }`}
                >
                  <FolderTree className="w-4 h-4 shrink-0" />
                  <span>Danh mục Tài sản</span>
                </Link>

                <Link
                  to="/settings/accounts"
                  className={`flex items-center gap-3 px-4 py-2 rounded-lg transition-all text-sm whitespace-nowrap ${
                    location.pathname === '/settings/accounts' 
                      ? 'bg-white text-blue-900 font-medium shadow-sm' 
                      : 'text-blue-200 hover:bg-blue-800 hover:text-white'
                  }`}
                >
                  <BookOpen className="w-4 h-4 shrink-0" />
                  <span>Tài khoản Kế toán</span>
                </Link>
              </div>
            </div>
          </div>
        </nav>

        <div className="p-4 border-t border-blue-700 shrink-0">
          <div className="flex items-center gap-3 px-4 py-2 whitespace-nowrap">
            <div className="w-8 h-8 rounded-full bg-blue-700 flex items-center justify-center shrink-0">
              <User className="w-4 h-4" />
            </div>
            <div className="flex-1 overflow-hidden">
              <p className="text-sm font-medium truncate">Nguyễn Văn A</p>
              <p className="text-xs text-blue-200 truncate">Kế toán trưởng</p>
            </div>
            <button className="text-blue-200 hover:text-white shrink-0">
              <LogOut className="w-4 h-4" />
            </button>
          </div>
        </div>
      </aside>

      {/* Main Content */}
      <div className="flex-1 flex flex-col min-w-0">
        {/* Header */}
        <header className="bg-white border-b border-gray-200 px-6 py-4 shrink-0">
          <div className="flex items-center justify-between">
            
            {/* Khu vực bên trái: Nút Menu nằm ngoài, CHỈ hiện khi Sidebar đóng */}
            <div className="flex items-center">
              {!sidebarOpen && (
                <button
                  onClick={() => setSidebarOpen(true)}
                  className="text-gray-600 hover:text-gray-900 focus:outline-none mr-4 transition-all"
                  title="Mở menu"
                >
                  <Menu className="w-6 h-6" />
                </button>
              )}
            </div>
            
            <div className="flex items-center gap-5 ml-auto">
              {/* === NÚT LÀM MỚI SIÊU MƯỢT (KHÔNG DÙNG WINDOW.RELOAD) === */}
              <button 
                onClick={() => refreshData()}
                disabled={isLoadingGlobal}
                className="flex items-center gap-2 text-sm font-medium text-gray-600 hover:text-blue-600 bg-gray-100 hover:bg-blue-50 px-3 py-1.5 rounded-lg transition-colors border border-gray-200 hover:border-blue-200 disabled:opacity-50"
                title="Tải lại dữ liệu hệ thống"
              >
                <RefreshCw className={`w-4 h-4 ${isLoadingGlobal ? 'animate-spin text-blue-600' : ''}`} />
                <span className="hidden sm:block">{isLoadingGlobal ? 'Đang đồng bộ...' : 'Làm mới'}</span>
              </button>

              <button className="relative text-gray-600 hover:text-gray-900 focus:outline-none">
                <Bell className="w-5 h-5" />
                <span className="absolute -top-1 -right-1 w-4 h-4 bg-red-500 rounded-full text-white text-xs flex items-center justify-center">
                  3
                </span>
              </button>
              <div className="text-right hidden sm:block border-l border-gray-200 pl-4 ml-1">
                <p className="text-sm font-medium text-gray-900">Công ty TNHH ABC</p>
                <p className="text-xs text-gray-500">Hôm nay: {new Date().toLocaleDateString('vi-VN')}</p>
              </div>
            </div>
          </div>
        </header>

        {/* Page Content */}
        <main className="flex-1 overflow-x-hidden overflow-y-auto bg-gray-50 p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
}

// BỌC LAYOUT INNER BẰNG GLOBAL PROVIDER ĐỂ CUNG CẤP DỮ LIỆU TOÀN CỤC
export function Layout() {
  return (
    <GlobalProvider>
      <LayoutInner />
    </GlobalProvider>
  );
}