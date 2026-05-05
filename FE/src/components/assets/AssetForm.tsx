import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router';
import { Save, AlertCircle } from 'lucide-react';
import { toast } from "sonner";
import { assetApi, TaiSan } from '../../api/assetApi';
import { departmentApi, Department } from '../../api/departmentApi';
import { assetCategoryApi, AssetCategory } from '../../api/assetCategoryApi';
import { accountApi, TaiKhoanKeToan } from '../../api/accountApi';

export function AssetForm() {
  const navigate = useNavigate();
  const { id } = useParams();
  const isEdit = !!id;

  const [departments, setDepartments] = useState<Department[]>([]);
  const [categories, setCategories] = useState<AssetCategory[]>([]);
  const [accounts, setAccounts] = useState<TaiKhoanKeToan[]>([]);
  
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
    nguoiDungId: undefined,
    phuongPhapKhauHao: 0, 
    trangThai: 0,
    maTaiKhoan: ''
  });

  useEffect(() => {
    departmentApi.getAll().then(res => { if(res.errorCode === 200) setDepartments(res.data) });
    assetCategoryApi.getAll().then(res => { if(res.errorCode === 200) setCategories(res.data) });
    accountApi.getAll().then(res => { if(res.errorCode === 200) setAccounts(res.data) });

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

  useEffect(() => {
    if (!isEdit && formData.danhMucId) {
      assetApi.generateCode(formData.danhMucId).then(res => {
        if (res.errorCode === 200 && res.data?.maTaiSan) {
          setFormData(prev => ({ ...prev, maTaiSan: res.data.maTaiSan }));
        }
      }).catch(() => {});
    } else if (!isEdit && !formData.danhMucId) {
      setFormData(prev => ({ ...prev, maTaiSan: '' }));
    }
  }, [formData.danhMucId, isEdit]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    const numberFields = ['danhMucId', 'phongBanId', 'nguoiDungId', 'nguyenGia', 'thoiGianKhauHao', 'phuongPhapKhauHao', 'trangThai'];
    const parsedValue = numberFields.includes(name) ? (value === '' ? undefined : Number(value)) : value;

    setFormData(prev => {
      const newData = { ...prev, [name]: parsedValue };
      
      const currentPhongBanId = name === 'phongBanId' ? parsedValue : prev.phongBanId;
      const currentNguoiDungId = name === 'nguoiDungId' ? parsedValue : prev.nguoiDungId;

      if (currentPhongBanId && currentNguoiDungId) {
        if (prev.trangThai === 0) newData.trangThai = 1; 
      } else {
        if (prev.trangThai === 1) newData.trangThai = 0; 
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
        navigate(-1);
      } else {
        toast.error(response.message || 'Lỗi xử lý!');
      }
    } catch (error) {
      toast.error('Lỗi kết nối máy chủ.');
    }
  };

  return (
    <div className="max-w-5xl mx-auto pb-12">
      <div className="mb-6">
        <h1 className="font-bold text-gray-900 text-2xl">{isEdit ? 'Chỉnh sửa Tài sản' : 'Thêm Tài sản mới'}</h1>
        <p className="text-sm text-gray-500 mt-1">Nhập thông tin chi tiết của tài sản cố định</p>
      </div>

      {/* Alert Note Box */}
      <div className="bg-blue-50 border border-blue-200 rounded-lg p-4 flex items-start gap-3 mb-6">
        <AlertCircle className="w-5 h-5 text-blue-600 mt-0.5 shrink-0" />
        <div>
          <h4 className="text-sm font-semibold text-blue-900">Lưu ý khi {isEdit ? 'sửa' : 'thêm'} tài sản</h4>
          <p className="text-sm text-blue-700 mt-1">
            {isEdit 
              ? 'Phần thông tin phân bổ (Phòng ban, Người nhận) đã bị khóa để đảm bảo lịch sử hệ thống. Vui lòng dùng module "Cấp phát & Điều chuyển" nếu muốn đổi người sử dụng.'
              : 'Mã tài sản sẽ được tự động tạo theo công thức sau khi bạn chọn Nhóm tài sản (Danh mục). Hệ thống sẽ tự động tạo bút toán và sinh chứng từ ghi tăng tài sản khi tài sản được cấp phát thành công.'
            }
          </p>
        </div>
      </div>

      <form onSubmit={handleSubmit} className="space-y-6">
        
        {/* Block 1: Thông tin cơ bản */}
        <div className="bg-white rounded-xl border border-gray-200 p-6 shadow-sm">
          <h3 className="font-bold text-gray-900 mb-5 text-lg">Thông tin cơ bản</h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Tên tài sản <span className="text-red-500">*</span></label>
              <input type="text" name="tenTaiSan" required value={formData.tenTaiSan} onChange={handleChange} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" placeholder="VD: MacBook Pro 16 inch M3 Max" />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Nhóm tài sản (Danh mục) <span className="text-red-500">*</span></label>
              <select name="danhMucId" required value={formData.danhMucId || ''} onChange={handleChange} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500">
                <option value="">-- Chọn nhóm tài sản --</option>
                {categories.map(cat => <option key={cat.id} value={cat.id}>{cat.tenDanhMuc}</option>)}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Mã Tài sản</label>
              <input 
                type="text" 
                name="maTaiSan" 
                required 
                value={formData.maTaiSan} 
                onChange={handleChange} 
                readOnly // Khóa vĩnh viễn không cho sửa tay
                className="w-full px-4 py-2 border border-gray-300 rounded-lg bg-gray-100 cursor-not-allowed text-gray-500 font-medium focus:outline-none" 
                placeholder="Tự động sinh khi chọn nhóm tài sản" 
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Số Serial</label>
              <input type="text" name="soSeri" value={formData.soSeri || ''} onChange={handleChange} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" placeholder="Nhập số Serial" />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Nguồn gốc / Nhà sản xuất</label>
              <input type="text" name="nhaSanXuat" value={formData.nhaSanXuat || ''} onChange={handleChange} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" placeholder="Nhập địa chỉ hoặc tên NSX" />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Mô tả thêm</label>
              <input type="text" name="moTa" value={formData.moTa || ''} onChange={handleChange} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" placeholder="Cấu hình, tình trạng..." />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Ngày mua <span className="text-red-500">*</span></label>
              <input type="date" name="ngayMua" required value={formData.ngayMua} onChange={handleChange} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Nguyên giá (VNĐ) <span className="text-red-500">*</span></label>
              <input type="number" name="nguyenGia" required value={formData.nguyenGia} onChange={handleChange} min="0" className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" placeholder="VD: 50000000" />
            </div>
          </div>
        </div>

        {/* Block 2: Thông tin kế toán */}
        <div className="bg-white rounded-xl border border-gray-200 p-6 shadow-sm">
          <h3 className="font-bold text-gray-900 mb-5 text-lg">Thông tin kế toán</h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Tài khoản Kế toán <span className="text-red-500">*</span></label>
              <select 
                name="maTaiKhoan" 
                value={formData.maTaiKhoan || ''} 
                onChange={handleChange} 
                required
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
              >
                <option value="">-- Chọn tài khoản --</option>
                {accounts.map(acc => (
                  <option key={acc.id} value={acc.maTaiKhoan}>
                    {acc.maTaiKhoan} {acc.tenTaiKhoan ? `- ${acc.tenTaiKhoan}` : ''}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Thời gian Khấu hao (Tháng) <span className="text-red-500">*</span></label>
              <input type="number" name="thoiGianKhauHao" required value={formData.thoiGianKhauHao} onChange={handleChange} min="1" className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" placeholder="VD: 36, 48, 60..." />
            </div>

            <div className="md:col-span-2">
              <label className="block text-sm font-medium text-gray-700 mb-1">Phương pháp khấu hao <span className="text-red-500">*</span></label>
              <select 
                value={0} 
                disabled 
                className="w-full px-4 py-2 border border-gray-300 rounded-lg bg-gray-100 cursor-not-allowed text-gray-500 font-medium"
              >
                <option value={0}>Khấu hao đường thẳng</option>
              </select>
              <input type="hidden" name="phuongPhapKhauHao" value={0} />
            </div>
          </div>
        </div>

        {/* Block 3: Thông tin cấp phát (KHÓA LẠI KHI EDIT) */}
        <div className="bg-white rounded-xl border border-gray-200 p-6 shadow-sm">
          <div className="flex items-center justify-between mb-5">
            <h3 className="font-bold text-gray-900 text-lg">Thông tin cấp phát</h3>
            {isEdit && <span className="text-sm font-medium text-red-500 bg-red-50 px-3 py-1 rounded-full">Đã khóa (Chỉ dành cho Thêm mới)</span>}
          </div>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Phòng ban sử dụng</label>
              <select 
                name="phongBanId" 
                value={formData.phongBanId || ''} 
                onChange={handleChange} 
                disabled={isEdit} // Khóa select
                className={`w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 ${isEdit ? 'bg-gray-100 cursor-not-allowed text-gray-500' : ''}`}
              >
                <option value="">-- Chưa gắn phòng ban --</option>
                {departments.map(dept => <option key={dept.id} value={dept.id}>{dept.tenPhongBan}</option>)}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Người nhận (Mã nhân viên)</label>
              <input 
                type="number" 
                name="nguoiDungId" 
                value={formData.nguoiDungId || ''} 
                onChange={handleChange} 
                readOnly={isEdit} // Khóa input
                placeholder="Nhập ID người nhận" 
                className={`w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 ${isEdit ? 'bg-gray-100 cursor-not-allowed text-gray-500' : ''}`} 
              />
            </div>
            
            <div className="md:col-span-2 p-4 bg-gray-50 border border-gray-200 rounded-lg flex items-center">
              <div className="flex-1">
                <label className="block text-sm font-medium text-gray-700 mb-1">Trạng thái luồng cấp phát</label>
                <select 
                  name="trangThai" 
                  value={formData.trangThai ?? 0} 
                  onChange={handleChange} 
                  disabled={isEdit} // Khóa trạng thái
                  className={`w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 ${isEdit ? 'bg-gray-100 cursor-not-allowed text-gray-500' : 'bg-white'}`}
                >
                  <option value={0}>Chưa cấp phát (Sẵn sàng)</option>
                  <option value={1}>Chờ người dùng xác nhận</option>
                  <option value={2}>Đang sử dụng (Đã xác nhận)</option>
                  <option value={3}>Đã thanh lý</option>
                </select>
              </div>
            </div>
          </div>
        </div>

        {/* Footer Buttons */}
        <div className="flex justify-end gap-3 pt-2">
          <button type="button" onClick={() => navigate(-1)} className="px-6 py-2.5 border border-gray-300 text-gray-700 font-medium rounded-lg hover:bg-gray-50 transition-colors">
            Hủy
          </button>
          <button type="submit" className="flex items-center gap-2 px-8 py-2.5 bg-blue-600 text-white font-medium rounded-lg hover:bg-blue-700 shadow-sm transition-colors">
            <Save className="w-5 h-5" /> Lưu Tài sản
          </button>
        </div>

      </form>
    </div>
  );
}