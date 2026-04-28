import { createBrowserRouter } from "react-router";
import { Layout } from "./components/Layout";
import { Dashboard } from "./components/Dashboard";
import { AssetList } from "./components/assets/AssetList";
import { AssetGroupView } from "./components/assets/AssetGroupView";
import { AssetBatchView } from "./components/assets/AssetBatchView";
import { AssetForm } from "./components/assets/AssetForm";
import { AssetDetail } from "./components/assets/AssetDetail";
import { DepreciationList } from "./components/depreciation/DepreciationList";
import { AllocationList } from "./components/allocation/AllocationList";
import { MaintenanceList } from "./components/maintenance/MaintenanceList";
import { MaintenanceForm } from "./components/maintenance/MaintenanceForm";
import { LiquidationList } from "./components/liquidation/LiquidationList";
import { LiquidationForm } from "./components/liquidation/LiquidationForm";
import { VoucherList } from "./components/vouchers/VoucherList";
import { VoucherDetail } from "./components/vouchers/VoucherDetail";
import { Reports } from "./components/reports/Reports";
import { AdminPanel } from "./components/admin/AdminPanel";
import { NotFound } from "./components/NotFound";
import { DepartmentList } from "./components/settings/DepartmentList";
import { AssetCategoryList } from "./components/settings/AssetCategoryList";
import { SystemConfig } from "./components/settings/SystemConfig";
import { AccountList } from "./components/settings/AccountList";
import { MyAssets } from './components/assets/MyAssets';
import { LoginPage } from "./components/login/LoginPage";
import { GeneralJournal } from './components/vouchers/GeneralJournal';

import { UserList } from "./components/settings/UserList";
import { RoleList } from "./components/settings/RoleList";
import { PermissionList } from "./components/settings/PermissionList";

// MỚI: Import thêm component ProtectedRoute bạn đã tạo
// Nhớ sửa lại đường dẫn import nếu bạn đặt file này ở thư mục khác
import { ProtectedRoute } from "./components/ProtectedRoute";

export const router = createBrowserRouter([
  { 
    path: "/login", 
    Component: LoginPage 
  },
  {
    // BỌC PROTECTED ROUTE TẠI ĐÂY LÀM CHA CỦA LAYOUT
    Component: ProtectedRoute,
    children: [
      {
        path: "/",
        Component: Layout,
        children: [
          { index: true, Component: Dashboard },
          { path: "assets", Component: AssetList },
          { path: "assets/groups", Component: AssetGroupView },
          { path: "assets/batch", Component: AssetBatchView },
          { path: "assets/new", Component: AssetForm },
          { path: "assets/:id", Component: AssetDetail },
          { path: "assets/:id/edit", Component: AssetForm },
          { path: "depreciation", Component: DepreciationList },
          { path: "allocation", Component: AllocationList },
          { path: "maintenance", Component: MaintenanceList },
          { path: "maintenance/new", Component: MaintenanceForm },
          { path: "liquidation", Component: LiquidationList },
          { path: "liquidation/new", Component: LiquidationForm },
          { path: "vouchers", Component: VoucherList },
          { path: "vouchers/:id", Component: VoucherDetail },
          { path: "reports", Component: Reports },
          { path: "admin", Component: AdminPanel },
          { path: "settings/departments", Component: DepartmentList },
          { path: "settings/categories", Component: AssetCategoryList },
          { path: "settings/system", Component: SystemConfig },
          { path: "settings/accounts", Component: AccountList },
          { path: "my-assets", Component: MyAssets },
          { path: "ledger", Component: GeneralJournal },
          { path: "settings/users", Component: UserList },
          { path: "settings/roles", Component: RoleList },
          { path: "settings/permissions", Component: PermissionList },
          { path: "*", Component: NotFound },
        ],
      }
    ]
  },
]);