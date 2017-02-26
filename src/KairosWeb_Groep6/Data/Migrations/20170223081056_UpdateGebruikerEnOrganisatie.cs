using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KairosWeb_Groep6.Data.Migrations
{
    public partial class UpdateGebruikerEnOrganisatie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobcoach");

            migrationBuilder.CreateTable(
                name: "Organisatie",
                columns: table => new
                {
                    OrganisatieId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Gemeente = table.Column<string>(maxLength: 50, nullable: false),
                    Naam = table.Column<string>(maxLength: 50, nullable: false),
                    Nummer = table.Column<int>(nullable: false),
                    Postcode = table.Column<int>(nullable: false),
                    Straat = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisatie", x => x.OrganisatieId);
                });

            migrationBuilder.CreateTable(
                name: "Gebruiker",
                columns: table => new
                {
                    GebruikerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AlAangemeld = table.Column<bool>(nullable: false),
                    Emailadres = table.Column<string>(maxLength: 100, nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false),
                    Naam = table.Column<string>(maxLength: 50, nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Voornaam = table.Column<string>(maxLength: 50, nullable: false),
                    OrganisatieId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gebruiker", x => x.GebruikerId);
                    table.ForeignKey(
                        name: "FK_Gebruiker_Organisatie_OrganisatieId",
                        column: x => x.OrganisatieId,
                        principalTable: "Organisatie",
                        principalColumn: "OrganisatieId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gebruiker_OrganisatieId",
                table: "Gebruiker",
                column: "OrganisatieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gebruiker");

            migrationBuilder.DropTable(
                name: "Organisatie");

            migrationBuilder.CreateTable(
                name: "Jobcoach",
                columns: table => new
                {
                    JobcoachId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Emailadres = table.Column<string>(maxLength: 50, nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false),
                    Naam = table.Column<string>(maxLength: 50, nullable: false),
                    Voornaam = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobcoach", x => x.JobcoachId);
                });
        }
    }
}
