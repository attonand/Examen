using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class Modificarconstraintparaagregareliminacionencascadaenvehiclebrand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleBrand_Brands_BrandId",
                table: "VehicleBrand");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleBrand_Vehicles_VehicleId",
                table: "VehicleBrand");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleBrand_Brands_BrandId",
                table: "VehicleBrand",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleBrand_Vehicles_VehicleId",
                table: "VehicleBrand",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleBrand_Brands_BrandId",
                table: "VehicleBrand");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleBrand_Vehicles_VehicleId",
                table: "VehicleBrand");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleBrand_Brands_BrandId",
                table: "VehicleBrand",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleBrand_Vehicles_VehicleId",
                table: "VehicleBrand",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id");
        }
    }
}
