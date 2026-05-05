using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TH.WebAPI.Migrations.AssetDb
{
    /// <inheritdoc />
    public partial class AddTaiSanDinhKem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tai_san_dinh_kem",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaiSanId = table.Column<int>(type: "integer", nullable: false),
                    TenFile = table.Column<string>(type: "text", nullable: false),
                    LoaiFile = table.Column<string>(type: "text", nullable: true),
                    DuongDan = table.Column<string>(type: "text", nullable: true),
                    KichThuoc = table.Column<long>(type: "bigint", nullable: true),
                    NgayTai = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MoTa = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tai_san_dinh_kem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tai_san_dinh_kem_tai_san_TaiSanId",
                        column: x => x.TaiSanId,
                        principalSchema: "asset",
                        principalTable: "tai_san",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tai_san_dinh_kem_TaiSanId",
                schema: "asset",
                table: "tai_san_dinh_kem",
                column: "TaiSanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tai_san_dinh_kem",
                schema: "asset");
        }
    }
}
