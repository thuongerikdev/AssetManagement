import { useState } from 'react';
import { Users, Shield, Package, CreditCard, Settings as SettingsIcon, Save } from 'lucide-react';

export function AdminPanel() {
  const [activeTab, setActiveTab] = useState<'users' | 'categories' | 'accounts' | 'config'>('users');

  const mockUsers = [
    { id: 1, name: 'Nguyễn Văn A', email: 'a@company.com', role: 'admin', status: 'active' },
    { id: 2, name: 'Trần Thị B', email: 'b@company.com', role: 'accountant', status: 'active' },
    { id: 3, name: 'Lê Văn C', email: 'c@company.com', role: 'it_asset', status: 'active' },
    { id: 4, name: 'Phạm Thị D', email: 'd@company.com', role: 'manager', status: 'active' },
  ];

  const mockCategories = [
    { id: 1, code: 'LAP', name: 'Laptop', depreciationPeriod: 36, account: '2113' },
    { id: 2, code: 'SRV', name: 'Server', depreciationPeriod: 60, account: '2111' },
    { id: 3, code: 'MON', name: 'Monitor', depreciationPeriod: 48, account: '2113' },
    { id: 4, code: 'PRT', name: 'Printer', depreciationPeriod: 36, account: '2113' },
  ];

  const mockAccounts = [
    { id: 1, code: '211', name: 'Tài sản cố định hữu hình', type: 'debit' },
    { id: 2, code: '2111', name: 'Máy móc thiết bị', type: 'debit', parent: '211' },
    { id: 3, code: '2113', name: 'Thiết bị công nghệ', type: 'debit', parent: '211' },
    { id: 4, code: '214', name: 'Hao mòn TSCĐ', type: 'credit' },
    { id: 5, code: '627', name: 'Chi phí sản xuất chung', type: 'debit' },
  ];

  const roleLabels: Record<string, string> = {
    admin: 'Quản trị',
    accountant: 'Kế toán',
    it_asset: 'IT Asset',
    manager: 'Quản lý',
    director: 'Ban giám đốc'
  };

  return (
    <div className="space-y-6">
      <div>
        <h1 className="font-bold text-gray-900">Quản trị Hệ thống</h1>
        <p className="text-sm text-gray-500 mt-1">Cấu hình và quản lý hệ thống</p>
      </div>

      {/* Tabs */}
      <div className="bg-white rounded-lg border border-gray-200">
        <div className="border-b border-gray-200">
          <nav className="flex -mb-px">
            <button
              onClick={() => setActiveTab('users')}
              className={`flex items-center gap-2 px-6 py-4 border-b-2 font-medium text-sm transition-colors ${
                activeTab === 'users'
                  ? 'border-blue-500 text-blue-600'
                  : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
              }`}
            >
              <Users className="w-5 h-5" />
              Quản lý User
            </button>
            <button
              onClick={() => setActiveTab('categories')}
              className={`flex items-center gap-2 px-6 py-4 border-b-2 font-medium text-sm transition-colors ${
                activeTab === 'categories'
                  ? 'border-blue-500 text-blue-600'
                  : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
              }`}
            >
              <Package className="w-5 h-5" />
              Danh mục Tài sản
            </button>
            <button
              onClick={() => setActiveTab('accounts')}
              className={`flex items-center gap-2 px-6 py-4 border-b-2 font-medium text-sm transition-colors ${
                activeTab === 'accounts'
                  ? 'border-blue-500 text-blue-600'
                  : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
              }`}
            >
              <CreditCard className="w-5 h-5" />
              Tài khoản Kế toán
            </button>
            <button
              onClick={() => setActiveTab('config')}
              className={`flex items-center gap-2 px-6 py-4 border-b-2 font-medium text-sm transition-colors ${
                activeTab === 'config'
                  ? 'border-blue-500 text-blue-600'
                  : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
              }`}
            >
              <SettingsIcon className="w-5 h-5" />
              Cấu hình
            </button>
          </nav>
        </div>

        <div className="p-6">
          {/* Users Tab */}
          {activeTab === 'users' && (
            <div className="space-y-4">
              <div className="flex justify-between items-center">
                <h3 className="font-semibold text-gray-900">Quản lý Người dùng</h3>
                <button className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors">
                  Thêm User
                </button>
              </div>

              <div className="overflow-x-auto">
                <table className="w-full">
                  <thead className="bg-gray-50 border-b border-gray-200">
                    <tr>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Họ tên</th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Email</th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Vai trò</th>
                      <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase">Trạng thái</th>
                      <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase">Thao tác</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-gray-200">
                    {mockUsers.map((user) => (
                      <tr key={user.id} className="hover:bg-gray-50">
                        <td className="px-6 py-4 text-sm font-medium text-gray-900">{user.name}</td>
                        <td className="px-6 py-4 text-sm text-gray-600">{user.email}</td>
                        <td className="px-6 py-4 text-sm text-gray-900">{roleLabels[user.role]}</td>
                        <td className="px-6 py-4 text-center">
                          <span className="inline-flex px-2 py-1 text-xs font-medium rounded-full bg-green-100 text-green-700">
                            Đang hoạt động
                          </span>
                        </td>
                        <td className="px-6 py-4 text-center">
                          <button className="text-blue-600 hover:text-blue-800 text-sm mr-3">
                            <Shield className="w-4 h-4 inline" /> Phân quyền
                          </button>
                          <button className="text-gray-600 hover:text-gray-800 text-sm">Sửa</button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>

              {/* Permission Matrix */}
              <div className="mt-6 p-4 bg-gray-50 rounded-lg">
                <h4 className="font-semibold text-gray-900 mb-3">Ma trận Phân quyền</h4>
                <div className="overflow-x-auto">
                  <table className="w-full text-sm">
                    <thead className="bg-white">
                      <tr>
                        <th className="px-4 py-2 text-left font-medium text-gray-700">Module</th>
                        <th className="px-4 py-2 text-center font-medium text-gray-700">Admin</th>
                        <th className="px-4 py-2 text-center font-medium text-gray-700">Kế toán</th>
                        <th className="px-4 py-2 text-center font-medium text-gray-700">IT Asset</th>
                        <th className="px-4 py-2 text-center font-medium text-gray-700">Manager</th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-gray-200">
                      <tr className="bg-white">
                        <td className="px-4 py-2">Quản lý Tài sản</td>
                        <td className="px-4 py-2 text-center text-green-600">✓ Full</td>
                        <td className="px-4 py-2 text-center text-blue-600">✓ Xem</td>
                        <td className="px-4 py-2 text-center text-green-600">✓ Full</td>
                        <td className="px-4 py-2 text-center text-blue-600">✓ Xem</td>
                      </tr>
                      <tr className="bg-white">
                        <td className="px-4 py-2">Khấu hao</td>
                        <td className="px-4 py-2 text-center text-green-600">✓ Full</td>
                        <td className="px-4 py-2 text-center text-green-600">✓ Full</td>
                        <td className="px-4 py-2 text-center text-blue-600">✓ Xem</td>
                        <td className="px-4 py-2 text-center text-blue-600">✓ Xem</td>
                      </tr>
                      <tr className="bg-white">
                        <td className="px-4 py-2">Cấp phát</td>
                        <td className="px-4 py-2 text-center text-green-600">✓ Full</td>
                        <td className="px-4 py-2 text-center text-blue-600">✓ Xem</td>
                        <td className="px-4 py-2 text-center text-green-600">✓ Full</td>
                        <td className="px-4 py-2 text-center text-orange-600">✓ Duyệt</td>
                      </tr>
                      <tr className="bg-white">
                        <td className="px-4 py-2">Bảo trì</td>
                        <td className="px-4 py-2 text-center text-green-600">✓ Full</td>
                        <td className="px-4 py-2 text-center text-green-600">✓ Full</td>
                        <td className="px-4 py-2 text-center text-green-600">✓ Full</td>
                        <td className="px-4 py-2 text-center text-blue-600">✓ Xem</td>
                      </tr>
                      <tr className="bg-white">
                        <td className="px-4 py-2">Báo cáo</td>
                        <td className="px-4 py-2 text-center text-green-600">✓ Full</td>
                        <td className="px-4 py-2 text-center text-green-600">✓ Full</td>
                        <td className="px-4 py-2 text-center text-blue-600">✓ Xem</td>
                        <td className="px-4 py-2 text-center text-green-600">✓ Full</td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          )}

          {/* Categories Tab */}
          {activeTab === 'categories' && (
            <div className="space-y-4">
              <div className="flex justify-between items-center">
                <h3 className="font-semibold text-gray-900">Danh mục Tài sản</h3>
                <button className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors">
                  Thêm Danh mục
                </button>
              </div>

              <div className="overflow-x-auto">
                <table className="w-full">
                  <thead className="bg-gray-50 border-b border-gray-200">
                    <tr>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Mã</th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Tên danh mục</th>
                      <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase">TG Khấu hao (tháng)</th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Tài khoản</th>
                      <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase">Thao tác</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-gray-200">
                    {mockCategories.map((cat) => (
                      <tr key={cat.id} className="hover:bg-gray-50">
                        <td className="px-6 py-4 text-sm font-medium text-blue-600">{cat.code}</td>
                        <td className="px-6 py-4 text-sm text-gray-900">{cat.name}</td>
                        <td className="px-6 py-4 text-sm text-center text-gray-900">{cat.depreciationPeriod}</td>
                        <td className="px-6 py-4 text-sm text-gray-900">{cat.account}</td>
                        <td className="px-6 py-4 text-center">
                          <button className="text-blue-600 hover:text-blue-800 text-sm mr-3">Sửa</button>
                          <button className="text-red-600 hover:text-red-800 text-sm">Xóa</button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* Accounts Tab */}
          {activeTab === 'accounts' && (
            <div className="space-y-4">
              <div className="flex justify-between items-center">
                <h3 className="font-semibold text-gray-900">Danh mục Tài khoản Kế toán</h3>
                <button className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors">
                  Thêm Tài khoản
                </button>
              </div>

              <div className="overflow-x-auto">
                <table className="w-full">
                  <thead className="bg-gray-50 border-b border-gray-200">
                    <tr>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Số TK</th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">Tên tài khoản</th>
                      <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase">Loại</th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-700 uppercase">TK cha</th>
                      <th className="px-6 py-3 text-center text-xs font-medium text-gray-700 uppercase">Thao tác</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-gray-200">
                    {mockAccounts.map((acc) => (
                      <tr key={acc.id} className="hover:bg-gray-50">
                        <td className="px-6 py-4 text-sm font-medium text-blue-600">{acc.code}</td>
                        <td className="px-6 py-4 text-sm text-gray-900">{acc.name}</td>
                        <td className="px-6 py-4 text-center">
                          <span className={`inline-flex px-2 py-1 text-xs font-medium rounded-full ${
                            acc.type === 'debit' ? 'bg-blue-100 text-blue-700' : 'bg-green-100 text-green-700'
                          }`}>
                            {acc.type === 'debit' ? 'Nợ' : 'Có'}
                          </span>
                        </td>
                        <td className="px-6 py-4 text-sm text-gray-600">{acc.parent || '-'}</td>
                        <td className="px-6 py-4 text-center">
                          <button className="text-blue-600 hover:text-blue-800 text-sm mr-3">Sửa</button>
                          <button className="text-red-600 hover:text-red-800 text-sm">Xóa</button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* Config Tab */}
          {activeTab === 'config' && (
            <div className="space-y-6">
              <h3 className="font-semibold text-gray-900">Cấu hình Hệ thống</h3>

              {/* Company Info */}
              <div className="p-6 bg-gray-50 rounded-lg">
                <h4 className="font-semibold text-gray-900 mb-4">Thông tin Công ty</h4>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Tên công ty</label>
                    <input
                      type="text"
                      defaultValue="Công ty TNHH ABC Technology"
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Mã số thuế</label>
                    <input
                      type="text"
                      defaultValue="0123456789"
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    />
                  </div>
                  <div className="md:col-span-2">
                    <label className="block text-sm font-medium text-gray-700 mb-2">Địa chỉ</label>
                    <input
                      type="text"
                      defaultValue="123 Đường ABC, Quận 1, TP.HCM"
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    />
                  </div>
                </div>
              </div>

              {/* Voucher Config */}
              <div className="p-6 bg-gray-50 rounded-lg">
                <h4 className="font-semibold text-gray-900 mb-4">Cấu hình Sinh mã Chứng từ</h4>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Prefix chứng từ</label>
                    <input
                      type="text"
                      defaultValue="CT"
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Số chứng từ bắt đầu</label>
                    <input
                      type="number"
                      defaultValue="001"
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    />
                  </div>
                </div>
              </div>

              {/* Depreciation Config */}
              <div className="p-6 bg-gray-50 rounded-lg">
                <h4 className="font-semibold text-gray-900 mb-4">Cấu hình Khấu hao</h4>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Phương pháp khấu hao mặc định</label>
                    <select className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent">
                      <option value="straight-line">Khấu hao đường thẳng</option>
                      <option value="declining-balance">Khấu hao số dư giảm dần</option>
                      <option value="sum-of-years">Khấu hao tổng số năm sử dụng</option>
                    </select>
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Tự động khấu hao</label>
                    <select className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent">
                      <option value="yes">Có - Tự động vào ngày đầu tháng</option>
                      <option value="no">Không - Khấu hao thủ công</option>
                    </select>
                  </div>
                </div>
              </div>

              {/* Asset Code Config */}
              <div className="p-6 bg-gray-50 rounded-lg">
                <h4 className="font-semibold text-gray-900 mb-4">Cấu hình Mã Tài sản</h4>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Format mã tài sản</label>
                    <input
                      type="text"
                      defaultValue="{CATEGORY}-{NUMBER}"
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    />
                    <p className="text-xs text-gray-500 mt-1">
                      VD: LAP-001, SRV-002
                    </p>
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Số chữ số</label>
                    <input
                      type="number"
                      defaultValue="3"
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    />
                  </div>
                </div>
              </div>

              <div className="flex justify-end">
                <button className="flex items-center gap-2 px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors">
                  <Save className="w-5 h-5" />
                  Lưu Cấu hình
                </button>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
