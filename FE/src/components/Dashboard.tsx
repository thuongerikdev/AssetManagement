import { useState, useEffect } from 'react';
import {
  Package,
  DollarSign,
  TrendingDown,
  TrendingUp,
} from 'lucide-react';
import {
  BarChart, Bar, LineChart, Line,
  XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer,
} from 'recharts';
import { useGlobalData } from '../context/GlobalContext';
import { maintenanceApi, BaoTriTaiSan } from '../api/maintenanceApi';
import { depreciationHistoryApi, LichSuKhauHao } from '../api/depreciationHistoryApi';
import { liquidationApi, ThanhLyTaiSan } from '../api/liquidationApi';

const DEPT_COLORS = [
  '#3b82f6', // blue
  '#6366f1', // indigo
  '#10b981', // emerald
  '#f59e0b', // amber
  '#8b5cf6', // violet
  '#ec4899', // pink
  '#06b6d4', // cyan
  '#f97316', // orange
];

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

  // ── Department Horizontal Bar ──────────────────────────────────────────────
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

  const departmentRows = Object.entries(deptCounts)
    .sort((a, b) => b[1] - a[1])
    .map(([name, count], i) => ({ name, count, color: DEPT_COLORS[i % DEPT_COLORS.length] }));

  const maxDeptCount = Math.max(...departmentRows.map(r => r.count), 1);

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
        <div className="bg-white rounded-lg border border-gray-200 p-6 h-64 animate-pulse">
          <div className="h-4 bg-gray-200 rounded w-1/3 mb-4" />
          <div className="h-full bg-gray-100 rounded" />
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

      {/* Department Stats — full width horizontal bar */}
      <div className="bg-white rounded-lg border border-gray-200 p-6">
        <h3 className="font-semibold text-gray-900 mb-1">Tài sản theo Phòng ban</h3>

        {departmentRows.length === 0 ? (
          <div className="flex items-center justify-center h-32 text-gray-400 text-sm">
            Chưa có dữ liệu phân bổ phòng ban
          </div>
        ) : (
          <>
            {/* Total */}
            <div className="mb-5">
              <span className="text-4xl font-bold text-gray-900">{totalAssets}</span>
              <span className="text-sm text-gray-400 ml-2">tổng tài sản</span>
            </div>

            {/* Rows */}
            <div className="space-y-3">
              {departmentRows.map((row) => {
                const pct = (row.count / maxDeptCount) * 100;
                return (
                  <div key={row.name} className="flex items-center gap-3">
                    {/* Dot + label */}
                    <div className="flex items-center gap-2 w-48 shrink-0">
                      <span
                        className="w-2.5 h-2.5 rounded-full shrink-0"
                        style={{ backgroundColor: row.color }}
                      />
                      <span className="text-sm text-gray-700 truncate">{row.name}</span>
                    </div>

                    {/* Bar */}
                    <div className="flex-1 bg-gray-100 rounded-full h-2.5 overflow-hidden">
                      <div
                        className="h-full rounded-full transition-all duration-500"
                        style={{ width: `${pct}%`, backgroundColor: row.color }}
                      />
                    </div>

                    {/* Count */}
                    <span className="text-sm font-semibold text-gray-800 w-8 text-right shrink-0">
                      {row.count}
                    </span>
                  </div>
                );
              })}
            </div>
          </>
        )}
      </div>

      {/* Charts Row — Asset Trend + Maintenance Cost */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
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

        {/* Maintenance Cost */}
        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <h3 className="font-semibold text-gray-900 mb-4">Chi phí Bảo trì (triệu VNĐ)</h3>
          <ResponsiveContainer width="100%" height={300}>
            <LineChart data={maintenanceCostData}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="month" />
              <YAxis />
              <Tooltip formatter={(value: number) => [`${value} triệu`, 'Chi phí']} />
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
      </div>
    </div>
  );
}