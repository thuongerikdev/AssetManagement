import { useState, useEffect, useMemo } from 'react';
import { Monitor, CheckCircle, Package, AlertTriangle, Clock, RefreshCw, XCircle, Users } from 'lucide-react';
import { toast } from 'sonner';
import { assetApi, TaiSan } from '../../api/assetApi';
import { authApi } from '../../api/authApi';
import { assetAllocationApi } from '../../api/assetAllocationApi';

export function MyAssets() {
  const [myAssets, setMyAssets] = useState<TaiSan[]>([]);
  
  // Dữ liệu cho Trưởng phòng
  const [departmentUsers, setDepartmentUsers] = useState<any[]>([]);
  const [departmentAssets, setDepartmentAssets] = useState<TaiSan[]>([]);
  
  const [isLoading, setIsLoading] = useState(true);
  
  // State quản lý Tab đang mở
  const [activeTab, setActiveTab] = useState<'my_assets' | 'staff_assets'>('my_assets');

  // ── 1. ĐỌC THÔNG TIN VÀ PHÂN QUYỀN TRƯỞNG PHÒNG ──────────────────
  const userInfoStr = localStorage.getItem('user_info');
  const userInfo = userInfoStr ? JSON.parse(userInfoStr) : null;
  const currentUserId = userInfo?.userID;
  const currentDeptId = userInfo?.departmentID;
  
  // KIỂM TRA TRƯỞNG PHÒNG (Dựa vào quyền lấy danh sách user trong phòng ban)
  const isManager = useMemo(() => {
    const permissions: string[] = userInfo?.permissions || [];
    // Mã quyền của Role Trưởng phòng (hoặc Giám đốc)
    return permissions.includes('user.get_by_department_id') || permissions.includes('user.admin_get_all');
  }, [userInfo]);

  // ── 2. FETCH DỮ LIỆU SONG SONG ────────────────────────────────────
  const fetchData = async () => {
    if (!currentUserId) return;
    setIsLoading(true);

    try {
      // API 1: Lấy tài sản của bản thân
      const myAssetsPromise = assetApi.getMyAssets(currentUserId);
      
      let deptUsersPromise = Promise.resolve({ data: [] });
      let deptAssetsPromise = Promise.resolve({ data: [] });

      // Nếu là quản lý và có ID phòng ban -> Gọi thêm 2 API nữa
      if (isManager && currentDeptId) {
        deptUsersPromise = authApi.getUsersByDepartment(Number(currentDeptId));
        deptAssetsPromise = assetApi.getAssetsByDepartment(Number(currentDeptId));
      }

      // Chờ cả 3 API chạy xong cùng lúc
      const [myRes, usersRes, assetsRes] = await Promise.all([myAssetsPromise, deptUsersPromise, deptAssetsPromise]);

      if (myRes.errorCode === 200) setMyAssets(myRes.data || []);
      
      if (isManager) {
        // Lọc bỏ chính bản thân quản lý ra khỏi danh sách nhân viên để không trùng lặp
        const staffList = (usersRes.data || []).filter((u: any) => u.userID !== currentUserId);
        setDepartmentUsers(staffList);
        setDepartmentAssets(assetsRes.data || []);
      }

    } catch (error) {
      toast.error('Lỗi khi tải dữ liệu.');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  // ── 3. XỬ LÝ NHẬN/TỪ CHỐI ─────────────────────────────────────────
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
    toast.info("Gọi logic API từ chối ở đây");
  };

  // ── 4. NHÓM DỮ LIỆU ĐỂ RENDER ─────────────────────────────────────
  
  // Lọc tài sản của mình
  const { pendingAssets, activeAssets } = useMemo(() => {
    return {
      pendingAssets: myAssets.filter(a => a.trangThai?.toString() === 'ChoXacNhan' || a.trangThai?.toString() === '1'),
      activeAssets: myAssets.filter(a => a.trangThai?.toString() === 'DangSuDung' || a.trangThai?.toString() === '2')
    };
  }, [myAssets]);

  // Gom nhóm tài sản theo từng nhân viên
  const groupedStaffAssets = useMemo(() => {
    if (!isManager || departmentUsers.length === 0) return [];

    return departmentUsers.map(staff => {
      // Tìm tài sản khớp với ID nhân viên
      const staffAssets = departmentAssets.filter(a => a.nguoiDungId === staff.userID);
      return {
        user: staff,
        assets: staffAssets
      };
    }).filter(group => group.assets.length > 0); // Chỉ lấy nhân viên nào đang cầm tài sản
  }, [departmentUsers, departmentAssets, isManager]);

  // ── 5. COMPONENT RENDER TỪNG THẺ TÀI SẢN ──────────────────────────
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
            <span className="font-medium text-gray-900 truncate max-w-[120px]" title={asset.soSeri || ''}>{asset.soSeri || '-'}</span>
          </div>
          <div className="flex justify-between">
            <span className="text-gray-500">Ngày cấp:</span>
            <span className="font-medium text-gray-900">{asset.ngayCapPhat ? new Date(asset.ngayCapPhat).toLocaleDateString('vi-VN') : 'Chưa nhận'}</span>
          </div>
        </div>

        {/* Nút hành động (Chỉ hiện nếu là đồ của mình VÀ đang chờ xác nhận) */}
        {isMyAsset && isPending && (
          <div className="flex gap-2 mt-4 pt-3 border-t border-yellow-200/50">
            <button
              onClick={() => asset.id && handleConfirm(asset.id)}
              className="flex-1 flex items-center justify-center gap-1.5 py-2 bg-yellow-500 hover:bg-yellow-600 text-white text-sm font-bold rounded-lg transition-colors shadow-sm"
            >
              <CheckCircle className="w-4 h-4" /> Nhận
            </button>
            <button
              onClick={() => handleReject(asset)}
              className="flex-1 flex items-center justify-center gap-1.5 py-2 bg-red-500 hover:bg-red-600 text-white text-sm font-bold rounded-lg transition-colors shadow-sm"
            >
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

      {/* TABS (Chỉ hiện nếu là Manager) */}
      {isManager && (
        <div className="flex space-x-1 bg-gray-200/50 p-1 rounded-xl w-fit">
          <button
            onClick={() => setActiveTab('my_assets')}
            className={`flex items-center gap-2 px-5 py-2.5 rounded-lg text-sm font-semibold transition-all ${
              activeTab === 'my_assets' 
                ? 'bg-white text-blue-600 shadow-sm' 
                : 'text-gray-600 hover:text-gray-900 hover:bg-gray-200'
            }`}
          >
            <Package className="w-4 h-4" />
            Tài sản của tôi
          </button>
          <button
            onClick={() => setActiveTab('staff_assets')}
            className={`flex items-center gap-2 px-5 py-2.5 rounded-lg text-sm font-semibold transition-all ${
              activeTab === 'staff_assets' 
                ? 'bg-white text-purple-600 shadow-sm' 
                : 'text-gray-600 hover:text-gray-900 hover:bg-gray-200'
            }`}
          >
            <Users className="w-4 h-4" />
            Tài sản nhân viên
          </button>
        </div>
      )}

      {isLoading ? (
        <div className="text-center py-12 text-gray-500 bg-white rounded-xl border border-gray-200">Đang tải dữ liệu...</div>
      ) : (
        <div className="mt-4">
          
          {/* TAB 1: TÀI SẢN CỦA TÔI */}
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

          {/* TAB 2: TÀI SẢN NHÂN VIÊN */}
          {activeTab === 'staff_assets' && isManager && (
            <div className="space-y-6 animate-in fade-in slide-in-from-bottom-2 duration-300">
              {groupedStaffAssets.length === 0 ? (
                <div className="text-center py-16 bg-white rounded-xl border border-gray-200 shadow-sm flex flex-col items-center">
                  <Users className="w-16 h-16 text-gray-300 mb-4" />
                  <h3 className="text-lg font-semibold text-gray-900">Không có dữ liệu</h3>
                  <p className="text-gray-500 mt-1">Chưa có nhân viên nào trong phòng được cấp phát tài sản.</p>
                </div>
              ) : (
                <div className="space-y-6">
                  {groupedStaffAssets.map((group, index) => (
                    <div key={index} className="bg-white rounded-xl border border-gray-200 overflow-hidden shadow-sm">
                      {/* Tiêu đề nhóm nhân viên */}
                      <div className="bg-gray-50 px-5 py-3 border-b border-gray-200 flex items-center justify-between">
                        <div className="flex items-center gap-3">
                          <div className="w-10 h-10 rounded-full bg-purple-100 text-purple-700 flex items-center justify-center font-bold text-lg border border-purple-200">
                            {group.user.fullName?.charAt(0) || 'U'}
                          </div>
                          <div>
                            <h3 className="font-bold text-gray-900 text-base">{group.user.fullName}</h3>
                            <p className="text-xs text-gray-500 font-medium">@{group.user.userName} • Đang giữ {group.assets.length} tài sản</p>
                          </div>
                        </div>
                      </div>

                      {/* Danh sách tài sản của nhân viên này */}
                      <div className="p-5 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 bg-gray-50/30">
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