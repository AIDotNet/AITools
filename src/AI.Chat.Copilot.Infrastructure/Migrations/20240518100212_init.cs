using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AI.Chat.Copilot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AIApps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Description = table.Column<string>(type: "varchar(200)", nullable: true),
                    Prompt = table.Column<string>(type: "text", nullable: true),
                    Temperature = table.Column<int>(type: "integer", nullable: false),
                    MaxTokens = table.Column<int>(type: "integer", nullable: false),
                    AIModelType = table.Column<int>(type: "integer", nullable: false),
                    ModelId = table.Column<string>(type: "varchar(50)", nullable: true),
                    Secret = table.Column<string>(type: "varchar(200)", nullable: true),
                    ProxyHost = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedTime = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIApps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppChatHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatId = table.Column<int>(type: "integer", nullable: false),
                    RoleName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppChatHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppChats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "varchar(50)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedTime = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppChats", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AIApps");

            migrationBuilder.DropTable(
                name: "AppChatHistories");

            migrationBuilder.DropTable(
                name: "AppChats");
        }
    }
}
