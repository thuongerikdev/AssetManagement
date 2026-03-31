import { useState, useEffect } from 'react';
import { Link } from 'react-router';
import { Plus, Search, Filter, Wrench, Calendar, Trash2, Eye, X, Edit, Save } from 'lucide-react';
import { toast } from 'sonner';
import { maintenanceApi, BaoTriTaiSan } from '../../api/maintenanceApi';
import { assetApi, TaiSan } from '../../api/assetApi';

// Map Enum trạng thái
const statusConfig: Record<number, { label: string; color: string }> = {
  0: { label: 'Chờ xử lý', color: 'bg-yellow-100 text-yellow-700' },
  1: { label: 'Đang thực hiện', color: 'bg-blue-100 text-blue-700' },
  2: { label: 'Hoàn thành', color: 'bg-green-100 text-green-700' },
};

// Map Enum Loại bảo trì
const loaiBaoTriConfig: Record<string, string> = {
  '0': 'Bảo trì định kỳ',
  '1': 'Sửa chữa',
  '2': 'Nâng cấp',
  '3': 'Vệ sinh',
  '4': 'Kiểm tra'
};

// Map Enum Loại chi phí
const costTypeConfig: Record<string, { label: string; color: string }> = {
  'sua_chua': { label: 'Sửa chữa', color: 'text-orange-600' },
  'nang_cap': { label: 'Nâng cấp', color: 'text-blue-600' },
};

export function MaintenanceList() {
  const [searchTerm, setSearchTerm] = useState('');
  const [filterStatus, setFilterStatus] = useState<string>('all');
  
  const [records, setRecords] = useState<BaoTriTaiSan[]>([]);
  const [assets, setAssets] = useState<TaiSan[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  // States cho Modal 2-trong-1 (Xem & Sửa)
  const [isViewModalOpen, setIsViewModalOpen] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const [selectedRecord, setSelectedRecord] = useState<BaoTriTaiSan | null>(null);
  
  // States cho Form trong Modal
  const [editForm, setEditForm] = useState<Partial<BaoTriTaiSan>>({});
  const [modalHasCost, setModalHasCost] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const fetchData = async () => {
    setIsLoading(true);
    try {
      const [recRes, assetRes] = await Promise.all([
        maintenanceApi.getAll(),
        assetApi.getAll()
      ]);
      if (recRes.errorCode === 200) setRecords(recRes.data);
      if (assetRes.errorCode === 200) setAssets(assetRes.data);
    } catch (error) {
      toast.error('Lỗi tải dữ liệu bảo trì.');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleDelete = async (id: number) => {
    if (window.confirm('Bạn có chắc chắn muốn xóa phiếu bảo trì này?')) {
      try {
        const response = await maintenanceApi.delete(id);
        if (response.errorCode === 200) {
          toast.success('Xóa phiếu thành công');
          fetchData();
        } else {
          toast.error(response.message || 'Lỗi khi xóa');
        }
      } catch (error) {
        toast.error('Lỗi kết nối máy chủ');
      }
    }
  };

  // Mở modal (Có thể chọn mở ở chế độ Xem hoặc Sửa)
  const openModal = (record: BaoTriTaiSan, editMode: boolean = false) => {
    setSelectedRecord(record);
    setEditForm({
      ...record,
      ngayThucHien: record.ngayThucHien ? record.ngayThucHien.split('T')[0] : ''
    });
    setModalHasCost(record.coChiPhi || (record.chiPhi && record.chiPhi > 0) ? true : false);
    setIsEditMode(editMode);
    setIsViewModalOpen(true);
  };

  // Handle thay đổi dữ liệu trong Modal Sửa
  const handleModalChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    const numberFields = ['taiSanId', 'chiPhi', 'trangThai'];
    setEditForm(prev => ({
      ...prev,
      [name]: numberFields.includes(name) ? (value === '' ? undefined : Number(value)) : value
    }));
  };

  // Submit cập nhật phiếu
  const handleModalSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    try {
      const payload = { ...editForm, coChiPhi: modalHasCost } as BaoTriTaiSan;
      if (!modalHasCost) {
        payload.chiPhi = 0;
        payload.loaiChiPhi = "";
      }
      const response = await maintenanceApi.update(payload);
      if (response.errorCode === 200) {
        toast.success('Cập nhật phiếu bảo trì thành công!');
        setIsViewModalOpen(false);
        fetchData(); // Load lại danh sách
      } else {
        toast.error(response.message || 'Lỗi khi cập nhật phiếu.');
      }
    } catch (error) {
      toast.error('Lỗi kết nối đến máy chủ.');
    } finally {
      setIsSubmitting(false);
    }
  };

  const getAsset = (id: number) => assets.find(a => a.id === id);

  const filteredRecords = records.filter(record => {
    const asset = getAsset(record.taiSanId);
    const searchStr = `${asset?.maTaiSan} ${asset?.tenTaiSan}`.toLowerCase();
    const matchesSearch = searchStr.includes(searchTerm.toLowerCase());
    const matchesStatus = filterStatus === 'all' || record.trangThai.toString() === filterStatus;
    return matchesSearch && matchesStatus;
  });

  const formatCurrency = (value?: number) => {
    if (!value) return '0 ₫';
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Bảo trì - Bảo dưỡng</h1>
          <p className="text-sm text-gray-500 mt-1">Quản lý bảo trì và sửa chữa tài sản</p>
        </div>
        <Link
          to="/maintenance/new"
          className="flex items-center gap-2 px-4 py-2 bg-orange-600 text-white rounded-lg hover:bg-orange-700 transition-colors"
        >
          <Plus className="w-5 h-5" />
          Tạo Phiếu bảo trì
        </Link>
      </div>

      {/* Stats và Filters giữ nguyên như cũ... */}
      <div className="bg-white rounded-lg border border-gray-200 p-4">
        <div className="flex flex-col lg:flex-row gap-4">
          <div className="flex-1 relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" />
            <input
              type="text"
              placeholder="Tìm kiếm theo mã hoặc tên tài sản..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
            />
          </div>
          <div className="flex gap-2">
            <select
              value={filterStatus}
              onChange={(e) => setFilterStatus(e.target.value)}
              className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
            >
              <option value="all">Tất cả trạng thái</option>
              <option value="0">Chờ xử lý</option>
              <option value="1">Đang thực hiện</option>
              <option value="2">Hoàn thành</option>
            </select>
          </div>
        </div>
      </div>

      {/* Maintenance Table */}
      <div className="bg-white rounded-lg border border-gray-200 overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50 border-b border-gray-200">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Ngày</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Tài sản</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Loại bảo trì</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Loại chi phí</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase">Chi phí</th>
                <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase">Trạng thái</th>
                <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase">Thao tác</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {isLoading ? (
                <tr><td colSpan={7} className="text-center py-8 text-gray-500">Đang tải dữ liệu...</td></tr>
              ) : filteredRecords.length === 0 ? (
                <tr><td colSpan={7} className="text-center py-8 text-gray-500">Không có dữ liệu phiếu bảo trì.</td></tr>
              ) : (
                filteredRecords.map((record) => {
                  const asset = getAsset(record.taiSanId);
                  const statusInfo = statusConfig[record.trangThai] || statusConfig[0];
                  const costInfo = record.loaiChiPhi ? costTypeConfig[record.loaiChiPhi] : null;

                  return (
                    <tr key={record.id} className="hover:bg-gray-50 transition-colors">
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                        {record.ngayThucHien ? new Date(record.ngayThucHien).toLocaleDateString('vi-VN') : 'N/A'}
                      </td>
                      <td className="px-6 py-4">
                        <div>
                          <p className="text-sm font-medium text-blue-600">{asset?.maTaiSan}</p>
                          <p className="text-sm text-gray-600">{asset?.tenTaiSan}</p>
                        </div>
                      </td>
                      <td className="px-6 py-4 text-sm text-gray-900">{loaiBaoTriConfig[record.loaiBaoTri]}</td>
                      <td className="px-6 py-4">
                        {costInfo ? (
                          <span className={`text-sm font-medium ${costInfo.color}`}>{costInfo.label}</span>
                        ) : <span className="text-sm text-gray-400">Không</span>}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium text-gray-900">
                        {formatCurrency(record.chiPhi)}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-center">
                        <span className={`inline-flex px-2 py-1 text-xs font-medium rounded-full ${statusInfo.color}`}>
                          {statusInfo.label}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-center">
                        <div className="flex items-center justify-center gap-2">
                          <button onClick={() => openModal(record, false)} className="p-1 text-blue-600 hover:bg-blue-50 rounded" title="Xem chi tiết">
                            <Eye className="w-4 h-4" />
                          </button>
                          {record.trangThai === 0 && (
                            <button onClick={() => openModal(record, true)} className="p-1 text-orange-600 hover:bg-orange-50 rounded" title="Sửa phiếu">
                              <Edit className="w-4 h-4" />
                            </button>
                          )}
                          {record.trangThai === 0 && (
                            <button onClick={() => record.id && handleDelete(record.id)} className="p-1 text-red-600 hover:bg-red-50 rounded" title="Xóa phiếu">
                              <Trash2 className="w-4 h-4" />
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

      {/* Modal 2-trong-1: Xem / Sửa Chi Tiết */}
      {isViewModalOpen && selectedRecord && (
        <div className="fixed inset-0 bg-gray-900/50 backdrop-blur-sm flex items-center justify-center z-50 p-4 transition-all">
          <div className="bg-white rounded-lg max-w-2xl w-full max-h-[90vh] overflow-hidden shadow-2xl flex flex-col">
            
            {/* Header Modal */}
            <div className="p-6 border-b border-gray-200 flex items-center justify-between bg-white shrink-0">
              <h3 className="font-bold text-gray-900 text-lg flex items-center gap-2">
                <Wrench className="w-5 h-5 text-orange-600" /> 
                {isEditMode ? 'Cập nhật Phiếu bảo trì' : 'Chi tiết Phiếu bảo trì'}
              </h3>
              <button onClick={() => setIsViewModalOpen(false)} className="text-gray-400 hover:text-gray-600">
                <X className="w-6 h-6" />
              </button>
            </div>
            
            {/* Body Modal */}
            <div className="p-6 overflow-y-auto space-y-6">
              
              {/* CHẾ ĐỘ XEM (VIEW MODE) */}
              {!isEditMode ? (
                <>
                  <div className="flex items-center justify-between p-4 bg-gray-50 rounded-lg border border-gray-200">
                    <div>
                      <p className="text-sm text-gray-500 mb-1">Ngày thực hiện</p>
                      <p className="font-semibold text-gray-900">{selectedRecord.ngayThucHien ? new Date(selectedRecord.ngayThucHien).toLocaleDateString('vi-VN') : 'N/A'}</p>
                    </div>
                    <div className="text-right">
                      <p className="text-sm text-gray-500 mb-1">Trạng thái</p>
                      <span className={`inline-flex px-3 py-1 text-xs font-medium rounded-full ${statusConfig[selectedRecord.trangThai]?.color}`}>
                        {statusConfig[selectedRecord.trangThai]?.label}
                      </span>
                    </div>
                  </div>

                  <div>
                    <h4 className="text-sm font-semibold text-gray-900 mb-3 uppercase tracking-wider">Thông tin Tài sản</h4>
                    <div className="grid grid-cols-2 gap-4">
                      <div className="col-span-2 p-3 bg-blue-50 border border-blue-100 rounded-lg">
                        <p className="text-sm font-medium text-blue-900">
                          [{getAsset(selectedRecord.taiSanId)?.maTaiSan}] - {getAsset(selectedRecord.taiSanId)?.tenTaiSan}
                        </p>
                      </div>
                      <div>
                        <p className="text-xs text-gray-500 mb-1">Loại bảo trì</p>
                        <p className="text-sm font-medium text-gray-900">{loaiBaoTriConfig[selectedRecord.loaiBaoTri]}</p>
                      </div>
                      <div>
                        <p className="text-xs text-gray-500 mb-1">Đơn vị thực hiện / NCC</p>
                        <p className="text-sm font-medium text-gray-900">{selectedRecord.nhaCungCap || 'Nội bộ'}</p>
                      </div>
                      <div className="col-span-2">
                        <p className="text-xs text-gray-500 mb-1">Nội dung chi tiết</p>
                        <p className="text-sm text-gray-900 bg-gray-50 p-3 rounded border border-gray-100 whitespace-pre-wrap">{selectedRecord.moTa}</p>
                      </div>
                    </div>
                  </div>

                  <div>
                    <h4 className="text-sm font-semibold text-gray-900 mb-3 uppercase tracking-wider">Thông tin Chi phí</h4>
                    {selectedRecord.chiPhi && selectedRecord.chiPhi > 0 ? (
                      <div className="grid grid-cols-2 gap-4">
                        <div>
                          <p className="text-xs text-gray-500 mb-1">Tổng chi phí</p>
                          <p className="text-lg font-bold text-red-600">{formatCurrency(selectedRecord.chiPhi)}</p>
                        </div>
                        <div>
                          <p className="text-xs text-gray-500 mb-1">Hạch toán</p>
                          <p className={`text-sm font-medium ${selectedRecord.loaiChiPhi ? costTypeConfig[selectedRecord.loaiChiPhi]?.color : ''}`}>
                            {selectedRecord.loaiChiPhi ? costTypeConfig[selectedRecord.loaiChiPhi]?.label : 'Không xác định'}
                          </p>
                        </div>
                      </div>
                    ) : (
                      <p className="text-sm text-gray-500 italic">Không phát sinh chi phí trong lần bảo trì này.</p>
                    )}
                  </div>
                  {selectedRecord.ghiChu && (
                    <div>
                      <h4 className="text-sm font-semibold text-gray-900 mb-2 uppercase tracking-wider">Ghi chú thêm</h4>
                      <p className="text-sm text-gray-700">{selectedRecord.ghiChu}</p>
                    </div>
                  )}
                </>
              ) 
              
              /* CHẾ ĐỘ SỬA (EDIT MODE) */
              : (
                <form id="edit-maintenance-form" onSubmit={handleModalSubmit} className="space-y-4">
                  <div className="grid grid-cols-2 gap-4">
                    <div className="col-span-2">
                      <label className="block text-sm font-medium text-gray-700 mb-1">Tài sản (Không được sửa)</label>
                      <input type="text" disabled value={`[${getAsset(editForm.taiSanId!)?.maTaiSan}] ${getAsset(editForm.taiSanId!)?.tenTaiSan}`} className="w-full px-3 py-2 border border-gray-300 rounded bg-gray-100" />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-1">Ngày thực hiện <span className="text-red-500">*</span></label>
                      <input type="date" name="ngayThucHien" value={editForm.ngayThucHien || ''} onChange={handleModalChange} required className="w-full px-3 py-2 border border-gray-300 rounded focus:ring-blue-500" />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-1">Trạng thái phiếu</label>
                      <select name="trangThai" value={editForm.trangThai ?? 0} onChange={handleModalChange} className="w-full px-3 py-2 border border-gray-300 rounded focus:ring-blue-500">
                        <option value={0}>Chờ xử lý</option>
                        <option value={1}>Đang thực hiện</option>
                        <option value={2}>Hoàn thành (Sinh chứng từ)</option>
                      </select>
                    </div>
                    <div className="col-span-2">
                      <label className="block text-sm font-medium text-gray-700 mb-1">Nội dung bảo trì <span className="text-red-500">*</span></label>
                      <textarea name="moTa" value={editForm.moTa || ''} onChange={handleModalChange} required rows={2} className="w-full px-3 py-2 border border-gray-300 rounded focus:ring-blue-500" />
                    </div>

                    <div className="col-span-2 pt-2 border-t">
                      <label className="flex items-center gap-2 cursor-pointer mb-3">
                        <input type="checkbox" checked={modalHasCost} onChange={(e) => setModalHasCost(e.target.checked)} className="rounded text-blue-600 focus:ring-blue-500" />
                        <span className="text-sm font-medium text-gray-900">Có phát sinh chi phí</span>
                      </label>
                      {modalHasCost && (
                        <div className="grid grid-cols-2 gap-4">
                          <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">Chi phí (VNĐ) <span className="text-red-500">*</span></label>
                            <input type="number" name="chiPhi" value={editForm.chiPhi || ''} onChange={handleModalChange} required min="0" className="w-full px-3 py-2 border border-gray-300 rounded focus:ring-blue-500" />
                          </div>
                          <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">Loại chi phí <span className="text-red-500">*</span></label>
                            <select name="loaiChiPhi" value={editForm.loaiChiPhi ?? ''} onChange={handleModalChange} required className="w-full px-3 py-2 border border-gray-300 rounded focus:ring-blue-500">
                              <option value="">Chọn loại</option>
                              <option value="sua_chua">Sửa chữa (Vào chi phí)</option>
                              <option value="nang_cap">Nâng cấp (Tăng nguyên giá)</option>
                            </select>
                          </div>
                        </div>
                      )}
                    </div>
                  </div>
                </form>
              )}
            </div>

            {/* Footer Modal */}
            <div className="p-4 border-t border-gray-200 bg-gray-50 flex justify-end shrink-0 gap-3">
              {isEditMode ? (
                <>
                  <button onClick={() => setIsEditMode(false)} type="button" className="px-4 py-2 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-100">
                    Hủy sửa
                  </button>
                  <button type="submit" form="edit-maintenance-form" disabled={isSubmitting} className="flex items-center gap-2 px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50">
                    <Save className="w-4 h-4" /> {isSubmitting ? 'Đang lưu...' : 'Lưu cập nhật'}
                  </button>
                </>
              ) : (
                <>
                  <button onClick={() => setIsViewModalOpen(false)} className="px-6 py-2 bg-gray-200 text-gray-800 font-medium rounded-lg hover:bg-gray-300">
                    Đóng
                  </button>
                  {/* Nút sửa nhanh ngay trong màn hình Xem (nếu phiếu chưa Hoàn thành) */}
                  {selectedRecord.trangThai === 0 && (
                    <button onClick={() => setIsEditMode(true)} className="flex items-center gap-2 px-6 py-2 bg-orange-600 text-white font-medium rounded-lg hover:bg-orange-700">
                      <Edit className="w-4 h-4" /> Sửa phiếu này
                    </button>
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