import { useState, useEffect, useMemo } from 'react';
import { Monitor, CheckCircle, Package, AlertTriangle, Clock, RefreshCw, XCircle, Users, Box } from 'lucide-react';
import { toast } from 'sonner';
import { assetApi, TaiSan } from '../../api/assetApi';
import { authApi } from '../../api/authApi';

export function MyAssets() {
  const [myAssets, setMyAssets] = useState<TaiSan[]>([]);
  
  // Dữ liệu cho Trưởng phòng
  const [departmentAssets, setDepartmentAssets] = useState<TaiSan[]>([]);
  
  const [isLoading, setIsLoading] = useState(true);
  const [activeTab, setActiveTab] = useState<'my_assets' | 'staff_assets'>('my_assets');

  // STATE ĐỂ HỨNG API TỪ BACKEND
  const [deptInfo, setDeptInfo] = useState({
    userId: 0,
    id: null as number | null,
    name: '',
    isManager: false
  });

  // ── 1. FETCH DỮ LIỆU ĐÃ ĐƯỢC TỐI ƯU TỪ BACKEND ───────────────────
  const fetchData = async () => {
    setIsLoading(true);
    try {
      // 1. Gọi API mới viết để lấy Cờ Trưởng phòng và Tên phòng
      const infoRes = await authApi.getMyDepartmentInfo();
      if (infoRes.errorCode !== 200 || !infoRes.data) {
        toast.error("Không lấy được thông tin phòng ban.");
        return;
      }

      const { userId, departmentId, departmentName, isManager } = infoRes.data;
      
      // Cập nhật State để UI hiển thị
      setDeptInfo({ userId, id: departmentId, name: departmentName, isManager });

      // 2. Lấy tài sản của bản thân
      const myRes = await assetApi.getMyAssets(userId);
      if (myRes.errorCode === 200) setMyAssets(myRes.data || []);

      // 3. NẾU LÀ TRƯỞNG PHÒNG -> Lấy toàn bộ tài sản phòng ban (kèm tên người dùng)
      if (isManager && departmentId) {
        const assetsRes = await assetApi.getAssetsByDepartment(departmentId);
        if (assetsRes.errorCode === 200) {
          setDepartmentAssets(assetsRes.data || []);
        }
      }
    } catch (error) {
      toast.error('Lỗi khi tải dữ liệu từ máy chủ.');
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  // ── 2. XỬ LÝ KÝ NHẬN / TỪ CHỐI ────────────────────────────────────
  const handleConfirm = async (id: number) => {
    if (window.confirm('Xác nhận bạn đã nhận tài sản này với tình trạng hoạt động tốt?')) {
      try {
        const res = await assetApi.confirm(id);
        if (res.errorCode === 200) {
          toast.success('Ký nhận tài sản thành công!');
          fetchData();
        } else {
          toast.error(res.message || 'Có lỗi xảy ra');
        }
      } catch { toast.error('Lỗi kết nối máy chủ'); }
    }
  };

  const handleReject = async (asset: TaiSan) => {
    // 1. Kiểm tra ID trước
    if (!asset.id) {
      toast.error('Không tìm thấy ID của tài sản này.');
      return;
    }

    const reason = window.prompt('Nhập lý do từ chối tài sản (bắt buộc):', '');
    if (!reason || reason.trim() === '') {
      toast.warning('Vui lòng nhập lý do từ chối.');
      return;
    }
    
    try {
      // Lúc này TypeScript biết chắc chắn asset.id là kiểu number rồi
      const res = await assetApi.reject(asset.id, reason); 
      if (res.errorCode === 200) {
        toast.success(res.message || 'Từ chối tài sản thành công!');
        fetchData();
      } else {
        toast.error(res.message || 'Có lỗi xảy ra khi từ chối tài sản');
      }
    } catch (error) {
      toast.error('Lỗi kết nối máy chủ');
      console.error(error);
    }
  };

  // ── 3. LỌC VÀ GOM NHÓM DỮ LIỆU ĐỂ RENDER ──────────────────────────
  
  // 3.1: Tài sản của tôi (Đang dùng / Chờ xác nhận)
  const { pendingAssets, activeAssets } = useMemo(() => {
    return {
      pendingAssets: myAssets.filter(a => a.trangThai?.toString() === 'ChoXacNhan' || a.trangThai?.toString() === '1'),
      activeAssets: myAssets.filter(a => a.trangThai?.toString() === 'DangSuDung' || a.trangThai?.toString() === '2')
    };
  }, [myAssets]);

  // 3.2: Gom nhóm tài sản theo từng NHÂN VIÊN
// 3.2: Gom nhóm tài sản theo từng NHÂN VIÊN
  const groupedStaffAssets = useMemo(() => {
    if (!deptInfo.isManager || departmentAssets.length === 0) return [];

    const groups: { user: any, assets: TaiSan[] }[] = [];
    
    // 1. Lọc ra danh sách các user duy nhất đang sở hữu tài sản (loại bỏ trường hợp null/undefined)
    const uniqueUsersMap = new Map();
    
    departmentAssets.forEach(asset => {
      // Giả sử backend trả về thông tin user lồng trong asset hoặc nằm cùng cấp
      // Nếu backend trả về dạng asset.nguoiDungId, asset.tenNguoiDung...
      if (asset.nguoiDungId) {
        uniqueUsersMap.set(asset.nguoiDungId, {
          userID: asset.nguoiDungId,
          // Đổi các trường dưới đây cho đúng với key thực tế backend trả về trong asset (nếu có)
          fullName: (asset as any).tenNguoiDung || `Nhân viên mã #${asset.nguoiDungId}`, 
          userName: (asset as any).taiKhoanNguoiDung || `user_${asset.nguoiDungId}`
        });
      }
    });

    // 2. Gom tài sản vào từng user tìm được
    uniqueUsersMap.forEach(staff => {
      const staffAssets = departmentAssets.filter(a => a.nguoiDungId === staff.userID);
      if (staffAssets.length > 0) {
        groups.push({ user: staff, assets: staffAssets });
      }
    });

    // 3. Gom các tài sản của phòng mà CHƯA giao cho nhân viên nào
    const unassignedAssets = departmentAssets.filter(a => !a.nguoiDungId);
    if (unassignedAssets.length > 0) {
      groups.push({
        user: { userID: -1, fullName: "Tài sản chung của phòng (Chưa giao ai)", userName: "system" },
        assets: unassignedAssets
      });
    }

    return groups;
  }, [departmentAssets, deptInfo.isManager]);

  // ── 4. COMPONENT THẺ TÀI SẢN ──────────────────────────────────────
  const RenderAssetCard = ({ asset, isMyAsset }: { asset: TaiSan, isMyAsset: boolean }) => {
    const isPending = asset.trangThai?.toString() === 'ChoXacNhan' || asset.trangThai?.toString() === '1';

    return (
      <div className={`rounded-xl border p-5 shadow-sm relative overflow-hidden ${isPending ? 'bg-yellow-50 border-yellow-200' : 'bg-white border-gray-200 hover:shadow-md transition-shadow'}`}>
        {isPending && <div className="absolute top-0 left-0 w-1 h-full bg-yellow-400"></div>}
        
        <div className="flex items-center gap-3 mb-4">
          <div className={`p-2.5 rounded-lg ${isPending ? 'bg-white text-yellow-600 shadow-sm' : 'bg-green-50 text-green-600'}`}>
            <Monitor className="w-5 h-5" />
          </div>
          <div className="flex-1">
            <h3 className="font-bold text-gray-900 leading-tight line-clamp-1" title={asset.tenTaiSan}>{asset.tenTaiSan}</h3>
            <p className="text-xs font-medium text-blue-600">{asset.maTaiSan}</p>
          </div>
          {!isPending && (
            <span className="text-green-600 font-medium bg-green-50 px-2 py-0.5 rounded-full text-[10px] border border-green-200 whitespace-nowrap">
              Đang sử dụng
            </span>
          )}
        </div>

        <div className="space-y-1.5 pt-3 border-t border-gray-100 text-sm">
          <div className="flex justify-between">
            <span className="text-gray-500">Số Serial:</span>
            <span className="font-medium text-gray-900 truncate max-w-[120px]">{asset.soSeri || '-'}</span>
          </div>
          <div className="flex justify-between">
            <span className="text-gray-500">Ngày cấp:</span>
            <span className="font-medium text-gray-900">{asset.ngayCapPhat ? new Date(asset.ngayCapPhat).toLocaleDateString('vi-VN') : 'Chưa nhận'}</span>
          </div>
        </div>

        {/* Nút hành động CHỈ hiện nếu là ĐỒ CỦA CHÍNH MÌNH và đang CHỜ NHẬN */}
        {isMyAsset && isPending && (
          <div className="flex gap-2 mt-4 pt-3 border-t border-yellow-200/50">
            <button onClick={() => asset.id && handleConfirm(asset.id)} className="flex-1 flex items-center justify-center gap-1.5 py-2 bg-yellow-500 hover:bg-yellow-600 text-white text-sm font-bold rounded-lg transition-colors">
              <CheckCircle className="w-4 h-4" /> Nhận
            </button>
            <button onClick={() => handleReject(asset)} className="flex-1 flex items-center justify-center gap-1.5 py-2 bg-red-500 hover:bg-red-600 text-white text-sm font-bold rounded-lg transition-colors">
              <XCircle className="w-4 h-4" /> Từ chối
            </button>
          </div>
        )}
      </div>
    );
  };

  return (
    <div className="space-y-6 max-w-6xl mx-auto py-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Quản lý tài sản</h1>
          <p className="text-gray-500 mt-1">Danh sách tài sản bạn đang quản lý hoặc sử dụng.</p>
        </div>
        
        <button 
          onClick={() => fetchData()}
          disabled={isLoading}
          className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-gray-600 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors disabled:opacity-50 shadow-sm"
        >
          <RefreshCw className={`w-4 h-4 ${isLoading ? 'animate-spin text-blue-600' : ''}`} />
          <span className="hidden sm:block">Làm mới</span>
        </button>
      </div>

      {/* HIỆN 2 TAB NẾU CỜ IsManager = TRUE */}
      {deptInfo.isManager && (
        <div className="flex space-x-1 bg-gray-200/50 p-1 rounded-xl w-fit">
          <button
            onClick={() => setActiveTab('my_assets')}
            className={`flex items-center gap-2 px-5 py-2.5 rounded-lg text-sm font-semibold transition-all ${
              activeTab === 'my_assets' ? 'bg-white text-blue-600 shadow-sm' : 'text-gray-600 hover:text-gray-900 hover:bg-gray-200'
            }`}
          >
            <Package className="w-4 h-4" />
            Tài sản của tôi
          </button>
          <button
            onClick={() => setActiveTab('staff_assets')}
            className={`flex items-center gap-2 px-5 py-2.5 rounded-lg text-sm font-semibold transition-all ${
              activeTab === 'staff_assets' ? 'bg-white text-purple-600 shadow-sm' : 'text-gray-600 hover:text-gray-900 hover:bg-gray-200'
            }`}
          >
            <Users className="w-4 h-4" />
            {/* IN TÊN PHÒNG BAN ĐÃ LẤY ĐƯỢC LÊN ĐÂY */}
            Tài sản nhân sự ({deptInfo.name})
          </button>
        </div>
      )}

      {isLoading ? (
        <div className="text-center py-12 text-gray-500 bg-white rounded-xl border border-gray-200">Đang tải dữ liệu...</div>
      ) : (
        <div className="mt-4">
          
          {/* ========================================================
              TAB 1: TÀI SẢN CỦA TÔI
          ========================================================= */}
          {activeTab === 'my_assets' && (
            <div className="space-y-6 animate-in fade-in slide-in-from-bottom-2 duration-300">
              {myAssets.length === 0 ? (
                <div className="text-center py-16 bg-white rounded-xl border border-gray-200 shadow-sm flex flex-col items-center">
                  <Package className="w-16 h-16 text-gray-300 mb-4" />
                  <h3 className="text-lg font-semibold text-gray-900">Chưa có tài sản</h3>
                  <p className="text-gray-500 mt-1">Bạn chưa được công ty cấp phát tài sản nào.</p>
                </div>
              ) : (
                <>
                  {pendingAssets.length > 0 && (
                    <div>
                      <h3 className="text-sm font-bold text-yellow-700 flex items-center gap-2 mb-3 bg-yellow-50 py-1.5 px-3 rounded-lg w-fit border border-yellow-200">
                        <AlertTriangle className="w-4 h-4" /> Cần xác nhận bàn giao ({pendingAssets.length})
                      </h3>
                      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                        {pendingAssets.map(a => <RenderAssetCard key={a.id} asset={a} isMyAsset={true} />)}
                      </div>
                    </div>
                  )}

                  {activeAssets.length > 0 && (
                    <div>
                      <h3 className="text-sm font-bold text-green-700 flex items-center gap-2 mb-3 bg-green-50 py-1.5 px-3 rounded-lg w-fit border border-green-200">
                        <CheckCircle className="w-4 h-4" /> Đang sử dụng ({activeAssets.length})
                      </h3>
                      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                        {activeAssets.map(a => <RenderAssetCard key={a.id} asset={a} isMyAsset={true} />)}
                      </div>
                    </div>
                  )}
                </>
              )}
            </div>
          )}

          {/* ========================================================
              TAB 2: TÀI SẢN NHÂN VIÊN GOM NHÓM
          ========================================================= */}
          {activeTab === 'staff_assets' && deptInfo.isManager && (
            <div className="space-y-6 animate-in fade-in slide-in-from-bottom-2 duration-300">
              {groupedStaffAssets.length === 0 ? (
                <div className="text-center py-16 bg-white rounded-xl border border-gray-200 shadow-sm flex flex-col items-center">
                  <Box className="w-16 h-16 text-gray-300 mb-4" />
                  <h3 className="text-lg font-semibold text-gray-900">Phòng ban trống</h3>
                  <p className="text-gray-500 mt-1">Chưa có nhân viên nào trong phòng được cấp phát tài sản.</p>
                </div>
              ) : (
                <div className="space-y-6">
                  {/* IN RA TỪNG CỤM NHÂN VIÊN VÀ TÀI SẢN BÊN DƯỚI */}
                  {groupedStaffAssets.map((group, index) => (
                    <div key={index} className="bg-white rounded-xl border border-gray-200 overflow-hidden shadow-sm">
                      <div className="bg-gray-50 px-5 py-3 border-b border-gray-200 flex items-center justify-between">
                        <div className="flex items-center gap-3">
                          <div className={`w-10 h-10 rounded-full flex items-center justify-center font-bold text-lg border ${group.user.userID === -1 ? 'bg-gray-200 text-gray-700 border-gray-300' : 'bg-purple-100 text-purple-700 border-purple-200'}`}>
                            {group.user.fullName?.charAt(0) || 'U'}
                          </div>
                          <div>
                            <h3 className="font-bold text-gray-900 text-base">{group.user.fullName}</h3>
                            <p className="text-xs text-gray-500 font-medium">@{group.user.userName} • Đang giữ {group.assets.length} tài sản</p>
                          </div>
                        </div>
                      </div>

                      <div className="p-5 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 bg-gray-50/30">
                        {/* Render từng thẻ tài sản của nhân viên này */}
                        {group.assets.map(asset => (
                          <RenderAssetCard key={asset.id} asset={asset} isMyAsset={false} />
                        ))}
                      </div>
                    </div>
                  ))}
                </div>
              )}
            </div>
          )}

        </div>
      )}
    </div>
  );
}