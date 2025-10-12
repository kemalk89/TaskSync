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
                name: "TicketLabels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketLabels", x => x.Id);
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
                    Email = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "TicketLabelMapping",
                columns: table => new
                {
                    LabelsId = table.Column<int>(type: "integer", nullable: false),
                    TicketEntityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketLabelMapping", x => new { x.LabelsId, x.TicketEntityId });
                    table.ForeignKey(
                        name: "FK_TicketLabelMapping_TicketLabels_LabelsId",
                        column: x => x.LabelsId,
                        principalTable: "TicketLabels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketLabelMapping_Tickets_TicketEntityId",
                        column: x => x.TicketEntityId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "ModifiedDate", "Title", "Visibility" },
                values: new object[,]
                {
                    { 1, 1, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the first project. This project has one member as well.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, "My First Project", null },
                    { 2, 2, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the 2nd project. This project has two members.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, "My 2nd Project", null },
                    { 3, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the 3rd project.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, "My 3rd Project", null },
                    { 4, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the 4rd project.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, "My 4rd Project", null }
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
                    { 1, 0, new DateTimeOffset(new DateTime(2025, 10, 12, 7, 14, 42, 53, DateTimeKind.Unspecified).AddTicks(3160), new TimeSpan(0, 0, 0, 0, 0)), "Kerem.Karacay@tasksync.test", null, null, "", "Kerem Karacay" },
                    { 2, 0, new DateTimeOffset(new DateTime(2025, 10, 12, 7, 14, 42, 53, DateTimeKind.Unspecified).AddTicks(3220), new TimeSpan(0, 0, 0, 0, 0)), "Deniz.Aslansu@tasksync.test", null, null, "", "Deniz Aslansu" },
                    { 3, 0, new DateTimeOffset(new DateTime(2025, 10, 12, 7, 14, 42, 53, DateTimeKind.Unspecified).AddTicks(3230), new TimeSpan(0, 0, 0, 0, 0)), "Ali.Balci@tasksync.test", null, null, "", "Ali Balcı" },
                    { 4, 0, new DateTimeOffset(new DateTime(2025, 10, 12, 7, 14, 42, 53, DateTimeKind.Unspecified).AddTicks(3240), new TimeSpan(0, 0, 0, 0, 0)), "Sven.Imker@tasksync.test", null, null, "", "Sven Imker" },
                    { 5, 0, new DateTimeOffset(new DateTime(2025, 10, 12, 7, 14, 42, 53, DateTimeKind.Unspecified).AddTicks(3250), new TimeSpan(0, 0, 0, 0, 0)), "Mina.Koch@tasksync.test", null, null, "", "Mina Koch" },
                    { 6, 0, new DateTimeOffset(new DateTime(2025, 10, 12, 7, 14, 42, 53, DateTimeKind.Unspecified).AddTicks(3290), new TimeSpan(0, 0, 0, 0, 0)), "IntegrationTests.User1@tasksync.test", "integration_tests|01", null, "", "Test User1" }
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
                    { 50, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #50.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 1, "Demo Ticket of type Task #50", 1 },
                    { 51, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #51.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 2, "Demo Ticket of type Bug #51", 0 },
                    { 52, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #52.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 3, "Demo Ticket of type Bug #52", 0 },
                    { 53, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #53.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 2, "Demo Ticket of type Story #53", 2 },
                    { 54, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #54.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 2, "Demo Ticket of type Story #54", 2 },
                    { 55, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #55.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 3, "Demo Ticket of type Story #55", 2 },
                    { 56, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #56.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 3, "Demo Ticket of type Story #56", 2 },
                    { 57, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #57.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 2, "Demo Ticket of type Bug #57", 0 },
                    { 58, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #58.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 3, "Demo Ticket of type Task #58", 1 },
                    { 59, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #59.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 2, "Demo Ticket of type Story #59", 2 },
                    { 60, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #60.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 3, "Demo Ticket of type Story #60", 2 },
                    { 61, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #61.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 1, "Demo Ticket of type Bug #61", 0 },
                    { 62, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #62.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 2, "Demo Ticket of type Bug #62", 0 },
                    { 63, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #63.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 2, "Demo Ticket of type Task #63", 1 },
                    { 64, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #64.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 1, "Demo Ticket of type Story #64", 2 },
                    { 65, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #65.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 2, "Demo Ticket of type Story #65", 2 },
                    { 66, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #66.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 3, "Demo Ticket of type Bug #66", 0 },
                    { 67, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #67.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 2, "Demo Ticket of type Story #67", 2 },
                    { 68, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #68.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 1, "Demo Ticket of type Story #68", 2 },
                    { 69, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #69.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 2, "Demo Ticket of type Task #69", 1 },
                    { 70, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #70.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 3, "Demo Ticket of type Story #70", 2 },
                    { 71, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #71.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 2, "Demo Ticket of type Bug #71", 0 },
                    { 72, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #72.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 3, "Demo Ticket of type Task #72", 1 },
                    { 73, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #73.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 3, "Demo Ticket of type Story #73", 2 },
                    { 74, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #74.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 2, "Demo Ticket of type Bug #74", 0 },
                    { 75, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #75.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 2, "Demo Ticket of type Bug #75", 0 },
                    { 76, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #76.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 2, "Demo Ticket of type Story #76", 2 },
                    { 77, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #77.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 1, "Demo Ticket of type Task #77", 1 },
                    { 78, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #78.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 2, "Demo Ticket of type Task #78", 1 },
                    { 79, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #79.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 3, "Demo Ticket of type Bug #79", 0 },
                    { 80, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #80.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 1, "Demo Ticket of type Bug #80", 0 },
                    { 81, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #81.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 1, "Demo Ticket of type Story #81", 2 },
                    { 82, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #82.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 3, "Demo Ticket of type Task #82", 1 },
                    { 83, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #83.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 2, "Demo Ticket of type Story #83", 2 },
                    { 84, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #84.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 3, "Demo Ticket of type Story #84", 2 },
                    { 85, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #85.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 1, "Demo Ticket of type Story #85", 2 },
                    { 86, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #86.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 2, "Demo Ticket of type Bug #86", 0 },
                    { 87, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #87.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 2, "Demo Ticket of type Task #87", 1 },
                    { 88, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #88.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 3, "Demo Ticket of type Task #88", 1 },
                    { 89, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #89.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 2, "Demo Ticket of type Bug #89", 0 },
                    { 90, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #90.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 3, "Demo Ticket of type Bug #90", 0 },
                    { 91, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #91.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 1, "Demo Ticket of type Task #91", 1 },
                    { 92, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #92.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 2, "Demo Ticket of type Story #92", 2 },
                    { 93, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #93.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 1, "Demo Ticket of type Story #93", 2 },
                    { 94, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #94.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 2, "Demo Ticket of type Bug #94", 0 },
                    { 95, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #95.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 2, 1, "Demo Ticket of type Bug #95", 0 },
                    { 96, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #96.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 3, 1, "Demo Ticket of type Task #96", 1 },
                    { 97, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #97.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 3, 2, "Demo Ticket of type Task #97", 1 },
                    { 98, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #98.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 3, 2, "Demo Ticket of type Story #98", 2 }
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
                name: "IX_TicketLabelMapping_TicketEntityId",
                table: "TicketLabelMapping",
                column: "TicketEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ProjectId",
                table: "Tickets",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_StatusId",
                table: "Tickets",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectMemberEntity");

            migrationBuilder.DropTable(
                name: "TicketComments");

            migrationBuilder.DropTable(
                name: "TicketLabelMapping");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TicketLabels");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "TicketStatus");
        }
    }
}
