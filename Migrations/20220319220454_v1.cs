using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebProjekat.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Firme",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImeKompanije = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BrSedista = table.Column<int>(type: "int", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Firme", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Putnik",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JMBG = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    BrTelefona = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Putnik", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Stanice",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImeStanice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImeGrada = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stanice", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Destinacije",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Drzava = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Grad = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IDFirmeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinacije", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Destinacije_Firme_IDFirmeID",
                        column: x => x.IDFirmeID,
                        principalTable: "Firme",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rute",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipSedista = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cena = table.Column<int>(type: "int", nullable: false),
                    PutnikID = table.Column<int>(type: "int", nullable: true),
                    DestinacijaID = table.Column<int>(type: "int", nullable: true),
                    StanicaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rute", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rute_Destinacije_DestinacijaID",
                        column: x => x.DestinacijaID,
                        principalTable: "Destinacije",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rute_Putnik_PutnikID",
                        column: x => x.PutnikID,
                        principalTable: "Putnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rute_Stanice_StanicaID",
                        column: x => x.StanicaID,
                        principalTable: "Stanice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Destinacije_IDFirmeID",
                table: "Destinacije",
                column: "IDFirmeID");

            migrationBuilder.CreateIndex(
                name: "IX_Rute_DestinacijaID",
                table: "Rute",
                column: "DestinacijaID");

            migrationBuilder.CreateIndex(
                name: "IX_Rute_PutnikID",
                table: "Rute",
                column: "PutnikID");

            migrationBuilder.CreateIndex(
                name: "IX_Rute_StanicaID",
                table: "Rute",
                column: "StanicaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rute");

            migrationBuilder.DropTable(
                name: "Destinacije");

            migrationBuilder.DropTable(
                name: "Putnik");

            migrationBuilder.DropTable(
                name: "Stanice");

            migrationBuilder.DropTable(
                name: "Firme");
        }
    }
}
