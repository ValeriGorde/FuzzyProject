using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuzzyProject.Migrations
{
    /// <inheritdoc />
    public partial class DataBaseNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Accounts_AccountId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Colorants_ColorantId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Materials_MaterialId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Parameters_ParametersId",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "ReferencesParams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reports",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Materials");

            migrationBuilder.RenameTable(
                name: "Reports",
                newName: "Report");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ParametersId",
                table: "Report",
                newName: "IX_Report_ParametersId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_MaterialId",
                table: "Report",
                newName: "IX_Report_MaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ColorantId",
                table: "Report",
                newName: "IX_Report_ColorantId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_AccountId",
                table: "Report",
                newName: "IX_Report_AccountId");

            migrationBuilder.AddColumn<bool>(
                name: "IsReference",
                table: "Materials",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PolymerTypeId",
                table: "Materials",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Report",
                table: "Report",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PolymerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolymerTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materials_PolymerTypeId",
                table: "Materials",
                column: "PolymerTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_PolymerTypes_PolymerTypeId",
                table: "Materials",
                column: "PolymerTypeId",
                principalTable: "PolymerTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Accounts_AccountId",
                table: "Report",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Colorants_ColorantId",
                table: "Report",
                column: "ColorantId",
                principalTable: "Colorants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Materials_MaterialId",
                table: "Report",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Parameters_ParametersId",
                table: "Report",
                column: "ParametersId",
                principalTable: "Parameters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_PolymerTypes_PolymerTypeId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_Accounts_AccountId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_Colorants_ColorantId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_Materials_MaterialId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_Parameters_ParametersId",
                table: "Report");

            migrationBuilder.DropTable(
                name: "PolymerTypes");

            migrationBuilder.DropIndex(
                name: "IX_Materials_PolymerTypeId",
                table: "Materials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Report",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "IsReference",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "PolymerTypeId",
                table: "Materials");

            migrationBuilder.RenameTable(
                name: "Report",
                newName: "Reports");

            migrationBuilder.RenameIndex(
                name: "IX_Report_ParametersId",
                table: "Reports",
                newName: "IX_Reports_ParametersId");

            migrationBuilder.RenameIndex(
                name: "IX_Report_MaterialId",
                table: "Reports",
                newName: "IX_Reports_MaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_Report_ColorantId",
                table: "Reports",
                newName: "IX_Reports_ColorantId");

            migrationBuilder.RenameIndex(
                name: "IX_Report_AccountId",
                table: "Reports",
                newName: "IX_Reports_AccountId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Materials",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reports",
                table: "Reports",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ReferencesParams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ColorantId = table.Column<int>(type: "INTEGER", nullable: false),
                    ParametersId = table.Column<int>(type: "INTEGER", nullable: false),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferencesParams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferencesParams_Colorants_ColorantId",
                        column: x => x.ColorantId,
                        principalTable: "Colorants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReferencesParams_Parameters_ParametersId",
                        column: x => x.ParametersId,
                        principalTable: "Parameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReferencesParams_ColorantId",
                table: "ReferencesParams",
                column: "ColorantId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferencesParams_ParametersId",
                table: "ReferencesParams",
                column: "ParametersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Accounts_AccountId",
                table: "Reports",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Colorants_ColorantId",
                table: "Reports",
                column: "ColorantId",
                principalTable: "Colorants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Materials_MaterialId",
                table: "Reports",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Parameters_ParametersId",
                table: "Reports",
                column: "ParametersId",
                principalTable: "Parameters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
