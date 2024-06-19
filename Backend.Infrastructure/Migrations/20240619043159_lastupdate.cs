using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class lastupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Assignments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Assignments");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$k6ShL4T1hGJztKeVrKSHxuZ4icWn3e6vqYwCperhZyOtIVbUnjkl.");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$d8UMsPhznFNb0QKEQryCa.I/maK3YLbZt.IOw29fPEw68y6ZbhZaC");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$W3ApzFqIdcazID47x/3Qae1WptT1XFTRXt0B.M2VFU5xtN29IEcjG");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Password",
                value: "$2a$11$qUrDj7AgHv.q1lu2NU/I4Oh7..G80tFnQ6P6gxmRhYAhz.vOzjTIy");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "Password",
                value: "$2a$11$67Yr/tVWtA231OHrusmNPunndxcu1tBLC08dc6tNxjKc36dAg3XUi");
        }
    }
}
