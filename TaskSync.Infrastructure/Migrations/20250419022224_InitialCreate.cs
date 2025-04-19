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
                    { 365, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #365.", null, 1, 1, "Demo Ticket of type Bug #365", 0 },
                    { 366, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #366.", null, 1, 3, "Demo Ticket of type Story #366", 2 },
                    { 367, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #367.", null, 1, 3, "Demo Ticket of type Story #367", 2 },
                    { 368, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #368.", null, 1, 3, "Demo Ticket of type Story #368", 2 },
                    { 369, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #369.", null, 1, 2, "Demo Ticket of type Story #369", 2 },
                    { 370, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #370.", null, 1, 3, "Demo Ticket of type Task #370", 1 },
                    { 371, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #371.", null, 1, 2, "Demo Ticket of type Bug #371", 0 },
                    { 372, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #372.", null, 1, 1, "Demo Ticket of type Task #372", 1 },
                    { 373, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #373.", null, 1, 3, "Demo Ticket of type Bug #373", 0 },
                    { 374, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #374.", null, 1, 1, "Demo Ticket of type Bug #374", 0 },
                    { 375, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #375.", null, 1, 1, "Demo Ticket of type Story #375", 2 },
                    { 376, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #376.", null, 1, 2, "Demo Ticket of type Task #376", 1 },
                    { 377, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #377.", null, 1, 2, "Demo Ticket of type Bug #377", 0 },
                    { 378, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #378.", null, 1, 3, "Demo Ticket of type Bug #378", 0 },
                    { 379, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #379.", null, 1, 1, "Demo Ticket of type Story #379", 2 },
                    { 380, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #380.", null, 1, 2, "Demo Ticket of type Story #380", 2 },
                    { 381, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #381.", null, 1, 2, "Demo Ticket of type Bug #381", 0 },
                    { 382, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #382.", null, 1, 3, "Demo Ticket of type Bug #382", 0 },
                    { 383, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #383.", null, 1, 3, "Demo Ticket of type Story #383", 2 },
                    { 384, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #384.", null, 1, 1, "Demo Ticket of type Story #384", 2 },
                    { 385, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #385.", null, 1, 1, "Demo Ticket of type Story #385", 2 },
                    { 386, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #386.", null, 1, 2, "Demo Ticket of type Task #386", 1 },
                    { 387, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #387.", null, 1, 3, "Demo Ticket of type Bug #387", 0 },
                    { 388, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #388.", null, 1, 2, "Demo Ticket of type Story #388", 2 },
                    { 389, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #389.", null, 1, 3, "Demo Ticket of type Task #389", 1 },
                    { 390, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #390.", null, 1, 3, "Demo Ticket of type Story #390", 2 },
                    { 391, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #391.", null, 1, 3, "Demo Ticket of type Bug #391", 0 },
                    { 392, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #392.", null, 1, 2, "Demo Ticket of type Task #392", 1 },
                    { 393, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #393.", null, 1, 1, "Demo Ticket of type Bug #393", 0 },
                    { 394, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #394.", null, 1, 1, "Demo Ticket of type Story #394", 2 },
                    { 395, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #395.", null, 1, 2, "Demo Ticket of type Story #395", 2 },
                    { 396, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #396.", null, 1, 2, "Demo Ticket of type Bug #396", 0 },
                    { 397, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #397.", null, 1, 3, "Demo Ticket of type Story #397", 2 },
                    { 398, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #398.", null, 1, 2, "Demo Ticket of type Task #398", 1 },
                    { 399, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #399.", null, 1, 2, "Demo Ticket of type Task #399", 1 },
                    { 400, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #400.", null, 1, 3, "Demo Ticket of type Task #400", 1 },
                    { 401, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #401.", null, 1, 3, "Demo Ticket of type Task #401", 1 },
                    { 402, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #402.", null, 1, 2, "Demo Ticket of type Task #402", 1 },
                    { 403, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #403.", null, 1, 1, "Demo Ticket of type Bug #403", 0 },
                    { 404, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #404.", null, 1, 1, "Demo Ticket of type Bug #404", 0 },
                    { 405, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #405.", null, 1, 3, "Demo Ticket of type Task #405", 1 },
                    { 406, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #406.", null, 1, 1, "Demo Ticket of type Task #406", 1 },
                    { 407, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #407.", null, 1, 3, "Demo Ticket of type Bug #407", 0 },
                    { 408, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #408.", null, 1, 3, "Demo Ticket of type Story #408", 2 },
                    { 409, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #409.", null, 1, 1, "Demo Ticket of type Bug #409", 0 },
                    { 410, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #410.", null, 1, 1, "Demo Ticket of type Bug #410", 0 },
                    { 411, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #411.", null, 1, 1, "Demo Ticket of type Task #411", 1 },
                    { 412, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #412.", null, 1, 3, "Demo Ticket of type Bug #412", 0 },
                    { 413, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #413.", null, 1, 3, "Demo Ticket of type Bug #413", 0 },
                    { 414, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #414.", null, 1, 2, "Demo Ticket of type Task #414", 1 },
                    { 415, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #415.", null, 1, 2, "Demo Ticket of type Bug #415", 0 },
                    { 416, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #416.", null, 1, 3, "Demo Ticket of type Bug #416", 0 },
                    { 417, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #417.", null, 1, 2, "Demo Ticket of type Task #417", 1 },
                    { 418, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #418.", null, 1, 3, "Demo Ticket of type Task #418", 1 },
                    { 419, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #419.", null, 1, 2, "Demo Ticket of type Story #419", 2 },
                    { 420, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #420.", null, 1, 1, "Demo Ticket of type Bug #420", 0 },
                    { 421, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #421.", null, 1, 1, "Demo Ticket of type Story #421", 2 },
                    { 422, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #422.", null, 1, 2, "Demo Ticket of type Task #422", 1 },
                    { 423, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #423.", null, 1, 1, "Demo Ticket of type Task #423", 1 },
                    { 424, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #424.", null, 1, 1, "Demo Ticket of type Story #424", 2 },
                    { 425, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #425.", null, 1, 3, "Demo Ticket of type Bug #425", 0 },
                    { 426, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #426.", null, 1, 2, "Demo Ticket of type Story #426", 2 },
                    { 427, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #427.", null, 1, 1, "Demo Ticket of type Task #427", 1 },
                    { 428, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #428.", null, 1, 3, "Demo Ticket of type Story #428", 2 },
                    { 429, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #429.", null, 1, 1, "Demo Ticket of type Story #429", 2 },
                    { 430, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #430.", null, 1, 1, "Demo Ticket of type Story #430", 2 },
                    { 431, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #431.", null, 1, 2, "Demo Ticket of type Task #431", 1 },
                    { 432, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #432.", null, 1, 1, "Demo Ticket of type Task #432", 1 },
                    { 433, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #433.", null, 1, 2, "Demo Ticket of type Task #433", 1 },
                    { 434, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #434.", null, 1, 1, "Demo Ticket of type Story #434", 2 },
                    { 435, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #435.", null, 1, 3, "Demo Ticket of type Story #435", 2 },
                    { 436, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #436.", null, 1, 2, "Demo Ticket of type Task #436", 1 },
                    { 437, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #437.", null, 1, 2, "Demo Ticket of type Bug #437", 0 },
                    { 438, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #438.", null, 1, 3, "Demo Ticket of type Bug #438", 0 },
                    { 439, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #439.", null, 1, 2, "Demo Ticket of type Task #439", 1 },
                    { 440, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #440.", null, 1, 3, "Demo Ticket of type Bug #440", 0 },
                    { 441, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #441.", null, 1, 2, "Demo Ticket of type Bug #441", 0 },
                    { 442, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #442.", null, 1, 2, "Demo Ticket of type Task #442", 1 },
                    { 443, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #443.", null, 1, 3, "Demo Ticket of type Task #443", 1 },
                    { 444, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #444.", null, 1, 3, "Demo Ticket of type Task #444", 1 },
                    { 445, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #445.", null, 1, 2, "Demo Ticket of type Task #445", 1 },
                    { 446, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #446.", null, 1, 1, "Demo Ticket of type Task #446", 1 },
                    { 447, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #447.", null, 1, 1, "Demo Ticket of type Task #447", 1 },
                    { 448, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #448.", null, 1, 1, "Demo Ticket of type Bug #448", 0 },
                    { 449, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #449.", null, 1, 2, "Demo Ticket of type Bug #449", 0 },
                    { 450, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #450.", null, 1, 1, "Demo Ticket of type Bug #450", 0 },
                    { 451, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #451.", null, 1, 1, "Demo Ticket of type Task #451", 1 },
                    { 452, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #452.", null, 1, 2, "Demo Ticket of type Task #452", 1 },
                    { 453, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #453.", null, 1, 1, "Demo Ticket of type Story #453", 2 },
                    { 454, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #454.", null, 1, 1, "Demo Ticket of type Story #454", 2 },
                    { 455, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #455.", null, 1, 2, "Demo Ticket of type Story #455", 2 },
                    { 456, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #456.", null, 1, 1, "Demo Ticket of type Task #456", 1 },
                    { 457, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #457.", null, 1, 2, "Demo Ticket of type Task #457", 1 },
                    { 458, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #458.", null, 1, 3, "Demo Ticket of type Bug #458", 0 },
                    { 459, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #459.", null, 1, 2, "Demo Ticket of type Task #459", 1 },
                    { 460, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #460.", null, 1, 3, "Demo Ticket of type Task #460", 1 },
                    { 461, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #461.", null, 1, 1, "Demo Ticket of type Task #461", 1 },
                    { 462, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #462.", null, 1, 3, "Demo Ticket of type Task #462", 1 },
                    { 463, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #463.", null, 1, 2, "Demo Ticket of type Bug #463", 0 },
                    { 464, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #464.", null, 1, 2, "Demo Ticket of type Story #464", 2 },
                    { 465, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #465.", null, 1, 2, "Demo Ticket of type Story #465", 2 },
                    { 466, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #466.", null, 1, 1, "Demo Ticket of type Bug #466", 0 },
                    { 467, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #467.", null, 1, 1, "Demo Ticket of type Task #467", 1 },
                    { 468, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #468.", null, 1, 1, "Demo Ticket of type Task #468", 1 },
                    { 469, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #469.", null, 1, 3, "Demo Ticket of type Bug #469", 0 },
                    { 470, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #470.", null, 1, 3, "Demo Ticket of type Bug #470", 0 },
                    { 471, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #471.", null, 1, 1, "Demo Ticket of type Task #471", 1 },
                    { 472, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #472.", null, 1, 2, "Demo Ticket of type Bug #472", 0 },
                    { 473, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #473.", null, 1, 3, "Demo Ticket of type Bug #473", 0 },
                    { 474, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #474.", null, 1, 1, "Demo Ticket of type Story #474", 2 },
                    { 475, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #475.", null, 1, 2, "Demo Ticket of type Story #475", 2 },
                    { 476, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #476.", null, 1, 2, "Demo Ticket of type Story #476", 2 },
                    { 477, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #477.", null, 1, 3, "Demo Ticket of type Story #477", 2 },
                    { 478, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #478.", null, 1, 1, "Demo Ticket of type Task #478", 1 },
                    { 479, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #479.", null, 1, 2, "Demo Ticket of type Task #479", 1 },
                    { 480, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #480.", null, 1, 1, "Demo Ticket of type Bug #480", 0 },
                    { 481, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #481.", null, 1, 1, "Demo Ticket of type Story #481", 2 },
                    { 482, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #482.", null, 1, 1, "Demo Ticket of type Bug #482", 0 },
                    { 483, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #483.", null, 1, 3, "Demo Ticket of type Task #483", 1 },
                    { 484, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #484.", null, 1, 3, "Demo Ticket of type Bug #484", 0 },
                    { 485, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #485.", null, 1, 1, "Demo Ticket of type Story #485", 2 },
                    { 486, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #486.", null, 1, 3, "Demo Ticket of type Story #486", 2 },
                    { 487, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #487.", null, 1, 2, "Demo Ticket of type Story #487", 2 },
                    { 488, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #488.", null, 1, 1, "Demo Ticket of type Task #488", 1 },
                    { 489, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #489.", null, 1, 1, "Demo Ticket of type Story #489", 2 },
                    { 490, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #490.", null, 1, 2, "Demo Ticket of type Story #490", 2 },
                    { 491, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #491.", null, 1, 1, "Demo Ticket of type Story #491", 2 },
                    { 492, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #492.", null, 2, 3, "Demo Ticket of type Task #492", 1 },
                    { 493, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #493.", null, 2, 3, "Demo Ticket of type Task #493", 1 },
                    { 494, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #494.", null, 2, 1, "Demo Ticket of type Bug #494", 0 },
                    { 495, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #495.", null, 2, 3, "Demo Ticket of type Bug #495", 0 },
                    { 496, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #496.", null, 2, 2, "Demo Ticket of type Task #496", 1 },
                    { 497, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #497.", null, 2, 3, "Demo Ticket of type Story #497", 2 },
                    { 498, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #498.", null, 2, 1, "Demo Ticket of type Bug #498", 0 },
                    { 499, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #499.", null, 2, 3, "Demo Ticket of type Bug #499", 0 },
                    { 500, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #500.", null, 2, 2, "Demo Ticket of type Task #500", 1 },
                    { 501, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #501.", null, 2, 1, "Demo Ticket of type Bug #501", 0 },
                    { 502, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #502.", null, 2, 3, "Demo Ticket of type Story #502", 2 },
                    { 503, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #503.", null, 2, 1, "Demo Ticket of type Story #503", 2 },
                    { 504, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #504.", null, 2, 1, "Demo Ticket of type Task #504", 1 },
                    { 505, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #505.", null, 2, 3, "Demo Ticket of type Bug #505", 0 },
                    { 506, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #506.", null, 2, 2, "Demo Ticket of type Bug #506", 0 },
                    { 507, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #507.", null, 2, 1, "Demo Ticket of type Task #507", 1 },
                    { 508, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #508.", null, 2, 2, "Demo Ticket of type Bug #508", 0 },
                    { 509, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #509.", null, 2, 1, "Demo Ticket of type Task #509", 1 },
                    { 510, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #510.", null, 2, 2, "Demo Ticket of type Bug #510", 0 },
                    { 511, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #511.", null, 2, 1, "Demo Ticket of type Story #511", 2 },
                    { 512, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #512.", null, 2, 3, "Demo Ticket of type Bug #512", 0 },
                    { 513, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #513.", null, 2, 3, "Demo Ticket of type Bug #513", 0 },
                    { 514, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #514.", null, 2, 1, "Demo Ticket of type Bug #514", 0 },
                    { 515, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #515.", null, 2, 1, "Demo Ticket of type Story #515", 2 },
                    { 516, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #516.", null, 2, 1, "Demo Ticket of type Story #516", 2 },
                    { 517, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #517.", null, 2, 1, "Demo Ticket of type Task #517", 1 },
                    { 518, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #518.", null, 2, 3, "Demo Ticket of type Task #518", 1 },
                    { 519, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #519.", null, 2, 3, "Demo Ticket of type Bug #519", 0 },
                    { 520, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #520.", null, 2, 1, "Demo Ticket of type Task #520", 1 },
                    { 521, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #521.", null, 2, 1, "Demo Ticket of type Task #521", 1 },
                    { 522, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #522.", null, 2, 2, "Demo Ticket of type Story #522", 2 },
                    { 523, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #523.", null, 2, 3, "Demo Ticket of type Story #523", 2 },
                    { 524, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #524.", null, 2, 2, "Demo Ticket of type Bug #524", 0 },
                    { 525, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #525.", null, 2, 1, "Demo Ticket of type Bug #525", 0 },
                    { 526, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #526.", null, 2, 3, "Demo Ticket of type Bug #526", 0 },
                    { 527, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #527.", null, 2, 3, "Demo Ticket of type Task #527", 1 },
                    { 528, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #528.", null, 2, 2, "Demo Ticket of type Task #528", 1 },
                    { 529, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #529.", null, 2, 3, "Demo Ticket of type Task #529", 1 },
                    { 530, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #530.", null, 2, 1, "Demo Ticket of type Story #530", 2 },
                    { 531, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #531.", null, 2, 1, "Demo Ticket of type Bug #531", 0 },
                    { 532, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #532.", null, 2, 2, "Demo Ticket of type Bug #532", 0 },
                    { 533, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #533.", null, 2, 2, "Demo Ticket of type Story #533", 2 },
                    { 534, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #534.", null, 2, 3, "Demo Ticket of type Bug #534", 0 },
                    { 535, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #535.", null, 2, 3, "Demo Ticket of type Bug #535", 0 },
                    { 536, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #536.", null, 2, 3, "Demo Ticket of type Story #536", 2 },
                    { 537, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #537.", null, 2, 1, "Demo Ticket of type Story #537", 2 },
                    { 538, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #538.", null, 2, 3, "Demo Ticket of type Task #538", 1 },
                    { 539, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #539.", null, 2, 2, "Demo Ticket of type Story #539", 2 },
                    { 540, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #540.", null, 2, 1, "Demo Ticket of type Bug #540", 0 },
                    { 541, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #541.", null, 2, 3, "Demo Ticket of type Bug #541", 0 },
                    { 542, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #542.", null, 2, 2, "Demo Ticket of type Bug #542", 0 },
                    { 543, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #543.", null, 2, 1, "Demo Ticket of type Story #543", 2 },
                    { 544, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #544.", null, 2, 2, "Demo Ticket of type Task #544", 1 },
                    { 545, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #545.", null, 2, 3, "Demo Ticket of type Story #545", 2 },
                    { 546, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #546.", null, 2, 3, "Demo Ticket of type Task #546", 1 },
                    { 547, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #547.", null, 2, 1, "Demo Ticket of type Bug #547", 0 },
                    { 548, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #548.", null, 2, 3, "Demo Ticket of type Bug #548", 0 },
                    { 549, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #549.", null, 2, 2, "Demo Ticket of type Story #549", 2 },
                    { 550, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #550.", null, 2, 2, "Demo Ticket of type Story #550", 2 },
                    { 551, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #551.", null, 2, 2, "Demo Ticket of type Bug #551", 0 },
                    { 552, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #552.", null, 2, 2, "Demo Ticket of type Story #552", 2 },
                    { 553, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #553.", null, 2, 1, "Demo Ticket of type Story #553", 2 },
                    { 554, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #554.", null, 2, 2, "Demo Ticket of type Task #554", 1 },
                    { 555, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #555.", null, 2, 1, "Demo Ticket of type Task #555", 1 },
                    { 556, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #556.", null, 2, 3, "Demo Ticket of type Task #556", 1 },
                    { 557, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #557.", null, 2, 3, "Demo Ticket of type Bug #557", 0 },
                    { 558, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #558.", null, 2, 3, "Demo Ticket of type Bug #558", 0 },
                    { 559, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #559.", null, 2, 2, "Demo Ticket of type Task #559", 1 },
                    { 560, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #560.", null, 2, 1, "Demo Ticket of type Story #560", 2 },
                    { 561, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #561.", null, 2, 2, "Demo Ticket of type Task #561", 1 },
                    { 562, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #562.", null, 2, 1, "Demo Ticket of type Story #562", 2 },
                    { 563, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #563.", null, 2, 2, "Demo Ticket of type Task #563", 1 },
                    { 564, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #564.", null, 2, 1, "Demo Ticket of type Bug #564", 0 },
                    { 565, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #565.", null, 2, 3, "Demo Ticket of type Bug #565", 0 },
                    { 566, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #566.", null, 2, 2, "Demo Ticket of type Task #566", 1 },
                    { 567, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #567.", null, 2, 1, "Demo Ticket of type Bug #567", 0 },
                    { 568, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #568.", null, 2, 3, "Demo Ticket of type Task #568", 1 },
                    { 569, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #569.", null, 2, 2, "Demo Ticket of type Story #569", 2 },
                    { 570, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #570.", null, 2, 1, "Demo Ticket of type Story #570", 2 },
                    { 571, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #571.", null, 2, 1, "Demo Ticket of type Story #571", 2 },
                    { 572, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #572.", null, 2, 3, "Demo Ticket of type Bug #572", 0 },
                    { 573, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #573.", null, 2, 2, "Demo Ticket of type Story #573", 2 },
                    { 574, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #574.", null, 2, 1, "Demo Ticket of type Task #574", 1 },
                    { 575, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #575.", null, 2, 2, "Demo Ticket of type Story #575", 2 },
                    { 576, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #576.", null, 2, 1, "Demo Ticket of type Story #576", 2 },
                    { 577, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #577.", null, 2, 2, "Demo Ticket of type Task #577", 1 },
                    { 578, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #578.", null, 2, 2, "Demo Ticket of type Bug #578", 0 },
                    { 579, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #579.", null, 2, 3, "Demo Ticket of type Bug #579", 0 },
                    { 580, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #580.", null, 2, 2, "Demo Ticket of type Story #580", 2 },
                    { 581, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #581.", null, 2, 1, "Demo Ticket of type Story #581", 2 },
                    { 582, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #582.", null, 2, 1, "Demo Ticket of type Bug #582", 0 },
                    { 583, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #583.", null, 2, 3, "Demo Ticket of type Task #583", 1 },
                    { 584, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #584.", null, 2, 2, "Demo Ticket of type Bug #584", 0 },
                    { 585, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #585.", null, 2, 1, "Demo Ticket of type Task #585", 1 },
                    { 586, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #586.", null, 2, 2, "Demo Ticket of type Bug #586", 0 },
                    { 587, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #587.", null, 2, 3, "Demo Ticket of type Story #587", 2 },
                    { 588, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #588.", null, 2, 3, "Demo Ticket of type Story #588", 2 },
                    { 589, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #589.", null, 2, 1, "Demo Ticket of type Story #589", 2 },
                    { 590, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #590.", null, 2, 2, "Demo Ticket of type Bug #590", 0 },
                    { 591, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #591.", null, 2, 3, "Demo Ticket of type Task #591", 1 },
                    { 592, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #592.", null, 2, 3, "Demo Ticket of type Task #592", 1 },
                    { 593, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #593.", null, 2, 1, "Demo Ticket of type Task #593", 1 },
                    { 594, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #594.", null, 2, 2, "Demo Ticket of type Story #594", 2 },
                    { 595, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #595.", null, 2, 3, "Demo Ticket of type Story #595", 2 },
                    { 596, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #596.", null, 2, 1, "Demo Ticket of type Story #596", 2 },
                    { 597, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #597.", null, 2, 3, "Demo Ticket of type Story #597", 2 },
                    { 598, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #598.", null, 2, 1, "Demo Ticket of type Story #598", 2 },
                    { 599, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #599.", null, 2, 1, "Demo Ticket of type Bug #599", 0 },
                    { 600, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #600.", null, 2, 2, "Demo Ticket of type Story #600", 2 },
                    { 601, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #601.", null, 2, 1, "Demo Ticket of type Story #601", 2 },
                    { 602, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #602.", null, 2, 2, "Demo Ticket of type Bug #602", 0 },
                    { 603, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #603.", null, 2, 2, "Demo Ticket of type Task #603", 1 },
                    { 604, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #604.", null, 2, 1, "Demo Ticket of type Task #604", 1 },
                    { 605, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #605.", null, 2, 2, "Demo Ticket of type Bug #605", 0 },
                    { 606, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #606.", null, 2, 1, "Demo Ticket of type Task #606", 1 },
                    { 607, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #607.", null, 2, 3, "Demo Ticket of type Story #607", 2 },
                    { 608, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #608.", null, 2, 2, "Demo Ticket of type Story #608", 2 },
                    { 609, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #609.", null, 2, 2, "Demo Ticket of type Task #609", 1 },
                    { 610, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #610.", null, 2, 1, "Demo Ticket of type Bug #610", 0 },
                    { 611, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #611.", null, 2, 1, "Demo Ticket of type Story #611", 2 },
                    { 612, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #612.", null, 2, 1, "Demo Ticket of type Story #612", 2 },
                    { 613, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #613.", null, 2, 3, "Demo Ticket of type Story #613", 2 },
                    { 614, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #614.", null, 2, 3, "Demo Ticket of type Story #614", 2 },
                    { 615, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #615.", null, 2, 3, "Demo Ticket of type Task #615", 1 },
                    { 616, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #616.", null, 2, 2, "Demo Ticket of type Task #616", 1 },
                    { 617, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #617.", null, 2, 3, "Demo Ticket of type Bug #617", 0 },
                    { 618, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #618.", null, 2, 1, "Demo Ticket of type Task #618", 1 },
                    { 619, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #619.", null, 2, 1, "Demo Ticket of type Task #619", 1 },
                    { 620, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #620.", null, 2, 2, "Demo Ticket of type Story #620", 2 },
                    { 621, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #621.", null, 2, 1, "Demo Ticket of type Task #621", 1 },
                    { 622, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #622.", null, 2, 3, "Demo Ticket of type Story #622", 2 },
                    { 623, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #623.", null, 2, 1, "Demo Ticket of type Story #623", 2 },
                    { 624, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #624.", null, 2, 1, "Demo Ticket of type Bug #624", 0 },
                    { 625, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #625.", null, 2, 1, "Demo Ticket of type Bug #625", 0 },
                    { 626, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #626.", null, 2, 3, "Demo Ticket of type Task #626", 1 },
                    { 627, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #627.", null, 2, 2, "Demo Ticket of type Story #627", 2 },
                    { 628, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #628.", null, 2, 3, "Demo Ticket of type Bug #628", 0 },
                    { 629, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #629.", null, 2, 2, "Demo Ticket of type Task #629", 1 },
                    { 630, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #630.", null, 2, 3, "Demo Ticket of type Story #630", 2 },
                    { 631, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #631.", null, 2, 2, "Demo Ticket of type Story #631", 2 },
                    { 632, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #632.", null, 2, 2, "Demo Ticket of type Task #632", 1 },
                    { 633, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #633.", null, 2, 3, "Demo Ticket of type Bug #633", 0 },
                    { 634, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #634.", null, 2, 3, "Demo Ticket of type Bug #634", 0 },
                    { 635, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #635.", null, 2, 3, "Demo Ticket of type Bug #635", 0 },
                    { 636, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #636.", null, 2, 3, "Demo Ticket of type Story #636", 2 },
                    { 637, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #637.", null, 2, 3, "Demo Ticket of type Bug #637", 0 },
                    { 638, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #638.", null, 2, 1, "Demo Ticket of type Bug #638", 0 },
                    { 639, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #639.", null, 2, 2, "Demo Ticket of type Bug #639", 0 },
                    { 640, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #640.", null, 2, 3, "Demo Ticket of type Bug #640", 0 },
                    { 641, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #641.", null, 2, 3, "Demo Ticket of type Bug #641", 0 },
                    { 642, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #642.", null, 2, 1, "Demo Ticket of type Story #642", 2 },
                    { 643, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #643.", null, 2, 3, "Demo Ticket of type Task #643", 1 },
                    { 644, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #644.", null, 2, 2, "Demo Ticket of type Task #644", 1 },
                    { 645, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #645.", null, 2, 2, "Demo Ticket of type Task #645", 1 },
                    { 646, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #646.", null, 2, 2, "Demo Ticket of type Bug #646", 0 },
                    { 647, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #647.", null, 2, 1, "Demo Ticket of type Bug #647", 0 },
                    { 648, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #648.", null, 2, 2, "Demo Ticket of type Bug #648", 0 },
                    { 649, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #649.", null, 2, 3, "Demo Ticket of type Bug #649", 0 },
                    { 650, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #650.", null, 2, 3, "Demo Ticket of type Bug #650", 0 },
                    { 651, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #651.", null, 2, 3, "Demo Ticket of type Task #651", 1 },
                    { 652, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #652.", null, 2, 1, "Demo Ticket of type Task #652", 1 },
                    { 653, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #653.", null, 2, 3, "Demo Ticket of type Bug #653", 0 },
                    { 654, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #654.", null, 2, 3, "Demo Ticket of type Bug #654", 0 },
                    { 655, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #655.", null, 2, 2, "Demo Ticket of type Story #655", 2 },
                    { 656, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #656.", null, 2, 3, "Demo Ticket of type Task #656", 1 },
                    { 657, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #657.", null, 2, 3, "Demo Ticket of type Task #657", 1 },
                    { 658, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #658.", null, 2, 2, "Demo Ticket of type Task #658", 1 },
                    { 659, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #659.", null, 2, 1, "Demo Ticket of type Bug #659", 0 },
                    { 660, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #660.", null, 2, 1, "Demo Ticket of type Bug #660", 0 },
                    { 661, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #661.", null, 2, 3, "Demo Ticket of type Bug #661", 0 },
                    { 662, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #662.", null, 2, 2, "Demo Ticket of type Bug #662", 0 },
                    { 663, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #663.", null, 2, 3, "Demo Ticket of type Task #663", 1 },
                    { 664, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #664.", null, 2, 2, "Demo Ticket of type Bug #664", 0 },
                    { 665, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #665.", null, 2, 2, "Demo Ticket of type Story #665", 2 },
                    { 666, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #666.", null, 2, 1, "Demo Ticket of type Bug #666", 0 },
                    { 667, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #667.", null, 2, 1, "Demo Ticket of type Bug #667", 0 },
                    { 668, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #668.", null, 2, 1, "Demo Ticket of type Bug #668", 0 },
                    { 669, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #669.", null, 2, 1, "Demo Ticket of type Story #669", 2 },
                    { 670, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #670.", null, 2, 1, "Demo Ticket of type Bug #670", 0 },
                    { 671, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #671.", null, 2, 3, "Demo Ticket of type Bug #671", 0 },
                    { 672, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #672.", null, 2, 1, "Demo Ticket of type Bug #672", 0 },
                    { 673, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #673.", null, 2, 1, "Demo Ticket of type Story #673", 2 },
                    { 674, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #674.", null, 2, 3, "Demo Ticket of type Story #674", 2 },
                    { 675, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #675.", null, 2, 1, "Demo Ticket of type Bug #675", 0 },
                    { 676, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #676.", null, 2, 3, "Demo Ticket of type Task #676", 1 },
                    { 677, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #677.", null, 2, 3, "Demo Ticket of type Task #677", 1 },
                    { 678, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #678.", null, 2, 1, "Demo Ticket of type Story #678", 2 },
                    { 679, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #679.", null, 2, 3, "Demo Ticket of type Story #679", 2 },
                    { 680, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #680.", null, 2, 1, "Demo Ticket of type Bug #680", 0 },
                    { 681, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #681.", null, 2, 3, "Demo Ticket of type Bug #681", 0 },
                    { 682, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #682.", null, 2, 3, "Demo Ticket of type Bug #682", 0 },
                    { 683, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #683.", null, 2, 3, "Demo Ticket of type Task #683", 1 },
                    { 684, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #684.", null, 2, 3, "Demo Ticket of type Bug #684", 0 },
                    { 685, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #685.", null, 2, 1, "Demo Ticket of type Task #685", 1 },
                    { 686, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #686.", null, 2, 3, "Demo Ticket of type Story #686", 2 },
                    { 687, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #687.", null, 2, 2, "Demo Ticket of type Story #687", 2 },
                    { 688, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #688.", null, 2, 2, "Demo Ticket of type Story #688", 2 },
                    { 689, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #689.", null, 2, 2, "Demo Ticket of type Story #689", 2 },
                    { 690, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #690.", null, 2, 2, "Demo Ticket of type Task #690", 1 },
                    { 691, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #691.", null, 2, 2, "Demo Ticket of type Bug #691", 0 },
                    { 692, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #692.", null, 2, 3, "Demo Ticket of type Story #692", 2 },
                    { 693, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #693.", null, 2, 1, "Demo Ticket of type Task #693", 1 },
                    { 694, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #694.", null, 2, 1, "Demo Ticket of type Story #694", 2 },
                    { 695, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #695.", null, 2, 3, "Demo Ticket of type Task #695", 1 },
                    { 696, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #696.", null, 2, 3, "Demo Ticket of type Task #696", 1 },
                    { 697, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #697.", null, 2, 2, "Demo Ticket of type Task #697", 1 },
                    { 698, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #698.", null, 2, 2, "Demo Ticket of type Bug #698", 0 },
                    { 699, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #699.", null, 2, 2, "Demo Ticket of type Bug #699", 0 },
                    { 700, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #700.", null, 2, 1, "Demo Ticket of type Story #700", 2 },
                    { 701, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #701.", null, 2, 3, "Demo Ticket of type Bug #701", 0 },
                    { 702, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #702.", null, 2, 1, "Demo Ticket of type Story #702", 2 },
                    { 703, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #703.", null, 2, 3, "Demo Ticket of type Story #703", 2 },
                    { 704, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #704.", null, 2, 2, "Demo Ticket of type Bug #704", 0 },
                    { 705, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #705.", null, 2, 2, "Demo Ticket of type Story #705", 2 },
                    { 706, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #706.", null, 2, 1, "Demo Ticket of type Story #706", 2 },
                    { 707, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #707.", null, 2, 2, "Demo Ticket of type Story #707", 2 },
                    { 708, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #708.", null, 2, 1, "Demo Ticket of type Task #708", 1 },
                    { 709, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #709.", null, 2, 2, "Demo Ticket of type Bug #709", 0 },
                    { 710, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #710.", null, 2, 3, "Demo Ticket of type Bug #710", 0 },
                    { 711, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #711.", null, 2, 1, "Demo Ticket of type Bug #711", 0 },
                    { 712, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #712.", null, 2, 2, "Demo Ticket of type Task #712", 1 },
                    { 713, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #713.", null, 2, 2, "Demo Ticket of type Bug #713", 0 },
                    { 714, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #714.", null, 2, 3, "Demo Ticket of type Task #714", 1 },
                    { 715, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #715.", null, 2, 3, "Demo Ticket of type Bug #715", 0 },
                    { 716, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #716.", null, 2, 1, "Demo Ticket of type Story #716", 2 },
                    { 717, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #717.", null, 2, 1, "Demo Ticket of type Bug #717", 0 },
                    { 718, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #718.", null, 2, 3, "Demo Ticket of type Bug #718", 0 },
                    { 719, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #719.", null, 2, 1, "Demo Ticket of type Task #719", 1 },
                    { 720, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #720.", null, 2, 3, "Demo Ticket of type Task #720", 1 },
                    { 721, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #721.", null, 2, 3, "Demo Ticket of type Task #721", 1 },
                    { 722, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #722.", null, 2, 3, "Demo Ticket of type Task #722", 1 },
                    { 723, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #723.", null, 2, 3, "Demo Ticket of type Bug #723", 0 },
                    { 724, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #724.", null, 2, 1, "Demo Ticket of type Bug #724", 0 },
                    { 725, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #725.", null, 2, 2, "Demo Ticket of type Task #725", 1 },
                    { 726, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #726.", null, 3, 3, "Demo Ticket of type Bug #726", 0 },
                    { 727, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #727.", null, 3, 3, "Demo Ticket of type Bug #727", 0 },
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
