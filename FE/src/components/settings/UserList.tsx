import { useState, useEffect, useMemo } from 'react';
import { 
  Users, Search, Trash2, ShieldCheck, 
  XCircle, CheckCircle2, Loader2, Mail, RefreshCw
} from 'lucide-react';
import { toast } from 'sonner';
import { authApi } from '../../api/authApi';

// ==========================================
// 1. BIẾN CACHE CỤC BỘ TRÊN RAM (Chỉ lưu Người dùng)
// ==========================================
let cachedUsers: any[] | null = null;

export function UserList() {
  // State quản lý danh sách Users
  const [users, setUsers] = useState<any[]>(cachedUsers || []);
  const [loading, setLoading] = useState(!cachedUsers);
  const [searchText, setSearchText] = useState('');

  // ==========================================
  // 2. HÀM TẢI DỮ LIỆU CÓ TÍCH HỢP CACHE
  // ==========================================
  const fetchUsers = async (forceRefresh = false) => {
    if (!forceRefresh && cachedUsers) {
      setUsers(cachedUsers);
      return;
    }

    setLoading(true);
    try {
      const response = await authApi.getAllUsers();
      if (response.errorCode === 200 && response.data) {
        const transformedData = response.data.map((user: any) => ({
          ...user,
          fullName: `${user.profile?.firstName || ''} ${user.profile?.lastName || ''}`.trim() || user.userName,
          sessionsCount: user.sessions?.length || 0,
        }));
        cachedUsers = transformedData; // Lưu vào Cache
        setUsers(transformedData);
      }
    } catch (error) {
      toast.error('Lỗi khi tải danh sách User');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  // ==========================================
  // 3. USE_MEMO BỘ LỌC TÌM KIẾM TỨC THÌ
  // ==========================================
  const filteredUsers = useMemo(() => {
    return users.filter((u) => {
      const lower = searchText.toLowerCase();
      return (
        u.fullName.toLowerCase().includes(lower) ||
        u.email.toLowerCase().includes(lower) ||
        u.userName.toLowerCase().includes(lower)
      );
    });
  }, [users, searchText]);

  const handleDelete = async (userId: number) => {
    if (window.confirm("Bạn có chắc chắn muốn xóa user này? Hành động này không thể hoàn tác.")) {
      try {
        const response = await authApi.deleteUser(userId);
        if (response.errorCode === 200) {
          toast.success("Xóa user thành công!");
          fetchUsers(true); // Ép tải lại dữ liệu mới nhất sau khi xóa
        } else {
          toast.error(response.errorMessage || "Xóa user thất bại");
        }
      } catch (error) {
        toast.error("Có lỗi xảy ra khi xóa user");
      }
    }
  };

  return (
    <div className="space-y-6 max-w-7xl mx-auto">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 flex items-center gap-2">
            <Users className="w-6 h-6 text-indigo-600" /> Quản lý Người dùng
          </h1>
          <p className="text-sm text-gray-500 mt-1">Danh sách tài khoản người dùng và nhân viên hệ thống</p>
        </div>
        
        {/* Nút Làm mới Data */}
        <button 
          onClick={() => fetchUsers(true)}
          disabled={loading}
          className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-gray-600 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors disabled:opacity-50 shadow-sm"
          title="Tải lại dữ liệu"
        >
          <RefreshCw className={`w-4 h-4 ${loading ? 'animate-spin text-indigo-600' : ''}`} />
          <span className="hidden sm:block">Làm mới</span>
        </button>
      </div>

      {/* Toolbar */}
      <div className="bg-white p-4 rounded-lg border border-gray-200 shadow-sm flex items-center gap-4">
        <div className="relative flex-1 max-w-md">
          <Search className="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
          <input 
            type="text" 
            placeholder="Tìm kiếm theo tên, email, username..." 
            value={searchText}
            onChange={(e) => setSearchText(e.target.value)}
            className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-indigo-500 outline-none transition-all"
          />
        </div>
      </div>

      {/* Table */}
      <div className="bg-white rounded-lg border border-gray-200 shadow-sm overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full text-left text-sm text-gray-600">
            <thead className="bg-gray-50 border-b border-gray-200 text-gray-900 font-medium">
              <tr>
                <th className="px-4 py-3 text-center w-16">ID</th>
                <th className="px-4 py-3">Họ và Tên</th>
                <th className="px-4 py-3">Tài khoản / Email</th>
                <th className="px-4 py-3 text-center">Xác thực</th>
                <th className="px-4 py-3 text-center">Trạng thái</th>
                <th className="px-4 py-3">Vai trò</th>
                <th className="px-4 py-3 text-center w-24">Hành động</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200">
              {loading && filteredUsers.length === 0 ? (
                <tr>
                  <td colSpan={7} className="px-4 py-10 text-center text-gray-500">
                    <Loader2 className="w-8 h-8 animate-spin mx-auto mb-2 text-indigo-600" />
                    Đang tải dữ liệu người dùng...
                  </td>
                </tr>
              ) : filteredUsers.length === 0 ? (
                <tr>
                  <td colSpan={7} className="px-4 py-8 text-center text-gray-500">Không tìm thấy người dùng nào.</td>
                </tr>
              ) : (
                filteredUsers.map((row) => (
                  <tr key={row.userID} className="hover:bg-gray-50 transition-colors">
                    <td className="px-4 py-3 text-center font-medium text-gray-900">{row.userID}</td>
                    <td className="px-4 py-3">
                      <div className="font-medium text-gray-900">{row.fullName}</div>
                      <div className="text-xs text-gray-500">@{row.userName}</div>
                    </td>
                    <td className="px-4 py-3">
                      <div className="flex items-center gap-2 text-sm">
                        <Mail className="w-4 h-4 text-gray-400" />
                        {row.email}
                      </div>
                    </td>
                   <td className="px-4 py-3 text-center">
                      {row.isEmailVerified ? (
                        <span title="Đã xác thực">
                          <CheckCircle2 className="w-5 h-5 text-green-500 mx-auto" />
                        </span>
                      ) : (
                        <span title="Chưa xác thực">
                          <XCircle className="w-5 h-5 text-red-500 mx-auto" />
                        </span>
                      )}
                    </td>
                    <td className="px-4 py-3 text-center">
                      <span className={`px-2 py-1 text-xs font-medium rounded-full ${
                        row.status === 'Active' ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'
                      }`}>
                        {row.status}
                      </span>
                    </td>
                    <td className="px-4 py-3">
                      <div className="flex flex-wrap gap-1">
                        {row.roles?.slice(0, 2).map((role: string, idx: number) => (
                          <span 
                            key={idx} 
                            className={`px-2 py-1 text-xs font-medium rounded border ${
                              role.toLowerCase().includes('admin') 
                                ? 'bg-red-50 text-red-700 border-red-200' 
                                : 'bg-blue-50 text-blue-700 border-blue-200'
                            }`}
                          >
                            {role}
                          </span>
                        ))}
                        {row.roles?.length > 2 && (
                          <span className="px-2 py-1 text-xs font-medium bg-gray-100 text-gray-600 rounded border border-gray-200" title={row.roles.slice(2).join(', ')}>
                            +{row.roles.length - 2}
                          </span>
                        )}
                        {(!row.roles || row.roles.length === 0) && <span className="text-gray-400 italic text-xs">Chưa có</span>}
                      </div>
                    </td>
                    <td className="px-4 py-3 text-center">
                      <button 
                        onClick={() => handleDelete(row.userID)} 
                        className="p-1.5 text-gray-500 hover:text-red-600 hover:bg-red-50 rounded transition-colors" 
                        title="Xóa tài khoản"
                      >
                        <Trash2 className="w-4 h-4" />
                      </button>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}