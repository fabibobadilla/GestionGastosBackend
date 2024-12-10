using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionGastos.Migrations
{
    /// <inheritdoc />
    public partial class actualizaciongasto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Usuarios_UsuarioId",
                table: "Gastos");

            migrationBuilder.RenameColumn(
                name: "FechaGasto",
                table: "Gastos",
                newName: "FechaCarga");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Gastos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Gastos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Usuarios_UsuarioId",
                table: "Gastos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Usuarios_UsuarioId",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Gastos");

            migrationBuilder.RenameColumn(
                name: "FechaCarga",
                table: "Gastos",
                newName: "FechaGasto");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Gastos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Usuarios_UsuarioId",
                table: "Gastos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
