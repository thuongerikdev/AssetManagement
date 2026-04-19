import { useState, useEffect } from 'react';
import { Link } from 'react-router';
import { Plus, Search, Eye, Trash2, Download, Package, ArrowLeftRight, UserPlus, X, Save, Send } from 'lucide-react';
import { toast } from "sonner";
import { apiClient } from '../../api/client'; // <-- ĐÃ THÊM IMPORT apiClient NÀY
import { assetApi, TaiSan } from '../../api/assetApi';
import { assetAllocationApi, DieuChuyenTaiSan } from '../../api/assetAllocationApi';
import * as XLSX from 'xlsx';
import { useGlobalData } from '../../context/GlobalContext';

const statusConfig: Record<string, { label: string; color: string; value: number }> = {
  '0': { label: 'Chưa cấp phát', color: 'bg-gray-100 text-gray-700', value: 0 },
  '1': { label: 'Chờ xác nhận', color: 'bg-yellow-100 text-yellow-700', value: 1 },
  '2': { label: 'Đang sử dụng', color: 'bg-green-100 text-green-700', value: 2 },
  '3': { label: 'Đã thanh lý', color: 'bg-red-100 text-red-700', value: 3 },
  'ChuaCapPhat': { label: 'Chưa cấp phát', color: 'bg-gray-100 text-gray-700', value: 0 },
  'ChoXacNhan': { label: 'Chờ xác nhận', color: 'bg-yellow-100 text-yellow-700', value: 1 },
  'DangSuDung': { label: 'Đang sử dụng', color: 'bg-green-100 text-green-700', value: 2 },
  'DaThanhLy': { label: 'Đã thanh lý', color: 'bg-red-100 text-red-700', value: 3 },
};

export function AssetList() {
  const { assets, departments, categories, isLoadingGlobal, refreshData } = useGlobalData();

  const [searchTerm, setSearchTerm] = useState('');
  const [filterStatus, setFilterStatus] = useState<string>('all');

  const [showAllocModal, setShowAllocModal] = useState(false);
  const [allocType, setAllocType] = useState<'CapPhat' | 'LuanChuyen'>('CapPhat');
  const [allocFormData, setAllocFormData] = useState<Partial<DieuChuyenTaiSan>>({});
  const [selectedAssetForModal, setSelectedAssetForModal] = useState<TaiSan | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  // === STATE & EFFECT CHO CHỨC NĂNG TÌM NHÂN VIÊN THEO PHÒNG BAN ===
  const [usersInDept, setUsersInDept] = useState<any[]>([]);
  const [isLoadingUsers, setIsLoadingUsers] = useState(false);

  useEffect(() => {
    const fetchUsersByDept = async () => {
      if (!allocFormData.denPhongBanId) {
        setUsersInDept([]);
        return;
      }
      setIsLoadingUsers(true);
      try {
        const data = await apiClient.get(`/user/GetByDepartmentId/${allocFormData.denPhongBanId}`);
        if (data.errorCode === 200) {
          setUsersInDept(data.data);
        } else {
          setUsersInDept([]);
        }
      } catch (error) {
        setUsersInDept([]);
      } finally {
        setIsLoadingUsers(false);
      }
    };
    fetchUsersByDept();
  }, [allocFormData.denPhongBanId]);
  // =================================================================

  const getDeptName = (id?: number) => departments.find((d: any) => d.id === id)?.tenPhongBan || 'Chưa phân bổ';
  const getCatName = (id?: number) => categories.find((c: any) => c.id === id)?.tenDanhMuc || 'N/A';

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
      const valA = statusConfig[a.trangThai?.toString() || '0']?.value ?? 0;
      const valB = statusConfig[b.trangThai?.toString() || '0']?.value ?? 0;
      return valA - valB;
    });

  const handleExportExcel = () => { /* Excel logic */ };

  const handleDelete = async (id: number) => {
    const assetToDelete = assets.find(a => a.id === id);
    const statusVal = statusConfig[assetToDelete?.trangThai?.toString() || '0']?.value;

    if (statusVal !== 0 && statusVal !== 3) {
      toast.error('Chỉ được phép xóa tài sản khi Chưa cấp phát hoặc Đã thanh lý!');
      return;
    }

    if (window.confirm('Bạn có chắc chắn muốn xóa vĩnh viễn tài sản này khỏi hệ thống?')) {
      try {
        const response = await assetApi.delete(id);
        if (response.errorCode === 200) {
          toast.success('Xóa tài sản thành công');
          refreshData();
        } else {
          toast.error(response.message || 'Lỗi khi xóa tài sản');
        }
      } catch (error) {
        toast.error('Không thể kết nối đến máy chủ');
      }
    }
  };

  const openAllocModal = (asset: TaiSan, type: 'CapPhat' | 'LuanChuyen') => {
    setSelectedAssetForModal(asset);
    setAllocType(type);
    setAllocFormData({
      taiSanId: asset.id,
      tuPhongBanId: type === 'LuanChuyen' ? asset.phongBanId : undefined,
      denPhongBanId: undefined,
      tuNguoiDungId: type === 'LuanChuyen' ? asset.nguoiDungId : undefined,
      denNguoiDungId: undefined,
      ngayThucHien: new Date().toISOString().split('T')[0],
      ghiChu: '',
    });
    setUsersInDept([]); // Reset users
    setShowAllocModal(true);
  };

  const handleAllocSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!allocFormData.taiSanId || !allocFormData.ngayThucHien || !allocFormData.denPhongBanId) {
      toast.error('Vui lòng điền đủ thông tin bắt buộc!');
      return;
    }

    setIsSubmitting(true);
    try {
      const payload: any = {
        taiSanId: allocFormData.taiSanId,
        loaiDieuChuyen: allocType, 
        ngayThucHien: allocFormData.ngayThucHien,
        tuPhongBanId: allocFormData.tuPhongBanId || undefined,
        denPhongBanId: allocFormData.denPhongBanId || undefined,
        tuNguoiDungId: allocFormData.tuNguoiDungId ? Number(allocFormData.tuNguoiDungId) : undefined, 
        denNguoiDungId: allocFormData.denNguoiDungId ? Number(allocFormData.denNguoiDungId) : undefined,
        ghiChu: allocFormData.ghiChu,
      };

      const response = await assetAllocationApi.create(payload);
      if (response.errorCode === 200) {
        toast.success('Lưu phiếu thành công!');
        setShowAllocModal(false);
        refreshData(); 
      } else {
        toast.error(response.message || 'Có lỗi xảy ra.');
      }
    } catch (error) {
      toast.error('Lỗi kết nối máy chủ.');
    } finally {
      setIsSubmitting(false);
    }
  };

  const formatCurrency = (value?: number) => {
    if (!value) return '0 ₫';
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
  };

  return (
    <div className="space-y-6 relative">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Quản lý Tài sản</h1>
          <p className="text-sm text-gray-500 mt-1">Danh sách tài sản cố định</p>
        </div>
        <div className="flex gap-3">
          <Link to="/assets/groups" className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors">
            <Package className="w-5 h-5" /> Xem theo Nhóm
          </Link>
          <Link to="/assets/new" className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors">
            <Plus className="w-5 h-5" /> Thêm Tài sản
          </Link>
        </div>
      </div>

      <div className="bg-white rounded-lg border border-gray-200 p-4">
        <div className="flex flex-col lg:flex-row gap-4">
          <div className="flex-1 relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" />
            <input type="text" placeholder="Tìm kiếm theo tên hoặc mã tài sản..." value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent" />
          </div>
          <div className="flex gap-2">
            <select value={filterStatus} onChange={(e) => setFilterStatus(e.target.value)} className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent">
              <option value="all">Tất cả trạng thái</option>
              <option value="0">Chưa cấp phát</option>
              <option value="1">Chờ xác nhận</option>
              <option value="2">Đang sử dụng</option>
              <option value="3">Đã thanh lý</option>
            </select>
            <button onClick={handleExportExcel} className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors">
              <Download className="w-5 h-5" /> Xuất Excel
            </button>
          </div>
        </div>
      </div>

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
              {isLoadingGlobal ? (
                <tr><td colSpan={8} className="text-center py-6 text-gray-500">Đang tải dữ liệu...</td></tr>
              ) : filteredAssets.length === 0 ? (
                <tr><td colSpan={8} className="text-center py-6 text-gray-500">Không tìm thấy tài sản nào</td></tr>
              ) : (
                filteredAssets.map((asset) => {
                  const assetStatusStr = asset.trangThai?.toString() || '0';
                  const currentStatus = statusConfig[assetStatusStr] || { label: 'Không xác định', color: 'bg-gray-100 text-gray-500', value: -1 };

                  return (
                    <tr key={asset.id} className="hover:bg-gray-50 transition-colors">
                      <td className="px-6 py-4 whitespace-nowrap"><span className="text-sm font-medium text-blue-600">{asset.maTaiSan}</span></td>
                      <td className="px-6 py-4"><span className="text-sm text-gray-900">{asset.tenTaiSan}</span></td>
                      <td className="px-6 py-4 whitespace-nowrap"><span className="text-sm text-gray-600">{getCatName(asset.danhMucId)}</span></td>
                      <td className="px-6 py-4 whitespace-nowrap"><span className="text-sm text-gray-600">{getDeptName(asset.phongBanId)}</span></td>
                      <td className="px-6 py-4 whitespace-nowrap text-right"><span className="text-sm text-gray-900">{formatCurrency(asset.nguyenGia)}</span></td>
                      <td className="px-6 py-4 whitespace-nowrap text-right"><span className="text-sm font-medium text-gray-900">{formatCurrency(asset.giaTriConLai)}</span></td>
                      <td className="px-6 py-4 whitespace-nowrap text-center"><span className={`inline-flex px-2 py-1 text-xs font-medium rounded-full ${currentStatus.color}`}>{currentStatus.label}</span></td>
                      <td className="px-6 py-4 whitespace-nowrap text-center">
                        <div className="flex items-center justify-center gap-2">
                          
                          {currentStatus.value === 0 && (
                            <button onClick={() => openAllocModal(asset, 'CapPhat')} className="p-1.5 text-green-600 hover:bg-green-100 rounded-md transition-colors" title="Cấp phát tài sản"><UserPlus className="w-4 h-4" /></button>
                          )}

                          {currentStatus.value === 2 && (
                            <button onClick={() => openAllocModal(asset, 'LuanChuyen')} className="p-1.5 text-blue-600 hover:bg-blue-100 rounded-md transition-colors" title="Điều chuyển tài sản"><ArrowLeftRight className="w-4 h-4" /></button>
                          )}

                          <Link to={`/assets/${asset.id}`} className="p-1.5 text-gray-600 hover:bg-gray-200 rounded-md transition-colors" title="Chi tiết"><Eye className="w-4 h-4" /></Link>

                          {(currentStatus.value === 0 || currentStatus.value === 3) && (
                            <button onClick={() => asset.id && handleDelete(asset.id)} className="p-1.5 text-red-600 hover:bg-red-100 rounded-md transition-colors" title="Xóa tài sản vĩnh viễn">
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

      {showAllocModal && selectedAssetForModal && (
        <div className="fixed inset-0 bg-gray-900/50 backdrop-blur-sm flex items-center justify-center z-[9999] p-4">
          <div className="bg-white rounded-lg max-w-xl w-full overflow-hidden shadow-2xl flex flex-col">
            <div className="p-6 border-b border-gray-200 flex items-center justify-between bg-white">
              <h3 className="font-bold text-gray-900 text-lg flex items-center gap-2">
                {allocType === 'CapPhat' ? <Send className="w-5 h-5 text-green-600" /> : <ArrowLeftRight className="w-5 h-5 text-blue-600" />}
                {allocType === 'CapPhat' ? 'Cấp phát Tài sản' : 'Điều chuyển Tài sản'}
              </h3>
              <button onClick={() => setShowAllocModal(false)} className="text-gray-400 hover:text-gray-600"><X className="w-6 h-6" /></button>
            </div>
            
            <form onSubmit={handleAllocSubmit} className="p-6 space-y-5 bg-gray-50/30">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1.5">Tài sản đang chọn</label>
                <div className="w-full px-4 py-3 border border-gray-200 rounded-lg bg-gray-100 flex items-center gap-3">
                  <Package className="w-5 h-5 text-gray-400" />
                  <span className="font-medium text-gray-700">{selectedAssetForModal.maTaiSan} - {selectedAssetForModal.tenTaiSan}</span>
                </div>
              </div>

              {allocType === 'LuanChuyen' && (
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">Từ phòng ban</label>
                  <input type="text" disabled value={getDeptName(allocFormData.tuPhongBanId)} className="w-full px-4 py-2 border border-gray-200 rounded-lg bg-gray-100 text-gray-600 cursor-not-allowed font-medium" />
                </div>
              )}

              <div className="grid grid-cols-2 gap-4">
                <div className="col-span-2">
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">
                    {allocType === 'LuanChuyen' ? 'Đến phòng ban' : 'Phòng ban nhận'} <span className="text-red-500">*</span>
                  </label>
                  <select 
                    required 
                    value={allocFormData.denPhongBanId || ''} 
                    onChange={(e) => {
                      setAllocFormData({
                        ...allocFormData, 
                        denPhongBanId: Number(e.target.value),
                        denNguoiDungId: undefined // Reset Nhân viên khi đổi phòng ban
                      });
                    }} 
                    className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 bg-white"
                  >
                    <option value="">-- Chọn phòng ban --</option>
                    {departments.map((dept: any) => (<option key={dept.id} value={dept.id}>{dept.tenPhongBan}</option>))}
                  </select>
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">
                    Nhân viên nhận {isLoadingUsers && <span className="text-blue-500 text-xs ml-1">(Đang tải...)</span>}
                  </label>
                  <select
                    value={allocFormData.denNguoiDungId || ''}
                    onChange={(e) => setAllocFormData({...allocFormData, denNguoiDungId: Number(e.target.value)})}
                    className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 bg-white disabled:bg-gray-50 disabled:text-gray-400"
                    disabled={!allocFormData.denPhongBanId || usersInDept.length === 0}
                  >
                    <option value="">
                      {!allocFormData.denPhongBanId 
                        ? '-- Chọn phòng ban trước --' 
                        : (usersInDept.length === 0 ? '-- P.Ban này chưa có NV --' : '-- Chọn nhân viên --')}
                    </option>
                    {usersInDept.map((user: any) => (
                      <option key={user.userID} value={user.userID}>
                        {user.profile?.lastName} {user.profile?.firstName} ({user.userName})
                      </option>
                    ))}
                  </select>
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">Ngày thực hiện <span className="text-red-500">*</span></label>
                  <input type="date" required value={allocFormData.ngayThucHien} onChange={(e) => setAllocFormData({...allocFormData, ngayThucHien: e.target.value})} className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" />
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1.5">Ghi chú thêm</label>
                <textarea rows={2} value={allocFormData.ghiChu || ''} onChange={(e) => setAllocFormData({...allocFormData, ghiChu: e.target.value})} className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" placeholder="Lý do cấp phát, tình trạng máy lúc giao..." />
              </div>

              <div className="pt-4 border-t border-gray-200 flex justify-end gap-3">
                <button type="button" onClick={() => setShowAllocModal(false)} className="px-5 py-2.5 border border-gray-300 rounded-lg hover:bg-gray-50 font-medium text-gray-700 transition-colors">Hủy</button>
                <button type="submit" disabled={isSubmitting} className="flex items-center gap-2 px-6 py-2.5 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 font-medium shadow-sm transition-colors"><Save className="w-4 h-4" /> {isSubmitting ? 'Đang xử lý...' : 'Lưu lại'}</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}