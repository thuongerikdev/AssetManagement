import { useState, useEffect } from 'react';
import {
  Package,
  DollarSign,
  TrendingDown,
  Wrench,
  TrendingUp,
} from 'lucide-react';
import {
  BarChart, Bar, LineChart, Line, PieChart, Pie, Cell,
  XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer,
} from 'recharts';
import { useGlobalData } from '../context/GlobalContext';
import { maintenanceApi, BaoTriTaiSan } from '../api/maintenanceApi';
import { depreciationHistoryApi, LichSuKhauHao } from '../api/depreciationHistoryApi';
import { liquidationApi, ThanhLyTaiSan } from '../api/liquidationApi';
import { TaiSan } from '../api/assetApi';

const COLORS = ['#3b82f6', '#10b981', '#f59e0b', '#8b5cf6', '#ec4899', '#06b6d4', '#f97316'];

function getLast6Months(): { label: string; key: string }[] {
  const result = [];
  const now = new Date();
  for (let i = 5; i >= 0; i--) {
    const d = new Date(now.getFullYear(), now.getMonth() - i, 1);
    const key = `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}`;
    const label = `T${d.getMonth() + 1}`;
    result.push({ label, key });
  }
  return result;
}

function formatCurrency(value: number): string {
  if (value >= 1_000_000_000) return `${(value / 1_000_000_000).toFixed(1)} tỷ`;
  if (value >= 1_000_000) return `${(value / 1_000_000).toFixed(0)} tr`;
  return value.toLocaleString('vi-VN');
}

function getMonthKey(offset = 0): string {
  const d = new Date();
  d.setMonth(d.getMonth() + offset);
  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}`;
}

export function Dashboard() {
  const { assets, departments, isLoadingGlobal } = useGlobalData();
  const [maintenances, setMaintenances] = useState<BaoTriTaiSan[]>([]);
  const [depreciations, setDepreciations] = useState<LichSuKhauHao[]>([]);
  const [liquidations, setLiquidations] = useState<ThanhLyTaiSan[]>([]);
  const [loadingLocal, setLoadingLocal] = useState(true);

  useEffect(() => {
    Promise.all([
      maintenanceApi.getAll(),
      depreciationHistoryApi.getAll(),
      liquidationApi.getAll(),
    ])
      .then(([maintRes, deprRes, liqRes]) => {
        if (maintRes?.errorCode === 200) setMaintenances(maintRes.data ?? []);
        if (deprRes?.errorCode === 200) setDepreciations(deprRes.data ?? []);
        if (liqRes?.errorCode === 200) setLiquidations(liqRes.data ?? []);
      })
      .catch(console.error)
      .finally(() => setLoadingLocal(false));
  }, []);

  const loading = isLoadingGlobal || loadingLocal;

  // ── Stats ──────────────────────────────────────────────────────────────────
  const totalAssets = assets.length;
  const totalNguyenGia = assets.reduce((s, a) => s + (a.nguyenGia || 0), 0);
  const totalGiaTriConLai = assets.reduce((s, a) => s + (a.giaTriConLai || 0), 0);

  const currentMonthKey = getMonthKey(0);
  const lastMonthKey = getMonthKey(-1);

  const monthlyDepreciation = depreciations
    .filter(d => d.kyKhauHao === currentMonthKey)
    .reduce((s, d) => s + (d.soTien || 0), 0);

  const assetsThisMonth = assets.filter(a => a.ngayTao?.startsWith(currentMonthKey)).length;
  const assetsLastMonth = assets.filter(a => a.ngayTao?.startsWith(lastMonthKey)).length;
  const assetChangePct =
    assetsLastMonth > 0
      ? Math.round(((assetsThisMonth - assetsLastMonth) / assetsLastMonth) * 100)
      : 0;
  const assetChangeLabel =
    assetChangePct > 0 ? `+${assetChangePct}%` : assetChangePct < 0 ? `${assetChangePct}%` : '0%';
  const assetChangeType =
    assetChangePct > 0 ? 'increase' : assetChangePct < 0 ? 'decrease' : 'neutral';

  const statsData = [
    {
      title: 'Tổng số Tài sản',
      value: totalAssets.toLocaleString('vi-VN'),
      icon: Package,
      color: 'bg-blue-500',
      change: assetChangeLabel,
      changeType: assetChangeType,
    },
    {
      title: 'Tổng Nguyên giá',
      value: formatCurrency(totalNguyenGia),
      icon: DollarSign,
      color: 'bg-green-500',
      change: '',
      changeType: 'neutral' as const,
    },
    {
      title: 'Giá trị Còn lại',
      value: formatCurrency(totalGiaTriConLai),
      icon: TrendingUp,
      color: 'bg-purple-500',
      change: totalNguyenGia > 0
        ? `${((totalGiaTriConLai / totalNguyenGia) * 100).toFixed(0)}% nguyên giá`
        : '',
      changeType: 'neutral' as const,
    },
    {
      title: 'Khấu hao Tháng này',
      value: formatCurrency(monthlyDepreciation),
      icon: TrendingDown,
      color: 'bg-orange-500',
      change: '',
      changeType: 'neutral' as const,
    },
  ];

  // ── Department Pie Chart ───────────────────────────────────────────────────
  const deptNameMap = departments.reduce((m, d) => {
    m[String(d.id)] = d.tenPhongBan;
    return m;
  }, {} as Record<string, string>);

  const deptCounts = assets.reduce((m, a) => {
    if (a.phongBanId) {
      const name = deptNameMap[String(a.phongBanId)] || `PB ${a.phongBanId}`;
      m[name] = (m[name] || 0) + 1;
    }
    return m;
  }, {} as Record<string, number>);

  const departmentChartData = Object.entries(deptCounts)
    .sort((a, b) => b[1] - a[1])
    .map(([name, value], i) => ({ name, value, color: COLORS[i % COLORS.length] }));

  // ── Asset Trend (last 6 months) ────────────────────────────────────────────
  const last6 = getLast6Months();

  const assetTrendData = last6.map(({ label, key }) => {
    const increase = assets.filter(a => a.ngayTao?.startsWith(key)).length;
    const decrease = liquidations.filter(l => l.ngayThanhLy?.startsWith(key)).length;
    return { month: label, increase, decrease };
  });

  // ── Maintenance Cost (last 6 months) ──────────────────────────────────────
  const maintenanceCostData = last6.map(({ label, key }) => {
    const cost = maintenances
      .filter(m => m.ngayThucHien?.startsWith(key))
      .reduce((s, m) => s + (m.chiPhi || 0), 0);
    return { month: label, cost: Math.round(cost / 1_000_000) };
  });

  // ── Upcoming Maintenance ───────────────────────────────────────────────────
  const assetMap = assets.reduce((m, a) => {
    if (a.id) m[a.id] = a;
    return m;
  }, {} as Record<number, TaiSan>);

  const today = new Date().toISOString().slice(0, 10);
  const upcomingMaintenanceData = maintenances
    .filter(m => m.ngayThucHien && m.ngayThucHien >= today)
    .sort((a, b) => (a.ngayThucHien || '').localeCompare(b.ngayThucHien || ''))
    .slice(0, 5)
    .map(m => {
      const asset = assetMap[m.taiSanId];
      const dept = asset?.phongBanId ? deptNameMap[String(asset.phongBanId)] : '';
      return {
        id: m.id,
        asset: asset?.tenTaiSan || `Tài sản #${m.taiSanId}`,
        code: asset?.maTaiSan || '',
        date: m.ngayThucHien || '',
        department: dept,
      };
    });

  // ── Loading skeleton ───────────────────────────────────────────────────────
  if (loading) {
    return (
      <div className="space-y-6">
        <div>
          <h1 className="font-bold text-gray-900">Dashboard</h1>
          <p className="text-sm text-gray-500 mt-1">Tổng quan hệ thống quản lý tài sản cố định</p>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          {[...Array(4)].map((_, i) => (
            <div key={i} className="bg-white rounded-lg border border-gray-200 p-6 animate-pulse">
              <div className="h-4 bg-gray-200 rounded w-3/4 mb-3" />
              <div className="h-8 bg-gray-200 rounded w-1/2 mb-2" />
              <div className="h-3 bg-gray-200 rounded w-1/3" />
            </div>
          ))}
        </div>
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
          {[...Array(2)].map((_, i) => (
            <div key={i} className="bg-white rounded-lg border border-gray-200 p-6 h-80 animate-pulse">
              <div className="h-4 bg-gray-200 rounded w-1/2 mb-4" />
              <div className="h-full bg-gray-100 rounded" />
            </div>
          ))}
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="font-bold text-gray-900">Dashboard</h1>
          <p className="text-sm text-gray-500 mt-1">Tổng quan hệ thống quản lý tài sản cố định</p>
        </div>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {statsData.map((stat, index) => {
          const Icon = stat.icon;
          return (
            <div
              key={index}
              className="bg-white rounded-lg border border-gray-200 p-6 hover:shadow-lg transition-shadow"
            >
              <div className="flex items-start justify-between">
                <div className="flex-1">
                  <p className="text-sm text-gray-600 mb-1">{stat.title}</p>
                  <p className="font-bold text-gray-900 text-2xl">{stat.value}</p>
                  {stat.change && (
                    <p
                      className={`text-xs mt-2 ${
                        stat.changeType === 'increase'
                          ? 'text-green-600'
                          : stat.changeType === 'decrease'
                          ? 'text-red-600'
                          : 'text-gray-500'
                      }`}
                    >
                      {stat.change}
                      {stat.changeType !== 'neutral' && ' so với tháng trước'}
                    </p>
                  )}
                </div>
                <div className={`${stat.color} p-3 rounded-lg`}>
                  <Icon className="w-6 h-6 text-white" />
                </div>
              </div>
            </div>
          );
        })}
      </div>

      {/* Charts Row 1 */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Asset by Department */}
        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <h3 className="font-semibold text-gray-900 mb-4">Tài sản theo Phòng ban</h3>
          {departmentChartData.length === 0 ? (
            <div className="flex items-center justify-center h-64 text-gray-400 text-sm">
              Chưa có dữ liệu phân bổ phòng ban
            </div>
          ) : (
            <ResponsiveContainer width="100%" height={300}>
              <PieChart>
                <Pie
                  data={departmentChartData}
                  cx="50%"
                  cy="50%"
                  labelLine={false}
                  label={({ name, percent }) =>
                    percent > 0.05 ? `${name}: ${(percent * 100).toFixed(0)}%` : ''
                  }
                  outerRadius={100}
                  dataKey="value"
                >
                  {departmentChartData.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={entry.color} />
                  ))}
                </Pie>
                <Tooltip formatter={(value: number) => [`${value} tài sản`, 'Số lượng']} />
                <Legend />
              </PieChart>
            </ResponsiveContainer>
          )}
        </div>

        {/* Asset Trend */}
        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <h3 className="font-semibold text-gray-900 mb-4">
            Biểu đồ Tăng giảm Tài sản (6 tháng gần nhất)
          </h3>
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={assetTrendData}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="month" />
              <YAxis allowDecimals={false} />
              <Tooltip />
              <Legend />
              <Bar dataKey="increase" fill="#10b981" name="Tăng" />
              <Bar dataKey="decrease" fill="#ef4444" name="Thanh lý" />
            </BarChart>
          </ResponsiveContainer>
        </div>
      </div>

      {/* Charts Row 2 */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Maintenance Cost */}
        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <h3 className="font-semibold text-gray-900 mb-4">Chi phí Bảo trì (triệu VNĐ)</h3>
          <ResponsiveContainer width="100%" height={300}>
            <LineChart data={maintenanceCostData}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="month" />
              <YAxis />
              <Tooltip
                formatter={(value: number) => [`${value} triệu`, 'Chi phí']}
              />
              <Legend />
              <Line
                type="monotone"
                dataKey="cost"
                stroke="#f59e0b"
                strokeWidth={2}
                name="Chi phí"
                dot={{ r: 4 }}
                activeDot={{ r: 6 }}
              />
            </LineChart>
          </ResponsiveContainer>
        </div>

        {/* Upcoming Maintenance */}
        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <h3 className="font-semibold text-gray-900 mb-4 flex items-center gap-2">
            <Wrench className="w-5 h-5 text-orange-500" />
            Tài sản sắp Bảo trì
          </h3>
          {upcomingMaintenanceData.length === 0 ? (
            <div className="flex items-center justify-center h-48 text-gray-400 text-sm">
              Không có lịch bảo trì sắp tới
            </div>
          ) : (
            <div className="space-y-3">
              {upcomingMaintenanceData.map((item) => (
                <div
                  key={item.id}
                  className="flex items-center justify-between p-3 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors"
                >
                  <div className="flex-1 min-w-0">
                    <p className="font-medium text-sm text-gray-900 truncate">{item.asset}</p>
                    <p className="text-xs text-gray-500 mt-1">
                      {item.code}
                      {item.department && ` • ${item.department}`}
                    </p>
                  </div>
                  <div className="text-right ml-3 shrink-0">
                    <p className="text-xs text-gray-600">
                      {new Date(item.date).toLocaleDateString('vi-VN')}
                    </p>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
