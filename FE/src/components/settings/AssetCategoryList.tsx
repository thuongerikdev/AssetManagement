import { useState, useMemo } from 'react';
import { Plus, Search, Edit, Trash2, X, Save, FolderTree, RefreshCw } from 'lucide-react';
import { toast } from "sonner";
import { assetCategoryApi, AssetCategory } from '../../api/assetCategoryApi';
import { useGlobalData } from '../../context/GlobalContext'; // <-- SỬ DỤNG GLOBAL CONTEXT

export function AssetCategoryList() {
  // 1. Lấy dữ liệu từ Kho chung (GlobalContext) - Tải 0 giây
  const { categories, isLoadingGlobal, refreshData } = useGlobalData();

  const [searchTerm, setSearchTerm] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingCategory, setEditingCategory] = useState<AssetCategory | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  
  const [formData, setFormData] = useState<AssetCategory>({
    maDanhMuc: '',
    tenDanhMuc: '',
    tienTo: '',
    thoiGianKhauHao: '',
    maTaiKhoan: ''
  });

  const handleOpenModal = (category?: AssetCategory) => {
    if (category) {
      setEditingCategory(category);
      setFormData({ 
        maDanhMuc: category.maDanhMuc || '', 
        tenDanhMuc: category.tenDanhMuc || '',
        tienTo: category.tienTo || '',
        thoiGianKhauHao: category.thoiGianKhauHao || '',
        maTaiKhoan: category.maTaiKhoan || ''
      });
    } else {
      setEditingCategory(null);
      setFormData({ 
        maDanhMuc: '', 
        tenDanhMuc: '',
        tienTo: '',
        thoiGianKhauHao: '',
        maTaiKhoan: ''
      });
    }
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setEditingCategory(null);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.maDanhMuc || !formData.tenDanhMuc) {
      toast.error('Vui lòng điền các trường bắt buộc.');
      return;
    }

    setIsSubmitting(true);
    try {
      const payload = {
        ...formData,
        thoiGianKhauHao: formData.thoiGianKhauHao ? Number(formData.thoiGianKhauHao) : 0
      };

      const submitData = editingCategory 
        ? { id: editingCategory.id, ...payload } 
        : payload;

      const data = editingCategory 
        ? await assetCategoryApi.update(submitData)
        : await assetCategoryApi.create(submitData);

      if (data.errorCode === 200) {
        toast.success(editingCategory ? 'Cập nhật thành công!' : 'Thêm mới thành công!');
        refreshData(); // Cập nhật lại kho dữ liệu chung cho toàn app
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

  const handleDelete = async (id: number) => {
    if (window.confirm('Bạn có chắc chắn muốn xóa danh mục này?')) {
      try {
        const data = await assetCategoryApi.delete(id);
        if (data.errorCode === 200) {
          toast.success('Xóa danh mục thành công!');
          refreshData(); // Cập nhật lại kho dữ liệu chung
        } else {
          toast.error(data.message || 'Không thể xóa danh mục.');
        }
      } catch (error) {
        toast.error('Không thể kết nối đến máy chủ.');
      }
    }
  };

  // ==========================================
  // 2. USE_MEMO BỘ LỌC ĐỂ RENDER MƯỢT MÀ
  // ==========================================
  const filteredCategories = useMemo(() => {
    return categories.filter(cat => 
      cat.tenDanhMuc?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      cat.maDanhMuc?.toLowerCase().includes(searchTerm.toLowerCase())
    );
  }, [categories, searchTerm]);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Danh mục Tài sản</h1>
          <p className="text-sm text-gray-500 mt-1">Cấu hình các loại tài sản và thông số khấu hao mặc định</p>
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
            className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-sm"
          >
            <Plus className="w-5 h-5" /> Thêm Danh mục
          </button>
        </div>
      </div>

      <div className="bg-white rounded-lg border border-gray-200 p-4">
        <div className="relative max-w-md">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" />
          <input
            type="text"
            placeholder="Tìm kiếm theo mã hoặc tên danh mục..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
          />
        </div>
      </div>

      <div className="bg-white rounded-lg border border-gray-200 overflow-hidden shadow-sm">
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50 border-b border-gray-200">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700 uppercase tracking-wider">Mã DM</th>
                <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700 uppercase tracking-wider">Tên Danh Mục</th>
                <th className="px-6 py-3 text-center text-xs font-semibold text-gray-700 uppercase tracking-wider">Tiền tố</th>
                <th className="px-6 py-3 text-center text-xs font-semibold text-gray-700 uppercase tracking-wider">T/g Khấu hao</th>
                <th className="px-6 py-3 text-center text-xs font-semibold text-gray-700 uppercase tracking-wider">Tài khoản</th>
                <th className="px-6 py-3 text-right text-xs font-semibold text-gray-700 uppercase tracking-wider">Thao tác</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200">
              {isLoadingGlobal && filteredCategories.length === 0 ? (
                <tr><td colSpan={6} className="px-6 py-8 text-center text-sm text-gray-500">Đang tải dữ liệu...</td></tr>
              ) : filteredCategories.length === 0 ? (
                <tr><td colSpan={6} className="px-6 py-8 text-center text-sm text-gray-500">Không tìm thấy danh mục nào.</td></tr>
              ) : (
                filteredCategories.map((cat) => (
                  <tr key={cat.id} className="hover:bg-gray-50 transition-colors">
                    <td className="px-6 py-4 whitespace-nowrap text-sm font-bold text-blue-600">{cat.maDanhMuc}</td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                      <div className="flex items-center gap-2">
                        <FolderTree className="w-4 h-4 text-gray-400" />
                        {cat.tenDanhMuc}
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-center text-gray-600">
                      <span className="px-2 py-1 bg-gray-100 rounded text-xs font-mono border border-gray-200">{cat.tienTo}</span>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-center text-gray-600">{cat.thoiGianKhauHao} tháng</td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-center text-gray-600 font-mono italic">{cat.maTaiKhoan}</td>
                    <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      <div className="flex justify-end gap-2">
                        <button onClick={() => handleOpenModal(cat)} className="p-1.5 text-blue-600 hover:bg-blue-100 rounded-md transition-colors" title="Sửa">
                          <Edit className="w-4 h-4" />
                        </button>
                        <button onClick={() => handleDelete(cat.id!)} className="p-1.5 text-red-600 hover:bg-red-100 rounded-md transition-colors" title="Xóa">
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
          <div className="bg-white rounded-xl shadow-2xl w-full max-w-2xl overflow-hidden border border-gray-200">
            <div className="flex justify-between items-center p-6 border-b border-gray-100 bg-white">
              <h2 className="text-xl font-bold text-gray-900 flex items-center gap-2">
                <FolderTree className="w-6 h-6 text-blue-600" />
                {editingCategory ? 'Sửa Danh mục' : 'Thêm Danh mục mới'}
              </h2>
              <button onClick={handleCloseModal} className="p-2 text-gray-400 hover:text-gray-600 hover:bg-gray-100 rounded-full transition-colors">
                <X className="w-6 h-6" />
              </button>
            </div>
            
            <form onSubmit={handleSubmit} className="p-6 space-y-4 bg-gray-50/30">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-semibold text-gray-700 mb-1">Mã Danh mục <span className="text-red-500">*</span></label>
                  <input
                    type="text"
                    value={formData.maDanhMuc}
                    onChange={(e) => setFormData({...formData, maDanhMuc: e.target.value})}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none bg-white"
                    placeholder="VD: DM_MAY_TINH"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-semibold text-gray-700 mb-1">Tên Danh mục <span className="text-red-500">*</span></label>
                  <input
                    type="text"
                    value={formData.tenDanhMuc}
                    onChange={(e) => setFormData({...formData, tenDanhMuc: e.target.value})}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none bg-white"
                    placeholder="VD: Máy vi tính, thiết bị điện tử"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-semibold text-gray-700 mb-1">Tiền tố tạo mã TS</label>
                  <input
                    type="text"
                    value={formData.tienTo}
                    onChange={(e) => setFormData({...formData, tienTo: e.target.value})}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none bg-white"
                    placeholder="VD: LAP, PC, MON"
                  />
                  <p className="text-[11px] text-gray-500 mt-1 italic">Dùng để sinh mã tự động (VD: LAP-001)</p>
                </div>
                <div>
                  <label className="block text-sm font-semibold text-gray-700 mb-1">T/g Khấu hao mặc định (Tháng)</label>
                  <input
                    type="number"
                    value={formData.thoiGianKhauHao}
                    onChange={(e) => setFormData({...formData, thoiGianKhauHao: e.target.value === '' ? '' : Number(e.target.value)})}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none bg-white"
                    placeholder="VD: 36"
                    min="1"
                  />
                </div>
                <div className="md:col-span-2">
                  <label className="block text-sm font-semibold text-gray-700 mb-1">Mã Tài khoản Kế toán</label>
                  <input
                    type="text"
                    value={formData.maTaiKhoan}
                    onChange={(e) => setFormData({...formData, maTaiKhoan: e.target.value})}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none bg-white"
                    placeholder="VD: 2112, 2113..."
                  />
                </div>
              </div>
              
              <div className="flex justify-end gap-3 mt-8 pt-4 border-t border-gray-100">
                <button type="button" onClick={handleCloseModal} className="px-5 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-100 font-medium transition-colors">
                  Hủy
                </button>
                <button 
                  type="submit" 
                  disabled={isSubmitting}
                  className="flex items-center gap-2 px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 font-bold shadow-md transition-all"
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