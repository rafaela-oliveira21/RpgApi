using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RpgApi.Migrations
{
    /// <inheritdoc />
    public partial class AjusteNarracaoMax : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Tx_Narracao",
                table: "TB_DISPUTAS",
                type: "varchar(max)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200);

            migrationBuilder.UpdateData(
                table: "TB_USUARIOS",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 94, 117, 66, 227, 163, 33, 32, 202, 96, 47, 47, 2, 45, 50, 190, 7, 82, 91, 118, 204, 19, 119, 76, 101, 154, 13, 158, 102, 164, 176, 106, 71, 93, 8, 135, 144, 59, 209, 184, 133, 185, 211, 32, 82, 154, 223, 170, 229, 122, 223, 54, 96, 135, 117, 219, 177, 187, 203, 109, 102, 8, 145, 32, 129 }, new byte[] { 72, 77, 208, 179, 226, 240, 177, 203, 129, 193, 127, 124, 155, 163, 170, 102, 221, 12, 101, 28, 145, 62, 189, 241, 68, 240, 197, 245, 47, 188, 249, 159, 149, 26, 6, 153, 46, 195, 6, 152, 50, 100, 7, 212, 128, 183, 3, 14, 23, 30, 128, 95, 195, 176, 213, 144, 197, 46, 96, 191, 91, 140, 177, 184, 72, 81, 85, 233, 90, 161, 24, 220, 149, 68, 220, 112, 161, 6, 135, 95, 156, 52, 25, 193, 169, 124, 207, 148, 142, 219, 80, 129, 97, 63, 39, 204, 65, 17, 29, 53, 218, 225, 180, 98, 14, 97, 151, 75, 249, 47, 23, 237, 193, 166, 21, 131, 246, 211, 212, 88, 101, 224, 126, 200, 94, 155, 174, 160 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Tx_Narracao",
                table: "TB_DISPUTAS",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldMaxLength: 200);

            migrationBuilder.UpdateData(
                table: "TB_USUARIOS",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 188, 228, 210, 93, 197, 7, 122, 242, 184, 219, 184, 186, 125, 75, 100, 13, 249, 29, 70, 142, 245, 197, 176, 192, 77, 11, 177, 6, 147, 68, 208, 190, 97, 165, 223, 104, 187, 171, 184, 4, 246, 90, 17, 61, 108, 167, 174, 104, 196, 10, 112, 201, 154, 238, 174, 26, 214, 148, 20, 202, 106, 216, 93, 39 }, new byte[] { 142, 109, 119, 167, 37, 201, 201, 100, 62, 61, 113, 130, 163, 143, 128, 133, 79, 159, 132, 94, 217, 115, 85, 134, 26, 2, 148, 57, 98, 36, 215, 180, 245, 18, 95, 37, 81, 201, 248, 6, 124, 251, 213, 37, 112, 194, 160, 163, 68, 11, 190, 160, 137, 35, 119, 149, 92, 117, 55, 213, 219, 209, 244, 95, 170, 164, 161, 138, 107, 8, 89, 158, 9, 165, 60, 136, 119, 11, 98, 187, 179, 226, 89, 92, 165, 68, 59, 195, 149, 148, 66, 43, 248, 166, 122, 138, 7, 9, 167, 215, 155, 96, 237, 25, 107, 78, 197, 188, 132, 155, 26, 244, 71, 2, 63, 234, 185, 165, 104, 2, 141, 63, 46, 127, 144, 82, 169, 125 } });
        }
    }
}
