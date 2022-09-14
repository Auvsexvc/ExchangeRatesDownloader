using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangeRatesDownloaderApp.Migrations
{
    public partial class mid_col_nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Mid",
                table: "ExchangeRates",
                type: "decimal(18,9)",
                precision: 18,
                scale: 9,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,9)",
                oldPrecision: 18,
                oldScale: 9);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Mid",
                table: "ExchangeRates",
                type: "decimal(18,9)",
                precision: 18,
                scale: 9,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,9)",
                oldPrecision: 18,
                oldScale: 9,
                oldNullable: true);
        }
    }
}
