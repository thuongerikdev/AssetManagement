import { useState, useEffect } from 'react';
import { Link, useParams, useNavigate } from 'react-router';
import { ArrowLeft, Edit, Send, ArrowLeftRight, Trash2, FileText, Clock, Package } from 'lucide-react';
import { toast } from "sonner";
import { assetApi, TaiSan } from '../../api/assetApi';
import { departmentApi, Department } from '../../api/departmentApi';
import { assetCategoryApi, AssetCategory } from '../../api/assetCategoryApi';

// Cập nhật cấu hình trạng thái theo chuỗi (Khớp Backend)
const statusConfig: Record<string, { label: string; color: string }> = {
  'ChuaCapPhat': { label: 'Chưa cấp phát', color: 'bg-gray-100 text-gray-700' },
  'ChoXacNhan': { label: 'Chờ xác nhận', color: 'bg-yellow-100 text-yellow-700' },
  'DangSuDung': { label: 'Đang sử dụng', color: 'bg-green-100 text-green-700' },
  'DaThanhLy': { label: 'Đã thanh lý', color: 'bg-red-100 text-red-700' },
};

export function AssetDetail() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [asset, setAsset] = useState<TaiSan | null>(null);
  const [departments, setDepartments] = useState<Department[]>([]);
  const [categories, setCategories] = useState<AssetCategory[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      setIsLoading(true);
      try {
        const [assetRes, deptRes, catRes] = await Promise.all([
          assetApi.getById(Number(id)),
          departmentApi.getAll(),
          assetCategoryApi.getAll()
        ]);

        if (assetRes.errorCode === 200) setAsset(assetRes.data);
        else toast.error('Không tìm thấy thông tin tài sản!');

        if (deptRes.errorCode === 200) setDepartments(deptRes.data);
        if (catRes.errorCode === 200) setCategories(catRes.data);
      } catch (error) {
        toast.error('Lỗi khi tải chi tiết tài sản.');
      } finally {
        setIsLoading(false);
      }
    };

    if (id) fetchData();
  }, [id]);

  const formatCurrency = (value?: number) => {
    if (!value) return '0 ₫';
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
  };

  const getDeptName = (deptId?: number) => departments.find(d => d.id === deptId)?.tenPhongBan || 'Chưa cấp phát';
  const getCatName = (catId?: number) => categories.find(c => c.id === catId)?.tenDanhMuc || 'Chưa phân loại';

  if (isLoading) {
    return <div className="text-center py-12 text-gray-500">Đang tải chi tiết tài sản...</div>;
  }

  if (!asset) {
    return (
      <div className="text-center py-12">
        <h2 className="text-xl font-bold text-gray-700">Tài sản không tồn tại</h2>
        <button onClick={() => navigate('/assets')} className="mt-4 text-blue-600 hover:underline">Quay lại danh sách</button>
      </div>
    );
  }

  // Tính toán phần trăm khấu hao
  const originalValue = asset.nguyenGia || 0;
  const accumulated = asset.khauHaoLuyKe || 0;
  const depreciationPercent = originalValue > 0 ? Math.min(100, Math.round((accumulated / originalValue) * 100)) : 0;
  const currentStatusStr = asset.trangThai?.toString() || 'ChuaCapPhat';
  const statusInfo = statusConfig[currentStatusStr] || { label: 'Không xác định', color: 'bg-gray-100 text-gray-500' };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <button
            onClick={() => navigate('/assets')}
            className="p-2 hover:bg-gray-100 rounded-lg transition-colors"
          >
            <ArrowLeft className="w-5 h-5" />
          </button>
          <div>
            <h1 className="font-bold text-gray-900">Chi tiết Tài sản</h1>
            <p className="text-sm text-gray-500 mt-1">Mã: <span className="font-medium text-blue-600">{asset.maTaiSan}</span></p>
          </div>
        </div>
        <div className="flex gap-2">
          <Link
            to={`/assets/${id}/edit`}
            className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors"
          >
            <Edit className="w-4 h-4" />
            Chỉnh sửa
          </Link>
          
          {/* Tùy chỉnh màu nút theo trạng thái thực tế */}
          {currentStatusStr === 'ChuaCapPhat' && (
             <Link to="/allocation" className="flex items-center gap-2 px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors">
               <Send className="w-4 h-4" /> Cấp phát
             </Link>
          )}
          {currentStatusStr === 'DangSuDung' && (
             <Link to="/allocation" className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors">
               <ArrowLeftRight className="w-4 h-4" /> Điều chuyển
             </Link>
          )}
          {currentStatusStr !== 'DaThanhLy' && (
            <button className="flex items-center gap-2 px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors">
              <Trash2 className="w-4 h-4" /> Thanh lý
            </button>
          )}
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Main Info */}
        <div className="lg:col-span-2 space-y-6">
          {/* Basic Info */}
          <div className="bg-white rounded-lg border border-gray-200 p-6 shadow-sm">
            <div className="flex items-center justify-between mb-6">
              <h3 className="font-semibold text-gray-900 text-lg">Thông tin cơ bản</h3>
              <span className={`inline-flex px-3 py-1 text-xs font-medium rounded-full ${statusInfo.color}`}>
                {statusInfo.label}
              </span>
            </div>
            <div className="grid grid-cols-2 gap-6">
              <div>
                <p className="text-sm text-gray-500 mb-1">Tên tài sản</p>
                <p className="text-base font-medium text-gray-900">{asset.tenTaiSan}</p>
              </div>
              <div>
                <p className="text-sm text-gray-500 mb-1">Danh mục</p>
                <p className="text-base font-medium text-gray-900">{getCatName(asset.danhMucId)}</p>
              </div>
              <div>
                <p className="text-sm text-gray-500 mb-1">Serial Number</p>
                <p className="text-base font-medium text-gray-900">{asset.soSeri || 'N/A'}</p>
              </div>
              <div>
                <p className="text-sm text-gray-500 mb-1">Nhà sản xuất</p>
                <p className="text-base font-medium text-gray-900">{asset.nhaSanXuat || 'N/A'}</p>
              </div>
              <div>
                <p className="text-sm text-gray-500 mb-1">Ngày mua</p>
                <p className="text-base font-medium text-gray-900">{asset.ngayMua ? new Date(asset.ngayMua).toLocaleDateString('vi-VN') : 'N/A'}</p>
              </div>
              <div className="col-span-2">
                <p className="text-sm text-gray-500 mb-1">Mô tả</p>
                <p className="text-base text-gray-900 bg-gray-50 p-3 rounded-lg border border-gray-100">{asset.moTa || 'Không có mô tả'}</p>
              </div>
            </div>
          </div>

          {/* Accounting Info */}
          <div className="bg-white rounded-lg border border-gray-200 p-6 shadow-sm">
            <h3 className="font-semibold text-gray-900 mb-6 text-lg">Thông tin kế toán</h3>
            <div className="grid grid-cols-2 gap-6">
              <div className="bg-gray-50 p-4 rounded-lg">
                <p className="text-sm text-gray-600 mb-1">Nguyên giá</p>
                <p className="text-xl font-bold text-gray-900">{formatCurrency(asset.nguyenGia)}</p>
              </div>
              <div className="bg-green-50 p-4 rounded-lg border border-green-100">
                <p className="text-sm text-green-700 mb-1">Giá trị còn lại</p>
                <p className="text-xl font-bold text-green-700">{formatCurrency(asset.giaTriConLai)}</p>
              </div>
              <div>
                <p className="text-sm text-gray-500 mb-1">Khấu hao lũy kế</p>
                <p className="text-base font-medium text-gray-900">{formatCurrency(asset.khauHaoLuyKe)}</p>
              </div>
              <div>
                <p className="text-sm text-gray-500 mb-1">Khấu hao/tháng</p>
                <p className="text-base font-medium text-gray-900">{formatCurrency(asset.khauHaoHangThang)}</p>
              </div>
              <div>
                <p className="text-sm text-gray-500 mb-1">Thời gian KH</p>
                <p className="text-base font-medium text-gray-900">{asset.thoiGianKhauHao ? `${asset.thoiGianKhauHao} tháng` : 'N/A'}</p>
              </div>
              <div>
                <p className="text-sm text-gray-500 mb-1">Tài khoản</p>
                <p className="text-base font-medium text-blue-600">{asset.maTaiKhoan || 'N/A'}</p>
              </div>
            </div>
          </div>

          {/* Usage History (Nếu Backend trả về list dieuChuyenTaiSans) */}
          <div className="bg-white rounded-lg border border-gray-200 p-6 shadow-sm">
            <h3 className="font-semibold text-gray-900 mb-4 flex items-center gap-2">
              <Clock className="w-5 h-5 text-blue-600" />
              Lịch sử Điều chuyển / Sử dụng
            </h3>
            <div className="space-y-3">
              {/* Giả định: Nếu chưa có API fetch chi tiết lịch sử, tạm dùng mảng rỗng */}
              {(!asset.dieuChuyenTaiSans || asset.dieuChuyenTaiSans.length === 0) ? (
                <p className="text-gray-500 text-sm italic">Chưa có lịch sử giao dịch nào được ghi nhận.</p>
              ) : (
                 <p className="text-sm text-gray-600">Đang có {asset.dieuChuyenTaiSans.length} giao dịch.</p>
                 // Map thực tế ở đây nếu model TaiSan trả về mảng dieuChuyenTaiSans
              )}
            </div>
          </div>
          
        </div>

        {/* Sidebar */}
        <div className="space-y-6">
          {/* Current Assignment */}
          <div className="bg-white rounded-lg border border-gray-200 p-6 shadow-sm">
            <h3 className="font-semibold text-gray-900 mb-4 flex items-center gap-2">
              <Package className="w-5 h-5 text-blue-600" /> Thông tin quản lý
            </h3>
            <div className="space-y-4">
              <div>
                <p className="text-sm text-gray-500 mb-1">Phòng ban quản lý/sử dụng</p>
                <p className="text-base font-medium text-gray-900">{getDeptName(asset.phongBanId)}</p>
              </div>
              <div>
                <p className="text-sm text-gray-500 mb-1">Mã Nhân viên</p>
                <p className="text-base font-medium text-gray-900">{asset.nguoiDungId ? `User #${asset.nguoiDungId}` : 'Chưa gắn User'}</p>
              </div>
              <div>
                <p className="text-sm text-gray-500 mb-1">Ngày nhận (Cấp phát/Luân chuyển)</p>
                <p className="text-base font-medium text-gray-900">
                  {asset.ngayCapPhat ? new Date(asset.ngayCapPhat).toLocaleDateString('vi-VN') : 'N/A'}
                </p>
              </div>
            </div>
          </div>

          {/* Quick Stats - Dynamic Progress Bar */}
          <div className="bg-gradient-to-br from-blue-500 to-blue-700 rounded-lg p-6 text-white shadow-md">
            <h3 className="font-semibold mb-4 text-blue-50">Tiến độ khấu hao</h3>
            <div className="space-y-3">
              <div>
                <div className="flex justify-between text-sm mb-2 font-medium">
                  <span>Đã khấu hao</span>
                  <span>{depreciationPercent}%</span>
                </div>
                <div className="w-full bg-blue-800 rounded-full h-2.5 overflow-hidden">
                  <div 
                    className="bg-white rounded-full h-2.5 transition-all duration-1000 ease-out" 
                    style={{ width: `${depreciationPercent}%` }}
                  ></div>
                </div>
              </div>
              <p className="text-sm text-blue-100">
                Lũy kế: {formatCurrency(accumulated)}
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}