import { useState, useEffect, useMemo } from 'react';
import { Link } from 'react-router';
import { Plus, Search, Trash2, Eye, Edit, X, Save, AlertCircle, CheckCircle, FileCheck, RefreshCw } from 'lucide-react';
import { toast } from 'sonner';
import { liquidationApi, ThanhLyTaiSan } from '../../api/liquidationApi';
import { useGlobalData } from '../../context/GlobalContext'; // <-- IMPORT GLOBAL CONTEXT

const statusConfig: Record<string, { label: string; color: string }> = {
  'ChoDuyet': { label: 'Chờ duyệt', color: 'bg-yellow-100 text-yellow-700 border-yellow-200' },
  'DaDuyet': { label: 'Đã duyệt', color: 'bg-blue-100 text-blue-700 border-blue-200' },
  'DaHoanThanh': { label: 'Hoàn thành', color: 'bg-green-100 text-green-700 border-green-200' },
};

// ==========================================
// 1. BIẾN CACHE CỤC BỘ TRÊN RAM (Chỉ lưu Phiếu thanh lý)
// ==========================================
let cachedLiquidationRecords: ThanhLyTaiSan[] | null = null;

export function LiquidationList() {
  // 2. Móc Tài sản từ KHO CHUNG (Tải 0 giây)
  const { assets, isLoadingGlobal, refreshData } = useGlobalData();

  const [searchTerm, setSearchTerm] = useState('');
  const [filterStatus, setFilterStatus] = useState<string>('all');
  
  // State quản lý Phiếu thanh lý
  const [records, setRecords] = useState<ThanhLyTaiSan[]>(cachedLiquidationRecords || []);
  const [isLoadingRecords, setIsLoadingRecords] = useState(!cachedLiquidationRecords);

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const [selectedRecord, setSelectedRecord] = useState<ThanhLyTaiSan | null>(null);
  const [editForm, setEditForm] = useState<Partial<ThanhLyTaiSan>>({});
  const [isSubmitting, setIsSubmitting] = useState(false);

  // ==========================================
  // 3. HÀM TẢI PHIẾU THANH LÝ CÓ TÍCH HỢP CACHE
  // ==========================================
  const fetchRecords = async (forceRefresh = false) => {
    if (!forceRefresh && cachedLiquidationRecords) {
      setRecords(cachedLiquidationRecords);
      return;
    }

    setIsLoadingRecords(true);
    try {
      const res = await liquidationApi.getAll();
      if (res.errorCode === 200) {
        cachedLiquidationRecords = res.data;
        setRecords(res.data);
      }
    } catch (error) {
      toast.error('Lỗi kết nối máy chủ!');
    } finally {
      setIsLoadingRecords(false);
    }
  };

  useEffect(() => { 
    fetchRecords(); 
  }, []);

  // Hàm gộp chung làm mới toàn bộ hệ thống
  const handleRefreshAll = async () => {
    await Promise.all([
      refreshData(),      // Làm mới Tài sản từ Global
      fetchRecords(true)  // Làm mới Phiếu thanh lý
    ]);
  };

  const getAsset = (id: number) => assets.find((a: any) => a.id === id);

  const openModal = (record: ThanhLyTaiSan, edit: boolean = false) => {
    setSelectedRecord(record);
    setEditForm({
      ...record,
      ngayThanhLy: record.ngayThanhLy ? record.ngayThanhLy.split('T')[0] : ''
    });
    setIsEditMode(edit);
    setIsModalOpen(true);
  };

  // Hàm cập nhật thông tin thông thường (Không đổi trạng thái)
  const handleUpdateInfo = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    try {
      const response = await liquidationApi.update(editForm as ThanhLyTaiSan);
      if (response.errorCode === 200) {
        toast.success('Cập nhật thông tin thành công!');
        setIsModalOpen(false);
        handleRefreshAll(); // Ép hệ thống làm mới dữ liệu
      } else {
        toast.error(response.message);
      }
    } catch (error) {
      toast.error('Lỗi hệ thống');
    } finally {
      setIsSubmitting(false);
    }
  };

  // === HÀM XỬ LÝ LUỒNG DUYỆT (WORKFLOW) ===
  const handleUpdateStatus = async (record: ThanhLyTaiSan, newStatus: string, actionName: string) => {
    if (!window.confirm(`Bạn có chắc chắn muốn ${actionName} tài sản này?`)) return;

    setIsSubmitting(true);
    try {
      const payload = { ...record, trangThai: newStatus };
      const response = await liquidationApi.update(payload);
      
      if (response.errorCode === 200) {
        toast.success(`${actionName} thành công!`);
        setIsModalOpen(false);
        handleRefreshAll(); // Ép hệ thống làm mới dữ liệu
      } else {
        toast.error(response.message);
      }
    } catch (error) {
      toast.error('Lỗi kết nối máy chủ khi duyệt.');
    } finally {
      setIsSubmitting(false);
    }
  };

  const formatCurrency = (value?: number) => {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value || 0);
  };

  // ==========================================
  // 4. USE_MEMO BỘ LỌC ĐỂ RENDER MƯỢT MÀ
  // ==========================================
  const filteredRecords = useMemo(() => {
    return records.filter(r => {
      const asset = getAsset(r.taiSanId);
      const matchesSearch = `${asset?.maTaiSan} ${asset?.tenTaiSan}`.toLowerCase().includes(searchTerm.toLowerCase());
      const matchesStatus = filterStatus === 'all' || r.trangThai?.toString() === filterStatus;
      return matchesSearch && matchesStatus;
    });
  }, [records, assets, searchTerm, filterStatus]);

  const isScreenLoading = isLoadingGlobal || isLoadingRecords;

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900 text-2xl">Thanh lý - Giảm Tài sản</h1>
          <p className="text-sm text-gray-500">Quản lý luồng duyệt thanh lý tài sản cố định</p>
        </div>
        
        <div className="flex gap-2">
          {/* Nút Làm mới */}
          <button 
            onClick={handleRefreshAll}
            disabled={isScreenLoading}
            className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-gray-600 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors disabled:opacity-50 shadow-sm"
            title="Tải lại dữ liệu"
          >
            <RefreshCw className={`w-4 h-4 ${isScreenLoading ? 'animate-spin text-blue-600' : ''}`} />
            <span className="hidden sm:block">Làm mới</span>
          </button>

          <Link to="/liquidation/new" className="flex items-center gap-2 px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 shadow-md transition-all">
            <Plus className="w-5 h-5" /> Tạo Phiếu thanh lý
          </Link>
        </div>
      </div>

      <div className="bg-white rounded-xl border border-gray-200 p-4 shadow-sm">
        <div className="flex flex-col lg:flex-row gap-4">
          <div className="flex-1 relative">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
            <input type="text" placeholder="Tìm mã hoặc tên tài sản..." value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-red-500" />
          </div>
          <select value={filterStatus} onChange={(e) => setFilterStatus(e.target.value)} className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-red-500 font-medium text-gray-700">
            <option value="all">Tất cả trạng thái</option>
            <option value="ChoDuyet">Chờ duyệt</option>
            <option value="DaDuyet">Đã duyệt</option>
            <option value="DaHoanThanh">Hoàn thành</option>
          </select>
        </div>
      </div>

      {/* Table */}
      <div className="bg-white rounded-xl border border-gray-200 overflow-hidden shadow-sm">
        <table className="w-full text-left border-collapse">
          <thead className="bg-gray-50 border-b border-gray-200">
            <tr>
              <th className="px-6 py-4 text-xs font-semibold text-gray-600 uppercase">Ngày TL</th>
              <th className="px-6 py-4 text-xs font-semibold text-gray-600 uppercase">Tài sản</th>
              <th className="px-6 py-4 text-right text-xs font-semibold text-gray-600 uppercase">Giá TL</th>
              <th className="px-6 py-4 text-right text-xs font-semibold text-gray-600 uppercase">Lãi/Lỗ</th>
              <th className="px-6 py-4 text-center text-xs font-semibold text-gray-600 uppercase">Trạng thái</th>
              <th className="px-6 py-4 text-center text-xs font-semibold text-gray-600 uppercase">Thao tác</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-100">
            {isScreenLoading && filteredRecords.length === 0 ? (
              <tr><td colSpan={6} className="text-center py-8 text-gray-500">Đang tải dữ liệu...</td></tr>
            ) : filteredRecords.length === 0 ? (
              <tr><td colSpan={6} className="text-center py-8 text-gray-500">Không có dữ liệu thanh lý.</td></tr>
            ) : filteredRecords.map((record) => {
              const asset = getAsset(record.taiSanId);
              const currentStatusStr = record.trangThai?.toString() || 'ChoDuyet';
              const currentStatus = statusConfig[currentStatusStr] || { label: 'Không xác định', color: 'bg-gray-100 text-gray-600' };

              return (
                <tr key={record.id} className="hover:bg-gray-50 transition-colors">
                  <td className="px-6 py-4 text-sm font-medium text-gray-700">{record.ngayThanhLy ? new Date(record.ngayThanhLy).toLocaleDateString('vi-VN') : 'N/A'}</td>
                  <td className="px-6 py-4">
                    <p className="text-sm font-bold text-blue-600">{asset?.maTaiSan}</p>
                    <p className="text-xs font-medium text-gray-500 mt-0.5">{asset?.tenTaiSan}</p>
                  </td>
                  <td className="px-6 py-4 text-right font-bold text-gray-900">{formatCurrency(record.giaTriThanhLy)}</td>
                  <td className={`px-6 py-4 text-right font-bold ${(record.laiLo || 0) >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                    {formatCurrency(record.laiLo)}
                  </td>
                  <td className="px-6 py-4 text-center">
                    <span className={`px-2.5 py-1 rounded border text-[11px] font-bold uppercase tracking-wide ${currentStatus.color}`}>
                      {currentStatus.label}
                    </span>
                  </td>
                  <td className="px-6 py-4">
                    <div className="flex justify-center gap-2">
                      <button onClick={() => openModal(record, false)} className="p-1.5 text-blue-600 hover:bg-blue-100 rounded-lg transition-colors" title="Xem chi tiết & Duyệt">
                        <Eye className="w-4 h-4" />
                      </button>
                    </div>
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>

      {/* MODAL CHI TIẾT & DUYỆT */}
      {isModalOpen && selectedRecord && (
        <div className="fixed inset-0 bg-gray-900/60 backdrop-blur-sm flex items-center justify-center z-50 p-4 transition-all">
          <div className="bg-white rounded-2xl max-w-2xl w-full max-h-[90vh] overflow-hidden shadow-2xl flex flex-col border border-gray-200">
            <div className="p-6 border-b border-gray-100 flex items-center justify-between bg-white">
              <h3 className="font-bold text-gray-900 text-xl flex items-center gap-2">
                <Trash2 className="w-6 h-6 text-red-600" /> {isEditMode ? 'Sửa thông tin phiếu' : 'Xử lý Phiếu Thanh lý'}
              </h3>
              <button onClick={() => setIsModalOpen(false)} className="p-2 hover:bg-gray-100 rounded-full transition-colors"><X className="w-6 h-6 text-gray-400" /></button>
            </div>

            <div className="p-6 overflow-y-auto space-y-6">
              {!isEditMode ? (
                /* CHẾ ĐỘ XEM & XỬ LÝ WORKFLOW */
                <div className="space-y-6">
                  {/* Trạng thái Workflow */}
                  <div className={`p-4 rounded-xl border flex items-center justify-between ${statusConfig[selectedRecord.trangThai?.toString() || 'ChoDuyet']?.color}`}>
                    <div className="flex items-center gap-2">
                      <FileCheck className="w-5 h-5" />
                      <span className="font-bold text-sm uppercase tracking-wide">
                        Trạng thái hiện tại: {statusConfig[selectedRecord.trangThai?.toString() || 'ChoDuyet']?.label}
                      </span>
                    </div>
                    {selectedRecord.trangThai === 'DaHoanThanh' && <span className="text-xs font-semibold">(Đã sinh chứng từ)</span>}
                  </div>

                  <div className="grid grid-cols-2 gap-4 p-4 bg-gray-50 rounded-xl border border-gray-100">
                    <div className="col-span-2">
                      <p className="text-xs text-gray-500 uppercase font-semibold mb-1">Tài sản cần thanh lý</p>
                      <p className="text-base font-bold text-blue-700">[{getAsset(selectedRecord.taiSanId)?.maTaiSan}] {getAsset(selectedRecord.taiSanId)?.tenTaiSan}</p>
                    </div>
                    <div>
                      <p className="text-xs text-gray-500 uppercase font-semibold mb-1">Lý do thanh lý</p>
                      <p className="font-medium text-gray-900">{selectedRecord.lyDo}</p>
                    </div>
                    <div>
                      <p className="text-xs text-gray-500 uppercase font-semibold mb-1">Ghi chú</p>
                      <p className="font-medium text-gray-900">{selectedRecord.ghiChu || '-'}</p>
                    </div>
                  </div>

                  <div className="grid grid-cols-3 gap-4">
                    <div className="p-3 bg-blue-50 rounded-lg border border-blue-100"><p className="text-xs text-blue-600 font-medium">Nguyên giá</p><p className="font-bold text-blue-900">{formatCurrency(selectedRecord.nguyenGia)}</p></div>
                    <div className="p-3 bg-orange-50 rounded-lg border border-orange-100"><p className="text-xs text-orange-600 font-medium">KH Lũy kế</p><p className="font-bold text-orange-900">{formatCurrency(selectedRecord.khauHaoLuyKe)}</p></div>
                    <div className="p-3 bg-gray-100 rounded-lg border border-gray-200"><p className="text-xs text-gray-600 font-medium">Giá trị còn lại</p><p className="font-bold text-gray-900">{formatCurrency(selectedRecord.giaTriConLai)}</p></div>
                  </div>

                  <div className="p-4 bg-red-50 rounded-xl border border-red-100">
                    <div className="flex justify-between items-center mb-2">
                      <p className="text-sm font-bold text-red-800">Thông tin Hạch toán dự kiến</p>
                      <p className="text-xs text-red-600 font-bold bg-red-100 px-2 py-1 rounded">Ngày TL: {selectedRecord.ngayThanhLy ? new Date(selectedRecord.ngayThanhLy).toLocaleDateString('vi-VN') : ''}</p>
                    </div>
                    <div className="flex justify-between border-t border-red-200 pt-2 mt-2">
                      <span className="text-red-800 font-medium">Giá thu hồi (Bán ra):</span>
                      <span className="font-bold text-red-700 text-lg">{formatCurrency(selectedRecord.giaTriThanhLy)}</span>
                    </div>
                    <div className="flex justify-between mt-1">
                      <span className="text-red-800 font-medium">Lãi / (Lỗ):</span>
                      <span className={`font-black text-lg ${(selectedRecord.laiLo || 0) >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                        {formatCurrency(selectedRecord.laiLo)}
                      </span>
                    </div>
                  </div>
                </div>
              ) : (
                /* CHẾ ĐỘ SỬA (Chỉ cho phép sửa nội dung, KHÔNG CHO CHỌN TRẠNG THÁI) */
                <form id="edit-liquidation" onSubmit={handleUpdateInfo} className="grid grid-cols-2 gap-5">
                  <div className="col-span-2 p-3 bg-blue-50 border border-blue-200 rounded-lg flex items-center gap-3">
                    <AlertCircle className="w-5 h-5 text-blue-600 shrink-0" />
                    <p className="text-xs text-blue-800">Trạng thái phiếu sẽ được cập nhật thông qua các nút Duyệt. Ở đây bạn chỉ có thể sửa thông tin nội dung.</p>
                  </div>
                  <div className="col-span-1">
                    <label className="block text-sm font-medium text-gray-700 mb-1">Ngày thanh lý</label>
                    <input type="date" value={editForm.ngayThanhLy || ''} onChange={e => setEditForm({...editForm, ngayThanhLy: e.target.value})} className="w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-red-500" required />
                  </div>
                  <div className="col-span-1">
                    <label className="block text-sm font-medium text-gray-700 mb-1">Giá thanh lý (VNĐ)</label>
                    <input type="number" value={editForm.giaTriThanhLy || ''} onChange={e => setEditForm({...editForm, giaTriThanhLy: Number(e.target.value)})} className="w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-red-500" required />
                  </div>
                  <div className="col-span-2">
                    <label className="block text-sm font-medium text-gray-700 mb-1">Lý do thanh lý</label>
                    <select value={editForm.lyDo || ''} onChange={e => setEditForm({...editForm, lyDo: e.target.value})} className="w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-red-500 bg-white" required>
                      <option value="">Chọn lý do</option>
                      <option value="Hết thời gian sử dụng">Hết thời gian sử dụng</option>
                      <option value="Hỏng không sửa được">Hỏng không sửa được</option>
                      <option value="Nâng cấp thiết bị mới">Nâng cấp thiết bị mới</option>
                      <option value="Lý do khác">Lý do khác</option>
                    </select>
                  </div>
                  <div className="col-span-2">
                    <label className="block text-sm font-medium text-gray-700 mb-1">Ghi chú thêm</label>
                    <textarea value={editForm.ghiChu || ''} onChange={e => setEditForm({...editForm, ghiChu: e.target.value})} rows={2} className="w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-red-500" />
                  </div>
                </form>
              )}
            </div>

            {/* FOOTER - NƠI CHỨA CÁC NÚT ĐIỀU KHIỂN LUỒNG */}
            <div className="p-4 border-t border-gray-200 bg-gray-50 flex items-center justify-between">
              {isEditMode ? (
                <>
                  <button type="button" onClick={() => setIsEditMode(false)} className="px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-200 font-medium text-gray-700 transition-colors">Quay lại xem</button>
                  <button type="submit" form="edit-liquidation" disabled={isSubmitting} className="flex items-center gap-2 px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 shadow-md disabled:opacity-50 transition-colors font-medium">
                    <Save className="w-4 h-4" /> Lưu thông tin
                  </button>
                </>
              ) : (
                <>
                  <button onClick={() => setIsModalOpen(false)} className="px-6 py-2 bg-gray-200 text-gray-800 font-medium rounded-lg hover:bg-gray-300 transition-colors">Đóng</button>
                  
                  <div className="flex gap-3">
                    {/* NẾU ĐANG CHỜ DUYỆT: CÓ THỂ SỬA HOẶC BẤM DUYỆT */}
                    {selectedRecord.trangThai === 'ChoDuyet' && (
                      <>
                        <button onClick={() => setIsEditMode(true)} className="flex items-center gap-2 px-4 py-2 bg-white border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-100 font-medium transition-colors">
                          <Edit className="w-4 h-4" /> Sửa thông tin
                        </button>
                        <button 
                          disabled={isSubmitting}
                          onClick={() => handleUpdateStatus(selectedRecord, 'DaDuyet', 'Duyệt')} 
                          className="flex items-center gap-2 px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 font-medium shadow-md transition-colors"
                        >
                          <CheckCircle className="w-5 h-5" /> Duyệt phiếu
                        </button>
                      </>
                    )}

                    {/* NẾU ĐÃ DUYỆT: BẤM HOÀN THÀNH ĐỂ GHI SỔ */}
                    {selectedRecord.trangThai === 'DaDuyet' && (
                      <button 
                        disabled={isSubmitting}
                        onClick={() => handleUpdateStatus(selectedRecord, 'DaHoanThanh', 'Hoàn thành và Ghi sổ')} 
                        className="flex items-center gap-2 px-6 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 font-medium shadow-md transition-colors"
                      >
                        <FileCheck className="w-5 h-5" /> Hoàn thành & Sinh chứng từ
                      </button>
                    )}
                  </div>
                </>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  );
}