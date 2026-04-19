using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TH.Constant
{
    public static class PermissionConstants
    {
        public static readonly Dictionary<string, string> Permissions = new()
        {
            // --- Account & Auth ---
            { "AccountMfaSetup", "account.mfa_setup" },
            { "AccountChangePassword", "account.change_password" },
            { "AuthForgotPassword", "auth.forgot_password" },
            { "AuthLogout", "auth.logout" },
            { "AuthRefresh", "auth.refresh" },
            { "AuthLogin", "auth.login" },
            { "AuthLoginGoogle", "auth.login_google" },
            { "AuthMfaVerify", "auth.mfa_verify" },
            { "AuthRegister", "auth.register" },

            // --- User ---
            { "UserDelete", "user.delete" },
            { "UserReadProfile", "user.read_profile" },
            { "UserUpdateProfile", "user.update_profile" },
            { "UserReadDetails", "user.read_details" },
            { "UserReadDetailsAdmin", "user.read_details.admin" },

            // --- Role & Permission ---
            { "RoleRead", "role.read" },
            { "RoleManage", "role.manage" },
            { "RoleManageAdmin", "role.manage.admin" },
            { "RoleAssign", "role.assign" },
            { "RoleAssignAdmin", "role.assign.admin" },
            { "PermissionManage", "permission.manage" },
            { "PermissionManageAdmin", "permission.manage.admin" },
            { "PermissionRead", "permission.read" },
            { "PermissionReadAdmin", "permission.read.admin" },
            { "PermissionAssign", "permission.assign" },
            { "PermissionAssignAdmin", "permission.assign.admin" },

            { "AuditLogManage", "audit_log.manage" },
            { "UserSessionManage", "usersession.manage" },

            // ============================================
            // ASSET MANAGEMENT PERMISSIONS
            // ============================================

            // --- Tài sản (Assets) ---
            { "AssetView", "asset.view" },                      // Xem danh sách tài sản (all users)
            { "AssetViewOwn", "asset.view_own" },              // Xem tài sản được phân bổ cho mình
            { "AssetViewDepartment", "asset.view_department" }, // Xem tài sản của phòng ban
            { "AssetViewAll", "asset.view_all" },              // Xem tất cả tài sản (kế toán, quản lý)
            { "AssetCreate", "asset.create" },                 // Tạo tài sản mới
            { "AssetUpdate", "asset.update" },                 // Cập nhật thông tin tài sản
            { "AssetDelete", "asset.delete" },                 // Xóa tài sản
            { "AssetImport", "asset.import" },                 // Import tài sản hàng loạt
            { "AssetExport", "asset.export" },                 // Export danh sách tài sản

            // --- Danh mục tài sản (Asset Categories) ---
            { "CategoryView", "category.view" },
            { "CategoryManage", "category.manage" },

            // --- Phòng ban (Departments) ---
            { "DepartmentView", "department.view" },
            { "DepartmentManage", "department.manage" },

            // --- Tài khoản kế toán (Accounting Accounts) ---
            { "AccountingAccountView", "accounting_account.view" },
            { "AccountingAccountManage", "accounting_account.manage" },

            // --- Lô tài sản (Asset Batches) ---
            { "BatchView", "batch.view" },
            { "BatchCreate", "batch.create" },
            { "BatchManage", "batch.manage" },

            // --- Khấu hao (Depreciation) ---
            { "DepreciationView", "depreciation.view" },           // Xem lịch sử khấu hao
            { "DepreciationCalculate", "depreciation.calculate" }, // Tính khấu hao
            { "DepreciationRun", "depreciation.run" },            // Chạy khấu hao tự động
            { "DepreciationManage", "depreciation.manage" },       // Quản lý toàn bộ khấu hao

            // --- Chứng từ (Documents/Vouchers) ---
            { "DocumentView", "document.view" },
            { "DocumentCreate", "document.create" },
            { "DocumentUpdate", "document.update" },
            { "DocumentDelete", "document.delete" },
            { "DocumentApprove", "document.approve" },            // Duyệt chứng từ

            // --- Hạch toán (Accounting Entries) ---
            { "AccountingEntryView", "accounting_entry.view" },
            { "AccountingEntryCreate", "accounting_entry.create" },
            { "AccountingEntryUpdate", "accounting_entry.update" },
            { "AccountingEntryDelete", "accounting_entry.delete" },

            // --- Cấp phát tài sản (Asset Allocation) ---
            { "AllocationRequest", "allocation.request" },         // Tạo yêu cầu cấp phát
            { "AllocationView", "allocation.view" },
            { "AllocationApprove", "allocation.approve" },         // Duyệt cấp phát
            { "AllocationManage", "allocation.manage" },           // Quản lý cấp phát

            // --- Điều chuyển tài sản (Asset Transfer) ---
            { "TransferRequest", "transfer.request" },             // Tạo yêu cầu điều chuyển
            { "TransferView", "transfer.view" },
            { "TransferApprove", "transfer.approve" },             // Duyệt điều chuyển
            { "TransferManage", "transfer.manage" },

            // --- Bảo trì tài sản (Asset Maintenance) ---
            { "MaintenanceRequest", "maintenance.request" },       // Tạo yêu cầu bảo trì/sửa chữa
            { "MaintenanceView", "maintenance.view" },
            { "MaintenanceViewAll", "maintenance.view_all" },
            { "MaintenanceProcess", "maintenance.process" },       // Xử lý bảo trì (kỹ thuật)
            { "MaintenanceUpdateCost", "maintenance.update_cost" }, // Cập nhật chi phí bảo trì
            { "MaintenanceApprove", "maintenance.approve" },       // Duyệt bảo trì
            { "MaintenanceManage", "maintenance.manage" },

            // --- Thanh lý tài sản (Asset Disposal) ---
            { "DisposalRequest", "disposal.request" },             // Đề xuất thanh lý
            { "DisposalView", "disposal.view" },
            { "DisposalApprove", "disposal.approve" },             // Duyệt thanh lý
            { "DisposalManage", "disposal.manage" },
            { "DisposalAccountingEntry", "disposal.accounting_entry" }, // Ghi sổ thanh lý

            // --- Kiểm kê (Inventory/Stocktaking) ---
            { "InventoryView", "inventory.view" },
            { "InventoryCreate", "inventory.create" },             // Tạo phiếu kiểm kê
            { "InventoryExecute", "inventory.execute" },           // Thực hiện kiểm kê
            { "InventoryApprove", "inventory.approve" },           // Duyệt kiểm kê
            { "InventoryManage", "inventory.manage" },

            // --- Báo cáo (Reports) ---
            { "ReportAssetList", "report.asset_list" },           // Báo cáo danh sách tài sản
            { "ReportAssetByDepartment", "report.asset_by_department" },
            { "ReportAssetByCategory", "report.asset_by_category" },
            { "ReportDepreciation", "report.depreciation" },       // Báo cáo khấu hao
            { "ReportMaintenance", "report.maintenance" },         // Báo cáo bảo trì
            { "ReportDisposal", "report.disposal" },              // Báo cáo thanh lý
            { "ReportFinancial", "report.financial" },            // Báo cáo tài chính
            { "ReportDashboard", "report.dashboard" },            // Dashboard tổng quan
            { "ReportExport", "report.export" },                  // Xuất báo cáo

            // --- Cấu hình hệ thống (System Configuration) ---
            { "SystemConfigView", "system_config.view" },
            { "SystemConfigManage", "system_config.manage" },     // Cấu hình hệ thống
            { "SystemHealth", "system.health" },
        };
    }
}