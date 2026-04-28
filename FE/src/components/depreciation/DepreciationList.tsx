import { useState, useEffect, useMemo } from 'react';
import { Calculator, FileText, Download, AlertCircle, Loader2, CheckCircle, RefreshCw } from 'lucide-react';
import { toast } from "sonner";
import { useGlobalData } from '../../context/GlobalContext';
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
  isAlreadyDepreciated: boolean;
}

// Giữ lại Cache cục bộ CHỈ DÀNH RIÊNG cho lịch sử khấu hao 
// (Vì Tài sản và Phòng ban đã có GlobalContext lo)
let cachedHistory: LichSuKhauHao[] | null = null;

export function DepreciationList() {
  // 1. Lấy Tài sản và Phòng ban siêu tốc từ kho chung (Giống hệt trang AssetList)
  const { assets: rawAssets, departments, isLoadingGlobal, refreshData } = useGlobalData();

  const [selectedMonth, setSelectedMonth] = useState(() => {
    const date = new Date();
    return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}`;
  });
  
  const [showVoucher, setShowVoucher] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  // State quản lý riêng cho Lịch sử khấu hao
  const [history, setHistory] = useState<LichSuKhauHao[]>(cachedHistory || []);
  const [isLoadingHistory, setIsLoadingHistory] = useState(!cachedHistory);
  const [manualSelection, setManualSelection] = useState<Record<number, boolean>>({});

  const getMaxMonth = () => {
    const today = new Date();
    return `${today.getFullYear()}-${String(today.getMonth() + 1).padStart(2, '0')}`;
  };

  // 2. Chỉ tải duy nhất Lịch sử khấu hao (nhẹ hơn rất nhiều)
  const fetchHistory = async (forceRefresh = false) => {
    if (!forceRefresh && cachedHistory) {
      setHistory(cachedHistory);
      return;
    }
    setIsLoadingHistory(true);
    try {
      const res = await depreciationHistoryApi.getAll();
      if (res.errorCode === 200) {
        cachedHistory = res.data;
        setHistory(res.data);
      }
    } catch (error) {
      toast.error('Lỗi tải lịch sử khấu hao.');
    } finally {
      setIsLoadingHistory(false);
    }
  };

  useEffect(() => {
    fetchHistory();
  }, []);

  // ==========================================
  // 3. THUẬT TOÁN TÍNH TOÁN (DÙNG USE_MEMO ĐỂ KHÔNG BỊ KHỰNG TRANG)
  // useMemo sẽ tính toán ngay lập tức khi render, không tạo ra khoảng trễ như useEffect
  // ==========================================
  const tableAssets = useMemo(() => {
    if (!rawAssets || rawAssets.length === 0) return [];

    const [selYear, selMonth] = selectedMonth.split('-').map(Number);
    const daysInSelectedMonth = new Date(selYear, selMonth, 0).getDate();
    const endOfSelMonth = new Date(selYear, selMonth - 1, daysInSelectedMonth, 23, 59, 59);

    // TỐI ƯU HÓA: Tạo Set() để tra cứu lịch sử cực nhanh, bỏ vòng lặp lồng nhau
    const depreciatedAssetIdsThisMonth = new Set(
      history
        .filter(h => h.kyKhauHao === selectedMonth)
        .map(h => h.taiSanId)
    );

    return rawAssets
      .filter(asset => {
        const status = asset.trangThai?.toString();
        if (status !== '2' && status !== 'DangSuDung') return false; 
        if (!asset.ngayCapPhat) return false;
        
        const capPhatDate = new Date(asset.ngayCapPhat);
        if (capPhatDate > endOfSelMonth) return false;
        if ((asset.giaTriConLai || 0) <= 0) return false;

        return true; 
      })
      .map(asset => {
        // Tra cứu nhanh bằng Set thay vì dùng history.some()
        const isAlreadyDepreciated = depreciatedAssetIdsThisMonth.has(asset.id!);

        let standardMonthlyAmt = asset.khauHaoHangThang || 0;
        if (standardMonthlyAmt === 0 && asset.nguyenGia && asset.thoiGianKhauHao) {
          standardMonthlyAmt = asset.nguyenGia / asset.thoiGianKhauHao;
        }

        let finalMonthlyAmt = standardMonthlyAmt;
        const capPhatDate = new Date(asset.ngayCapPhat!);

        if (capPhatDate.getFullYear() === selYear && (capPhatDate.getMonth() + 1) === selMonth) {
          const usedDays = daysInSelectedMonth - capPhatDate.getDate() + 1; 
          finalMonthlyAmt = (standardMonthlyAmt / daysInSelectedMonth) * usedDays;
        }

        const remaining = asset.giaTriConLai || 0;
        if (finalMonthlyAmt > remaining) {
            finalMonthlyAmt = remaining;
        }

        // Ưu tiên trạng thái chọn thủ công (manualSelection), nếu không thì mặc định chọn những cái chưa khấu hao
        const isSelected = manualSelection[asset.id!] !== undefined 
            ? manualSelection[asset.id!] 
            : !isAlreadyDepreciated;

        return {
          id: asset.id!,
          code: asset.maTaiSan,
          name: asset.tenTaiSan,
          department: departments.find((d: any) => d.id === asset.phongBanId)?.tenPhongBan || 'Chưa cấp phát',
          originalValue: asset.nguyenGia || 0,
          accumulatedDepreciation: asset.khauHaoLuyKe || 0,
          monthlyDepreciation: Math.round(finalMonthlyAmt),
          remainingValue: remaining,
          selected: isSelected, 
          ngayCapPhatStr: capPhatDate.toLocaleDateString('vi-VN'),
          isAlreadyDepreciated 
        };
      });
  }, [rawAssets, history, selectedMonth, departments, manualSelection]);

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
  };

  const toggleSelect = (id: number) => {
    const asset = tableAssets.find(a => a.id === id);
    if (asset && !asset.isAlreadyDepreciated) {
      setManualSelection(prev => ({ ...prev, [id]: !asset.selected }));
    }
  };

  const toggleSelectAll = () => {
    const selectableAssets = tableAssets.filter(a => !a.isAlreadyDepreciated);
    if (selectableAssets.length === 0) return;

    const allSelected = selectableAssets.every(a => a.selected);
    const newSelection = { ...manualSelection };
    
    selectableAssets.forEach(asset => {
      newSelection[asset.id] = !allSelected;
    });
    
    setManualSelection(newSelection);
  };

  const selectedAssets = tableAssets.filter(a => a.selected && !a.isAlreadyDepreciated);
  const totalDepreciation = selectedAssets.reduce((sum, a) => sum + a.monthlyDepreciation, 0);

  // Gộp chung hàm làm mới cả hệ thống lẫn lịch sử
  const handleRefreshAll = async () => {
    await Promise.all([
      refreshData(), // Cập nhật lại kho Tài sản & Phòng ban
      fetchHistory(true) // Cập nhật lại kho Lịch sử Khấu hao
    ]);
  };

  const handleGhiSo = async () => {
    if (selectedAssets.length === 0) {
      toast.error('Vui lòng chọn ít nhất 1 tài sản để khấu hao.');
      return;
    }

    setIsSubmitting(true);
    try {
      const payload = {
        kyKhauHao: selectedMonth,
        danhSachTaiSan: selectedAssets.map(asset => ({
          taiSanId: asset.id,
          soTien: asset.monthlyDepreciation
        }))
      };

      const response = await depreciationHistoryApi.createBulk(payload);
      
      if (response.errorCode === 200) {
        toast.success(`Đã ghi sổ Chứng từ tổng cho ${selectedAssets.length} tài sản!`);
        setShowVoucher(false);
        setManualSelection({}); // Reset lựa chọn
        handleRefreshAll(); // Ép làm mới toàn bộ để khóa các checkbox lại
      } else {
        toast.error(response.message || 'Lỗi khi ghi sổ khấu hao.');
      }
    } catch (error) {
      toast.error('Lỗi hệ thống khi ghi sổ khấu hao.');
    } finally {
      setIsSubmitting(false);
    }
  };

  // Cờ trạng thái loading tổng
  const isScreenLoading = isLoadingGlobal || isLoadingHistory;

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Khấu hao Tài sản</h1>
          <p className="text-sm text-gray-500 mt-1">Tính và ghi nhận khấu hao (Tự động chia lẻ theo ngày cấp phát)</p>
        </div>
        <button 
          onClick={handleRefreshAll}
          disabled={isScreenLoading}
          className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-gray-600 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors disabled:opacity-50"
        >
          <RefreshCw className={`w-4 h-4 ${isScreenLoading ? 'animate-spin text-blue-600' : ''}`} />
          Làm mới dữ liệu
        </button>
      </div>

      <div className="bg-white rounded-lg border border-gray-200 p-6">
        <div className="flex flex-col lg:flex-row gap-4 items-start lg:items-end">
          <div className="flex-1">
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Chọn kỳ khấu hao (Tháng/Năm)
            </label>
            <input
              type="month"
              value={selectedMonth}
              max={getMaxMonth()}
              onChange={(e) => {
                setSelectedMonth(e.target.value);
                setManualSelection({}); // Reset checkbox khi đổi tháng
              }}
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
              disabled={isScreenLoading || tableAssets.length === 0 || selectedAssets.length === 0}
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

      <div className="bg-blue-50 border border-blue-200 rounded-lg p-4 flex items-start gap-3">
        <AlertCircle className="w-5 h-5 text-blue-600 mt-0.5" />
        <div className="flex-1">
          <p className="text-sm text-blue-900 font-medium">Thông tin khấu hao tháng {selectedMonth}</p>
          <p className="text-sm text-blue-700 mt-1">
            Hệ thống tự động lọc ra các tài sản đang hoạt động. <b>Lưu ý:</b> Tài sản mới cấp phát trong tháng này sẽ được tính khấu hao lẻ theo số ngày sử dụng thực tế.
          </p>
        </div>
      </div>

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
              {isScreenLoading && tableAssets.length === 0 ? (
                <tr><td colSpan={7} className="text-center py-6 text-gray-500">Đang tải dữ liệu...</td></tr>
              ) : tableAssets.length === 0 ? (
                <tr>
                  <td colSpan={7} className="text-center py-6 text-gray-500">
                    Không có tài sản nào cần khấu hao trong tháng {selectedMonth}. 
                  </td>
                </tr>
              ) : (
                tableAssets.map((asset) => (
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

      {showVoucher && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-lg max-w-3xl w-full max-h-[90vh] overflow-auto shadow-2xl">
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
                className="px-4 py-2 border border-gray-300 bg-white rounded-lg hover:bg-gray-100 transition-colors font-medium text-gray-700"
              >
                Hủy
              </button>
              <button 
                onClick={handleGhiSo}
                disabled={isSubmitting}
                className="flex items-center gap-2 px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors disabled:opacity-70 font-medium"
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