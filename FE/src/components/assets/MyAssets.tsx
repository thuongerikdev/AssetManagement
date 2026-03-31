import { useState, useEffect } from 'react';
import { Monitor, CheckCircle, Package, AlertTriangle, Clock } from 'lucide-react';
import { toast } from 'sonner';
import { assetApi, TaiSan } from '../../api/assetApi';

export function MyAssets() {
  const [assets, setAssets] = useState<TaiSan[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  // GIẢ LẬP: Đang đăng nhập bằng User có ID = 15 (Bạn có thể đổi số này để test)
  // Sau này có Auth, bạn sẽ lấy ID này từ Redux/Context (ví dụ: currentUser.id)
  const currentUserId = 15; 

  const fetchMyAssets = async () => {
    setIsLoading(true);
    try {
      const response = await assetApi.getMyAssets(currentUserId);
      if (response.errorCode === 200) {
        setAssets(response.data);
      }
    } catch (error) {
      toast.error('Không thể tải danh sách tài sản của bạn.');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchMyAssets();
  }, []);

  const handleConfirm = async (id: number) => {
    if (window.confirm('Xác nhận bạn đã nhận tài sản này với tình trạng hoạt động tốt?')) {
      try {
        const response = await assetApi.confirm(id);
        if (response.errorCode === 200) {
          toast.success('Ký nhận tài sản thành công!');
          fetchMyAssets(); // Load lại để thẻ chuyển sang màu xanh
        } else {
          toast.error(response.message || 'Có lỗi xảy ra');
        }
      } catch (error) {
        toast.error('Lỗi kết nối máy chủ');
      }
    }
  };

  const formatCurrency = (val?: number) => 
    new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(val || 0);

  // Chia làm 2 nhóm để hiển thị
  const pendingAssets = assets.filter(a => a.trangThai?.toString() === 'ChoXacNhan');
  const activeAssets = assets.filter(a => a.trangThai?.toString() === 'DangSuDung');

  return (
    <div className="space-y-8 max-w-5xl mx-auto py-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900">Tài sản của tôi</h1>
        <p className="text-gray-500 mt-1">Quản lý và xác nhận các tài sản được công ty cấp phát cho bạn.</p>
      </div>

      {isLoading ? (
        <div className="text-center py-12 text-gray-500 bg-white rounded-xl border border-gray-200">
          Đang tải dữ liệu...
        </div>
      ) : assets.length === 0 ? (
        <div className="text-center py-16 bg-white rounded-xl border border-gray-200 shadow-sm flex flex-col items-center">
          <Package className="w-16 h-16 text-gray-300 mb-4" />
          <h3 className="text-lg font-semibold text-gray-900">Chưa có tài sản nào</h3>
          <p className="text-gray-500 mt-1">Hiện tại bạn chưa được cấp phát tài sản nào từ công ty.</p>
        </div>
      ) : (
        <div className="space-y-8">
          
          {/* PHẦN 1: TÀI SẢN CẦN XÁC NHẬN BÀN GIAO */}
          {pendingAssets.length > 0 && (
            <section>
              <h2 className="text-lg font-bold text-yellow-800 flex items-center gap-2 mb-4">
                <AlertTriangle className="w-5 h-5" />
                Cần xác nhận bàn giao ({pendingAssets.length})
              </h2>
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                {pendingAssets.map(asset => (
                  <div key={asset.id} className="bg-yellow-50 rounded-xl border border-yellow-200 p-5 shadow-sm hover:shadow-md transition-shadow relative overflow-hidden">
                    <div className="absolute top-0 left-0 w-1 h-full bg-yellow-400"></div>
                    <div className="flex justify-between items-start mb-4">
                      <div className="flex items-center gap-3">
                        <div className="p-3 bg-white rounded-lg shadow-sm text-yellow-600">
                          <Monitor className="w-6 h-6" />
                        </div>
                        <div>
                          <h3 className="font-bold text-gray-900 text-lg leading-tight">{asset.tenTaiSan}</h3>
                          <p className="text-sm font-medium text-blue-600">{asset.maTaiSan}</p>
                        </div>
                      </div>
                    </div>
                    <div className="space-y-2 mb-6">
                      <div className="flex justify-between text-sm">
                        <span className="text-gray-600">Ngày cấp phát:</span>
                        <span className="font-medium text-gray-900">{asset.ngayCapPhat ? new Date(asset.ngayCapPhat).toLocaleDateString('vi-VN') : 'N/A'}</span>
                      </div>
                      <div className="flex justify-between text-sm">
                        <span className="text-gray-600">Số Serial:</span>
                        <span className="font-medium text-gray-900">{asset.soSeri || 'Không có'}</span>
                      </div>
                    </div>
                    <button 
                      onClick={() => asset.id && handleConfirm(asset.id)}
                      className="w-full flex items-center justify-center gap-2 py-2.5 bg-yellow-500 hover:bg-yellow-600 text-white font-semibold rounded-lg transition-colors shadow-sm"
                    >
                      <CheckCircle className="w-5 h-5" />
                      Ký nhận Tài sản này
                    </button>
                  </div>
                ))}
              </div>
            </section>
          )}

          {/* PHẦN 2: TÀI SẢN ĐANG SỬ DỤNG */}
          {activeAssets.length > 0 && (
            <section>
              <h2 className="text-lg font-bold text-gray-800 flex items-center gap-2 mb-4 border-b pb-2">
                <Package className="w-5 h-5 text-green-600" />
                Tài sản đang sử dụng ({activeAssets.length})
              </h2>
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {activeAssets.map(asset => (
                  <div key={asset.id} className="bg-white rounded-xl border border-gray-200 p-5 shadow-sm hover:shadow-md transition-shadow">
                    <div className="flex items-center gap-3 mb-4">
                      <div className="p-2.5 bg-green-50 rounded-lg text-green-600">
                        <Monitor className="w-5 h-5" />
                      </div>
                      <div>
                        <h3 className="font-bold text-gray-900 leading-tight line-clamp-1" title={asset.tenTaiSan}>{asset.tenTaiSan}</h3>
                        <p className="text-xs font-medium text-blue-600">{asset.maTaiSan}</p>
                      </div>
                    </div>
                    <div className="space-y-1.5 pt-3 border-t border-gray-100">
                      <div className="flex items-center gap-2 text-sm text-gray-600">
                        <Clock className="w-4 h-4 text-gray-400" />
                        Nhận ngày: {asset.ngayCapPhat ? new Date(asset.ngayCapPhat).toLocaleDateString('vi-VN') : 'N/A'}
                      </div>
                      <div className="flex justify-between text-sm">
                        <span className="text-gray-500">Trạng thái:</span>
                        <span className="text-green-600 font-medium bg-green-50 px-2 py-0.5 rounded-full text-xs">Đang sử dụng</span>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            </section>
          )}
        </div>
      )}
    </div>
  );
}