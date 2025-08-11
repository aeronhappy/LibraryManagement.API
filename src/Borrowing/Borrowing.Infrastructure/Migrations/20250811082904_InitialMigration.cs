using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Borrowing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Borrowing");

            migrationBuilder.CreateTable(
                name: "Books",
                schema: "Borrowing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBorrowed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                schema: "Borrowing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxBooksAllowed = table.Column<int>(type: "int", nullable: false),
                    BorrowedBooksCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BorrowingRecords",
                schema: "Borrowing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BorrowerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateBorrowed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOverdue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateReturned = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowingRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowingRecords_Books_BookId",
                        column: x => x.BookId,
                        principalSchema: "Borrowing",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BorrowingRecords_Members_BorrowerId",
                        column: x => x.BorrowerId,
                        principalSchema: "Borrowing",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BorrowingRequests",
                schema: "Borrowing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BorrowerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowingRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowingRequests_Books_BookId",
                        column: x => x.BookId,
                        principalSchema: "Borrowing",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BorrowingRequests_Members_BorrowerId",
                        column: x => x.BorrowerId,
                        principalSchema: "Borrowing",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingRecords_BookId",
                schema: "Borrowing",
                table: "BorrowingRecords",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingRecords_BorrowerId",
                schema: "Borrowing",
                table: "BorrowingRecords",
                column: "BorrowerId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingRequests_BookId",
                schema: "Borrowing",
                table: "BorrowingRequests",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingRequests_BorrowerId",
                schema: "Borrowing",
                table: "BorrowingRequests",
                column: "BorrowerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorrowingRecords",
                schema: "Borrowing");

            migrationBuilder.DropTable(
                name: "BorrowingRequests",
                schema: "Borrowing");

            migrationBuilder.DropTable(
                name: "Books",
                schema: "Borrowing");

            migrationBuilder.DropTable(
                name: "Members",
                schema: "Borrowing");
        }
    }
}
