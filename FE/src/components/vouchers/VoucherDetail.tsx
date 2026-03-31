import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router';
import { ArrowLeft, FileText, Lock, Printer, Download, Unlock } from 'lucide-react';
import { toast } from 'sonner';
import { voucherApi, ChungTu } from '../../api/voucherApi';

const typeConfig: Record<number, string> = {
  0: 'Ghi tăng TSCĐ',
  1: 'Khấu hao TSCĐ',
  2: 'Bảo trì TSCĐ',
  3: 'Thanh lý TSCĐ',
  4: 'Điều chuyển TSCĐ',
};

export function VoucherDetail() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [voucher, setVoucher] = useState<ChungTu | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  const fetchVoucherDetail = async () => {
    if (!id) return;
    setIsLoading(true);
    try {
      const response = await voucherApi.getById(Number(id));
      if (response.errorCode === 200) {
        setVoucher(response.data);
      } else {
        toast.error(response.message || 'Lỗi lấy chi tiết chứng từ');
        navigate('/vouchers');
      }
    } catch (error) {
      toast.error('Lỗi kết nối đến máy chủ');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchVoucherDetail();
  }, [id]);

  const handlePostVoucher = async () => {
    if (!id) return;
    if (window.confirm('Bạn có chắc chắn muốn ghi sổ chứng từ này?')) {
      try {
        const response = await voucherApi.postVoucher(Number(id));
        if (response.errorCode === 200) {
          toast.success('Ghi sổ thành công!');
          fetchVoucherDetail(); // Reload lại để cập nhật trạng thái
        } else {
          toast.error(response.message || 'Lỗi ghi sổ chứng từ.');
        }
      } catch (error) {
        toast.error('Lỗi kết nối máy chủ.');
      }
    }
  };

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
  };

  if (isLoading) {
    return <div className="text-center py-12 text-gray-500">Đang tải dữ liệu chứng từ...</div>;
  }

  if (!voucher) return null;

  // Trạng thái: 0 (Nháp), 1 (Ghi sổ), 2 (Đã khóa)
  const isLocked = voucher.trangThai === 'hoan_thanh' || voucher.trangThai === 'da_khoa';
  const entries = voucher.chiTietChungTus || [];

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <button onClick={() => navigate('/vouchers')} className="p-2 hover:bg-gray-100 rounded-lg">
            <ArrowLeft className="w-5 h-5" />
          </button>
          <div>
            <h1 className="font-bold text-gray-900">Chi tiết Chứng từ</h1>
            <p className="text-sm text-gray-500 mt-1">{voucher.maChungTu}</p>
          </div>
        </div>
        <div className="flex gap-2">
          <button className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50">
            <Printer className="w-4 h-4" /> In
          </button>
          <button className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50">
            <Download className="w-4 h-4" /> Xuất PDF
          </button>
        </div>
      </div>

      {/* Voucher Info */}
      <div className="bg-white rounded-lg border border-gray-200 p-6">
        <div className="flex items-start justify-between mb-6">
          <div className="flex items-center gap-3">
            <div className="p-3 bg-blue-100 rounded-lg">
              <FileText className="w-6 h-6 text-blue-600" />
            </div>
            <div>
              <h2 className="font-bold text-gray-900">{voucher.maChungTu}</h2>
              <p className="text-sm text-gray-600 mt-1">{typeConfig[voucher.loaiChungTu] || 'Khác'}</p>
            </div>
          </div>
          {isLocked && (
            <div className="flex items-center gap-2 px-3 py-2 bg-green-100 text-green-700 rounded-lg">
              <Lock className="w-4 h-4" />
              <span className="text-sm font-medium">Đã ghi sổ</span>
            </div>
          )}
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 pb-6 border-b border-gray-200">
          <div>
            <p className="text-sm text-gray-600 mb-1">Ngày chứng từ</p>
            <p className="font-medium text-gray-900">
              {voucher.ngayLap ? new Date(voucher.ngayLap).toLocaleDateString('vi-VN') : 'N/A'}
            </p>
          </div>
          <div>
            <p className="text-sm text-gray-600 mb-1">Diễn giải</p>
           <p className="font-medium text-gray-900">{voucher.moTa}</p>
          </div>
          <div>
            <p className="text-sm text-gray-600 mb-1">Người lập / Ngày tạo</p>
            <p className="font-medium text-gray-900">{voucher.nguoiLapId || 'Hệ thống tự động'}</p>
            <p className="text-xs text-gray-500">
              {voucher.ngayTao ? new Date(voucher.ngayTao).toLocaleDateString('vi-VN') : '-'}
            </p>
          </div>
          {isLocked && (
            <div>
              <p className="text-sm text-gray-600 mb-1">Người ghi sổ / Ngày ghi</p>
              <p className="font-medium text-gray-900">{voucher.nguoiGhiSo || 'Hệ thống'}</p>
              <p className="text-xs text-gray-500">
                {voucher.ngayGhiSo ? new Date(voucher.ngayGhiSo).toLocaleDateString('vi-VN') : '-'}
              </p>
            </div>
          )}
        </div>

        {isLocked && (
          <div className="mt-4 p-4 bg-yellow-50 border border-yellow-200 rounded-lg flex items-center gap-3">
            <Lock className="w-5 h-5 text-yellow-600" />
            <p className="text-sm text-yellow-800">Chứng từ đã được ghi sổ và không thể chỉnh sửa</p>
          </div>
        )}
      </div>

      {/* Accounting Entries */}
      <div className="bg-white rounded-lg border border-gray-200 overflow-hidden">
        <div className="px-6 py-4 border-b border-gray-200 bg-gray-50">
          <h3 className="font-semibold text-gray-900">Bảng định khoản ({entries.length} dòng)</h3>
        </div>

        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50 border-b border-gray-200">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">STT</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">TK Nợ</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">TK Có</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Diễn giải</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase">Số tiền</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {entries.length === 0 ? (
                <tr><td colSpan={5} className="px-6 py-4 text-center text-gray-500">Chưa có chi tiết định khoản</td></tr>
              ) : (
                entries.map((entry, index) => (
                  <tr key={entry.id || index} className="hover:bg-gray-50">
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">{index + 1}</td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{entry.taiKhoanNo}</td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{entry.taiKhoanCo}</td>
                    <td className="px-6 py-4">
                      <p className="text-sm text-gray-900">{entry.moTa}</p>
                      {entry.maTaiSan && <p className="text-xs text-blue-600 mt-1">Mã TS: {entry.maTaiSan}</p>}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium text-gray-900">
                      {formatCurrency(entry.soTien)}
                    </td>
                  </tr>
                ))
              )}
            </tbody>
            <tfoot className="bg-gray-50 border-t-2 border-gray-300">
              <tr>
                <td colSpan={4} className="px-6 py-4 text-right font-semibold text-gray-900">Tổng cộng:</td>
                <td className="px-6 py-4 text-right font-bold text-blue-600">
                  {formatCurrency(voucher.tongTien)}
                </td>
              </tr>
            </tfoot>
          </table>
        </div>
      </div>

      {/* Actions */}
      {!isLocked && (
        <div className="flex justify-end gap-4">
          <button
            onClick={() => navigate('/vouchers')}
            className="px-6 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors"
          >
            Đóng
          </button>
          <button
            onClick={handlePostVoucher}
            className="flex items-center gap-2 px-6 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors"
          >
            <Unlock className="w-5 h-5" /> Ghi sổ Kế toán
          </button>
        </div>
      )}
    </div>
  );
}