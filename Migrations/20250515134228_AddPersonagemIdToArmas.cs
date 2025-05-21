using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RpgApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonagemIdToArmas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_DISPUTAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dt_Disputa = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AtacanteId = table.Column<int>(type: "int", nullable: false),
                    OponenteId = table.Column<int>(type: "int", nullable: false),
                    Tx_Narracao = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_DISPUTAS", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "TB_USUARIOS",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 188, 228, 210, 93, 197, 7, 122, 242, 184, 219, 184, 186, 125, 75, 100, 13, 249, 29, 70, 142, 245, 197, 176, 192, 77, 11, 177, 6, 147, 68, 208, 190, 97, 165, 223, 104, 187, 171, 184, 4, 246, 90, 17, 61, 108, 167, 174, 104, 196, 10, 112, 201, 154, 238, 174, 26, 214, 148, 20, 202, 106, 216, 93, 39 }, new byte[] { 142, 109, 119, 167, 37, 201, 201, 100, 62, 61, 113, 130, 163, 143, 128, 133, 79, 159, 132, 94, 217, 115, 85, 134, 26, 2, 148, 57, 98, 36, 215, 180, 245, 18, 95, 37, 81, 201, 248, 6, 124, 251, 213, 37, 112, 194, 160, 163, 68, 11, 190, 160, 137, 35, 119, 149, 92, 117, 55, 213, 219, 209, 244, 95, 170, 164, 161, 138, 107, 8, 89, 158, 9, 165, 60, 136, 119, 11, 98, 187, 179, 226, 89, 92, 165, 68, 59, 195, 149, 148, 66, 43, 248, 166, 122, 138, 7, 9, 167, 215, 155, 96, 237, 25, 107, 78, 197, 188, 132, 155, 26, 244, 71, 2, 63, 234, 185, 165, 104, 2, 141, 63, 46, 127, 144, 82, 169, 125 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_DISPUTAS");

            migrationBuilder.UpdateData(
                table: "TB_USUARIOS",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 14, 40, 106, 194, 211, 55, 39, 61, 85, 209, 77, 71, 97, 8, 159, 52, 138, 175, 254, 59, 142, 152, 27, 133, 183, 136, 229, 47, 106, 134, 174, 60, 254, 92, 72, 48, 184, 142, 33, 104, 128, 190, 80, 194, 102, 174, 54, 101, 246, 242, 254, 128, 83, 23, 90, 205, 131, 197, 198, 225, 229, 49, 71, 153 }, new byte[] { 111, 64, 83, 118, 96, 149, 107, 182, 176, 62, 251, 16, 96, 58, 185, 223, 73, 232, 59, 190, 45, 21, 127, 142, 178, 129, 23, 201, 63, 50, 25, 41, 20, 125, 54, 217, 66, 114, 189, 249, 30, 203, 124, 23, 37, 65, 99, 17, 34, 158, 175, 241, 144, 104, 254, 140, 169, 170, 170, 63, 125, 175, 108, 54, 69, 64, 208, 66, 72, 212, 71, 233, 228, 169, 77, 241, 83, 28, 21, 95, 151, 188, 59, 154, 182, 34, 105, 36, 24, 92, 26, 17, 234, 198, 134, 248, 37, 59, 189, 83, 186, 106, 186, 110, 33, 160, 110, 118, 81, 106, 2, 128, 35, 62, 159, 153, 247, 15, 58, 243, 101, 18, 214, 243, 74, 236, 12, 49 } });
        }
    }
}
