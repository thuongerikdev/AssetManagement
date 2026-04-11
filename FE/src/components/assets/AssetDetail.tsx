import { useState, useEffect, useCallback } from 'react';
import { Link, useParams, useNavigate } from 'react-router';
import { 
  ArrowLeft, Edit, Send, ArrowLeftRight, Trash2, Printer, 
  Wrench, DollarSign, TrendingDown, Calendar, Building2, 
  Settings, History, LineChart, FileText, Paperclip, Home, ChevronRight, X, Save, 
  Package, AlertCircle, Calculator
} from 'lucide-react';
import { toast } from "sonner";

// APIs
import { assetApi, TaiSan } from '../../api/assetApi';
import { departmentApi, Department } from '../../api/departmentApi';
import { assetCategoryApi, AssetCategory } from '../../api/assetCategoryApi';
import { assetAllocationApi, DieuChuyenTaiSan } from '../../api/assetAllocationApi';
import { maintenanceApi, BaoTriTaiSan } from '../../api/maintenanceApi';
import { liquidationApi, ThanhLyTaiSan } from '../../api/liquidationApi';
import { voucherApi } from '../../api/voucherApi';
import { depreciationHistoryApi } from '../../api/depreciationHistoryApi';

// CONFIG TRẠNG THÁI
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

const loaiBaoTriMap: Record<string, string> = { '0': 'Bảo trì định kỳ', '1': 'Sửa chữa', '2': 'Nâng cấp', '3': 'Vệ sinh', '4': 'Kiểm tra' };
const loaiDieuChuyenMap: Record<string, string> = { 
  'CapPhat': 'Cấp phát', '0': 'Cấp phát', 
  'ThuHoi': 'Thu hồi', '1': 'Thu hồi',
  'LuanChuyen': 'Điều chuyển', '2': 'Điều chuyển' 
};

export function AssetDetail() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [asset, setAsset] = useState<TaiSan | null>(null);
  const [departments, setDepartments] = useState<Department[]>([]);
  const [categories, setCategories] = useState<AssetCategory[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  
  const [activeTab, setActiveTab] = useState<'general' | 'usage' | 'depreciation' | 'vouchers'>('general');
  const [historyTimeline, setHistoryTimeline] = useState<any[]>([]);
  const [isLoadingHistory, setIsLoadingHistory] = useState(false);

  // --- STATE MODALS ---
  const [isSubmitting, setIsSubmitting] = useState(false);
  
  // 1. Cấp phát/Điều chuyển
  const [showAllocModal, setShowAllocModal] = useState(false);
  const [allocType, setAllocType] = useState<'CapPhat' | 'LuanChuyen'>('CapPhat');
  const [allocFormData, setAllocFormData] = useState<Partial<DieuChuyenTaiSan>>({});

  // 2. Bảo trì
  const [showMaintModal, setShowMaintModal] = useState(false);
  const [maintFormData, setMaintFormData] = useState<Partial<BaoTriTaiSan>>({});
  const [maintHasCost, setMaintHasCost] = useState(false);

  // 3. Thanh lý
  const [showLiqModal, setShowLiqModal] = useState(false);
  const [liqFormData, setLiqFormData] = useState<Partial<ThanhLyTaiSan>>({});

  const [vouchers, setVouchers] = useState<any[]>([]);
  const [isLoadingVouchers, setIsLoadingVouchers] = useState(false);

  const [depreciations, setDepreciations] = useState<any[]>([]);
  const [isLoadingDepreciations, setIsLoadingDepreciations] = useState(false);

  // Hook gọi API khi mở tab Lịch sử khấu hao
  useEffect(() => {
    if (activeTab === 'depreciation' && asset?.id) {
      const fetchDepreciations = async () => {
        setIsLoadingDepreciations(true);
        try {
          const res = await depreciationHistoryApi.getByAssetId(asset.id!);
          if (res.errorCode === 200) {
            setDepreciations(res.data);
          }
        } catch (error) {
          toast.error("Không thể tải lịch sử khấu hao.");
        } finally {
          setIsLoadingDepreciations(false);
        }
      };
      fetchDepreciations();
    }
  }, [activeTab, asset?.id]);


  useEffect(() => {
    if (activeTab === 'vouchers' && asset?.id) {
      const fetchVouchers = async () => {
        setIsLoadingVouchers(true);
        try {
          const res = await voucherApi.getByAssetId(asset.id!);
          if (res.errorCode === 200) {
            setVouchers(res.data);
          }
        } catch (error) {
          toast.error("Không thể tải danh sách chứng từ.");
        } finally {
          setIsLoadingVouchers(false);
        }
      };
      fetchVouchers();
    }
  }, [activeTab, asset?.id]);

  const loadAssetData = useCallback(async () => {
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
  }, [id]);

  useEffect(() => {
    if (id) loadAssetData();
  }, [id, loadAssetData]);

  useEffect(() => {
    if (activeTab === 'usage' && asset?.id) {
      fetchHistoryData(asset.id);
    }
  }, [activeTab, asset?.id]);

  // HÀM HỖ TRỢ XỬ LÝ LỊCH SỬ THÔNG MINH
  const getDeptName = (deptId?: number) => departments.find(d => d.id === deptId)?.tenPhongBan || 'Kho / Chưa xác định';
  
  const fetchHistoryData = async (assetId: number) => {
    setIsLoadingHistory(true);
    try {
      const [allocRes, maintRes, liqRes] = await Promise.all([
        assetAllocationApi.getByAssetId(assetId),
        maintenanceApi.getByAssetId(assetId),
        liquidationApi.getByAssetId(assetId)
      ]);

      let timelineData: any[] = [];

      // 1. LỊCH SỬ CẤP PHÁT / ĐIỀU CHUYỂN
      if (allocRes.errorCode === 200 && allocRes.data) {
        allocRes.data.forEach((item: any) => {
          let actionDesc = '';
          
          // Chuyển kiểu về chuỗi để kiểm tra an toàn (bắt cả số lẫn chữ)
          const type = item.loaiDieuChuyen?.toString();
          
          if (type === 'CapPhat' || type === '0') {
            actionDesc = `Đã cấp phát tài sản cho ${getDeptName(item.denPhongBanId)}.`;
            if (item.denNguoiDungId) actionDesc += ` (Mã NV nhận: ${item.denNguoiDungId})`;
          } else if (type === 'LuanChuyen' || type === '2') {
            actionDesc = `Điều chuyển tài sản từ ${getDeptName(item.tuPhongBanId)} sang ${getDeptName(item.denPhongBanId)}.`;
          } else if (type === 'ThuHoi' || type === '1') {
            actionDesc = `Đã thu hồi tài sản từ ${getDeptName(item.tuPhongBanId)} về kho.`;
          }

          const finalDesc = item.ghiChu ? `${actionDesc} Ghi chú: ${item.ghiChu}` : actionDesc;

          timelineData.push({
            id: `alloc_${item.id}`,
            date: new Date(item.ngayThucHien || item.ngayTao || new Date()),
            title: loaiDieuChuyenMap[type] || 'Điều chuyển',
            description: finalDesc,
            details: [], 
            icon: (type === 'CapPhat' || type === '0') ? Send : ArrowLeftRight,
            iconColor: (type === 'CapPhat' || type === '0') ? 'bg-green-100 text-green-600' : 'bg-blue-100 text-blue-600'
          });
        });
      }

      // 2. LỊCH SỬ BẢO TRÌ / SỬA CHỮA
      if (maintRes.errorCode === 200 && maintRes.data) {
        maintRes.data.forEach((item: any) => {
          const mainDesc = item.moTa ? `Nội dung: ${item.moTa}.` : 'Thực hiện bảo trì/sửa chữa thiết bị.';
          const finalDesc = item.ghiChu ? `${mainDesc} Ghi chú thêm: ${item.ghiChu}` : mainDesc;

          timelineData.push({
            id: `maint_${item.id}`,
            date: new Date(item.ngayThucHien || new Date()),
            title: loaiBaoTriMap[item.loaiBaoTri?.toString()] || 'Bảo trì',
            description: finalDesc,
            details: [
              `Đơn vị thực hiện: ${item.nhaCungCap || 'Nội bộ'}`,
              // SỬA LẠI ĐIỀU KIỆN HIỂN THỊ CHI PHÍ: Cứ chiPhi > 0 là hiển thị định dạng tiền
              (item.chiPhi && item.chiPhi > 0) ? `Chi phí: ${formatCurrency(item.chiPhi)}` : 'Chi phí: Không phát sinh'
            ],
            icon: Wrench,
            iconColor: 'bg-orange-100 text-orange-600'
          });
        });
      }

      // 3. LỊCH SỬ THANH LÝ
      if (liqRes.errorCode === 200 && liqRes.data) {
        liqRes.data.forEach((item: any) => {
          const reasonDesc = item.lyDo ? `Lý do: ${item.lyDo}.` : 'Tiến hành thanh lý tài sản.';
          const finalDesc = item.ghiChu ? `${reasonDesc} Ghi chú: ${item.ghiChu}` : reasonDesc;
          
          let statusText = 'Đang chờ xử lý';
          if (item.trangThai === 'DaHoanThanh' || item.trangThai === 2 || item.trangThai === '2') statusText = 'Hoàn tất (Đã ghi sổ)';
          if (item.trangThai === 'DaDuyet' || item.trangThai === 1 || item.trangThai === '1') statusText = 'Đã duyệt';

          timelineData.push({
            id: `liq_${item.id}`,
            date: new Date(item.ngayThanhLy || item.ngayTao || new Date()),
            title: 'Thanh lý tài sản',
            description: finalDesc,
            details: [
              `Thu hồi: ${formatCurrency(item.giaTriThanhLy)}`,
              `Trạng thái: ${statusText}`
            ],
            icon: Trash2,
            iconColor: 'bg-red-100 text-red-600'
          });
        });
      }

      // Sắp xếp timeline theo ngày mới nhất lên trên
      timelineData.sort((a, b) => b.date.getTime() - a.date.getTime());
      setHistoryTimeline(timelineData);

    } catch (error) {
      toast.error('Lỗi khi tải dữ liệu lịch sử.');
    } finally {
      setIsLoadingHistory(false);
    }
  };

  // ==========================================
  // HANDLERS CẤP PHÁT / ĐIỀU CHUYỂN
  // ==========================================
  const openAllocModal = (type: 'CapPhat' | 'LuanChuyen') => {
    setAllocType(type);
    setAllocFormData({
      taiSanId: asset?.id,
      tuPhongBanId: type === 'LuanChuyen' ? asset?.phongBanId : undefined,
      denPhongBanId: undefined,
      tuNguoiDungId: type === 'LuanChuyen' ? asset?.nguoiDungId : undefined,
      denNguoiDungId: undefined,
      ngayThucHien: new Date().toISOString().split('T')[0],
      ghiChu: '',
    });
    setShowAllocModal(true);
  };

  const handleAllocSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
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
        loadAssetData(); 
        if (activeTab === 'usage') fetchHistoryData(asset!.id!); 
      } else {
        toast.error(response.message || 'Có lỗi xảy ra.');
      }
    } catch (error) {
      toast.error('Lỗi kết nối máy chủ.');
    } finally {
      setIsSubmitting(false);
    }
  };

  // ==========================================
  // HANDLERS BẢO TRÌ
  // ==========================================
  const openMaintModal = () => {
    setMaintFormData({
      taiSanId: asset?.id,
      ngayThucHien: new Date().toISOString().split('T')[0],
      loaiBaoTri: '0',
      moTa: 'Ghi nhận bảo trì/sửa chữa',
      chiPhi: undefined,
      loaiChiPhi: '',
      nhaCungCap: '',
      ghiChu: '',
      trangThai: 2 // Hoàn thành
    });
    setMaintHasCost(false);
    setShowMaintModal(true);
  };

  const handleMaintSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    try {
      const payload = { 
        ...maintFormData,
        coChiPhi: maintHasCost 
      } as any; 
      
      if (!maintHasCost) payload.chiPhi = 0; 

      const response = await maintenanceApi.create(payload);
      if (response.errorCode === 200) {
        toast.success('Ghi nhận bảo trì thành công!');
        setShowMaintModal(false);
        loadAssetData();
        if (activeTab === 'usage') fetchHistoryData(asset!.id!);
      } else {
        toast.error(response.message || 'Lỗi khi lưu.');
      }
    } catch (error) {
      toast.error('Lỗi kết nối máy chủ.');
    } finally {
      setIsSubmitting(false);
    }
  };

  // ==========================================
  // HANDLERS THANH LÝ
  // ==========================================
  const openLiqModal = () => {
    setLiqFormData({
      taiSanId: asset?.id,
      ngayThanhLy: new Date().toISOString().split('T')[0],
      giaTriThanhLy: 0,
      lyDo: '',
      ghiChu: '',
      trangThai: 'ChoDuyet'
    });
    setShowLiqModal(true);
  };

  const handleLiqSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    try {
      const payload = { ...liqFormData } as ThanhLyTaiSan;
      const response = await liquidationApi.create(payload);
      if (response.errorCode === 200) {
        toast.success('Tạo phiếu thanh lý thành công!');
        setShowLiqModal(false);
        loadAssetData();
        if (activeTab === 'usage') fetchHistoryData(asset!.id!);
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

  const getCatName = (catId?: number) => categories.find(c => c.id === catId)?.tenDanhMuc || 'Chưa phân loại';

  const handleDelete = async () => {
    if (statusInfo.value !== 3) {
      toast.error('Chỉ được phép xóa vĩnh viễn các tài sản "Đã thanh lý"!');
      return;
    }
    if (window.confirm(`Bạn có chắc chắn muốn xóa vĩnh viễn tài sản [${asset?.maTaiSan}] khỏi hệ thống?`)) {
      try {
        const response = await assetApi.delete(asset!.id!);
        if (response.errorCode === 200) {
          toast.success('Xóa tài sản thành công');
          navigate(-1);
        } else {
          toast.error(response.message || 'Lỗi khi xóa tài sản');
        }
      } catch (error) {
        toast.error('Không thể kết nối đến máy chủ');
      }
    }
  };

  if (isLoading) {
    return <div className="text-center py-12 text-gray-500">Đang tải chi tiết tài sản...</div>;
  }

  if (!asset) {
    return (
      <div className="text-center py-12">
        <h2 className="text-xl font-bold text-gray-700">Tài sản không tồn tại</h2>
        <button onClick={() => navigate(-1)} className="mt-4 text-blue-600 hover:underline">Quay lại trang trước</button>
      </div>
    );
  }

  const currentStatusStr = asset.trangThai?.toString() || '0';
  const statusInfo = statusConfig[currentStatusStr] || { label: 'Không xác định', color: 'bg-gray-100 text-gray-500', value: -1 };

  const remainingValue = asset?.giaTriConLai || 0;
  const liquidationValue = Number(liqFormData.giaTriThanhLy) || 0;
  const profitLoss = liquidationValue - remainingValue;

  return (
    <div className="max-w-full mx-auto space-y-4 pb-12 relative">
      {/* Breadcrumb & Back Button */}
      <div className="flex flex-col gap-3">
        <div className="flex items-center text-sm text-gray-500">
          <Home className="w-4 h-4 mr-2" />
          <ChevronRight className="w-4 h-4 mx-1" />
          <Link to="/assets" className="hover:text-blue-600 transition-colors">Danh sách tài sản</Link>
          <ChevronRight className="w-4 h-4 mx-1" />
          <span className="font-medium text-gray-900">Chi tiết tài sản</span>
        </div>
        
        <button 
          onClick={() => navigate(-1)}
          className="flex items-center gap-2 text-gray-600 hover:text-gray-900 transition-colors w-fit font-medium"
        >
          <ArrowLeft className="w-4 h-4" /> Quay lại
        </button>
      </div>

      {/* Header: Title & Action Buttons */}
      <div className="flex flex-col xl:flex-row xl:items-start justify-between gap-6 pb-2">
        <div>
          <div className="flex items-center gap-3">
            <h1 className="text-2xl font-bold text-gray-900">{asset.tenTaiSan}</h1>
            <span className={`inline-flex px-2.5 py-1 text-xs font-semibold rounded-full ${statusInfo.color}`}>
              {statusInfo.label}
            </span>
          </div>
          <p className="text-gray-500 mt-1.5 font-medium">Mã tài sản: <span className="text-blue-600">{asset.maTaiSan}</span></p>
        </div>

        <div className="flex flex-wrap items-center gap-2">
          <button 
            onClick={handleDelete} 
            disabled={statusInfo.value !== 3}
            className={`flex items-center gap-2 px-4 py-2 text-sm font-medium rounded-lg transition-colors border ${
              statusInfo.value === 3 ? 'bg-red-50 text-red-600 border-red-200 hover:bg-red-100' : 'bg-gray-50 text-gray-400 border-gray-200 cursor-not-allowed'
            }`}
          >
            <Trash2 className="w-4 h-4" /> Xóa
          </button>

          {statusInfo.value !== 3 && (
            <button 
              onClick={openLiqModal}
              className="flex items-center gap-2 px-4 py-2 bg-orange-50 text-orange-600 border border-orange-200 text-sm font-medium rounded-lg hover:bg-orange-100 transition-colors"
            >
              Thanh lý
            </button>
          )}

          {statusInfo.value === 2 && (
            <button 
              onClick={openMaintModal}
              className="flex items-center gap-2 px-4 py-2 bg-teal-50 text-teal-600 border border-teal-200 text-sm font-medium rounded-lg hover:bg-teal-100 transition-colors"
            >
              <Wrench className="w-4 h-4" /> Bảo trì
            </button>
          )}

          {statusInfo.value === 0 && (
            <button 
              onClick={() => openAllocModal('CapPhat')}
              className="flex items-center gap-2 px-4 py-2 bg-green-50 text-green-700 border border-green-200 text-sm font-medium rounded-lg hover:bg-green-100 transition-colors cursor-pointer"
            >
              <Send className="w-4 h-4" /> Cấp phát
            </button>
          )}

          {statusInfo.value === 2 && (
            <button 
              onClick={() => openAllocModal('LuanChuyen')}
              className="flex items-center gap-2 px-4 py-2 bg-blue-50 text-blue-700 border border-blue-200 text-sm font-medium rounded-lg hover:bg-blue-100 transition-colors cursor-pointer"
            >
              <ArrowLeftRight className="w-4 h-4" /> Điều chuyển
            </button>
          )}

          <Link to={`/assets/${id}/edit`} className="flex items-center gap-2 px-4 py-2 bg-white text-gray-700 border border-gray-300 text-sm font-medium rounded-lg hover:bg-gray-50 transition-colors shadow-sm">
            <Edit className="w-4 h-4" /> Chỉnh sửa
          </Link>

          <button className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white text-sm font-medium rounded-lg hover:bg-blue-700 transition-colors shadow-sm">
            <Printer className="w-4 h-4" /> In thông tin
          </button>
        </div>
      </div>

      {/* Summary Cards */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-0 bg-white rounded-xl border border-gray-200 shadow-sm overflow-hidden divide-y sm:divide-y-0 sm:divide-x divide-gray-100">
        <div className="p-5 flex items-center gap-4 hover:bg-gray-50 transition-colors">
          <div className="w-12 h-12 rounded-xl bg-blue-50 flex items-center justify-center text-blue-600 shrink-0">
            <DollarSign className="w-6 h-6" />
          </div>
          <div>
            <p className="text-xs font-semibold text-gray-500 uppercase tracking-wide mb-1">Nguyên giá</p>
            <p className="text-lg font-bold text-gray-900">{formatCurrency(asset.nguyenGia)}</p>
          </div>
        </div>
        <div className="p-5 flex items-center gap-4 hover:bg-gray-50 transition-colors">
          <div className="w-12 h-12 rounded-xl bg-green-50 flex items-center justify-center text-green-600 shrink-0">
            <TrendingDown className="w-6 h-6" />
          </div>
          <div>
            <p className="text-xs font-semibold text-gray-500 uppercase tracking-wide mb-1">Giá trị còn lại</p>
            <p className="text-lg font-bold text-gray-900">{formatCurrency(asset.giaTriConLai)}</p>
          </div>
        </div>
        <div className="p-5 flex items-center gap-4 hover:bg-gray-50 transition-colors">
          <div className="w-12 h-12 rounded-xl bg-orange-50 flex items-center justify-center text-orange-500 shrink-0">
            <Calendar className="w-6 h-6" />
          </div>
          <div>
            <p className="text-xs font-semibold text-gray-500 uppercase tracking-wide mb-1">Ngày mua</p>
            <p className="text-lg font-bold text-gray-900">{asset.ngayMua ? new Date(asset.ngayMua).toLocaleDateString('vi-VN') : 'N/A'}</p>
          </div>
        </div>
        <div className="p-5 flex items-center gap-4 hover:bg-gray-50 transition-colors">
          <div className="w-12 h-12 rounded-xl bg-purple-50 flex items-center justify-center text-purple-600 shrink-0">
            <Building2 className="w-6 h-6" />
          </div>
          <div>
            <p className="text-xs font-semibold text-gray-500 uppercase tracking-wide mb-1">Phòng ban</p>
            <p className="text-lg font-bold text-gray-900 truncate max-w-[150px]">{getDeptName(asset.phongBanId)}</p>
          </div>
        </div>
      </div>

      {/* Tabs Section */}
      <div className="bg-white rounded-xl border border-gray-200 shadow-sm overflow-hidden flex flex-col">
        <div className="flex items-center overflow-x-auto no-scrollbar border-b border-gray-200 px-2 pt-2">
          <button 
            onClick={() => setActiveTab('general')}
            className={`flex items-center gap-2 px-4 py-3 font-medium text-sm transition-all border-b-2 ${activeTab === 'general' ? 'border-blue-600 text-blue-600' : 'border-transparent text-gray-500 hover:text-gray-900 hover:border-gray-300'}`}
          >
            <Settings className="w-4 h-4" /> Thông tin chung
          </button>
          
          <button 
            onClick={() => setActiveTab('usage')}
            className={`flex items-center gap-2 px-4 py-3 font-medium text-sm transition-all border-b-2 ${activeTab === 'usage' ? 'border-blue-600 text-blue-600' : 'border-transparent text-gray-500 hover:text-gray-900 hover:border-gray-300'}`}
          >
            <History className="w-4 h-4" /> Lịch sử sử dụng
          </button>
          
          <button 
            onClick={() => setActiveTab('depreciation')}
            className={`flex items-center gap-2 px-4 py-3 font-medium text-sm transition-all border-b-2 ${activeTab === 'depreciation' ? 'border-blue-600 text-blue-600' : 'border-transparent text-gray-500 hover:text-gray-900 hover:border-gray-300'}`}
          >
            <LineChart className="w-4 h-4" /> Lịch sử khấu hao
          </button>

          <button 
            onClick={() => setActiveTab('vouchers')}
            className={`flex items-center gap-2 px-4 py-3 font-medium text-sm transition-all border-b-2 ${activeTab === 'vouchers' ? 'border-blue-600 text-blue-600' : 'border-transparent text-gray-500 hover:text-gray-900 hover:border-gray-300'}`}
          >
            <FileText className="w-4 h-4" /> Chứng từ kế toán
          </button>

          <div className="flex-1"></div>
          <button className="px-4 py-3 text-gray-400 hover:text-gray-700 transition-colors">
            <Paperclip className="w-5 h-5" />
          </button>
        </div>

        <div className="p-6 bg-gray-50/30">
          
          {/* TAB 1: THÔNG TIN CHUNG */}
          {activeTab === 'general' && (
            <div className="grid grid-cols-1 md:grid-cols-2 gap-x-12 gap-y-8">
              <div className="space-y-6">
                <div>
                  <h3 className="text-base font-bold text-gray-900 mb-4 border-b pb-2">Phân loại & Đặc điểm</h3>
                  <div className="space-y-4">
                    <div className="grid grid-cols-3 gap-4 border-b border-gray-100 pb-3">
                      <p className="text-sm text-gray-500 col-span-1">Tên tài sản:</p>
                      <p className="text-sm font-medium text-gray-900 col-span-2">{asset.tenTaiSan}</p>
                    </div>
                    <div className="grid grid-cols-3 gap-4 border-b border-gray-100 pb-3">
                      <p className="text-sm text-gray-500 col-span-1">Danh mục:</p>
                      <p className="text-sm font-medium text-gray-900 col-span-2">{getCatName(asset.danhMucId)}</p>
                    </div>
                    <div className="grid grid-cols-3 gap-4 border-b border-gray-100 pb-3">
                      <p className="text-sm text-gray-500 col-span-1">Số Serial:</p>
                      <p className="text-sm font-medium text-gray-900 col-span-2">{asset.soSeri || '-'}</p>
                    </div>
                    <div className="grid grid-cols-3 gap-4 border-b border-gray-100 pb-3">
                      <p className="text-sm text-gray-500 col-span-1">Nhà sản xuất:</p>
                      <p className="text-sm font-medium text-gray-900 col-span-2">{asset.nhaSanXuat || '-'}</p>
                    </div>
                    <div className="grid grid-cols-3 gap-4">
                      <p className="text-sm text-gray-500 col-span-1">Mô tả thêm:</p>
                      <p className="text-sm text-gray-900 col-span-2 whitespace-pre-wrap">{asset.moTa || 'Không có mô tả'}</p>
                    </div>
                  </div>
                </div>

                <div>
                  <h3 className="text-base font-bold text-gray-900 mb-4 border-b pb-2 mt-8">Theo dõi Phân bổ</h3>
                  <div className="space-y-4">
                    <div className="grid grid-cols-3 gap-4 border-b border-gray-100 pb-3">
                      <p className="text-sm text-gray-500 col-span-1">Mã Nhân viên:</p>
                      <p className="text-sm font-medium text-gray-900 col-span-2">{asset.nguoiDungId ? `User #${asset.nguoiDungId}` : 'Chưa gắn User'}</p>
                    </div>
                    <div className="grid grid-cols-3 gap-4">
                      <p className="text-sm text-gray-500 col-span-1">Ngày nhận (CP/LC):</p>
                      <p className="text-sm font-medium text-gray-900 col-span-2">{asset.ngayCapPhat ? new Date(asset.ngayCapPhat).toLocaleDateString('vi-VN') : '-'}</p>
                    </div>
                  </div>
                </div>
              </div>

              <div className="space-y-6">
                <div>
                  <h3 className="text-base font-bold text-gray-900 mb-4 border-b pb-2">Thông số Kế toán</h3>
                  <div className="space-y-4">
                    <div className="grid grid-cols-3 gap-4 border-b border-gray-100 pb-3">
                      <p className="text-sm text-gray-500 col-span-1">Nguyên giá:</p>
                      <p className="text-sm font-bold text-gray-900 col-span-2">{formatCurrency(asset.nguyenGia)}</p>
                    </div>
                    <div className="grid grid-cols-3 gap-4 border-b border-gray-100 pb-3">
                      <p className="text-sm text-gray-500 col-span-1">Giá trị còn lại:</p>
                      <p className="text-sm font-bold text-green-600 col-span-2">{formatCurrency(asset.giaTriConLai)}</p>
                    </div>
                    <div className="grid grid-cols-3 gap-4 border-b border-gray-100 pb-3">
                      <p className="text-sm text-gray-500 col-span-1">KH Lũy kế:</p>
                      <p className="text-sm font-medium text-gray-900 col-span-2">{formatCurrency(asset.khauHaoLuyKe)}</p>
                    </div>
                    <div className="grid grid-cols-3 gap-4 border-b border-gray-100 pb-3">
                      <p className="text-sm text-gray-500 col-span-1">Mức KH/tháng:</p>
                      <p className="text-sm font-medium text-gray-900 col-span-2">{formatCurrency(asset.khauHaoHangThang)}</p>
                    </div>
                    <div className="grid grid-cols-3 gap-4 border-b border-gray-100 pb-3">
                      <p className="text-sm text-gray-500 col-span-1">Thời gian KH:</p>
                      <p className="text-sm font-medium text-gray-900 col-span-2">{asset.thoiGianKhauHao ? `${asset.thoiGianKhauHao} tháng` : '-'}</p>
                    </div>
                    <div className="grid grid-cols-3 gap-4 border-b border-gray-100 pb-3">
                      <p className="text-sm text-gray-500 col-span-1">Phương pháp KH:</p>
                      <p className="text-sm font-medium text-gray-900 col-span-2">Khấu hao đường thẳng</p>
                    </div>
                    <div className="grid grid-cols-3 gap-4">
                      <p className="text-sm text-gray-500 col-span-1">Tài khoản hạch toán:</p>
                      <p className="text-sm font-medium text-blue-600 col-span-2">{asset.maTaiKhoan || '-'}</p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          )}

          {/* TAB 2: TIMELINE LỊCH SỬ */}
          {activeTab === 'usage' && (
            <div className="max-w-3xl mx-auto py-4">
              {isLoadingHistory ? (
                <div className="text-center text-gray-500 py-8">Đang tải lịch sử...</div>
              ) : historyTimeline.length === 0 ? (
                <div className="py-8 text-center text-gray-500 bg-white rounded-lg border border-gray-200 border-dashed">
                  <History className="w-12 h-12 mx-auto text-gray-300 mb-3" />
                  <p>Chưa có dữ liệu lịch sử sử dụng, điều chuyển hay bảo trì nào.</p>
                </div>
              ) : (
                <div className="relative border-l-2 border-gray-200 ml-4 space-y-8 pb-4">
                  {historyTimeline.map((item) => {
                    const Icon = item.icon;
                    return (
                      <div key={item.id} className="relative pl-8">
                        <div className={`absolute -left-[17px] top-1 w-8 h-8 rounded-full flex items-center justify-center ring-4 ring-gray-50 shadow-sm ${item.iconColor}`}>
                          <Icon className="w-4 h-4" />
                        </div>
                        
                        <div className="bg-white border border-gray-200 rounded-lg p-4 shadow-sm hover:shadow-md transition-shadow">
                          <div className="flex items-center justify-between mb-2">
                            <h4 className="font-bold text-gray-900">{item.title}</h4>
                            <span className="text-sm font-medium text-gray-500 bg-gray-100 px-2 py-0.5 rounded">
                              {item.date.toLocaleDateString('vi-VN')}
                            </span>
                          </div>
                          
                          <p className="text-sm text-gray-600 mb-4">{item.description}</p>
                          
                          {item.details.length > 0 && (
                            <div className="flex flex-wrap gap-2 pt-3 border-t border-gray-100">
                              {item.details.map((detail: string, idx: number) => (
                                <span key={idx} className="inline-flex items-center px-2.5 py-1 rounded-md text-xs font-medium bg-gray-50 text-gray-700 border border-gray-200">
                                  {detail}
                                </span>
                              ))}
                            </div>
                          )}
                        </div>
                      </div>
                    )
                  })}
                </div>
              )}
            </div>
          )}

       {/* TAB 3: LỊCH SỬ KHẤU HAO */}
          {activeTab === 'depreciation' && (
            <div className="max-w-4xl mx-auto py-2">
              {isLoadingDepreciations ? (
                <div className="text-center text-gray-500 py-8">Đang tải lịch sử khấu hao...</div>
              ) : depreciations.length === 0 ? (
                <div className="py-8 text-center text-gray-500 bg-white rounded-lg border border-gray-200 border-dashed">
                  <LineChart className="w-12 h-12 mx-auto text-gray-300 mb-3" />
                  <p>Chưa có dữ liệu khấu hao nào được ghi nhận cho tài sản này.</p>
                </div>
              ) : (
                <div className="bg-white border border-gray-200 rounded-lg overflow-hidden shadow-sm">
                  <table className="w-full text-left text-sm">
                    <thead className="bg-gray-50 border-b border-gray-200 text-gray-600 font-medium">
                      <tr>
                        <th className="px-6 py-4">Kỳ khấu hao</th>
                        <th className="px-6 py-4 text-right">Số tiền trích (VNĐ)</th>
                        <th className="px-6 py-4">Ngày ghi sổ</th>
                        <th className="px-6 py-4 text-center">Trạng thái</th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-gray-200">
                      {depreciations.map((item, idx) => (
                        <tr key={idx} className="hover:bg-gray-50">
                          <td className="px-6 py-4">
                            <div className="flex items-center gap-3">
                              <div className="w-8 h-8 rounded-lg bg-blue-50 text-blue-600 flex items-center justify-center font-bold text-xs">
                                {item.kyKhauHao ? item.kyKhauHao.split('-')[1] : 'M'}
                              </div>
                              <div>
                                <p className="font-semibold text-gray-900">Tháng {item.kyKhauHao?.split('-')[1] || 'N/A'}</p>
                                <p className="text-xs text-gray-500">Năm {item.kyKhauHao?.split('-')[0] || 'N/A'}</p>
                              </div>
                            </div>
                          </td>
                          <td className="px-6 py-4 text-right font-bold text-blue-600">
                            +{formatCurrency(item.soTien)}
                          </td>
                          <td className="px-6 py-4 text-gray-600">
                            {item.ngayTao ? new Date(item.ngayTao).toLocaleDateString('vi-VN') : '-'}
                          </td>
                          <td className="px-6 py-4 text-center">
                            <span className="inline-flex px-2 py-1 text-[11px] font-medium rounded-full bg-green-100 text-green-700">
                              Đã hạch toán
                            </span>
                          </td>
                        </tr>
                      ))}
                    </tbody>
                    <tfoot className="bg-gray-50 border-t-2 border-gray-200">
                      <tr>
                        <td className="px-6 py-4 font-semibold text-gray-900 text-right">Tổng lũy kế đã trích:</td>
                        <td className="px-6 py-4 text-right font-bold text-gray-900 text-base">
                          {formatCurrency(depreciations.reduce((sum, item) => sum + (item.soTien || 0), 0))}
                        </td>
                        <td colSpan={2}></td>
                      </tr>
                    </tfoot>
                  </table>
                </div>
              )}
            </div>
          )}

          {/* TAB 4: CHỨNG TỪ KẾ TOÁN */}
          {activeTab === 'vouchers' && (
            <div className="max-w-4xl mx-auto py-2">
              {isLoadingVouchers ? (
                <div className="text-center text-gray-500 py-8">Đang tải chứng từ liên quan...</div>
              ) : vouchers.length === 0 ? (
                <div className="py-8 text-center text-gray-500 bg-white rounded-lg border border-gray-200 border-dashed">
                  <FileText className="w-12 h-12 mx-auto text-gray-300 mb-3" />
                  <p>Chưa có chứng từ kế toán nào liên kết với tài sản này.</p>
                </div>
              ) : (
                <div className="bg-white border border-gray-200 rounded-lg overflow-hidden shadow-sm">
                  <div className="overflow-x-auto">
                    <table className="w-full text-left text-sm">
                      <thead className="bg-gray-50 border-b border-gray-200 text-gray-600 font-medium">
                        <tr>
                          <th className="px-4 py-3">Ngày chứng từ</th>
                          <th className="px-4 py-3">Số CT</th>
                          <th className="px-4 py-3">Loại/Diễn giải</th>
                          <th className="px-4 py-3">Hạch toán</th>
                          <th className="px-4 py-3 text-right">Số tiền (VNĐ)</th>
                          <th className="px-4 py-3 text-center">Trạng thái</th>
                        </tr>
                      </thead>
                      <tbody className="divide-y divide-gray-200">
                        {vouchers.map((voucher, idx) => {
                          // KIỂM TRA TRẠNG THÁI: Nếu là 'hoan_thanh' hoặc khác các giá trị nháp
                          const isGhiSo = voucher.trangThai === 'hoan_thanh' || 
                                          (voucher.trangThai !== 'nhap' && 
                                           voucher.trangThai !== 'Nhap' && 
                                           voucher.trangThai !== '0' && 
                                           voucher.trangThai !== 0);

                          const detail = voucher.chiTietChungTus?.[0]; 

                          // MAP LOẠI CHỨNG TỪ THEO SỐ VÀ CHỮ
                          let loaiChungTuLabel = 'Chứng từ khác';
                          const loaiCT = voucher.loaiChungTu?.toString();
                          if (loaiCT === '0' || loaiCT === 'GhiTang') loaiChungTuLabel = 'Ghi tăng TSCĐ';
                          else if (loaiCT === '1' || loaiCT === 'SuaChua') loaiChungTuLabel = 'Sửa chữa/Bảo trì';
                          else if (loaiCT === '2' || loaiCT === 'DieuChuyen') loaiChungTuLabel = 'Điều chuyển';
                          else if (loaiCT === '3' || loaiCT === 'ThanhLy') loaiChungTuLabel = 'Thanh lý';
                          else if (loaiCT === '4' || loaiCT === 'KhauHao') loaiChungTuLabel = 'Khấu hao TSCĐ';

                          return (
                            <tr key={idx} className="hover:bg-gray-50">
                              <td className="px-4 py-4 text-gray-900 font-medium">
                                {voucher.ngayLap ? new Date(voucher.ngayLap).toLocaleDateString('vi-VN') : '-'}
                              </td>
                              <td className="px-4 py-4">
                                <Link to={`/vouchers/${voucher.id}`} className="text-blue-600 font-semibold hover:underline">
                                  {voucher.maChungTu}
                                </Link>
                              </td>
                              <td className="px-4 py-4">
                                <p className="font-medium text-gray-900">{loaiChungTuLabel}</p>
                                <p className="text-xs text-gray-500 mt-0.5 truncate max-w-[200px]" title={detail?.moTa || voucher.moTa}>
                                  {detail?.moTa || voucher.moTa}
                                </p>
                              </td>
                              <td className="px-4 py-4 text-xs font-mono text-gray-600">
                                {detail ? (
                                  <>
                                    {detail.taiKhoanNo && <p className="text-red-600">Nợ: {detail.taiKhoanNo}</p>}
                                    {detail.taiKhoanCo && <p className="text-blue-600">Có: {detail.taiKhoanCo}</p>}
                                  </>
                                ) : '-'}
                              </td>
                              <td className="px-4 py-4 text-right font-bold text-gray-900">
                                {formatCurrency(detail?.soTien || voucher.tongTien)}
                              </td>
                              <td className="px-4 py-4 text-center">
                                <span className={`inline-flex px-2 py-1 text-[11px] font-medium rounded-full ${isGhiSo ? 'bg-green-100 text-green-700' : 'bg-gray-100 text-gray-600'}`}>
                                  {isGhiSo ? 'Đã ghi sổ' : 'Bản nháp'}
                                </span>
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
          )}

        </div>
      </div>

      {/* ========================================================= */}
      {/* ======================= MODALS TẠI CHỖ ================== */}
      {/* ========================================================= */}

      {/* 1. MODAL CẤP PHÁT / ĐIỀU CHUYỂN */}
      {showAllocModal && (
        <div className="fixed inset-0 bg-gray-900/50 backdrop-blur-sm flex items-center justify-center z-[9999] p-4">
          <div className="bg-white rounded-lg max-w-xl w-full overflow-hidden shadow-2xl flex flex-col">
            <div className="p-6 border-b border-gray-200 flex items-center justify-between bg-white">
              <h3 className="font-bold text-gray-900 text-lg flex items-center gap-2">
                {allocType === 'CapPhat' ? <Send className="w-5 h-5 text-green-600" /> : <ArrowLeftRight className="w-5 h-5 text-blue-600" />}
                {allocType === 'CapPhat' ? 'Cấp phát Tài sản' : 'Điều chuyển Tài sản'}
              </h3>
              <button onClick={() => setShowAllocModal(false)} className="text-gray-400 hover:text-gray-600">
                <X className="w-6 h-6" />
              </button>
            </div>
            
            <form onSubmit={handleAllocSubmit} className="p-6 space-y-5 bg-gray-50/30">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1.5">Tài sản đang chọn</label>
                <div className="w-full px-4 py-3 border border-gray-200 rounded-lg bg-gray-100 flex items-center gap-3">
                  <Package className="w-5 h-5 text-gray-400" />
                  <span className="font-medium text-gray-700">{asset.maTaiSan} - {asset.tenTaiSan}</span>
                </div>
              </div>

              {allocType === 'LuanChuyen' && (
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">Từ phòng ban</label>
                  <input
                    type="text"
                    disabled
                    value={getDeptName(allocFormData.tuPhongBanId)}
                    className="w-full px-4 py-2 border border-gray-200 rounded-lg bg-gray-100 text-gray-600 cursor-not-allowed"
                  />
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
                    onChange={(e) => setAllocFormData({...allocFormData, denPhongBanId: Number(e.target.value)})}
                    className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 bg-white"
                  >
                    <option value="">-- Chọn phòng ban --</option>
                    {departments.map(dept => (
                      <option key={dept.id} value={dept.id}>{dept.tenPhongBan}</option>
                    ))}
                  </select>
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">Mã nhân viên nhận</label>
                  <input
                    type="number"
                    value={allocFormData.denNguoiDungId || ''}
                    onChange={(e) => setAllocFormData({...allocFormData, denNguoiDungId: Number(e.target.value)})}
                    className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                    placeholder="VD: 102"
                  />
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">Ngày thực hiện <span className="text-red-500">*</span></label>
                  <input
                    type="date"
                    required
                    value={allocFormData.ngayThucHien}
                    onChange={(e) => setAllocFormData({...allocFormData, ngayThucHien: e.target.value})}
                    className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                  />
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1.5">Ghi chú thêm</label>
                <textarea
                  rows={2}
                  value={allocFormData.ghiChu || ''}
                  onChange={(e) => setAllocFormData({...allocFormData, ghiChu: e.target.value})}
                  className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                  placeholder="Lý do cấp phát, tình trạng máy lúc giao..."
                />
              </div>

              <div className="pt-2 flex justify-end gap-3">
                <button type="button" onClick={() => setShowAllocModal(false)} className="px-5 py-2.5 border border-gray-300 rounded-lg hover:bg-gray-100 font-medium text-gray-700">
                  Hủy bỏ
                </button>
                <button type="submit" disabled={isSubmitting} className="flex items-center gap-2 px-6 py-2.5 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 font-medium shadow-sm">
                  <Save className="w-4 h-4" /> {isSubmitting ? 'Đang xử lý...' : 'Lưu lại'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* 2. MODAL BẢO TRÌ */}
      {showMaintModal && (
        <div className="fixed inset-0 bg-gray-900/50 backdrop-blur-sm flex items-center justify-center z-[9999] p-4">
          <div className="bg-white rounded-lg max-w-2xl w-full overflow-hidden shadow-2xl flex flex-col">
            <div className="p-6 border-b border-gray-200 flex items-center justify-between bg-white">
              <h3 className="font-bold text-gray-900 text-lg flex items-center gap-2">
                <Wrench className="w-5 h-5 text-teal-600" />
                Ghi nhận Bảo trì / Sửa chữa
              </h3>
              <button onClick={() => setShowMaintModal(false)} className="text-gray-400 hover:text-gray-600">
                <X className="w-6 h-6" />
              </button>
            </div>
            
            <form onSubmit={handleMaintSubmit} className="p-6 space-y-5 bg-gray-50/30">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div className="md:col-span-2">
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">Tài sản đang chọn</label>
                  <div className="w-full px-4 py-3 border border-gray-200 rounded-lg bg-gray-100 flex items-center gap-3">
                    <Package className="w-5 h-5 text-gray-400" />
                    <span className="font-medium text-gray-700">{asset.maTaiSan} - {asset.tenTaiSan}</span>
                  </div>
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">Ngày bảo trì <span className="text-red-500">*</span></label>
                  <input
                    type="date"
                    required
                    value={maintFormData.ngayThucHien}
                    onChange={(e) => setMaintFormData({...maintFormData, ngayThucHien: e.target.value})}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                  />
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">Loại bảo trì <span className="text-red-500">*</span></label>
                  <select
                    required
                    value={maintFormData.loaiBaoTri ?? '0'}
                    onChange={(e) => setMaintFormData({...maintFormData, loaiBaoTri: e.target.value})}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 bg-white"
                  >
                    <option value="0">Bảo trì định kỳ</option>
                    <option value="1">Sửa chữa</option>
                    <option value="2">Nâng cấp</option>
                    <option value="3">Vệ sinh</option>
                    <option value="4">Kiểm tra</option>
                  </select>
                </div>
              </div>

              <div className="bg-white rounded-lg border border-gray-200 p-4">
                <label className="flex items-center gap-2 cursor-pointer mb-3">
                  <input
                    type="checkbox"
                    checked={maintHasCost}
                    onChange={(e) => setMaintHasCost(e.target.checked)}
                    className="rounded border-gray-300 text-blue-600 focus:ring-blue-500"
                  />
                  <span className="text-sm font-medium text-gray-700">Có phát sinh chi phí</span>
                </label>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  {maintHasCost && (
                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-1.5">Tổng chi phí (VNĐ) <span className="text-red-500">*</span></label>
                      <input
                        type="number"
                        required={maintHasCost}
                        min="0"
                        value={maintFormData.chiPhi || ''}
                        onChange={(e) => setMaintFormData({...maintFormData, chiPhi: Number(e.target.value)})}
                        className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                        placeholder="0"
                      />
                    </div>
                  )}

                  <div className={!maintHasCost ? "md:col-span-2" : ""}>
                    <label className="block text-sm font-medium text-gray-700 mb-1.5">Nhà cung cấp / Đơn vị sửa chữa</label>
                    <input
                      type="text"
                      value={maintFormData.nhaCungCap || ''}
                      onChange={(e) => setMaintFormData({...maintFormData, nhaCungCap: e.target.value})}
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                      placeholder="VD: Dell Vietnam, Thợ ngoài..."
                    />
                  </div>
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1.5">Ghi chú tình trạng</label>
                <textarea
                  rows={2}
                  value={maintFormData.ghiChu || ''}
                  onChange={(e) => setMaintFormData({...maintFormData, ghiChu: e.target.value})}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                  placeholder="Ghi chú tình trạng máy, linh kiện thay thế..."
                />
              </div>

              <div className="pt-2 flex justify-end gap-3">
                <button type="button" onClick={() => setShowMaintModal(false)} className="px-5 py-2.5 border border-gray-300 rounded-lg hover:bg-gray-100 font-medium text-gray-700">
                  Hủy bỏ
                </button>
                <button type="submit" disabled={isSubmitting} className="flex items-center gap-2 px-6 py-2.5 bg-teal-600 text-white rounded-lg hover:bg-teal-700 disabled:opacity-50 font-medium shadow-sm">
                  <Save className="w-4 h-4" /> {isSubmitting ? 'Đang lưu...' : 'Lưu thông tin'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* 3. MODAL THANH LÝ */}
      {showLiqModal && (
        <div className="fixed inset-0 bg-gray-900/50 backdrop-blur-sm flex items-center justify-center z-[9999] p-4">
          <div className="bg-white rounded-lg max-w-2xl w-full max-h-[90vh] overflow-y-auto shadow-2xl flex flex-col">
            <div className="p-6 border-b border-gray-200 flex items-center justify-between bg-white sticky top-0">
              <h3 className="font-bold text-gray-900 text-lg flex items-center gap-2">
                <Trash2 className="w-5 h-5 text-red-600" />
                Tạo Phiếu thanh lý
              </h3>
              <button onClick={() => setShowLiqModal(false)} className="text-gray-400 hover:text-gray-600">
                <X className="w-6 h-6" />
              </button>
            </div>
            
            <form onSubmit={handleLiqSubmit} className="p-6 space-y-5 bg-gray-50/30">
              <div className="bg-red-50 border border-red-200 rounded-lg p-4 flex items-start gap-3">
                <AlertCircle className="w-5 h-5 text-red-600 mt-0.5 shrink-0" />
                <div className="flex-1 text-sm text-red-700">
                  <p className="font-medium text-red-900 mb-1">Lưu ý quan trọng</p>
                  <ul className="list-disc list-inside space-y-0.5">
                    <li>Hệ thống sẽ tự động tính toán lãi/lỗ thanh lý.</li>
                    <li>Trạng thái tài sản sẽ tự động chuyển sang "Đã thanh lý".</li>
                  </ul>
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1.5">Tài sản đang chọn</label>
                <div className="w-full px-4 py-3 border border-gray-200 rounded-lg bg-gray-100 flex items-center gap-3">
                  <Package className="w-5 h-5 text-gray-400" />
                  <span className="font-medium text-gray-700">{asset.maTaiSan} - {asset.tenTaiSan}</span>
                </div>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">Ngày thanh lý <span className="text-red-500">*</span></label>
                  <input
                    type="date"
                    required
                    value={liqFormData.ngayThanhLy}
                    onChange={(e) => setLiqFormData({...liqFormData, ngayThanhLy: e.target.value})}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                  />
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">Giá thanh lý thu về (VNĐ) <span className="text-red-500">*</span></label>
                  <input
                    type="number"
                    required
                    min="0"
                    value={liqFormData.giaTriThanhLy === 0 ? '' : liqFormData.giaTriThanhLy}
                    onChange={(e) => setLiqFormData({...liqFormData, giaTriThanhLy: Number(e.target.value)})}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                    placeholder="0"
                  />
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">Lý do thanh lý <span className="text-red-500">*</span></label>
                  <select
                    required
                    value={liqFormData.lyDo}
                    onChange={(e) => setLiqFormData({...liqFormData, lyDo: e.target.value})}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 bg-white"
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
                  <label className="block text-sm font-medium text-gray-700 mb-1.5">Trạng thái phiếu <span className="text-red-500">*</span></label>
                  <select
                    required
                    value={liqFormData.trangThai ?? 'ChoDuyet'}
                    onChange={(e) => setLiqFormData({...liqFormData, trangThai: e.target.value})} 
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 bg-white"
                  >
                    <option value="ChoDuyet">Chờ duyệt</option>
                    <option value="DaDuyet">Đã duyệt</option> 
                    <option value="DaHoanThanh">Hoàn thành (Ghi sổ)</option>
                  </select>
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1.5">Ghi chú thêm</label>
                <textarea
                  rows={2}
                  value={liqFormData.ghiChu || ''}
                  onChange={(e) => setLiqFormData({...liqFormData, ghiChu: e.target.value})}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                  placeholder="Các thông tin khác..."
                />
              </div>

              {/* Tính toán Lãi / Lỗ */}
              <div className="bg-gradient-to-r from-blue-50 to-purple-50 rounded-lg border border-blue-200 p-4">
                <div className="flex items-center gap-2 mb-3">
                  <Calculator className="w-5 h-5 text-blue-600" />
                  <h4 className="font-semibold text-gray-900 text-sm">Tính toán Lãi/Lỗ thanh lý (Dự kiến)</h4>
                </div>
                <div className="grid grid-cols-1 md:grid-cols-3 gap-3">
                  <div className="bg-white rounded-md p-3 border border-gray-100">
                    <p className="text-xs text-gray-500 mb-1">Giá trị còn lại</p>
                    <p className="font-bold text-gray-900 text-sm">{formatCurrency(remainingValue)}</p>
                  </div>
                  <div className="bg-white rounded-md p-3 border border-gray-100">
                    <p className="text-xs text-gray-500 mb-1">Giá thanh lý</p>
                    <p className="font-bold text-gray-900 text-sm">{formatCurrency(liquidationValue)}</p>
                  </div>
                  <div className={`bg-white rounded-md p-3 border ${profitLoss >= 0 ? 'border-green-400' : 'border-red-400'}`}>
                    <p className="text-xs text-gray-500 mb-1">{profitLoss >= 0 ? 'Lãi thanh lý' : 'Lỗ thanh lý'}</p>
                    <p className={`font-bold text-sm ${profitLoss >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                      {formatCurrency(Math.abs(profitLoss))}
                    </p>
                  </div>
                </div>
              </div>

              <div className="pt-2 flex justify-end gap-3 sticky bottom-0">
                <button type="button" onClick={() => setShowLiqModal(false)} className="px-5 py-2.5 border border-gray-300 rounded-lg hover:bg-gray-100 font-medium text-gray-700">
                  Hủy bỏ
                </button>
                <button type="submit" disabled={isSubmitting} className="flex items-center gap-2 px-6 py-2.5 bg-red-600 text-white rounded-lg hover:bg-red-700 disabled:opacity-50 font-medium shadow-sm">
                  <Save className="w-4 h-4" /> {isSubmitting ? 'Đang xử lý...' : 'Lưu & Thanh lý'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

    </div>
  );
}