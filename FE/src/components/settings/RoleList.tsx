import { useState, useEffect, useMemo } from 'react';
import { 
  Shield, Plus, Search, Edit2, Trash2, Copy, 
  Eye, Loader2, X, CheckSquare, Square, RefreshCw, Save
} from 'lucide-react';
import { toast } from 'sonner';
import { authApi } from '../../api/authApi';

// ==========================================
// 1. BIẾN CACHE CỤC BỘ TRÊN RAM
// ==========================================
let cachedRoles: any[] | null = null;

export function RoleList() {
  // State quản lý danh sách Role
  const [roles, setRoles] = useState<any[]>(cachedRoles || []);
  const [loading, setLoading] = useState(!cachedRoles);
  const [searchText, setSearchText] = useState('');
  
  // Dialog States
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isCloneModalOpen, setIsCloneModalOpen] = useState(false);
  const [dialogMode, setDialogMode] = useState<'add' | 'edit'>('add');
  const [isSaving, setIsSaving] = useState(false);
  
  const [currentRole, setCurrentRole] = useState({
    roleID: 0,
    roleName: '',
    roleDescription: '',
    scope: 'user',
    isDefault: false,
  });

  const [cloneData, setCloneData] = useState({
    sourceRoleId: 0,
    newRoleName: '',
    newRoleDescription: '',
    newScope: 'user',
    isDefault: false,
  });

  // ==========================================
  // 2. HÀM TẢI DỮ LIỆU CÓ TÍCH HỢP CACHE
  // ==========================================
  const fetchRoles = async (forceRefresh = false) => {
    if (!forceRefresh && cachedRoles) {
      setRoles(cachedRoles);
      return;
    }

    setLoading(true);
    try {
      const response = await authApi.getAllRoles();
      if (response.errorCode === 200 && response.data) {
        cachedRoles = response.data; // Lưu vào Cache
        setRoles(response.data);
      } else {
        toast.error(response.errorMessage || 'Lỗi khi tải danh sách Role');
      }
    } catch (error) {
      toast.error('Lỗi kết nối khi tải danh sách Role');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchRoles();
  }, []);

  // ==========================================
  // 3. USE_MEMO BỘ LỌC TÌM KIẾM TỨC THÌ
  // ==========================================
  const filteredRoles = useMemo(() => {
    const lower = searchText.toLowerCase();
    return roles.filter(r => 
      r.roleName.toLowerCase().includes(lower) || 
      (r.roleDescription && r.roleDescription.toLowerCase().includes(lower))
    );
  }, [roles, searchText]);

  // Handlers
  const handleOpenModal = (mode: 'add' | 'edit', role: any = null) => {
    setDialogMode(mode);
    if (mode === 'edit' && role) {
      setCurrentRole(role);
    } else {
      setCurrentRole({ roleID: 0, roleName: '', roleDescription: '', scope: 'user', isDefault: false });
    }
    setIsModalOpen(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!currentRole.roleName) {
      toast.error("Vui lòng nhập Tên Role!");
      return;
    }
    
    setIsSaving(true);
    try {
      let response;
      if (dialogMode === 'add') {
        const { roleID, ...payload } = currentRole;
        response = await authApi.addRole(payload);
      } else {
        response = await authApi.updateRole(currentRole);
      }

      if (response.errorCode === 200 || response.errorCode === 201) {
        toast.success(dialogMode === 'add' ? 'Thêm Role thành công!' : 'Cập nhật thành công!');
        setIsModalOpen(false);
        fetchRoles(true); // Ép làm mới Cache sau khi thao tác thành công
      } else {
        toast.error(response.errorMessage || 'Có lỗi xảy ra!');
      }
    } catch (error) {
      toast.error('Lỗi kết nối đến máy chủ.');
    } finally {
      setIsSaving(false);
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Bạn có chắc chắn muốn xóa Role này?')) {
      try {
        const response = await authApi.deleteRole(id);
        if (response.errorCode === 200) {
          toast.success('Xóa Role thành công!');
          fetchRoles(true); // Ép làm mới Cache
        } else {
          toast.error(response.errorMessage || 'Xóa thất bại!');
        }
      } catch (error) {
        toast.error('Có lỗi xảy ra khi xóa.');
      }
    }
  };

  const handleCloneSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!cloneData.newRoleName) {
      toast.error("Vui lòng nhập Tên Role mới!");
      return;
    }

    setIsSaving(true);
    try {
      const response = await authApi.cloneRole(cloneData);
      if (response.errorCode === 200 || response.errorCode === 201) {
        toast.success("Sao chép Role thành công!");
        setIsCloneModalOpen(false);
        fetchRoles(true); // Ép làm mới Cache
      } else {
        toast.error(response.errorMessage || "Sao chép thất bại!");
      }
    } catch (error) {
      toast.error("Lỗi kết nối đến máy chủ.");
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <div className="space-y-6 max-w-7xl mx-auto">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 flex items-center gap-2">
            <Shield className="w-6 h-6 text-blue-600" /> Quản lý Vai trò (Roles)
          </h1>
          <p className="text-sm text-gray-500 mt-1">Phân quyền và cấu hình vai trò người dùng trong hệ thống</p>
        </div>
        
        <div className="flex gap-2">
          {/* Nút Làm mới Data */}
          <button 
            onClick={() => fetchRoles(true)}
            disabled={loading}
            className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-gray-600 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors disabled:opacity-50 shadow-sm"
            title="Tải lại dữ liệu"
          >
            <RefreshCw className={`w-4 h-4 ${loading ? 'animate-spin text-blue-600' : ''}`} />
            <span className="hidden sm:block">Làm mới</span>
          </button>

          <button 
            onClick={() => handleOpenModal('add')}
            className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-sm font-medium"
          >
            <Plus className="w-5 h-5" /> Thêm Role
          </button>
        </div>
      </div>

      {/* Toolbar */}
      <div className="bg-white p-4 rounded-xl border border-gray-200 shadow-sm">
        <div className="relative max-w-md">
          <Search className="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
          <input 
            type="text" 
            placeholder="Tìm kiếm Role theo tên, mô tả..." 
            value={searchText}
            onChange={(e) => setSearchText(e.target.value)}
            className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none transition-all"
          />
        </div>
      </div>

      {/* Table */}
      <div className="bg-white rounded-xl border border-gray-200 shadow-sm overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full text-left text-sm text-gray-600">
            <thead className="bg-gray-50 border-b border-gray-200 text-gray-900 font-semibold uppercase tracking-wider text-[11px]">
              <tr>
                <th className="px-6 py-4 text-center w-16">ID</th>
                <th className="px-6 py-4">Tên Role</th>
                <th className="px-6 py-4 text-center w-32">Phạm vi</th>
                <th className="px-6 py-4 text-center w-24">Mặc định</th>
                <th className="px-6 py-4">Mô tả</th>
                <th className="px-6 py-4 text-center w-40">Hành động</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200">
              {loading && filteredRoles.length === 0 ? (
                <tr>
                  <td colSpan={6} className="px-6 py-10 text-center text-gray-500">
                    <Loader2 className="w-8 h-8 animate-spin mx-auto mb-2 text-blue-600" />
                    Đang tải dữ liệu vai trò...
                  </td>
                </tr>
              ) : filteredRoles.length === 0 ? (
                <tr>
                  <td colSpan={6} className="px-6 py-12 text-center text-gray-500 italic">Không tìm thấy Role nào phù hợp.</td>
                </tr>
              ) : (
                filteredRoles.map((row) => (
                  <tr key={row.roleID} className="hover:bg-blue-50/30 transition-colors">
                    <td className="px-6 py-4 text-center font-medium">{row.roleID}</td>
                    <td className="px-6 py-4 font-bold text-gray-900">
                      <div className="flex items-center gap-2">
                        <Shield className={`w-4 h-4 ${row.roleName === 'admin' ? 'text-red-500' : 'text-blue-500'}`} />
                        {row.roleName}
                      </div>
                    </td>
                    <td className="px-6 py-4 text-center">
                      <span className={`px-2.5 py-1 text-[10px] font-bold uppercase tracking-tight rounded-full ${
                        row.scope === 'staff' ? 'bg-purple-100 text-purple-700 border border-purple-200' : 'bg-blue-100 text-blue-700 border border-blue-200'
                      }`}>
                        {row.scope}
                      </span>
                    </td>
                    <td className="px-6 py-4 text-center">
                      {row.isDefault ? (
                        <span title="Vai trò mặc định">
                          <CheckSquare className="w-5 h-5 text-green-500 mx-auto" />
                        </span>
                      ) : (
                        <Square className="w-5 h-5 text-gray-300 mx-auto" />
                      )}
                    </td>
                    <td className="px-6 py-4 text-gray-500 truncate max-w-xs" title={row.roleDescription}>
                      {row.roleDescription || '-'}
                    </td>
                    <td className="px-6 py-4">
                      <div className="flex items-center justify-center gap-2">
                        <button className="p-1.5 text-blue-600 hover:bg-blue-100 rounded-md transition-colors" title="Xem chi tiết">
                          <Eye className="w-4 h-4" />
                        </button>
                        <button 
                          onClick={() => {
                            setCloneData({ 
                              sourceRoleId: row.roleID, 
                              newRoleName: `${row.roleName}_copy`, 
                              newRoleDescription: row.roleDescription, 
                              newScope: row.scope, 
                              isDefault: false 
                            });
                            setIsCloneModalOpen(true);
                          }}
                          className="p-1.5 text-green-600 hover:bg-green-100 rounded-md transition-colors" 
                          title="Sao chép"
                        >
                          <Copy className="w-4 h-4" />
                        </button>
                        <button onClick={() => handleOpenModal('edit', row)} className="p-1.5 text-orange-600 hover:bg-orange-100 rounded-md transition-colors" title="Sửa">
                          <Edit2 className="w-4 h-4" />
                        </button>
                        <button onClick={() => handleDelete(row.roleID)} className="p-1.5 text-red-600 hover:bg-red-100 rounded-md transition-colors" title="Xóa">
                          <Trash2 className="w-4 h-4" />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>

      {/* Modal Add/Edit */}
      {isModalOpen && (
        <div className="fixed inset-0 z-[100] flex items-center justify-center bg-gray-900/50 backdrop-blur-sm p-4">
          <div className="bg-white rounded-2xl shadow-2xl w-full max-w-lg overflow-hidden animate-in fade-in zoom-in-95 duration-200 border border-gray-200">
            <div className="px-6 py-5 border-b border-gray-100 flex items-center justify-between bg-white">
              <h2 className="text-xl font-bold text-gray-900 flex items-center gap-2">
                <Shield className="w-6 h-6 text-blue-600" />
                {dialogMode === 'add' ? 'Thêm Role Mới' : 'Cập nhật Role'}
              </h2>
              <button onClick={() => setIsModalOpen(false)} className="p-2 text-gray-400 hover:text-gray-600 hover:bg-gray-100 rounded-full transition-colors font-bold">✕</button>
            </div>
            <form onSubmit={handleSubmit} className="p-6 space-y-5 bg-gray-50/30">
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-1.5">Tên Role <span className="text-red-500">*</span></label>
                <input 
                  type="text" required
                  value={currentRole.roleName}
                  onChange={(e) => setCurrentRole({...currentRole, roleName: e.target.value})}
                  className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none bg-white transition-all shadow-sm" 
                  placeholder="VD: Quản trị viên"
                />
              </div>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-semibold text-gray-700 mb-1.5">Phạm vi (Scope)</label>
                  <select 
                    value={currentRole.scope}
                    onChange={(e) => setCurrentRole({...currentRole, scope: e.target.value})}
                    className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none bg-white transition-all shadow-sm"
                  >
                    <option value="user">User (Người dùng)</option>
                    <option value="staff">Staff (Nhân viên)</option>
                  </select>
                </div>
                <div className="flex items-center pt-6 px-2">
                  <label className="flex items-center gap-2 cursor-pointer group">
                    <input 
                      type="checkbox" 
                      checked={currentRole.isDefault}
                      onChange={(e) => setCurrentRole({...currentRole, isDefault: e.target.checked})}
                      className="w-5 h-5 text-blue-600 border-gray-300 rounded-lg focus:ring-blue-500 transition-all cursor-pointer" 
                    />
                    <span className="text-sm font-medium text-gray-700 group-hover:text-blue-600 transition-colors">Đặt làm mặc định</span>
                  </label>
                </div>
              </div>
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-1.5">Mô tả</label>
                <textarea 
                  rows={3}
                  value={currentRole.roleDescription}
                  onChange={(e) => setCurrentRole({...currentRole, roleDescription: e.target.value})}
                  className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none bg-white resize-none transition-all shadow-sm" 
                  placeholder="Mô tả chức năng của vai trò này..."
                />
              </div>
              <div className="pt-4 flex items-center justify-end gap-3 border-t border-gray-100 mt-6">
                <button 
                  type="button" 
                  onClick={() => setIsModalOpen(false)} 
                  className="px-5 py-2.5 text-gray-700 bg-white border border-gray-300 rounded-xl hover:bg-gray-100 font-medium transition-colors"
                >
                  Hủy bỏ
                </button>
                <button 
                  type="submit" 
                  disabled={isSaving} 
                  className="px-6 py-2.5 bg-blue-600 text-white rounded-xl hover:bg-blue-700 font-bold shadow-md transition-all disabled:opacity-50 flex items-center gap-2"
                >
                  {isSaving ? <Loader2 className="w-4 h-4 animate-spin" /> : <Save className="w-4 h-4" />}
                  {dialogMode === 'add' ? 'Thêm mới' : 'Lưu thay đổi'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Modal Clone */}
      {isCloneModalOpen && (
        <div className="fixed inset-0 z-[100] flex items-center justify-center bg-gray-900/50 backdrop-blur-sm p-4">
          <div className="bg-white rounded-2xl shadow-2xl w-full max-w-lg overflow-hidden animate-in fade-in zoom-in-95 duration-200 border border-gray-200">
            <div className="px-6 py-5 border-b border-gray-100 flex items-center justify-between bg-white">
              <div>
                <h2 className="text-xl font-bold text-gray-900 flex items-center gap-2">
                   <Copy className="w-6 h-6 text-green-600" /> Sao chép Role
                </h2>
                <p className="text-xs text-gray-500 mt-1">Tạo vai trò mới kế thừa quyền từ Role ID: {cloneData.sourceRoleId}</p>
              </div>
              <button onClick={() => setIsCloneModalOpen(false)} className="p-2 text-gray-400 hover:text-gray-600 hover:bg-gray-100 rounded-full transition-colors font-bold">✕</button>
            </div>
            <form onSubmit={handleCloneSubmit} className="p-6 space-y-5 bg-gray-50/30">
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-1.5">Tên Role Mới <span className="text-red-500">*</span></label>
                <input 
                  type="text" required
                  value={cloneData.newRoleName}
                  onChange={(e) => setCloneData({...cloneData, newRoleName: e.target.value})}
                  className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none bg-white transition-all shadow-sm" 
                  placeholder="Ví dụ: admin-copy"
                />
              </div>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-semibold text-gray-700 mb-1.5">Phạm vi (Scope) Mới</label>
                  <select 
                    value={cloneData.newScope}
                    onChange={(e) => setCloneData({...cloneData, newScope: e.target.value})}
                    className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none bg-white transition-all shadow-sm"
                  >
                    <option value="user">User (Người dùng)</option>
                    <option value="staff">Staff (Nhân viên)</option>
                  </select>
                </div>
                <div className="flex items-center pt-6 px-2">
                  <label className="flex items-center gap-2 cursor-pointer group">
                    <input 
                      type="checkbox" 
                      checked={cloneData.isDefault}
                      onChange={(e) => setCloneData({...cloneData, isDefault: e.target.checked})}
                      className="w-5 h-5 text-blue-600 border-gray-300 rounded-lg focus:ring-blue-500 transition-all cursor-pointer" 
                    />
                    <span className="text-sm font-medium text-gray-700 group-hover:text-blue-600 transition-colors">Đặt làm mặc định</span>
                  </label>
                </div>
              </div>
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-1.5">Mô tả Mới</label>
                <textarea 
                  rows={3}
                  value={cloneData.newRoleDescription}
                  onChange={(e) => setCloneData({...cloneData, newRoleDescription: e.target.value})}
                  className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none bg-white resize-none transition-all shadow-sm" 
                />
              </div>
              <div className="pt-4 flex items-center justify-end gap-3 border-t border-gray-100 mt-6">
                <button type="button" onClick={() => setIsCloneModalOpen(false)} className="px-5 py-2.5 text-gray-700 bg-white border border-gray-300 rounded-xl hover:bg-gray-100 font-medium transition-colors">
                  Hủy bỏ
                </button>
                <button type="submit" disabled={isSaving} className="px-6 py-2.5 bg-green-600 text-white rounded-xl hover:bg-green-700 font-bold shadow-md transition-all disabled:opacity-50 flex items-center gap-2">
                  {isSaving ? <Loader2 className="w-4 h-4 animate-spin" /> : <Copy className="w-4 h-4" />}
                  Sao chép ngay
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}