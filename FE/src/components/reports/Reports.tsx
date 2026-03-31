import { useState, useEffect } from 'react';
import { FileText, Download, Calendar, TrendingUp, TrendingDown, DollarSign, Loader2 } from 'lucide-react';
import { BarChart, Bar, LineChart, Line, PieChart, Pie, Cell, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { toast } from 'sonner';

// Import toàn bộ API đã xây dựng
import { assetApi } from '../../api/assetApi';
import { departmentApi } from '../../api/departmentApi';
import { depreciationHistoryApi } from '../../api/depreciationHistoryApi';
import { maintenanceApi } from '../../api/maintenanceApi';
import { liquidationApi } from '../../api/liquidationApi';

export function Reports() {
  const [reportType, setReportType] = useState('asset-by-dept');
  const [fromDate, setFromDate] = useState('2026-01-01');
  const [toDate, setToDate] = useState('2026-12-31');
  const [isLoading, setIsLoading] = useState(true);

  // States lưu trữ dữ liệu đã phân tích cho biểu đồ
  const [assetByDeptData, setAssetByDeptData] = useState<any[]>([]);
  const [assetMovementData, setAssetMovementData] = useState<any[]>([]);
  const [depreciationData, setDepreciationData] = useState<any[]>([]);
  const [maintenanceCostData, setMaintenanceCostData] = useState<any[]>([]);

  useEffect(() => {
    const fetchAndAggregateData = async () => {
      setIsLoading(true);
      try {
        // Gọi 5 API song song để tối ưu tốc độ
        const [assetsRes, deptsRes, depHistRes, maintRes, liqRes] = await Promise.all([
          assetApi.getAll(),
          departmentApi.getAll(),
          depreciationHistoryApi.getAll(),
          maintenanceApi.getAll(),
          liquidationApi.getAll()
        ]);

        const assets = assetsRes.data || [];
        const depts = deptsRes.data || [];
        const depHist = depHistRes.data || [];
        const maints = maintRes.data || [];
        const liqs = liqRes.data || [];

        // 1. Phân tích: Tài sản theo phòng ban
        const deptMap: Record<number, string> = {};
        depts.forEach((d: any) => deptMap[d.id] = d.tenPhongBan);
        
        const assetByDept: Record<string, any> = {};
        assets.forEach((a: any) => {
          const dName = deptMap[a.phongBanId] || 'Chưa cấp phát';
          if (!assetByDept[dName]) assetByDept[dName] = { department: dName, count: 0, value: 0 };
          assetByDept[dName].count += 1;
          assetByDept[dName].value += (a.nguyenGia || 0);
        });
        setAssetByDeptData(Object.values(assetByDept).sort((a, b) => b.value - a.value));

        // 2. Phân tích: Lịch sử Khấu hao theo tháng
        const depMap: Record<string, any> = {};
        depHist.forEach((h: any) => {
          const month = h.kyKhauHao; // Format YYYY-MM
          if (!depMap[month]) depMap[month] = { month, amount: 0 };
          depMap[month].amount += (h.soTien || 0);
        });
        setDepreciationData(Object.values(depMap).sort((a, b) => a.month.localeCompare(b.month)));

        // 3. Phân tích: Chi phí bảo trì theo loại
        const maintMap: Record<string, any> = {};
        const maintTypes: Record<number, string> = { 0: 'Định kỳ', 1: 'Sửa chữa', 2: 'Nâng cấp', 3: 'Vệ sinh', 4: 'Kiểm tra' };
        const maintColors: Record<number, string> = { 0: '#10b981', 1: '#f59e0b', 2: '#3b82f6', 3: '#8b5cf6', 4: '#64748b' };
        
        maints.forEach((m: any) => {
          if (!m.chiPhi) return;
          const tName = maintTypes[m.loaiBaoTri] || 'Khác';
          if (!maintMap[tName]) maintMap[tName] = { category: tName, cost: 0, color: maintColors[m.loaiBaoTri] || '#000' };
          maintMap[tName].cost += m.chiPhi;
        });
        setMaintenanceCostData(Object.values(maintMap));

        // 4. Phân tích: Tăng/Giảm tài sản theo tháng (Từ Mua mới và Thanh lý)
        const moveMap: Record<string, any> = {};
        
        // Ghi nhận Tăng (Dựa vào Ngày Mua)
        assets.forEach((a: any) => {
          if (!a.ngayMua) return;
          const m = a.ngayMua.substring(0, 7); // Cắt lấy YYYY-MM
          if (!moveMap[m]) moveMap[m] = { month: m, increase: 0, decrease: 0, increaseValue: 0, decreaseValue: 0 };
          moveMap[m].increase += 1;
          moveMap[m].increaseValue += (a.nguyenGia || 0);
        });

        // Ghi nhận Giảm (Dựa vào Ngày Thanh lý)
        liqs.forEach((l: any) => {
          if (!l.ngayThanhLy) return;
          const m = l.ngayThanhLy.substring(0, 7);
          if (!moveMap[m]) moveMap[m] = { month: m, increase: 0, decrease: 0, increaseValue: 0, decreaseValue: 0 };
          moveMap[m].decrease += 1;
          moveMap[m].decreaseValue += (l.nguyenGia || 0); // Giảm đi phần nguyên giá tương ứng
        });

        setAssetMovementData(Object.values(moveMap).sort((a, b) => a.month.localeCompare(b.month)));

      } catch (error) {
        toast.error('Lỗi khi phân tích và tổng hợp dữ liệu báo cáo.');
      } finally {
        setIsLoading(false);
      }
    };

    fetchAndAggregateData();
  }, [fromDate, toDate]); // Có thể mở rộng để filter dữ liệu theo ngày

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('vi-VN', {
      style: 'currency',
      currency: 'VND',
      notation: 'compact',
      compactDisplay: 'short'
    }).format(value);
  };

  const formatFullCurrency = (value: number) => {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
  };

  // Tính tổng số liệu cho màn hình Tăng giảm
  const totalIncrease = assetMovementData.reduce((s, m) => s + m.increase, 0);
  const totalIncreaseVal = assetMovementData.reduce((s, m) => s + m.increaseValue, 0);
  const totalDecrease = assetMovementData.reduce((s, m) => s + m.decrease, 0);
  const totalDecreaseVal = assetMovementData.reduce((s, m) => s + m.decreaseValue, 0);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Báo cáo & Thống kê</h1>
          <p className="text-sm text-gray-500 mt-1">Phân tích dữ liệu tài sản theo thời gian thực</p>
        </div>
      </div>

      {/* Report Filter */}
      <div className="bg-white rounded-lg border border-gray-200 p-6">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <div className="md:col-span-2">
            <label className="block text-sm font-medium text-gray-700 mb-2">Loại báo cáo</label>
            <select
              value={reportType}
              onChange={(e) => setReportType(e.target.value)}
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            >
              <option value="asset-by-dept">Báo cáo Tài sản theo Phòng ban</option>
              <option value="asset-movement">Báo cáo Tăng giảm Tài sản</option>
              <option value="depreciation">Báo cáo Lịch sử Khấu hao</option>
              <option value="maintenance-cost">Báo cáo Chi phí Bảo trì</option>
            </select>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">Từ ngày</label>
            <input type="date" value={fromDate} onChange={(e) => setFromDate(e.target.value)} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">Đến ngày</label>
            <input type="date" value={toDate} onChange={(e) => setToDate(e.target.value)} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" />
          </div>
        </div>
        <div className="flex gap-2 mt-4">
          <button className="flex items-center gap-2 px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors">
            {isLoading ? <Loader2 className="w-5 h-5 animate-spin" /> : <FileText className="w-5 h-5" />}
            Tải lại dữ liệu
          </button>
        </div>
      </div>

      {isLoading ? (
        <div className="bg-white rounded-lg border border-gray-200 p-12 text-center flex flex-col items-center justify-center">
          <Loader2 className="w-8 h-8 text-blue-600 animate-spin mb-4" />
          <p className="text-gray-500">Đang tổng hợp dữ liệu hệ thống...</p>
        </div>
      ) : (
        <>
          {/* Report Content */}
          {reportType === 'asset-by-dept' && (
            <div className="space-y-6">
              <div className="bg-white rounded-lg border border-gray-200 p-6">
                <h3 className="font-semibold text-gray-900 mb-4">Tài sản theo Phòng ban</h3>
                <div className="overflow-x-auto">
                  <table className="w-full">
                    <thead className="bg-gray-50 border-b border-gray-200">
                      <tr>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Phòng ban</th>
                        <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase">Số lượng</th>
                        <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase">Tổng giá trị</th>
                        <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase">Tỷ lệ</th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-gray-200">
                      {assetByDeptData.length === 0 && <tr><td colSpan={4} className="text-center py-4">Chưa có dữ liệu</td></tr>}
                      {assetByDeptData.map((dept) => {
                        const total = assetByDeptData.reduce((sum, d) => sum + d.value, 0);
                        const percentage = total > 0 ? ((dept.value / total) * 100).toFixed(1) : 0;
                        return (
                          <tr key={dept.department} className="hover:bg-gray-50">
                            <td className="px-6 py-4 text-sm font-medium text-gray-900">{dept.department}</td>
                            <td className="px-6 py-4 text-sm text-right text-gray-900">{dept.count}</td>
                            <td className="px-6 py-4 text-sm text-right font-medium text-gray-900">{formatFullCurrency(dept.value)}</td>
                            <td className="px-6 py-4 text-sm text-right text-gray-600">{percentage}%</td>
                          </tr>
                        );
                      })}
                    </tbody>
                  </table>
                </div>
              </div>

              {assetByDeptData.length > 0 && (
                <div className="bg-white rounded-lg border border-gray-200 p-6">
                  <h3 className="font-semibold text-gray-900 mb-4">Biểu đồ phân bổ</h3>
                  <ResponsiveContainer width="100%" height={300}>
                    <BarChart data={assetByDeptData}>
                      <CartesianGrid strokeDasharray="3 3" />
                      <XAxis dataKey="department" />
                      <YAxis tickFormatter={(val) => formatCurrency(val)} />
                      <Tooltip formatter={(value: any) => formatFullCurrency(value)} />
                      <Legend />
                      <Bar dataKey="value" fill="#3b82f6" name="Giá trị (VNĐ)" />
                    </BarChart>
                  </ResponsiveContainer>
                </div>
              )}
            </div>
          )}

          {reportType === 'asset-movement' && (
            <div className="space-y-6">
              <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div className="bg-white rounded-lg border border-gray-200 p-6">
                  <div className="flex items-center gap-3">
                    <div className="p-3 bg-green-100 rounded-lg"><TrendingUp className="w-6 h-6 text-green-600" /></div>
                    <div>
                      <p className="text-sm text-gray-600">Tổng Tăng</p>
                      <p className="font-bold text-gray-900">{totalIncrease} tài sản</p>
                      <p className="text-xs text-green-600 mt-1">{formatCurrency(totalIncreaseVal)}</p>
                    </div>
                  </div>
                </div>
                <div className="bg-white rounded-lg border border-gray-200 p-6">
                  <div className="flex items-center gap-3">
                    <div className="p-3 bg-red-100 rounded-lg"><TrendingDown className="w-6 h-6 text-red-600" /></div>
                    <div>
                      <p className="text-sm text-gray-600">Tổng Giảm</p>
                      <p className="font-bold text-gray-900">{totalDecrease} tài sản</p>
                      <p className="text-xs text-red-600 mt-1">{formatCurrency(totalDecreaseVal)}</p>
                    </div>
                  </div>
                </div>
                <div className="bg-white rounded-lg border border-gray-200 p-6">
                  <div className="flex items-center gap-3">
                    <div className="p-3 bg-blue-100 rounded-lg"><DollarSign className="w-6 h-6 text-blue-600" /></div>
                    <div>
                      <p className="text-sm text-gray-600">Tăng ròng</p>
                      <p className="font-bold text-gray-900">{totalIncrease - totalDecrease} tài sản</p>
                      <p className="text-xs text-blue-600 mt-1">{formatCurrency(totalIncreaseVal - totalDecreaseVal)}</p>
                    </div>
                  </div>
                </div>
              </div>

              {assetMovementData.length > 0 && (
                <div className="bg-white rounded-lg border border-gray-200 p-6">
                  <h3 className="font-semibold text-gray-900 mb-4">Biểu đồ Tăng giảm theo tháng</h3>
                  <ResponsiveContainer width="100%" height={300}>
                    <BarChart data={assetMovementData}>
                      <CartesianGrid strokeDasharray="3 3" />
                      <XAxis dataKey="month" />
                      <YAxis />
                      <Tooltip />
                      <Legend />
                      <Bar dataKey="increase" fill="#10b981" name="Số lượng Tăng" />
                      <Bar dataKey="decrease" fill="#ef4444" name="Số lượng Giảm" />
                    </BarChart>
                  </ResponsiveContainer>
                </div>
              )}
            </div>
          )}

          {reportType === 'depreciation' && (
            <div className="space-y-6">
              {depreciationData.length > 0 ? (
                <>
                  <div className="bg-white rounded-lg border border-gray-200 p-6">
                    <h3 className="font-semibold text-gray-900 mb-4">Chi phí Khấu hao theo tháng</h3>
                    <ResponsiveContainer width="100%" height={300}>
                      <LineChart data={depreciationData}>
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="month" />
                        <YAxis tickFormatter={(val) => formatCurrency(val)} />
                        <Tooltip formatter={(value: any) => formatFullCurrency(value)} />
                        <Legend />
                        <Line type="monotone" dataKey="amount" stroke="#f59e0b" strokeWidth={2} name="Khấu hao (VNĐ)" />
                      </LineChart>
                    </ResponsiveContainer>
                  </div>
                  <div className="bg-white rounded-lg border border-gray-200 p-6">
                    <div className="overflow-x-auto">
                      <table className="w-full">
                        <thead className="bg-gray-50 border-b border-gray-200">
                          <tr>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Tháng hạch toán</th>
                            <th className="px-6 py-3 text-right text-xs font-medium text-gray-700 uppercase">Số tiền</th>
                          </tr>
                        </thead>
                        <tbody className="divide-y divide-gray-200">
                          {depreciationData.map((item) => (
                            <tr key={item.month} className="hover:bg-gray-50">
                              <td className="px-6 py-4 text-sm text-gray-900">{item.month}</td>
                              <td className="px-6 py-4 text-sm text-right font-medium text-gray-900">{formatFullCurrency(item.amount)}</td>
                            </tr>
                          ))}
                        </tbody>
                      </table>
                    </div>
                  </div>
                </>
              ) : (
                <div className="bg-white rounded-lg border border-gray-200 p-12 text-center text-gray-500">
                  Chưa có dữ liệu khấu hao nào được ghi nhận.
                </div>
              )}
            </div>
          )}

          {reportType === 'maintenance-cost' && (
            <div className="space-y-6">
              {maintenanceCostData.length > 0 ? (
                <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
                  <div className="bg-white rounded-lg border border-gray-200 p-6">
                    <h3 className="font-semibold text-gray-900 mb-4">Chi phí theo Loại</h3>
                    <ResponsiveContainer width="100%" height={300}>
                      <PieChart>
                        <Pie
                          data={maintenanceCostData}
                          cx="50%" cy="50%"
                          labelLine={false}
                          label={({ name, percent }) => `${name}: ${(percent * 100).toFixed(0)}%`}
                          outerRadius={100}
                          dataKey="cost"
                        >
                          {maintenanceCostData.map((entry, index) => (
                            <Cell key={`cell-${index}`} fill={entry.color} />
                          ))}
                        </Pie>
                        <Tooltip formatter={(value: any) => formatFullCurrency(value)} />
                      </PieChart>
                    </ResponsiveContainer>
                  </div>

                  <div className="bg-white rounded-lg border border-gray-200 p-6">
                    <h3 className="font-semibold text-gray-900 mb-4">Chi tiết Chi phí</h3>
                    <div className="space-y-4">
                      {maintenanceCostData.map((item) => (
                        <div key={item.category} className="flex items-center justify-between p-4 bg-gray-50 rounded-lg">
                          <div className="flex items-center gap-3">
                            <div className="w-4 h-4 rounded" style={{ backgroundColor: item.color }}></div>
                            <span className="text-sm font-medium text-gray-900">{item.category}</span>
                          </div>
                          <span className="text-sm font-semibold text-gray-900">{formatFullCurrency(item.cost)}</span>
                        </div>
                      ))}
                      <div className="flex items-center justify-between p-4 bg-blue-50 rounded-lg border-2 border-blue-200">
                        <span className="text-sm font-semibold text-blue-900">Tổng cộng</span>
                        <span className="font-bold text-blue-900">
                          {formatFullCurrency(maintenanceCostData.reduce((sum, d) => sum + d.cost, 0))}
                        </span>
                      </div>
                    </div>
                  </div>
                </div>
              ) : (
                <div className="bg-white rounded-lg border border-gray-200 p-12 text-center text-gray-500">
                  Chưa có chi phí bảo trì nào được ghi nhận.
                </div>
              )}
            </div>
          )}
        </>
      )}
    </div>
  );
}