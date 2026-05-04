import { useState, useEffect, useMemo } from 'react';
import { Link } from 'react-router';
import { Search, FileText, Eye, Lock, Unlock, Download, RefreshCw } from 'lucide-react';
import { toast } from 'sonner';
import { voucherApi, ChungTu } from '../../api/voucherApi';
import * as XLSX from 'xlsx'; 

// ==========================================
// 1. CHUẨN HÓA DỮ LIỆU ĐẦU VÀO TỪ BACKEND
// ==========================================
const getStandardType = (type: any): string => {
  const t = type?.toString();
  if (t === '0' || t === 'GhiTang') return '0';
  if (t === '1' || t === '4' || t === 'KhauHao') return '1';
  if (t === '2' || t === 'BaoTri' || t === 'SuaChua') return '2';
  if (t === '3' || t === 'ThanhLy') return '3';
  if (t === 'DieuChuyen') return '4';
  return 'other';
};

const getStandardStatus = (status: any): string => {
  const s = status?.toString().toLowerCase();
  if (s === '0' || s === 'nhap') return 'nhap';
  if (s === '1' || s === 'hoan_thanh' || s === 'daghiso') return 'hoan_thanh';
  if (s === '2' || s === 'da_khoa') return 'da_khoa';
  if (s && s !== 'nhap' && s !== '0') return 'hoan_thanh'; 
  return 'nhap'; 
};

// Cấu hình UI
const typeConfig: Record<string, { label: string; color: string }> = {
  '0': { label: 'Ghi tăng', color: 'bg-green-100 text-green-700 border-green-200' },
  '1': { label: 'Khấu hao', color: 'bg-orange-100 text-orange-700 border-orange-200' },
  // '2': { label: 'Bảo trì', color: 'bg-blue-100 text-blue-700 border-blue-200' },
  '3': { label: 'Thanh lý', color: 'bg-red-100 text-red-700 border-red-200' },
  // '4': { label: 'Điều chuyển', color: 'bg-purple-100 text-purple-700 border-purple-200' },
  'other': { label: 'Khác', color: 'bg-gray-100 text-gray-700 border-gray-200' }
};

const statusConfig: Record<string, { label: string; color: string; icon: any }> = {
  'nhap': { label: 'Bản nháp', color: 'bg-gray-100 text-gray-600', icon: FileText },
  'hoan_thanh': { label: 'Đã ghi sổ', color: 'bg-green-100 text-green-700', icon: Lock },
  'da_khoa': { label: 'Đã khóa', color: 'bg-red-100 text-red-700', icon: Lock },
};

// ==========================================
// 2. BIẾN CACHE CỤC BỘ TRÊN RAM (Chỉ lưu Chứng từ)
// ==========================================
let cachedVouchers: ChungTu[] | null = null;

export function VoucherList() {
  const [searchTerm, setSearchTerm] = useState('');
  const [filterType, setFilterType] = useState<string>('all');
  const [filterStatus, setFilterStatus] = useState<string>('all');
  
  // State quản lý Chứng từ
  const [vouchers, setVouchers] = useState<ChungTu[]>(cachedVouchers || []);
  const [isLoading, setIsLoading] = useState(!cachedVouchers);

  // ==========================================
  // 3. HÀM TẢI CHỨNG TỪ CÓ TÍCH HỢP CACHE
  // ==========================================
  const fetchVouchers = async (forceRefresh = false) => {
    if (!forceRefresh && cachedVouchers) {
      setVouchers(cachedVouchers);
      return;
    }

    setIsLoading(true);
    try {
      const response = await voucherApi.getAll();
      if (response.errorCode === 200) {
        cachedVouchers = response.data;
        setVouchers(response.data);
      }
    } catch (error) {
      toast.error('Lỗi kết nối đến máy chủ.');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => { 
    fetchVouchers(); 
  }, []);

  const handlePostVoucher = async (id: number) => {
    if (window.confirm('Bạn có chắc chắn muốn ghi sổ chứng từ này?')) {
      try {
        const response = await voucherApi.postVoucher(id);
        if (response.errorCode === 200) {
          toast.success('Ghi sổ thành công!');
          fetchVouchers(true); // Ép làm mới để cập nhật trạng thái mới nhất
        }
      } catch (error) {
        toast.error('Lỗi kết nối đến máy chủ.');
      }
    }
  };

  // ==========================================
  // 4. USE_MEMO BỘ LỌC ĐỂ RENDER MƯỢT MÀ
  // ==========================================
  const filteredVouchers = useMemo(() => {
    return vouchers.filter(voucher => {
      const searchStr = `${voucher.maChungTu} ${voucher.moTa}`.toLowerCase();
      const matchesSearch = searchStr.includes(searchTerm.toLowerCase());
      
      const standardType = getStandardType(voucher.loaiChungTu);
      const standardStatus = getStandardStatus(voucher.trangThai);

      const matchesType = filterType === 'all' || standardType === filterType;
      const matchesStatus = filterStatus === 'all' || standardStatus === filterStatus;
      
      return matchesSearch && matchesType && matchesStatus;
    });
  }, [vouchers, searchTerm, filterType, filterStatus]);

  // ==========================================
  // 5. USE_MEMO CHO THỐNG KÊ (Đếm số lượng tức thì)
  // ==========================================
  const stats = useMemo(() => {
    let draft = 0, posted = 0, locked = 0;
    vouchers.forEach(v => {
      const s = getStandardStatus(v.trangThai);
      if (s === 'nhap') draft++;
      else if (s === 'hoan_thanh') posted++;
      else if (s === 'da_khoa') locked++;
    });
    return { draft, posted, locked, total: vouchers.length };
  }, [vouchers]);

  const handleExportExcel = () => {
    if (filteredVouchers.length === 0) {
      toast.error("Không có dữ liệu để xuất!");
      return;
    }
    try {
      const dataToExport = filteredVouchers.map((v) => {
        const stType = getStandardType(v.loaiChungTu);
        const stStatus = getStandardStatus(v.trangThai);
        
        return {
          "Số chứng từ": v.maChungTu,
          "Ngày lập": v.ngayLap ? new Date(v.ngayLap).toLocaleDateString('vi-VN') : 'N/A',
          "Loại chứng từ": typeConfig[stType].label,
          "Diễn giải": v.moTa,
          "Số tiền (VNĐ)": v.tongTien,
          "Trạng thái": statusConfig[stStatus].label,
          "Ngày tạo hệ thống": v.ngayTao ? new Date(v.ngayTao).toLocaleString('vi-VN') : ''
        };
      });

      const worksheet = XLSX.utils.json_to_sheet(dataToExport);
      const workbook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(workbook, worksheet, "Danh sách Chứng từ");

      const wscols = [{ wch: 25 }, { wch: 15 }, { wch: 20 }, { wch: 45 }, { wch: 15 }, { wch: 15 }, { wch: 20 }];
      worksheet['!cols'] = wscols;

      XLSX.writeFile(workbook, `Danh_Sach_Chung_Tu_${new Date().getTime()}.xlsx`);
      toast.success("Xuất file Excel thành công!");
    } catch (error) {
      toast.error("Có lỗi xảy ra khi xuất file Excel.");
    }
  };

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900 text-xl">Chứng từ Kế toán</h1>
          <p className="text-sm text-gray-500 mt-1">Quản lý và ghi sổ chứng từ tài sản cố định</p>
        </div>
        <div className="flex gap-2">
          {/* Nút Làm mới Data */}
          <button 
            onClick={() => fetchVouchers(true)}
            disabled={isLoading}
            className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-gray-600 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors disabled:opacity-50 shadow-sm"
            title="Tải lại dữ liệu"
          >
            <RefreshCw className={`w-4 h-4 ${isLoading ? 'animate-spin text-blue-600' : ''}`} />
            <span className="hidden sm:block">Làm mới</span>
          </button>

          <button onClick={handleExportExcel} className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors shadow-sm bg-white font-medium text-gray-700">
            <Download className="w-4 h-4" /> Xuất Excel
          </button>
        </div>
      </div>

      {/* Stats Section */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        <div className="bg-white rounded-lg border border-gray-200 p-4 shadow-sm">
          <p className="text-sm font-medium text-gray-500 mb-1">Tổng chứng từ</p>
          <p className="text-2xl font-bold text-gray-900">{stats.total}</p>
        </div>
        <div className="bg-white rounded-lg border border-gray-200 p-4 shadow-sm border-l-4 border-l-gray-400">
          <p className="text-sm font-medium text-gray-500 mb-1">Bản nháp</p>
          <p className="text-2xl font-bold text-gray-700">{stats.draft}</p>
        </div>
        <div className="bg-white rounded-lg border border-gray-200 p-4 shadow-sm border-l-4 border-l-green-500">
          <p className="text-sm font-medium text-gray-500 mb-1">Đã ghi sổ</p>
          <p className="text-2xl font-bold text-green-600">{stats.posted}</p>
        </div>
        <div className="bg-white rounded-lg border border-gray-200 p-4 shadow-sm border-l-4 border-l-red-500">
          <p className="text-sm font-medium text-gray-500 mb-1">Đã khóa</p>
          <p className="text-2xl font-bold text-red-600">{stats.locked}</p>
        </div>
      </div>

      {/* Filters */}
      <div className="bg-white rounded-lg border border-gray-200 p-4 shadow-sm">
        <div className="flex flex-col lg:flex-row gap-4">
          <div className="flex-1 relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" />
            <input
              type="text"
              placeholder="Tìm kiếm theo số chứng từ hoặc diễn giải..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
            />
          </div>
          <div className="flex gap-2">
            <select value={filterType} onChange={(e) => setFilterType(e.target.value)} className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 font-medium text-gray-700">
              <option value="all">Tất cả loại nghiệp vụ</option>
              <option value="0">Ghi tăng TSCĐ</option>
              <option value="1">Khấu hao TSCĐ</option>
              {/* <option value="2">Bảo trì TSCĐ</option> */}
              <option value="3">Thanh lý TSCĐ</option>
              {/* <option value="4">Điều chuyển</option> */}
            </select>
            <select value={filterStatus} onChange={(e) => setFilterStatus(e.target.value)} className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 font-medium text-gray-700">
              <option value="all">Tất cả trạng thái</option>
              <option value="nhap">Bản nháp</option>
              <option value="hoan_thanh">Đã ghi sổ</option>
              <option value="da_khoa">Đã khóa</option>
            </select>
          </div>
        </div>
      </div>

      {/* Table */}
      <div className="bg-white rounded-lg border border-gray-200 overflow-hidden shadow-sm">
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50 border-b border-gray-200">
              <tr>
                <th className="px-6 py-3.5 text-left text-xs font-semibold text-gray-500 uppercase tracking-wider">Số chứng từ</th>
                <th className="px-6 py-3.5 text-left text-xs font-semibold text-gray-500 uppercase tracking-wider">Ngày lập</th>
                <th className="px-6 py-3.5 text-left text-xs font-semibold text-gray-500 uppercase tracking-wider">Nghiệp vụ</th>
                <th className="px-6 py-3.5 text-left text-xs font-semibold text-gray-500 uppercase tracking-wider">Diễn giải</th>
                <th className="px-6 py-3.5 text-right text-xs font-semibold text-gray-500 uppercase tracking-wider">Số tiền (VNĐ)</th>
                <th className="px-6 py-3.5 text-center text-xs font-semibold text-gray-500 uppercase tracking-wider">Trạng thái</th>
                <th className="px-6 py-3.5 text-center text-xs font-semibold text-gray-500 uppercase tracking-wider">Thao tác</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-100">
              {isLoading && filteredVouchers.length === 0 ? (
                <tr><td colSpan={7} className="text-center py-12 text-gray-500">Đang tải dữ liệu...</td></tr>
              ) : filteredVouchers.length === 0 ? (
                <tr><td colSpan={7} className="text-center py-12 text-gray-500">Không tìm thấy chứng từ phù hợp.</td></tr>
              ) : (
                filteredVouchers.map((voucher) => {
                  const stType = getStandardType(voucher.loaiChungTu);
                  const stStatus = getStandardStatus(voucher.trangThai);
                  
                  const typeInfo = typeConfig[stType];
                  const statusInfo = statusConfig[stStatus];
                  const StatusIcon = statusInfo.icon;

                  return (
                    <tr key={voucher.id} className="hover:bg-blue-50/50 transition-colors">
                      <td className="px-6 py-4 whitespace-nowrap">
                        <Link to={`/vouchers/${voucher.id}`} className="text-sm font-bold text-blue-600 hover:text-blue-800 hover:underline">
                          {voucher.maChungTu}
                        </Link>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-700">
                        {voucher.ngayLap ? new Date(voucher.ngayLap).toLocaleDateString('vi-VN') : '-'}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap">
                        <span className={`inline-flex px-2.5 py-1 text-[11px] font-bold uppercase tracking-wide rounded border ${typeInfo.color}`}>
                          {typeInfo.label}
                        </span>
                      </td>
                      <td className="px-6 py-4 text-sm text-gray-700 max-w-xs truncate" title={voucher.moTa}>
                        {voucher.moTa || '-'}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-bold text-gray-900">
                        {formatCurrency(voucher.tongTien)}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-center">
                        <span className={`inline-flex items-center gap-1.5 px-2.5 py-1 text-xs font-semibold rounded-full border border-transparent ${stStatus === 'nhap' ? 'bg-gray-100 text-gray-600 border-gray-200' : statusInfo.color}`}>
                          <StatusIcon className="w-3.5 h-3.5" />
                          {statusInfo.label}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-center">
                        <div className="flex items-center justify-center gap-2">
                          <Link to={`/vouchers/${voucher.id}`} className="p-1.5 text-blue-600 hover:bg-blue-100 rounded-md transition-colors" title="Xem chi tiết">
                            <Eye className="w-4 h-4" />
                          </Link>
                          {stStatus === 'nhap' ? (
                            <button
                              onClick={() => voucher.id && handlePostVoucher(voucher.id)}
                              className="p-1.5 text-green-600 hover:bg-green-100 rounded-md transition-colors"
                              title="Ghi sổ kế toán"
                            >
                              <Unlock className="w-4 h-4" />
                            </button>
                          ) : (
                            <button disabled className="p-1.5 text-gray-300 cursor-not-allowed rounded-md" title="Đã ghi sổ">
                              <Lock className="w-4 h-4" />
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