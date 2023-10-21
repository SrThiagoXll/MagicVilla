using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class Akumentar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Esta encima de los balsos", new DateTime(2023, 10, 20, 17, 8, 23, 96, DateTimeKind.Local).AddTicks(666), new DateTime(2023, 10, 20, 17, 8, 23, 96, DateTimeKind.Local).AddTicks(657), "", 50, "Villa Real", 7, 200.0 },
                    { 2, "", "Desde el balcon del Hotel se ve playa", new DateTime(2023, 10, 20, 17, 8, 23, 96, DateTimeKind.Local).AddTicks(671), new DateTime(2023, 10, 20, 17, 8, 23, 96, DateTimeKind.Local).AddTicks(670), "", 100, "Vista a la playa", 30, 120.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
