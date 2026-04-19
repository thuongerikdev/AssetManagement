import { useState, useEffect } from 'react';
import { Calculator, FileText, Download, AlertCircle, Loader2, CheckCircle } from 'lucide-react';
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
  ngayCapPhatStr?: string;
  isAlreadyDepreciated: boolean; // Cờ nhận biết tài sản đã được tính khấu hao trong kỳ này
}

export function DepreciationList() {
  const [selectedMonth, setSelectedMonth] = useState(() => {
    const date = new Date();
    return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}`;
  });
  
  const [showVoucher, setShowVoucher] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const [rawAssets, setRawAssets] = useState<TaiSan[]>([]);
  const [departments, setDepartments] = useState<Department[]>([]);
  const [history, setHistory] = useState<LichSuKhauHao[]>([]);
  const [tableAssets, setTableAssets] = useState<DepreciationAsset[]>([]);

  // Lấy giá trị max (Tháng hiện tại) để chặn chọn tháng tương lai
  const getMaxMonth = () => {
    const today = new Date();
    return `${today.getFullYear()}-${String(today.getMonth() + 1).padStart(2, '0')}`;
  };

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

  // THUẬT TOÁN TÍNH KHẤU HAO THEO NGÀY CẤP PHÁT
  useEffect(() => {
    if (!rawAssets.length) return;

    const [selYear, selMonth] = selectedMonth.split('-').map(Number);
    // Lấy ngày cuối cùng của tháng đang chọn
    const daysInSelectedMonth = new Date(selYear, selMonth, 0).getDate();
    const endOfSelMonth = new Date(selYear, selMonth - 1, daysInSelectedMonth, 23, 59, 59);

    const validAssets = rawAssets
      .filter(asset => {
        const status = asset.trangThai?.toString();
        // Chỉ lấy tài sản Đang sử dụng
        if (status !== '2' && status !== 'DangSuDung') return false; 
        
        // Phải có ngày cấp phát và ngày cấp phát không được lớn hơn tháng đang chọn
        if (!asset.ngayCapPhat) return false;
        const capPhatDate = new Date(asset.ngayCapPhat);
        if (capPhatDate > endOfSelMonth) return false;

        // Bỏ qua tài sản đã hết giá trị
        if ((asset.giaTriConLai || 0) <= 0) return false;

        return true; // GIỮ LẠI TẤT CẢ (Kể cả đã khấu hao)
      })
      .map(asset => {
        // KIỂM TRA ĐÃ KHẤU HAO TRONG THÁNG NÀY CHƯA
        const isAlreadyDepreciated = history.some(h => 
          h.taiSanId === asset.id && h.kyKhauHao === selectedMonth
        );

        // Tính mức khấu hao cứng 1 tháng
        let standardMonthlyAmt = asset.khauHaoHangThang || 0;
        if (standardMonthlyAmt === 0 && asset.nguyenGia && asset.thoiGianKhauHao) {
          standardMonthlyAmt = asset.nguyenGia / asset.thoiGianKhauHao;
        }

        let finalMonthlyAmt = standardMonthlyAmt;
        const capPhatDate = new Date(asset.ngayCapPhat!);

        // LOGIC CHIA LẺ NGÀY: Nếu tài sản được cấp phát vào chính cái tháng đang tính
        if (capPhatDate.getFullYear() === selYear && (capPhatDate.getMonth() + 1) === selMonth) {
          const usedDays = daysInSelectedMonth - capPhatDate.getDate() + 1; // Số ngày sử dụng thực tế
          finalMonthlyAmt = (standardMonthlyAmt / daysInSelectedMonth) * usedDays;
        }

        // Đảm bảo khấu hao không vượt quá giá trị còn lại
        const remaining = asset.giaTriConLai || 0;
        if (finalMonthlyAmt > remaining) {
            finalMonthlyAmt = remaining;
        }

        return {
          id: asset.id!,
          code: asset.maTaiSan,
          name: asset.tenTaiSan,
          department: departments.find(d => d.id === asset.phongBanId)?.tenPhongBan || 'Chưa cấp phát',
          originalValue: asset.nguyenGia || 0,
          accumulatedDepreciation: asset.khauHaoLuyKe || 0,
          monthlyDepreciation: Math.round(finalMonthlyAmt),
          remainingValue: remaining,
          selected: !isAlreadyDepreciated, // Tự động bỏ chọn nếu đã khấu hao
          ngayCapPhatStr: capPhatDate.toLocaleDateString('vi-VN'),
          isAlreadyDepreciated // Cờ để render UI
        };
      });

    setTableAssets(validAssets as any[]);
  }, [rawAssets, history, selectedMonth, departments]);

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
  };

  const toggleSelect = (id: number) => {
    setTableAssets(tableAssets.map(asset => {
      // Chặn không cho toggle nếu đã khấu hao
      if (asset.id === id && !asset.isAlreadyDepreciated) {
        return { ...asset, selected: !asset.selected };
      }
      return asset;
    }));
  };

  const toggleSelectAll = () => {
    // Chỉ tính những tài sản chưa khấu hao
    const selectableAssets = tableAssets.filter(a => !a.isAlreadyDepreciated);
    if (selectableAssets.length === 0) return;

    const allSelected = selectableAssets.every(a => a.selected);
    setTableAssets(tableAssets.map(asset => {
      if (!asset.isAlreadyDepreciated) {
        return { ...asset, selected: !allSelected };
      }
      return asset;
    }));
  };

  const selectedAssets = tableAssets.filter(a => a.selected && !a.isAlreadyDepreciated);
  const totalDepreciation = selectedAssets.reduce((sum, a) => sum + a.monthlyDepreciation, 0);

  // ==========================================
  // HÀM XỬ LÝ GHI SỔ KHẤU HAO (BULK CREATE)
  // ==========================================
  const handleGhiSo = async () => {
    if (selectedAssets.length === 0) {
      toast.error('Vui lòng chọn ít nhất 1 tài sản để khấu hao.');
      return;
    }

    setIsSubmitting(true);
    try {
      // ĐÓNG GÓI DỮ LIỆU THÀNH 1 PAYLOAD CHUẨN CỦA API BULK
      const payload = {
        kyKhauHao: selectedMonth,
        danhSachTaiSan: selectedAssets.map(asset => ({
          taiSanId: asset.id,
          soTien: asset.monthlyDepreciation
        }))
      };

      // GỌI API ĐÚNG 1 LẦN DUY NHẤT
      const response = await depreciationHistoryApi.createBulk(payload);
      
      if (response.errorCode === 200) {
        toast.success(`Đã ghi sổ Chứng từ tổng cho ${selectedAssets.length} tài sản!`);
        setShowVoucher(false);
        fetchData(); // Tải lại giao diện để các checkbox tự động bị khóa
      } else {
        toast.error(response.message || 'Lỗi khi ghi sổ khấu hao.');
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
          <p className="text-sm text-gray-500 mt-1">Tính và ghi nhận khấu hao (Tự động chia lẻ theo ngày cấp phát)</p>
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
              max={getMaxMonth()} // Chặn người dùng chọn tháng trong tương lai
              onChange={(e) => setSelectedMonth(e.target.value)}
              className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
          </div>
          <div className="flex gap-2">
            <button
              onClick={() => {
                if (selectedAssets.length === 0) {
                  toast.error("Không có tài sản nào hợp lệ được chọn!");
                  return;
                }
                setShowVoucher(true);
              }}
              disabled={isLoading || tableAssets.length === 0 || selectedAssets.length === 0}
              className="flex items-center gap-2 px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors disabled:opacity-50"
            >
              <Calculator className="w-5 h-5" /> Tính Khấu hao
            </button>
            <button className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors">
              <Download className="w-5 h-5" /> Xuất Excel
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
            Hệ thống tự động lọc ra các tài sản đang hoạt động. <b>Lưu ý:</b> Tài sản mới cấp phát trong tháng này sẽ được tính khấu hao lẻ theo số ngày sử dụng thực tế.
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
                    checked={tableAssets.filter(a => !a.isAlreadyDepreciated).length > 0 && tableAssets.filter(a => !a.isAlreadyDepreciated).every(a => a.selected)}
                    onChange={toggleSelectAll}
                    disabled={tableAssets.filter(a => !a.isAlreadyDepreciated).length === 0}
                    className="rounded border-gray-300 text-blue-600 focus:ring-blue-500 disabled:opacity-50"
                  />
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Mã TS</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Tên tài sản</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Ngày cấp phát</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase">Nguyên giá</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase">KH lũy kế</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase bg-blue-50">KH tháng này</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {isLoading ? (
                <tr><td colSpan={7} className="text-center py-6 text-gray-500">Đang tải dữ liệu...</td></tr>
              ) : tableAssets.length === 0 ? (
                <tr>
                  <td colSpan={7} className="text-center py-6 text-gray-500">
                    Không có tài sản nào cần khấu hao trong tháng {selectedMonth}. 
                  </td>
                </tr>
              ) : (
                tableAssets.map((asset: any) => (
                  <tr key={asset.id} className={`hover:bg-gray-50 transition-colors ${(!asset.selected && !asset.isAlreadyDepreciated) ? 'opacity-50' : ''} ${asset.isAlreadyDepreciated ? 'bg-gray-50/50' : ''}`}>
                    <td className="px-6 py-4">
                      {asset.isAlreadyDepreciated ? (
                        <div className="w-4 h-4 rounded bg-gray-200 border border-gray-300 flex items-center justify-center" title="Đã tính khấu hao">
                          <CheckCircle className="w-3 h-3 text-gray-500" />
                        </div>
                      ) : (
                        <input
                          type="checkbox"
                          checked={asset.selected}
                          onChange={() => toggleSelect(asset.id)}
                          className="rounded border-gray-300 text-blue-600 focus:ring-blue-500"
                        />
                      )}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap"><span className={`text-sm font-medium ${asset.isAlreadyDepreciated ? 'text-gray-500' : 'text-blue-600'}`}>{asset.code}</span></td>
                    <td className="px-6 py-4">
                      <span className={`text-sm ${asset.isAlreadyDepreciated ? 'text-gray-500' : 'text-gray-900'}`}>{asset.name}</span>
                      {asset.isAlreadyDepreciated && (
                        <span className="ml-2 inline-flex items-center px-2 py-0.5 rounded text-[10px] font-medium bg-gray-100 text-gray-600 border border-gray-200">
                          Đã tính kỳ này
                        </span>
                      )}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap"><span className="text-sm text-gray-600">{asset.ngayCapPhatStr}</span></td>
                    <td className="px-6 py-4 whitespace-nowrap text-right"><span className="text-sm text-gray-900">{formatCurrency(asset.originalValue)}</span></td>
                    <td className="px-6 py-4 whitespace-nowrap text-right"><span className="text-sm text-gray-600">{formatCurrency(asset.accumulatedDepreciation)}</span></td>
                    <td className="px-6 py-4 whitespace-nowrap text-right bg-blue-50/30">
                      <span className={`text-sm font-bold ${asset.isAlreadyDepreciated ? 'text-gray-500 line-through' : 'text-blue-700'}`}>
                        {formatCurrency(asset.monthlyDepreciation)}
                      </span>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
            {tableAssets.length > 0 && selectedAssets.length > 0 && (
              <tfoot className="bg-gray-50 border-t-2 border-gray-300">
                <tr>
                  <td colSpan={6} className="px-6 py-4 text-right font-semibold text-gray-900">
                    Tổng khấu hao dự tính (các mục đã chọn):
                  </td>
                  <td className="px-6 py-4 text-right font-bold text-blue-700 text-lg">
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

              <div>
                <h4 className="font-semibold text-gray-900 mb-3">Chi tiết tài sản được trích khấu hao</h4>
                <div className="space-y-2 max-h-60 overflow-y-auto pr-2">
                  {selectedAssets.map((asset) => (
                    <div key={asset.id} className="flex justify-between items-center p-3 bg-gray-50 rounded border border-gray-100">
                      <div>
                        <p className="text-sm font-medium text-gray-900">{asset.code} - {asset.name}</p>
                        <p className="text-xs text-gray-600">{asset.department} | Ngày cấp: {asset.ngayCapPhatStr}</p>
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