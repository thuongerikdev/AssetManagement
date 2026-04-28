import { useState, useEffect, useMemo } from 'react';
import { Link } from 'react-router';
import { Search, Download, Plus, Calendar, ChevronDown, ChevronRight, FileText, Info, BookText, Calculator, RefreshCw } from 'lucide-react';
import { toast } from 'sonner';
import { voucherApi, ChungTu } from '../../api/voucherApi';
import * as XLSX from 'xlsx';

// ==========================================
// 1. HÀM CHUẨN HÓA VÀ CONFIG
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

const typeConfig: Record<string, { label: string; color: string }> = {
  '0': { label: 'Ghi tăng', color: 'bg-green-100 text-green-700 border-green-200' },
  '1': { label: 'Khấu hao', color: 'bg-orange-100 text-orange-700 border-orange-200' },
  '2': { label: 'Bảo trì', color: 'bg-blue-100 text-blue-700 border-blue-200' },
  '3': { label: 'Thanh lý', color: 'bg-red-100 text-red-700 border-red-200' },
  '4': { label: 'Điều chuyển', color: 'bg-purple-100 text-purple-700 border-purple-200' },
  'other': { label: 'Khác', color: 'bg-gray-100 text-gray-700 border-gray-200' }
};

interface JournalEntry {
  id: string;
  chungTuId: number;
  maChungTu: string;
  ngayLap: string;
  moTa: string;
  taiKhoanNo: string | null;
  taiKhoanCo: string | null;
  soTien: number;
  loaiChungTu: number;
}

// ==========================================
// 2. BIẾN CACHE CỤC BỘ TRÊN RAM
// ==========================================
let cachedJournalVouchers: ChungTu[] | null = null;

export function GeneralJournal() {
  // State lưu dữ liệu gốc
  const [vouchers, setVouchers] = useState<ChungTu[]>(cachedJournalVouchers || []);
  const [isLoading, setIsLoading] = useState(!cachedJournalVouchers);
  
  // Filters state
  const [searchTerm, setSearchTerm] = useState('');
  // Mặc định lấy từ đầu tháng hiện tại đến cuối tháng hiện tại
  const [fromDate, setFromDate] = useState(() => {
    const d = new Date();
    return new Date(d.getFullYear(), d.getMonth(), 1).toISOString().split('T')[0];
  });
  const [toDate, setToDate] = useState(() => {
    const d = new Date();
    return new Date(d.getFullYear(), d.getMonth() + 1, 0).toISOString().split('T')[0];
  });
  const [typeFilter, setTypeFilter] = useState('all');

  // ==========================================
  // 3. HÀM TẢI DỮ LIỆU CÓ CACHE
  // ==========================================
  const fetchJournalEntries = async (forceRefresh = false) => {
    if (!forceRefresh && cachedJournalVouchers) {
      setVouchers(cachedJournalVouchers);
      return;
    }

    setIsLoading(true);
    try {
      const response = await voucherApi.getAll();
      if (response.errorCode === 200) {
        cachedJournalVouchers = response.data;
        setVouchers(response.data);
      }
    } catch (error) {
      toast.error('Lỗi khi tải dữ liệu sổ cái.');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchJournalEntries();
  }, []);

  // ==========================================
  // 4. USE_MEMO CHUYỂN ĐỔI DỮ LIỆU (Trích xuất bút toán)
  // ==========================================
  const entries = useMemo(() => {
    const flattenedEntries: JournalEntry[] = [];

    // Chỉ lấy các chứng từ đã ghi sổ/hoàn thành
    const postedVouchers = vouchers.filter(v => 
      v.trangThai === 'hoan_thanh' || v.trangThai === 'da_khoa' || v.trangThai === '1' || v.trangThai === '2'
    );

    postedVouchers.forEach(voucher => {
      if (voucher.chiTietChungTus && voucher.chiTietChungTus.length > 0) {
        voucher.chiTietChungTus.forEach(detail => {
          flattenedEntries.push({
            id: `${voucher.id}-${detail.id}`,
            chungTuId: voucher.id!,
            maChungTu: voucher.maChungTu,
            ngayLap: voucher.ngayLap || voucher.ngayTao || '',
            moTa: detail.moTa || voucher.moTa || 'Không có diễn giải',
            taiKhoanNo: detail.taiKhoanNo || null,
            taiKhoanCo: detail.taiKhoanCo || null,
            soTien: detail.soTien || 0,
            loaiChungTu: voucher.loaiChungTu
          });
        });
      }
    });

    return flattenedEntries.sort((a, b) => new Date(a.ngayLap).getTime() - new Date(b.ngayLap).getTime());
  }, [vouchers]);

  // ==========================================
  // 5. USE_MEMO LỌC DỮ LIỆU TỨC THÌ
  // ==========================================
  const filteredEntries = useMemo(() => {
    return entries.filter(entry => {
      const searchStr = `${entry.maChungTu} ${entry.moTa}`.toLowerCase();
      const matchesSearch = searchStr.includes(searchTerm.toLowerCase());
      
      const standardType = getStandardType(entry.loaiChungTu);
      const matchesType = typeFilter === 'all' || standardType === typeFilter;

      // Lọc theo ngày (Date filtering)
      const entryDate = new Date(entry.ngayLap);
      const from = fromDate ? new Date(fromDate) : new Date('2000-01-01');
      const to = toDate ? new Date(toDate) : new Date('2100-01-01');
      to.setHours(23, 59, 59, 999); // Tính đến cuối ngày
      const matchesDate = entryDate >= from && entryDate <= to;

      return matchesSearch && matchesType && matchesDate;
    });
  }, [entries, searchTerm, typeFilter, fromDate, toDate]);

  // ==========================================
  // 6. USE_MEMO THỐNG KÊ TỨC THÌ
  // ==========================================
  const { totalAmount, totalVouchers } = useMemo(() => {
    const amount = filteredEntries.reduce((sum, item) => sum + item.soTien, 0);
    const uniqueVouchers = new Set(filteredEntries.map(e => e.maChungTu));
    return { totalAmount: amount, totalVouchers: uniqueVouchers.size };
  }, [filteredEntries]);

  const handleExportExcel = () => {
    if (filteredEntries.length === 0) {
      toast.error('Không có dữ liệu để xuất');
      return;
    }
    
    const exportData = filteredEntries.map(entry => ({
      'Ngày hạch toán': entry.ngayLap ? new Date(entry.ngayLap).toLocaleDateString('vi-VN') : '',
      'Số chứng từ': entry.maChungTu,
      'Diễn giải': entry.moTa,
      'TK Nợ': entry.taiKhoanNo || '',
      'TK Có': entry.taiKhoanCo || '',
      'Số phát sinh (VNĐ)': entry.soTien
    }));

    const worksheet = XLSX.utils.json_to_sheet(exportData);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, "Nhật ký chung");
    
    const wscols = [{ wch: 15 }, { wch: 20 }, { wch: 45 }, { wch: 10 }, { wch: 10 }, { wch: 20 }];
    worksheet['!cols'] = wscols;

    XLSX.writeFile(workbook, `Nhat_Ky_Chung_${new Date().getTime()}.xlsx`);
    toast.success('Xuất file thành công!');
  };

  const formatCurrency = (val: number) => new Intl.NumberFormat('vi-VN').format(val);
  const formatDateVN = (dateString: string) => {
    if (!dateString) return '...';
    const [y, m, d] = dateString.split('-');
    return `${d}/${m}/${y}`;
  };

  const getTypeBadge = (type: any) => {
    const stType = getStandardType(type);
    const typeInfo = typeConfig[stType];
    return (
      <span className={`${typeInfo.color} px-2 py-0.5 rounded text-xs font-medium border`}>
        {typeInfo.label}
      </span>
    );
  };

  const getAccountBadge = (acc: string | null) => {
    if (!acc) return null;
    if (acc.startsWith('211')) return <span className="bg-green-50 text-green-600 px-2 py-0.5 rounded text-xs font-semibold border border-green-100">{acc}</span>;
    if (acc.startsWith('111') || acc.startsWith('3')) return <span className="bg-red-50 text-red-600 px-2 py-0.5 rounded text-xs font-semibold border border-red-100">{acc}</span>;
    return <span className="bg-gray-50 text-gray-600 px-2 py-0.5 rounded text-xs font-semibold border border-gray-200">{acc}</span>;
  };

  return (
    <div className="space-y-4 bg-gray-50 min-h-screen pb-10">
      {/* Header */}
      <div className="flex items-center justify-between bg-white px-6 py-4 border-b border-gray-200">
        <div>
          <h1 className="font-bold text-gray-900 text-xl">Sổ Nhật ký chung</h1>
          <p className="text-sm text-gray-500 mt-0.5">Ghi chép toàn bộ nghiệp vụ kinh tế phát sinh theo trình tự thời gian</p>
        </div>
        <div className="flex items-center gap-3">
          {/* Nút Làm mới */}
          <button 
            onClick={() => fetchJournalEntries(true)}
            disabled={isLoading}
            className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-md hover:bg-gray-50 bg-white font-medium text-sm text-gray-700 transition-colors disabled:opacity-50"
            title="Tải lại dữ liệu"
          >
            <RefreshCw className={`w-4 h-4 ${isLoading ? 'animate-spin text-blue-600' : ''}`} />
            <span className="hidden sm:block">Làm mới</span>
          </button>

          <button 
            onClick={handleExportExcel}
            className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-md hover:bg-gray-50 bg-white font-medium text-sm text-gray-700 transition-colors shadow-sm"
          >
            <Download className="w-4 h-4" /> Xuất Excel
          </button>
          <button className="flex items-center gap-2 px-4 py-2 bg-blue-600 hover:bg-blue-700 rounded-md font-medium text-sm text-white transition-colors shadow-sm">
            <Plus className="w-4 h-4" /> Tạo bút toán
          </button>
        </div>
      </div>

      <div className="px-6 space-y-4">
        {/* Summary Cards */}
        <div className="grid grid-cols-4 gap-4">
          <div className="bg-white p-4 rounded-lg border border-gray-200 shadow-sm flex flex-col justify-center">
            <div className="flex justify-between items-start">
              <span className="text-xs font-medium text-gray-500 uppercase tracking-wide">Tổng bút toán</span>
              <div className="bg-blue-50 p-1.5 rounded text-blue-600"><BookText className="w-4 h-4" /></div>
            </div>
            <div className="mt-2 text-2xl font-bold text-gray-900">{totalVouchers}</div>
          </div>
          
          <div className="bg-white p-4 rounded-lg border border-gray-200 shadow-sm flex flex-col justify-center">
            <div className="flex justify-between items-start">
              <span className="text-xs font-medium text-gray-500 uppercase tracking-wide">Tổng phát sinh</span>
              <div className="bg-green-50 p-1.5 rounded text-green-600"><Calculator className="w-4 h-4" /></div>
            </div>
            <div className="mt-2 text-2xl font-bold text-blue-600">{formatCurrency(totalAmount)} ₫</div>
          </div>

          <div className="bg-white p-4 rounded-lg border border-gray-200 shadow-sm flex flex-col justify-center">
            <span className="text-xs font-medium text-gray-500 uppercase tracking-wide">Từ ngày</span>
            <div className="mt-2 text-base font-semibold text-gray-900">{formatDateVN(fromDate)}</div>
          </div>

          <div className="bg-white p-4 rounded-lg border border-gray-200 shadow-sm flex flex-col justify-center">
            <span className="text-xs font-medium text-gray-500 uppercase tracking-wide">Đến ngày</span>
            <div className="mt-2 text-base font-semibold text-gray-900">{formatDateVN(toDate)}</div>
          </div>
        </div>

        {/* Filter Bar */}
        <div className="flex gap-4">
          <div className="relative flex-1 bg-white border border-gray-200 rounded-md shadow-sm">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-4 h-4 text-gray-400" />
            <input
              type="text"
              placeholder="Tìm kiếm chứng từ..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="w-full pl-9 pr-4 py-2 text-sm bg-transparent border-none focus:ring-0 outline-none"
            />
          </div>
          
          <div className="relative w-48 bg-white border border-gray-200 rounded-md shadow-sm flex items-center">
            <Calendar className="absolute left-3 w-4 h-4 text-gray-400" />
            <input
              type="date"
              value={fromDate}
              onChange={(e) => setFromDate(e.target.value)}
              className="w-full pl-9 pr-3 py-2 text-sm bg-transparent border-none focus:ring-0 outline-none text-gray-600"
            />
          </div>

          <div className="relative w-48 bg-white border border-gray-200 rounded-md shadow-sm flex items-center">
            <Calendar className="absolute left-3 w-4 h-4 text-gray-400" />
            <input
              type="date"
              value={toDate}
              onChange={(e) => setToDate(e.target.value)}
              className="w-full pl-9 pr-3 py-2 text-sm bg-transparent border-none focus:ring-0 outline-none text-gray-600"
            />
          </div>

          <div className="relative w-48 bg-white border border-gray-200 rounded-md shadow-sm flex items-center">
            <select 
              value={typeFilter}
              onChange={(e) => setTypeFilter(e.target.value)}
              className="w-full pl-3 pr-8 py-2 text-sm bg-transparent border-none focus:ring-0 outline-none appearance-none cursor-pointer"
            >
              <option value="all">Tất cả loại</option>
              <option value="0">Ghi tăng</option>
              <option value="1">Khấu hao</option>
              <option value="2">Bảo trì</option>
              <option value="3">Thanh lý</option>
              <option value="4">Điều chuyển</option>
            </select>
            <ChevronDown className="absolute right-3 w-4 h-4 text-gray-400 pointer-events-none" />
          </div>
        </div>

        {/* Main Table */}
        <div className="bg-white rounded-lg border border-gray-200 overflow-hidden shadow-sm">
          <div className="bg-blue-50/50 px-4 py-3 border-b border-gray-200 flex items-center gap-2 text-sm text-blue-700 font-medium">
            <BookText className="w-4 h-4" /> Sổ Nhật ký chung từ {formatDateVN(fromDate)} đến {formatDateVN(toDate)}
          </div>
          
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead className="bg-gray-50 border-b border-gray-200">
                <tr>
                  <th className="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wider w-32">Ngày</th>
                  <th className="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wider w-40">Số chứng từ</th>
                  <th className="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wider">Diễn giải</th>
                  <th className="px-4 py-3 text-center text-xs font-semibold text-gray-500 uppercase tracking-wider w-24">TK Nợ</th>
                  <th className="px-4 py-3 text-center text-xs font-semibold text-gray-500 uppercase tracking-wider w-24">TK Có</th>
                  <th className="px-4 py-3 text-right text-xs font-semibold text-gray-500 uppercase tracking-wider w-36">Số tiền</th>
                  <th className="px-4 py-3 text-center text-xs font-semibold text-gray-500 uppercase tracking-wider w-28">Loại</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100 bg-white">
                {isLoading ? (
                  <tr><td colSpan={7} className="px-6 py-12 text-center text-gray-500">Đang tổng hợp số liệu...</td></tr>
                ) : filteredEntries.length === 0 ? (
                  <tr>
                    <td colSpan={7} className="px-6 py-12 text-center text-gray-500">
                      <FileText className="w-10 h-10 mx-auto text-gray-300 mb-3" />
                      Không tìm thấy bút toán nào trong khoảng thời gian này
                    </td>
                  </tr>
                ) : (
                  filteredEntries.map((entry) => (
                    <tr key={entry.id} className="hover:bg-gray-50/50 transition-colors group">
                      <td className="px-4 py-3 whitespace-nowrap text-sm text-gray-600 flex items-center gap-1">
                        {getStandardType(entry.loaiChungTu) === '1' ? <ChevronRight className="w-3.5 h-3.5 text-gray-400" /> : <span className="w-3.5" />}
                        {entry.ngayLap ? new Date(entry.ngayLap).toLocaleDateString('vi-VN') : '-'}
                      </td>
                      <td className="px-4 py-3 whitespace-nowrap">
                        <Link to={`/vouchers/${entry.chungTuId}`} className="text-sm font-medium text-blue-600 hover:text-blue-800">
                          {entry.maChungTu}
                        </Link>
                      </td>
                      <td className="px-4 py-3 text-sm text-gray-700">
                        {entry.moTa}
                      </td>
                      <td className="px-4 py-3 whitespace-nowrap text-center">
                        {getAccountBadge(entry.taiKhoanNo) || (getStandardType(entry.loaiChungTu) === '1' ? <span className="text-xs text-gray-500">Nợ 9</span> : '-')}
                      </td>
                      <td className="px-4 py-3 whitespace-nowrap text-center">
                        {getAccountBadge(entry.taiKhoanCo) || (getStandardType(entry.loaiChungTu) === '1' ? <span className="text-xs text-gray-500">Có 9</span> : '-')}
                      </td>
                      <td className="px-4 py-3 whitespace-nowrap text-right text-sm font-medium text-gray-900">
                        {formatCurrency(entry.soTien)} ₫
                      </td>
                      <td className="px-4 py-3 whitespace-nowrap text-center">
                        {getTypeBadge(entry.loaiChungTu)}
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
              {filteredEntries.length > 0 && (
                <tfoot className="bg-gray-50 border-t border-gray-200">
                  <tr>
                    <td colSpan={5} className="px-4 py-3 text-left font-medium text-sm text-gray-700">
                      Tổng cộng ({totalVouchers} bút toán)
                    </td>
                    <td className="px-4 py-3 text-right font-bold text-blue-600 text-sm">
                      {formatCurrency(totalAmount)} ₫
                    </td>
                    <td></td>
                  </tr>
                </tfoot>
              )}
            </table>
          </div>
        </div>

        {/* Info Box Bottom */}
        <div className="bg-[#f8faff] rounded-lg border border-blue-100 overflow-hidden mt-6">
          <div className="bg-blue-50/50 px-4 py-3 border-b border-blue-100 flex items-center gap-2 text-sm text-blue-800 font-medium">
            <Info className="w-4 h-4" /> Về Sổ Nhật ký chung
          </div>
          <div className="p-5 grid grid-cols-2 gap-8">
            <div>
              <h4 className="text-sm font-semibold text-blue-800 mb-2">Định nghĩa:</h4>
              <p className="text-xs text-blue-600 leading-relaxed">
                Sổ Nhật ký chung là sổ kế toán tổng hợp ghi chép toàn bộ các nghiệp vụ kinh tế, tài chính phát sinh theo trình tự thời gian (từ ngày đầu đến cuối kỳ kế toán).
              </p>
            </div>
            <div>
              <h4 className="text-sm font-semibold text-blue-800 mb-2">Ý nghĩa:</h4>
              <ul className="text-xs text-blue-600 space-y-1 list-disc pl-4">
                <li>Ghi chép đầy đủ, chi tiết các nghiệp vụ phát sinh</li>
                <li>Kiểm tra tính chính xác của bút toán</li>
                <li>Làm căn cứ để ghi vào Sổ Cái</li>
                <li>Tuân thủ quy định kế toán Việt Nam</li>
              </ul>
            </div>
          </div>
          <div className="px-5 pb-5 pt-2 border-t border-blue-100/50">
            <h4 className="text-sm font-semibold text-blue-800 mb-3">Nguyên tắc ghi sổ:</h4>
            <div className="grid grid-cols-3 gap-4 text-xs text-blue-600">
              <div><span className="font-bold">1.</span> Ghi theo thứ tự thời gian phát sinh</div>
              <div><span className="font-bold">2.</span> Ghi đầy đủ nội dung nghiệp vụ</div>
              <div><span className="font-bold">3.</span> Đảm bảo nguyên tắc Nợ = Có</div>
            </div>
          </div>
        </div>

      </div>
    </div>
  );
}