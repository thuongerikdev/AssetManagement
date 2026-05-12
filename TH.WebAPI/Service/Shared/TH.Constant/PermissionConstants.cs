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
            // ============================================
            // AUTH - Login (api/Login)
            // ============================================
            { "AuthLogin",                  "auth.login" },                    // POST userLogin
            { "AuthLoginStaff",             "auth.login_staff" },              // POST StaffLogin
            { "AuthLoginMobile",            "auth.login_mobile" },             // POST login/mobile
            { "AuthLoginGoogle",            "auth.login_google" },             // GET  google-login
            { "AuthLoginGoogleCallback",    "auth.login_google_callback" },    // GET  google/callback
            { "AuthSigninGoogle",           "auth.signin_google" },            // GET  signin-google
            { "AuthLoginMobileGoogle",      "auth.login_mobile_google" },      // POST login/mobile/google
            { "AuthMfaVerify",              "auth.mfa_verify" },               // POST mfa/verify
            { "AuthRefresh",                "auth.refresh" },                  // POST auth/refresh
            { "AuthLogout",                 "auth.logout" },                   // POST logout
            { "AuthLogoutSession",          "auth.logout_session" },           // POST logout/session/{sessionId}
            { "AuthLogoutAll",              "auth.logout_all" },               // POST logout/all

            // ============================================
            // AUTH - Register (api/Register)
            // ============================================
            { "AuthRegister",               "auth.register" },                 // POST Register
            { "AuthVerifyRegisterEmail",    "auth.verify_register_email" },    // POST verifyRegisterEmail
            { "AuthCreateUser",             "auth.create_user" },              // POST createUser

            // ============================================
            // AUTH - Account (api/Account)
            // ============================================
            { "AccountMfaTotpStart",              "account.mfa_totp_start" },              // POST mfa/totp/start
            { "AccountMfaTotpConfirm",            "account.mfa_totp_confirm" },            // POST mfa/totp/confirm
            { "AccountMfaTotpDisable",            "account.mfa_totp_disable" },            // POST mfa/totp/disable
            { "AccountPasswordChangeEmailStart",  "account.password_change_email_start" }, // POST password/change/email/start
            { "AccountPasswordChangeEmailVerify", "account.password_change_email_verify"}, // POST password/change/email/verify
            { "AccountPasswordChangeMfaVerify",   "account.password_change_mfa_verify" },  // POST password/change/mfa/verify
            { "AccountPasswordChangeCommit",      "account.password_change_commit" },      // POST password/change/commit
            { "AccountForgotEmailStart",          "account.forgot_email_start" },          // POST password/forgot/email/start
            { "AccountForgotEmailVerify",         "account.forgot_email_verify" },         // POST password/forgot/email/verify
            { "AccountForgotMfaVerify",           "account.forgot_mfa_verify" },           // POST password/forgot/mfa/verify
            { "AccountForgotCommit",              "account.forgot_commit" },               // POST password/forgot/commit

            // ============================================
            // USER (api/User)
            // ============================================
            { "UserGetMe",              "user.get_me" },              // GET  me
            { "UserGetAll",             "user.get_all" },             // GET  getAllUsers
            { "UserGetAllSlim",         "user.get_all_slim" },        // GET  getAllUsersSlim
            { "UserGetSlimById",        "user.get_slim_by_id" },      // GET  GetUserSlimById/{userID}
            { "UserGetByDepartmentId",  "user.get_by_department_id"}, // GET  GetByDepartmentId/{departmentID}
            { "UserUpdateProfile",      "user.update_profile" },      // PUT  update/profile
            { "UserUpdateUsername",     "user.update_username" },     // PUT  update/username
            { "UserDelete",             "user.delete" },              // DELETE deleteUser
            { "UserAdminGetAll",        "user.admin_get_all" },       // GET  admin/getAllUsers
            { "UserAdminGetSlimById",   "user.admin_get_slim_by_id"}, // GET  admin/GetUserSlimById/{userID}
            { "UserAdminGetById",       "user.admin_get_by_id" },     // GET  admin/getUserById

            // ============================================
            // ROLE (api/Role)
            // ============================================
            { "RoleGetAll",         "role.get_all" },           // GET  getall
            { "RoleGetAllScopeUser","role.get_all_scope_user" }, // GET  getallscope-user
            { "RoleGetByUserId",    "role.get_by_user_id" },    // GET  getRoleByUserID/{userID}
            { "RoleAdd",            "role.add" },               // POST addRole
            { "RoleUpdate",         "role.update" },            // PUT  updateRole
            { "RoleDelete",         "role.delete" },            // DELETE deleteRole/{roleID}
            { "RoleClone",          "role.clone" },             // POST clonerole
            { "RoleAdminAdd",       "role.admin_add" },         // POST admin/addRole
            { "RoleAdminUpdate",    "role.admin_update" },      // PUT  admin/updateRole
            { "RoleAdminDelete",    "role.admin_delete" },      // DELETE admin/deleteRole/{roleID}
            { "RoleAdminClone",     "role.admin_clone" },       // POST admin/clonerole

            // ============================================
            // PERMISSION (api/Permission)
            // ============================================
            { "PermissionGetAll",           "permission.get_all" },             // GET  getall
            { "PermissionGetById",          "permission.get_by_id" },           // GET  getbyid/{permissionId}
            { "PermissionGetByUserId",      "permission.get_by_user_id" },      // GET  getbyUserID/{ID}
            { "PermissionGetByRoleId",      "permission.get_by_role_id" },      // GET  getbyRoleID/{ID}
            { "PermissionBulkCreate",       "permission.bulk_create" },         // POST BulkCreate
            { "PermissionUpdate",           "permission.update" },              // PUT  updatePermission
            { "PermissionDelete",           "permission.delete" },              // DELETE delete
            { "PermissionAdminGetAll",      "permission.admin_get_all" },       // GET  admin/getall
            { "PermissionAdminGetById",     "permission.admin_get_by_id" },     // GET  admin/getbyid/{permissionId}
            { "PermissionAdminGetByUserId", "permission.admin_get_by_user_id"}, // GET  admin/getbyUserID/{ID}
            { "PermissionAdminGetByRoleId", "permission.admin_get_by_role_id"}, // GET  admin/getbyRoleID/{ID}
            { "PermissionAdminBulkCreate",  "permission.admin_bulk_create" },   // POST admin/BulkCreate
            { "PermissionAdminAdd",         "permission.admin_add" },           // POST admin/addPermission
            { "PermissionAdminUpdate",      "permission.admin_update" },        // PUT  admin/updatePermission
            { "PermissionAdminDelete",      "permission.admin_delete" },        // DELETE admin/delete

            // ============================================
            // ROLE-PERMISSION (api/RolePermission)
            // ============================================
            { "RolePermissionAssign",       "role_permission.assign" },         // POST assign-permissions
            { "RolePermissionAdminAssign",  "role_permission.admin_assign" },   // POST admin/assign-permissions

            // ============================================
            // USER-ROLE (api/UserRole)
            // ============================================
            { "UserRoleAssign",             "user_role.assign" },               // POST assign-roles
            { "UserRoleAdminAssign",        "user_role.admin_assign" },         // POST admin/assign-roles

            // ============================================
            // USER SESSION (api/UserSession)
            // ============================================
            { "UserSessionGetAll",          "user_session.get_all" },           // GET getall
            { "UserSessionGetByUserId",     "user_session.get_by_user_id" },    // GET getByUserId/{userId}
            { "UserSessionGetBySessionId",  "user_session.get_by_session_id" }, // GET getBySessionId/{sessionId}

            // ============================================
            // AUDIT LOG (api/AuditLog)
            // ============================================
            { "AuditLogGetAll",         "audit_log.get_all" },          // GET getall
            { "AuditLogGetById",        "audit_log.get_by_id" },        // GET getbyid/{id}
            { "AuditLogGetByUserId",    "audit_log.get_by_user_id" },   // GET getbyuser/{userId}

            // ============================================
            // TÀI SẢN (api/TaiSan)
            // ============================================
            { "TaiSanGetAll",       "tai_san.get_all" },        // GET  get-all
            { "TaiSanGetById",      "tai_san.get_by_id" },      // GET  get/{id}
            { "TaiSanGetMine",      "tai_san.get_mine" },       // GET  my-assets/{userId}
            { "TaiSanGenerateCode", "tai_san.generate_code" },  // GET  generate-code
            { "TaiSanCreate",       "tai_san.create" },         // POST create
            { "TaiSanConfirm",      "tai_san.confirm" },        // POST confirm/{id}
            { "TaiSanReject",       "tai_san.reject" },         // POST reject/{id}
            { "TaiSanUpdate",       "tai_san.update" },         // PUT  update
            { "TaiSanDelete",       "tai_san.delete" },         // DELETE delete/{id}

            // ============================================
            // DANH MỤC TÀI SẢN (api/DanhMucTaiSan)
            // ============================================
            { "DanhMucTaiSanGetAll",    "danh_muc_tai_san.get_all" },   // GET  get-all
            { "DanhMucTaiSanGetById",   "danh_muc_tai_san.get_by_id" }, // GET  get/{id}
            { "DanhMucTaiSanCreate",    "danh_muc_tai_san.create" },    // POST create
            { "DanhMucTaiSanUpdate",    "danh_muc_tai_san.update" },    // PUT  update
            { "DanhMucTaiSanDelete",    "danh_muc_tai_san.delete" },    // DELETE delete/{id}

            // ============================================
            // PHÒNG BAN (api/PhongBan)
            // ============================================
            { "PhongBanGetAll",     "phong_ban.get_all" },      // GET  get-all
            { "PhongBanGetById",    "phong_ban.get_by_id" },    // GET  get/{id}
            { "PhongBanCreate",     "phong_ban.create" },       // POST create
            { "PhongBanUpdate",     "phong_ban.update" },       // PUT  update
            { "PhongBanDelete",     "phong_ban.delete" },       // DELETE delete/{id}

            // ============================================
            // TÀI KHOẢN KẾ TOÁN (api/TaiKhoanKeToan)
            // ============================================
            { "TaiKhoanKeToanGetAll",   "tai_khoan_ke_toan.get_all" },   // GET  get-all
            { "TaiKhoanKeToanGetById",  "tai_khoan_ke_toan.get_by_id" }, // GET  get/{id}
            { "TaiKhoanKeToanCreate",   "tai_khoan_ke_toan.create" },    // POST create
            { "TaiKhoanKeToanUpdate",   "tai_khoan_ke_toan.update" },    // PUT  update
            { "TaiKhoanKeToanDelete",   "tai_khoan_ke_toan.delete" },    // DELETE delete/{id}

            // ============================================
            // LỊCH SỬ KHẤU HAO (api/LichSuKhauHao)
            // ============================================
            { "LichSuKhauHaoGetAll",        "lich_su_khau_hao.get_all" },       // GET  get-all
            { "LichSuKhauHaoGetById",       "lich_su_khau_hao.get_by_id" },     // GET  get/{id}
            { "LichSuKhauHaoGetByAsset",    "lich_su_khau_hao.get_by_asset" },  // GET  get-by-asset/{taiSanId}
            { "LichSuKhauHaoCreate",        "lich_su_khau_hao.create" },        // POST create
            { "LichSuKhauHaoCreateBulk",    "lich_su_khau_hao.create_bulk" },   // POST create-bulk
            { "LichSuKhauHaoUpdate",        "lich_su_khau_hao.update" },        // PUT  update
            { "LichSuKhauHaoDelete",        "lich_su_khau_hao.delete" },        // DELETE delete/{id}

            // ============================================
            // CHỨNG TỪ (api/ChungTu)
            // ============================================
            { "ChungTuGetAll",          "chung_tu.get_all" },          // GET  get-all
            { "ChungTuGetById",         "chung_tu.get_by_id" },        // GET  get/{id}
            { "ChungTuGetByAsset",      "chung_tu.get_by_asset" },     // GET  get-by-asset/{taiSanId}
            { "ChungTuGenerateCode",    "chung_tu.generate_code" },    // GET  generate-code
            { "ChungTuCreate",          "chung_tu.create" },           // POST create
            { "ChungTuUpdate",          "chung_tu.update" },           // PUT  update
            { "ChungTuDelete",          "chung_tu.delete" },           // DELETE delete/{id}

            // ============================================
            // SỔ CÁI (api/SoCai)
            // ============================================
            { "SoCaiGetTomTat",     "so_cai.get_tom_tat" },    // GET tom-tat
            { "SoCaiGetChiTiet",    "so_cai.get_chi_tiet" },   // GET chi-tiet/{maTaiKhoan}

            // ============================================
            // TÀI SẢN ĐÍNH KÈM (api/TaiSanDinhKem)
            // ============================================
            { "TaiSanDinhKemGetByAsset",    "tai_san_dinh_kem.get_by_asset" }, // GET  by-asset/{taiSanId}
            { "TaiSanDinhKemUpload",        "tai_san_dinh_kem.upload" },       // POST upload/{taiSanId}
            { "TaiSanDinhKemDelete",        "tai_san_dinh_kem.delete" },       // DELETE {id}

            // ============================================
            // THANH LÝ TÀI SẢN (api/ThanhLyTaiSan)
            // ============================================
            { "ThanhLyTaiSanGetAll",        "thanh_ly_tai_san.get_all" },      // GET  get-all
            { "ThanhLyTaiSanGetById",       "thanh_ly_tai_san.get_by_id" },    // GET  get/{id}
            { "ThanhLyTaiSanGetByAsset",    "thanh_ly_tai_san.get_by_asset" }, // GET  get-by-asset/{taiSanId}
            { "ThanhLyTaiSanCreate",        "thanh_ly_tai_san.create" },       // POST create
            { "ThanhLyTaiSanUpdate",        "thanh_ly_tai_san.update" },       // PUT  update
            { "ThanhLyTaiSanDelete",        "thanh_ly_tai_san.delete" },       // DELETE delete/{id}

            // ============================================
            // BẢO TRÌ TÀI SẢN (api/BaoTriTaiSan)
            // ============================================
            { "BaoTriTaiSanGetAll",     "bao_tri_tai_san.get_all" },       // GET  get-all
            { "BaoTriTaiSanGetById",    "bao_tri_tai_san.get_by_id" },     // GET  get/{id}
            { "BaoTriTaiSanGetByAsset", "bao_tri_tai_san.get_by_asset" },  // GET  get-by-asset/{taiSanId}
            { "BaoTriTaiSanCreate",     "bao_tri_tai_san.create" },        // POST create
            { "BaoTriTaiSanUpdate",     "bao_tri_tai_san.update" },        // PUT  update
            { "BaoTriTaiSanDelete",     "bao_tri_tai_san.delete" },        // DELETE delete/{id}

            // ============================================
            // ĐIỀU CHUYỂN TÀI SẢN (api/DieuChuyenTaiSan)
            // ============================================
            { "DieuChuyenTaiSanGetAll",     "dieu_chuyen_tai_san.get_all" },       // GET  get-all
            { "DieuChuyenTaiSanGetById",    "dieu_chuyen_tai_san.get_by_id" },     // GET  get/{id}
            { "DieuChuyenTaiSanGetByAsset", "dieu_chuyen_tai_san.get_by_asset" },  // GET  get-by-asset/{taiSanId}
            { "DieuChuyenTaiSanCreate",     "dieu_chuyen_tai_san.create" },        // POST create
            { "DieuChuyenTaiSanUpdate",     "dieu_chuyen_tai_san.update" },        // PUT  update
            { "DieuChuyenTaiSanDelete",     "dieu_chuyen_tai_san.delete" },        // DELETE delete/{id}

            // ============================================
            // CẤU HÌNH HỆ THỐNG (api/CauHinhHeThong)
            // ============================================
            { "CauHinhHeThongGet",      "cau_hinh_he_thong.get" },      // GET get
            { "CauHinhHeThongUpdate",   "cau_hinh_he_thong.update" },   // PUT update
        };
    }
}
