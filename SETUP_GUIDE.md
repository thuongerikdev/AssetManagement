# Hướng Dẫn Cài Đặt Hệ Thống Quản Lý Tài Sản Cố Định

## 1. Giới Thiệu

Hệ thống Quản Lý Tài Sản Cố Định là ứng dụng web full-stack giúp quản lý tài sản, khấu hao, điều chuyển, bảo trì và thanh lý tài sản trong doanh nghiệp.

### Công Nghệ Sử Dụng

- **Backend**: .NET 8 (ASP.NET Core Web API)
- **Frontend**: React 18 + TypeScript + Vite
- **Database**: PostgreSQL 17
- **ORM**: Entity Framework Core 8
- **Authentication**: JWT Bearer Tokens

---

## 2. Yêu Cầu Hệ Thống

### 2.1. Backend (.NET)

- **.NET SDK 8.0** trở lên
  - Tải tại: https://dotnet.microsoft.com/download/dotnet/8.0
- **PostgreSQL 17** trở lên
  - Tải tại: https://www.postgresql.org/download/
  - Hoặc sử dụng dịch vụ cloud: Neon, Supabase, Azure Database for PostgreSQL
- **Git** (để clone repository)

### 2.2. Frontend (Node.js)

- **Node.js 18+** hoặc **Node.js 20+**
  - Tải tại: https://nodejs.org/
- **npm** hoặc **yarn** (được cài kèm Node.js)

---

## 3. Cài Đặt Backend

### 3.1. Clone Repository

```bash
cd D:\PersonalProject\GiangProject\TH.WebAPI
```

### 3.2. Cấu Hình Kết Nối Database

Tạo file `.env` trong thư mục `TH.WebAPI\TH.WebAPI` với nội dung sau:

```env
# Connection String cho PostgreSQL
ConnectionStrings__Default=Host=localhost;Port=5432;Database=asset_management;Username=postgres;Password=your_password

# Redis (tùy chọn)
Redis__ConnectionString=localhost:6333

# CORS Origins
Cors__AllowedOrigins[]=http://localhost:3000
```

**Lưu ý**: Thay `your_password` bằng mật khẩu PostgreSQL của bạn.

### 3.3. Cấu Hình appsettings.json

File `TH.WebAPI\TH.WebAPI\appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:3000",
      "https://your-domain.com"
    ]
  }
}
```

### 3.4. Restore Dependencies

```bash
cd TH.WebAPI\TH.WebAPI
dotnet restore
```

### 3.5. Tạo Database và Chạy Migrations

System sử dụng 2 schemas riêng biệt:
- **auth**: Quản lý người dùng, roles, permissions
- **asset**: Quản lý tài sản, phòng ban, khấu hao

#### Bước 1: Tạo database PostgreSQL

```sql
CREATE DATABASE asset_management;
```

#### Bước 2: Chạy EF Migrations

```bash
# Migration cho Auth module
dotnet ef database update --connection "Host=localhost;Port=5432;Database=asset_management;Username=postgres;Password=your_password"

# Hoặc nếu đã cấu hình connection string trong .env
dotnet ef database update
```

**Lưu ý**: Các migration đã được tạo sẵn trong thư mục:
- `TH.WebAPI\Migrations\` (Auth migrations)
- `TH.WebAPI\Migrations\AssetDb\` (Asset migrations)

### 3.6. Seed Initial Data (Optional)

Hệ thống tự động seed dữ liệu ban đầu khi chạy lần đầu:
- Roles mặc định (Admin, User, Manager)
- Permissions
- Users mặc định

### 3.7. Build và Chạy Backend

```bash
dotnet build
dotnet run
```

Backend sẽ chạy tại: **http://localhost:5000** hoặc **https://localhost:5001**

### 3.8. Kiểm Tra API

Access Swagger UI tại: **http://localhost:5000/swagger**

---

## 4. Cài Đặt Frontend

### 4.1. Di Chuyển Vào Thư Mục FE

```bash
cd D:\PersonalProject\GiangProject\FE
```

### 4.2. Cài Đặt Dependencies

```bash
npm install
```

### 4.3. Cấu Hình API Base URL

Tạo file `.env` trong thư mục `FE` (nếu cần):

```env
VITE_API_BASE_URL=http://localhost:5000/api
```

### 4.4. Chạy Development Server

```bash
npm run dev
```

Frontend sẽ chạy tại: **http://localhost:3000**

### 4.5. Build Production

```bash
npm run build
```

Build output sẽ nằm trong thư mục `build/`.

---

## 5. Cấu trúc Database

### 5.1. Schema: auth

| Table | Mô tả |
|-------|-------|
| `authUsers` | Người dùng hệ thống |
| `authProfiles` | Thông tin chi tiết người dùng |
| `authRoles` | Vai trò (Admin, User, Manager) |
| `authUserRoles` | Gán role cho user |
| `authPermissions` | Permissions |
| `authRolePermissions` | Gán permission cho role |
| `authRefreshTokens` | Refresh tokens |
| `authEmailVerifications` | Email verification |
| `authPasswordReset` | Password reset tokens |
| `authMfaSecrets` | MFA secrets |
| `authAuditLogs` | Audit logs |
| `authUserSessions` | User sessions |

### 5.2. Schema: asset

| Table | Mô tả |
|-------|-------|
| `phongBans` | Phòng ban |
| `taiKhoanKeToan` | Tài khoản kế toán |
| `danhMucTaiSan` | Danh mục tài sản |
| `loTaiSan` | Lô tài sản |
| `taiSans` | Tài sản |
| `dieChuyenTaiSans` | Điều chuyển tài sản |
| `baoTriTaiSans` | Bảo trì tài sản |
| `thanhLyTaiSans` | Thanh lý tài sản |
| `lichSuKhauHaos` | Lịch sử khấu hao |
| `chungTus` | Chứng từ kế toán |
| `chiTietChungTus` | Chi tiết chứng từ |
| `taiSanDinhKems` | Tài sản đính kèm |
| `cauHinhHeThong` | Cấu hình hệ thống |

---

## 6. Troubleshooting

### 6.1. Lỗi Kết Nối Database

**Triệu chứng**: `Npgsql.NpgsqlException : Failed to establish a connection`

**Giải pháp**:
1. Kiểm tra PostgreSQL đã chạy
2. Kiểm tra connection string trong `.env`
3. Kiểm tra firewall

### 6.2. Lỗi CORS

**Triệu chứng**: `Access to fetch at '...' from origin '...' has been blocked by CORS policy`

**Giải pháp**: Thêm origin vào `appsettings.json`:
```json
"Cors": {
  "AllowedOrigins": [
    "http://localhost:3000",
    "http://localhost:5173"
  ]
}
```

### 6.3. Lỗi EF Migrations

**Triệu chứng**: `No migrations were found`

**Giải pháp**:
```bash
dotnet ef migrations list
# Nếu vẫn lỗi, kiểm tra project reference
dotnet restore
```

### 6.4. Lỗi Port Already In Use

**Giải pháp**:
```bash
# Windows
netstat -ano | findstr :5000
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:5000 | xargs kill -9
```

---

## 7. Deploy

### 7.1. Environment Variables Cho Production

```env
# Database
ConnectionStrings__Default=Host=<host>;Port=5432;Database=<db>;Username=<user>;Password=<password>

# Redis
Redis__ConnectionString=<redis-host>:6379

# CORS
Cors__AllowedOrigins[]=https://your-production-domain.com
```

### 7.2. Build Docker Image (Optional)

```bash
cd TH.WebAPI
docker build -t asset-management-api .
docker run -p 5000:8080 asset-management-api
```

---

## 8. Tài Liệu Tham Khảo

- **.NET Documentation**: https://docs.microsoft.com/dotnet/
- **Entity Framework Core**: https://learn.microsoft.com/ef/core/
- **React Documentation**: https://react.dev/
- **Vite Documentation**: https://vitejs.dev/
- **PostgreSQL Documentation**: https://www.postgresql.org/docs/

---

## 9. Contact

- **Developer**: Trần Văn Giang
- **Email**: giang@example.com
- **GitHub**: https://github.com/varas900

---

*Last updated: 2026-05-18*