import { useState, useEffect } from 'react';
import { Link } from 'react-router';
import { Search, Filter, FileText, Eye, Lock, Unlock, Download } from 'lucide-react';
import { toast } from 'sonner';
import { voucherApi, ChungTu } from '../../api/voucherApi';
import * as XLSX from 'xlsx'; // 1. Import thư viện XLSX

// Map LoaiChungTu theo Enum C#
const typeConfig: Record<number, { label: string; color: string }> = {
  0: { label: 'Ghi tăng', color: 'bg-green-100 text-green-700' },
  1: { label: 'Khấu hao', color: 'bg-orange-100 text-orange-700' },
  2: { label: 'Bảo trì', color: 'bg-blue-100 text-blue-700' },
  3: { label: 'Thanh lý', color: 'bg-red-100 text-red-700' },
  4: { label: 'Điều chuyển', color: 'bg-purple-100 text-purple-700' },
};

const statusConfig: Record<string, { label: string; color: string; icon: any }> = {
  'nhap': { label: 'Nháp', color: 'bg-gray-100 text-gray-700', icon: FileText },
  'hoan_thanh': { label: 'Đã ghi sổ', color: 'bg-green-100 text-green-700', icon: Lock },
  'da_khoa': { label: 'Đã khóa', color: 'bg-red-100 text-red-700', icon: Lock },
};

export function VoucherList() {
  const [searchTerm, setSearchTerm] = useState('');
  const [filterType, setFilterType] = useState<string>('all');
  const [filterStatus, setFilterStatus] = useState<string>('all');
  
  const [vouchers, setVouchers] = useState<ChungTu[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  const fetchVouchers = async () => {
    setIsLoading(true);
    try {
      const response = await voucherApi.getAll();
      if (response.errorCode === 200) {
        setVouchers(response.data);
      }
    } catch (error) {
      toast.error('Lỗi kết nối đến máy chủ.');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => { fetchVouchers(); }, []);

  // 2. Hàm xử lý Xuất Excel
  const handleExportExcel = () => {
    if (filteredVouchers.length === 0) {
      toast.error("Không có dữ liệu để xuất!");
      return;
    }

    try {
      // Chuyển đổi dữ liệu sang định dạng bảng Excel (Tiếng Việt)
      const dataToExport = filteredVouchers.map((v) => ({
        "Số chứng từ": v.maChungTu,
        "Ngày lập": v.ngayLap ? new Date(v.ngayLap).toLocaleDateString('vi-VN') : 'N/A',
        "Loại chứng từ": typeConfig[v.loaiChungTu]?.label || 'Khác',
        "Diễn giải": v.moTa,
        "Số tiền (VNĐ)": v.tongTien,
        "Trạng thái": statusConfig[v.trangThai]?.label || 'N/A',
        "Ngày tạo hệ thống": v.ngayTao ? new Date(v.ngayTao).toLocaleString('vi-VN') : ''
      }));

      // Tạo sheet và workbook
      const worksheet = XLSX.utils.json_to_sheet(dataToExport);
      const workbook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(workbook, worksheet, "Danh sách Chứng từ");

      // Cấu hình độ rộng cột tự động đơn giản
      const wscols = [
        { wch: 20 }, // Số chứng từ
        { wch: 12 }, // Ngày lập
        { wch: 15 }, // Loại
        { wch: 40 }, // Diễn giải
        { wch: 15 }, // Số tiền
        { wch: 15 }, // Trạng thái
        { wch: 20 }, // Ngày tạo
      ];
      worksheet['!cols'] = wscols;

      // Xuất file với tên có timestamp
      const fileName = `Danh_Sach_Chung_Tu_${new Date().getTime()}.xlsx`;
      XLSX.writeFile(workbook, fileName);
      
      toast.success("Xuất file Excel thành công!");
    } catch (error) {
      console.error("Export error:", error);
      toast.error("Có lỗi xảy ra khi xuất file Excel.");
    }
  };

  const handlePostVoucher = async (id: number) => {
    if (window.confirm('Bạn có chắc chắn muốn ghi sổ chứng từ này?')) {
      try {
        const response = await voucherApi.postVoucher(id);
        if (response.errorCode === 200) {
          toast.success('Ghi sổ thành công!');
          fetchVouchers();
        }
      } catch (error) {
        toast.error('Lỗi kết nối đến máy chủ.');
      }
    }
  };

  const filteredVouchers = vouchers.filter(voucher => {
    const searchStr = `${voucher.maChungTu} ${voucher.moTa}`.toLowerCase();
    const matchesSearch = searchStr.includes(searchTerm.toLowerCase());
    const matchesType = filterType === 'all' || voucher.loaiChungTu.toString() === filterType;
    const matchesStatus = filterStatus === 'all' || voucher.trangThai === filterStatus;
    return matchesSearch && matchesType && matchesStatus;
  });

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
  };

  const totalDraft = vouchers.filter(v => v.trangThai === 'nhap').length;
  const totalPosted = vouchers.filter(v => v.trangThai === 'hoan_thanh').length;
  const totalLocked = vouchers.filter(v => v.trangThai === 'da_khoa').length;

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Chứng từ Kế toán</h1>
          <p className="text-sm text-gray-500 mt-1">Danh sách chứng từ tài sản cố định</p>
        </div>
        {/* 3. Gán onClick cho nút Xuất Excel */}
        <button 
          onClick={handleExportExcel}
          className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors shadow-sm bg-white"
        >
          <Download className="w-5 h-5" /> Xuất Excel
        </button>
      </div>

      {/* Stats Section giữ nguyên */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        <div className="bg-white rounded-lg border border-gray-200 p-4">
          <p className="text-sm text-gray-600 mb-1">Tổng chứng từ</p>
          <p className="font-bold text-gray-900">{vouchers.length}</p>
        </div>
        <div className="bg-white rounded-lg border border-gray-200 p-4 border-l-4 border-l-yellow-400">
          <p className="text-sm text-gray-600 mb-1">Chờ ghi sổ</p>
          <p className="font-bold text-yellow-600">{totalDraft}</p>
        </div>
        <div className="bg-white rounded-lg border border-gray-200 p-4 border-l-4 border-l-green-400">
          <p className="text-sm text-gray-600 mb-1">Đã ghi sổ</p>
          <p className="font-bold text-green-600">{totalPosted}</p>
        </div>
        <div className="bg-white rounded-lg border border-gray-200 p-4 border-l-4 border-l-red-400">
          <p className="text-sm text-gray-600 mb-1">Đã khóa</p>
          <p className="font-bold text-red-600">{totalLocked}</p>
        </div>
      </div>

      {/* Filters and Table giữ nguyên... */}
      <div className="bg-white rounded-lg border border-gray-200 p-4">
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
            <select value={filterType} onChange={(e) => setFilterType(e.target.value)} className="px-4 py-2 border border-gray-300 rounded-lg">
              <option value="all">Tất cả loại</option>
              <option value="0">Ghi tăng</option>
              <option value="1">Khấu hao</option>
              <option value="2">Bảo trì</option>
              <option value="3">Thanh lý</option>
              <option value="4">Điều chuyển</option>
            </select>
            <select value={filterStatus} onChange={(e) => setFilterStatus(e.target.value)} className="px-4 py-2 border border-gray-300 rounded-lg">
              <option value="all">Tất cả trạng thái</option>
              <option value="nhap">Nháp</option>
              <option value="hoan_thanh">Đã ghi sổ</option>
              <option value="da_khoa">Đã khóa</option>
            </select>
          </div>
        </div>
      </div>

      <div className="bg-white rounded-lg border border-gray-200 overflow-hidden shadow-sm">
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50 border-b border-gray-200">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Số chứng từ</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Ngày</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Loại</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Diễn giải</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase">Số tiền</th>
                <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase">Trạng thái</th>
                <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase">Thao tác</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {isLoading ? (
                <tr><td colSpan={7} className="text-center py-6 text-gray-500">Đang tải dữ liệu...</td></tr>
              ) : filteredVouchers.length === 0 ? (
                <tr><td colSpan={7} className="text-center py-6 text-gray-500">Không tìm thấy chứng từ.</td></tr>
              ) : (
                filteredVouchers.map((voucher) => {
                  const statusInfo = statusConfig[voucher.trangThai] || { label: 'N/A', color: 'bg-gray-100', icon: FileText };
                  const StatusIcon = statusInfo.icon;
                  const typeInfo = typeConfig[voucher.loaiChungTu] || { label: 'Khác', color: 'bg-gray-100' };

                  return (
                    <tr key={voucher.id} className="hover:bg-gray-50 transition-colors">
                      <td className="px-6 py-4 whitespace-nowrap">
                        <Link to={`/vouchers/${voucher.id}`} className="text-sm font-medium text-blue-600 hover:underline">
                          {voucher.maChungTu}
                        </Link>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                        {voucher.ngayLap ? new Date(voucher.ngayLap).toLocaleDateString('vi-VN') : 'N/A'}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap">
                        <span className={`inline-flex px-2 py-1 text-xs font-medium rounded-full ${typeInfo.color}`}>
                          {typeInfo.label}
                        </span>
                      </td>
                      <td className="px-6 py-4 text-sm text-gray-900 max-w-xs truncate">
                        {voucher.moTa}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium text-gray-900">
                        {formatCurrency(voucher.tongTien)}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-center">
                        <span className={`inline-flex items-center gap-1 px-2 py-1 text-xs font-medium rounded-full ${statusInfo.color}`}>
                          <StatusIcon className="w-3 h-3" />
                          {statusInfo.label}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-center">
                        <div className="flex items-center justify-center gap-2">
                          <Link to={`/vouchers/${voucher.id}`} className="p-1 text-blue-600 hover:bg-blue-50 rounded" title="Xem chi tiết">
                            <Eye className="w-4 h-4" />
                          </Link>
                          {voucher.trangThai === 'nhap' && (
                            <button
                              onClick={() => voucher.id && handlePostVoucher(voucher.id)}
                              className="p-1 text-green-600 hover:bg-green-50 rounded"
                              title="Ghi sổ"
                            >
                              <Unlock className="w-4 h-4" />
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