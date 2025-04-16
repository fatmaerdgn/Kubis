using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KubisDeneme.DAL.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kitaplar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    SayfaSayisi = table.Column<int>(type: "integer", nullable: false),
                    İlkYayinYili = table.Column<int>(type: "integer", nullable: false),
                    ISBN = table.Column<string>(type: "text", nullable: false),
                    EklenmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellenmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kitaplar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KitapTurleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    EklenmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellenmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitapTurleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ulkeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    EklenmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellenmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ulkeler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KitapKitapTurleri",
                columns: table => new
                {
                    KitapId = table.Column<int>(type: "integer", nullable: false),
                    KitapTuruId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitapKitapTurleri", x => new { x.KitapId, x.KitapTuruId });
                    table.ForeignKey(
                        name: "FK_KitapKitapTurleri_KitapTurleri_KitapTuruId",
                        column: x => x.KitapTuruId,
                        principalTable: "KitapTurleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KitapKitapTurleri_Kitaplar_KitapId",
                        column: x => x.KitapId,
                        principalTable: "Kitaplar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Yazarlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    ISNI = table.Column<string>(type: "text", nullable: false),
                    DogumTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EklenmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellenmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    UlkeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yazarlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Yazarlar_Ulkeler_UlkeId",
                        column: x => x.UlkeId,
                        principalTable: "Ulkeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KitapYazarlar",
                columns: table => new
                {
                    KitapId = table.Column<int>(type: "integer", nullable: false),
                    YazarId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitapYazarlar", x => new { x.KitapId, x.YazarId });
                    table.ForeignKey(
                        name: "FK_KitapYazarlar_Kitaplar_KitapId",
                        column: x => x.KitapId,
                        principalTable: "Kitaplar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KitapYazarlar_Yazarlar_YazarId",
                        column: x => x.YazarId,
                        principalTable: "Yazarlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KitapKitapTurleri_KitapTuruId",
                table: "KitapKitapTurleri",
                column: "KitapTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_KitapYazarlar_YazarId",
                table: "KitapYazarlar",
                column: "YazarId");

            migrationBuilder.CreateIndex(
                name: "IX_Yazarlar_UlkeId",
                table: "Yazarlar",
                column: "UlkeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KitapKitapTurleri");

            migrationBuilder.DropTable(
                name: "KitapYazarlar");

            migrationBuilder.DropTable(
                name: "KitapTurleri");

            migrationBuilder.DropTable(
                name: "Kitaplar");

            migrationBuilder.DropTable(
                name: "Yazarlar");

            migrationBuilder.DropTable(
                name: "Ulkeler");
        }
    }
}
