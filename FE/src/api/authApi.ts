import { apiClient } from './client';

export interface LoginRequest {
  userName: string;
  password: string;
}

export interface UserData {
  userID: number;
  userName: string;
  email: string;
  isEmailVerified: boolean;
  token: string;
  refreshToken: string;
  sessionId: number;
  mfaTicket: string | null;
  requiresMfa: boolean | null;
  deviceId: string;
  permissions: string[];
  roles: string[];
  tokenExpiration: string;
  refreshTokenExpiration: string;
}

export interface LoginResponse {
  errorCode: number;
  errorMessage: string;
  data?: UserData;
}

export interface CreateUserRequest {
  userName: string;
  email: string;
  password: string;
  autoVerifyEmail?: boolean;
  scope?: string;
  roleIds?: number[];
  departmentID?: string;
  firstName?: string;
  lastName?: string;
  gender?: string;
  dateOfBirth?: string;
}

export interface UpdateUserProfileRequest {
  userID: number;
  newUserName: string;
  firstName?: string;
  lastName?: string;
  gender?: string;
  dateOfBirth?: string;
  departmentID?: string;
}

export interface Department {
  id: number;
  maPhongBan: string;
  tenPhongBan: string;
}

export const authApi = {
  login: async (credentials: { userName: string; password: string }) => {
    return await apiClient.post('/Login/userLogin', credentials);
  },

  // --- USERS ---
  getAllUsers: () => apiClient.get('/user/admin/getAllUsers'),
  getUserById: (id: number) => apiClient.get(`/user/admin/getUserById?userId=${id}`),
  deleteUser: (id: number) => apiClient.delete(`/user/deleteUser?userId=${id}`),
  createUser: (data: CreateUserRequest) => apiClient.post('/Register/createUser', data),
  updateUserProfile: (data: UpdateUserProfileRequest) => {
    const formData = new FormData();
    formData.append('userID', String(data.userID));
    formData.append('newUserName', data.newUserName);
    if (data.firstName !== undefined) formData.append('firstName', data.firstName);
    if (data.lastName !== undefined) formData.append('lastName', data.lastName);
    if (data.gender) formData.append('gender', data.gender);
    if (data.dateOfBirth) formData.append('dateOfBirth', data.dateOfBirth);
    if (data.departmentID !== undefined) formData.append('departmentID', data.departmentID);
    return apiClient.putFormData('/user/update/profile', formData);
  },

  getUsersByDepartment: async (departmentId: number) => {
    return await apiClient.get(`/Auth/User/getbyDepartmentID/${departmentId}`);
  },

  // --- ROLES ---
  getAllRoles: () => apiClient.get('/Role/getall'),
  addRole: (data: any) => apiClient.post('/Role/addRole', data),
  updateRole: (data: any) => apiClient.put('/Role/updateRole', data),
  deleteRole: (id: number) => apiClient.delete(`/Role/deleteRole/${id}`),
  cloneRole: (data: any) => apiClient.post('/Role/clonerole', data),
  getRolesByUserId: (userId: number) => apiClient.get(`/Role/getRoleByUserID/${userId}`),

  // --- PERMISSIONS ---
  getAllPermissions: () => apiClient.get('/permission/admin/getall'),
  addPermission: (data: any) => apiClient.post('/permission/BulkCreate', [data]),
  updatePermission: (data: any) => apiClient.put('/permission/updatePermission', data),
  deletePermission: (id: number) => apiClient.delete(`/permission/delete?permissionId=${id}`),
  getPermissionsByRoleId: (roleId: number) => apiClient.get(`/permission/admin/getbyRoleID/${roleId}`),

  // --- ASSIGN ---
  assignRolesToUser: (data: { userId: number; roleIds: number[] }) =>
    apiClient.post('/userrole/admin/assign-roles', data),
  assignPermissionsToRole: (data: { roleId: number; permissionIds: number[] }) =>
    apiClient.post('/rolepermission/admin/assign-permissions', data),


  getMyDepartmentInfo: async () => {
    return await apiClient.get('/PhongBan/my-info');
  },

  // --- DEPARTMENTS ---
  getAllDepartments: () => apiClient.get('/PhongBan/get-all'),
};