using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bank",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    api_settings = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bank", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "currency",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currency", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rate",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    value = table.Column<decimal>(type: "numeric", nullable: false),
                    base_currency_id = table.Column<int>(type: "integer", nullable: false),
                    currency_id = table.Column<int>(type: "integer", nullable: false),
                    bank_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rate", x => x.id);
                    table.ForeignKey(
                        name: "fk_rate_bank_bank_id",
                        column: x => x.bank_id,
                        principalTable: "bank",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_rate_currency_base_currency_id",
                        column: x => x.base_currency_id,
                        principalTable: "currency",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_rate_currency_currency_id",
                        column: x => x.currency_id,
                        principalTable: "currency",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "bank",
                columns: new[] { "id", "api_settings", "description", "name" },
                values: new object[] { 1, "{\"LatestRatesUrl\":\"http://api.exchangeratesapi.io/v1/latest?access_key=413d96c4d38020d4cbf67e45d5cca487\"}", "Bank A...", "Bank A" });

            migrationBuilder.InsertData(
                table: "currency",
                columns: new[] { "id", "code", "description", "name" },
                values: new object[,]
                {
                    { 1, "USD", "Unites States Dollar", "Unites States Dollar" },
                    { 2, "EUR", "Euro", "Euro" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_bank_name",
                table: "bank",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_currency_code",
                table: "currency",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_currency_name",
                table: "currency",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_rate_bank_id",
                table: "rate",
                column: "bank_id");

            migrationBuilder.CreateIndex(
                name: "ix_rate_base_currency_id",
                table: "rate",
                column: "base_currency_id");

            migrationBuilder.CreateIndex(
                name: "ix_rate_currency_id",
                table: "rate",
                column: "currency_id");

            migrationBuilder.CreateIndex(
                name: "ix_rate_date_time_currency_id_base_currency_id_bank_id",
                table: "rate",
                columns: new[] { "date_time", "currency_id", "base_currency_id", "bank_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rate");

            migrationBuilder.DropTable(
                name: "bank");

            migrationBuilder.DropTable(
                name: "currency");
        }
    }
}
