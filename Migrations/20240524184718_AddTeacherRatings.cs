using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniYarWiki.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeacherRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    KnowledgeRating = table.Column<double>(type: "float", nullable: false),
                    TeachingSkillRating = table.Column<double>(type: "float", nullable: false),
                    CommunicationRating = table.Column<double>(type: "float", nullable: false),
                    EasinessRating = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherRatings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeacherRatings");
        }
    }
}
