import { 
  Package, 
  DollarSign, 
  TrendingDown, 
  Wrench,
  AlertCircle,
  TrendingUp
} from 'lucide-react';
import { BarChart, Bar, LineChart, Line, PieChart, Pie, Cell, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';

const statsData = [
  { 
    title: 'Tổng số Tài sản', 
    value: '1,234', 
    icon: Package, 
    color: 'bg-blue-500',
    change: '+12%',
    changeType: 'increase'
  },
  { 
    title: 'Tổng Nguyên giá', 
    value: '15.5 tỷ', 
    icon: DollarSign, 
    color: 'bg-green-500',
    change: '+8%',
    changeType: 'increase'
  },
  { 
    title: 'Giá trị Còn lại', 
    value: '12.3 tỷ', 
    icon: TrendingUp, 
    color: 'bg-purple-500',
    change: '-3%',
    changeType: 'decrease'
  },
  { 
    title: 'Khấu hao Tháng này', 
    value: '285 tr', 
    icon: TrendingDown, 
    color: 'bg-orange-500',
    change: '0%',
    changeType: 'neutral'
  },
];

const departmentData = [
  { name: 'IT', value: 450, color: '#3b82f6' },
  { name: 'Kỹ thuật', value: 320, color: '#10b981' },
  { name: 'Sales', value: 180, color: '#f59e0b' },
  { name: 'Marketing', value: 145, color: '#8b5cf6' },
  { name: 'Admin', value: 139, color: '#ec4899' },
];

const assetTrendData = [
  { month: 'T1', increase: 45, decrease: 12 },
  { month: 'T2', increase: 52, decrease: 8 },
  { month: 'T3', increase: 38, decrease: 15 },
  { month: 'T4', increase: 61, decrease: 10 },
  { month: 'T5', increase: 55, decrease: 18 },
  { month: 'T6', increase: 67, decrease: 9 },
];

const maintenanceCostData = [
  { month: 'T1', cost: 45 },
  { month: 'T2', cost: 62 },
  { month: 'T3', cost: 38 },
  { month: 'T4', cost: 71 },
  { month: 'T5', cost: 55 },
  { month: 'T6', cost: 68 },
];

const upcomingMaintenance = [
  { id: 1, asset: 'Server Dell R740', code: 'SRV-001', date: '2026-02-15', department: 'IT' },
  { id: 2, asset: 'MacBook Pro 16"', code: 'LAP-045', date: '2026-02-18', department: 'Marketing' },
  { id: 3, asset: 'HP Printer LaserJet', code: 'PRT-023', date: '2026-02-20', department: 'Admin' },
  { id: 4, asset: 'Dell Monitor 27"', code: 'MON-156', date: '2026-02-22', department: 'Sales' },
];

const notifications = [
  { id: 1, type: 'warning', message: '15 tài sản cần kiểm kê định kỳ trong tuần này', time: '2 giờ trước' },
  { id: 2, type: 'info', message: 'Đã hoàn thành khấu hao tháng 1/2026', time: '5 giờ trước' },
  { id: 3, type: 'alert', message: '3 tài sản đã quá hạn bảo trì', time: '1 ngày trước' },
];

export function Dashboard() {
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
            <div key={index} className="bg-white rounded-lg border border-gray-200 p-6 hover:shadow-lg transition-shadow">
              <div className="flex items-start justify-between">
                <div className="flex-1">
                  <p className="text-sm text-gray-600 mb-1">{stat.title}</p>
                  <p className="font-bold text-gray-900">{stat.value}</p>
                  <p className={`text-xs mt-2 ${
                    stat.changeType === 'increase' ? 'text-green-600' : 
                    stat.changeType === 'decrease' ? 'text-red-600' : 
                    'text-gray-600'
                  }`}>
                    {stat.change} so với tháng trước
                  </p>
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
          <ResponsiveContainer width="100%" height={300}>
            <PieChart>
              <Pie
                data={departmentData}
                cx="50%"
                cy="50%"
                labelLine={false}
                label={({ name, percent }) => `${name}: ${(percent * 100).toFixed(0)}%`}
                outerRadius={100}
                fill="#8884d8"
                dataKey="value"
              >
                {departmentData.map((entry, index) => (
                  <Cell key={`cell-${index}`} fill={entry.color} />
                ))}
              </Pie>
              <Tooltip />
            </PieChart>
          </ResponsiveContainer>
        </div>

        {/* Asset Trend */}
        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <h3 className="font-semibold text-gray-900 mb-4">Biểu đồ Tăng giảm Tài sản (6 tháng)</h3>
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={assetTrendData}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="month" />
              <YAxis />
              <Tooltip />
              <Legend />
              <Bar dataKey="increase" fill="#10b981" name="Tăng" />
              <Bar dataKey="decrease" fill="#ef4444" name="Giảm" />
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
              <Tooltip />
              <Legend />
              <Line type="monotone" dataKey="cost" stroke="#f59e0b" strokeWidth={2} name="Chi phí" />
            </LineChart>
          </ResponsiveContainer>
        </div>

        {/* Upcoming Maintenance */}
        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <h3 className="font-semibold text-gray-900 mb-4 flex items-center gap-2">
            <Wrench className="w-5 h-5 text-orange-500" />
            Tài sản sắp Bảo trì
          </h3>
          <div className="space-y-3">
            {upcomingMaintenance.map((item) => (
              <div key={item.id} className="flex items-center justify-between p-3 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors">
                <div className="flex-1">
                  <p className="font-medium text-sm text-gray-900">{item.asset}</p>
                  <p className="text-xs text-gray-500 mt-1">
                    {item.code} • {item.department}
                  </p>
                </div>
                <div className="text-right">
                  <p className="text-xs text-gray-600">{new Date(item.date).toLocaleDateString('vi-VN')}</p>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* Notifications */}
      <div className="bg-white rounded-lg border border-gray-200 p-6">
        <h3 className="font-semibold text-gray-900 mb-4 flex items-center gap-2">
          <AlertCircle className="w-5 h-5 text-blue-500" />
          Thông báo & Nhắc nhở
        </h3>
        <div className="space-y-3">
          {notifications.map((notif) => (
            <div key={notif.id} className="flex items-start gap-3 p-3 bg-blue-50 rounded-lg border border-blue-100">
              <AlertCircle className={`w-5 h-5 mt-0.5 ${
                notif.type === 'warning' ? 'text-orange-500' : 
                notif.type === 'alert' ? 'text-red-500' : 
                'text-blue-500'
              }`} />
              <div className="flex-1">
                <p className="text-sm text-gray-900">{notif.message}</p>
                <p className="text-xs text-gray-500 mt-1">{notif.time}</p>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
