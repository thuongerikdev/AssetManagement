import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router';
import { Save, X, Calculator, User } from 'lucide-react';
import { toast } from "sonner";
import { assetApi, TaiSan } from '../../api/assetApi';
import { departmentApi, Department } from '../../api/departmentApi';
import { assetCategoryApi, AssetCategory } from '../../api/assetCategoryApi';

export function AssetForm() {
  const navigate = useNavigate();
  const { id } = useParams();
  const isEdit = !!id;

  const [departments, setDepartments] = useState<Department[]>([]);
  const [categories, setCategories] = useState<AssetCategory[]>([]);
  
  const [formData, setFormData] = useState<Partial<TaiSan>>({
    maTaiSan: '',
    tenTaiSan: '',
    danhMucId: undefined,
    nhaSanXuat: '',
    soSeri: '',
    moTa: '',
    ngayMua: new Date().toISOString().split('T')[0],
    nguyenGia: 0,
    thoiGianKhauHao: 36,
    phongBanId: undefined,
    nguoiDungId: undefined, // Thêm trường quản lý người dùng
    phuongPhapKhauHao: 0, 
    trangThai: 0 // 0 = Chưa cấp phát (Mặc định)
  });

  useEffect(() => {
    departmentApi.getAll().then(res => { if(res.errorCode === 200) setDepartments(res.data) });
    assetCategoryApi.getAll().then(res => { if(res.errorCode === 200) setCategories(res.data) });

    if (isEdit && id) {
      assetApi.getById(Number(id)).then(res => {
        if(res.errorCode === 200) {
          const fetchedData = { ...res.data };
          if (fetchedData.ngayMua) {
            fetchedData.ngayMua = fetchedData.ngayMua.split('T')[0];
          }
          setFormData(fetchedData);
        }
      });
    }
  }, [id, isEdit]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    const numberFields = ['danhMucId', 'phongBanId', 'nguoiDungId', 'nguyenGia', 'thoiGianKhauHao', 'phuongPhapKhauHao', 'trangThai'];
    const parsedValue = numberFields.includes(name) ? (value === '' ? undefined : Number(value)) : value;

    setFormData(prev => {
      const newData = { ...prev, [name]: parsedValue };
      
      // AUTO LOGIC: Nếu đang ở trạng thái "Chưa cấp phát" mà gán ID Người dùng -> Tự nhảy sang "Chờ xác nhận"
      if (name === 'nguoiDungId' && parsedValue !== undefined && prev.trangThai === 0) {
        newData.trangThai = 1; // 1 = Chờ xác nhận
      }
      // Nếu xóa ID Người dùng mà đang ở trạng thái "Chờ xác nhận" -> Quay về "Chưa cấp phát"
      if (name === 'nguoiDungId' && parsedValue === undefined && prev.trangThai === 1) {
        newData.trangThai = 0; 
      }

      return newData;
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const payload = { ...formData } as TaiSan;
      if (!payload.phongBanId) payload.phongBanId = undefined;
      if (!payload.danhMucId) payload.danhMucId = undefined;
      if (!payload.nguoiDungId) payload.nguoiDungId = undefined;

      const response = isEdit 
        ? await assetApi.update(payload) 
        : await assetApi.create(payload);

      if (response.errorCode === 200) {
        toast.success(isEdit ? 'Cập nhật thành công!' : 'Thêm mới thành công!');
        navigate('/assets');
      } else {
        toast.error(response.message || 'Lỗi xử lý!');
      }
    } catch (error) {
      toast.error('Lỗi kết nối máy chủ.');
    }
  };

  return (
    <div className="space-y-6 max-w-4xl mx-auto">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900 text-xl">{isEdit ? 'Chỉnh sửa Tài sản' : 'Thêm Tài sản mới'}</h1>
        </div>
        <button onClick={() => navigate('/assets')} className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50">
          <X className="w-5 h-5" /> Hủy
        </button>
      </div>

      <form onSubmit={handleSubmit} className="space-y-6">
        {/* Basic Info */}
        <div className="bg-white rounded-xl border border-gray-200 p-6 shadow-sm">
          <h3 className="font-semibold text-gray-900 mb-4 text-lg">Thông tin cơ bản</h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Mã Tài sản <span className="text-red-500">*</span></label>
              <input type="text" name="maTaiSan" required value={formData.maTaiSan} onChange={handleChange} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" placeholder="VD: LAP-001" />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Tên tài sản <span className="text-red-500">*</span></label>
              <input type="text" name="tenTaiSan" required value={formData.tenTaiSan} onChange={handleChange} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Danh mục</label>
              <select name="danhMucId" value={formData.danhMucId || ''} onChange={handleChange} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500">
                <option value="">-- Chọn danh mục --</option>
                {categories.map(cat => <option key={cat.id} value={cat.id}>{cat.tenDanhMuc}</option>)}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Số Serial</label>
              <input type="text" name="soSeri" value={formData.soSeri || ''} onChange={handleChange} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Nhà sản xuất</label>
              <input type="text" name="nhaSanXuat" value={formData.nhaSanXuat || ''} onChange={handleChange} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" />
            </div>

            <div className="md:col-span-2">
              <label className="block text-sm font-medium text-gray-700 mb-1">Mô tả</label>
              <textarea name="moTa" value={formData.moTa || ''} onChange={handleChange} rows={3} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" />
            </div>
          </div>
        </div>

        {/* Phân bổ & Cấp phát Info */}
        <div className="bg-white rounded-xl border border-gray-200 p-6 shadow-sm">
          <h3 className="font-semibold text-gray-900 mb-4 text-lg flex items-center gap-2">
            <User className="w-5 h-5 text-blue-600" /> Quản lý & Cấp phát
          </h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Thuộc Phòng ban <span className="text-gray-400 font-normal">(Quản lý tài sản)</span></label>
              <select name="phongBanId" value={formData.phongBanId || ''} onChange={handleChange} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500">
                <option value="">-- Chưa gắn phòng ban --</option>
                {departments.map(dept => <option key={dept.id} value={dept.id}>{dept.tenPhongBan}</option>)}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Cấp phát cho User (ID)</label>
              <input 
                type="number" 
                name="nguoiDungId" 
                value={formData.nguoiDungId || ''} 
                onChange={handleChange} 
                placeholder="Nhập ID User (Hệ thống ngoài)" 
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" 
              />
            </div>
            
            <div className="md:col-span-2 p-4 bg-blue-50 border border-blue-100 rounded-lg flex gap-4 items-center">
              <div className="flex-1">
                <label className="block text-sm font-bold text-blue-900 mb-1">Trạng thái luồng cấp phát</label>
                <select name="trangThai" value={formData.trangThai ?? 0} onChange={handleChange} className="...">
                  <option value={0}>Chưa cấp phát (Sẵn sàng)</option>
                  <option value={1}>Chờ người dùng xác nhận</option>
                  <option value={2}>Đang sử dụng (Đã xác nhận)</option>
                  <option value={3}>Đã thanh lý</option>
                </select>
              </div>
            </div>
          </div>
        </div>

        {/* Kế toán Info */}
        <div className="bg-white rounded-xl border border-gray-200 p-6 shadow-sm">
          <h3 className="font-semibold text-gray-900 mb-4 text-lg flex items-center gap-2">
            <Calculator className="w-5 h-5 text-blue-600" /> Thông tin kế toán
          </h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Ngày mua <span className="text-red-500">*</span></label>
              <input type="date" name="ngayMua" required value={formData.ngayMua} onChange={handleChange} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Nguyên giá (VNĐ) <span className="text-red-500">*</span></label>
              <input type="number" name="nguyenGia" required value={formData.nguyenGia} onChange={handleChange} min="0" className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Phương pháp khấu hao <span className="text-red-500">*</span></label>
              <select name="phuongPhapKhauHao" value={formData.phuongPhapKhauHao ?? ''} onChange={handleChange} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500">
                <option value={0}>Khấu hao đường thẳng</option>
                <option value={1}>Khấu hao số dư giảm dần</option>
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Thời gian Khấu hao (Tháng) <span className="text-red-500">*</span></label>
              <input type="number" name="thoiGianKhauHao" required value={formData.thoiGianKhauHao} onChange={handleChange} min="1" className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" />
            </div>

            <div className="md:col-span-2">
              <label className="block text-sm font-medium text-gray-700 mb-1">Tài khoản Kế toán</label>
              <input type="text" name="maTaiKhoan" value={formData.maTaiKhoan || ''} onChange={handleChange} placeholder="VD: 2112" className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" />
            </div>
          </div>
        </div>

        <div className="flex justify-end gap-4 pb-8">
          <button type="submit" className="flex items-center gap-2 px-8 py-3 bg-blue-600 text-white font-medium rounded-lg hover:bg-blue-700 shadow-md">
            <Save className="w-5 h-5" /> Lưu Tài sản
          </button>
        </div>
      </form>
    </div>
  );
}