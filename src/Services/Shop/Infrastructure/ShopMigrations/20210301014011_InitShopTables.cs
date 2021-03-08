using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WangZhen.Techniques.Shop.Api.Infrastructure.ShopMigrations
{
    public partial class InitShopTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shop",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    ImgUrl = table.Column<string>(nullable: false),
                    sales = table.Column<int>(nullable: false),
                    ExpressLimit = table.Column<int>(nullable: false),
                    ExpressPrice = table.Column<int>(nullable: false),
                    Slogan = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shop", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shop");
        }
    }
}
