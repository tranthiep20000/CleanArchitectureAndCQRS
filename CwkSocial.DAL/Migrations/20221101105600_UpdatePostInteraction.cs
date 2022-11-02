using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CwkSocial.DAL.Migrations
{
    public partial class UpdatePostInteraction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "PostInteraction",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "PostInteraction",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileId",
                table: "PostInteraction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PostInteraction");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "PostInteraction");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "PostInteraction");
        }
    }
}
