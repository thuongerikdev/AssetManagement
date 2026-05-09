import { useState, useEffect, useMemo } from 'react';
import { Monitor, CheckCircle, Package, AlertTriangle, Clock, RefreshCw, XCircle, Users, ChevronDown } from 'lucide-react';
import { toast } from 'sonner';
import { assetApi, TaiSan } from '../../api/assetApi';
import { authApi } from '../../api/authApi';
import { assetAllocationApi } from '../../api/assetAllocationApi';

export function MyAssets() {
  const [myAssets, setMyAssets] = useState<TaiSan[]>([]);
  
  // State cho Trưởng phòng
  const [departmentUsers, setDepartmentUsers] = useState<any[]>([]);
  const [departmentAssets, setDepartmentAssets] = useState<TaiSan[]>([]);
  
  const [isLoading, setIsLoading] = useState(true);

  // ── 1. ĐỌC THÔNG TIN USER ──────────────────────────────────────────
  const userInfoStr = localStorage.getItem('user_info');
  const userInfo = userInfoStr ? JSON.parse(userInfoStr) : null;
  const currentUserId = userInfo?.userID;
  const currentDeptId = userInfo?.departmentID; // departmentID thường lưu kiểu string hoặc number
  
  // Kiểm tra xem User này có phải Trưởng phòng / Giám đốc không?
  const isManager = useMemo(() => {
    const roles: string[] = userInfo?.roles || [];
    return roles.some(r => r.toLowerCase().includes('truong_phong') || r.toLowerCase().includes('giam_doc'));
  }, [userInfo]);

  // ── 2. FETCH DỮ LIỆU ──────────────────────────────────────────────
  const fetchData = async () => {
    if (!currentUserId) return;
    setIsLoading(true);

    try {
      // Gọi API mặc định: Tài sản của chính mình
      const myAssetsPromise = assetApi.getMyAssets(currentUserId);
      
      let deptUsersPromise = Promise.resolve({ data: [] });
      let deptAssetsPromise = Promise.resolve({ data: [] });

      // Nếu là quản lý và có ID phòng ban -> Gọi thêm API phòng ban
      if (isManager && currentDeptId) {
        deptUsersPromise = authApi.getUsersByDepartment(Number(currentDeptId));
        deptAssetsPromise = assetApi.getAssetsByDepartment(Number(currentDeptId));
      }

      // Chạy song song cho mượt
      const [myRes, usersRes, assetsRes] = await Promise.all([myAssetsPromise, deptUsersPromise, deptAssetsPromise]);

      if (myRes.errorCode === 200) setMyAssets(myRes.data || []);
      
      if (isManager) {
        // Lấy danh sách nhân sự (LOẠI TRỪ CHÍNH QUẢN LÝ RA ĐỂ KHÔNG TRÙNG LẶP)
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

  // ── 3. HÀM XỬ LÝ (KÝ NHẬN / TỪ CHỐI) ───────────────────────────────
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
    // ... Giữ nguyên logic Reject cũ của bạn ...
    toast.info("Tính năng từ chối (Vui lòng tự ghép logic reject cũ vào đây)");
  };

  // ── 4. NHÓM DỮ LIỆU ĐỂ RENDER ─────────────────────────────────────
  // 4.1: Tài sản cá nhân
  const { pendingAssets, activeAssets } = useMemo(() => {
    return {
      pendingAssets: myAssets.filter(a => a.trangThai?.toString() === 'ChoXacNhan' || a.trangThai?.toString() === '1'),
      activeAssets: myAssets.filter(a => a.trangThai?.toString() === 'DangSuDung' || a.trangThai?.toString() === '2')
    };
  }, [myAssets]);

  // 4.2: Nhóm tài sản nhân viên theo Từng User
  const groupedStaffAssets = useMemo(() => {
    if (!isManager || departmentUsers.length === 0) return [];

    return departmentUsers.map(staff => {
      // Tìm các tài sản thuộc về nhân viên này
      const staffAssets = departmentAssets.filter(a => a.nguoiDungId === staff.userID);
      return {
        user: staff,
        assets: staffAssets
      };
    }).filter(group => group.assets.length > 0); // Chỉ hiển thị nhân viên nào CÓ tài sản
  }, [departmentUsers, departmentAssets, isManager]);

  // ── RENDER COMPONENT TÀI SẢN (DÙNG CHUNG) ─────────────────────────
  const RenderAssetCard = ({ asset, isMyAsset }: { asset: TaiSan, isMyAsset: boolean }) => {
    const isPending = asset.trangThai?.toString() === 'ChoXacNhan' || asset.trangThai?.toString() === '1';

    return (
      <div className={`rounded-xl border p-5 shadow-sm relative overflow-hidden ${isPending ? 'bg-yellow-50 border-yellow-200' : 'bg-white border-gray-200 hover:shadow-md'}`}>
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
            <span className="font-medium text-gray-900">{asset.soSeri || '-'}</span>
          </div>
          <div className="flex justify-between">
            <span className="text-gray-500">Ngày cấp:</span>
            <span className="font-medium text-gray-900">{asset.ngayCapPhat ? new Date(asset.ngayCapPhat).toLocaleDateString('vi-VN') : 'Chưa nhận'}</span>
          </div>
        </div>

        {/* Nút hành động (Chỉ hiển thị nếu là tài sản CỦA MÌNH và đang CHỜ XÁC NHẬN) */}
        {isMyAsset && isPending && (
          <div className="flex gap-2 mt-4 pt-3 border-t border-yellow-200/50">
            <button
              onClick={() => asset.id && handleConfirm(asset.id)}
              className="flex-1 flex items-center justify-center gap-2 py-2 bg-yellow-500 hover:bg-yellow-600 text-white text-sm font-semibold rounded-lg transition-colors shadow-sm"
            >
              <CheckCircle className="w-4 h-4" /> Ký nhận
            </button>
            <button
              onClick={() => handleReject(asset)}
              className="flex-1 flex items-center justify-center gap-2 py-2 bg-red-500 hover:bg-red-600 text-white text-sm font-semibold rounded-lg transition-colors shadow-sm"
            >
              <XCircle className="w-4 h-4" /> Từ chối
            </button>
          </div>
        )}
      </div>
    );
  };

  return (
    <div className="space-y-8 max-w-5xl mx-auto py-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Tài sản của tôi</h1>
          <p className="text-gray-500 mt-1">Quản lý các tài sản cá nhân {isManager ? 'và tài sản của nhân sự trong phòng ban.' : 'được công ty cấp phát.'}</p>
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

      {isLoading ? (
        <div className="text-center py-12 text-gray-500 bg-white rounded-xl border border-gray-200">Đang tải dữ liệu...</div>
      ) : (
        <div className="space-y-10">
          
          {/* ========================================================
              PHẦN 1: TÀI SẢN CỦA TÔI
          ========================================================= */}
          <section>
            <h2 className="text-lg font-bold text-blue-800 flex items-center gap-2 mb-4 bg-blue-50 py-2 px-3 rounded-lg border border-blue-100 w-fit">
              <Package className="w-5 h-5 text-blue-600" />
              Tài sản của tôi
            </h2>

            {myAssets.length === 0 ? (
              <p className="text-sm text-gray-500 italic px-2">Bạn chưa được cấp phát tài sản nào.</p>
            ) : (
              <div className="space-y-6">
                {pendingAssets.length > 0 && (
                  <div>
                    <h3 className="text-sm font-semibold text-yellow-700 flex items-center gap-2 mb-3">
                      <AlertTriangle className="w-4 h-4" /> Cần xác nhận bàn giao ({pendingAssets.length})
                    </h3>
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                      {pendingAssets.map(a => <RenderAssetCard key={a.id} asset={a} isMyAsset={true} />)}
                    </div>
                  </div>
                )}

                {activeAssets.length > 0 && (
                  <div>
                    <h3 className="text-sm font-semibold text-green-700 flex items-center gap-2 mb-3">
                      <CheckCircle className="w-4 h-4" /> Đang sử dụng ({activeAssets.length})
                    </h3>
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                      {activeAssets.map(a => <RenderAssetCard key={a.id} asset={a} isMyAsset={true} />)}
                    </div>
                  </div>
                )}
              </div>
            )}
          </section>

          {/* ========================================================
              PHẦN 2: TÀI SẢN CỦA NHÂN VIÊN (CHỈ HIỆN VỚI TRƯỞNG PHÒNG)
          ========================================================= */}
          {isManager && (
            <section className="pt-6 border-t border-gray-200">
              <h2 className="text-lg font-bold text-purple-800 flex items-center gap-2 mb-6 bg-purple-50 py-2 px-3 rounded-lg border border-purple-100 w-fit">
                <Users className="w-5 h-5 text-purple-600" />
                Tài sản của nhân viên trong phòng
              </h2>

              {groupedStaffAssets.length === 0 ? (
                <div className="text-center py-10 bg-white rounded-xl border border-gray-200 shadow-sm flex flex-col items-center">
                  <Package className="w-12 h-12 text-gray-300 mb-3" />
                  <p className="text-gray-500">Chưa có nhân viên nào trong phòng được cấp phát tài sản.</p>
                </div>
              ) : (
                <div className="space-y-6">
                  {groupedStaffAssets.map((group, index) => (
                    <div key={index} className="bg-white rounded-xl border border-gray-200 overflow-hidden shadow-sm">
                      {/* Tiêu đề nhóm nhân viên */}
                      <div className="bg-gray-50 px-4 py-3 border-b border-gray-200 flex justify-between items-center">
                        <div className="flex items-center gap-3">
                          <div className="w-8 h-8 rounded-full bg-purple-100 text-purple-600 flex items-center justify-center font-bold text-sm">
                            {group.user.fullName?.charAt(0) || 'U'}
                          </div>
                          <div>
                            <h3 className="font-bold text-gray-900">{group.user.fullName}</h3>
                            <p className="text-xs text-gray-500">@{group.user.userName} • {group.assets.length} tài sản</p>
                          </div>
                        </div>
                      </div>

                      {/* Danh sách tài sản của nhân viên */}
                      <div className="p-4 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                        {group.assets.map(asset => (
                          <RenderAssetCard key={asset.id} asset={asset} isMyAsset={false} />
                        ))}
                      </div>
                    </div>
                  ))}
                </div>
              )}
            </section>
          )}

        </div>
      )}
    </div>
  );
}