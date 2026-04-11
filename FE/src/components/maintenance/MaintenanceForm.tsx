import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router';
import { Save, X } from 'lucide-react';
import { toast } from 'sonner';
import { maintenanceApi, BaoTriTaiSan } from '../../api/maintenanceApi';
import { assetApi, TaiSan } from '../../api/assetApi';

export function MaintenanceForm() {
  const navigate = useNavigate();

  const [assets, setAssets] = useState<TaiSan[]>([]);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [hasCost, setHasCost] = useState(false);

  const [formData, setFormData] = useState<Partial<BaoTriTaiSan>>({
    taiSanId: undefined,
    ngayThucHien: new Date().toISOString().split('T')[0],
    loaiBaoTri: '0',
    moTa: 'Ghi nhận bảo trì/sửa chữa', // Gắn cứng mặc định để Backend không báo lỗi require
    chiPhi: undefined,
    loaiChiPhi: '', // Bỏ logic loại chi phí
    nhaCungCap: '',
    ghiChu: '',
    trangThai: 2 // 2 = Hoàn thành (Lưu xong là chốt luôn)
  });

  useEffect(() => {
    // Chỉ lấy các tài sản đang sử dụng (trangThai = 2 hoặc 1 tuỳ config của bạn)
    assetApi.getAll().then(res => {
      if (res.errorCode === 200) {
        // Giả sử 2 là đang sử dụng (Dựa theo config cũ của bạn)
        setAssets(res.data.filter((a: TaiSan) => a.trangThai?.toString() === '2' || a.trangThai === 1 || a.trangThai?.toString() === 'DangSuDung'));
      }
    });
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    const numberFields = ['taiSanId', 'chiPhi'];

    setFormData(prev => ({
      ...prev,
      [name]: numberFields.includes(name) ? (value === '' ? undefined : Number(value)) : value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.taiSanId) {
      toast.error('Vui lòng chọn tài sản bảo trì!');
      return;
    }

    setIsSubmitting(true);
    try {
      const payload = { 
        ...formData,
        coChiPhi: hasCost 
      } as any; 
      
      if (!hasCost) {
        payload.chiPhi = 0; 
      }

      const response = await maintenanceApi.create(payload);
      if (response.errorCode === 200) {
        toast.success('Ghi nhận bảo trì thành công!');
        navigate('/maintenance');
      } else {
        toast.error(response.message || 'Lỗi khi lưu.');
      }
    } catch (error) {
      toast.error('Lỗi kết nối đến máy chủ.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="space-y-6 max-w-4xl mx-auto">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Ghi nhận Bảo trì / Sửa chữa</h1>
          <p className="text-sm text-gray-500 mt-1">Lưu lịch sử sửa chữa tài sản</p>
        </div>
        <button
          onClick={() => navigate('/maintenance')}
          className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors"
        >
          <X className="w-5 h-5" /> Hủy
        </button>
      </div>

      <form onSubmit={handleSubmit} className="space-y-6">
        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <h3 className="font-semibold text-gray-900 mb-4">Thông tin cơ bản</h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="md:col-span-2">
              <label className="block text-sm font-medium text-gray-700 mb-2">Chọn tài sản <span className="text-red-500">*</span></label>
              <select
                name="taiSanId"
                value={formData.taiSanId || ''}
                onChange={handleChange}
                required
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
              >
                <option value="">-- Chọn tài sản --</option>
                {assets.map(asset => (
                  <option key={asset.id} value={asset.id}>{asset.maTaiSan} - {asset.tenTaiSan}</option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">Ngày bảo trì <span className="text-red-500">*</span></label>
              <input
                type="date"
                name="ngayThucHien"
                value={formData.ngayThucHien}
                onChange={handleChange}
                required
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">Loại bảo trì <span className="text-red-500">*</span></label>
              <select
                name="loaiBaoTri"
                value={formData.loaiBaoTri ?? '0'}
                onChange={handleChange}
                required
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
              >
                <option value="0">Bảo trì định kỳ</option>
                <option value="1">Sửa chữa</option>
                <option value="2">Nâng cấp</option>
                <option value="3">Vệ sinh</option>
                <option value="4">Kiểm tra</option>
              </select>
            </div>
          </div>
        </div>

        {/* Cost Information */}
        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <h3 className="font-semibold text-gray-900 mb-4">Chi phí & Đơn vị thực hiện</h3>
          <div className="mb-4">
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="checkbox"
                checked={hasCost}
                onChange={(e) => setHasCost(e.target.checked)}
                className="rounded border-gray-300 text-blue-600 focus:ring-blue-500"
              />
              <span className="text-sm font-medium text-gray-700">Có phát sinh chi phí</span>
            </label>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6 pt-2">
            {hasCost && (
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">Tổng chi phí (VNĐ) <span className="text-red-500">*</span></label>
                <input
                  type="number"
                  name="chiPhi"
                  value={formData.chiPhi || ''}
                  onChange={handleChange}
                  required={hasCost}
                  min="0"
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                  placeholder="0"
                />
              </div>
            )}

            <div className={!hasCost ? "md:col-span-2" : ""}>
              <label className="block text-sm font-medium text-gray-700 mb-2">Nhà cung cấp / Đơn vị bảo trì</label>
              <input
                type="text"
                name="nhaCungCap"
                value={formData.nhaCungCap || ''}
                onChange={handleChange}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                placeholder="VD: Dell Vietnam, Thợ ngoài..."
              />
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <h3 className="font-semibold text-gray-900 mb-4">Ghi chú thêm</h3>
          <div>
            <textarea
              name="ghiChu"
              value={formData.ghiChu || ''}
              onChange={handleChange}
              rows={3}
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
              placeholder="Ghi chú tình trạng máy, linh kiện thay thế..."
            />
          </div>
        </div>

        <div className="flex justify-end gap-4">
          <button type="button" onClick={() => navigate('/maintenance')} className="px-6 py-2 border border-gray-300 rounded-lg hover:bg-gray-50">
            Hủy
          </button>
          <button type="submit" disabled={isSubmitting} className="flex items-center gap-2 px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50">
            <Save className="w-5 h-5" /> {isSubmitting ? 'Đang lưu...' : 'Lưu thông tin'}
          </button>
        </div>
      </form>
    </div>
  );
}