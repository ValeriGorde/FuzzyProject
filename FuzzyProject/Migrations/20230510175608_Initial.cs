using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuzzyProject.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Colorants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colorants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    _L = table.Column<double>(type: "REAL", nullable: false),
                    _A = table.Column<double>(type: "REAL", nullable: false),
                    _B = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: false),
                    ParametersId = table.Column<int>(type: "INTEGER", nullable: false),
                    ColorantId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_Colorants_ColorantId",
                        column: x => x.ColorantId,
                        principalTable: "Colorants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Materials_Parameters_ParametersId",
                        column: x => x.ParametersId,
                        principalTable: "Parameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReferencesParams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: false),
                    ParametersId = table.Column<int>(type: "INTEGER", nullable: false),
                    ColorantId = table.Column<int>(type: "INTEGER", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    ColorantId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaterialId = table.Column<int>(type: "INTEGER", nullable: false),
                    ParametersId = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Colorants_ColorantId",
                        column: x => x.ColorantId,
                        principalTable: "Colorants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Parameters_ParametersId",
                        column: x => x.ParametersId,
                        principalTable: "Parameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ColorantId",
                table: "Materials",
                column: "ColorantId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ParametersId",
                table: "Materials",
                column: "ParametersId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferencesParams_ColorantId",
                table: "ReferencesParams",
                column: "ColorantId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferencesParams_ParametersId",
                table: "ReferencesParams",
                column: "ParametersId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AccountId",
                table: "Reports",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ColorantId",
                table: "Reports",
                column: "ColorantId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_MaterialId",
                table: "Reports",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ParametersId",
                table: "Reports",
                column: "ParametersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferencesParams");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Colorants");

            migrationBuilder.DropTable(
                name: "Parameters");
        }
    }
}
