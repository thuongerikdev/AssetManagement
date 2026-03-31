import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router';
import { Save, X, AlertCircle, Calculator } from 'lucide-react';
import { toast } from 'sonner';
import { assetApi, TaiSan } from '../../api/assetApi';
import { liquidationApi, ThanhLyTaiSan } from '../../api/liquidationApi';

export function LiquidationForm() {
  const navigate = useNavigate();

  const [assets, setAssets] = useState<TaiSan[]>([]);
  const [selectedAsset, setSelectedAsset] = useState<TaiSan | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const [formData, setFormData] = useState<Partial<ThanhLyTaiSan>>({
    taiSanId: undefined,
    ngayThanhLy: new Date().toISOString().split('T')[0],
    giaTriThanhLy: 0,
    lyDo: '',
    ghiChu: '',
    trangThai: 'ChoDuyet' // Giả sử mặc định chọn: 1 - Đã hoàn thành (Chốt thanh lý luôn)
  });

  useEffect(() => {
    // Load tài sản, chỉ hiển thị tài sản CHƯA thanh lý (trangThai !== 3)
    assetApi.getAll().then(res => {
      if (res.errorCode === 200) {
        setAssets(res.data.filter((a: TaiSan) => a.trangThai !== 3));
      }
    });
  }, []);

  const handleAssetSelect = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const assetId = Number(e.target.value);
    const asset = assets.find(a => a.id === assetId) || null;
    
    setSelectedAsset(asset);
    setFormData(prev => ({
      ...prev,
      taiSanId: asset ? assetId : undefined
    }));
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    const numberFields = ['giaTriThanhLy', 'trangThai'];

    setFormData(prev => ({
      ...prev,
      [name]: numberFields.includes(name) ? (value === '' ? undefined : Number(value)) : value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.taiSanId) {
      toast.error('Vui lòng chọn tài sản để thanh lý.');
      return;
    }

    setIsSubmitting(true);
    try {
      const payload = { ...formData } as ThanhLyTaiSan;
      
      const response = await liquidationApi.create(payload);
      if (response.errorCode === 200) {
        toast.success('Tạo phiếu thanh lý thành công!');
        navigate('/liquidation');
      } else {
        toast.error(response.message || 'Lỗi khi tạo phiếu.');
      }
    } catch (error) {
      toast.error('Lỗi kết nối máy chủ.');
    } finally {
      setIsSubmitting(false);
    }
  };

  const formatCurrency = (value?: number) => {
    if (value === undefined) return '0 ₫';
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
  };

  // Tính toán Lãi/Lỗ tạm thời trên UI (Backend cũng sẽ tự tính toán)
  const remainingValue = selectedAsset?.giaTriConLai || 0;
  const liquidationValue = formData.giaTriThanhLy || 0;
  const profitLoss = liquidationValue - remainingValue;

  return (
    <div className="space-y-6 max-w-4xl mx-auto">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Tạo Phiếu thanh lý</h1>
          <p className="text-sm text-gray-500 mt-1">Ghi nhận thanh lý tài sản cố định</p>
        </div>
        <button
          onClick={() => navigate('/liquidation')}
          className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors"
        >
          <X className="w-5 h-5" />
          Hủy
        </button>
      </div>

      {/* Info Alert */}
      <div className="bg-red-50 border border-red-200 rounded-lg p-4 flex items-start gap-3">
        <AlertCircle className="w-5 h-5 text-red-600 mt-0.5" />
        <div className="flex-1">
          <p className="text-sm text-red-900 font-medium">Lưu ý khi thanh lý</p>
          <ul className="text-sm text-red-700 mt-2 space-y-1 list-disc list-inside">
            <li>Hệ thống sẽ tự động lấy sổ sách để tính toán lãi/lỗ thanh lý</li>
            <li>Tự động sinh chứng từ giảm tài sản và bút toán kế toán</li>
            <li>Trạng thái tài sản sẽ tự động được chuyển sang "Đã thanh lý" nếu phiếu được duyệt/hoàn thành</li>
          </ul>
        </div>
      </div>

      {/* Form */}
      <form onSubmit={handleSubmit} className="space-y-6">
        {/* Asset Selection */}
        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <h3 className="font-semibold text-gray-900 mb-4">Chọn tài sản thanh lý</h3>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Tài sản <span className="text-red-500">*</span>
            </label>
            <select
              name="taiSanId"
              value={formData.taiSanId || ''}
              onChange={handleAssetSelect}
              required
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            >
              <option value="">-- Chọn tài sản cần thanh lý --</option>
              {assets.map(asset => (
                <option key={asset.id} value={asset.id}>
                  {asset.maTaiSan} - {asset.tenTaiSan} (Còn lại: {formatCurrency(asset.giaTriConLai)})
                </option>
              ))}
            </select>
          </div>
        </div>

        {/* Asset Information */}
        {selectedAsset && (
          <div className="bg-white rounded-lg border border-gray-200 p-6">
            <h3 className="font-semibold text-gray-900 mb-4">Dữ liệu sổ sách tài sản</h3>
            <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
              <div className="p-4 bg-gray-50 rounded-lg">
                <p className="text-xs text-gray-600 mb-1">Mã tài sản</p>
                <p className="text-sm font-medium text-gray-900">{selectedAsset.maTaiSan}</p>
              </div>
              <div className="p-4 bg-gray-50 rounded-lg col-span-2">
                <p className="text-xs text-gray-600 mb-1">Tên tài sản</p>
                <p className="text-sm font-medium text-gray-900">{selectedAsset.tenTaiSan}</p>
              </div>
              <div className="p-4 bg-blue-50 rounded-lg">
                <p className="text-xs text-blue-700 mb-1">Nguyên giá</p>
                <p className="text-sm font-semibold text-blue-900">{formatCurrency(selectedAsset.nguyenGia)}</p>
              </div>
              <div className="p-4 bg-orange-50 rounded-lg">
                <p className="text-xs text-orange-700 mb-1">KH lũy kế</p>
                <p className="text-sm font-semibold text-orange-900">{formatCurrency(selectedAsset.khauHaoLuyKe)}</p>
              </div>
              <div className="p-4 bg-green-50 rounded-lg">
                <p className="text-xs text-green-700 mb-1">Giá trị còn lại</p>
                <p className="text-sm font-bold text-green-900">{formatCurrency(remainingValue)}</p>
              </div>
            </div>
          </div>
        )}

        {/* Liquidation Details */}
        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <h3 className="font-semibold text-gray-900 mb-4">Thông tin thanh lý</h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Ngày thanh lý <span className="text-red-500">*</span>
              </label>
              <input
                type="date"
                name="ngayThanhLy"
                value={formData.ngayThanhLy}
                onChange={handleChange}
                required
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Giá thanh lý thu về (VNĐ) <span className="text-red-500">*</span>
              </label>
              <input
                type="number"
                name="giaTriThanhLy"
                value={formData.giaTriThanhLy === 0 ? '' : formData.giaTriThanhLy}
                onChange={handleChange}
                required
                min="0"
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                placeholder="Nhập số tiền thu được..."
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Lý do thanh lý <span className="text-red-500">*</span>
              </label>
              <select
                name="lyDo"
                value={formData.lyDo}
                onChange={handleChange}
                required
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              >
                <option value="">Chọn lý do</option>
                <option value="Hết thời gian sử dụng">Hết thời gian sử dụng</option>
                <option value="Hỏng không sửa được">Hỏng không sửa được</option>
                <option value="Nâng cấp thiết bị mới">Nâng cấp thiết bị mới</option>
                <option value="Lạc hậu công nghệ">Lạc hậu công nghệ</option>
                <option value="Lý do khác">Lý do khác</option>
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Trạng thái <span className="text-red-500">*</span>
              </label>
              <select
                name="trangThai"
                value={formData.trangThai ?? 0}
                onChange={handleChange}
                required
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              >
                <option value={0}>Chờ duyệt</option>
                <option value={1}>Đã duyệt</option> 
                <option value={2}>Hoàn thành (Ghi sổ)</option> {/* <--- THÊM/SỬA THÀNH SỐ 2 */}
              </select>
            </div>
            <div className="md:col-span-2">
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Ghi chú thêm
              </label>
              <textarea
                name="ghiChu"
                value={formData.ghiChu}
                onChange={handleChange}
                rows={2}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                placeholder="Các ghi chú khác về việc thanh lý..."
              />
            </div>
          </div>
        </div>

        {/* Calculation Result Preview */}
        {selectedAsset && (
          <div className="bg-gradient-to-r from-blue-50 to-purple-50 rounded-lg border border-blue-200 p-6">
            <div className="flex items-center gap-3 mb-4">
              <Calculator className="w-6 h-6 text-blue-600" />
              <h3 className="font-semibold text-gray-900">Tính toán Lãi/Lỗ thanh lý (Dự kiến)</h3>
            </div>
            
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
              <div className="bg-white rounded-lg p-4">
                <p className="text-sm text-gray-600 mb-1">Giá trị còn lại</p>
                <p className="font-bold text-gray-900">{formatCurrency(remainingValue)}</p>
              </div>
              <div className="bg-white rounded-lg p-4">
                <p className="text-sm text-gray-600 mb-1">Giá thanh lý</p>
                <p className="font-bold text-gray-900">{formatCurrency(liquidationValue)}</p>
              </div>
              <div className={`bg-white rounded-lg p-4 ${profitLoss >= 0 ? 'border-2 border-green-500' : 'border-2 border-red-500'}`}>
                <p className="text-sm text-gray-600 mb-1">
                  {profitLoss >= 0 ? 'Lãi thanh lý' : 'Lỗ thanh lý'}
                </p>
                <p className={`font-bold text-lg ${profitLoss >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                  {formatCurrency(Math.abs(profitLoss))}
                </p>
              </div>
            </div>

            <div className="mt-4 p-4 bg-white rounded-lg">
              <p className="text-sm text-gray-700 font-medium mb-2">Bút toán sẽ được ghi nhận:</p>
              <div className="space-y-1 text-sm text-gray-600">
                <p>• Nợ TK 214: {formatCurrency(selectedAsset.khauHaoLuyKe)} (Khấu hao lũy kế)</p>
                {profitLoss >= 0 ? (
                  <>
                    <p>• Nợ TK 111/112: {formatCurrency(liquidationValue)} (Thu tiền thanh lý)</p>
                    <p>• Có TK 711: {formatCurrency(profitLoss)} (Thu nhập khác)</p>
                  </>
                ) : (
                  <>
                    <p>• Nợ TK 111/112: {formatCurrency(liquidationValue)} (Thu tiền thanh lý)</p>
                    <p>• Nợ TK 811: {formatCurrency(Math.abs(profitLoss))} (Chi phí khác)</p>
                  </>
                )}
                <p>• Có TK 211: {formatCurrency(selectedAsset.nguyenGia)} (Nguyên giá TSCĐ)</p>
              </div>
            </div>
          </div>
        )}

        {/* Action Buttons */}
        <div className="flex justify-end gap-4">
          <button
            type="button"
            onClick={() => navigate('/liquidation')}
            className="px-6 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors"
          >
            Hủy
          </button>
          <button
            type="submit"
            disabled={isSubmitting}
            className="flex items-center gap-2 px-6 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors disabled:opacity-50"
          >
            <Save className="w-5 h-5" />
            {isSubmitting ? 'Đang lưu...' : 'Lưu & Sinh chứng từ'}
          </button>
        </div>
      </form>
    </div>
  );
}