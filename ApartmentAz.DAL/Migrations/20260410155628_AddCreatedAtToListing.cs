using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApartmentAz.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtToListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Listings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Listings");
        }
    }
}
