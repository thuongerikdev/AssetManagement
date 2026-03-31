// src/components/settings/DepartmentList.tsx
import { useState, useEffect } from 'react';
import { Plus, Search, Edit, Trash2, X, Save } from 'lucide-react';
import { toast } from "sonner";
import { departmentApi, Department } from '../../api/departmentApi'; // Import API vừa tạo

export function DepartmentList() {
  const [departments, setDepartments] = useState<Department[]>([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingDept, setEditingDept] = useState<Department | null>(null);
  const [formData, setFormData] = useState({ maPhongBan: '', tenPhongBan: '' });
  const [isLoading, setIsLoading] = useState(false);

  const fetchDepartments = async () => {
    try {
      setIsLoading(true);
      const data = await departmentApi.getAll(); // Gọi API sạch sẽ và gọn gàng
      
      if (data.errorCode === 200) {
        setDepartments(data.data);
      } else {
        toast.error('Lỗi khi tải danh sách phòng ban: ' + data.message);
      }
    } catch (error) {
      toast.error('Không thể kết nối đến máy chủ.');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchDepartments();
  }, []);

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

    try {
      // Xác định dữ liệu gửi đi (có ID nếu là update)
      const submitData = editingDept 
        ? { id: editingDept.id, ...formData } 
        : formData;

      // Gọi API tương ứng dựa vào trạng thái Edit
      const data = editingDept 
        ? await departmentApi.update(submitData)
        : await departmentApi.create(submitData);

      if (data.errorCode === 200) {
        toast.success(editingDept ? 'Cập nhật thành công!' : 'Thêm mới thành công!');
        fetchDepartments();
        handleCloseModal();
      } else {
        toast.error(data.message || 'Có lỗi xảy ra.');
      }
    } catch (error) {
      toast.error('Không thể kết nối đến máy chủ.');
    }
  };

  const handleDelete = async (id: string | number) => {
    if (window.confirm('Bạn có chắc chắn muốn xóa phòng ban này?')) {
      try {
        const data = await departmentApi.delete(id); // Gọi API Xóa
        
        if (data.errorCode === 200) {
          toast.success('Xóa phòng ban thành công!');
          fetchDepartments();
        } else {
          toast.error(data.message || 'Không thể xóa phòng ban.');
        }
      } catch (error) {
        toast.error('Không thể kết nối đến máy chủ.');
      }
    }
  };

  const filteredDepartments = departments.filter(dept => 
    dept.tenPhongBan?.toLowerCase().includes(searchTerm.toLowerCase()) ||
    dept.maPhongBan?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="space-y-6">
      {/* ... Toàn bộ phần giao diện (UI) bên dưới giữ nguyên như cũ của bạn ... */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Quản lý Phòng ban</h1>
          <p className="text-sm text-gray-500 mt-1">Cấu hình danh mục phòng ban trong hệ thống</p>
        </div>
        <button
          onClick={() => handleOpenModal()}
          className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
        >
          <Plus className="w-5 h-5" />
          Thêm Phòng ban
        </button>
      </div>

      <div className="bg-white rounded-lg border border-gray-200 p-4">
        <div className="relative max-w-md">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" />
          <input
            type="text"
            placeholder="Tìm kiếm theo mã hoặc tên phòng ban..."
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
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase tracking-wider">
                Mã Phòng ban
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase tracking-wider">
                Tên Phòng ban
              </th>
              <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase tracking-wider">
                Thao tác
              </th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-200">
            {isLoading ? (
              <tr>
                <td colSpan={3} className="px-6 py-4 text-center text-sm text-gray-500">Đang tải dữ liệu...</td>
              </tr>
            ) : filteredDepartments.length === 0 ? (
              <tr>
                <td colSpan={3} className="px-6 py-4 text-center text-sm text-gray-500">Không tìm thấy phòng ban nào.</td>
              </tr>
            ) : (
              filteredDepartments.map((dept) => (
                <tr key={dept.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-blue-600">
                    {dept.maPhongBan}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {dept.tenPhongBan}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                    <button
                      onClick={() => handleOpenModal(dept)}
                      className="text-blue-600 hover:text-blue-900 mr-4"
                      title="Sửa"
                    >
                      <Edit className="w-4 h-4 inline" />
                    </button>
                    <button
                      onClick={() => handleDelete(dept.id!)}
                      className="text-red-600 hover:text-red-900"
                      title="Xóa"
                    >
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
          <div className="bg-white rounded-lg shadow-xl w-full max-w-md p-6">
            <div className="flex justify-between items-center mb-4">
              <h2 className="text-xl font-bold text-gray-900">
                {editingDept ? 'Sửa Phòng ban' : 'Thêm Phòng ban mới'}
              </h2>
              <button onClick={handleCloseModal} className="text-gray-500 hover:text-gray-700">
                <X className="w-5 h-5" />
              </button>
            </div>
            
            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Mã Phòng ban <span className="text-red-500">*</span>
                </label>
                <input
                  type="text"
                  value={formData.maPhongBan}
                  onChange={(e) => setFormData({...formData, maPhongBan: e.target.value})}
                  className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                  placeholder="VD: PB_IT"
                  required
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Tên Phòng ban <span className="text-red-500">*</span>
                </label>
                <input
                  type="text"
                  value={formData.tenPhongBan}
                  onChange={(e) => setFormData({...formData, tenPhongBan: e.target.value})}
                  className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                  placeholder="VD: Phòng Công nghệ thông tin"
                  required
                />
              </div>
              <div className="flex justify-end gap-3 mt-6">
                <button
                  type="button"
                  onClick={handleCloseModal}
                  className="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50"
                >
                  Hủy
                </button>
                <button
                  type="submit"
                  className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
                >
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