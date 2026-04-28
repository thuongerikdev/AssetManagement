import { useState, useEffect, useMemo } from 'react';
import { 
  Key, Plus, Search, Edit2, Trash2, 
  Loader2, ShieldAlert, RefreshCw
} from 'lucide-react';
import { toast } from 'sonner';
import { authApi } from '../../api/authApi';

// ==========================================
// 1. BIẾN CACHE CỤC BỘ TRÊN RAM
// ==========================================
let cachedPermissions: any[] | null = null;

export function PermissionList() {
  // State quản lý danh sách Quyền
  const [permissions, setPermissions] = useState<any[]>(cachedPermissions || []);
  const [loading, setLoading] = useState(!cachedPermissions);
  const [searchText, setSearchText] = useState('');
  
  // Dialog States
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [dialogMode, setDialogMode] = useState<'add' | 'edit'>('add');
  const [isSaving, setIsSaving] = useState(false);
  
  const [currentPermission, setCurrentPermission] = useState({
    permissionID: 0,
    permissionName: '',
    permissionDescription: '',
    code: '',
    scope: 'user',
  });

  // ==========================================
  // 2. HÀM TẢI DỮ LIỆU CÓ TÍCH HỢP CACHE
  // ==========================================
  const fetchPermissions = async (forceRefresh = false) => {
    if (!forceRefresh && cachedPermissions) {
      setPermissions(cachedPermissions);
      return;
    }

    setLoading(true);
    try {
      const response = await authApi.getAllPermissions();
      if (response.errorCode === 200 && response.data) {
        cachedPermissions = response.data; // Lưu vào Cache
        setPermissions(response.data);
      }
    } catch (error) {
      toast.error('Lỗi khi tải danh sách Quyền hạn');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPermissions();
  }, []);

  // ==========================================
  // 3. USE_MEMO BỘ LỌC TÌM KIẾM TỨC THÌ
  // ==========================================
  const filteredPermissions = useMemo(() => {
    const lower = searchText.toLowerCase();
    return permissions.filter(p => 
      p.permissionName.toLowerCase().includes(lower) || 
      p.code.toLowerCase().includes(lower) ||
      (p.scope && p.scope.toLowerCase().includes(lower))
    );
  }, [permissions, searchText]);

  const handleOpenModal = (mode: 'add' | 'edit', permission: any = null) => {
    setDialogMode(mode);
    if (mode === 'edit' && permission) {
      setCurrentPermission(permission);
    } else {
      setCurrentPermission({ permissionID: 0, permissionName: '', permissionDescription: '', code: '', scope: 'user' });
    }
    setIsModalOpen(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!currentPermission.permissionName || !currentPermission.code) {
      toast.error("Vui lòng nhập Tên và Code!");
      return;
    }
    
    setIsSaving(true);
    try {
      let response;
      if (dialogMode === 'add') {
        const { permissionID, ...payload } = currentPermission;
        response = await authApi.addPermission(payload);
      } else {
        response = await authApi.updatePermission(currentPermission);
      }

      if (response.errorCode === 200 || response.errorCode === 201) {
        toast.success(dialogMode === 'add' ? 'Thêm mới thành công!' : 'Cập nhật thành công!');
        setIsModalOpen(false);
        fetchPermissions(true); // Ép làm mới Cache sau khi lưu
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
    if (window.confirm('Bạn có chắc chắn muốn xóa Permission này?')) {
      try {
        const response = await authApi.deletePermission(id);
        if (response.errorCode === 200) {
          toast.success('Xóa thành công!');
          fetchPermissions(true); // Ép làm mới Cache sau khi xóa
        } else {
          toast.error(response.errorMessage || 'Xóa thất bại!');
        }
      } catch (error) {
        toast.error('Lỗi khi xóa permission.');
      }
    }
  };

  return (
    <div className="space-y-6 max-w-7xl mx-auto">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 flex items-center gap-2">
            <Key className="w-6 h-6 text-orange-500" /> Quản lý Quyền (Permissions)
          </h1>
          <p className="text-sm text-gray-500 mt-1">Danh sách các quyền hạn thao tác trong hệ thống</p>
        </div>
        
        <div className="flex gap-2">
          {/* Nút Làm mới Data */}
          <button 
            onClick={() => fetchPermissions(true)}
            disabled={loading}
            className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-gray-600 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors disabled:opacity-50 shadow-sm"
            title="Tải lại dữ liệu"
          >
            <RefreshCw className={`w-4 h-4 ${loading ? 'animate-spin text-orange-500' : ''}`} />
            <span className="hidden sm:block">Làm mới</span>
          </button>

          <button 
            onClick={() => handleOpenModal('add')}
            className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-sm font-medium"
          >
            <Plus className="w-5 h-5" /> Thêm Quyền
          </button>
        </div>
      </div>

      {/* Toolbar */}
      <div className="bg-white p-4 rounded-xl border border-gray-200 shadow-sm">
        <div className="relative max-w-md">
          <Search className="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
          <input 
            type="text" 
            placeholder="Tìm kiếm theo tên, code..." 
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
                <th className="px-6 py-4">Tên Quyền</th>
                <th className="px-6 py-4">Code (Key)</th>
                <th className="px-6 py-4 text-center w-32">Scope</th>
                <th className="px-6 py-4">Mô tả</th>
                <th className="px-6 py-4 text-center w-32">Hành động</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200">
              {loading && filteredPermissions.length === 0 ? (
                <tr>
                  <td colSpan={6} className="px-6 py-10 text-center text-gray-500">
                    <Loader2 className="w-8 h-8 animate-spin mx-auto mb-2 text-blue-600" />
                    Đang tải dữ liệu...
                  </td>
                </tr>
              ) : filteredPermissions.length === 0 ? (
                <tr>
                  <td colSpan={6} className="px-6 py-12 text-center text-gray-500 italic">Không tìm thấy Quyền nào phù hợp.</td>
                </tr>
              ) : (
                filteredPermissions.map((row) => (
                  <tr key={row.permissionID} className="hover:bg-blue-50/30 transition-colors">
                    <td className="px-6 py-4 text-center font-medium">{row.permissionID}</td>
                    <td className="px-6 py-4 font-bold text-gray-900">{row.permissionName}</td>
                    <td className="px-6 py-4">
                      <code className="font-mono text-xs text-blue-600 bg-blue-50 rounded-md px-2 py-1 border border-blue-100">
                        {row.code}
                      </code>
                    </td>
                    <td className="px-6 py-4 text-center">
                      <span className="px-2 py-1 text-[10px] font-bold uppercase tracking-tight bg-gray-100 text-gray-700 rounded-md border border-gray-200">
                        {row.scope || 'Global'}
                      </span>
                    </td>
                    <td className="px-6 py-4 text-gray-500 truncate max-w-xs" title={row.permissionDescription}>
                      {row.permissionDescription || '-'}
                    </td>
                    <td className="px-6 py-4">
                      <div className="flex items-center justify-center gap-2">
                        <button onClick={() => handleOpenModal('edit', row)} className="p-1.5 text-orange-600 hover:bg-orange-100 rounded-md transition-colors" title="Sửa">
                          <Edit2 className="w-4 h-4" />
                        </button>
                        <button onClick={() => handleDelete(row.permissionID)} className="p-1.5 text-red-600 hover:bg-red-100 rounded-md transition-colors" title="Xóa">
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
              <div className="flex items-center gap-2">
                <ShieldAlert className="w-6 h-6 text-blue-600" />
                <h2 className="text-xl font-bold text-gray-900">
                  {dialogMode === 'add' ? 'Thêm Quyền Mới' : 'Chỉnh sửa Quyền'}
                </h2>
              </div>
              <button onClick={() => setIsModalOpen(false)} className="p-2 text-gray-400 hover:text-gray-600 hover:bg-gray-100 rounded-full transition-colors font-bold">✕</button>
            </div>
            
            <form onSubmit={handleSubmit} className="p-6 space-y-5 bg-gray-50/30">
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-1.5">Tên Quyền <span className="text-red-500">*</span></label>
                <input 
                  type="text" required
                  value={currentPermission.permissionName}
                  onChange={(e) => setCurrentPermission({...currentPermission, permissionName: e.target.value})}
                  className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none bg-white transition-all shadow-sm" 
                  placeholder="VD: Xem báo cáo doanh thu"
                />
              </div>
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-1.5">Code (Key) <span className="text-red-500">*</span></label>
                <input 
                  type="text" required
                  value={currentPermission.code}
                  onChange={(e) => setCurrentPermission({...currentPermission, code: e.target.value})}
                  className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none bg-white font-mono text-sm transition-all shadow-sm" 
                  placeholder="VD: report.revenue.view"
                />
              </div>
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-1.5">Scope (Phạm vi)</label>
                <select 
                  value={currentPermission.scope || 'user'}
                  onChange={(e) => setCurrentPermission({...currentPermission, scope: e.target.value})}
                  className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none bg-white transition-all shadow-sm"
                >
                  <option value="user">User (Người dùng)</option>
                  <option value="staff">Staff (Nhân viên)</option>
                </select>
              </div>
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-1.5">Mô tả</label>
                <textarea 
                  rows={3}
                  value={currentPermission.permissionDescription}
                  onChange={(e) => setCurrentPermission({...currentPermission, permissionDescription: e.target.value})}
                  className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none bg-white resize-none transition-all shadow-sm" 
                  placeholder="Giải thích chi tiết về quyền này..."
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
    </div>
  );
}

// Bổ sung thêm Icon Save nếu cần (tùy theo lucide-react version)
import { Save } from 'lucide-react';