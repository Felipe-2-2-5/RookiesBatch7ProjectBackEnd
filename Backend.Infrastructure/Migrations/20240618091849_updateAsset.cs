using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
               name: "Location",
               table: "Assets",
               type: "int",
               nullable: false,
               defaultValue: 0);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<string>(
                name: "AssetName",
                table: "Assets",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
             name: "Location",
             table: "Assets");

            migrationBuilder.AlterColumn<string>(
                name: "AssetName",
                table: "Assets",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DateOfBirth", "FirstLogin", "FirstName", "Gender", "IsDeleted", "JoinedDate", "LastName", "Location", "ModifiedAt", "ModifiedBy", "Password", "StaffCode", "Type", "UserName" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(1985, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "John", 1, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Doe", 1, null, null, "$2a$11$gpoEMRfihL7mvAc2b5xvtuZVu0f1y4KWJXWcBje1YGVxBRo4G.Ts6", "SD0001", 1, "johnd" },
                    { 2, null, null, new DateTime(1990, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Jane", 2, null, new DateTime(2019, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Smith", 0, null, null, "$2a$11$qT2B7IGRCCL2EZrqg93g3emxvKkKIeLgDgfB0FBHbzNCba2hWO8Cy", "SD0002", 0, "janes" },
                    { 3, null, null, new DateTime(1975, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Michael", 1, null, new DateTime(2018, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brown", 1, null, null, "$2a$11$pE6V6A8okMYbYMRFELpzyeH3TdAbFvt/3WZq6DDTa/YG4Kb65Ekky", "SD0003", 1, "michaelb" },
                    { 4, null, null, new DateTime(1988, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Emily", 2, null, new DateTime(2021, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jones", 1, null, null, "$2a$11$qqcFmrBMH1e66q2ym1L0z.Vp9EePfNMmresXyMukUP6j/5N0WUDu6", "SD0004", 0, "emilyj" },
                    { 5, null, null, new DateTime(1995, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "David", 1, null, new DateTime(2017, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Williams", 0, null, null, "$2a$11$RxIWeZedXvlr1dkFFATEGuZ0UxvQmOjeMZisx2cW.kx1u3yWUVWm2", "SD0005", 0, "davidw" }
                });
        }
    }
}
