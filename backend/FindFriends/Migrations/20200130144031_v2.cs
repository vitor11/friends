using Microsoft.EntityFrameworkCore.Migrations;

namespace FindFriends.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FriendInfo",
                table: "FriendInfo");

            migrationBuilder.RenameTable(
                name: "FriendInfo",
                newName: "Friend");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friend",
                table: "Friend",
                columns: new[] { "UserID", "LatitudeB", "LongitudeB" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Friend",
                table: "Friend");

            migrationBuilder.RenameTable(
                name: "Friend",
                newName: "FriendInfo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FriendInfo",
                table: "FriendInfo",
                column: "FriendId");
        }
    }
}
