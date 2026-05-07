import { useState, useEffect } from 'react';
import { Link } from 'react-router';
import { 
  Plus, Search, Eye, Trash2, Download, Package, ArrowLeftRight, 
  UserPlus, X, Save, Send, Calculator, AlertCircle 
} from 'lucide-react';
import { toast } from "sonner";
import { apiClient } from '../../api/client';
import { assetApi, TaiSan ,PHUONG_THUC_THANH_TOAN_OPTIONS, PhuongThucThanhToan } from '../../api/assetApi';
import { assetAllocationApi, DieuChuyenTaiSan } from '../../api/assetAllocationApi';
import { accountApi, TaiKhoanKeToan } from '../../api/accountApi'; // <-- IMPORT TÀI KHOẢN KẾ TOÁN
import * as XLSX from 'xlsx';
import { useGlobalData } from '../../context/GlobalContext';

// Hàm hỗ trợ tạo tiền tố từ Tên danh mục tự động
const generatePrefix = (name: string) => {
  const cleanName = name.normalize("NFD").replace(/[\u0300-\u036f]/g, "").toUpperCase();
  const words = cleanName.split(' ').filter(w => w.length > 0);
  if (words.length === 1) {
    return words[0].substring(0, 3); 
  }
  return words.map(w => w[0]).join(''); 
};

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
  const [accounts, setAccounts] = useState<TaiKhoanKeToan[]>([]);

  // Load danh sách tài khoản kế toán
  useEffect(() => {
    accountApi.getAll().then(res => { if(res.errorCode === 200) setAccounts(res.data) });
  }, []);

  // ================= MODAL ĐIỀU CHUYỂN =================
  const [showAllocModal, setShowAllocModal] = useState(false);
  const [allocType, setAllocType] = useState<'CapPhat' | 'LuanChuyen'>('CapPhat');
  const [allocFormData, setAllocFormData] = useState<Partial<DieuChuyenTaiSan>>({});
  const [selectedAssetForModal, setSelectedAssetForModal] = useState<TaiSan | null>(null);
  
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

  // ================= MODAL TẠO MỚI TÀI SẢN =================
  const [showAddModal, setShowAddModal] = useState(false);
  const [addFormData, setAddFormData] = useState<Partial<TaiSan>>({
      tenTaiSan: '',
      maTaiSan: '',
      danhMucId: undefined,
      nhaSanXuat: '',
      soSeri: '',
      moTa: '',
      ngayMua: new Date().toISOString().split('T')[0],
      nguyenGia: 0,
      giaTriConLai: 0,
      thoiGianKhauHao: 36,
      phuongPhapKhauHao: 0,
      maTaiKhoan: '',
      phongBanId: undefined,
      nguoiDungId: undefined,
      trangThai: 0,
      phuongThucThanhToan: undefined,   // <-- THÊM
});

  const [usersInAddDept, setUsersInAddDept] = useState<any[]>([]);
  const [isLoadingAddUsers, setIsLoadingAddUsers] = useState(false);

  // Logic tự động tạo Mã Tài Sản (mang từ form cũ sang)
  useEffect(() => {
    if (showAddModal && addFormData.danhMucId && categories.length > 0 && assets.length >= 0) {
      const category = categories.find((c: any) => c.id === addFormData.danhMucId);
      if (category && category.tenDanhMuc) {
        const prefix = generatePrefix(category.tenDanhMuc);
        const assetsInCat = assets.filter((a: any) => a.danhMucId === addFormData.danhMucId);
        let maxSeq = 0;
        
        assetsInCat.forEach((a: any) => {
          if (a.maTaiSan) {
            const parts = a.maTaiSan.split('-');
            if (parts.length > 1) {
              const num = parseInt(parts[parts.length - 1], 10);
              if (!isNaN(num) && num > maxSeq) {
                maxSeq = num;
              }
            }
          }
        });

        const sequence = (maxSeq + 1).toString().padStart(3, '0');
        setAddFormData(prev => ({ ...prev, maTaiSan: `${prefix}-${sequence}` }));
      }
    } else if (showAddModal && !addFormData.danhMucId) {
      setAddFormData(prev => ({ ...prev, maTaiSan: '' }));
    }
  }, [addFormData.danhMucId, categories, assets, showAddModal]);

  // Effect load user cho Modal Tạo mới
  useEffect(() => {
    const fetchUsersForAddModal = async () => {
      if (!addFormData.phongBanId) {
        setUsersInAddDept([]);
        return;
      }
      setIsLoadingAddUsers(true);
      try {
        const data = await apiClient.get(`/user/GetByDepartmentId/${addFormData.phongBanId}`);
        if (data.errorCode === 200) {
          setUsersInAddDept(data.data);
        } else {
          setUsersInAddDept([]);
        }
      } catch (error) {
        setUsersInAddDept([]);
      } finally {
        setIsLoadingAddUsers(false);
      }
    };
    fetchUsersForAddModal();
  }, [addFormData.phongBanId]);

  const [isSubmitting, setIsSubmitting] = useState(false);

  // ================= CÁC HÀM XỬ LÝ =================
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

  const handleAddSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    try {
      const payload = { ...addFormData } as TaiSan;
      if (!payload.phongBanId) payload.phongBanId = undefined;
      if (!payload.danhMucId) payload.danhMucId = undefined;
      if (!payload.nguoiDungId) payload.nguoiDungId = undefined;

      const response = await assetApi.create(payload);
      if (response.errorCode === 200) {
        toast.success('Thêm tài sản thành công!');
        setShowAddModal(false);
        refreshData();
        // Reset form
       setAddFormData({
          tenTaiSan: '', maTaiSan: '', danhMucId: undefined, nhaSanXuat: '', soSeri: '',
          moTa: '', ngayMua: new Date().toISOString().split('T')[0], nguyenGia: 0, giaTriConLai: 0,
          thoiGianKhauHao: 36, phuongPhapKhauHao: 0, maTaiKhoan: '', phongBanId: undefined,
          nguoiDungId: undefined, trangThai: 0,
          phuongThucThanhToan: undefined,   // <-- THÊM
        });
      } else {
        toast.error(response.message || 'Có lỗi xảy ra khi thêm tài sản.');
      }
    } catch (error) {
      toast.error('Có lỗi xảy ra khi thêm tài sản.');
    } finally {
      setIsSubmitting(false);
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
    setUsersInDept([]);
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
        taiSanId: allocFormData.taiSanId, loaiDieuChuyen: allocType, ngayThucHien: allocFormData.ngayThucHien,
        tuPhongBanId: allocFormData.tuPhongBanId || undefined, denPhongBanId: allocFormData.denPhongBanId || undefined,
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
          <button 
            onClick={() => setShowAddModal(true)} 
            className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
          >
            <Plus className="w-5 h-5" /> Thêm Tài sản
          </button>
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
                <th className="px-4 py-3 text-center text-xs font-medium text-gray-700 uppercase tracking-wider w-16">STT</th>
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
                <tr><td colSpan={9} className="text-center py-6 text-gray-500">Đang tải dữ liệu...</td></tr>
              ) : filteredAssets.length === 0 ? (
                <tr><td colSpan={9} className="text-center py-6 text-gray-500">Không tìm thấy tài sản nào</td></tr>
              ) : (
                filteredAssets.map((asset, index) => {
                  const assetStatusStr = asset.trangThai?.toString() || '0';
                  const currentStatus = statusConfig[assetStatusStr] || { label: 'Không xác định', color: 'bg-gray-100 text-gray-500', value: -1 };

                  return (
                    <tr key={asset.id} className="hover:bg-gray-50 transition-colors">
                      
                      <td className="px-4 py-4 whitespace-nowrap text-sm text-gray-500 text-center">{index + 1}</td>
                      <td className="px-6 py-4 whitespace-nowrap"><span className="text-sm font-medium text-blue-600">{asset.maTaiSan}</span></td>
                      <td className="px-6 py-4"><span className="text-sm text-gray-900">{asset.tenTaiSan}</span></td>
                      <td className="px-6 py-4 whitespace-nowrap"><span className="text-sm text-gray-600">{getCatName(asset.danhMucId)}</span></td>
                      <td className="px-6 py-4 whitespace-nowrap"><span className="text-sm text-gray-600">{getDeptName(asset.phongBanId)}</span></td>
                      <td className="px-6 py-4 whitespace-nowrap text-right"><span className="text-sm text-gray-900">{formatCurrency(asset.nguyenGia)}</span></td>
                      <td className="px-6 py-4 whitespace-nowrap">
                          <span className="text-sm text-gray-600">
                            {asset.phuongThucThanhToan !== undefined && asset.phuongThucThanhToan !== null
                              ? PHUONG_THUC_THANH_TOAN_OPTIONS.find(o => o.value === asset.phuongThucThanhToan)?.label ?? '—'
                              : '—'}
                          </span>
                        </td>
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

      {/* MODAL THÊM TÀI SẢN MỚI FULL THÔNG TIN */}
      {showAddModal && (
        <div className="fixed inset-0 bg-gray-900/50 backdrop-blur-sm flex items-center justify-center z-[9999] p-4">
          <div className="bg-white rounded-lg max-w-4xl w-full max-h-[90vh] overflow-y-auto shadow-2xl flex flex-col">
            <div className="p-6 border-b border-gray-200 flex items-center justify-between bg-white sticky top-0 z-10">
              <h3 className="font-bold text-gray-900 text-lg flex items-center gap-2">
                <Plus className="w-5 h-5 text-blue-600" />
                Thêm mới Tài sản
              </h3>
              <button onClick={() => setShowAddModal(false)} className="text-gray-400 hover:text-gray-600">
                <X className="w-6 h-6" />
              </button>
            </div>
            
            <form onSubmit={handleAddSubmit} className="p-6 space-y-6 bg-gray-50/30">
              
              {/* Alert Note Box */}
              <div className="bg-blue-50 border border-blue-200 rounded-lg p-4 flex items-start gap-3">
                <AlertCircle className="w-5 h-5 text-blue-600 mt-0.5 shrink-0" />
                <div>
                  <h4 className="text-sm font-semibold text-blue-900">Lưu ý khi thêm tài sản</h4>
                  <p className="text-sm text-blue-700 mt-1">
                    Mã tài sản sẽ được tự động tạo theo công thức sau khi bạn chọn Nhóm tài sản (Danh mục). Nếu bạn chọn "Phòng ban" và "Nhân viên", trạng thái tài sản sẽ tự động chuyển sang luồng Cấp phát.
                  </p>
                </div>
              </div>

              {/* THÔNG TIN CƠ BẢN */}
              <div className="bg-white p-5 rounded-lg border border-gray-200 shadow-sm">
                <h4 className="font-bold text-gray-900 mb-4 text-base border-b pb-2">Thông tin cơ bản</h4>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">Tên tài sản <span className="text-red-500">*</span></label>
                    <input 
                      type="text" required 
                      value={addFormData.tenTaiSan || ''} 
                      onChange={(e) => setAddFormData({...addFormData, tenTaiSan: e.target.value})} 
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 bg-white" 
                      placeholder="VD: MacBook Pro 16 inch M3 Max" 
                    />
                  </div>
                  <div>
                 <label className="block text-sm font-medium text-gray-700 mb-1.5">
            Phương thức thanh toán
                    </label>
                    <select
                      value={addFormData.phuongThucThanhToan ?? ''}
                      onChange={(e) =>
                        setAddFormData({
                          ...addFormData,
                          phuongThucThanhToan: e.target.value !== ''
                            ? (Number(e.target.value) as PhuongThucThanhToan)
                            : undefined,
                        })
                      }
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 bg-white"
                    >
                      <option value="">-- Chọn phương thức --</option>
                      {PHUONG_THUC_THANH_TOAN_OPTIONS.map(opt => (
                        <option key={opt.value} value={opt.value}>{opt.label}</option>
                      ))}
                    </select>
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">Nhóm tài sản (Danh mục) <span className="text-red-500">*</span></label>
                    <select 
                      required
                      value={addFormData.danhMucId || ''} 
                      onChange={(e) => setAddFormData({...addFormData, danhMucId: Number(e.target.value)})} 
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 bg-white"
                    >
                      <option value="">-- Chọn nhóm tài sản --</option>
                      {categories.map((cat: any) => (<option key={cat.id} value={cat.id}>{cat.tenDanhMuc}</option>))}
                    </select>
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">Mã tài sản</label>
                    <input 
                      type="text" required 
                      value={addFormData.maTaiSan || ''} 
                      readOnly
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg bg-gray-100 cursor-not-allowed text-gray-500 font-medium focus:outline-none" 
                      placeholder="Tự động sinh khi chọn nhóm tài sản" 
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">Số Serial</label>
                    <input 
                      type="text" 
                      value={addFormData.soSeri || ''} 
                      onChange={(e) => setAddFormData({...addFormData, soSeri: e.target.value})} 
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 bg-white" 
                      placeholder="Nhập số Serial" 
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">Nguồn gốc / Nhà sản xuất</label>
                    <input 
                      type="text" 
                      value={addFormData.nhaSanXuat || ''} 
                      onChange={(e) => setAddFormData({...addFormData, nhaSanXuat: e.target.value})} 
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 bg-white" 
                      placeholder="Nhập địa chỉ hoặc tên NSX" 
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">Ngày mua <span className="text-red-500">*</span></label>
                    <input 
                      type="date" required 
                      value={addFormData.ngayMua || ''} 
                      onChange={(e) => setAddFormData({...addFormData, ngayMua: e.target.value})} 
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 bg-white" 
                    />
                  </div>
                  <div className="md:col-span-2">
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">Mô tả thêm</label>
                    <input 
                      type="text" 
                      value={addFormData.moTa || ''} 
                      onChange={(e) => setAddFormData({...addFormData, moTa: e.target.value})} 
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 bg-white" 
                      placeholder="Cấu hình, tình trạng..." 
                    />
                  </div>
                </div>
              </div>

              {/* THÔNG TIN KẾ TOÁN */}
              <div className="bg-gradient-to-r from-blue-50 to-purple-50 rounded-lg border border-blue-200 p-5">
                <div className="flex items-center gap-2 mb-4">
                  <Calculator className="w-5 h-5 text-blue-600" />
                  <h4 className="font-semibold text-gray-900 text-sm border-b border-blue-200/50 pb-2 w-full">Thông tin Kế toán & Khấu hao</h4>
                </div>
                
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div className="bg-white rounded-md p-4 border border-gray-100 shadow-sm">
                    <label className="block text-xs font-semibold text-gray-500 mb-1.5 uppercase tracking-wide">Nguyên giá (VNĐ) <span className="text-red-500">*</span></label>
                    <input 
                      type="number" required min="0"
                      value={addFormData.nguyenGia === 0 ? '' : addFormData.nguyenGia} 
                      onChange={(e) => {
                        const val = Number(e.target.value);
                        setAddFormData({...addFormData, nguyenGia: val, giaTriConLai: val});
                      }} 
                      className="w-full px-3 py-2 border border-gray-200 rounded-md focus:ring-2 focus:ring-blue-500 text-gray-900 font-bold" 
                      placeholder="0" 
                    />
                  </div>

                  <div className="bg-white rounded-md p-4 border border-gray-100 shadow-sm">
                    <label className="block text-xs font-semibold text-gray-500 mb-1.5 uppercase tracking-wide">Tài khoản Kế toán <span className="text-red-500">*</span></label>
                    <select 
                      required
                      value={addFormData.maTaiKhoan || ''} 
                      onChange={(e) => setAddFormData({...addFormData, maTaiKhoan: e.target.value})} 
                      className="w-full px-3 py-2 border border-gray-200 rounded-md focus:ring-2 focus:ring-blue-500 text-blue-600 font-medium bg-white"
                    >
                      <option value="">-- Chọn tài khoản --</option>
                      {accounts.map(acc => (
                        <option key={acc.id} value={acc.maTaiKhoan}>
                          {acc.maTaiKhoan} {acc.tenTaiKhoan ? `- ${acc.tenTaiKhoan}` : ''}
                        </option>
                      ))}
                    </select>
                  </div>

                  <div className="bg-white rounded-md p-4 border border-gray-100 shadow-sm">
                    <label className="block text-xs font-semibold text-gray-500 mb-1.5 uppercase tracking-wide">Thời gian KH (Tháng) <span className="text-red-500">*</span></label>
                    <input 
                      type="number" required min="1"
                      value={addFormData.thoiGianKhauHao === 0 ? '' : addFormData.thoiGianKhauHao} 
                      onChange={(e) => setAddFormData({...addFormData, thoiGianKhauHao: Number(e.target.value)})} 
                      className="w-full px-3 py-2 border border-gray-200 rounded-md focus:ring-2 focus:ring-blue-500 text-gray-900 font-bold" 
                      placeholder="VD: 36" 
                    />
                  </div>

                  <div className="bg-white rounded-md p-4 border border-gray-100 shadow-sm">
                    <label className="block text-xs font-semibold text-gray-500 mb-1.5 uppercase tracking-wide">Phương pháp khấu hao <span className="text-red-500">*</span></label>
                    <select 
                      value={0} 
                      disabled 
                      className="w-full px-3 py-2 border border-gray-200 rounded-md bg-gray-50 cursor-not-allowed text-gray-500 font-medium"
                    >
                      <option value={0}>Khấu hao đường thẳng</option>
                    </select>
                  </div>
                </div>
                
                <div className="mt-4 flex items-center justify-between text-sm bg-white/60 p-3 rounded border border-blue-100">
                  <span className="text-gray-600">Dự kiến mức khấu hao hàng tháng:</span>
                  <span className="font-bold text-blue-700">
                    {addFormData.nguyenGia && addFormData.thoiGianKhauHao 
                      ? formatCurrency(Math.round(addFormData.nguyenGia / addFormData.thoiGianKhauHao)) 
                      : '0 ₫'}
                  </span>
                </div>
              </div>

              {/* KHỐI PHÂN BỔ / CẤP PHÁT NHANH */}
              <div className="bg-blue-50/50 border border-blue-100 rounded-lg p-5">
                <div className="flex items-center gap-2 mb-4 border-b border-blue-100 pb-2">
                  <UserPlus className="w-5 h-5 text-blue-600" />
                  <h4 className="font-semibold text-gray-900 text-sm">Phân bổ / Cấp phát nhanh (Tùy chọn)</h4>
                </div>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">Phòng ban quản lý</label>
                    <select 
                      value={addFormData.phongBanId || ''} 
                      onChange={(e) => {
                        const newDeptId = e.target.value ? Number(e.target.value) : undefined;
                        setAddFormData({
                          ...addFormData, 
                          phongBanId: newDeptId,
                          nguoiDungId: undefined, // Reset người dùng khi đổi phòng ban
                          trangThai: newDeptId ? 1 : 0 // Nếu chọn phòng ban, tự chuyển thành Chờ xác nhận
                        });
                      }} 
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 bg-white"
                    >
                      <option value="">-- Chưa gắn phòng ban --</option>
                      {departments.map((dept: any) => (<option key={dept.id} value={dept.id}>{dept.tenPhongBan}</option>))}
                    </select>
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">
                      Nhân viên sử dụng {isLoadingAddUsers && <span className="text-blue-500 text-xs ml-1">(Đang tải...)</span>}
                    </label>
                    <select
                      value={addFormData.nguoiDungId || ''}
                      onChange={(e) => {
                        const newUserId = e.target.value ? Number(e.target.value) : undefined;
                        setAddFormData({
                          ...addFormData, 
                          nguoiDungId: newUserId,
                          trangThai: (addFormData.phongBanId || newUserId) ? 1 : 0
                        });
                      }}
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 bg-white disabled:bg-gray-100 disabled:text-gray-400"
                      disabled={!addFormData.phongBanId || usersInAddDept.length === 0}
                    >
                      <option value="">
                        {!addFormData.phongBanId 
                          ? '-- Chọn phòng ban trước --' 
                          : (usersInAddDept.length === 0 ? '-- P.Ban này chưa có NV --' : '-- Chọn nhân viên --')}
                      </option>
                      {usersInAddDept.map((user: any) => {
                        const fullName = user.profile 
                          ? `${user.profile.lastName} ${user.profile.firstName}`.trim() 
                          : user.userName;
                        return (
                          <option key={user.userID} value={user.userID}>
                            {fullName} ({user.email || user.userName})
                          </option>
                        );
                      })}
                    </select>
                  </div>
                  
                  <div className="md:col-span-2 p-3 bg-white border border-gray-200 rounded-lg flex items-center">
                    <div className="flex-1">
                      <label className="block text-sm font-medium text-gray-700 mb-1">Trạng thái luồng cấp phát</label>
                      <select 
                        value={addFormData.trangThai ?? 0} 
                        onChange={(e) => setAddFormData({...addFormData, trangThai: Number(e.target.value)})} 
                        className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 bg-white"
                      >
                        <option value={0}>Chưa cấp phát</option>
                        <option value={1}>Chờ người dùng xác nhận</option>
                        <option value={2}>Đang sử dụng</option>
                      </select>
                    </div>
                  </div>
                </div>
              </div>

              {/* NÚT ACTION */}
              <div className="pt-2 flex justify-end gap-3 sticky bottom-0">
                <button type="button" onClick={() => setShowAddModal(false)} className="px-5 py-2.5 border border-gray-300 rounded-lg hover:bg-gray-100 font-medium text-gray-700">
                  Hủy bỏ
                </button>
                <button type="submit" disabled={isSubmitting} className="flex items-center gap-2 px-6 py-2.5 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 font-medium shadow-sm">
                  <Save className="w-4 h-4" /> {isSubmitting ? 'Đang lưu...' : 'Thêm tài sản'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* MODAL CẤP PHÁT / LUÂN CHUYỂN BÊN NGOÀI BẢNG */}
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
                        denNguoiDungId: undefined 
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
                    {usersInDept.map((user: any) => {
                      const fullName = user.profile 
                        ? `${user.profile.lastName} ${user.profile.firstName}`.trim() 
                        : user.userName;
                      return (
                        <option key={user.userID} value={user.userID}>
                          {fullName} ({user.email || user.userName})
                        </option>
                      );
                    })}
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