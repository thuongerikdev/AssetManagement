import { useState, useEffect } from 'react';
import { Send, ArrowLeftRight, Search, Clock, X, Save } from 'lucide-react';
import { toast } from 'sonner';
import { assetAllocationApi, DieuChuyenTaiSan } from '../../api/assetAllocationApi';
import { assetApi, TaiSan } from '../../api/assetApi';
import { departmentApi, Department } from '../../api/departmentApi';

// Mapping Loại điều chuyển bằng CHUỖI (String) khớp với Enum C#
const typeConfig: Record<string, { label: string; color: string; icon: any }> = {
  'CapPhat': { label: 'Cấp phát', color: 'bg-green-100 text-green-700', icon: Send },
  'LuanChuyen': { label: 'Điều chuyển', color: 'bg-blue-100 text-blue-700', icon: ArrowLeftRight },
};

// Map TrangThai
const statusConfig: Record<string, { label: string; color: string }> = {
  'da_hoan_thanh': { label: 'Hoàn thành', color: 'bg-green-100 text-green-700' },
  'cho_xu_ly': { label: 'Chờ xử lý', color: 'bg-yellow-100 text-yellow-700' },
};

export function AllocationList() {
  const [searchTerm, setSearchTerm] = useState('');
  const [filterType, setFilterType] = useState<string>('all');
  
  const [records, setRecords] = useState<DieuChuyenTaiSan[]>([]);
  const [assets, setAssets] = useState<TaiSan[]>([]);
  const [departments, setDepartments] = useState<Department[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  // Modal states dùng kiểu chuỗi (Đã bỏ ThuHoi)
  const [showModal, setShowModal] = useState(false);
  const [modalType, setModalType] = useState<'CapPhat' | 'LuanChuyen'>('CapPhat');
  const [isSubmitting, setIsSubmitting] = useState(false);

  // Form states
  const [formData, setFormData] = useState<Partial<DieuChuyenTaiSan>>({
    taiSanId: undefined,
    tuPhongBanId: undefined,
    denPhongBanId: undefined,
    tuNguoiDungId: undefined,
    denNguoiDungId: undefined,
    ngayThucHien: new Date().toISOString().split('T')[0],
    ghiChu: '',
  });

  const fetchData = async () => {
    setIsLoading(true);
    try {
      const [recRes, assetRes, deptRes] = await Promise.all([
        assetAllocationApi.getAll(),
        assetApi.getAll(),
        departmentApi.getAll()
      ]);
      if (recRes.errorCode === 200) setRecords(recRes.data);
      if (assetRes.errorCode === 200) setAssets(assetRes.data);
      if (deptRes.errorCode === 200) setDepartments(deptRes.data);
    } catch (error) {
      toast.error('Lỗi khi tải dữ liệu.');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const openModal = (type: 'CapPhat' | 'LuanChuyen') => {
    setModalType(type);
    setFormData({
      taiSanId: undefined,
      tuPhongBanId: undefined,
      denPhongBanId: undefined,
      tuNguoiDungId: undefined,
      denNguoiDungId: undefined,
      ngayThucHien: new Date().toISOString().split('T')[0],
      ghiChu: '',
    });
    setShowModal(true);
  };

  // Tự động load phòng ban gốc khi chọn Tài sản để Điều chuyển
  const handleAssetSelect = (assetIdStr: string) => {
    const assetId = Number(assetIdStr);
    const selectedAsset = assets.find(a => a.id === assetId);
    
    setFormData(prev => ({
      ...prev,
      taiSanId: assetId,
      tuPhongBanId: modalType === 'LuanChuyen' ? selectedAsset?.phongBanId : undefined,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.taiSanId || !formData.ngayThucHien) {
      toast.error('Vui lòng chọn tài sản và ngày thực hiện!');
      return;
    }

    setIsSubmitting(true);
    try {
      const payload: any = {
        taiSanId: formData.taiSanId,
        loaiDieuChuyen: modalType, // Gửi chuỗi 'CapPhat' hoặc 'LuanChuyen'
        ngayThucHien: formData.ngayThucHien,
        tuPhongBanId: formData.tuPhongBanId || undefined,
        denPhongBanId: formData.denPhongBanId || undefined,
        tuNguoiDungId: formData.tuNguoiDungId ? Number(formData.tuNguoiDungId) : undefined, 
        denNguoiDungId: formData.denNguoiDungId ? Number(formData.denNguoiDungId) : undefined,
        ghiChu: formData.ghiChu,
      };

      const response = await assetAllocationApi.create(payload);
      if (response.errorCode === 200) {
        toast.success('Lưu phiếu thành công!');
        setShowModal(false);
        fetchData(); // Reload list
      } else {
        toast.error(response.message || 'Có lỗi xảy ra.');
      }
    } catch (error) {
      toast.error('Lỗi kết nối máy chủ.');
    } finally {
      setIsSubmitting(false);
    }
  };

  // Helpers
  const getAsset = (id: number) => assets.find(a => a.id === id);
  const getDept = (id?: number) => departments.find(d => d.id === id)?.tenPhongBan || 'N/A';

  const filteredRecords = records.filter(record => {
    // Ẩn Thu hồi khỏi danh sách phiếu (nếu database đang có lưu trước đó)
    if (record.loaiDieuChuyen === 'ThuHoi') return false;

    const asset = getAsset(record.taiSanId!);
    const searchStr = `${asset?.maTaiSan} ${asset?.tenTaiSan}`.toLowerCase();
    const matchesSearch = searchStr.includes(searchTerm.toLowerCase());
    
    // So sánh bằng chuỗi
    const matchesType = filterType === 'all' || record.loaiDieuChuyen === filterType;
    
    return matchesSearch && matchesType;
  });

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Cấp phát & Điều chuyển</h1>
          <p className="text-sm text-gray-500 mt-1">Quản lý cấp phát và điều chuyển tài sản</p>
        </div>
        <div className="flex gap-2">
          <button onClick={() => openModal('CapPhat')} className="flex items-center gap-2 px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors shadow-sm">
            <Send className="w-5 h-5" /> Cấp phát
          </button>
          <button onClick={() => openModal('LuanChuyen')} className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-sm">
            <ArrowLeftRight className="w-5 h-5" /> Điều chuyển
          </button>
        </div>
      </div>

      {/* Filters */}
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
              value={filterType}
              onChange={(e) => setFilterType(e.target.value)}
              className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
            >
              <option value="all">Tất cả loại</option>
              <option value="CapPhat">Cấp phát</option>
              <option value="LuanChuyen">Điều chuyển</option>
            </select>
          </div>
        </div>
      </div>

      {/* Records List */}
      {isLoading ? (
        <div className="text-center py-12 bg-white rounded-lg border border-gray-200">Đang tải dữ liệu...</div>
      ) : filteredRecords.length === 0 ? (
        <div className="text-center py-12 bg-white rounded-lg border border-gray-200">Không có dữ liệu phiếu nào.</div>
      ) : (
        <div className="space-y-4">
          {filteredRecords.map((record) => {
            const typeInfo = typeConfig[record.loaiDieuChuyen as string] || { label: 'Khác', color: 'bg-gray-100', icon: Send };
            const TypeIcon = typeInfo.icon;
            const assetInfo = getAsset(record.taiSanId!);
            const statusInfo = statusConfig[record.trangThai || 'da_hoan_thanh'] || statusConfig['da_hoan_thanh'];
            
            return (
              <div key={record.id} className="bg-white rounded-lg border border-gray-200 p-6 hover:shadow-md transition-shadow">
                <div className="flex items-start justify-between">
                  <div className="flex items-start gap-4 flex-1">
                    <div className={`p-3 rounded-lg ${typeInfo.color}`}>
                      <TypeIcon className="w-6 h-6" />
                    </div>
                    
                    <div className="flex-1">
                      <div className="flex items-center gap-3 mb-2">
                        <span className={`inline-flex px-3 py-1 text-xs font-medium rounded-full ${typeInfo.color}`}>
                          {typeInfo.label}
                        </span>
                        <span className={`inline-flex px-3 py-1 text-xs font-medium rounded-full ${statusInfo.color}`}>
                          {statusInfo.label}
                        </span>
                      </div>
                      
                      <h3 className="font-semibold text-gray-900 mb-2">
                        {assetInfo?.maTaiSan} - {assetInfo?.tenTaiSan}
                      </h3>
                      
                      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 text-sm">
                        {record.loaiDieuChuyen === 'LuanChuyen' && (
                          <div>
                            <p className="text-gray-600">Từ phòng ban:</p>
                            <p className="text-gray-900 font-medium">{getDept(record.tuPhongBanId)}</p>
                          </div>
                        )}
                        {(record.loaiDieuChuyen === 'CapPhat' || record.loaiDieuChuyen === 'LuanChuyen') && (
                          <div>
                            <p className="text-gray-600">{record.loaiDieuChuyen === 'LuanChuyen' ? 'Đến phòng ban:' : 'Cấp cho phòng ban:'}</p>
                            <p className="text-gray-900 font-medium">{getDept(record.denPhongBanId)}</p>
                          </div>
                        )}
                      </div>
                      
                      {record.ghiChu && (
                        <p className="text-sm text-gray-600 mt-3">
                          <span className="font-medium">Ghi chú:</span> {record.ghiChu}
                        </p>
                      )}
                    </div>
                  </div>
                  
                  <div className="text-right">
                    <div className="flex items-center gap-2 text-sm text-gray-500 mb-2">
                      <Clock className="w-4 h-4" />
                      {record.ngayThucHien ? new Date(record.ngayThucHien).toLocaleDateString('vi-VN') : 'N/A'}
                    </div>
                  </div>
                </div>
              </div>
            );
          })}
        </div>
      )}

      {/* Modal - Đã sửa lỗi đen màn hình bằng bg-gray-900/50 và backdrop-blur-sm */}
      {showModal && (
        <div className="fixed inset-0 bg-gray-900/50 backdrop-blur-sm flex items-center justify-center z-[9999] p-4">
          <div className="bg-white rounded-lg max-w-2xl w-full max-h-[90vh] overflow-auto shadow-2xl">
            <div className="p-6 border-b border-gray-200 flex items-center justify-between sticky top-0 bg-white z-10">
              <h3 className="font-bold text-gray-900 text-lg">
                {modalType === 'CapPhat' ? 'Cấp phát Tài sản' : 'Điều chuyển Tài sản'}
              </h3>
              <button onClick={() => setShowModal(false)} className="text-gray-400 hover:text-gray-600">
                <X className="w-6 h-6" />
              </button>
            </div>
            
            <form onSubmit={handleSubmit} className="p-6 space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">Chọn tài sản <span className="text-red-500">*</span></label>
                <select 
                  required 
                  value={formData.taiSanId || ''} 
                  onChange={(e) => handleAssetSelect(e.target.value)}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                >
                  <option value="">-- Chọn tài sản --</option>
                  {assets.filter(a => {
                    // Cấp phát -> Chỉ chọn TS Chưa cấp phát (0)
                    // Điều chuyển -> Chỉ chọn TS Đang sử dụng (2)
                    if (modalType === 'CapPhat') return a.trangThai?.toString() === '0' || a.trangThai === 'ChuaCapPhat'; 
                    return a.trangThai?.toString() === '2' || a.trangThai === 'DangSuDung'; 
                  }).map(asset => (
                    <option key={asset.id} value={asset.id}>{asset.maTaiSan} - {asset.tenTaiSan}</option>
                  ))}
                </select>
              </div>

              {modalType === 'LuanChuyen' && (
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">Từ phòng ban (Tự động lấy)</label>
                  <input
                    type="text"
                    disabled
                    value={getDept(formData.tuPhongBanId)}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg bg-gray-50"
                  />
                </div>
              )}

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  {modalType === 'LuanChuyen' ? 'Đến phòng ban' : 'Phòng ban nhận'} <span className="text-red-500">*</span>
                </label>
                <select
                  required
                  value={formData.denPhongBanId || ''}
                  onChange={(e) => setFormData({...formData, denPhongBanId: Number(e.target.value)})}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                >
                  <option value="">-- Chọn phòng ban --</option>
                  {departments.map(dept => (
                    <option key={dept.id} value={dept.id}>{dept.tenPhongBan}</option>
                  ))}
                </select>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">Mã Nhân viên nhận</label>
                  <input
                    type="number"
                    value={formData.denNguoiDungId || ''}
                    onChange={(e) => setFormData({...formData, denNguoiDungId: Number(e.target.value)})}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                    placeholder="Nhập ID (nếu có)"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">Ngày thực hiện <span className="text-red-500">*</span></label>
                  <input
                    type="date"
                    required
                    value={formData.ngayThucHien}
                    onChange={(e) => setFormData({...formData, ngayThucHien: e.target.value})}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                  />
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">Ghi chú</label>
                <textarea
                  rows={3}
                  value={formData.ghiChu || ''}
                  onChange={(e) => setFormData({...formData, ghiChu: e.target.value})}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                  placeholder="Nhập ghi chú hoặc lý do..."
                />
              </div>

              <div className="pt-4 border-t border-gray-200 flex justify-end gap-3">
                <button type="button" onClick={() => setShowModal(false)} className="px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50">
                  Hủy
                </button>
                <button type="submit" disabled={isSubmitting} className="flex items-center gap-2 px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50">
                  <Save className="w-5 h-5" /> {isSubmitting ? 'Đang lưu...' : 'Lưu lại'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}