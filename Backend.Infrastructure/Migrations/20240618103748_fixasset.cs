using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixasset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
             name: "Location",
             table: "Assets");
            migrationBuilder.AddColumn<int>(
               name: "Location",
               table: "Assets",
               type: "int",
               nullable: false,
               defaultValue: 0);



            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DateOfBirth", "FirstLogin", "FirstName", "Gender", "IsDeleted", "JoinedDate", "LastName", "Location", "ModifiedAt", "ModifiedBy", "Password", "StaffCode", "Type", "UserName" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(1985, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "John", 1, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Doe", 1, null, null, "$2a$11$k6ShL4T1hGJztKeVrKSHxuZ4icWn3e6vqYwCperhZyOtIVbUnjkl.", "SD0001", 1, "johnd" },
                    { 2, null, null, new DateTime(1990, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Jane", 2, null, new DateTime(2019, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Smith", 0, null, null, "$2a$11$d8UMsPhznFNb0QKEQryCa.I/maK3YLbZt.IOw29fPEw68y6ZbhZaC", "SD0002", 0, "janes" },
                    { 3, null, null, new DateTime(1975, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Michael", 1, null, new DateTime(2018, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brown", 1, null, null, "$2a$11$W3ApzFqIdcazID47x/3Qae1WptT1XFTRXt0B.M2VFU5xtN29IEcjG", "SD0003", 1, "michaelb" },
                    { 4, null, null, new DateTime(1988, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Emily", 2, null, new DateTime(2021, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jones", 1, null, null, "$2a$11$qUrDj7AgHv.q1lu2NU/I4Oh7..G80tFnQ6P6gxmRhYAhz.vOzjTIy", "SD0004", 0, "emilyj" },
                    { 5, null, null, new DateTime(1995, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "David", 1, null, new DateTime(2017, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Williams", 0, null, null, "$2a$11$67Yr/tVWtA231OHrusmNPunndxcu1tBLC08dc6tNxjKc36dAg3XUi", "SD0005", 0, "davidw" }
                });
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

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Assets");

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
        }
    }
}
