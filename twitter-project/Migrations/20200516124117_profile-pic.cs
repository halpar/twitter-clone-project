using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace twitter_project.Migrations
{
    public partial class profilepic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(2020, 5, 16, 15, 41, 17, 705, DateTimeKind.Local).AddTicks(4732),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 5, 15, 17, 53, 35, 622, DateTimeKind.Local).AddTicks(1082));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendTime",
                table: "Tweets",
                nullable: false,
                defaultValue: new DateTime(2020, 5, 16, 15, 41, 17, 698, DateTimeKind.Local).AddTicks(8875),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 5, 15, 17, 53, 35, 615, DateTimeKind.Local).AddTicks(4985));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 5, 15, 17, 53, 35, 622, DateTimeKind.Local).AddTicks(1082),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 5, 16, 15, 41, 17, 705, DateTimeKind.Local).AddTicks(4732));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendTime",
                table: "Tweets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 5, 15, 17, 53, 35, 615, DateTimeKind.Local).AddTicks(4985),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 5, 16, 15, 41, 17, 698, DateTimeKind.Local).AddTicks(8875));
        }
    }
}
