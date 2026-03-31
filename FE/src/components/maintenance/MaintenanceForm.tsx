import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router';
import { Save, X, AlertCircle } from 'lucide-react';
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
    ngayThucHien: new Date().toISOString().split('T')[0], // <-- Đổi tên thành ngayThucHien
    loaiBaoTri: '0',
    moTa: '',
    chiPhi: undefined,
    loaiChiPhi: undefined,
    nhaCungCap: '',
    ghiChu: '',
    trangThai: 0 
  });

  useEffect(() => {
    // Chỉ lấy các tài sản đang sử dụng (trangThai = 1) để bảo trì
    assetApi.getAll().then(res => {
      if (res.errorCode === 200) {
        setAssets(res.data.filter((a: TaiSan) => a.trangThai === 1));
      }
    });
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    
    // Chỉ ép kiểu số cho taiSanId, chiPhi và trangThai
    const numberFields = ['taiSanId', 'chiPhi', 'trangThai'];

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
        coChiPhi: hasCost // <-- Bổ sung dòng này để gửi cờ coChiPhi lên Backend
      } as any; // Tạm dùng any nếu Interface chưa cập nhật kịp
      
      // Xóa chi phí nếu không có
      if (!hasCost) {
        payload.chiPhi = 0; // Đổi về 0 thay vì undefined cho chuẩn JSON
        payload.loaiChiPhi = "";
      }

      const response = await maintenanceApi.create(payload);
      if (response.errorCode === 200) {
        toast.success('Tạo phiếu bảo trì thành công!');
        navigate('/maintenance');
      } else {
        toast.error(response.message || 'Lỗi khi tạo phiếu.');
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
          <h1 className="font-bold text-gray-900">Tạo Phiếu bảo trì</h1>
          <p className="text-sm text-gray-500 mt-1">Ghi nhận bảo trì và chi phí phát sinh</p>
        </div>
        <button
          onClick={() => navigate('/maintenance')}
          className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors"
        >
          <X className="w-5 h-5" /> Hủy
        </button>
      </div>

      <div className="bg-orange-50 border border-orange-200 rounded-lg p-4 flex items-start gap-3">
        <AlertCircle className="w-5 h-5 text-orange-600 mt-0.5" />
        <div className="flex-1">
          <p className="text-sm text-orange-900 font-medium">Logic hệ thống</p>
          <ul className="text-sm text-orange-700 mt-2 space-y-1 list-disc list-inside">
            <li>Nếu có chi phí → Bắt buộc chọn loại chi phí</li>
            <li>Chi phí sửa chữa → Hệ thống sinh chứng từ chi phí</li>
            <li>Nâng cấp tài sản → Tăng nguyên giá tài sản và tính lại khấu hao</li>
          </ul>
        </div>
      </div>

      <form onSubmit={handleSubmit} className="space-y-6">
        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <h3 className="font-semibold text-gray-900 mb-4">Thông tin tài sản</h3>
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
                <option value="">-- Chọn tài sản cần bảo trì --</option>
                {assets.map(asset => (
                  <option key={asset.id} value={asset.id}>{asset.maTaiSan} - {asset.tenTaiSan}</option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">Ngày bảo trì <span className="text-red-500">*</span></label>
              <input
                type="date"
                name="ngayThucHien" // <-- Đổi name ở đây
                value={formData.ngayThucHien} // <-- Đổi value ở đây
                onChange={handleChange}
                required
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">Loại bảo trì <span className="text-red-500">*</span></label>
              <select
                name="loaiBaoTri"
                value={formData.loaiBaoTri ?? '0'} // <-- Ép kiểu dự phòng là chuỗi '0'
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

            <div className="md:col-span-2">
              <label className="block text-sm font-medium text-gray-700 mb-2">Trạng thái phiếu</label>
              <select
                name="trangThai"
                value={formData.trangThai ?? 0}
                onChange={handleChange}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
              >
                <option value={0}>Chờ xử lý</option>
                <option value={1}>Đang thực hiện</option>
                <option value={2}>Hoàn thành</option>
              </select>
            </div>

            <div className="md:col-span-2">
              <label className="block text-sm font-medium text-gray-700 mb-2">Nội dung bảo trì <span className="text-red-500">*</span></label>
              <textarea
                name="moTa"
                value={formData.moTa}
                onChange={handleChange}
                required
                rows={3}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                placeholder="Mô tả chi tiết về công việc bảo trì..."
              />
            </div>
          </div>
        </div>

        {/* Cost Information */}
        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <h3 className="font-semibold text-gray-900 mb-4">Thông tin chi phí</h3>
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

          {hasCost && (
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6 pt-4 border-t border-gray-200">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">Chi phí phát sinh (VNĐ) <span className="text-red-500">*</span></label>
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

<div>
                <label className="block text-sm font-medium text-gray-700 mb-2">Loại chi phí <span className="text-red-500">*</span></label>
                <select
                  name="loaiChiPhi"
                  value={formData.loaiChiPhi ?? ''}
                  onChange={handleChange}
                  required={hasCost}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                >
                  <option value="">Chọn loại chi phí</option>
                  <option value="sua_chua">Chi phí sửa chữa (hạch toán chi phí)</option>
                  <option value="nang_cap">Nâng cấp tài sản (tăng nguyên giá)</option>
                </select>
              </div>

              {formData.loaiChiPhi !== undefined && (
                <div className="md:col-span-2 p-4 bg-blue-50 border border-blue-200 rounded-lg">
                  <p className="text-sm text-blue-900 font-medium mb-1">
                    {/* Sửa lại điều kiện check ở giao diện */}
                    {formData.loaiChiPhi === 'sua_chua' ? '📝 Chi phí sửa chữa' : '⬆️ Nâng cấp tài sản'}
                  </p>
                  <p className="text-sm text-blue-700">
                    {formData.loaiChiPhi === 'sua_chua' 
                      ? 'Hệ thống sẽ tự động ghi nhận vào chi phí (TK 627) và sinh chứng từ kế toán.'
                      : 'Hệ thống sẽ tự động tăng nguyên giá tài sản và tính lại khấu hao.'}
                  </p>
                </div>
              )}

              <div className="md:col-span-2">
                <label className="block text-sm font-medium text-gray-700 mb-2">Nhà cung cấp / Đơn vị bảo trì</label>
                <input
                  type="text"
                  name="nhaCungCap"
                  value={formData.nhaCungCap || ''}
                  onChange={handleChange}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                  placeholder="VD: Dell Vietnam, Apple Authorized..."
                />
              </div>
            </div>
          )}
        </div>

        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <h3 className="font-semibold text-gray-900 mb-4">Thông tin bổ sung</h3>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">Ghi chú</label>
            <textarea
              name="ghiChu"
              value={formData.ghiChu || ''}
              onChange={handleChange}
              rows={3}
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
              placeholder="Các ghi chú khác..."
            />
          </div>
        </div>

        <div className="flex justify-end gap-4">
          <button type="button" onClick={() => navigate('/maintenance')} className="px-6 py-2 border border-gray-300 rounded-lg hover:bg-gray-50">
            Hủy
          </button>
          <button type="submit" disabled={isSubmitting} className="flex items-center gap-2 px-6 py-2 bg-orange-600 text-white rounded-lg hover:bg-orange-700 disabled:opacity-50">
            <Save className="w-5 h-5" /> {isSubmitting ? 'Đang lưu...' : 'Lưu Phiếu bảo trì'}
          </button>
        </div>
      </form>
    </div>
  );
}