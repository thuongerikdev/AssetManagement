import { useState, useEffect } from 'react';
import { Save, Building2, Hash, Calculator, FileText, Loader2 } from 'lucide-react';
import { toast } from 'sonner';
import { systemConfigApi, CauHinhHeThong } from '../../api/systemConfigApi';

export function SystemConfig() {
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  
  const [formData, setFormData] = useState<CauHinhHeThong>({
    tenCongTy: '',
    maSoThue: '',
    diaChi: '',
    tienToChungTu: 'CT',
    soBatDauChungTu: 1,
    phuongPhapKhauHaoMacDinh: 0,
    tuDongKhauHao: true,
    dinhDangMaTaiSan: '{DANH_MUC}-{SO_THU_TU}',
    doDaiMaTaiSan: 4
  });

  const fetchConfig = async () => {
    setIsLoading(true);
    try {
      const response = await systemConfigApi.get();
      if (response.errorCode === 200 && response.data) {
        setFormData(response.data);
      } else {
        toast.error(response.message || 'Không thể lấy cấu hình hệ thống.');
      }
    } catch (error) {
      toast.error('Lỗi kết nối đến máy chủ.');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchConfig();
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value, type } = e.target;
    
    let parsedValue: any = value;
    if (type === 'checkbox') {
      parsedValue = (e.target as HTMLInputElement).checked;
    } else if (type === 'number' || name === 'phuongPhapKhauHaoMacDinh') {
      parsedValue = value === '' ? 0 : Number(value);
    }

    setFormData(prev => ({
      ...prev,
      [name]: parsedValue
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSaving(true);
    try {
      const response = await systemConfigApi.update(formData);
      if (response.errorCode === 200) {
        toast.success('Cập nhật cấu hình hệ thống thành công!');
        fetchConfig(); // Reload lại ID nếu trước đó chưa có ID
      } else {
        toast.error(response.message || 'Lỗi khi cập nhật cấu hình.');
      }
    } catch (error) {
      toast.error('Lỗi kết nối đến máy chủ.');
    } finally {
      setIsSaving(false);
    }
  };

  if (isLoading) {
    return (
      <div className="flex flex-col items-center justify-center py-20 text-gray-500">
        <Loader2 className="w-8 h-8 animate-spin mb-4 text-blue-600" />
        <p>Đang tải cấu hình hệ thống...</p>
      </div>
    );
  }

  return (
    <div className="space-y-6 max-w-5xl mx-auto">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Cấu hình Hệ thống</h1>
          <p className="text-sm text-gray-500 mt-1">Cài đặt các thông số hoạt động chung của phần mềm</p>
        </div>
        <button 
          onClick={handleSubmit}
          disabled={isSaving}
          className="flex items-center gap-2 px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors disabled:opacity-70"
        >
          {isSaving ? <Loader2 className="w-5 h-5 animate-spin" /> : <Save className="w-5 h-5" />}
          {isSaving ? 'Đang lưu...' : 'Lưu Cấu hình'}
        </button>
      </div>

      <form onSubmit={handleSubmit} className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        
        {/* Thông tin đơn vị */}
        <div className="bg-white rounded-lg border border-gray-200 p-6 shadow-sm h-fit">
          <h3 className="font-semibold text-gray-900 mb-4 flex items-center gap-2">
            <Building2 className="w-5 h-5 text-blue-600" /> Thông tin Đơn vị
          </h3>
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Tên Công ty / Đơn vị <span className="text-red-500">*</span></label>
              <input type="text" name="tenCongTy" value={formData.tenCongTy || ''} onChange={handleChange} required className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500" />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Mã số thuế</label>
              <input type="text" name="maSoThue" value={formData.maSoThue || ''} onChange={handleChange} className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500" />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Địa chỉ</label>
              <input type="text" name="diaChi" value={formData.diaChi || ''} onChange={handleChange} className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500" />
            </div>
          </div>
        </div>

        {/* Cấu hình Mã Tài sản */}
        <div className="bg-white rounded-lg border border-gray-200 p-6 shadow-sm h-fit">
          <h3 className="font-semibold text-gray-900 mb-4 flex items-center gap-2">
            <Hash className="w-5 h-5 text-purple-600" /> Cấu hình Sinh mã Tài sản
          </h3>
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Định dạng Mã tài sản</label>
              <input type="text" name="dinhDangMaTaiSan" value={formData.dinhDangMaTaiSan || ''} onChange={handleChange} className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500" />
              <p className="text-xs text-gray-500 mt-1">Hỗ trợ: {'{DANH_MUC}'}, {'{SO_THU_TU}'}, {'{NAM}'}</p>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Độ dài số thứ tự</label>
              <input type="number" name="doDaiMaTaiSan" value={formData.doDaiMaTaiSan || 4} onChange={handleChange} min="3" max="10" className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500" />
              <p className="text-xs text-gray-500 mt-1">Ví dụ độ dài 4: 0001, 0002...</p>
            </div>
            <div className="p-3 bg-gray-50 rounded border border-gray-100 mt-2">
              <p className="text-sm text-gray-600">Mã xem trước: <span className="font-bold text-gray-900">LAP-{(1).toString().padStart(formData.doDaiMaTaiSan || 4, '0')}</span></p>
            </div>
          </div>
        </div>

        {/* Cấu hình Khấu hao */}
        <div className="bg-white rounded-lg border border-gray-200 p-6 shadow-sm h-fit">
          <h3 className="font-semibold text-gray-900 mb-4 flex items-center gap-2">
            <Calculator className="w-5 h-5 text-green-600" /> Cài đặt Khấu hao
          </h3>
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Phương pháp khấu hao mặc định</label>
              <select name="phuongPhapKhauHaoMacDinh" value={formData.phuongPhapKhauHaoMacDinh ?? 0} onChange={handleChange} className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500">
                <option value={0}>Khấu hao đường thẳng</option>
                <option value={1}>Khấu hao số dư giảm dần</option>
              </select>
            </div>
            <div className="flex items-center gap-3 pt-2">
              <input type="checkbox" id="tuDongKhauHao" name="tuDongKhauHao" checked={formData.tuDongKhauHao || false} onChange={handleChange} className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500" />
              <label htmlFor="tuDongKhauHao" className="text-sm font-medium text-gray-700 cursor-pointer">
                Tự động sinh chứng từ khấu hao cuối tháng
              </label>
            </div>
          </div>
        </div>

        {/* Cấu hình Chứng từ */}
        <div className="bg-white rounded-lg border border-gray-200 p-6 shadow-sm h-fit">
          <h3 className="font-semibold text-gray-900 mb-4 flex items-center gap-2">
            <FileText className="w-5 h-5 text-orange-600" /> Cài đặt Chứng từ Kế toán
          </h3>
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Tiền tố mã chứng từ</label>
              <input type="text" name="tienToChungTu" value={formData.tienToChungTu || ''} onChange={handleChange} className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500" placeholder="VD: CT" />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Số bắt đầu hiện tại</label>
              <input type="number" name="soBatDauChungTu" value={formData.soBatDauChungTu || 1} onChange={handleChange} min="1" className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500" />
            </div>
          </div>
        </div>

      </form>
    </div>
  );
}