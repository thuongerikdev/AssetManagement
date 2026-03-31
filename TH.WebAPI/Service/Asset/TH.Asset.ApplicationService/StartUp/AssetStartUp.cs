using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Service;
using TH.Asset.Infrastructure.Database;
// Nhớ using các namespace chứa Service và Repository của bạn
// using TH.Asset.ApplicationService.Service;
// using TH.Asset.Infrastructure.Repository;
using TH.Constant.Database; // Nếu bạn dùng chung biến Constant DbSchema

namespace TH.Asset.ApplicationService.StartUp
{
    public static class AssetStartUp
    {
        // Helper: nhận URL (postgres:// / postgresql://) và trả về KV-form Npgsql hiểu được.
        // Nếu đã là KV-form thì chỉ đảm bảo có SSL Mode/Trust Server Certificate.
        private static string NormalizePg(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return raw;
            raw = raw.Trim();

            bool IsUrl(string s) =>
                s.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
                s.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase);

            if (IsUrl(raw))
            {
                var uri = new Uri(raw);

                // user:pass
                string? user = null, pass = null;
                if (!string.IsNullOrEmpty(uri.UserInfo))
                {
                    var parts = uri.UserInfo.Split(':', 2);
                    user = Uri.UnescapeDataString(parts[0]);
                    if (parts.Length == 2) pass = Uri.UnescapeDataString(parts[1]);
                }

                // db name
                var db = Uri.UnescapeDataString(uri.AbsolutePath.Trim('/'));
                var port = uri.IsDefaultPort || uri.Port <= 0 ? 5432 : uri.Port;

                // parse query (?a=b&c=d)
                var qs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var pair in uri.Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
                {
                    var kv = pair.Split('=', 2);
                    var k = Uri.UnescapeDataString(kv[0]);
                    var v = kv.Length == 2 ? Uri.UnescapeDataString(kv[1]) : "";
                    qs[k] = v;
                }

                var sslMode = qs.TryGetValue("sslmode", out var s) ? s : "require";
                var channel = qs.TryGetValue("channel_binding", out var cb) ? cb : null;

                var sb = new StringBuilder();
                sb.Append($"Host={uri.Host};Port={port};Database={db};Username={user};");
                if (!string.IsNullOrEmpty(pass)) sb.Append($"Password={pass};");
                sb.Append($"SSL Mode={sslMode};Trust Server Certificate=true;");
                if (!string.IsNullOrEmpty(channel)) sb.Append($"Channel Binding={channel};");

                return sb.ToString();
            }

            // KV-form: đảm bảo có SSL Mode/TrustServerCertificate
            if (!raw.Contains("SSL Mode", StringComparison.OrdinalIgnoreCase))
                raw += (raw.EndsWith(";") ? "" : ";") + "SSL Mode=Require";
            if (!raw.Contains("Trust Server Certificate", StringComparison.OrdinalIgnoreCase))
                raw += (raw.EndsWith(";") ? "" : ";") + "Trust Server Certificate=true";

            return raw;
        }

        public static void ConfigureAsset(this WebApplicationBuilder builder, string? assemblyName)
        {
            // === DB (PostgreSQL) ===
            builder.Services.AddDbContext<AssetDbContext>(
                options =>
                {
                    // Ưu tiên ConnectionStrings:Default; fallback DATABASE_URL
                    var raw =
                        builder.Configuration.GetConnectionString("Default")
                        ?? Environment.GetEnvironmentVariable("DATABASE_URL")
                        ?? throw new InvalidOperationException("Missing Postgres connection string");

                    // Chuẩn hóa: nếu là URL → chuyển sang KV; nếu KV → đảm bảo SSL Mode/TrustServerCertificate
                    var conn = NormalizePg(raw);

                    options.UseNpgsql(
                        conn,
                        npg =>
                        {
                            if (!string.IsNullOrWhiteSpace(assemblyName))
                                npg.MigrationsAssembly(assemblyName);

                            // Đổi Schema sang Asset cho bảng lịch sử Migration
                            // Giả sử DbSchema.Asset = "asset"
                            npg.MigrationsHistoryTable("__EFMigrationsHistory", "asset");
                            npg.EnableRetryOnFailure();
                        });

                    // Bật tính năng Snake Case Naming (nếu bạn đã cài EFCore.NamingConventions)
                    // options.UseSnakeCaseNamingConvention();
                },
                ServiceLifetime.Scoped
            );

            // === APPLICATION SERVICES DI ===

            // 1. Các danh mục cơ bản
            builder.Services.AddScoped<IPhongBanService, PhongBanService>();
            builder.Services.AddScoped<ITaiKhoanKeToanService, TaiKhoanKeToanService>();
            builder.Services.AddScoped<IDanhMucTaiSanService, DanhMucTaiSanService>();
            builder.Services.AddScoped<ICauHinhHeThongService, CauHinhHeThongService>();
            builder.Services.AddScoped<ILoTaiSanService, LoTaiSanService>();

            // 2. Trùm cuối Tài sản
            builder.Services.AddScoped<ITaiSanService, TaiSanService>();

            // 3. Các nghiệp vụ vòng đời tài sản
            builder.Services.AddScoped<IDieuChuyenTaiSanService, DieuChuyenTaiSanService>();
            builder.Services.AddScoped<IBaoTriTaiSanService, BaoTriTaiSanService>();
            builder.Services.AddScoped<IThanhLyTaiSanService, ThanhLyTaiSanService>();

            // 4. Các nghiệp vụ Kế toán / Sổ sách
            builder.Services.AddScoped<ILichSuKhauHaoService, LichSuKhauHaoService>();
            builder.Services.AddScoped<IChungTuService, ChungTuService>();
        }

        public static async Task SeedAssetDataAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("AssetSeeder");

                try
                {
                    var context = services.GetRequiredService<AssetDbContext>();

                    logger.LogInformation("Applying migrations for Asset module...");
                    await context.Database.MigrateAsync();

                    // Chạy Seeder cho các dữ liệu mặc định của module Tài sản (nếu cần)
                    // logger.LogInformation("Starting Asset Seeding...");
                    // await AssetDataSeeder.SeedDefaultConfigAsync(context);
                    // await AssetDataSeeder.SeedDefaultCategoriesAsync(context);
                    // logger.LogInformation("Asset Seeding completed successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating/seeding the Asset database.");
                }
            }
        }
    }
}