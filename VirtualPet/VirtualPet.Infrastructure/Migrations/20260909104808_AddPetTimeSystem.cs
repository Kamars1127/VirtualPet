using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualPet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPetTimeSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastStatusUpdateAt",
                table: "Pets",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastStatusUpdateAt",
                table: "Pets");
        }
    }
}
