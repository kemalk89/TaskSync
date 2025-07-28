using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Visibility = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: false),
                    ExternalUserId = table.Column<string>(type: "text", nullable: true),
                    Picture = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: true),
                    AssigneeId = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "TicketStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectMemberEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Role = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMemberEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMemberEntity_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMemberEntity_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TicketComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketComments_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "ModifiedDate", "Title", "Visibility" },
                values: new object[,]
                {
                    { 1, 1, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the first project. This project has one member as well.", null, "My First Project", null },
                    { 2, 2, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the 2nd project. This project has two members.", null, "My 2nd Project", null },
                    { 3, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the 3rd project.", null, "My 3rd Project", null },
                    { 4, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the 4rd project.", null, "My 4rd Project", null }
                });

            migrationBuilder.InsertData(
                table: "TicketStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Todo" },
                    { 2, "In Progress" },
                    { 3, "Done" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Email", "ExternalUserId", "ModifiedDate", "Picture", "Username" },
                values: new object[,]
                {
                    { 1, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "empty", null, null, "", "Kerem Karacay" },
                    { 2, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "empty", null, null, "", "Deniz Aslansu" }
                });

            migrationBuilder.InsertData(
                table: "ProjectMemberEntity",
                columns: new[] { "Id", "ProjectId", "Role", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "Software Developer", 1 },
                    { 2, 2, "ProjectManager", 1 },
                    { 3, 2, "UI / UX", 2 }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "AssigneeId", "CreatedBy", "CreatedDate", "Description", "ModifiedDate", "ProjectId", "StatusId", "Title", "Type" },
                values: new object[,]
                {
                    { 365, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #365.", null, 1, 3, "Demo Ticket of type Bug #365", 0 },
                    { 366, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #366.", null, 1, 1, "Demo Ticket of type Bug #366", 0 },
                    { 367, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #367.", null, 1, 3, "Demo Ticket of type Story #367", 2 },
                    { 368, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #368.", null, 1, 2, "Demo Ticket of type Story #368", 2 },
                    { 369, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #369.", null, 1, 2, "Demo Ticket of type Task #369", 1 },
                    { 370, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #370.", null, 1, 2, "Demo Ticket of type Bug #370", 0 },
                    { 371, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #371.", null, 1, 3, "Demo Ticket of type Task #371", 1 },
                    { 372, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #372.", null, 1, 1, "Demo Ticket of type Story #372", 2 },
                    { 373, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #373.", null, 1, 3, "Demo Ticket of type Bug #373", 0 },
                    { 374, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #374.", null, 1, 2, "Demo Ticket of type Bug #374", 0 },
                    { 375, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #375.", null, 1, 1, "Demo Ticket of type Task #375", 1 },
                    { 376, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #376.", null, 1, 3, "Demo Ticket of type Story #376", 2 },
                    { 377, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #377.", null, 1, 2, "Demo Ticket of type Bug #377", 0 },
                    { 378, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #378.", null, 1, 2, "Demo Ticket of type Task #378", 1 },
                    { 379, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #379.", null, 1, 3, "Demo Ticket of type Task #379", 1 },
                    { 380, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #380.", null, 1, 1, "Demo Ticket of type Task #380", 1 },
                    { 381, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #381.", null, 1, 1, "Demo Ticket of type Bug #381", 0 },
                    { 382, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #382.", null, 1, 2, "Demo Ticket of type Task #382", 1 },
                    { 383, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #383.", null, 1, 2, "Demo Ticket of type Bug #383", 0 },
                    { 384, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #384.", null, 1, 1, "Demo Ticket of type Story #384", 2 },
                    { 385, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #385.", null, 1, 3, "Demo Ticket of type Task #385", 1 },
                    { 386, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #386.", null, 1, 2, "Demo Ticket of type Bug #386", 0 },
                    { 387, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #387.", null, 1, 3, "Demo Ticket of type Task #387", 1 },
                    { 388, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #388.", null, 1, 3, "Demo Ticket of type Task #388", 1 },
                    { 389, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #389.", null, 1, 2, "Demo Ticket of type Story #389", 2 },
                    { 390, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #390.", null, 1, 1, "Demo Ticket of type Story #390", 2 },
                    { 391, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #391.", null, 1, 3, "Demo Ticket of type Story #391", 2 },
                    { 392, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #392.", null, 1, 1, "Demo Ticket of type Story #392", 2 },
                    { 393, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #393.", null, 1, 2, "Demo Ticket of type Task #393", 1 },
                    { 394, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #394.", null, 1, 3, "Demo Ticket of type Bug #394", 0 },
                    { 395, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #395.", null, 1, 1, "Demo Ticket of type Bug #395", 0 },
                    { 396, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #396.", null, 1, 3, "Demo Ticket of type Story #396", 2 },
                    { 397, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #397.", null, 1, 1, "Demo Ticket of type Bug #397", 0 },
                    { 398, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #398.", null, 1, 1, "Demo Ticket of type Bug #398", 0 },
                    { 399, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #399.", null, 1, 2, "Demo Ticket of type Task #399", 1 },
                    { 400, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #400.", null, 1, 2, "Demo Ticket of type Bug #400", 0 },
                    { 401, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #401.", null, 1, 3, "Demo Ticket of type Story #401", 2 },
                    { 402, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #402.", null, 1, 3, "Demo Ticket of type Task #402", 1 },
                    { 403, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #403.", null, 1, 3, "Demo Ticket of type Bug #403", 0 },
                    { 404, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #404.", null, 1, 1, "Demo Ticket of type Task #404", 1 },
                    { 405, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #405.", null, 1, 3, "Demo Ticket of type Story #405", 2 },
                    { 406, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #406.", null, 1, 2, "Demo Ticket of type Story #406", 2 },
                    { 407, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #407.", null, 1, 2, "Demo Ticket of type Story #407", 2 },
                    { 408, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #408.", null, 1, 2, "Demo Ticket of type Story #408", 2 },
                    { 409, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #409.", null, 1, 3, "Demo Ticket of type Task #409", 1 },
                    { 410, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #410.", null, 1, 1, "Demo Ticket of type Task #410", 1 },
                    { 411, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #411.", null, 1, 1, "Demo Ticket of type Bug #411", 0 },
                    { 412, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #412.", null, 1, 1, "Demo Ticket of type Bug #412", 0 },
                    { 413, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #413.", null, 1, 1, "Demo Ticket of type Story #413", 2 },
                    { 414, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #414.", null, 1, 3, "Demo Ticket of type Task #414", 1 },
                    { 415, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #415.", null, 1, 2, "Demo Ticket of type Story #415", 2 },
                    { 416, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #416.", null, 1, 1, "Demo Ticket of type Task #416", 1 },
                    { 417, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #417.", null, 1, 2, "Demo Ticket of type Bug #417", 0 },
                    { 418, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #418.", null, 1, 3, "Demo Ticket of type Story #418", 2 },
                    { 419, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #419.", null, 1, 1, "Demo Ticket of type Story #419", 2 },
                    { 420, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #420.", null, 1, 1, "Demo Ticket of type Bug #420", 0 },
                    { 421, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #421.", null, 1, 2, "Demo Ticket of type Bug #421", 0 },
                    { 422, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #422.", null, 1, 1, "Demo Ticket of type Task #422", 1 },
                    { 423, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #423.", null, 1, 3, "Demo Ticket of type Bug #423", 0 },
                    { 424, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #424.", null, 1, 1, "Demo Ticket of type Task #424", 1 },
                    { 425, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #425.", null, 1, 2, "Demo Ticket of type Bug #425", 0 },
                    { 426, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #426.", null, 1, 1, "Demo Ticket of type Story #426", 2 },
                    { 427, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #427.", null, 1, 1, "Demo Ticket of type Story #427", 2 },
                    { 428, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #428.", null, 1, 3, "Demo Ticket of type Bug #428", 0 },
                    { 429, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #429.", null, 1, 1, "Demo Ticket of type Story #429", 2 },
                    { 430, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #430.", null, 1, 2, "Demo Ticket of type Bug #430", 0 },
                    { 431, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #431.", null, 1, 2, "Demo Ticket of type Task #431", 1 },
                    { 432, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #432.", null, 1, 3, "Demo Ticket of type Bug #432", 0 },
                    { 433, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #433.", null, 1, 1, "Demo Ticket of type Bug #433", 0 },
                    { 434, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #434.", null, 1, 3, "Demo Ticket of type Story #434", 2 },
                    { 435, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #435.", null, 1, 3, "Demo Ticket of type Bug #435", 0 },
                    { 436, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #436.", null, 1, 3, "Demo Ticket of type Story #436", 2 },
                    { 437, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #437.", null, 1, 2, "Demo Ticket of type Bug #437", 0 },
                    { 438, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #438.", null, 1, 3, "Demo Ticket of type Bug #438", 0 },
                    { 439, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #439.", null, 1, 3, "Demo Ticket of type Task #439", 1 },
                    { 440, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #440.", null, 1, 3, "Demo Ticket of type Task #440", 1 },
                    { 441, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #441.", null, 1, 3, "Demo Ticket of type Bug #441", 0 },
                    { 442, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #442.", null, 1, 1, "Demo Ticket of type Bug #442", 0 },
                    { 443, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #443.", null, 1, 2, "Demo Ticket of type Task #443", 1 },
                    { 444, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #444.", null, 1, 2, "Demo Ticket of type Story #444", 2 },
                    { 445, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #445.", null, 1, 1, "Demo Ticket of type Task #445", 1 },
                    { 446, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #446.", null, 1, 3, "Demo Ticket of type Task #446", 1 },
                    { 447, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #447.", null, 1, 2, "Demo Ticket of type Story #447", 2 },
                    { 448, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #448.", null, 1, 2, "Demo Ticket of type Task #448", 1 },
                    { 449, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #449.", null, 1, 2, "Demo Ticket of type Task #449", 1 },
                    { 450, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #450.", null, 1, 2, "Demo Ticket of type Bug #450", 0 },
                    { 451, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #451.", null, 1, 2, "Demo Ticket of type Story #451", 2 },
                    { 452, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #452.", null, 1, 1, "Demo Ticket of type Task #452", 1 },
                    { 453, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #453.", null, 1, 1, "Demo Ticket of type Bug #453", 0 },
                    { 454, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #454.", null, 1, 1, "Demo Ticket of type Task #454", 1 },
                    { 455, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #455.", null, 1, 1, "Demo Ticket of type Task #455", 1 },
                    { 456, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #456.", null, 1, 2, "Demo Ticket of type Story #456", 2 },
                    { 457, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #457.", null, 1, 1, "Demo Ticket of type Bug #457", 0 },
                    { 458, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #458.", null, 1, 1, "Demo Ticket of type Story #458", 2 },
                    { 459, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #459.", null, 1, 2, "Demo Ticket of type Bug #459", 0 },
                    { 460, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #460.", null, 1, 2, "Demo Ticket of type Bug #460", 0 },
                    { 461, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #461.", null, 1, 3, "Demo Ticket of type Bug #461", 0 },
                    { 462, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #462.", null, 1, 1, "Demo Ticket of type Story #462", 2 },
                    { 463, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #463.", null, 1, 3, "Demo Ticket of type Story #463", 2 },
                    { 464, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #464.", null, 1, 1, "Demo Ticket of type Bug #464", 0 },
                    { 465, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #465.", null, 1, 2, "Demo Ticket of type Bug #465", 0 },
                    { 466, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #466.", null, 1, 3, "Demo Ticket of type Bug #466", 0 },
                    { 467, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #467.", null, 1, 1, "Demo Ticket of type Bug #467", 0 },
                    { 468, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #468.", null, 1, 2, "Demo Ticket of type Story #468", 2 },
                    { 469, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #469.", null, 1, 3, "Demo Ticket of type Bug #469", 0 },
                    { 470, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #470.", null, 1, 1, "Demo Ticket of type Bug #470", 0 },
                    { 471, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #471.", null, 1, 3, "Demo Ticket of type Story #471", 2 },
                    { 472, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #472.", null, 1, 2, "Demo Ticket of type Task #472", 1 },
                    { 473, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #473.", null, 1, 3, "Demo Ticket of type Task #473", 1 },
                    { 474, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #474.", null, 1, 1, "Demo Ticket of type Story #474", 2 },
                    { 475, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #475.", null, 1, 2, "Demo Ticket of type Bug #475", 0 },
                    { 476, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #476.", null, 1, 1, "Demo Ticket of type Bug #476", 0 },
                    { 477, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #477.", null, 1, 1, "Demo Ticket of type Bug #477", 0 },
                    { 478, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #478.", null, 1, 1, "Demo Ticket of type Task #478", 1 },
                    { 479, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #479.", null, 1, 1, "Demo Ticket of type Story #479", 2 },
                    { 480, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #480.", null, 1, 2, "Demo Ticket of type Task #480", 1 },
                    { 481, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #481.", null, 1, 1, "Demo Ticket of type Story #481", 2 },
                    { 482, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #482.", null, 1, 1, "Demo Ticket of type Task #482", 1 },
                    { 483, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #483.", null, 1, 3, "Demo Ticket of type Bug #483", 0 },
                    { 484, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #484.", null, 1, 2, "Demo Ticket of type Story #484", 2 },
                    { 485, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #485.", null, 1, 1, "Demo Ticket of type Bug #485", 0 },
                    { 486, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #486.", null, 1, 1, "Demo Ticket of type Story #486", 2 },
                    { 487, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #487.", null, 1, 1, "Demo Ticket of type Bug #487", 0 },
                    { 488, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #488.", null, 1, 3, "Demo Ticket of type Story #488", 2 },
                    { 489, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #489.", null, 1, 3, "Demo Ticket of type Bug #489", 0 },
                    { 490, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #490.", null, 1, 3, "Demo Ticket of type Story #490", 2 },
                    { 491, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #491.", null, 1, 2, "Demo Ticket of type Bug #491", 0 },
                    { 492, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #492.", null, 2, 1, "Demo Ticket of type Story #492", 2 },
                    { 493, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #493.", null, 2, 2, "Demo Ticket of type Bug #493", 0 },
                    { 494, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #494.", null, 2, 3, "Demo Ticket of type Story #494", 2 },
                    { 495, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #495.", null, 2, 2, "Demo Ticket of type Bug #495", 0 },
                    { 496, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #496.", null, 2, 1, "Demo Ticket of type Task #496", 1 },
                    { 497, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #497.", null, 2, 1, "Demo Ticket of type Story #497", 2 },
                    { 498, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #498.", null, 2, 3, "Demo Ticket of type Story #498", 2 },
                    { 499, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #499.", null, 2, 1, "Demo Ticket of type Bug #499", 0 },
                    { 500, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #500.", null, 2, 3, "Demo Ticket of type Bug #500", 0 },
                    { 501, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #501.", null, 2, 1, "Demo Ticket of type Story #501", 2 },
                    { 502, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #502.", null, 2, 2, "Demo Ticket of type Bug #502", 0 },
                    { 503, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #503.", null, 2, 2, "Demo Ticket of type Task #503", 1 },
                    { 504, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #504.", null, 2, 1, "Demo Ticket of type Bug #504", 0 },
                    { 505, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #505.", null, 2, 1, "Demo Ticket of type Task #505", 1 },
                    { 506, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #506.", null, 2, 3, "Demo Ticket of type Task #506", 1 },
                    { 507, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #507.", null, 2, 2, "Demo Ticket of type Story #507", 2 },
                    { 508, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #508.", null, 2, 3, "Demo Ticket of type Bug #508", 0 },
                    { 509, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #509.", null, 2, 1, "Demo Ticket of type Bug #509", 0 },
                    { 510, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #510.", null, 2, 1, "Demo Ticket of type Story #510", 2 },
                    { 511, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #511.", null, 2, 2, "Demo Ticket of type Story #511", 2 },
                    { 512, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #512.", null, 2, 3, "Demo Ticket of type Task #512", 1 },
                    { 513, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #513.", null, 2, 1, "Demo Ticket of type Bug #513", 0 },
                    { 514, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #514.", null, 2, 2, "Demo Ticket of type Bug #514", 0 },
                    { 515, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #515.", null, 2, 1, "Demo Ticket of type Bug #515", 0 },
                    { 516, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #516.", null, 2, 2, "Demo Ticket of type Bug #516", 0 },
                    { 517, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #517.", null, 2, 2, "Demo Ticket of type Task #517", 1 },
                    { 518, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #518.", null, 2, 2, "Demo Ticket of type Task #518", 1 },
                    { 519, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #519.", null, 2, 1, "Demo Ticket of type Story #519", 2 },
                    { 520, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #520.", null, 2, 2, "Demo Ticket of type Bug #520", 0 },
                    { 521, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #521.", null, 2, 1, "Demo Ticket of type Story #521", 2 },
                    { 522, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #522.", null, 2, 3, "Demo Ticket of type Bug #522", 0 },
                    { 523, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #523.", null, 2, 1, "Demo Ticket of type Bug #523", 0 },
                    { 524, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #524.", null, 2, 2, "Demo Ticket of type Bug #524", 0 },
                    { 525, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #525.", null, 2, 3, "Demo Ticket of type Story #525", 2 },
                    { 526, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #526.", null, 2, 3, "Demo Ticket of type Story #526", 2 },
                    { 527, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #527.", null, 2, 2, "Demo Ticket of type Bug #527", 0 },
                    { 528, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #528.", null, 2, 3, "Demo Ticket of type Story #528", 2 },
                    { 529, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #529.", null, 2, 1, "Demo Ticket of type Task #529", 1 },
                    { 530, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #530.", null, 2, 1, "Demo Ticket of type Bug #530", 0 },
                    { 531, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #531.", null, 2, 3, "Demo Ticket of type Story #531", 2 },
                    { 532, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #532.", null, 2, 3, "Demo Ticket of type Story #532", 2 },
                    { 533, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #533.", null, 2, 1, "Demo Ticket of type Task #533", 1 },
                    { 534, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #534.", null, 2, 2, "Demo Ticket of type Bug #534", 0 },
                    { 535, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #535.", null, 2, 3, "Demo Ticket of type Story #535", 2 },
                    { 536, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #536.", null, 2, 3, "Demo Ticket of type Bug #536", 0 },
                    { 537, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #537.", null, 2, 2, "Demo Ticket of type Bug #537", 0 },
                    { 538, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #538.", null, 2, 1, "Demo Ticket of type Story #538", 2 },
                    { 539, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #539.", null, 2, 1, "Demo Ticket of type Task #539", 1 },
                    { 540, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #540.", null, 2, 1, "Demo Ticket of type Task #540", 1 },
                    { 541, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #541.", null, 2, 1, "Demo Ticket of type Bug #541", 0 },
                    { 542, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #542.", null, 2, 3, "Demo Ticket of type Bug #542", 0 },
                    { 543, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #543.", null, 2, 3, "Demo Ticket of type Task #543", 1 },
                    { 544, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #544.", null, 2, 3, "Demo Ticket of type Story #544", 2 },
                    { 545, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #545.", null, 2, 3, "Demo Ticket of type Bug #545", 0 },
                    { 546, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #546.", null, 2, 1, "Demo Ticket of type Task #546", 1 },
                    { 547, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #547.", null, 2, 2, "Demo Ticket of type Story #547", 2 },
                    { 548, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #548.", null, 2, 1, "Demo Ticket of type Bug #548", 0 },
                    { 549, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #549.", null, 2, 1, "Demo Ticket of type Story #549", 2 },
                    { 550, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #550.", null, 2, 2, "Demo Ticket of type Story #550", 2 },
                    { 551, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #551.", null, 2, 1, "Demo Ticket of type Bug #551", 0 },
                    { 552, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #552.", null, 2, 1, "Demo Ticket of type Task #552", 1 },
                    { 553, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #553.", null, 2, 2, "Demo Ticket of type Story #553", 2 },
                    { 554, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #554.", null, 2, 3, "Demo Ticket of type Story #554", 2 },
                    { 555, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #555.", null, 2, 2, "Demo Ticket of type Task #555", 1 },
                    { 556, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #556.", null, 2, 3, "Demo Ticket of type Story #556", 2 },
                    { 557, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #557.", null, 2, 2, "Demo Ticket of type Story #557", 2 },
                    { 558, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #558.", null, 2, 1, "Demo Ticket of type Task #558", 1 },
                    { 559, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #559.", null, 2, 2, "Demo Ticket of type Bug #559", 0 },
                    { 560, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #560.", null, 2, 1, "Demo Ticket of type Story #560", 2 },
                    { 561, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #561.", null, 2, 3, "Demo Ticket of type Story #561", 2 },
                    { 562, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #562.", null, 2, 1, "Demo Ticket of type Bug #562", 0 },
                    { 563, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #563.", null, 2, 1, "Demo Ticket of type Bug #563", 0 },
                    { 564, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #564.", null, 2, 3, "Demo Ticket of type Story #564", 2 },
                    { 565, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #565.", null, 2, 2, "Demo Ticket of type Task #565", 1 },
                    { 566, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #566.", null, 2, 1, "Demo Ticket of type Task #566", 1 },
                    { 567, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #567.", null, 2, 3, "Demo Ticket of type Story #567", 2 },
                    { 568, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #568.", null, 2, 3, "Demo Ticket of type Task #568", 1 },
                    { 569, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #569.", null, 2, 3, "Demo Ticket of type Story #569", 2 },
                    { 570, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #570.", null, 2, 2, "Demo Ticket of type Task #570", 1 },
                    { 571, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #571.", null, 2, 2, "Demo Ticket of type Task #571", 1 },
                    { 572, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #572.", null, 2, 2, "Demo Ticket of type Story #572", 2 },
                    { 573, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #573.", null, 2, 1, "Demo Ticket of type Task #573", 1 },
                    { 574, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #574.", null, 2, 2, "Demo Ticket of type Story #574", 2 },
                    { 575, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #575.", null, 2, 3, "Demo Ticket of type Bug #575", 0 },
                    { 576, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #576.", null, 2, 2, "Demo Ticket of type Task #576", 1 },
                    { 577, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #577.", null, 2, 2, "Demo Ticket of type Story #577", 2 },
                    { 578, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #578.", null, 2, 2, "Demo Ticket of type Story #578", 2 },
                    { 579, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #579.", null, 2, 2, "Demo Ticket of type Story #579", 2 },
                    { 580, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #580.", null, 2, 1, "Demo Ticket of type Story #580", 2 },
                    { 581, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #581.", null, 2, 1, "Demo Ticket of type Story #581", 2 },
                    { 582, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #582.", null, 2, 3, "Demo Ticket of type Bug #582", 0 },
                    { 583, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #583.", null, 2, 2, "Demo Ticket of type Story #583", 2 },
                    { 584, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #584.", null, 2, 3, "Demo Ticket of type Story #584", 2 },
                    { 585, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #585.", null, 2, 2, "Demo Ticket of type Story #585", 2 },
                    { 586, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #586.", null, 2, 1, "Demo Ticket of type Bug #586", 0 },
                    { 587, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #587.", null, 2, 1, "Demo Ticket of type Bug #587", 0 },
                    { 588, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #588.", null, 2, 3, "Demo Ticket of type Bug #588", 0 },
                    { 589, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #589.", null, 2, 2, "Demo Ticket of type Bug #589", 0 },
                    { 590, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #590.", null, 2, 3, "Demo Ticket of type Story #590", 2 },
                    { 591, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #591.", null, 2, 1, "Demo Ticket of type Task #591", 1 },
                    { 592, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #592.", null, 2, 3, "Demo Ticket of type Bug #592", 0 },
                    { 593, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #593.", null, 2, 1, "Demo Ticket of type Task #593", 1 },
                    { 594, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #594.", null, 2, 3, "Demo Ticket of type Task #594", 1 },
                    { 595, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #595.", null, 2, 2, "Demo Ticket of type Bug #595", 0 },
                    { 596, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #596.", null, 2, 2, "Demo Ticket of type Story #596", 2 },
                    { 597, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #597.", null, 2, 1, "Demo Ticket of type Task #597", 1 },
                    { 598, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #598.", null, 2, 1, "Demo Ticket of type Task #598", 1 },
                    { 599, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #599.", null, 2, 2, "Demo Ticket of type Task #599", 1 },
                    { 600, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #600.", null, 2, 1, "Demo Ticket of type Story #600", 2 },
                    { 601, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #601.", null, 2, 3, "Demo Ticket of type Task #601", 1 },
                    { 602, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #602.", null, 2, 2, "Demo Ticket of type Task #602", 1 },
                    { 603, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #603.", null, 2, 2, "Demo Ticket of type Task #603", 1 },
                    { 604, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #604.", null, 2, 3, "Demo Ticket of type Bug #604", 0 },
                    { 605, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #605.", null, 2, 2, "Demo Ticket of type Story #605", 2 },
                    { 606, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #606.", null, 2, 3, "Demo Ticket of type Bug #606", 0 },
                    { 607, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #607.", null, 2, 1, "Demo Ticket of type Bug #607", 0 },
                    { 608, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #608.", null, 2, 1, "Demo Ticket of type Story #608", 2 },
                    { 609, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #609.", null, 2, 2, "Demo Ticket of type Story #609", 2 },
                    { 610, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #610.", null, 2, 1, "Demo Ticket of type Bug #610", 0 },
                    { 611, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #611.", null, 2, 2, "Demo Ticket of type Story #611", 2 },
                    { 612, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #612.", null, 2, 2, "Demo Ticket of type Bug #612", 0 },
                    { 613, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #613.", null, 2, 3, "Demo Ticket of type Task #613", 1 },
                    { 614, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #614.", null, 2, 1, "Demo Ticket of type Bug #614", 0 },
                    { 615, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #615.", null, 2, 3, "Demo Ticket of type Bug #615", 0 },
                    { 616, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #616.", null, 2, 1, "Demo Ticket of type Story #616", 2 },
                    { 617, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #617.", null, 2, 3, "Demo Ticket of type Bug #617", 0 },
                    { 618, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #618.", null, 2, 1, "Demo Ticket of type Story #618", 2 },
                    { 619, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #619.", null, 2, 2, "Demo Ticket of type Task #619", 1 },
                    { 620, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #620.", null, 2, 2, "Demo Ticket of type Story #620", 2 },
                    { 621, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #621.", null, 2, 2, "Demo Ticket of type Task #621", 1 },
                    { 622, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #622.", null, 2, 3, "Demo Ticket of type Task #622", 1 },
                    { 623, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #623.", null, 2, 1, "Demo Ticket of type Task #623", 1 },
                    { 624, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #624.", null, 2, 3, "Demo Ticket of type Task #624", 1 },
                    { 625, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #625.", null, 2, 2, "Demo Ticket of type Bug #625", 0 },
                    { 626, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #626.", null, 2, 3, "Demo Ticket of type Story #626", 2 },
                    { 627, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #627.", null, 2, 3, "Demo Ticket of type Story #627", 2 },
                    { 628, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #628.", null, 2, 3, "Demo Ticket of type Task #628", 1 },
                    { 629, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #629.", null, 2, 3, "Demo Ticket of type Task #629", 1 },
                    { 630, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #630.", null, 2, 3, "Demo Ticket of type Story #630", 2 },
                    { 631, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #631.", null, 2, 1, "Demo Ticket of type Task #631", 1 },
                    { 632, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #632.", null, 2, 2, "Demo Ticket of type Bug #632", 0 },
                    { 633, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #633.", null, 2, 2, "Demo Ticket of type Task #633", 1 },
                    { 634, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #634.", null, 2, 3, "Demo Ticket of type Bug #634", 0 },
                    { 635, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #635.", null, 2, 2, "Demo Ticket of type Bug #635", 0 },
                    { 636, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #636.", null, 2, 3, "Demo Ticket of type Bug #636", 0 },
                    { 637, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #637.", null, 2, 3, "Demo Ticket of type Task #637", 1 },
                    { 638, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #638.", null, 2, 1, "Demo Ticket of type Story #638", 2 },
                    { 639, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #639.", null, 2, 2, "Demo Ticket of type Story #639", 2 },
                    { 640, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #640.", null, 2, 1, "Demo Ticket of type Story #640", 2 },
                    { 641, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #641.", null, 2, 3, "Demo Ticket of type Story #641", 2 },
                    { 642, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #642.", null, 2, 1, "Demo Ticket of type Bug #642", 0 },
                    { 643, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #643.", null, 2, 3, "Demo Ticket of type Bug #643", 0 },
                    { 644, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #644.", null, 2, 3, "Demo Ticket of type Bug #644", 0 },
                    { 645, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #645.", null, 2, 1, "Demo Ticket of type Bug #645", 0 },
                    { 646, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #646.", null, 2, 3, "Demo Ticket of type Story #646", 2 },
                    { 647, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #647.", null, 2, 2, "Demo Ticket of type Task #647", 1 },
                    { 648, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #648.", null, 2, 2, "Demo Ticket of type Bug #648", 0 },
                    { 649, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #649.", null, 2, 2, "Demo Ticket of type Story #649", 2 },
                    { 650, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #650.", null, 2, 3, "Demo Ticket of type Bug #650", 0 },
                    { 651, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #651.", null, 2, 3, "Demo Ticket of type Story #651", 2 },
                    { 652, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #652.", null, 2, 2, "Demo Ticket of type Story #652", 2 },
                    { 653, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #653.", null, 2, 3, "Demo Ticket of type Bug #653", 0 },
                    { 654, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #654.", null, 2, 2, "Demo Ticket of type Bug #654", 0 },
                    { 655, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #655.", null, 2, 1, "Demo Ticket of type Bug #655", 0 },
                    { 656, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #656.", null, 2, 3, "Demo Ticket of type Story #656", 2 },
                    { 657, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #657.", null, 2, 1, "Demo Ticket of type Story #657", 2 },
                    { 658, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #658.", null, 2, 2, "Demo Ticket of type Task #658", 1 },
                    { 659, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #659.", null, 2, 1, "Demo Ticket of type Task #659", 1 },
                    { 660, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #660.", null, 2, 3, "Demo Ticket of type Task #660", 1 },
                    { 661, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #661.", null, 2, 1, "Demo Ticket of type Bug #661", 0 },
                    { 662, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #662.", null, 2, 3, "Demo Ticket of type Bug #662", 0 },
                    { 663, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #663.", null, 2, 3, "Demo Ticket of type Story #663", 2 },
                    { 664, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #664.", null, 2, 1, "Demo Ticket of type Bug #664", 0 },
                    { 665, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #665.", null, 2, 3, "Demo Ticket of type Task #665", 1 },
                    { 666, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #666.", null, 2, 3, "Demo Ticket of type Story #666", 2 },
                    { 667, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #667.", null, 2, 2, "Demo Ticket of type Task #667", 1 },
                    { 668, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #668.", null, 2, 1, "Demo Ticket of type Story #668", 2 },
                    { 669, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #669.", null, 2, 2, "Demo Ticket of type Story #669", 2 },
                    { 670, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #670.", null, 2, 2, "Demo Ticket of type Task #670", 1 },
                    { 671, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #671.", null, 2, 1, "Demo Ticket of type Bug #671", 0 },
                    { 672, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #672.", null, 2, 1, "Demo Ticket of type Task #672", 1 },
                    { 673, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #673.", null, 2, 3, "Demo Ticket of type Story #673", 2 },
                    { 674, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #674.", null, 2, 3, "Demo Ticket of type Task #674", 1 },
                    { 675, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #675.", null, 2, 1, "Demo Ticket of type Bug #675", 0 },
                    { 676, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #676.", null, 2, 2, "Demo Ticket of type Bug #676", 0 },
                    { 677, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #677.", null, 2, 2, "Demo Ticket of type Story #677", 2 },
                    { 678, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #678.", null, 2, 1, "Demo Ticket of type Task #678", 1 },
                    { 679, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #679.", null, 2, 3, "Demo Ticket of type Story #679", 2 },
                    { 680, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #680.", null, 2, 2, "Demo Ticket of type Bug #680", 0 },
                    { 681, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #681.", null, 2, 1, "Demo Ticket of type Bug #681", 0 },
                    { 682, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #682.", null, 2, 3, "Demo Ticket of type Task #682", 1 },
                    { 683, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #683.", null, 2, 2, "Demo Ticket of type Bug #683", 0 },
                    { 684, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #684.", null, 2, 1, "Demo Ticket of type Story #684", 2 },
                    { 685, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #685.", null, 2, 3, "Demo Ticket of type Bug #685", 0 },
                    { 686, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #686.", null, 2, 2, "Demo Ticket of type Task #686", 1 },
                    { 687, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #687.", null, 2, 3, "Demo Ticket of type Task #687", 1 },
                    { 688, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #688.", null, 2, 2, "Demo Ticket of type Task #688", 1 },
                    { 689, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #689.", null, 2, 1, "Demo Ticket of type Task #689", 1 },
                    { 690, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #690.", null, 2, 2, "Demo Ticket of type Story #690", 2 },
                    { 691, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #691.", null, 2, 1, "Demo Ticket of type Task #691", 1 },
                    { 692, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #692.", null, 2, 3, "Demo Ticket of type Story #692", 2 },
                    { 693, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #693.", null, 2, 2, "Demo Ticket of type Bug #693", 0 },
                    { 694, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #694.", null, 2, 3, "Demo Ticket of type Story #694", 2 },
                    { 695, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #695.", null, 2, 3, "Demo Ticket of type Bug #695", 0 },
                    { 696, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #696.", null, 2, 3, "Demo Ticket of type Bug #696", 0 },
                    { 697, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #697.", null, 2, 2, "Demo Ticket of type Bug #697", 0 },
                    { 698, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #698.", null, 2, 2, "Demo Ticket of type Story #698", 2 },
                    { 699, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #699.", null, 2, 1, "Demo Ticket of type Story #699", 2 },
                    { 700, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #700.", null, 2, 1, "Demo Ticket of type Bug #700", 0 },
                    { 701, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #701.", null, 2, 3, "Demo Ticket of type Story #701", 2 },
                    { 702, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #702.", null, 2, 1, "Demo Ticket of type Bug #702", 0 },
                    { 703, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #703.", null, 2, 2, "Demo Ticket of type Bug #703", 0 },
                    { 704, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #704.", null, 2, 1, "Demo Ticket of type Story #704", 2 },
                    { 705, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #705.", null, 2, 2, "Demo Ticket of type Task #705", 1 },
                    { 706, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #706.", null, 2, 2, "Demo Ticket of type Bug #706", 0 },
                    { 707, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #707.", null, 2, 2, "Demo Ticket of type Task #707", 1 },
                    { 708, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #708.", null, 2, 3, "Demo Ticket of type Story #708", 2 },
                    { 709, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #709.", null, 2, 1, "Demo Ticket of type Task #709", 1 },
                    { 710, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #710.", null, 2, 2, "Demo Ticket of type Task #710", 1 },
                    { 711, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #711.", null, 2, 2, "Demo Ticket of type Story #711", 2 },
                    { 712, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #712.", null, 2, 2, "Demo Ticket of type Bug #712", 0 },
                    { 713, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #713.", null, 2, 2, "Demo Ticket of type Task #713", 1 },
                    { 714, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #714.", null, 2, 2, "Demo Ticket of type Task #714", 1 },
                    { 715, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #715.", null, 2, 1, "Demo Ticket of type Story #715", 2 },
                    { 716, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #716.", null, 2, 2, "Demo Ticket of type Story #716", 2 },
                    { 717, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #717.", null, 2, 2, "Demo Ticket of type Task #717", 1 },
                    { 718, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #718.", null, 2, 3, "Demo Ticket of type Task #718", 1 },
                    { 719, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #719.", null, 2, 3, "Demo Ticket of type Bug #719", 0 },
                    { 720, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #720.", null, 2, 3, "Demo Ticket of type Story #720", 2 },
                    { 721, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #721.", null, 2, 2, "Demo Ticket of type Bug #721", 0 },
                    { 722, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #722.", null, 2, 1, "Demo Ticket of type Bug #722", 0 },
                    { 723, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #723.", null, 2, 2, "Demo Ticket of type Bug #723", 0 },
                    { 724, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #724.", null, 2, 1, "Demo Ticket of type Bug #724", 0 },
                    { 725, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #725.", null, 2, 2, "Demo Ticket of type Story #725", 2 },
                    { 726, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #726.", null, 3, 2, "Demo Ticket of type Story #726", 2 },
                    { 727, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #727.", null, 3, 3, "Demo Ticket of type Story #727", 2 },
                    { 728, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #728.", null, 3, 2, "Demo Ticket of type Task #728", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMemberEntity_ProjectId",
                table: "ProjectMemberEntity",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMemberEntity_UserId",
                table: "ProjectMemberEntity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketComments_TicketId",
                table: "TicketComments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ProjectId",
                table: "Tickets",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_StatusId",
                table: "Tickets",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectMemberEntity");

            migrationBuilder.DropTable(
                name: "TicketComments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "TicketStatus");
        }
    }
}
