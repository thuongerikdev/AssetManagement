import { useState, useEffect } from 'react';
import { Calculator, FileText, Download, AlertCircle, Loader2 } from 'lucide-react';
import { toast } from "sonner";
import { assetApi, TaiSan } from '../../api/assetApi';
import { departmentApi, Department } from '../../api/departmentApi';
import { depreciationHistoryApi, LichSuKhauHao } from '../../api/depreciationHistoryApi';

interface DepreciationAsset {
  id: number;
  code: string;
  name: string;
  department: string;
  originalValue: number;
  accumulatedDepreciation: number;
  monthlyDepreciation: number;
  remainingValue: number;
  selected: boolean;
}

export function DepreciationList() {
  const [selectedMonth, setSelectedMonth] = useState(() => {
    // Mặc định chọn tháng hiện tại (YYYY-MM)
    const date = new Date();
    return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}`;
  });
  
  const [showVoucher, setShowVoucher] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  // Raw data từ API
  const [rawAssets, setRawAssets] = useState<TaiSan[]>([]);
  const [departments, setDepartments] = useState<Department[]>([]);
  const [history, setHistory] = useState<LichSuKhauHao[]>([]);

  // Dữ liệu đã xử lý để hiển thị lên bảng
  const [tableAssets, setTableAssets] = useState<DepreciationAsset[]>([]);

  // 1. Tải toàn bộ dữ liệu cần thiết
  const fetchData = async () => {
    setIsLoading(true);
    try {
      const [assetRes, deptRes, historyRes] = await Promise.all([
        assetApi.getAll(),
        departmentApi.getAll(),
        depreciationHistoryApi.getAll()
      ]);

      if (assetRes.errorCode === 200) setRawAssets(assetRes.data);
      if (deptRes.errorCode === 200) setDepartments(deptRes.data);
      if (historyRes.errorCode === 200) setHistory(historyRes.data);
    } catch (error) {
      toast.error('Lỗi khi tải dữ liệu từ máy chủ.');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  // 2. Tính toán lại danh sách tài sản HỢP LỆ mỗi khi đổi Tháng hoặc Data thay đổi
  useEffect(() => {
    if (!rawAssets.length) return;

    const validAssets = rawAssets
      .filter(asset => {
        // Chỉ lấy tài sản Đang sử dụng (Enum là 2 hoặc chuỗi 'DangSuDung')
        const status = asset.trangThai?.toString();
        if (status !== '2' && status !== 'DangSuDung') return false; 
        
        // Bỏ qua tài sản đã hết giá trị
        if ((asset.giaTriConLai || 0) <= 0) return false;

        // Kiểm tra xem tháng này tài sản đã được khấu hao chưa
        const isAlreadyDepreciated = history.some(h => 
          h.taiSanId === asset.id && h.kyKhauHao === selectedMonth
        );
        return !isAlreadyDepreciated;
      })
      .map(asset => {
        // Nếu API backend chưa tính KhauHaoHangThang, ta tự tính tạm trên UI
        let monthlyAmt = asset.khauHaoHangThang || 0;
        if (monthlyAmt === 0 && asset.nguyenGia && asset.thoiGianKhauHao) {
          monthlyAmt = asset.nguyenGia / asset.thoiGianKhauHao;
        }

        return {
          id: asset.id!,
          code: asset.maTaiSan,
          name: asset.tenTaiSan,
          department: departments.find(d => d.id === asset.phongBanId)?.tenPhongBan || 'Chưa cấp phát',
          originalValue: asset.nguyenGia || 0,
          accumulatedDepreciation: asset.khauHaoLuyKe || 0,
          monthlyDepreciation: monthlyAmt,
          remainingValue: asset.giaTriConLai || 0,
          selected: true // Mặc định chọn tất cả
        };
      });

    setTableAssets(validAssets);
  }, [rawAssets, history, selectedMonth, departments]);

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
  };

  const toggleSelect = (id: number) => {
    setTableAssets(tableAssets.map(asset => 
      asset.id === id ? { ...asset, selected: !asset.selected } : asset
    ));
  };

  const toggleSelectAll = () => {
    const allSelected = tableAssets.every(a => a.selected);
    setTableAssets(tableAssets.map(asset => ({ ...asset, selected: !allSelected })));
  };

  const selectedAssets = tableAssets.filter(a => a.selected);
  const totalDepreciation = selectedAssets.reduce((sum, a) => sum + a.monthlyDepreciation, 0);

  // 3. Xử lý GHI SỔ KHẤU HAO
  const handleGhiSo = async () => {
    if (selectedAssets.length === 0) {
      toast.error('Vui lòng chọn ít nhất 1 tài sản để khấu hao.');
      return;
    }

    setIsSubmitting(true);
    try {
      // Gửi request khấu hao cho từng tài sản đã chọn
      // Backend sẽ tự động tính toán và sinh Chứng từ kế toán cho từng tài sản
      const promises = selectedAssets.map(asset => 
        depreciationHistoryApi.create({
          taiSanId: asset.id,
          kyKhauHao: selectedMonth,
          soTien: asset.monthlyDepreciation
        })
      );

      const results = await Promise.all(promises);
      
      const failedCount = results.filter(r => r.errorCode !== 200).length;
      
      if (failedCount === 0) {
        // SỬA LẠI CÂU THÔNG BÁO Ở ĐÂY CHO RÕ RÀNG
        toast.success(`Đã ghi sổ và tự động sinh Chứng từ khấu hao cho ${selectedAssets.length} tài sản!`);
        setShowVoucher(false);
        fetchData(); // Load lại data để cập nhật số liệu mới nhất
      } else {
        toast.warning(`Hoàn thành một phần. Có ${failedCount} tài sản bị lỗi.`);
        fetchData();
      }
    } catch (error) {
      toast.error('Lỗi hệ thống khi ghi sổ khấu hao.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Khấu hao Tài sản</h1>
          <p className="text-sm text-gray-500 mt-1">Tính và ghi nhận khấu hao tài sản cố định</p>
        </div>
      </div>

      {/* Period Selection */}
      <div className="bg-white rounded-lg border border-gray-200 p-6">
        <div className="flex flex-col lg:flex-row gap-4 items-start lg:items-end">
          <div className="flex-1">
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Chọn kỳ khấu hao (Tháng/Năm)
            </label>
            <input
              type="month"
              value={selectedMonth}
              onChange={(e) => setSelectedMonth(e.target.value)}
              className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
          </div>
          <div className="flex gap-2">
            <button
              onClick={() => {
                if (selectedAssets.length === 0) {
                  toast.error("Không có tài sản nào được chọn!");
                  return;
                }
                setShowVoucher(true);
              }}
              disabled={isLoading || tableAssets.length === 0}
              className="flex items-center gap-2 px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors disabled:opacity-50"
            >
              <Calculator className="w-5 h-5" />
              Tính Khấu hao
            </button>
            <button className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors">
              <Download className="w-5 h-5" />
              Xuất Excel
            </button>
          </div>
        </div>
      </div>

      {/* Info Alert */}
      <div className="bg-blue-50 border border-blue-200 rounded-lg p-4 flex items-start gap-3">
        <AlertCircle className="w-5 h-5 text-blue-600 mt-0.5" />
        <div className="flex-1">
          <p className="text-sm text-blue-900 font-medium">Thông tin khấu hao tháng {selectedMonth}</p>
          <p className="text-sm text-blue-700 mt-1">
            Hệ thống tự động lọc ra các tài sản đang hoạt động và CHƯA được khấu hao trong tháng này.
            Tổng khấu hao dự kiến kỳ này: <span className="font-semibold">{formatCurrency(totalDepreciation)}</span>
          </p>
        </div>
      </div>

      {/* Asset List */}
      <div className="bg-white rounded-lg border border-gray-200 overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50 border-b border-gray-200">
              <tr>
                <th className="px-6 py-3 text-left">
                  <input
                    type="checkbox"
                    checked={tableAssets.length > 0 && tableAssets.every(a => a.selected)}
                    onChange={toggleSelectAll}
                    disabled={tableAssets.length === 0}
                    className="rounded border-gray-300 text-blue-600 focus:ring-blue-500"
                  />
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Mã TS</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Tên tài sản</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Phòng ban</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase">Nguyên giá</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase">KH lũy kế</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase bg-blue-50">KH tháng này</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase">Còn lại</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {isLoading ? (
                <tr><td colSpan={8} className="text-center py-6 text-gray-500">Đang tải dữ liệu...</td></tr>
              ) : tableAssets.length === 0 ? (
                <tr>
                  <td colSpan={8} className="text-center py-6 text-gray-500">
                    Không có tài sản nào cần khấu hao trong tháng {selectedMonth}. 
                    Có thể tất cả tài sản đã được khấu hao hoặc không còn giá trị.
                  </td>
                </tr>
              ) : (
                tableAssets.map((asset) => (
                  <tr key={asset.id} className={`hover:bg-gray-50 transition-colors ${!asset.selected ? 'opacity-50' : ''}`}>
                    <td className="px-6 py-4">
                      <input
                        type="checkbox"
                        checked={asset.selected}
                        onChange={() => toggleSelect(asset.id)}
                        className="rounded border-gray-300 text-blue-600 focus:ring-blue-500"
                      />
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap"><span className="text-sm font-medium text-blue-600">{asset.code}</span></td>
                    <td className="px-6 py-4"><span className="text-sm text-gray-900">{asset.name}</span></td>
                    <td className="px-6 py-4 whitespace-nowrap"><span className="text-sm text-gray-600">{asset.department}</span></td>
                    <td className="px-6 py-4 whitespace-nowrap text-right"><span className="text-sm text-gray-900">{formatCurrency(asset.originalValue)}</span></td>
                    <td className="px-6 py-4 whitespace-nowrap text-right"><span className="text-sm text-gray-600">{formatCurrency(asset.accumulatedDepreciation)}</span></td>
                    <td className="px-6 py-4 whitespace-nowrap text-right bg-blue-50/30"><span className="text-sm font-bold text-blue-700">{formatCurrency(asset.monthlyDepreciation)}</span></td>
                    <td className="px-6 py-4 whitespace-nowrap text-right"><span className="text-sm font-medium text-green-600">{formatCurrency(asset.remainingValue)}</span></td>
                  </tr>
                ))
              )}
            </tbody>
            {tableAssets.length > 0 && (
              <tfoot className="bg-gray-50 border-t-2 border-gray-300">
                <tr>
                  <td colSpan={6} className="px-6 py-4 text-right font-semibold text-gray-900">
                    Tổng khấu hao dự tính (các mục đã chọn):
                  </td>
                  <td colSpan={2} className="px-6 py-4 text-left font-bold text-blue-700 text-lg">
                    {formatCurrency(totalDepreciation)}
                  </td>
                </tr>
              </tfoot>
            )}
          </table>
        </div>
      </div>

      {/* Voucher Preview Modal */}
      {showVoucher && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-lg max-w-3xl w-full max-h-[90vh] overflow-auto">
            <div className="p-6 border-b border-gray-200">
              <div className="flex items-center justify-between">
                <h3 className="font-bold text-gray-900">Xác nhận & Ghi sổ Khấu hao</h3>
                <button onClick={() => setShowVoucher(false)} className="text-gray-400 hover:text-gray-600">✕</button>
              </div>
            </div>
            
            <div className="p-6 space-y-6">
              <div className="grid grid-cols-2 gap-4 pb-4 border-b border-gray-200">
                <div>
                  <p className="text-sm text-gray-600">Kỳ hạch toán</p>
                  <p className="font-semibold text-gray-900">Tháng {selectedMonth.split('-')[1]} Năm {selectedMonth.split('-')[0]}</p>
                </div>
                <div>
                  <p className="text-sm text-gray-600">Ngày ghi sổ</p>
                  <p className="font-semibold text-gray-900">{new Date().toLocaleDateString('vi-VN')}</p>
                </div>
                <div className="col-span-2">
                  <p className="text-sm text-gray-600">Diễn giải</p>
                  <p className="font-semibold text-gray-900">
                    Trích khấu hao Tài sản cố định kỳ {selectedMonth} cho {selectedAssets.length} tài sản.
                  </p>
                </div>
              </div>

              {/* Asset Details */}
              <div>
                <h4 className="font-semibold text-gray-900 mb-3">Chi tiết tài sản được trích khấu hao</h4>
                <div className="space-y-2 max-h-60 overflow-y-auto">
                  {selectedAssets.map((asset) => (
                    <div key={asset.id} className="flex justify-between items-center p-3 bg-gray-50 rounded border border-gray-100">
                      <div>
                        <p className="text-sm font-medium text-gray-900">{asset.code} - {asset.name}</p>
                        <p className="text-xs text-gray-600">{asset.department}</p>
                      </div>
                      <p className="text-sm font-bold text-blue-600">+{formatCurrency(asset.monthlyDepreciation)}</p>
                    </div>
                  ))}
                </div>
              </div>
              
              <div className="flex justify-between items-center bg-blue-50 p-4 rounded-lg border border-blue-100">
                <span className="font-semibold text-blue-900">Tổng tiền khấu hao ghi nhận:</span>
                <span className="font-bold text-xl text-blue-700">{formatCurrency(totalDepreciation)}</span>
              </div>
            </div>

            <div className="p-6 border-t border-gray-200 flex justify-end gap-4 bg-gray-50 rounded-b-lg">
              <button
                onClick={() => setShowVoucher(false)}
                disabled={isSubmitting}
                className="px-4 py-2 border border-gray-300 bg-white rounded-lg hover:bg-gray-100 transition-colors"
              >
                Hủy
              </button>
              <button 
                onClick={handleGhiSo}
                disabled={isSubmitting}
                className="flex items-center gap-2 px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors disabled:opacity-70"
              >
                {isSubmitting ? <Loader2 className="w-4 h-4 animate-spin" /> : <FileText className="w-4 h-4" />}
                {isSubmitting ? 'Đang xử lý...' : 'Xác nhận Ghi sổ'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}