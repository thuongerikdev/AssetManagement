import { useState, useMemo } from 'react';
import { Plus, Search, Edit, Trash2, X, Save, RefreshCw, Building2 } from 'lucide-react';
import { toast } from "sonner";
import { departmentApi, Department } from '../../api/departmentApi';
import { useGlobalData } from '../../context/GlobalContext'; // <-- SỬ DỤNG GLOBAL CONTEXT

export function DepartmentList() {
  // 1. Lấy dữ liệu từ Kho chung (GlobalContext) - Tải 0 giây
  const { departments, isLoadingGlobal, refreshData } = useGlobalData();

  const [searchTerm, setSearchTerm] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingDept, setEditingDept] = useState<Department | null>(null);
  const [formData, setFormData] = useState({ maPhongBan: '', tenPhongBan: '' });
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleOpenModal = (dept?: Department) => {
    if (dept) {
      setEditingDept(dept);
      setFormData({ maPhongBan: dept.maPhongBan, tenPhongBan: dept.tenPhongBan });
    } else {
      setEditingDept(null);
      setFormData({ maPhongBan: '', tenPhongBan: '' });
    }
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setEditingDept(null);
    setFormData({ maPhongBan: '', tenPhongBan: '' });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.maPhongBan || !formData.tenPhongBan) {
      toast.error('Vui lòng điền đầy đủ thông tin.');
      return;
    }

    setIsSubmitting(true);
    try {
      const submitData = editingDept 
        ? { id: editingDept.id, ...formData } 
        : formData;

      const data = editingDept 
        ? await departmentApi.update(submitData)
        : await departmentApi.create(submitData);

      if (data.errorCode === 200) {
        toast.success(editingDept ? 'Cập nhật thành công!' : 'Thêm mới thành công!');
        refreshData(); // Cập nhật lại kho dữ liệu chung cho toàn bộ App
        handleCloseModal();
      } else {
        toast.error(data.message || 'Có lỗi xảy ra.');
      }
    } catch (error) {
      toast.error('Không thể kết nối đến máy chủ.');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleDelete = async (id: string | number) => {
    if (window.confirm('Bạn có chắc chắn muốn xóa phòng ban này?')) {
      try {
        const data = await departmentApi.delete(id);
        
        if (data.errorCode === 200) {
          toast.success('Xóa phòng ban thành công!');
          refreshData(); // Làm mới kho dữ liệu chung
        } else {
          toast.error(data.message || 'Không thể xóa phòng ban.');
        }
      } catch (error) {
        toast.error('Không thể kết nối đến máy chủ.');
      }
    }
  };

  // ==========================================
  // 2. USE_MEMO BỘ LỌC ĐỂ RENDER MƯỢT MÀ
  // ==========================================
  const filteredDepartments = useMemo(() => {
    return departments.filter(dept => 
      dept.tenPhongBan?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      dept.maPhongBan?.toLowerCase().includes(searchTerm.toLowerCase())
    );
  }, [departments, searchTerm]);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900 text-2xl">Quản lý Phòng ban</h1>
          <p className="text-sm text-gray-500 mt-1">Cấu hình danh mục phòng ban trong hệ thống</p>
        </div>
        <div className="flex gap-2">
          {/* Nút Làm mới Data */}
          <button 
            onClick={() => refreshData()}
            disabled={isLoadingGlobal}
            className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-gray-600 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors disabled:opacity-50 shadow-sm"
            title="Tải lại dữ liệu"
          >
            <RefreshCw className={`w-4 h-4 ${isLoadingGlobal ? 'animate-spin text-blue-600' : ''}`} />
            <span className="hidden sm:block">Làm mới</span>
          </button>

          <button
            onClick={() => handleOpenModal()}
            className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 shadow-md transition-all font-medium"
          >
            <Plus className="w-5 h-5" />
            Thêm Phòng ban
          </button>
        </div>
      </div>

      <div className="bg-white rounded-xl border border-gray-200 p-4 shadow-sm">
        <div className="relative max-w-md">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" />
          <input
            type="text"
            placeholder="Tìm kiếm theo mã hoặc tên phòng ban..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none transition-all"
          />
        </div>
      </div>

      <div className="bg-white rounded-xl border border-gray-200 overflow-hidden shadow-sm">
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50 border-b border-gray-200">
              <tr>
                <th className="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                  Mã Phòng ban
                </th>
                <th className="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                  Tên Phòng ban
                </th>
                <th className="px-6 py-4 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider">
                  Thao tác
                </th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200">
              {isLoadingGlobal && filteredDepartments.length === 0 ? (
                <tr>
                  <td colSpan={3} className="px-6 py-8 text-center text-sm text-gray-500 italic">Đang tải dữ liệu...</td>
                </tr>
              ) : filteredDepartments.length === 0 ? (
                <tr>
                  <td colSpan={3} className="px-6 py-8 text-center text-sm text-gray-500 italic">Không tìm thấy phòng ban nào.</td>
                </tr>
              ) : (
                filteredDepartments.map((dept) => (
                  <tr key={dept.id} className="hover:bg-blue-50/30 transition-colors">
                    <td className="px-6 py-4 whitespace-nowrap text-sm font-bold text-blue-600">
                      {dept.maPhongBan}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900 font-medium">
                      <div className="flex items-center gap-2">
                        <Building2 className="w-4 h-4 text-gray-400" />
                        {dept.tenPhongBan}
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      <div className="flex justify-end gap-2">
                        <button
                          onClick={() => handleOpenModal(dept)}
                          className="p-1.5 text-blue-600 hover:bg-blue-100 rounded-md transition-colors"
                          title="Sửa"
                        >
                          <Edit className="w-4 h-4" />
                        </button>
                        <button
                          onClick={() => handleDelete(dept.id!)}
                          className="p-1.5 text-red-600 hover:bg-red-100 rounded-md transition-colors"
                          title="Xóa"
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

      {/* Modal Thêm/Sửa */}
      {isModalOpen && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-gray-900/50 backdrop-blur-sm p-4">
          <div className="bg-white rounded-2xl shadow-2xl w-full max-w-md overflow-hidden border border-gray-100">
            <div className="flex justify-between items-center p-6 border-b border-gray-100 bg-white">
              <h2 className="text-xl font-bold text-gray-900 flex items-center gap-2">
                <Building2 className="w-6 h-6 text-blue-600" />
                {editingDept ? 'Sửa Phòng ban' : 'Thêm Phòng ban mới'}
              </h2>
              <button onClick={handleCloseModal} className="p-2 text-gray-400 hover:text-gray-600 hover:bg-gray-100 rounded-full transition-colors">
                <X className="w-6 h-6" />
              </button>
            </div>
            
            <form onSubmit={handleSubmit} className="p-6 space-y-5 bg-gray-50/30">
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-1.5">
                  Mã Phòng ban <span className="text-red-500">*</span>
                </label>
                <input
                  type="text"
                  value={formData.maPhongBan}
                  onChange={(e) => setFormData({...formData, maPhongBan: e.target.value})}
                  className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none bg-white transition-all shadow-sm"
                  placeholder="VD: PB_IT"
                  required
                />
              </div>
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-1.5">
                  Tên Phòng ban <span className="text-red-500">*</span>
                </label>
                <input
                  type="text"
                  value={formData.tenPhongBan}
                  onChange={(e) => setFormData({...formData, tenPhongBan: e.target.value})}
                  className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none bg-white transition-all shadow-sm"
                  placeholder="VD: Phòng Công nghệ thông tin"
                  required
                />
              </div>
              
              <div className="flex justify-end gap-3 mt-8 pt-4 border-t border-gray-100">
                <button
                  type="button"
                  onClick={handleCloseModal}
                  className="px-5 py-2.5 border border-gray-300 rounded-xl text-gray-700 hover:bg-white font-medium transition-colors"
                >
                  Hủy
                </button>
                <button
                  type="submit"
                  disabled={isSubmitting}
                  className="flex items-center gap-2 px-6 py-2.5 bg-blue-600 text-white rounded-xl hover:bg-blue-700 disabled:opacity-50 font-bold shadow-md transition-all"
                >
                  {isSubmitting ? <RefreshCw className="w-4 h-4 animate-spin" /> : <Save className="w-4 h-4" />}
                  Lưu lại
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}