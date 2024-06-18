using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Users_UserId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_UserId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "ReturnedDate",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Assignments",
                newName: "State");

            migrationBuilder.AddColumn<int>(
                name: "AssignedById",
                table: "Assignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AssignedToId",
                table: "Assignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AssignmentId",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$dVTsAWC.At4WhFMDqhNcruXTQTgwasFhZ3QP8OpApqebyKP2wn4mm");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$RhGqdAmgkxbV0.3NK9GyletduMulLhNtAxFtarsx5P1VoQVJJK.vS");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$dESOe1DQW5eZsFnSxlqAP.j.K6.e4h/GVo8ijrpqJibHobQg.0rB2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Password",
                value: "$2a$11$sKMGqj609vAr9hFmOvBMQ.V41RVtHktvagZ..UKaLccmEObQ4gf2i");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "Password",
                value: "$2a$11$N.P6HJsOO749MRgTMlysVOo0cO.IyyB2Wmb00TVpLKniPyGcDCbIC");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignedById",
                table: "Assignments",
                column: "AssignedById");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignedToId",
                table: "Assignments",
                column: "AssignedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Users_AssignedById",
                table: "Assignments",
                column: "AssignedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Users_AssignedToId",
                table: "Assignments",
                column: "AssignedToId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Users_AssignedById",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Users_AssignedToId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_AssignedById",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_AssignedToId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "AssignedById",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Assets");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Assignments",
                newName: "UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnedDate",
                table: "Assignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$t2QiRaKTxLCvg5Cy6ECQZe7siYJ9t1FQXCp1jsLJIJWF9ypGu2Juq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$G6.EE37ajj8UtlyVwbfxuubsVrLMse/7zEGHudzaQdQ2s70EbSOkS");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$8cTx.2QwvmBmcn.E6zGweuWdiacF2Bc9Scus3fJ0dsy4X5TPKiB3O");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Password",
                value: "$2a$11$GPEPU/w9vyJgDnR3w/NxIOWmcXS2dDA2HbixFMWs4FVFxrJtoj29a");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "Password",
                value: "$2a$11$UPMcsLpnT1n0kFOnLbRQkOcGP8hyyKLeoFQXmMDX.J5WjDhcvZ5Di");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_UserId",
                table: "Assignments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Users_UserId",
                table: "Assignments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
