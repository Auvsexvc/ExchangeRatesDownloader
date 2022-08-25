﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangeRatesDownloaderApp.Migrations
{
    public partial class model_changed_props_names : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.DropTable(
                name: "ExchangeRatesTables");

            migrationBuilder.CreateTable(
                name: "ExchangeTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    No = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TradingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeTables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bid = table.Column<decimal>(type: "decimal(18,9)", precision: 18, scale: 9, nullable: true),
                    Ask = table.Column<decimal>(type: "decimal(18,9)", precision: 18, scale: 9, nullable: true),
                    Mid = table.Column<decimal>(type: "decimal(18,9)", precision: 18, scale: 9, nullable: false),
                    TableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_ExchangeTables_TableId",
                        column: x => x.TableId,
                        principalTable: "ExchangeTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_TableId",
                table: "ExchangeRates",
                column: "TableId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangeRates");

            migrationBuilder.DropTable(
                name: "ExchangeTables");

            migrationBuilder.CreateTable(
                name: "ExchangeRatesTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    No = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TradingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRatesTables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableId = table.Column<int>(type: "int", nullable: false),
                    Ask = table.Column<decimal>(type: "decimal(18,9)", precision: 18, scale: 9, nullable: true),
                    Bid = table.Column<decimal>(type: "decimal(18,9)", precision: 18, scale: 9, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mid = table.Column<decimal>(type: "decimal(18,9)", precision: 18, scale: 9, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rates_ExchangeRatesTables_TableId",
                        column: x => x.TableId,
                        principalTable: "ExchangeRatesTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rates_TableId",
                table: "Rates",
                column: "TableId");
        }
    }
}
