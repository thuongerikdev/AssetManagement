import { useState, useEffect } from 'react';
import { Link } from 'react-router';
import { Plus, Search, Eye, Trash2, Download, Package, ArrowLeftRight, UserPlus } from 'lucide-react';
import { toast } from "sonner";
import { assetApi, TaiSan } from '../../api/assetApi';
import { departmentApi, Department } from '../../api/departmentApi';
import { assetCategoryApi, AssetCategory } from '../../api/assetCategoryApi';
import * as XLSX from 'xlsx';

// 1. ĐỒNG BỘ CONFIG TRẠNG THÁI (Khớp với Enum C# là 0, 1, 2, 3)
const statusConfig: Record<string, { label: string; color: string; value: number }> = {
  '0': { label: 'Chưa cấp phát', color: 'bg-gray-100 text-gray-700', value: 0 },
  '1': { label: 'Chờ xác nhận', color: 'bg-yellow-100 text-yellow-700', value: 1 },
  '2': { label: 'Đang sử dụng', color: 'bg-green-100 text-green-700', value: 2 },
  '3': { label: 'Đã thanh lý', color: 'bg-red-100 text-red-700', value: 3 },
  // Dự phòng trường hợp API trả về chuỗi text
  'ChuaCapPhat': { label: 'Chưa cấp phát', color: 'bg-gray-100 text-gray-700', value: 0 },
  'ChoXacNhan': { label: 'Chờ xác nhận', color: 'bg-yellow-100 text-yellow-700', value: 1 },
  'DangSuDung': { label: 'Đang sử dụng', color: 'bg-green-100 text-green-700', value: 2 },
  'DaThanhLy': { label: 'Đã thanh lý', color: 'bg-red-100 text-red-700', value: 3 },
};

export function AssetList() {
  const [searchTerm, setSearchTerm] = useState('');
  const [filterStatus, setFilterStatus] = useState<string>('all');
  const [assets, setAssets] = useState<TaiSan[]>([]);
  const [departments, setDepartments] = useState<Department[]>([]);
  const [categories, setCategories] = useState<AssetCategory[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  const getDeptName = (id?: number) => departments.find(d => d.id === id)?.tenPhongBan || 'Chưa phân bổ';
  const getCatName = (id?: number) => categories.find(c => c.id === id)?.tenDanhMuc || 'N/A';

  // 2. LỌC VÀ SẮP XẾP THEO TRẠNG THÁI
  const filteredAssets = assets
    .filter(asset => {
      const searchStr = `${asset.maTaiSan} ${asset.tenTaiSan}`.toLowerCase();
      const matchesSearch = searchStr.includes(searchTerm.toLowerCase());
      
      const currentStatusStr = asset.trangThai?.toString() || '0';
      const matchesStatus = filterStatus === 'all' || 
                            currentStatusStr === filterStatus || 
                            statusConfig[currentStatusStr]?.value.toString() === filterStatus;
      
      return matchesSearch && matchesStatus;
    })
    .sort((a, b) => {
      // Sắp xếp tự động: Chưa cấp phát (0) -> Chờ (1) -> Đang dùng (2) -> Thanh lý (3)
      const valA = statusConfig[a.trangThai?.toString() || '0']?.value ?? 0;
      const valB = statusConfig[b.trangThai?.toString() || '0']?.value ?? 0;
      return valA - valB;
    });

  const handleExportExcel = () => {
    if (filteredAssets.length === 0) {
      toast.error("Không có dữ liệu để xuất!");
      return;
    }

    try {
      const dataToExport = filteredAssets.map((asset) => ({
        "Mã Tài Sản": asset.maTaiSan,
        "Tên Tài Sản": asset.tenTaiSan,
        "Danh Mục": getCatName(asset.danhMucId),
        "Phòng Ban": getDeptName(asset.phongBanId),
        "Nguyên Giá (VNĐ)": asset.nguyenGia || 0,
        "Giá Trị Còn Lại (VNĐ)": asset.giaTriConLai || 0,
        "Trạng Thái": statusConfig[asset.trangThai?.toString() || '0']?.label || 'Không xác định',
        "Ngày Mua": asset.ngayMua ? new Date(asset.ngayMua).toLocaleDateString('vi-VN') : '',
        "Số Serial": asset.soSeri || '',
        "Nhà Sản Xuất": asset.nhaSanXuat || '',
        "Mô Tả": asset.moTa || ''
      }));

      const worksheet = XLSX.utils.json_to_sheet(dataToExport);
      const workbook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(workbook, worksheet, "Danh sách tài sản");

      const wscols = [
        { wch: 15 }, { wch: 30 }, { wch: 20 }, { wch: 20 },
        { wch: 15 }, { wch: 15 }, { wch: 15 }, { wch: 12 },
      ];
      worksheet['!cols'] = wscols;

      const fileName = `Danh_Sach_Tai_San_${new Date().getTime()}.xlsx`;
      XLSX.writeFile(workbook, fileName);
      toast.success("Xuất file Excel thành công!");
    } catch (error) {
      console.error("Export error:", error);
      toast.error("Có lỗi xảy ra khi xuất file Excel.");
    }
  };

  const fetchData = async () => {
    setIsLoading(true);
    try {
      const [assetRes, deptRes, catRes] = await Promise.all([
        assetApi.getAll(),
        departmentApi.getAll(),
        assetCategoryApi.getAll()
      ]);

      if (assetRes.errorCode === 200) setAssets(assetRes.data);
      if (deptRes.errorCode === 200) setDepartments(deptRes.data);
      if (catRes.errorCode === 200) setCategories(catRes.data);
    } catch (error) {
      toast.error('Lỗi kết nối máy chủ khi lấy dữ liệu tài sản.');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleDelete = async (id: number) => {
    // 1. CHẶN LOGIC: Tìm tài sản đang muốn xóa để check trạng thái
    const assetToDelete = assets.find(a => a.id === id);
    const statusVal = statusConfig[assetToDelete?.trangThai?.toString() || '0']?.value;

    if (statusVal !== 3) {
      toast.error('Chỉ được phép xóa vĩnh viễn các tài sản "Đã thanh lý"!');
      return;
    }

    // 2. Nếu đã thanh lý (value === 3) thì mới cho đi tiếp
    if (window.confirm('Bạn có chắc chắn muốn xóa vĩnh viễn tài sản này khỏi hệ thống?')) {
      try {
        const response = await assetApi.delete(id);
        if (response.errorCode === 200) {
          toast.success('Xóa tài sản thành công');
          setAssets(assets.filter(a => a.id !== id));
        } else {
          toast.error(response.message || 'Lỗi khi xóa tài sản');
        }
      } catch (error) {
        toast.error('Không thể kết nối đến máy chủ');
      }
    }
  };

  const formatCurrency = (value?: number) => {
    if (!value) return '0 ₫';
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Quản lý Tài sản</h1>
          <p className="text-sm text-gray-500 mt-1">Danh sách tài sản cố định</p>
        </div>
        <div className="flex gap-3">
          <Link
            to="/assets/groups"
            className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors"
          >
            <Package className="w-5 h-5" />
            Xem theo Nhóm
          </Link>
          <Link
            to="/assets/new"
            className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
          >
            <Plus className="w-5 h-5" />
            Thêm Tài sản
          </Link>
        </div>
      </div>

      {/* Filters */}
      <div className="bg-white rounded-lg border border-gray-200 p-4">
        <div className="flex flex-col lg:flex-row gap-4">
          <div className="flex-1 relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" />
            <input
              type="text"
              placeholder="Tìm kiếm theo tên hoặc mã tài sản..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
          </div>
          <div className="flex gap-2">
            <select
              value={filterStatus}
              onChange={(e) => setFilterStatus(e.target.value)}
              className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            >
              <option value="all">Tất cả trạng thái</option>
              <option value="0">Chưa cấp phát</option>
              <option value="1">Chờ xác nhận</option>
              <option value="2">Đang sử dụng</option>
              <option value="3">Đã thanh lý</option>
            </select>
            <button 
              onClick={handleExportExcel}
              className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors"
            >
              <Download className="w-5 h-5" />
              Xuất Excel
            </button>
          </div>
        </div>
      </div>

      {/* Asset Table */}
      <div className="bg-white rounded-lg border border-gray-200 overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50 border-b border-gray-200">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase tracking-wider">Mã TS</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase tracking-wider">Tên tài sản</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase tracking-wider">Danh mục</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase tracking-wider">Phòng ban</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase tracking-wider">Nguyên giá</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase tracking-wider">Giá trị còn lại</th>
                <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase tracking-wider">Trạng thái</th>
                <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase tracking-wider">Thao tác</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {isLoading ? (
                <tr><td colSpan={8} className="text-center py-6 text-gray-500">Đang tải dữ liệu...</td></tr>
              ) : filteredAssets.length === 0 ? (
                <tr><td colSpan={8} className="text-center py-6 text-gray-500">Không tìm thấy tài sản nào</td></tr>
              ) : (
                filteredAssets.map((asset) => {
                  const assetStatusStr = asset.trangThai?.toString() || '0';
                  const currentStatus = statusConfig[assetStatusStr] || { label: 'Không xác định', color: 'bg-gray-100 text-gray-500', value: -1 };

                  return (
                    <tr key={asset.id} className="hover:bg-gray-50 transition-colors">
                      <td className="px-6 py-4 whitespace-nowrap">
                        <span className="text-sm font-medium text-blue-600">{asset.maTaiSan}</span>
                      </td>
                      <td className="px-6 py-4">
                        <span className="text-sm text-gray-900">{asset.tenTaiSan}</span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap">
                        <span className="text-sm text-gray-600">{getCatName(asset.danhMucId)}</span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap">
                        <span className="text-sm text-gray-600">{getDeptName(asset.phongBanId)}</span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-right">
                        <span className="text-sm text-gray-900">{formatCurrency(asset.nguyenGia)}</span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-right">
                        <span className="text-sm font-medium text-gray-900">{formatCurrency(asset.giaTriConLai)}</span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-center">
                        <span className={`inline-flex px-2 py-1 text-xs font-medium rounded-full ${currentStatus.color}`}>
                          {currentStatus.label}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-center">
                        <div className="flex items-center justify-center gap-2">
                          
                          {/* Nút Cấp phát (Chỉ hiện khi chưa cấp phát - Trạng thái 0) */}
                          {currentStatus.value === 0 && (
                            <Link to={`/assets/${asset.id}/edit`} className="p-1 text-green-600 hover:bg-green-100 rounded" title="Cấp phát tài sản">
                              <UserPlus className="w-4 h-4" />
                            </Link>
                          )}

                          {/* Nút Điều chuyển (Chỉ hiện khi Đang sử dụng - Trạng thái 2) */}
                          {currentStatus.value === 2 && (
                            <Link to={`/allocation?assetId=${asset.id}`} className="p-1 text-orange-500 hover:bg-orange-100 rounded" title="Điều chuyển tài sản">
                              <ArrowLeftRight className="w-4 h-4" />
                            </Link>
                          )}

                          {/* Nút Xem chi tiết */}
                          <Link to={`/assets/${asset.id}`} className="p-1 text-blue-600 hover:bg-blue-100 rounded" title="Xem chi tiết">
                            <Eye className="w-4 h-4" />
                          </Link>

                          {/* Nút Xóa/Thanh lý */}
                          {currentStatus.value === 3 ? (
                            // Nếu đã thanh lý (3) => Hiện nút đỏ cho phép Xóa
                            <button onClick={() => asset.id && handleDelete(asset.id)} className="p-1 text-red-600 hover:bg-red-100 rounded transition-colors" title="Xóa tài sản vĩnh viễn">
                              <Trash2 className="w-4 h-4" />
                            </button>
                          ) : (
                            // Nếu chưa thanh lý => Hiện nút xám và khóa click (disabled)
                            <button disabled className="p-1 text-gray-300 cursor-not-allowed rounded" title="Chỉ được phép xóa tài sản Đã thanh lý">
                              <Trash2 className="w-4 h-4" />
                            </button>
                          )}
                          
                        </div>
                      </td>
                    </tr>
                  );
                })
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}