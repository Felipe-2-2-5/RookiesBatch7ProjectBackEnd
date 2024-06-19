using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class lastUpdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Assignments",
                type: "nvarchar(600)",
                maxLength: 600,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$JJAoazqMKsAQeRyHXAVmvOVWmpCyWHWC/Z8u.HbX35JOHVNagSKBa");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$TFLqU9booOUTVq3PWAhiguPtuqtFtbMFNGw4ZOzXabwzAJ3KLsrCq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$KYBG8L1w5xnaFTmAz4UkseO1q/qGA70ci/uEJVCL71LTP7eUCO4oe");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Password",
                value: "$2a$11$wQy.k8NidS81BzQ5d0jXc.ZEHImVzw71Wsl/dPhwipxBNkTpz2Pzq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "Password",
                value: "$2a$11$qLipztaWwK3bIyubY5R4WuWgZ4OGKTTdiV8Ka1xrNBHRGu5F7K/eW");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
               name: "Assetcode",
               table: "Assets",
               type: "nvarchar(6)",
               maxLength: 6,
               nullable: false,
               oldClrType: typeof(string),
               oldType: "nvarchar(10)",
               oldMaxLength: 10);
            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Assignments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(600)",
                oldMaxLength: 600);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$PMgBGlVs7DU.lamy9TnHiOEAeOkQMjf9VppQG2pl0DduQ.GDQ11iG");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$t3pCujUie5QcErIp.W.o2ubkg4bXrBz0/WLr/oiuFh6tjM.7V4BMi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$wtQABknmm5j2XrF1Q1m0lujanNgnV3xpu1lVLDDUmDUfSdD2GSzX6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Password",
                value: "$2a$11$fmZlG31IHBp0w.20EsgC4.UliWE4HuitwGceusVp4oPD3kgCiUpge");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "Password",
                value: "$2a$11$8nJItDxFG5YdYbEplofuMORKGilnNLdFwWGvR9cHMCYMF69YOjPpG");
        }
    }
}
