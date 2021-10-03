using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrayTool.Repository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    DirectoryId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Path = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseModel_BaseModel_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "BaseModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseModel_BaseModel_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BaseModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Arguments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: true),
                    Concatenator = table.Column<string>(type: "TEXT", nullable: true),
                    ItemId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arguments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arguments_BaseModel_ItemId",
                        column: x => x.ItemId,
                        principalTable: "BaseModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Arguments_ItemId",
                table: "Arguments",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseModel_DirectoryId",
                table: "BaseModel",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseModel_ParentId",
                table: "BaseModel",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Arguments");

            migrationBuilder.DropTable(
                name: "BaseModel");
        }
    }
}
