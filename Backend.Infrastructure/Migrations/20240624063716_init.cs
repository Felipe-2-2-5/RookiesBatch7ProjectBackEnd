using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prefix = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<int>(type: "int", maxLength: 30, nullable: false),
                    FirstLogin = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AssetName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Specification = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true),
                    InstalledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignedToId = table.Column<int>(type: "int", nullable: false),
                    AssignedById = table.Column<int>(type: "int", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Users_AssignedById",
                        column: x => x.AssignedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Assignments_Users_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DateOfBirth", "FirstLogin", "FirstName", "Gender", "IsDeleted", "JoinedDate", "LastName", "Location", "ModifiedAt", "ModifiedBy", "Password", "StaffCode", "Type", "UserName" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(1985, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "John", 1, false, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Doe", 1, null, null, "$2a$11$FQgJ5INYTnlqFsBi6JokXu87XRoI7hX0ChTxpkBuerLq3fU5r67Iy", "SD0001", 1, "johnd" },
                    { 2, null, null, new DateTime(1990, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Jane", 2, false, new DateTime(2019, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Smith", 0, null, null, "$2a$11$Ha1sEO9BGt2PlTS47zQdUud9PnPyrRm.P.tI4pmgWWrGgx7bwjHBC", "SD0002", 0, "janes" },
                    { 3, null, null, new DateTime(1975, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Michael", 1, false, new DateTime(2018, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brown", 1, null, null, "$2a$11$HVygcNRgjKnElS2GyliX3uydegQHwTZEDz9ezUzwhffS1Dzw9AC8G", "SD0003", 1, "michaelb" },
                    { 4, null, null, new DateTime(1988, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Emily", 2, false, new DateTime(2021, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jones", 1, null, null, "$2a$11$3Vo3iO46gG549An4TtHRhOmIcz4BKr3fl5V.zTjzReZPZTSjMRrWu", "SD0004", 0, "emilyj" },
                    { 5, null, null, new DateTime(1995, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "David", 1, false, new DateTime(2017, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Williams", 0, null, null, "$2a$11$QwsGuvhd7PrSSX6f37cv1ODg9s.9MLVP4yJJ/NCePGRCeYZ.5XUSi", "SD0005", 0, "davidw" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_CategoryId",
                table: "Assets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssetId",
                table: "Assignments",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignedById",
                table: "Assignments",
                column: "AssignedById");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignedToId",
                table: "Assignments",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_StaffCode",
                table: "Users",
                column: "StaffCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
