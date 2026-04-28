import { useState, useEffect, useMemo } from 'react';
import { Plus, Search, Edit, Trash2, X, Save, BookOpen, RefreshCw } from 'lucide-react';
import { toast } from 'sonner';
import { accountApi, TaiKhoanKeToan } from '../../api/accountApi';

// ==========================================
// 1. BIẾN CACHE CỤC BỘ TRÊN RAM (Chỉ lưu Tài khoản kế toán)
// ==========================================
let cachedAccounts: TaiKhoanKeToan[] | null = null;

export function AccountList() {
  // State quản lý danh sách tài khoản
  const [accounts, setAccounts] = useState<TaiKhoanKeToan[]>(cachedAccounts || []);
  const [isLoading, setIsLoading] = useState(!cachedAccounts);
  const [searchTerm, setSearchTerm] = useState('');

  // States for Modal
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  
  const [formData, setFormData] = useState<Partial<TaiKhoanKeToan>>({
    maTaiKhoan: '',
    tenTaiKhoan: '',
    loaiTaiKhoan: 'Tài sản',
    maTaiKhoanCha: ''
  });

  // ==========================================
  // 2. HÀM TẢI DỮ LIỆU CÓ TÍCH HỢP CACHE
  // ==========================================
  const fetchAccounts = async (forceRefresh = false) => {
    if (!forceRefresh && cachedAccounts) {
      setAccounts(cachedAccounts);
      return;
    }

    setIsLoading(true);
    try {
      const response = await accountApi.getAll();
      if (response.errorCode === 200) {
        cachedAccounts = response.data;
        setAccounts(response.data);
      }
    } catch (error) {
      toast.error('Lỗi tải danh sách tài khoản.');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchAccounts();
  }, []);

  const openModal = (account?: TaiKhoanKeToan) => {
    if (account) {
      setEditingId(account.id!);
      setFormData({ ...account });
    } else {
      setEditingId(null);
      setFormData({
        maTaiKhoan: '',
        tenTaiKhoan: '',
        loaiTaiKhoan: 'Tài sản',
        maTaiKhoanCha: ''
      });
    }
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setEditingId(null);
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.maTaiKhoan || !formData.tenTaiKhoan) {
      toast.error('Vui lòng nhập đủ mã và tên tài khoản!');
      return;
    }

    setIsSubmitting(true);
    try {
      let response;
      if (editingId) {
        response = await accountApi.update({ ...formData, id: editingId } as TaiKhoanKeToan);
      } else {
        response = await accountApi.create(formData as TaiKhoanKeToan);
      }

      if (response.errorCode === 200) {
        toast.success(editingId ? 'Cập nhật thành công!' : 'Thêm mới thành công!');
        closeModal();
        fetchAccounts(true); // Ép tải lại dữ liệu mới nhất sau khi sửa/thêm
      } else {
        toast.error(response.message || 'Có lỗi xảy ra.');
      }
    } catch (error) {
      toast.error('Lỗi kết nối đến máy chủ.');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Bạn có chắc chắn muốn xóa hệ thống tài khoản này không?')) {
      try {
        const response = await accountApi.delete(id);
        if (response.errorCode === 200) {
          toast.success('Xóa tài khoản thành công!');
          fetchAccounts(true); // Ép tải lại dữ liệu sau khi xóa
        } else {
          toast.error(response.message || 'Lỗi khi xóa.');
        }
      } catch (error) {
        toast.error('Lỗi kết nối máy chủ.');
      }
    }
  };

  // ==========================================
  // 3. USE_MEMO BỘ LỌC ĐỂ RENDER MƯỢT MÀ
  // ==========================================
  const filteredAccounts = useMemo(() => {
    return accounts.filter(acc => 
      acc.maTaiKhoan.toLowerCase().includes(searchTerm.toLowerCase()) ||
      (acc.tenTaiKhoan && acc.tenTaiKhoan.toLowerCase().includes(searchTerm.toLowerCase()))
    );
  }, [accounts, searchTerm]);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Hệ thống Tài khoản Kế toán</h1>
          <p className="text-sm text-gray-500 mt-1">Quản lý danh mục tài khoản định khoản</p>
        </div>
        <div className="flex gap-2">
          {/* Nút Làm mới Data */}
          <button 
            onClick={() => fetchAccounts(true)}
            disabled={isLoading}
            className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-gray-600 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors disabled:opacity-50 shadow-sm"
            title="Tải lại dữ liệu"
          >
            <RefreshCw className={`w-4 h-4 ${isLoading ? 'animate-spin text-blue-600' : ''}`} />
            <span className="hidden sm:block">Làm mới</span>
          </button>

          <button
            onClick={() => openModal()}
            className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-sm"
          >
            <Plus className="w-5 h-5" /> Thêm Tài khoản
          </button>
        </div>
      </div>

      <div className="bg-white rounded-lg border border-gray-200 p-4">
        <div className="relative">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" />
          <input
            type="text"
            placeholder="Tìm kiếm theo mã hoặc tên tài khoản..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
          />
        </div>
      </div>

      <div className="bg-white rounded-lg border border-gray-200 overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50 border-b border-gray-200">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Mã TK</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Tên tài khoản</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Tính chất (Loại)</th>
                <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase">TK Cha</th>
                <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase">Thao tác</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200">
              {isLoading && filteredAccounts.length === 0 ? (
                <tr><td colSpan={5} className="text-center py-8 text-gray-500">Đang tải dữ liệu...</td></tr>
              ) : filteredAccounts.length === 0 ? (
                <tr><td colSpan={5} className="text-center py-8 text-gray-500">Không tìm thấy tài khoản nào.</td></tr>
              ) : (
                filteredAccounts.map((acc) => (
                  <tr key={acc.id} className="hover:bg-gray-50 transition-colors">
                    <td className="px-6 py-4 whitespace-nowrap">
                      <span className="font-bold text-blue-600">{acc.maTaiKhoan}</span>
                    </td>
                    <td className="px-6 py-4">
                      <span className={`text-sm text-gray-900 ${acc.maTaiKhoanCha ? 'ml-4' : 'font-medium'}`}>
                        {acc.maTaiKhoanCha && '↳ '} {acc.tenTaiKhoan}
                      </span>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">
                      {acc.loaiTaiKhoan}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-center text-sm text-gray-500">
                      {acc.maTaiKhoanCha ? (
                        <span className="px-2 py-1 bg-gray-100 rounded-md border border-gray-200">{acc.maTaiKhoanCha}</span>
                      ) : '-'}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-center">
                      <div className="flex items-center justify-center gap-2">
                        <button onClick={() => openModal(acc)} className="p-1.5 text-orange-600 hover:bg-orange-100 rounded-md transition-colors" title="Sửa">
                          <Edit className="w-4 h-4" />
                        </button>
                        <button onClick={() => acc.id && handleDelete(acc.id)} className="p-1.5 text-red-600 hover:bg-red-100 rounded-md transition-colors" title="Xóa">
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

      {/* Modal Thêm/Sửa */}
      {isModalOpen && (
        <div className="fixed inset-0 bg-gray-900/50 backdrop-blur-sm flex items-center justify-center z-50 p-4 transition-all">
          <div className="bg-white rounded-lg max-w-md w-full shadow-2xl flex flex-col overflow-hidden">
            <div className="p-6 border-b border-gray-200 flex items-center justify-between bg-white">
              <h3 className="font-bold text-gray-900 text-lg flex items-center gap-2">
                <BookOpen className="w-5 h-5 text-blue-600" />
                {editingId ? 'Cập nhật Tài khoản' : 'Thêm Tài khoản mới'}
              </h3>
              <button onClick={closeModal} className="text-gray-400 hover:text-gray-600">
                <X className="w-6 h-6" />
              </button>
            </div>
            
            <form onSubmit={handleSubmit} className="p-6 space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Mã tài khoản <span className="text-red-500">*</span></label>
                <input
                  type="text"
                  name="maTaiKhoan"
                  value={formData.maTaiKhoan}
                  onChange={handleChange}
                  required
                  placeholder="VD: 211, 2141..."
                  className="w-full px-3 py-2 border border-gray-300 rounded focus:ring-blue-500"
                />
              </div>
              
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Tên tài khoản <span className="text-red-500">*</span></label>
                <input
                  type="text"
                  name="tenTaiKhoan"
                  value={formData.tenTaiKhoan}
                  onChange={handleChange}
                  required
                  placeholder="VD: Tài sản cố định hữu hình"
                  className="w-full px-3 py-2 border border-gray-300 rounded focus:ring-blue-500"
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Tính chất (Loại tài khoản)</label>
                <select
                  name="loaiTaiKhoan"
                  value={formData.loaiTaiKhoan}
                  onChange={handleChange}
                  className="w-full px-3 py-2 border border-gray-300 rounded focus:ring-blue-500 bg-white"
                >
                  <option value="Tài sản">Tài sản (Dư Nợ)</option>
                  <option value="Nguồn vốn">Nguồn vốn (Dư Có)</option>
                  <option value="Lưỡng tính">Lưỡng tính (Nợ/Có)</option>
                  <option value="Doanh thu">Doanh thu</option>
                  <option value="Chi phí">Chi phí</option>
                </select>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Thuộc tài khoản cha (Nếu có)</label>
                <select
                  name="maTaiKhoanCha"
                  value={formData.maTaiKhoanCha || ''}
                  onChange={handleChange}
                  className="w-full px-3 py-2 border border-gray-300 rounded focus:ring-blue-500 bg-white"
                >
                  <option value="">-- Không có tài khoản cha --</option>
                  {accounts
                    // Không cho phép chọn chính nó làm cha
                    .filter(a => a.maTaiKhoan !== formData.maTaiKhoan) 
                    .map(acc => (
                      <option key={acc.id} value={acc.maTaiKhoan}>
                        {acc.maTaiKhoan} - {acc.tenTaiKhoan}
                      </option>
                    ))
                  }
                </select>
              </div>

              <div className="pt-4 flex justify-end gap-3 border-t mt-6">
                <button type="button" onClick={closeModal} className="px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-100 text-gray-700 transition-colors">
                  Hủy
                </button>
                <button type="submit" disabled={isSubmitting} className="flex items-center gap-2 px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 transition-colors shadow-sm">
                  <Save className="w-4 h-4" /> {isSubmitting ? 'Đang lưu...' : 'Lưu lại'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}