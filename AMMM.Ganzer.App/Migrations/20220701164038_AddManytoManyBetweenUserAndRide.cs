using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMM.Ganzer.App.Migrations
{
    public partial class AddManytoManyBetweenUserAndRide : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserRide",
                columns: table => new
                {
                    ApplicationUsersId = table.Column<string>(type: "text", nullable: false),
                    RidesRideID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserRide", x => new { x.ApplicationUsersId, x.RidesRideID });
                    table.ForeignKey(
                        name: "FK_ApplicationUserRide_AspNetUsers_ApplicationUsersId",
                        column: x => x.ApplicationUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserRide_Rides_RidesRideID",
                        column: x => x.RidesRideID,
                        principalTable: "Rides",
                        principalColumn: "RideID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserRide_RidesRideID",
                table: "ApplicationUserRide",
                column: "RidesRideID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserRide");
        }
    }
}
