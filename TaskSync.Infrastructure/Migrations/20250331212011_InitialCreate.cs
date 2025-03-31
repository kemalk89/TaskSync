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
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
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
                name: "ProjectMemberEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Role = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMemberEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMemberEntity_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: true),
                    AssigneeId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
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
                name: "TicketComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
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
                    { 1, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the first project.", null, "My First Project", null },
                    { 2, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the 2nd project.", null, "My 2nd Project", null },
                    { 3, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the 3rd project.", null, "My 3rd Project", null },
                    { 4, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the 4rd project.", null, "My 4rd Project", null }
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
                table: "Tickets",
                columns: new[] { "Id", "AssigneeId", "CreatedBy", "CreatedDate", "Description", "ModifiedDate", "ProjectId", "StatusId", "Title" },
                values: new object[,]
                {
                    { 365, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #365.", null, 1, 2, "Demo Ticket #365" },
                    { 366, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #366.", null, 1, 2, "Demo Ticket #366" },
                    { 367, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #367.", null, 1, 3, "Demo Ticket #367" },
                    { 368, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #368.", null, 1, 1, "Demo Ticket #368" },
                    { 369, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #369.", null, 1, 2, "Demo Ticket #369" },
                    { 370, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #370.", null, 1, 2, "Demo Ticket #370" },
                    { 371, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #371.", null, 1, 2, "Demo Ticket #371" },
                    { 372, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #372.", null, 1, 2, "Demo Ticket #372" },
                    { 373, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #373.", null, 1, 1, "Demo Ticket #373" },
                    { 374, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #374.", null, 1, 1, "Demo Ticket #374" },
                    { 375, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #375.", null, 1, 3, "Demo Ticket #375" },
                    { 376, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #376.", null, 1, 3, "Demo Ticket #376" },
                    { 377, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #377.", null, 1, 3, "Demo Ticket #377" },
                    { 378, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #378.", null, 1, 3, "Demo Ticket #378" },
                    { 379, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #379.", null, 1, 2, "Demo Ticket #379" },
                    { 380, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #380.", null, 1, 2, "Demo Ticket #380" },
                    { 381, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #381.", null, 1, 2, "Demo Ticket #381" },
                    { 382, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #382.", null, 1, 2, "Demo Ticket #382" },
                    { 383, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #383.", null, 1, 3, "Demo Ticket #383" },
                    { 384, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #384.", null, 1, 1, "Demo Ticket #384" },
                    { 385, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #385.", null, 1, 2, "Demo Ticket #385" },
                    { 386, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #386.", null, 1, 2, "Demo Ticket #386" },
                    { 387, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #387.", null, 1, 2, "Demo Ticket #387" },
                    { 388, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #388.", null, 1, 1, "Demo Ticket #388" },
                    { 389, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #389.", null, 1, 3, "Demo Ticket #389" },
                    { 390, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #390.", null, 1, 1, "Demo Ticket #390" },
                    { 391, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #391.", null, 1, 2, "Demo Ticket #391" },
                    { 392, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #392.", null, 1, 1, "Demo Ticket #392" },
                    { 393, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #393.", null, 1, 1, "Demo Ticket #393" },
                    { 394, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #394.", null, 1, 1, "Demo Ticket #394" },
                    { 395, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #395.", null, 1, 3, "Demo Ticket #395" },
                    { 396, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #396.", null, 1, 3, "Demo Ticket #396" },
                    { 397, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #397.", null, 1, 3, "Demo Ticket #397" },
                    { 398, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #398.", null, 1, 1, "Demo Ticket #398" },
                    { 399, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #399.", null, 1, 2, "Demo Ticket #399" },
                    { 400, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #400.", null, 1, 1, "Demo Ticket #400" },
                    { 401, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #401.", null, 1, 1, "Demo Ticket #401" },
                    { 402, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #402.", null, 1, 1, "Demo Ticket #402" },
                    { 403, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #403.", null, 1, 3, "Demo Ticket #403" },
                    { 404, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #404.", null, 1, 1, "Demo Ticket #404" },
                    { 405, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #405.", null, 1, 3, "Demo Ticket #405" },
                    { 406, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #406.", null, 1, 2, "Demo Ticket #406" },
                    { 407, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #407.", null, 1, 2, "Demo Ticket #407" },
                    { 408, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #408.", null, 1, 2, "Demo Ticket #408" },
                    { 409, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #409.", null, 1, 2, "Demo Ticket #409" },
                    { 410, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #410.", null, 1, 2, "Demo Ticket #410" },
                    { 411, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #411.", null, 1, 1, "Demo Ticket #411" },
                    { 412, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #412.", null, 1, 1, "Demo Ticket #412" },
                    { 413, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #413.", null, 1, 1, "Demo Ticket #413" },
                    { 414, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #414.", null, 1, 2, "Demo Ticket #414" },
                    { 415, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #415.", null, 1, 2, "Demo Ticket #415" },
                    { 416, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #416.", null, 1, 2, "Demo Ticket #416" },
                    { 417, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #417.", null, 1, 1, "Demo Ticket #417" },
                    { 418, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #418.", null, 1, 1, "Demo Ticket #418" },
                    { 419, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #419.", null, 1, 2, "Demo Ticket #419" },
                    { 420, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #420.", null, 1, 2, "Demo Ticket #420" },
                    { 421, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #421.", null, 1, 2, "Demo Ticket #421" },
                    { 422, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #422.", null, 1, 2, "Demo Ticket #422" },
                    { 423, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #423.", null, 1, 3, "Demo Ticket #423" },
                    { 424, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #424.", null, 1, 1, "Demo Ticket #424" },
                    { 425, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #425.", null, 1, 1, "Demo Ticket #425" },
                    { 426, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #426.", null, 1, 3, "Demo Ticket #426" },
                    { 427, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #427.", null, 1, 3, "Demo Ticket #427" },
                    { 428, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #428.", null, 1, 2, "Demo Ticket #428" },
                    { 429, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #429.", null, 1, 1, "Demo Ticket #429" },
                    { 430, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #430.", null, 1, 3, "Demo Ticket #430" },
                    { 431, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #431.", null, 1, 1, "Demo Ticket #431" },
                    { 432, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #432.", null, 1, 1, "Demo Ticket #432" },
                    { 433, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #433.", null, 1, 1, "Demo Ticket #433" },
                    { 434, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #434.", null, 1, 3, "Demo Ticket #434" },
                    { 435, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #435.", null, 1, 2, "Demo Ticket #435" },
                    { 436, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #436.", null, 1, 2, "Demo Ticket #436" },
                    { 437, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #437.", null, 1, 1, "Demo Ticket #437" },
                    { 438, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #438.", null, 1, 2, "Demo Ticket #438" },
                    { 439, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #439.", null, 1, 3, "Demo Ticket #439" },
                    { 440, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #440.", null, 1, 3, "Demo Ticket #440" },
                    { 441, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #441.", null, 1, 3, "Demo Ticket #441" },
                    { 442, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #442.", null, 1, 2, "Demo Ticket #442" },
                    { 443, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #443.", null, 1, 1, "Demo Ticket #443" },
                    { 444, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #444.", null, 1, 3, "Demo Ticket #444" },
                    { 445, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #445.", null, 1, 2, "Demo Ticket #445" },
                    { 446, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #446.", null, 1, 2, "Demo Ticket #446" },
                    { 447, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #447.", null, 1, 1, "Demo Ticket #447" },
                    { 448, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #448.", null, 1, 2, "Demo Ticket #448" },
                    { 449, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #449.", null, 1, 1, "Demo Ticket #449" },
                    { 450, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #450.", null, 1, 3, "Demo Ticket #450" },
                    { 451, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #451.", null, 1, 2, "Demo Ticket #451" },
                    { 452, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #452.", null, 1, 2, "Demo Ticket #452" },
                    { 453, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #453.", null, 1, 3, "Demo Ticket #453" },
                    { 454, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #454.", null, 1, 2, "Demo Ticket #454" },
                    { 455, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #455.", null, 1, 2, "Demo Ticket #455" },
                    { 456, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #456.", null, 1, 3, "Demo Ticket #456" },
                    { 457, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #457.", null, 1, 3, "Demo Ticket #457" },
                    { 458, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #458.", null, 1, 3, "Demo Ticket #458" },
                    { 459, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #459.", null, 1, 3, "Demo Ticket #459" },
                    { 460, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #460.", null, 1, 1, "Demo Ticket #460" },
                    { 461, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #461.", null, 1, 3, "Demo Ticket #461" },
                    { 462, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #462.", null, 1, 2, "Demo Ticket #462" },
                    { 463, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #463.", null, 1, 3, "Demo Ticket #463" },
                    { 464, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #464.", null, 1, 3, "Demo Ticket #464" },
                    { 465, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #465.", null, 1, 2, "Demo Ticket #465" },
                    { 466, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #466.", null, 1, 1, "Demo Ticket #466" },
                    { 467, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #467.", null, 1, 1, "Demo Ticket #467" },
                    { 468, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #468.", null, 1, 1, "Demo Ticket #468" },
                    { 469, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #469.", null, 1, 2, "Demo Ticket #469" },
                    { 470, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #470.", null, 1, 2, "Demo Ticket #470" },
                    { 471, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #471.", null, 1, 1, "Demo Ticket #471" },
                    { 472, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #472.", null, 1, 1, "Demo Ticket #472" },
                    { 473, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #473.", null, 1, 3, "Demo Ticket #473" },
                    { 474, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #474.", null, 1, 3, "Demo Ticket #474" },
                    { 475, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #475.", null, 1, 2, "Demo Ticket #475" },
                    { 476, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #476.", null, 1, 3, "Demo Ticket #476" },
                    { 477, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #477.", null, 1, 2, "Demo Ticket #477" },
                    { 478, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #478.", null, 1, 2, "Demo Ticket #478" },
                    { 479, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #479.", null, 1, 1, "Demo Ticket #479" },
                    { 480, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #480.", null, 1, 3, "Demo Ticket #480" },
                    { 481, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #481.", null, 1, 2, "Demo Ticket #481" },
                    { 482, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #482.", null, 1, 1, "Demo Ticket #482" },
                    { 483, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #483.", null, 1, 2, "Demo Ticket #483" },
                    { 484, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #484.", null, 1, 2, "Demo Ticket #484" },
                    { 485, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #485.", null, 1, 2, "Demo Ticket #485" },
                    { 486, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #486.", null, 1, 1, "Demo Ticket #486" },
                    { 487, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #487.", null, 1, 3, "Demo Ticket #487" },
                    { 488, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #488.", null, 1, 1, "Demo Ticket #488" },
                    { 489, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #489.", null, 1, 2, "Demo Ticket #489" },
                    { 490, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #490.", null, 1, 3, "Demo Ticket #490" },
                    { 491, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #491.", null, 1, 3, "Demo Ticket #491" },
                    { 492, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #492.", null, 2, 3, "Demo Ticket #492" },
                    { 493, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #493.", null, 2, 3, "Demo Ticket #493" },
                    { 494, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #494.", null, 2, 1, "Demo Ticket #494" },
                    { 495, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #495.", null, 2, 3, "Demo Ticket #495" },
                    { 496, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #496.", null, 2, 3, "Demo Ticket #496" },
                    { 497, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #497.", null, 2, 3, "Demo Ticket #497" },
                    { 498, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #498.", null, 2, 1, "Demo Ticket #498" },
                    { 499, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #499.", null, 2, 2, "Demo Ticket #499" },
                    { 500, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #500.", null, 2, 1, "Demo Ticket #500" },
                    { 501, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #501.", null, 2, 3, "Demo Ticket #501" },
                    { 502, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #502.", null, 2, 1, "Demo Ticket #502" },
                    { 503, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #503.", null, 2, 3, "Demo Ticket #503" },
                    { 504, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #504.", null, 2, 3, "Demo Ticket #504" },
                    { 505, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #505.", null, 2, 3, "Demo Ticket #505" },
                    { 506, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #506.", null, 2, 3, "Demo Ticket #506" },
                    { 507, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #507.", null, 2, 1, "Demo Ticket #507" },
                    { 508, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #508.", null, 2, 2, "Demo Ticket #508" },
                    { 509, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #509.", null, 2, 1, "Demo Ticket #509" },
                    { 510, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #510.", null, 2, 3, "Demo Ticket #510" },
                    { 511, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #511.", null, 2, 3, "Demo Ticket #511" },
                    { 512, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #512.", null, 2, 3, "Demo Ticket #512" },
                    { 513, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #513.", null, 2, 1, "Demo Ticket #513" },
                    { 514, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #514.", null, 2, 3, "Demo Ticket #514" },
                    { 515, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #515.", null, 2, 3, "Demo Ticket #515" },
                    { 516, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #516.", null, 2, 3, "Demo Ticket #516" },
                    { 517, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #517.", null, 2, 2, "Demo Ticket #517" },
                    { 518, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #518.", null, 2, 3, "Demo Ticket #518" },
                    { 519, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #519.", null, 2, 2, "Demo Ticket #519" },
                    { 520, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #520.", null, 2, 3, "Demo Ticket #520" },
                    { 521, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #521.", null, 2, 1, "Demo Ticket #521" },
                    { 522, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #522.", null, 2, 2, "Demo Ticket #522" },
                    { 523, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #523.", null, 2, 1, "Demo Ticket #523" },
                    { 524, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #524.", null, 2, 1, "Demo Ticket #524" },
                    { 525, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #525.", null, 2, 3, "Demo Ticket #525" },
                    { 526, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #526.", null, 2, 3, "Demo Ticket #526" },
                    { 527, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #527.", null, 2, 3, "Demo Ticket #527" },
                    { 528, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #528.", null, 2, 3, "Demo Ticket #528" },
                    { 529, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #529.", null, 2, 1, "Demo Ticket #529" },
                    { 530, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #530.", null, 2, 2, "Demo Ticket #530" },
                    { 531, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #531.", null, 2, 2, "Demo Ticket #531" },
                    { 532, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #532.", null, 2, 3, "Demo Ticket #532" },
                    { 533, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #533.", null, 2, 3, "Demo Ticket #533" },
                    { 534, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #534.", null, 2, 1, "Demo Ticket #534" },
                    { 535, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #535.", null, 2, 1, "Demo Ticket #535" },
                    { 536, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #536.", null, 2, 1, "Demo Ticket #536" },
                    { 537, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #537.", null, 2, 3, "Demo Ticket #537" },
                    { 538, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #538.", null, 2, 3, "Demo Ticket #538" },
                    { 539, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #539.", null, 2, 3, "Demo Ticket #539" },
                    { 540, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #540.", null, 2, 2, "Demo Ticket #540" },
                    { 541, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #541.", null, 2, 2, "Demo Ticket #541" },
                    { 542, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #542.", null, 2, 2, "Demo Ticket #542" },
                    { 543, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #543.", null, 2, 2, "Demo Ticket #543" },
                    { 544, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #544.", null, 2, 2, "Demo Ticket #544" },
                    { 545, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #545.", null, 2, 3, "Demo Ticket #545" },
                    { 546, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #546.", null, 2, 1, "Demo Ticket #546" },
                    { 547, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #547.", null, 2, 2, "Demo Ticket #547" },
                    { 548, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #548.", null, 2, 3, "Demo Ticket #548" },
                    { 549, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #549.", null, 2, 1, "Demo Ticket #549" },
                    { 550, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #550.", null, 2, 3, "Demo Ticket #550" },
                    { 551, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #551.", null, 2, 2, "Demo Ticket #551" },
                    { 552, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #552.", null, 2, 1, "Demo Ticket #552" },
                    { 553, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #553.", null, 2, 3, "Demo Ticket #553" },
                    { 554, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #554.", null, 2, 2, "Demo Ticket #554" },
                    { 555, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #555.", null, 2, 1, "Demo Ticket #555" },
                    { 556, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #556.", null, 2, 2, "Demo Ticket #556" },
                    { 557, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #557.", null, 2, 2, "Demo Ticket #557" },
                    { 558, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #558.", null, 2, 3, "Demo Ticket #558" },
                    { 559, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #559.", null, 2, 2, "Demo Ticket #559" },
                    { 560, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #560.", null, 2, 1, "Demo Ticket #560" },
                    { 561, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #561.", null, 2, 3, "Demo Ticket #561" },
                    { 562, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #562.", null, 2, 3, "Demo Ticket #562" },
                    { 563, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #563.", null, 2, 3, "Demo Ticket #563" },
                    { 564, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #564.", null, 2, 1, "Demo Ticket #564" },
                    { 565, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #565.", null, 2, 1, "Demo Ticket #565" },
                    { 566, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #566.", null, 2, 2, "Demo Ticket #566" },
                    { 567, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #567.", null, 2, 1, "Demo Ticket #567" },
                    { 568, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #568.", null, 2, 3, "Demo Ticket #568" },
                    { 569, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #569.", null, 2, 3, "Demo Ticket #569" },
                    { 570, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #570.", null, 2, 2, "Demo Ticket #570" },
                    { 571, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #571.", null, 2, 1, "Demo Ticket #571" },
                    { 572, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #572.", null, 2, 1, "Demo Ticket #572" },
                    { 573, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #573.", null, 2, 3, "Demo Ticket #573" },
                    { 574, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #574.", null, 2, 1, "Demo Ticket #574" },
                    { 575, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #575.", null, 2, 3, "Demo Ticket #575" },
                    { 576, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #576.", null, 2, 2, "Demo Ticket #576" },
                    { 577, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #577.", null, 2, 3, "Demo Ticket #577" },
                    { 578, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #578.", null, 2, 2, "Demo Ticket #578" },
                    { 579, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #579.", null, 2, 1, "Demo Ticket #579" },
                    { 580, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #580.", null, 2, 3, "Demo Ticket #580" },
                    { 581, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #581.", null, 2, 3, "Demo Ticket #581" },
                    { 582, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #582.", null, 2, 1, "Demo Ticket #582" },
                    { 583, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #583.", null, 2, 1, "Demo Ticket #583" },
                    { 584, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #584.", null, 2, 1, "Demo Ticket #584" },
                    { 585, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #585.", null, 2, 2, "Demo Ticket #585" },
                    { 586, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #586.", null, 2, 3, "Demo Ticket #586" },
                    { 587, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #587.", null, 2, 2, "Demo Ticket #587" },
                    { 588, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #588.", null, 2, 2, "Demo Ticket #588" },
                    { 589, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #589.", null, 2, 1, "Demo Ticket #589" },
                    { 590, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #590.", null, 2, 3, "Demo Ticket #590" },
                    { 591, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #591.", null, 2, 3, "Demo Ticket #591" },
                    { 592, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #592.", null, 2, 1, "Demo Ticket #592" },
                    { 593, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #593.", null, 2, 2, "Demo Ticket #593" },
                    { 594, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #594.", null, 2, 1, "Demo Ticket #594" },
                    { 595, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #595.", null, 2, 3, "Demo Ticket #595" },
                    { 596, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #596.", null, 2, 3, "Demo Ticket #596" },
                    { 597, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #597.", null, 2, 3, "Demo Ticket #597" },
                    { 598, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #598.", null, 2, 3, "Demo Ticket #598" },
                    { 599, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #599.", null, 2, 3, "Demo Ticket #599" },
                    { 600, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #600.", null, 2, 3, "Demo Ticket #600" },
                    { 601, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #601.", null, 2, 2, "Demo Ticket #601" },
                    { 602, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #602.", null, 2, 2, "Demo Ticket #602" },
                    { 603, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #603.", null, 2, 2, "Demo Ticket #603" },
                    { 604, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #604.", null, 2, 3, "Demo Ticket #604" },
                    { 605, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #605.", null, 2, 1, "Demo Ticket #605" },
                    { 606, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #606.", null, 2, 1, "Demo Ticket #606" },
                    { 607, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #607.", null, 2, 1, "Demo Ticket #607" },
                    { 608, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #608.", null, 2, 2, "Demo Ticket #608" },
                    { 609, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #609.", null, 2, 3, "Demo Ticket #609" },
                    { 610, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #610.", null, 2, 3, "Demo Ticket #610" },
                    { 611, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #611.", null, 2, 1, "Demo Ticket #611" },
                    { 612, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #612.", null, 2, 2, "Demo Ticket #612" },
                    { 613, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #613.", null, 2, 2, "Demo Ticket #613" },
                    { 614, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #614.", null, 2, 2, "Demo Ticket #614" },
                    { 615, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #615.", null, 2, 2, "Demo Ticket #615" },
                    { 616, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #616.", null, 2, 2, "Demo Ticket #616" },
                    { 617, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #617.", null, 2, 3, "Demo Ticket #617" },
                    { 618, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #618.", null, 2, 3, "Demo Ticket #618" },
                    { 619, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #619.", null, 2, 2, "Demo Ticket #619" },
                    { 620, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #620.", null, 2, 2, "Demo Ticket #620" },
                    { 621, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #621.", null, 2, 2, "Demo Ticket #621" },
                    { 622, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #622.", null, 2, 3, "Demo Ticket #622" },
                    { 623, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #623.", null, 2, 3, "Demo Ticket #623" },
                    { 624, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #624.", null, 2, 3, "Demo Ticket #624" },
                    { 625, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #625.", null, 2, 2, "Demo Ticket #625" },
                    { 626, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #626.", null, 2, 3, "Demo Ticket #626" },
                    { 627, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #627.", null, 2, 3, "Demo Ticket #627" },
                    { 628, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #628.", null, 2, 2, "Demo Ticket #628" },
                    { 629, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #629.", null, 2, 2, "Demo Ticket #629" },
                    { 630, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #630.", null, 2, 2, "Demo Ticket #630" },
                    { 631, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #631.", null, 2, 2, "Demo Ticket #631" },
                    { 632, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #632.", null, 2, 2, "Demo Ticket #632" },
                    { 633, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #633.", null, 2, 3, "Demo Ticket #633" },
                    { 634, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #634.", null, 2, 3, "Demo Ticket #634" },
                    { 635, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #635.", null, 2, 1, "Demo Ticket #635" },
                    { 636, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #636.", null, 2, 3, "Demo Ticket #636" },
                    { 637, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #637.", null, 2, 2, "Demo Ticket #637" },
                    { 638, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #638.", null, 2, 3, "Demo Ticket #638" },
                    { 639, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #639.", null, 2, 2, "Demo Ticket #639" },
                    { 640, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #640.", null, 2, 1, "Demo Ticket #640" },
                    { 641, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #641.", null, 2, 1, "Demo Ticket #641" },
                    { 642, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #642.", null, 2, 1, "Demo Ticket #642" },
                    { 643, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #643.", null, 2, 1, "Demo Ticket #643" },
                    { 644, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #644.", null, 2, 3, "Demo Ticket #644" },
                    { 645, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #645.", null, 2, 3, "Demo Ticket #645" },
                    { 646, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #646.", null, 2, 3, "Demo Ticket #646" },
                    { 647, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #647.", null, 2, 1, "Demo Ticket #647" },
                    { 648, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #648.", null, 2, 2, "Demo Ticket #648" },
                    { 649, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #649.", null, 2, 3, "Demo Ticket #649" },
                    { 650, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #650.", null, 2, 1, "Demo Ticket #650" },
                    { 651, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #651.", null, 2, 2, "Demo Ticket #651" },
                    { 652, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #652.", null, 2, 2, "Demo Ticket #652" },
                    { 653, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #653.", null, 2, 2, "Demo Ticket #653" },
                    { 654, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #654.", null, 2, 1, "Demo Ticket #654" },
                    { 655, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #655.", null, 2, 2, "Demo Ticket #655" },
                    { 656, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #656.", null, 2, 1, "Demo Ticket #656" },
                    { 657, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #657.", null, 2, 3, "Demo Ticket #657" },
                    { 658, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #658.", null, 2, 2, "Demo Ticket #658" },
                    { 659, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #659.", null, 2, 2, "Demo Ticket #659" },
                    { 660, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #660.", null, 2, 1, "Demo Ticket #660" },
                    { 661, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #661.", null, 2, 3, "Demo Ticket #661" },
                    { 662, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #662.", null, 2, 2, "Demo Ticket #662" },
                    { 663, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #663.", null, 2, 1, "Demo Ticket #663" },
                    { 664, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #664.", null, 2, 2, "Demo Ticket #664" },
                    { 665, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #665.", null, 2, 1, "Demo Ticket #665" },
                    { 666, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #666.", null, 2, 3, "Demo Ticket #666" },
                    { 667, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #667.", null, 2, 2, "Demo Ticket #667" },
                    { 668, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #668.", null, 2, 3, "Demo Ticket #668" },
                    { 669, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #669.", null, 2, 3, "Demo Ticket #669" },
                    { 670, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #670.", null, 2, 1, "Demo Ticket #670" },
                    { 671, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #671.", null, 2, 1, "Demo Ticket #671" },
                    { 672, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #672.", null, 2, 1, "Demo Ticket #672" },
                    { 673, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #673.", null, 2, 1, "Demo Ticket #673" },
                    { 674, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #674.", null, 2, 2, "Demo Ticket #674" },
                    { 675, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #675.", null, 2, 1, "Demo Ticket #675" },
                    { 676, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #676.", null, 2, 2, "Demo Ticket #676" },
                    { 677, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #677.", null, 2, 1, "Demo Ticket #677" },
                    { 678, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #678.", null, 2, 1, "Demo Ticket #678" },
                    { 679, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #679.", null, 2, 2, "Demo Ticket #679" },
                    { 680, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #680.", null, 2, 3, "Demo Ticket #680" },
                    { 681, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #681.", null, 2, 1, "Demo Ticket #681" },
                    { 682, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #682.", null, 2, 3, "Demo Ticket #682" },
                    { 683, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #683.", null, 2, 3, "Demo Ticket #683" },
                    { 684, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #684.", null, 2, 2, "Demo Ticket #684" },
                    { 685, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #685.", null, 2, 1, "Demo Ticket #685" },
                    { 686, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #686.", null, 2, 3, "Demo Ticket #686" },
                    { 687, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #687.", null, 2, 1, "Demo Ticket #687" },
                    { 688, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #688.", null, 2, 1, "Demo Ticket #688" },
                    { 689, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #689.", null, 2, 1, "Demo Ticket #689" },
                    { 690, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #690.", null, 2, 3, "Demo Ticket #690" },
                    { 691, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #691.", null, 2, 2, "Demo Ticket #691" },
                    { 692, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #692.", null, 2, 1, "Demo Ticket #692" },
                    { 693, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #693.", null, 2, 3, "Demo Ticket #693" },
                    { 694, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #694.", null, 2, 2, "Demo Ticket #694" },
                    { 695, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #695.", null, 2, 2, "Demo Ticket #695" },
                    { 696, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #696.", null, 2, 3, "Demo Ticket #696" },
                    { 697, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #697.", null, 2, 2, "Demo Ticket #697" },
                    { 698, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #698.", null, 2, 2, "Demo Ticket #698" },
                    { 699, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #699.", null, 2, 2, "Demo Ticket #699" },
                    { 700, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #700.", null, 2, 1, "Demo Ticket #700" },
                    { 701, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #701.", null, 2, 1, "Demo Ticket #701" },
                    { 702, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #702.", null, 2, 3, "Demo Ticket #702" },
                    { 703, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #703.", null, 2, 3, "Demo Ticket #703" },
                    { 704, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #704.", null, 2, 3, "Demo Ticket #704" },
                    { 705, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #705.", null, 2, 1, "Demo Ticket #705" },
                    { 706, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #706.", null, 2, 2, "Demo Ticket #706" },
                    { 707, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #707.", null, 2, 1, "Demo Ticket #707" },
                    { 708, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #708.", null, 2, 2, "Demo Ticket #708" },
                    { 709, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #709.", null, 2, 3, "Demo Ticket #709" },
                    { 710, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #710.", null, 2, 1, "Demo Ticket #710" },
                    { 711, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #711.", null, 2, 3, "Demo Ticket #711" },
                    { 712, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #712.", null, 2, 2, "Demo Ticket #712" },
                    { 713, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #713.", null, 2, 1, "Demo Ticket #713" },
                    { 714, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #714.", null, 2, 2, "Demo Ticket #714" },
                    { 715, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #715.", null, 2, 1, "Demo Ticket #715" },
                    { 716, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #716.", null, 2, 3, "Demo Ticket #716" },
                    { 717, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #717.", null, 2, 3, "Demo Ticket #717" },
                    { 718, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #718.", null, 2, 1, "Demo Ticket #718" },
                    { 719, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #719.", null, 2, 3, "Demo Ticket #719" },
                    { 720, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #720.", null, 2, 3, "Demo Ticket #720" },
                    { 721, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #721.", null, 2, 3, "Demo Ticket #721" },
                    { 722, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #722.", null, 2, 3, "Demo Ticket #722" },
                    { 723, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #723.", null, 2, 1, "Demo Ticket #723" },
                    { 724, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #724.", null, 2, 3, "Demo Ticket #724" },
                    { 725, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #725.", null, 2, 3, "Demo Ticket #725" },
                    { 726, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #726.", null, 3, 2, "Demo Ticket #726" },
                    { 727, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #727.", null, 3, 2, "Demo Ticket #727" },
                    { 728, null, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "This is the description of the demo ticket #728.", null, 3, 1, "Demo Ticket #728" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMemberEntity_ProjectId",
                table: "ProjectMemberEntity",
                column: "ProjectId");

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
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "TicketStatus");
        }
    }
}
