using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AssetCode",
                table: "Assets",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$gpoEMRfihL7mvAc2b5xvtuZVu0f1y4KWJXWcBje1YGVxBRo4G.Ts6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$qT2B7IGRCCL2EZrqg93g3emxvKkKIeLgDgfB0FBHbzNCba2hWO8Cy");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$pE6V6A8okMYbYMRFELpzyeH3TdAbFvt/3WZq6DDTa/YG4Kb65Ekky");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Password",
                value: "$2a$11$qqcFmrBMH1e66q2ym1L0z.Vp9EePfNMmresXyMukUP6j/5N0WUDu6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "Password",
                value: "$2a$11$RxIWeZedXvlr1dkFFATEGuZ0UxvQmOjeMZisx2cW.kx1u3yWUVWm2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AssetCode",
                table: "Assets",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$kYf.O1LpLDEHMphxJjhrQOybtftRS8FQDM130enGYeGs2pAp/i0nK");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$2nTb/E4nr6RwPJsMqmpkJ.yW4ev4ipw1IBiJAqXkPliTUzJTOzh.2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$vQLi4lG5LiHTw.Htq7L/l.KfAdpKVCzdISar..4fsKriGChI9gIGi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Password",
                value: "$2a$11$FSQV9ri7rWPWY2jThd.1kergu.4TpL1qA4EryM3n3OCfUD3X4dkEq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "Password",
                value: "$2a$11$s.fKFctYwNC7Tfhw1iy3E.IzHkQJcgEDhEMA2rL5plss7VnfKU4vu");
        }
    }
}
