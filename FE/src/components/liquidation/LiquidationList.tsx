import { useState, useEffect } from 'react';
import { Link } from 'react-router';
import { Plus, Search, Filter, Trash2, Eye, Edit, X, Save, Calculator, AlertCircle } from 'lucide-react';
import { toast } from 'sonner';
import { liquidationApi, ThanhLyTaiSan } from '../../api/liquidationApi';
import { assetApi, TaiSan } from '../../api/assetApi';

// 1. SỬA LẠI KEY THÀNH CHUỖI ĐỂ KHỚP VỚI BACKEND ENUM STRING
const statusConfig: Record<string, { label: string; color: string }> = {
  'ChoDuyet': { label: 'Chờ duyệt', color: 'bg-yellow-100 text-yellow-700' },
  'DaDuyet': { label: 'Đã duyệt', color: 'bg-blue-100 text-blue-700' },
  'DaHoanThanh': { label: 'Hoàn thành', color: 'bg-green-100 text-green-700' },
};

export function LiquidationList() {
  const [searchTerm, setSearchTerm] = useState('');
  const [filterStatus, setFilterStatus] = useState<string>('all');
  const [records, setRecords] = useState<ThanhLyTaiSan[]>([]);
  const [assets, setAssets] = useState<TaiSan[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  // Modal states
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const [selectedRecord, setSelectedRecord] = useState<ThanhLyTaiSan | null>(null);
  const [editForm, setEditForm] = useState<Partial<ThanhLyTaiSan>>({});
  const [isSubmitting, setIsSubmitting] = useState(false);

  const fetchData = async () => {
    setIsLoading(true);
    try {
      const [recRes, assetRes] = await Promise.all([liquidationApi.getAll(), assetApi.getAll()]);
      if (recRes.errorCode === 200) setRecords(recRes.data);
      if (assetRes.errorCode === 200) setAssets(assetRes.data);
    } catch (error) {
      toast.error('Lỗi kết nối máy chủ!');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => { fetchData(); }, []);

  const getAsset = (id: number) => assets.find(a => a.id === id);

  const openModal = (record: ThanhLyTaiSan, edit: boolean = false) => {
    setSelectedRecord(record);
    setEditForm({
      ...record,
      ngayThanhLy: record.ngayThanhLy ? record.ngayThanhLy.split('T')[0] : ''
    });
    setIsEditMode(edit);
    setIsModalOpen(true);
  };

  const handleUpdate = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    try {
      const response = await liquidationApi.update(editForm as ThanhLyTaiSan);
      if (response.errorCode === 200) {
        toast.success('Cập nhật thành công!');
        setIsModalOpen(false);
        fetchData();
      } else {
        toast.error(response.message);
      }
    } catch (error) {
      toast.error('Lỗi hệ thống');
    } finally {
      setIsSubmitting(false);
    }
  };

  const formatCurrency = (value?: number) => {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value || 0);
  };

  const filteredRecords = records.filter(r => {
    const asset = getAsset(r.taiSanId);
    const matchesSearch = `${asset?.maTaiSan} ${asset?.tenTaiSan}`.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesStatus = filterStatus === 'all' || r.trangThai?.toString() === filterStatus;
    return matchesSearch && matchesStatus;
  });

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900 text-2xl">Thanh lý - Giảm Tài sản</h1>
          <p className="text-sm text-gray-500">Quản lý vòng đời cuối của tài sản cố định</p>
        </div>
        <Link to="/liquidation/new" className="flex items-center gap-2 px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 shadow-md transition-all">
          <Plus className="w-5 h-5" /> Tạo Phiếu thanh lý
        </Link>
      </div>

      <div className="bg-white rounded-xl border border-gray-200 p-4 shadow-sm">
        <div className="flex flex-col lg:flex-row gap-4">
          <div className="flex-1 relative">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
            <input type="text" placeholder="Tìm mã hoặc tên tài sản..." value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-red-500" />
          </div>
          {/* 2. SỬA CÁC OPTION LỌC THÀNH CHUỖI */}
          <select value={filterStatus} onChange={(e) => setFilterStatus(e.target.value)} className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-red-500">
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
            {isLoading ? (
              <tr><td colSpan={6} className="text-center py-8 text-gray-500">Đang tải dữ liệu...</td></tr>
            ) : filteredRecords.length === 0 ? (
              <tr><td colSpan={6} className="text-center py-8 text-gray-500">Không có dữ liệu thanh lý.</td></tr>
            ) : filteredRecords.map((record) => {
              const asset = getAsset(record.taiSanId);
              
              // 3. AN TOÀN KHI LẤY MÀU STATUS
              const currentStatusStr = record.trangThai?.toString() || 'ChoDuyet';
              const currentStatus = statusConfig[currentStatusStr] || { label: 'Không xác định', color: 'bg-gray-100 text-gray-600' };

              return (
                <tr key={record.id} className="hover:bg-gray-50 transition-colors">
                  <td className="px-6 py-4 text-sm text-gray-600">{record.ngayThanhLy ? new Date(record.ngayThanhLy).toLocaleDateString('vi-VN') : 'N/A'}</td>
                  <td className="px-6 py-4">
                    <p className="text-sm font-bold text-blue-600">{asset?.maTaiSan}</p>
                    <p className="text-xs text-gray-500">{asset?.tenTaiSan}</p>
                  </td>
                  <td className="px-6 py-4 text-right font-medium text-gray-900">{formatCurrency(record.giaTriThanhLy)}</td>
                  <td className={`px-6 py-4 text-right font-bold ${(record.laiLo || 0) >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                    {formatCurrency(record.laiLo)}
                  </td>
                  <td className="px-6 py-4 text-center">
                    <span className={`px-2.5 py-1 rounded-full text-xs font-medium ${currentStatus.color}`}>
                      {currentStatus.label}
                    </span>
                  </td>
                  <td className="px-6 py-4">
                    <div className="flex justify-center gap-2">
                      <button onClick={() => openModal(record, false)} className="p-1.5 text-blue-600 hover:bg-blue-50 rounded-lg"><Eye className="w-4 h-4" /></button>
                      {record.trangThai !== 'DaHoanThanh' && (
                        <button onClick={() => openModal(record, true)} className="p-1.5 text-orange-600 hover:bg-orange-50 rounded-lg"><Edit className="w-4 h-4" /></button>
                      )}
                    </div>
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>

      {/* MODAL 2-TRONG-1 (XEM & SỬA) */}
      {isModalOpen && selectedRecord && (
        <div className="fixed inset-0 bg-gray-900/60 backdrop-blur-sm flex items-center justify-center z-50 p-4 transition-all">
          <div className="bg-white rounded-2xl max-w-2xl w-full max-h-[90vh] overflow-hidden shadow-2xl flex flex-col border border-gray-200">
            <div className="p-6 border-b border-gray-100 flex items-center justify-between bg-white">
              <h3 className="font-bold text-gray-900 text-xl flex items-center gap-2">
                <Trash2 className="w-6 h-6 text-red-600" /> {isEditMode ? 'Cập nhật Thanh lý' : 'Chi tiết Thanh lý'}
              </h3>
              <button onClick={() => setIsModalOpen(false)} className="p-2 hover:bg-gray-100 rounded-full transition-colors"><X className="w-6 h-6 text-gray-400" /></button>
            </div>

            <div className="p-6 overflow-y-auto space-y-6">
              {!isEditMode ? (
                /* CHẾ ĐỘ XEM */
                <div className="space-y-6">
                  <div className="grid grid-cols-2 gap-4 p-4 bg-gray-50 rounded-xl border border-gray-100">
                    <div><p className="text-xs text-gray-500 uppercase font-semibold">Tài sản</p><p className="text-sm font-bold text-blue-700">[{getAsset(selectedRecord.taiSanId)?.maTaiSan}] {getAsset(selectedRecord.taiSanId)?.tenTaiSan}</p></div>
                    <div className="text-right"><p className="text-xs text-gray-500 uppercase font-semibold">Trạng thái</p><span className={`px-2 py-0.5 rounded-full text-xs font-bold ${statusConfig[selectedRecord.trangThai?.toString() || 'ChoDuyet']?.color || 'bg-gray-100'}`}>{statusConfig[selectedRecord.trangThai?.toString() || 'ChoDuyet']?.label || 'Unknown'}</span></div>
                  </div>
                  <div className="grid grid-cols-3 gap-4">
                    <div className="p-3 bg-blue-50 rounded-lg border border-blue-100"><p className="text-xs text-blue-600 font-medium">Nguyên giá</p><p className="font-bold text-blue-900">{formatCurrency(selectedRecord.nguyenGia)}</p></div>
                    <div className="p-3 bg-orange-50 rounded-lg border border-orange-100"><p className="text-xs text-orange-600 font-medium">KH Lũy kế</p><p className="font-bold text-orange-900">{formatCurrency(selectedRecord.khauHaoLuyKe)}</p></div>
                    <div className="p-3 bg-green-50 rounded-lg border border-green-100"><p className="text-xs text-green-600 font-medium">Giá trị còn lại</p><p className="font-bold text-green-900">{formatCurrency(selectedRecord.giaTriConLai)}</p></div>
                  </div>
                  <div className="p-4 bg-red-50 rounded-xl border border-red-100">
                    <div className="flex justify-between items-center mb-2"><p className="text-sm font-semibold text-red-800">Kết quả thanh lý</p><p className="text-xs text-red-500 font-medium">{selectedRecord.ngayThanhLy ? new Date(selectedRecord.ngayThanhLy).toLocaleDateString('vi-VN') : ''}</p></div>
                    <div className="flex justify-between border-t border-red-200 pt-2"><span>Giá thu hồi:</span><span className="font-bold">{formatCurrency(selectedRecord.giaTriThanhLy)}</span></div>
                    <div className="flex justify-between mt-1"><span>Lãi / (Lỗ):</span><span className={`font-black ${(selectedRecord.laiLo || 0) >= 0 ? 'text-green-600' : 'text-red-600'}`}>{formatCurrency(selectedRecord.laiLo)}</span></div>
                  </div>
                </div>
              ) : (
                /* CHẾ ĐỘ SỬA */
                <form id="edit-liquidation" onSubmit={handleUpdate} className="grid grid-cols-2 gap-5">
                  <div className="col-span-2 p-3 bg-yellow-50 border border-yellow-200 rounded-lg flex items-center gap-3">
                    <AlertCircle className="w-5 h-5 text-yellow-600" />
                    <p className="text-xs text-yellow-800">Lưu ý: Nếu chuyển trạng thái sang <b>Hoàn thành</b>, hệ thống sẽ tự động sinh chứng từ kế toán và không thể sửa lại.</p>
                  </div>
                  <div className="col-span-1">
                    <label className="block text-sm font-medium text-gray-700 mb-1">Ngày thanh lý</label>
                    <input type="date" value={editForm.ngayThanhLy || ''} onChange={e => setEditForm({...editForm, ngayThanhLy: e.target.value})} className="w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-red-500" required />
                  </div>
                  <div className="col-span-1">
                    {/* 4. SỬA LẠI OPTION FORM CẬP NHẬT THEO CHUỖI */}
                    <label className="block text-sm font-medium text-gray-700 mb-1">Trạng thái</label>
                    <select value={editForm.trangThai || 'ChoDuyet'} onChange={e => setEditForm({...editForm, trangThai: e.target.value})} className="w-full px-3 py-2 border rounded-lg bg-white">
                      <option value="ChoDuyet">Chờ duyệt</option>
                      <option value="DaDuyet">Đã duyệt</option>
                      <option value="DaHoanThanh">Hoàn thành (Ghi sổ)</option>
                    </select>
                  </div>
                  <div className="col-span-2">
                    <label className="block text-sm font-medium text-gray-700 mb-1">Giá thanh lý (VNĐ)</label>
                    <input type="number" value={editForm.giaTriThanhLy || ''} onChange={e => setEditForm({...editForm, giaTriThanhLy: Number(e.target.value)})} className="w-full px-3 py-2 border rounded-lg" required />
                  </div>
                  <div className="col-span-2">
                    <label className="block text-sm font-medium text-gray-700 mb-1">Lý do</label>
                    <textarea value={editForm.lyDo || ''} onChange={e => setEditForm({...editForm, lyDo: e.target.value})} rows={2} className="w-full px-3 py-2 border rounded-lg" />
                  </div>
                </form>
              )}
            </div>

            <div className="p-4 border-t border-gray-100 bg-gray-50 flex justify-end gap-3">
              {isEditMode ? (
                <>
                  <button type="button" onClick={() => setIsEditMode(false)} className="px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-100">Hủy</button>
                  <button type="submit" form="edit-liquidation" disabled={isSubmitting} className="flex items-center gap-2 px-6 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 shadow-lg disabled:opacity-50">
                    <Save className="w-4 h-4" /> {isSubmitting ? 'Đang lưu...' : 'Lưu & Sinh chứng từ'}
                  </button>
                </>
              ) : (
                <>
                  <button onClick={() => setIsModalOpen(false)} className="px-6 py-2 bg-gray-200 text-gray-800 rounded-lg hover:bg-gray-300">Đóng</button>
                  {selectedRecord.trangThai !== 'DaHoanThanh' && (
                    <button onClick={() => setIsEditMode(true)} className="flex items-center gap-2 px-6 py-2 bg-orange-600 text-white rounded-lg hover:bg-orange-700"><Edit className="w-4 h-4" /> Sửa phiếu</button>
                  )}
                </>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  );
}