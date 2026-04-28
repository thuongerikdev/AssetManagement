import { useState, useEffect, useMemo } from 'react';
import { Link } from 'react-router';
import { Plus, Search, Wrench, Trash2, Eye, X, Edit, Save, RefreshCw } from 'lucide-react';
import { toast } from 'sonner';
import { maintenanceApi, BaoTriTaiSan } from '../../api/maintenanceApi';
import { useGlobalData } from '../../context/GlobalContext'; // <-- IMPORT GLOBAL CONTEXT

// Map Enum Loại bảo trì
const loaiBaoTriConfig: Record<string, string> = {
  '0': 'Bảo trì định kỳ',
  '1': 'Sửa chữa',
  '2': 'Nâng cấp',
  '3': 'Vệ sinh',
  '4': 'Kiểm tra'
};

// ==========================================
// 1. BIẾN CACHE CỤC BỘ TRÊN RAM (Chỉ lưu Lịch sử bảo trì)
// ==========================================
let cachedMaintenanceRecords: BaoTriTaiSan[] | null = null;

export function MaintenanceList() {
  // 2. Móc Tài sản từ KHO CHUNG (Tải 0 giây)
  const { assets, isLoadingGlobal, refreshData } = useGlobalData();

  const [searchTerm, setSearchTerm] = useState('');
  
  // State quản lý Lịch sử bảo trì
  const [records, setRecords] = useState<BaoTriTaiSan[]>(cachedMaintenanceRecords || []);
  const [isLoadingRecords, setIsLoadingRecords] = useState(!cachedMaintenanceRecords);

  // States cho Modal 2-trong-1 (Xem & Sửa)
  const [isViewModalOpen, setIsViewModalOpen] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const [selectedRecord, setSelectedRecord] = useState<BaoTriTaiSan | null>(null);
  
  // States cho Form trong Modal
  const [editForm, setEditForm] = useState<Partial<BaoTriTaiSan>>({});
  const [modalHasCost, setModalHasCost] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  // ==========================================
  // 3. HÀM TẢI LỊCH SỬ BẢO TRÌ CÓ TÍCH HỢP CACHE
  // ==========================================
  const fetchRecords = async (forceRefresh = false) => {
    if (!forceRefresh && cachedMaintenanceRecords) {
      setRecords(cachedMaintenanceRecords);
      return;
    }

    setIsLoadingRecords(true);
    try {
      const res = await maintenanceApi.getAll();
      if (res.errorCode === 200) {
        cachedMaintenanceRecords = res.data;
        setRecords(res.data);
      }
    } catch (error) {
      toast.error('Lỗi tải dữ liệu bảo trì.');
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
      fetchRecords(true)  // Làm mới Lịch sử bảo trì
    ]);
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Bạn có chắc chắn muốn xóa bản ghi bảo trì này?')) {
      try {
        const response = await maintenanceApi.delete(id);
        if (response.errorCode === 200) {
          toast.success('Xóa thành công');
          handleRefreshAll(); // Ép làm mới để cập nhật UI ngay lập tức
        } else {
          toast.error(response.message || 'Lỗi khi xóa');
        }
      } catch (error) {
        toast.error('Lỗi kết nối máy chủ');
      }
    }
  };

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

  const handleModalChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    const numberFields = ['taiSanId', 'chiPhi'];
    setEditForm(prev => ({
      ...prev,
      [name]: numberFields.includes(name) ? (value === '' ? undefined : Number(value)) : value
    }));
  };

  const handleModalSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    try {
      const payload = { 
        ...editForm, 
        coChiPhi: modalHasCost,
        trangThai: 2 // Luôn chốt là hoàn thành
      } as BaoTriTaiSan;

      if (!modalHasCost) {
        payload.chiPhi = 0;
      }

      const response = await maintenanceApi.update(payload);
      if (response.errorCode === 200) {
        toast.success('Cập nhật thông tin thành công!');
        setIsViewModalOpen(false);
        handleRefreshAll(); // Cập nhật lại dữ liệu sau khi sửa
      } else {
        toast.error(response.message || 'Lỗi khi cập nhật.');
      }
    } catch (error) {
      toast.error('Lỗi kết nối đến máy chủ.');
    } finally {
      setIsSubmitting(false);
    }
  };

  const getAsset = (id: number) => assets.find((a: any) => a.id === id);

  // ==========================================
  // 4. USE_MEMO BỘ LỌC ĐỂ RENDER MƯỢT MÀ
  // ==========================================
  const filteredRecords = useMemo(() => {
    return records.filter(record => {
      const asset = getAsset(record.taiSanId);
      const searchStr = `${asset?.maTaiSan} ${asset?.tenTaiSan}`.toLowerCase();
      return searchStr.includes(searchTerm.toLowerCase());
    });
  }, [records, assets, searchTerm]);

  const formatCurrency = (value?: number) => {
    if (!value) return '0 ₫';
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
  };

  const isScreenLoading = isLoadingGlobal || isLoadingRecords;

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Lịch sử Bảo trì / Sửa chữa</h1>
          <p className="text-sm text-gray-500 mt-1">Quản lý lịch sử sửa chữa tài sản</p>
        </div>
        <div className="flex gap-2">
          {/* Nút Làm mới */}
          <button 
            onClick={handleRefreshAll}
            disabled={isScreenLoading}
            className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-gray-600 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors disabled:opacity-50"
            title="Tải lại dữ liệu"
          >
            <RefreshCw className={`w-4 h-4 ${isScreenLoading ? 'animate-spin text-blue-600' : ''}`} />
            <span className="hidden sm:block">Làm mới</span>
          </button>
          
          <Link
            to="/maintenance/new"
            className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-sm"
          >
            <Plus className="w-5 h-5" />
            Ghi nhận sửa chữa
          </Link>
        </div>
      </div>

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
        </div>
      </div>

      {/* Maintenance Table */}
      <div className="bg-white rounded-lg border border-gray-200 overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50 border-b border-gray-200">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Ngày thực hiện</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Tài sản</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Loại bảo trì</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Đơn vị thực hiện</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase">Chi phí</th>
                <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase">Thao tác</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {isScreenLoading && filteredRecords.length === 0 ? (
                <tr><td colSpan={6} className="text-center py-8 text-gray-500">Đang tải dữ liệu...</td></tr>
              ) : filteredRecords.length === 0 ? (
                <tr><td colSpan={6} className="text-center py-8 text-gray-500">Không có lịch sử bảo trì.</td></tr>
              ) : (
                filteredRecords.map((record) => {
                  const asset = getAsset(record.taiSanId);

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
                      <td className="px-6 py-4 text-sm text-gray-900">{loaiBaoTriConfig[record.loaiBaoTri] || 'Khác'}</td>
                      <td className="px-6 py-4 text-sm text-gray-600">{record.nhaCungCap || 'Nội bộ'}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium text-red-600">
                        {record.chiPhi && record.chiPhi > 0 ? formatCurrency(record.chiPhi) : '-'}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-center">
                        <div className="flex items-center justify-center gap-2">
                          <button onClick={() => openModal(record, false)} className="p-1 text-blue-600 hover:bg-blue-50 rounded" title="Xem chi tiết">
                            <Eye className="w-4 h-4" />
                          </button>
                          <button onClick={() => openModal(record, true)} className="p-1 text-orange-600 hover:bg-orange-50 rounded" title="Sửa thông tin">
                            <Edit className="w-4 h-4" />
                          </button>
                          <button onClick={() => record.id && handleDelete(record.id)} className="p-1 text-red-600 hover:bg-red-50 rounded" title="Xóa lịch sử">
                            <Trash2 className="w-4 h-4" />
                          </button>
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
                <Wrench className="w-5 h-5 text-blue-600" /> 
                {isEditMode ? 'Cập nhật thông tin' : 'Chi tiết sửa chữa'}
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
                        <p className="text-sm font-medium text-gray-900">{loaiBaoTriConfig[selectedRecord.loaiBaoTri] || 'Khác'}</p>
                      </div>
                      <div>
                        <p className="text-xs text-gray-500 mb-1">Đơn vị thực hiện / NCC</p>
                        <p className="text-sm font-medium text-gray-900">{selectedRecord.nhaCungCap || 'Nội bộ'}</p>
                      </div>
                    </div>
                  </div>

                  <div>
                    <h4 className="text-sm font-semibold text-gray-900 mb-3 uppercase tracking-wider">Thông tin Chi phí</h4>
                    {selectedRecord.chiPhi && selectedRecord.chiPhi > 0 ? (
                      <div className="p-4 border border-red-100 bg-red-50 rounded-lg">
                        <p className="text-xs text-red-600 mb-1">Tổng chi phí sửa chữa</p>
                        <p className="text-lg font-bold text-red-700">{formatCurrency(selectedRecord.chiPhi)}</p>
                      </div>
                    ) : (
                      <p className="text-sm text-gray-500 italic">Không phát sinh chi phí.</p>
                    )}
                  </div>
                  
                  {selectedRecord.ghiChu && (
                    <div>
                      <h4 className="text-sm font-semibold text-gray-900 mb-2 uppercase tracking-wider">Ghi chú</h4>
                      <p className="text-sm text-gray-700 bg-gray-50 p-3 rounded border border-gray-100 whitespace-pre-wrap">{selectedRecord.ghiChu}</p>
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
                      <label className="block text-sm font-medium text-gray-700 mb-1">Loại bảo trì</label>
                      <select name="loaiBaoTri" value={editForm.loaiBaoTri ?? '0'} onChange={handleModalChange} required className="w-full px-3 py-2 border border-gray-300 rounded focus:ring-blue-500">
                        <option value="0">Bảo trì định kỳ</option>
                        <option value="1">Sửa chữa</option>
                        <option value="2">Nâng cấp</option>
                        <option value="3">Vệ sinh</option>
                        <option value="4">Kiểm tra</option>
                      </select>
                    </div>

                    <div className="col-span-2 pt-2 border-t">
                      <label className="flex items-center gap-2 cursor-pointer mb-3">
                        <input type="checkbox" checked={modalHasCost} onChange={(e) => setModalHasCost(e.target.checked)} className="rounded text-blue-600 focus:ring-blue-500" />
                        <span className="text-sm font-medium text-gray-900">Có phát sinh chi phí</span>
                      </label>
                      
                      <div className="grid grid-cols-2 gap-4">
                        {modalHasCost && (
                          <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">Chi phí (VNĐ) <span className="text-red-500">*</span></label>
                            <input type="number" name="chiPhi" value={editForm.chiPhi || ''} onChange={handleModalChange} required min="0" className="w-full px-3 py-2 border border-gray-300 rounded focus:ring-blue-500" />
                          </div>
                        )}
                        <div className={!modalHasCost ? "col-span-2" : ""}>
                          <label className="block text-sm font-medium text-gray-700 mb-1">Nhà cung cấp / Đơn vị sửa chữa</label>
                          <input type="text" name="nhaCungCap" value={editForm.nhaCungCap || ''} onChange={handleModalChange} className="w-full px-3 py-2 border border-gray-300 rounded focus:ring-blue-500" />
                        </div>
                      </div>
                    </div>

                    <div className="col-span-2">
                      <label className="block text-sm font-medium text-gray-700 mb-1">Ghi chú</label>
                      <textarea name="ghiChu" value={editForm.ghiChu || ''} onChange={handleModalChange} rows={2} className="w-full px-3 py-2 border border-gray-300 rounded focus:ring-blue-500" />
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
                  <button onClick={() => setIsEditMode(true)} className="flex items-center gap-2 px-6 py-2 bg-blue-600 text-white font-medium rounded-lg hover:bg-blue-700">
                    <Edit className="w-4 h-4" /> Sửa lịch sử này
                  </button>
                </>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  );
}