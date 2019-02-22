using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DNTFrameworkCoreTemplateAPI.Infrastructure.Migrations
{
    public partial class CreateInitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "AuditLog",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<long>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    ServiceName = table.Column<string>(maxLength: 256, nullable: false),
                    MethodName = table.Column<string>(maxLength: 256, nullable: false),
                    Parameters = table.Column<string>(nullable: true),
                    ReturnValue = table.Column<string>(nullable: true),
                    ExecutionDateTime = table.Column<DateTimeOffset>(nullable: false),
                    ExecutionDuration = table.Column<int>(nullable: false),
                    UserIp = table.Column<string>(maxLength: 20, nullable: true),
                    UserBrowserName = table.Column<string>(maxLength: 1024, nullable: true),
                    Exception = table.Column<string>(nullable: true),
                    ImpersonatorUserId = table.Column<long>(nullable: true),
                    ImpersonatorTenantId = table.Column<long>(nullable: true),
                    ExtensionJson = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cache",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 449, nullable: false),
                    Value = table.Column<byte[]>(nullable: false),
                    ExpiresAtTime = table.Column<DateTimeOffset>(nullable: false),
                    SlidingExpirationInSeconds = table.Column<long>(nullable: true),
                    AbsoluteExpiration = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cache", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataProtectionKey",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FriendlyName = table.Column<string>(nullable: false),
                    XmlValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationDateTime = table.Column<DateTimeOffset>(nullable: false),
                    CreatorBrowserName = table.Column<string>(maxLength: 1024, nullable: true),
                    CreatorIp = table.Column<string>(maxLength: 256, nullable: true),
                    CreatorUserId = table.Column<long>(nullable: true),
                    Message = table.Column<string>(nullable: false),
                    Level = table.Column<string>(maxLength: 50, nullable: false),
                    Logger = table.Column<string>(maxLength: 256, nullable: false),
                    EventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cache_ExpiresAtTime",
                schema: "dbo",
                table: "Cache",
                column: "ExpiresAtTime");

            migrationBuilder.CreateIndex(
                name: "IX_DataProtectionKey_FriendlyName",
                schema: "dbo",
                table: "DataProtectionKey",
                column: "FriendlyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Log_Level",
                schema: "dbo",
                table: "Log",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_Log_Logger",
                schema: "dbo",
                table: "Log",
                column: "Logger");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Cache",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DataProtectionKey",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Log",
                schema: "dbo");
        }
    }
}
