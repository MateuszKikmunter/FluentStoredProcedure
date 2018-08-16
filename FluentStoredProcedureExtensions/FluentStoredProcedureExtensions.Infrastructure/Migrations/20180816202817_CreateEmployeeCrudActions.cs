using FluentStoredProcedureExtensions.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FluentStoredProcedureExtensions.Infrastructure.Migrations
{
    public partial class CreateEmployeeCrudActions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RunFile("CreateEmployeeCrudActions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropStoredProcedureIfExists("GetEmployee");
            migrationBuilder.DropStoredProcedureIfExists("UpdateEmployee");
            migrationBuilder.DropStoredProcedureIfExists("GetAllEmployees");
            migrationBuilder.DropStoredProcedureIfExists("DeleteEmployee");
            migrationBuilder.DropStoredProcedureIfExists("CreateEmployee");
        }
    }
}
