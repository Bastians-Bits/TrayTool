using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrayTool.Repository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DirectoryId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: true),
                    Concatenator = table.Column<string>(type: "TEXT", nullable: true),
                    ItemId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Path = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseModels_BaseModels_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "BaseModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseModels_BaseModels_ItemId",
                        column: x => x.ItemId,
                        principalTable: "BaseModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseModels_BaseModels_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BaseModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseModels_DirectoryId",
                table: "BaseModels",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseModels_ItemId",
                table: "BaseModels",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseModels_ParentId",
                table: "BaseModels",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseModels");
        }
    }
}
