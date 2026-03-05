using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Comercios.Migrations
{
    /// <inheritdoc />
    public partial class InitialComerciosIdstring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "comercios");

            migrationBuilder.CreateTable(
                name: "Comercios",
                schema: "comercios",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Abierto = table.Column<bool>(type: "boolean", nullable: false),
                    Calificacion = table.Column<decimal>(type: "numeric", nullable: false),
                    Categorias = table.Column<List<string>>(type: "text[]", nullable: true),
                    ImgBannerUrl = table.Column<string>(type: "text", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Usuario_Id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comercios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatosComercio",
                schema: "comercios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Comercio_id = table.Column<string>(type: "character varying(26)", nullable: false),
                    Direccion = table.Column<string>(type: "text", nullable: false),
                    Ciudad = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatosComercio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DatosComercio_Comercios_Comercio_id",
                        column: x => x.Comercio_id,
                        principalSchema: "comercios",
                        principalTable: "Comercios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatosComercio_Comercio_id",
                schema: "comercios",
                table: "DatosComercio",
                column: "Comercio_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatosComercio",
                schema: "comercios");

            migrationBuilder.DropTable(
                name: "Comercios",
                schema: "comercios");
        }
    }
}
