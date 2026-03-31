import { useState, useEffect } from 'react';
import { Plus, Search, Edit, Trash2, X, Save, FolderTree } from 'lucide-react';
import { toast } from "sonner";
import { assetCategoryApi, AssetCategory } from '../../api/assetCategoryApi';

export function AssetCategoryList() {
  const [categories, setCategories] = useState<AssetCategory[]>([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingCategory, setEditingCategory] = useState<AssetCategory | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  
  const [formData, setFormData] = useState<AssetCategory>({
    maDanhMuc: '',
    tenDanhMuc: '',
    tienTo: '',
    thoiGianKhauHao: '',
    maTaiKhoan: ''
  });

  const fetchCategories = async () => {
    try {
      setIsLoading(true);
      const data = await assetCategoryApi.getAll();
      if (data.errorCode === 200) {
        setCategories(data.data);
      } else {
        toast.error('Lỗi khi tải danh sách: ' + data.message);
      }
    } catch (error) {
      toast.error('Không thể kết nối đến máy chủ.');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchCategories();
  }, []);

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

    try {
      // Đảm bảo thoiGianKhauHao là số nguyên
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
        fetchCategories();
        handleCloseModal();
      } else {
        toast.error(data.message || 'Có lỗi xảy ra.');
      }
    } catch (error) {
      toast.error('Không thể kết nối đến máy chủ.');
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Bạn có chắc chắn muốn xóa danh mục này?')) {
      try {
        const data = await assetCategoryApi.delete(id);
        if (data.errorCode === 200) {
          toast.success('Xóa danh mục thành công!');
          fetchCategories();
        } else {
          toast.error(data.message || 'Không thể xóa danh mục.');
        }
      } catch (error) {
        toast.error('Không thể kết nối đến máy chủ.');
      }
    }
  };

  const filteredCategories = categories.filter(cat => 
    cat.tenDanhMuc?.toLowerCase().includes(searchTerm.toLowerCase()) ||
    cat.maDanhMuc?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Danh mục Tài sản</h1>
          <p className="text-sm text-gray-500 mt-1">Cấu hình các loại tài sản và thông số khấu hao mặc định</p>
        </div>
        <button
          onClick={() => handleOpenModal()}
          className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
        >
          <Plus className="w-5 h-5" />
          Thêm Danh mục
        </button>
      </div>

      <div className="bg-white rounded-lg border border-gray-200 p-4">
        <div className="relative max-w-md">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" />
          <input
            type="text"
            placeholder="Tìm kiếm theo mã hoặc tên danh mục..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          />
        </div>
      </div>

      <div className="bg-white rounded-lg border border-gray-200 overflow-hidden">
        <table className="w-full">
          <thead className="bg-gray-50 border-b border-gray-200">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase tracking-wider">Mã DM</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase tracking-wider">Tên Danh Mục</th>
              <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase tracking-wider">Tiền tố</th>
              <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase tracking-wider">T/g Khấu hao</th>
              <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase tracking-wider">Tài khoản</th>
              <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase tracking-wider">Thao tác</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-200">
            {isLoading ? (
              <tr><td colSpan={6} className="px-6 py-4 text-center text-sm text-gray-500">Đang tải dữ liệu...</td></tr>
            ) : filteredCategories.length === 0 ? (
              <tr><td colSpan={6} className="px-6 py-4 text-center text-sm text-gray-500">Không tìm thấy danh mục nào.</td></tr>
            ) : (
              filteredCategories.map((cat) => (
                <tr key={cat.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-blue-600">{cat.maDanhMuc}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    <div className="flex items-center gap-2">
                      <FolderTree className="w-4 h-4 text-gray-400" />
                      {cat.tenDanhMuc}
                    </div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-center text-gray-600">
                    <span className="px-2 py-1 bg-gray-100 rounded text-xs font-mono">{cat.tienTo}</span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-center text-gray-600">{cat.thoiGianKhauHao} tháng</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-center text-gray-600 font-mono">{cat.maTaiKhoan}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                    <button onClick={() => handleOpenModal(cat)} className="text-blue-600 hover:text-blue-900 mr-4" title="Sửa">
                      <Edit className="w-4 h-4 inline" />
                    </button>
                    <button onClick={() => handleDelete(cat.id!)} className="text-red-600 hover:text-red-900" title="Xóa">
                      <Trash2 className="w-4 h-4 inline" />
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* Modal Thêm/Sửa */}
      {isModalOpen && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
          <div className="bg-white rounded-lg shadow-xl w-full max-w-2xl p-6">
            <div className="flex justify-between items-center mb-4 border-b pb-3">
              <h2 className="text-xl font-bold text-gray-900">
                {editingCategory ? 'Sửa Danh mục' : 'Thêm Danh mục mới'}
              </h2>
              <button onClick={handleCloseModal} className="text-gray-500 hover:text-gray-700">
                <X className="w-5 h-5" />
              </button>
            </div>
            
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Mã Danh mục <span className="text-red-500">*</span></label>
                  <input
                    type="text"
                    value={formData.maDanhMuc}
                    onChange={(e) => setFormData({...formData, maDanhMuc: e.target.value})}
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500"
                    placeholder="VD: DM_MAY_TINH"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Tên Danh mục <span className="text-red-500">*</span></label>
                  <input
                    type="text"
                    value={formData.tenDanhMuc}
                    onChange={(e) => setFormData({...formData, tenDanhMuc: e.target.value})}
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500"
                    placeholder="VD: Máy vi tính, thiết bị điện tử"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Tiền tố tạo mã TS</label>
                  <input
                    type="text"
                    value={formData.tienTo}
                    onChange={(e) => setFormData({...formData, tienTo: e.target.value})}
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500"
                    placeholder="VD: LAP, PC, MON"
                  />
                  <p className="text-xs text-gray-500 mt-1">Dùng để sinh mã tự động (VD: LAP-001)</p>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">T/g Khấu hao mặc định (Tháng)</label>
                  <input
                    type="number"
                    value={formData.thoiGianKhauHao}
                    onChange={(e) => setFormData({...formData, thoiGianKhauHao: e.target.value === '' ? '' : Number(e.target.value)})}
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500"
                    placeholder="VD: 36"
                    min="1"
                  />
                </div>
                <div className="col-span-2">
                  <label className="block text-sm font-medium text-gray-700 mb-1">Mã Tài khoản Kế toán</label>
                  <input
                    type="text"
                    value={formData.maTaiKhoan}
                    onChange={(e) => setFormData({...formData, maTaiKhoan: e.target.value})}
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500"
                    placeholder="VD: 2112, 2113..."
                  />
                </div>
              </div>
              <div className="flex justify-end gap-3 mt-6 pt-4 border-t">
                <button type="button" onClick={handleCloseModal} className="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50">
                  Hủy
                </button>
                <button type="submit" className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700">
                  <Save className="w-4 h-4" />
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