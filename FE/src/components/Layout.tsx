import { Outlet, Link, useLocation, useNavigate } from 'react-router';
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
  BookOpenCheck,
  MonitorSmartphone,
  RefreshCw,
  Shield,
  Key
} from 'lucide-react';
import { useState, useEffect } from 'react';

// Import Global Context
import { GlobalProvider, useGlobalData } from '../context/GlobalContext';

// Import useAuth từ AuthContext để gọi hàm logout
import { useAuth } from './AuthContext';
import { systemConfigApi } from '../api/systemConfigApi';

// 1. THÊM requiredPermission VÀO INTERFACE
interface NavItem {
  path: string;
  label: string;
  icon: React.ElementType;
  requiredPermission?: string; // Mã code quyền, VD: 'tai_san.get_all'. Nếu rỗng = ai cũng xem được
}

// 2. GẮN MÃ QUYỀN VÀO TỪNG MENU DỰA TRÊN DATA BẠN CUNG CẤP
const navItems: NavItem[] = [
  { path: '/', label: 'Dashboard', icon: LayoutDashboard,requiredPermission: 'tai_san.get_all' }, // Không yêu cầu quyền
  { path: '/assets', label: 'Quản lý Tài sản', icon: Package, requiredPermission: 'tai_san.get_all' },
  { path: '/allocation', label: 'Cấp phát & Điều chuyển', icon: ArrowLeftRight, requiredPermission: 'dieu_chuyen_tai_san.get_all' },
  { path: '/depreciation', label: 'Khấu hao', icon: TrendingDown, requiredPermission: 'lich_su_khau_hao.get_all' },
  { path: '/maintenance', label: 'Bảo trì - Sửa chữa', icon: Wrench, requiredPermission: 'bao_tri_tai_san.get_all' },
  { path: '/liquidation', label: 'Thanh lý', icon: Trash2, requiredPermission: 'thanh_ly_tai_san.get_all' },
  { path: '/vouchers', label: 'Chứng từ Kế toán', icon: FileText, requiredPermission: 'chung_tu.get_all' },
  { path: '/so-cai', label: 'Sổ Cái', icon: BookOpen, requiredPermission: 'so_cai.get_tom_tat' },
  { path: '/ledger', label: 'Nhật ký chung', icon: BookOpenCheck, requiredPermission: 'so_cai.get_chi_tiet' },
  { path: '/reports', label: 'Báo cáo', icon: BarChart3 }, // Có thể bổ sung quyền báo cáo sau
  { path: '/my-assets', label: 'Tài sản của tôi', icon: MonitorSmartphone, requiredPermission: 'tai_san.get_mine' },
];

function LayoutInner() {
  const location = useLocation();
  const navigate = useNavigate();
  const [sidebarOpen, setSidebarOpen] = useState(true);
  const [companyName, setCompanyName] = useState('TSCĐ Manager');

  const isSettingsActive = location.pathname.startsWith('/settings');
  const [isSettingsOpen, setIsSettingsOpen] = useState(isSettingsActive);

  const { isLoadingGlobal, refreshData } = useGlobalData();
  const { logout } = useAuth();

  useEffect(() => {
    systemConfigApi.get().then(res => {
      if (res.errorCode === 200 && res.data?.tenCongTy) {
        setCompanyName(res.data.tenCongTy);
      }
    }).catch(() => {});
  }, []);

  const handleLogout = () => {
    logout(); 
    localStorage.removeItem('access_token'); 
    localStorage.removeItem('user_info'); 
    navigate('/login', { replace: true }); 
  };

  useEffect(() => {
    if (location.pathname.startsWith('/settings')) {
      setIsSettingsOpen(true);
    }
  }, [location.pathname]);

  // 3. ĐỌC THÔNG TIN USER VÀ MẢNG PERMISSIONS
  const userInfoStr = localStorage.getItem('user_info');
  const userInfo = userInfoStr 
    ? JSON.parse(userInfoStr) 
    : { userName: 'Người dùng', email: 'Đang tải...', permissions: [] };

  const userPermissions: string[] = userInfo.permissions || [];

  // HÀM KIỂM TRA QUYỀN
  const hasPermission = (permissionCode?: string) => {
    if (!permissionCode) return true; // Nếu menu không cấu hình quyền -> Ai cũng xem được
    return userPermissions.includes(permissionCode);
  };

  // KIỂM TRA QUYỀN CHO CÁC MENU CON TRONG MỤC "CẤU HÌNH"
  const canViewSystem = hasPermission('cau_hinh_he_thong.get');
  const canViewDepartments = hasPermission('phong_ban.get_all');
  const canViewCategories = hasPermission('danh_muc_tai_san.get_all');
  const canViewAccounts = hasPermission('tai_khoan_ke_toan.get_all');
  // Check quyền xem user (Có thể dùng user.get_all hoặc permission tương đương)
  const canViewUsers = hasPermission('user.admin_get_all') || hasPermission('user.get_all_slim');
  const canViewRoles = hasPermission('role.get_all');
  const canViewPermissions = hasPermission('permission.admin_get_all');

  // Nếu user không có bất kỳ quyền cấu hình nào -> Ẩn luôn menu cha "Cấu hình"
  const canViewSettingsMenu = canViewSystem || canViewDepartments || canViewCategories || canViewAccounts || canViewUsers || canViewRoles || canViewPermissions;

  return (
    <div className="flex h-screen bg-gray-50">
      {/* Sidebar */}
      <aside 
        className={`relative bg-gradient-to-b from-blue-900 to-blue-800 text-white transition-all duration-300 ${
          sidebarOpen ? 'w-64' : 'w-0'
        } overflow-hidden shrink-0 flex flex-col`}
      >
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
          {/* 4. LỌC DANH SÁCH MENU CHÍNH BẰNG HÀM hasPermission */}
          {navItems.filter(item => hasPermission(item.requiredPermission)).map((item) => {
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

          {/* Menu Dropdown: Cấu hình (Chỉ hiện khi có ít nhất 1 quyền con) */}
          {canViewSettingsMenu && (
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
              
              <div 
                className={`overflow-hidden transition-all duration-300 ${
                  isSettingsOpen ? 'max-h-96 mt-1 opacity-100' : 'max-h-0 opacity-0'
                }`}
              >
                <div className="ml-4 pl-4 border-l border-blue-700/50 space-y-1">
                  {/* Từng mục cấu hình cũng check quyền tương ứng */}
                  {canViewSystem && (
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
                  )}

                  {canViewDepartments && (
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
                  )}

                  {canViewCategories && (
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
                  )}

                  {canViewAccounts && (
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
                  )}

                  {canViewUsers && (
                    <Link
                      to="/settings/users"
                      className={`flex items-center gap-3 px-4 py-2 rounded-lg transition-all text-sm whitespace-nowrap ${
                        location.pathname.startsWith('/settings/users')
                          ? 'bg-white text-blue-900 font-medium shadow-sm' 
                          : 'text-blue-200 hover:bg-blue-800 hover:text-white'
                      }`}
                    >
                      <User className="w-4 h-4 shrink-0" />
                      <span>Người dùng</span>
                    </Link>
                  )}

                  {canViewRoles && (
                    <Link
                      to="/settings/roles"
                      className={`flex items-center gap-3 px-4 py-2 rounded-lg transition-all text-sm whitespace-nowrap ${
                        location.pathname.startsWith('/settings/roles')
                          ? 'bg-white text-blue-900 font-medium shadow-sm' 
                          : 'text-blue-200 hover:bg-blue-800 hover:text-white'
                      }`}
                    >
                      <Shield className="w-4 h-4 shrink-0" />
                      <span>Vai trò</span>
                    </Link>
                  )}

                  {canViewPermissions && (
                    <Link
                      to="/settings/permissions"
                      className={`flex items-center gap-3 px-4 py-2 rounded-lg transition-all text-sm whitespace-nowrap ${
                        location.pathname.startsWith('/settings/permissions')
                          ? 'bg-white text-blue-900 font-medium shadow-sm' 
                          : 'text-blue-200 hover:bg-blue-800 hover:text-white'
                      }`}
                    >
                      <Key className="w-4 h-4 shrink-0" />
                      <span>Quyền hạn</span>
                    </Link>
                  )}
                </div>
              </div>
            </div>
          )}
        </nav>

        {/* THÔNG TIN NGƯỜI DÙNG & ĐĂNG XUẤT */}
        <div className="p-4 border-t border-blue-700 shrink-0">
          <div className="flex items-center gap-3 px-4 py-2 whitespace-nowrap">
            <div className="w-8 h-8 rounded-full bg-blue-700 flex items-center justify-center shrink-0">
              <User className="w-4 h-4" />
            </div>
            <div className="flex-1 overflow-hidden">
              <p className="text-sm font-medium truncate" title={userInfo.userName}>
                {userInfo.userName}
              </p>
              <p className="text-xs text-blue-200 truncate" title={userInfo.email}>
                {userInfo.email}
              </p>
            </div>
            <button 
              onClick={handleLogout}
              className="text-blue-200 hover:text-white hover:bg-blue-800 shrink-0 p-1.5 rounded transition-colors"
              title="Đăng xuất"
            >
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
                <p className="text-sm font-medium text-gray-900">{companyName}</p>
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

// BỌC LAYOUT INNER BẰNG GLOBAL PROVIDER
export function Layout() {
  return (
    <GlobalProvider>
      <LayoutInner />
    </GlobalProvider>
  );
}