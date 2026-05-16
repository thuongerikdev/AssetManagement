import { useState, useEffect, useMemo } from 'react';
import {
  Users, Search, Trash2, ShieldCheck,
  XCircle, CheckCircle2, Loader2, Mail, RefreshCw,
  Plus, UserCog, Save, X, Eye, EyeOff,
} from 'lucide-react';
import { toast } from 'sonner';
import { authApi, CreateUserRequest } from '../../api/authApi';

let cachedUsers: any[] | null = null;
let cachedRoles: any[] | null = null;

const GENDER_OPTIONS = [
  { value: '', label: '-- Không chọn --' },
  { value: 'Male', label: 'Nam' },
  { value: 'Female', label: 'Nữ' },
  { value: 'Other', label: 'Khác' },
];

const emptyForm: CreateUserRequest = {
  userName: '',
  email: '',
  password: '',
  autoVerifyEmail: true,
  scope: 'staff',
  roleIds: [],
  firstName: '',
  lastName: '',
  gender: '',
};

export function UserList() {
  const [users, setUsers] = useState<any[]>(cachedUsers || []);
  const [roles, setRoles] = useState<any[]>(cachedRoles || []);
  const [loading, setLoading] = useState(!cachedUsers);
  const [searchText, setSearchText] = useState('');

  // Modal: tạo user
  const [isCreateOpen, setIsCreateOpen] = useState(false);
  const [createForm, setCreateForm] = useState<CreateUserRequest>(emptyForm);
  const [isSaving, setIsSaving] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  // Modal: gán role
  const [isAssignOpen, setIsAssignOpen] = useState(false);
  const [assignUser, setAssignUser] = useState<any | null>(null);
  const [selectedRoleIds, setSelectedRoleIds] = useState<number[]>([]);
  const [isAssigning, setIsAssigning] = useState(false);

  // ── Load data ────────────────────────────────────────────────────────────────
  const fetchUsers = async (forceRefresh = false) => {
    if (!forceRefresh && cachedUsers) { setUsers(cachedUsers); return; }
    setLoading(true);
    try {
      const response = await authApi.getAllUsers();
      if (response.errorCode === 200 && response.data) {
        const transformed = response.data.map((user: any) => ({
          ...user,
          fullName: `${user.profile?.firstName || ''} ${user.profile?.lastName || ''}`.trim() || user.userName,
          sessionsCount: user.sessions?.length || 0,
        }));
        cachedUsers = transformed;
        setUsers(transformed);
      }
    } catch {
      toast.error('Lỗi khi tải danh sách User');
    } finally {
      setLoading(false);
    }
  };

  const fetchRoles = async () => {
    if (cachedRoles) { setRoles(cachedRoles); return; }
    try {
      const res = await authApi.getAllRoles();
      if (res.errorCode === 200 && res.data) {
        cachedRoles = res.data;
        setRoles(res.data);
      }
    } catch {
      toast.error('Lỗi khi tải danh sách Role');
    }
  };

  useEffect(() => {
    fetchUsers();
    fetchRoles();
  }, []);

  // ── Search filter ────────────────────────────────────────────────────────────
  const filteredUsers = useMemo(() => {
    const lower = searchText.toLowerCase();
    return users.filter(u =>
      u.fullName.toLowerCase().includes(lower) ||
      u.email.toLowerCase().includes(lower) ||
      u.userName.toLowerCase().includes(lower)
    );
  }, [users, searchText]);

  // ── Handlers ─────────────────────────────────────────────────────────────────
  const handleDelete = async (userId: number) => {
    if (!window.confirm('Bạn có chắc chắn muốn xóa user này?')) return;
    try {
      const res = await authApi.deleteUser(userId);
      if (res.errorCode === 200) {
        toast.success('Xóa user thành công!');
        fetchUsers(true);
      } else {
        toast.error(res.errorMessage || 'Xóa user thất bại');
      }
    } catch {
      toast.error('Có lỗi xảy ra khi xóa user');
    }
  };

  const handleCreateSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!createForm.userName || !createForm.email || !createForm.password) {
      toast.error('Vui lòng điền đủ thông tin bắt buộc!');
      return;
    }
    setIsSaving(true);
    try {
      const payload: CreateUserRequest = {
        ...createForm,
        scope: 'staff', // luôn gửi staff
        roleIds: createForm.roleIds?.length ? createForm.roleIds : undefined,
        gender: createForm.gender || undefined,
        firstName: createForm.firstName || undefined,
        lastName: createForm.lastName || undefined,
      };
      const res = await authApi.createUser(payload);
      if (res.errorCode === 200 || res.errorCode === 201) {
        toast.success('Tạo tài khoản thành công!');
        setIsCreateOpen(false);
        setCreateForm(emptyForm);
        fetchUsers(true);
      } else {
        toast.error(res.errorMessage || 'Tạo tài khoản thất bại');
      }
    } catch {
      toast.error('Lỗi kết nối máy chủ');
    } finally {
      setIsSaving(false);
    }
  };

  const openAssignModal = (user: any) => {
    setAssignUser(user);
    const currentRoleNames: string[] = (user.roles || []).map((r: string) => r.toLowerCase());
    const matchedRoleIds = roles
      .filter(r => currentRoleNames.includes((r.roleName || '').toLowerCase()))
      .map(r => r.roleID ?? r.roleId ?? r.id);
    setSelectedRoleIds(matchedRoleIds);
    setIsAssignOpen(true);
  };

  const handleAssignRoles = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!assignUser) return;
    setIsAssigning(true);
    try {
      const res = await authApi.assignRolesToUser({
        userId: assignUser.userID,
        roleIds: selectedRoleIds,
      });
      if (res.errorCode === 200 || res.errorCode === 201) {
        toast.success('Phân quyền thành công!');
        setIsAssignOpen(false);
        fetchUsers(true);
      } else {
        toast.error(res.errorMessage || 'Phân quyền thất bại');
      }
    } catch {
      toast.error('Lỗi kết nối máy chủ');
    } finally {
      setIsAssigning(false);
    }
  };

  const toggleRoleId = (id: number) => {
    setSelectedRoleIds(prev =>
      prev.includes(id) ? prev.filter(x => x !== id) : [...prev, id]
    );
  };

  const toggleCreateRole = (id: number) => {
    setCreateForm(prev => {
      const ids = prev.roleIds || [];
      return {
        ...prev,
        roleIds: ids.includes(id) ? ids.filter(x => x !== id) : [...ids, id],
      };
    });
  };

  // ── Render ───────────────────────────────────────────────────────────────────
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
        <div className="flex gap-2">
          <button
            onClick={() => fetchUsers(true)}
            disabled={loading}
            className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-gray-600 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors disabled:opacity-50 shadow-sm"
          >
            <RefreshCw className={`w-4 h-4 ${loading ? 'animate-spin text-indigo-600' : ''}`} />
            <span className="hidden sm:block">Làm mới</span>
          </button>
          <button
            onClick={() => { setCreateForm(emptyForm); setIsCreateOpen(true); }}
            className="flex items-center gap-2 px-4 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-colors shadow-sm font-medium"
          >
            <Plus className="w-5 h-5" /> Thêm người dùng
          </button>
        </div>
      </div>

      {/* Toolbar */}
      <div className="bg-white p-4 rounded-lg border border-gray-200 shadow-sm flex items-center gap-4">
        <div className="relative flex-1 max-w-md">
          <Search className="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
          <input
            type="text"
            placeholder="Tìm kiếm theo tên, email, username..."
            value={searchText}
            onChange={e => setSearchText(e.target.value)}
            className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-indigo-500 outline-none transition-all"
          />
        </div>
        <span className="text-sm text-gray-500">{filteredUsers.length} người dùng</span>
      </div>

      {/* Table */}
      <div className="bg-white rounded-lg border border-gray-200 shadow-sm overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full text-left text-sm text-gray-600">
            <thead className="bg-gray-50 border-b border-gray-200 text-gray-900 font-medium text-[11px] uppercase tracking-wider">
              <tr>
                <th className="px-4 py-3 text-center w-16">ID</th>
                <th className="px-4 py-3">Họ và Tên</th>
                <th className="px-4 py-3">Tài khoản / Email</th>
                <th className="px-4 py-3 text-center">Xác thực</th>
                <th className="px-4 py-3 text-center">Trạng thái</th>
                <th className="px-4 py-3">Vai trò</th>
                <th className="px-4 py-3 text-center w-32">Hành động</th>
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
                  <td colSpan={7} className="px-4 py-8 text-center text-gray-500 italic">
                    Không tìm thấy người dùng nào.
                  </td>
                </tr>
              ) : (
                filteredUsers.map(row => (
                  <tr key={row.userID} className="hover:bg-gray-50 transition-colors">
                    <td className="px-4 py-3 text-center font-medium text-gray-900">{row.userID}</td>
                    <td className="px-4 py-3">
                      <div className="font-medium text-gray-900">{row.fullName}</div>
                      <div className="text-xs text-gray-500">@{row.userName}</div>
                    </td>
                    <td className="px-4 py-3">
                      <div className="flex items-center gap-2 text-sm">
                        <Mail className="w-4 h-4 text-gray-400 shrink-0" />
                        {row.email}
                      </div>
                    </td>
                    <td className="px-4 py-3 text-center">
                      {row.isEmailVerified
                        ? <span title="Đã xác thực"><CheckCircle2 className="w-5 h-5 text-green-500 mx-auto" /></span>
                        : <span title="Chưa xác thực"><XCircle className="w-5 h-5 text-red-500 mx-auto" /></span>}
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
                          <span key={idx} className={`px-2 py-1 text-xs font-medium rounded border ${
                            role.toLowerCase().includes('admin')
                              ? 'bg-red-50 text-red-700 border-red-200'
                              : 'bg-blue-50 text-blue-700 border-blue-200'
                          }`}>
                            {role}
                          </span>
                        ))}
                        {row.roles?.length > 2 && (
                          <span className="px-2 py-1 text-xs bg-gray-100 text-gray-600 rounded border border-gray-200" title={row.roles.slice(2).join(', ')}>
                            +{row.roles.length - 2}
                          </span>
                        )}
                        {(!row.roles || row.roles.length === 0) && (
                          <span className="text-gray-400 italic text-xs">Chưa có</span>
                        )}
                      </div>
                    </td>
                    <td className="px-4 py-3">
                      <div className="flex items-center justify-center gap-1">
                        <button
                          onClick={() => openAssignModal(row)}
                          className="p-1.5 text-indigo-600 hover:bg-indigo-50 rounded transition-colors"
                          title="Phân quyền vai trò"
                        >
                          <UserCog className="w-4 h-4" />
                        </button>
                        <button
                          onClick={() => handleDelete(row.userID)}
                          className="p-1.5 text-gray-500 hover:text-red-600 hover:bg-red-50 rounded transition-colors"
                          title="Xóa tài khoản"
                        >
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

      {/* ── Modal: Tạo User ─────────────────────────────────────────────────────── */}
      {isCreateOpen && (
        <div className="fixed inset-0 z-[100] flex items-center justify-center bg-gray-900/50 backdrop-blur-sm p-4">
          <div className="bg-white rounded-2xl shadow-2xl w-full max-w-xl overflow-hidden border border-gray-200">
            <div className="px-6 py-4 border-b border-gray-100 flex items-center justify-between">
              <h2 className="text-xl font-bold text-gray-900 flex items-center gap-2">
                <Users className="w-5 h-5 text-indigo-600" /> Tạo Tài khoản Mới
              </h2>
              <button onClick={() => setIsCreateOpen(false)} className="p-2 text-gray-400 hover:text-gray-600 hover:bg-gray-100 rounded-full transition-colors">
                <X className="w-5 h-5" />
              </button>
            </div>

            <form onSubmit={handleCreateSubmit} className="p-6 space-y-4 max-h-[75vh] overflow-y-auto">
              {/* Thông tin đăng nhập */}
              <p className="text-xs font-semibold uppercase tracking-wider text-gray-400">Thông tin đăng nhập</p>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-semibold text-gray-700 mb-1">
                    Tên đăng nhập <span className="text-red-500">*</span>
                  </label>
                  <input
                    type="text" required
                    value={createForm.userName}
                    onChange={e => setCreateForm(p => ({ ...p, userName: e.target.value }))}
                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 outline-none text-sm"
                    placeholder="username"
                  />
                </div>
                <div>
                  <label className="block text-sm font-semibold text-gray-700 mb-1">
                    Email <span className="text-red-500">*</span>
                  </label>
                  <input
                    type="email" required
                    value={createForm.email}
                    onChange={e => setCreateForm(p => ({ ...p, email: e.target.value }))}
                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 outline-none text-sm"
                    placeholder="user@example.com"
                  />
                </div>
              </div>

              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-1">
                  Mật khẩu <span className="text-red-500">*</span>
                </label>
                <div className="relative">
                  <input
                    type={showPassword ? 'text' : 'password'} required
                    value={createForm.password}
                    onChange={e => setCreateForm(p => ({ ...p, password: e.target.value }))}
                    className="w-full px-3 py-2 pr-10 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 outline-none text-sm"
                    placeholder="Mật khẩu mạnh..."
                  />
                  <button type="button" onClick={() => setShowPassword(p => !p)}
                    className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-600">
                    {showPassword ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                  </button>
                </div>
              </div>

              {/* Hồ sơ cá nhân */}
              <p className="text-xs font-semibold uppercase tracking-wider text-gray-400 pt-2">Hồ sơ cá nhân</p>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-semibold text-gray-700 mb-1">Họ</label>
                  <input
                    type="text"
                    value={createForm.firstName}
                    onChange={e => setCreateForm(p => ({ ...p, firstName: e.target.value }))}
                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 outline-none text-sm"
                    placeholder="Nguyễn"
                  />
                </div>
                <div>
                  <label className="block text-sm font-semibold text-gray-700 mb-1">Tên</label>
                  <input
                    type="text"
                    value={createForm.lastName}
                    onChange={e => setCreateForm(p => ({ ...p, lastName: e.target.value }))}
                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 outline-none text-sm"
                    placeholder="Văn A"
                  />
                </div>
              </div>

              {/* Giới tính — full width vì đã bỏ Scope */}
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-1">Giới tính</label>
                <select
                  value={createForm.gender}
                  onChange={e => setCreateForm(p => ({ ...p, gender: e.target.value }))}
                  className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 outline-none text-sm bg-white"
                >
                  {GENDER_OPTIONS.map(o => <option key={o.value} value={o.value}>{o.label}</option>)}
                </select>
              </div>

              {/* Gán Role ngay khi tạo */}
              <p className="text-xs font-semibold uppercase tracking-wider text-gray-400 pt-2">Vai trò (tùy chọn)</p>
              {roles.length === 0 ? (
                <p className="text-xs text-gray-400 italic">Chưa tải được danh sách vai trò</p>
              ) : (
                <div className="grid grid-cols-2 gap-2 max-h-40 overflow-y-auto p-3 border border-gray-200 rounded-lg bg-gray-50">
                  {roles.map(r => (
                    <label key={r.roleID} className="flex items-center gap-2 cursor-pointer group">
                      <input
                        type="checkbox"
                        checked={(createForm.roleIds || []).includes(r.roleID)}
                        onChange={() => toggleCreateRole(r.roleID)}
                        className="w-4 h-4 text-indigo-600 rounded border-gray-300 focus:ring-indigo-500"
                      />
                      <span className="text-sm text-gray-700 group-hover:text-indigo-600 transition-colors truncate">
                        {r.roleName}
                        <span className="text-xs text-gray-400 ml-1">({r.scope})</span>
                      </span>
                    </label>
                  ))}
                </div>
              )}

              <div className="flex items-center gap-2 pt-2">
                <input
                  type="checkbox"
                  id="autoVerify"
                  checked={createForm.autoVerifyEmail}
                  onChange={e => setCreateForm(p => ({ ...p, autoVerifyEmail: e.target.checked }))}
                  className="w-4 h-4 text-indigo-600 rounded border-gray-300"
                />
                <label htmlFor="autoVerify" className="text-sm text-gray-700 cursor-pointer">
                  Tự động xác thực email (không cần gửi link xác nhận)
                </label>
              </div>

              {/* Footer */}
              <div className="flex items-center justify-end gap-3 pt-4 border-t border-gray-100">
                <button type="button" onClick={() => setIsCreateOpen(false)}
                  className="px-5 py-2 text-gray-700 bg-white border border-gray-300 rounded-xl hover:bg-gray-100 font-medium transition-colors text-sm">
                  Hủy bỏ
                </button>
                <button type="submit" disabled={isSaving}
                  className="px-6 py-2 bg-indigo-600 text-white rounded-xl hover:bg-indigo-700 font-bold shadow-md transition-all disabled:opacity-50 flex items-center gap-2 text-sm">
                  {isSaving ? <Loader2 className="w-4 h-4 animate-spin" /> : <Save className="w-4 h-4" />}
                  Tạo tài khoản
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* ── Modal: Gán Role cho User ─────────────────────────────────────────────── */}
      {isAssignOpen && assignUser && (
        <div className="fixed inset-0 z-[100] flex items-center justify-center bg-gray-900/50 backdrop-blur-sm p-4">
          <div className="bg-white rounded-2xl shadow-2xl w-full max-w-md overflow-hidden border border-gray-200">
            <div className="px-6 py-4 border-b border-gray-100 flex items-center justify-between">
              <div>
                <h2 className="text-lg font-bold text-gray-900 flex items-center gap-2">
                  <UserCog className="w-5 h-5 text-indigo-600" /> Phân quyền Vai trò
                </h2>
                <p className="text-xs text-gray-500 mt-0.5">
                  User: <span className="font-semibold text-gray-700">{assignUser.fullName}</span>
                  {' '}<span className="text-gray-400">(@{assignUser.userName})</span>
                </p>
              </div>
              <button onClick={() => setIsAssignOpen(false)}
                className="p-2 text-gray-400 hover:text-gray-600 hover:bg-gray-100 rounded-full transition-colors">
                <X className="w-5 h-5" />
              </button>
            </div>

            <form onSubmit={handleAssignRoles} className="p-6">
              <p className="text-sm text-gray-600 mb-3">
                Chọn các vai trò muốn gán cho người dùng này:
              </p>
              {roles.length === 0 ? (
                <p className="text-sm text-gray-400 italic">Không có vai trò nào trong hệ thống.</p>
              ) : (
                <div className="space-y-2 max-h-64 overflow-y-auto pr-1">
                  {roles.map(r => {
                    const rId = r.roleID ?? r.roleId ?? r.id;
                    const isChecked = selectedRoleIds.includes(rId);
                    return (
                      <label key={rId}
                        className={`flex items-center gap-3 p-3 rounded-xl cursor-pointer border transition-all ${
                          isChecked
                            ? 'border-indigo-300 bg-indigo-50'
                            : 'border-gray-200 bg-white hover:border-gray-300 hover:bg-gray-50'
                        }`}
                      >
                        <input
                          type="checkbox"
                          checked={isChecked}
                          onChange={() => toggleRoleId(rId)}
                          className="w-4 h-4 text-indigo-600 rounded border-gray-300 focus:ring-indigo-500"
                        />
                        <div className="flex-1 min-w-0">
                          <p className="text-sm font-semibold text-gray-800">{r.roleName}</p>
                          {r.roleDescription && (
                            <p className="text-xs text-gray-500 truncate">{r.roleDescription}</p>
                          )}
                        </div>
                        <span className={`px-2 py-0.5 text-[10px] font-bold uppercase rounded-full ${
                          r.scope === 'staff'
                            ? 'bg-purple-100 text-purple-700'
                            : 'bg-blue-100 text-blue-700'
                        }`}>
                          {r.scope}
                        </span>
                      </label>
                    );
                  })}
                </div>
              )}

              <div className="flex items-center justify-between pt-4 border-t border-gray-100 mt-4">
                <span className="text-xs text-gray-500">
                  Đã chọn: <span className="font-semibold text-indigo-600">{selectedRoleIds.length}</span> vai trò
                </span>
                <div className="flex gap-3">
                  <button type="button" onClick={() => setIsAssignOpen(false)}
                    className="px-4 py-2 text-gray-700 bg-white border border-gray-300 rounded-xl hover:bg-gray-100 font-medium transition-colors text-sm">
                    Hủy
                  </button>
                  <button type="submit" disabled={isAssigning}
                    className="px-5 py-2 bg-indigo-600 text-white rounded-xl hover:bg-indigo-700 font-bold shadow-md transition-all disabled:opacity-50 flex items-center gap-2 text-sm">
                    {isAssigning ? <Loader2 className="w-4 h-4 animate-spin" /> : <ShieldCheck className="w-4 h-4" />}
                    Lưu phân quyền
                  </button>
                </div>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}