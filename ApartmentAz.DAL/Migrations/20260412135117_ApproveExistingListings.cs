using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApartmentAz.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ApproveExistingListings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Listings SET IsApproved = 1;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Listings SET IsApproved = 0;");
        }
    }
}
