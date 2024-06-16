using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$bC/EsramJqHXe59uEYnMzelH1JHWp9kDx4VEtJ6Zq9xac2P21APPO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$U1Q86/BM8md2O9q4/1qltuUz45FkLWd1JY4HKy.RvuAuz1dDRwXp2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$HAkzpIX2dhbQ6dN.VkIG.OCvFyrkTMMpdBS9U5EFObROwnXDYmK5O");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Password",
                value: "$2a$11$tovkUnU5/LkFyds4tWxvLugown6145ysUcIerwrGNRZN0QfWcDc3.");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "Password",
                value: "$2a$11$BuoESZUzlCr45YGkRk1E4.JNUz45g6ZAxPvS1dMIiOZXJQj1FhXfy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$xgCh/RwgM.Hb.i4B1jOmCelf5gA.4EIs3VdwAS7X0eeyrNbBWHf0u");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$//qYOk.Byx9cxN1bTF36.u.as4SI/CkjRvMmsyQvisn6logEjVIBm");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$HtN9/mZDmIWyUb5wKu3cbuRi9QbbZLZ100/miEZ948nwyU7UyWr/S");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Password",
                value: "$2a$11$trA.63QjNLvzbr9/SVN9SOeLhlJUU/0chddEriIAsSlm6TWPmWPjK");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "Password",
                value: "$2a$11$gJ90ppXfyqAVqzvyWSZWiumFgJZv/hHSnfUEpUAwzifEry9FEABVe");
        }
    }
}
