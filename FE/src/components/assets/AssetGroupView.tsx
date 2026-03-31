import { useState, useEffect } from 'react';
import { Link } from 'react-router';
import { Package, ChevronDown, ChevronRight, Plus, Eye, ArrowLeft, List } from 'lucide-react';
import { toast } from "sonner";
import { assetApi, TaiSan } from '../../api/assetApi';
import { departmentApi, Department } from '../../api/departmentApi';
import { assetCategoryApi, AssetCategory } from '../../api/assetCategoryApi';

// 1. CẬP NHẬT LẠI CONFIG TRẠNG THÁI THEO CHUỖI ENUM C#
const statusConfig: Record<string, { label: string; color: string }> = {
  'ChuaCapPhat': { label: 'Chưa cấp phát', color: 'bg-gray-100 text-gray-700' },
  'ChoXacNhan': { label: 'Chờ xác nhận', color: 'bg-yellow-100 text-yellow-700' },
  'DangSuDung': { label: 'Đang sử dụng', color: 'bg-green-100 text-green-700' },
  'DaThanhLy': { label: 'Đã thanh lý', color: 'bg-red-100 text-red-700' },
};

export function AssetGroupView() {
  const [categories, setCategories] = useState<AssetCategory[]>([]);
  const [assets, setAssets] = useState<TaiSan[]>([]);
  const [departments, setDepartments] = useState<Department[]>([]);
  const [expandedGroups, setExpandedGroups] = useState<Set<number>>(new Set());
  const [isLoading, setIsLoading] = useState(true);

  const fetchData = async () => {
    setIsLoading(true);
    try {
      const [catRes, assetRes, deptRes] = await Promise.all([
        assetCategoryApi.getAll(),
        assetApi.getAll(),
        departmentApi.getAll()
      ]);

      if (catRes.errorCode === 200) setCategories(catRes.data);
      if (assetRes.errorCode === 200) setAssets(assetRes.data);
      if (deptRes.errorCode === 200) setDepartments(deptRes.data);
    } catch (error) {
      toast.error('Lỗi tải dữ liệu nhóm tài sản.');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const toggleGroup = (groupId: number) => {
    const newExpanded = new Set(expandedGroups);
    if (newExpanded.has(groupId)) {
      newExpanded.delete(groupId);
    } else {
      newExpanded.add(groupId);
    }
    setExpandedGroups(newExpanded);
  };

  const formatCurrency = (value?: number) => {
    if (!value) return '0 ₫';
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
  };

  const getDeptName = (id?: number) => departments.find(d => d.id === id)?.tenPhongBan || 'Chưa cấp phát';

  return (
    <div className="space-y-6">
      {/* Breadcrumb */}
      <div className="flex items-center gap-2 text-sm">
        <Link to="/assets" className="text-blue-600 hover:text-blue-700 flex items-center gap-1">
          <ArrowLeft className="w-4 h-4" />
          Danh sách
        </Link>
        <span className="text-gray-400">/</span>
        <span className="text-gray-900 font-medium">Xem theo Nhóm</span>
      </div>

      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Quản lý Tài sản theo Nhóm</h1>
          <p className="text-sm text-gray-500 mt-1">
            Tổng số: {assets.length} tài sản được phân loại trong {categories.length} nhóm
          </p>
        </div>
        <div className="flex gap-3">
          <Link to="/assets" className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors">
            <List className="w-5 h-5" />
            Xem Danh sách
          </Link>
          <Link to="/assets/new" className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors">
            <Plus className="w-5 h-5" />
            Thêm Tài sản
          </Link>
        </div>
      </div>

      {/* Grouped Assets */}
      {isLoading ? (
        <div className="text-center py-12 bg-white rounded-lg border border-gray-200 text-gray-500">
          Đang tải dữ liệu từ máy chủ...
        </div>
      ) : categories.length === 0 ? (
        <div className="text-center py-12 bg-white rounded-lg border border-gray-200 text-gray-500">
          Chưa có danh mục tài sản nào được thiết lập.
        </div>
      ) : (
        <div className="space-y-4">
          {categories.map((category) => {
            // Lọc tài sản thuộc danh mục này
            const groupAssets = assets.filter(a => a.danhMucId === category.id);
            const totalValue = groupAssets.reduce((sum, a) => sum + (a.nguyenGia || 0), 0);
            const isExpanded = category.id != null && expandedGroups.has(category.id);

            if (groupAssets.length === 0) return null; // Không hiện danh mục rỗng

            return (
              <div key={category.id} className="bg-white rounded-lg border border-gray-200 overflow-hidden">
                {/* Group Header */}
                <button
                  onClick={() => category.id && toggleGroup(category.id)}
                  className="w-full px-6 py-4 flex items-center justify-between hover:bg-gray-50 transition-colors"
                >
                  <div className="flex items-center gap-3">
                    {isExpanded ? (
                      <ChevronDown className="w-5 h-5 text-gray-500" />
                    ) : (
                      <ChevronRight className="w-5 h-5 text-gray-500" />
                    )}
                    <div className="p-2 rounded-lg bg-blue-100 text-blue-700">
                      <Package className="w-5 h-5" />
                    </div>
                    <div className="text-left">
                      <h3 className="font-semibold text-gray-900">{category.tenDanhMuc}</h3>
                      <p className="text-sm text-gray-500">
                        {groupAssets.length} tài sản • Tổng giá trị: {formatCurrency(totalValue)}
                      </p>
                    </div>
                  </div>
                  <span className="px-3 py-1 rounded-full text-sm font-medium bg-blue-100 text-blue-700">
                    Mã DM: {category.maDanhMuc}
                  </span>
                </button>

                {/* Group Content */}
                {isExpanded && (
                  <div className="border-t border-gray-200">
                    <div className="overflow-x-auto">
                      <table className="min-w-full divide-y divide-gray-200">
                        <thead className="bg-gray-50">
                          <tr>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Mã TS</th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Tên tài sản</th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Phòng ban</th>
                            <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase">Nguyên giá</th>
                            <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase">Giá trị còn lại</th>
                            <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase">Trạng thái</th>
                            <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase">Chi tiết</th>
                          </tr>
                        </thead>
                        <tbody className="bg-white divide-y divide-gray-200">
                          {groupAssets.map((asset) => {
                            // 2. LẤY NHÃN VÀ MÀU TRẠNG THÁI CHUẨN XÁC
                            const assetStatusStr = asset.trangThai?.toString() || 'ChuaCapPhat';
                            const currentStatus = statusConfig[assetStatusStr] || { label: 'Không xác định', color: 'bg-gray-100 text-gray-500' };

                            return (
                              <tr key={asset.id} className="hover:bg-gray-50 transition-colors">
                                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-blue-600">{asset.maTaiSan}</td>
                                <td className="px-6 py-4 text-sm text-gray-900">{asset.tenTaiSan}</td>
                                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">{getDeptName(asset.phongBanId)}</td>
                                <td className="px-6 py-4 whitespace-nowrap text-right text-sm text-gray-900">{formatCurrency(asset.nguyenGia)}</td>
                                <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium text-gray-900">{formatCurrency(asset.giaTriConLai)}</td>
                                <td className="px-6 py-4 whitespace-nowrap text-center">
                                  <span className={`inline-flex px-2 py-1 text-xs font-medium rounded-full ${currentStatus.color}`}>
                                    {currentStatus.label}
                                  </span>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap text-center">
                                  <Link to={`/assets/${asset.id}`} className="p-1 text-blue-600 hover:bg-blue-50 rounded inline-block" title="Chi tiết">
                                    <Eye className="w-4 h-4" />
                                  </Link>
                                </td>
                              </tr>
                            );
                          })}
                        </tbody>
                      </table>
                    </div>
                  </div>
                )}
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
}