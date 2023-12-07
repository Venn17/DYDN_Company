using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DYDN_Company.Migrations
{
    public partial class venn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblAccount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblLogined",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblLogined", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblCart",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblCart_tblAccount_AccountID",
                        column: x => x.AccountID,
                        principalTable: "tblAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Price = table.Column<float>(nullable: false),
                    SalePrice = table.Column<float>(nullable: false),
                    Sold = table.Column<float>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    CartId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblProduct_tblCart_CartId",
                        column: x => x.CartId,
                        principalTable: "tblCart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblProduct_tblCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "tblCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblAccount_Email",
                table: "tblAccount",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tblCart_AccountID",
                table: "tblCart",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_tblProduct_CartId",
                table: "tblProduct",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_tblProduct_CategoryId",
                table: "tblProduct",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_tblProduct_Name",
                table: "tblProduct",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblLogined");

            migrationBuilder.DropTable(
                name: "tblProduct");

            migrationBuilder.DropTable(
                name: "tblCart");

            migrationBuilder.DropTable(
                name: "tblCategory");

            migrationBuilder.DropTable(
                name: "tblAccount");
        }
    }
}
