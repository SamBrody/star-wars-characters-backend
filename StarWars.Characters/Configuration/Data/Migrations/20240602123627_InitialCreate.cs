using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StarWars.Characters.Configuration.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "movies",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "planets",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_planets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "species",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_species", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "characters",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    home_world_id = table.Column<int>(type: "INTEGER", nullable: false),
                    gender = table.Column<int>(type: "INTEGER", nullable: false),
                    species_id = table.Column<int>(type: "INTEGER", nullable: false),
                    height = table.Column<int>(type: "INTEGER", nullable: false),
                    hair_color = table.Column<string>(type: "TEXT", nullable: false),
                    eye_color = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    birth_day_era = table.Column<int>(type: "INTEGER", nullable: false),
                    birth_day_year = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_characters", x => x.id);
                    table.ForeignKey(
                        name: "fk_characters_planets_home_world_id",
                        column: x => x.home_world_id,
                        principalTable: "planets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_characters_species_species_id",
                        column: x => x.species_id,
                        principalTable: "species",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "characters_movies",
                columns: table => new
                {
                    characters_id = table.Column<int>(type: "INTEGER", nullable: false),
                    movies_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_characters_movies", x => new { x.characters_id, x.movies_id });
                    table.ForeignKey(
                        name: "fk_characters_movies_characters_characters_id",
                        column: x => x.characters_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_characters_movies_movies_movies_id",
                        column: x => x.movies_id,
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "movies",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Звездные войны: Эпизод 4 — Новая надежда" },
                    { 2, "Звездные войны: Эпизод 5 — Империя наносит ответный удар" },
                    { 3, "Звездные войны: Эпизод 6 — Возвращение джедая" },
                    { 4, "Звездные войны: Эпизод 1 — Скрытая угроза" },
                    { 5, "Звездные войны: Эпизод 2 — Атака клонов" },
                    { 6, "Звездные войны: Эпизод 3 — Месть ситхов" },
                    { 7, "Звездные войны: Пробуждение силы" },
                    { 8, "Изгой-один: Звездные войны. Истории" },
                    { 9, "Звездные войны: Последние джедаи" },
                    { 10, "Хан Соло: Звездные войны. Истории" }
                });

            migrationBuilder.InsertData(
                table: "planets",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Корусант" },
                    { 2, "Хосниан-Прайм" },
                    { 3, "Альдераан" },
                    { 4, "Набу" },
                    { 5, "Татуин" },
                    { 6, "Коррибан" },
                    { 7, "Датомир" },
                    { 8, "Камино" },
                    { 9, "Мандалор" },
                    { 10, "Кашиик" }
                });

            migrationBuilder.InsertData(
                table: "species",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Раса Йода" },
                    { 2, "Вуки" },
                    { 3, "Человек" },
                    { 4, "Кали" },
                    { 5, "Хатт" },
                    { 6, "Тви’лек" },
                    { 7, "Дурос" },
                    { 8, "Юужань-вонги" },
                    { 9, "Талз" },
                    { 10, "Куаррен" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_characters_home_world_id",
                table: "characters",
                column: "home_world_id");

            migrationBuilder.CreateIndex(
                name: "ix_characters_species_id",
                table: "characters",
                column: "species_id");

            migrationBuilder.CreateIndex(
                name: "ix_characters_movies_movies_id",
                table: "characters_movies",
                column: "movies_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "characters_movies");

            migrationBuilder.DropTable(
                name: "characters");

            migrationBuilder.DropTable(
                name: "movies");

            migrationBuilder.DropTable(
                name: "planets");

            migrationBuilder.DropTable(
                name: "species");
        }
    }
}
