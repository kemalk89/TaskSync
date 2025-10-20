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
                name: "TicketLabels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketLabels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketLabels_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                values: new object[] { 1, 1, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the first project. This project has one member as well.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, "My First Project", null });

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
                    { 1, 0, new DateTimeOffset(new DateTime(2025, 10, 20, 10, 58, 7, 105, DateTimeKind.Unspecified).AddTicks(7710), new TimeSpan(0, 0, 0, 0, 0)), "Kerem.Karacay@tasksync.test", null, null, "", "Kerem Karacay" },
                    { 2, 0, new DateTimeOffset(new DateTime(2025, 10, 20, 10, 58, 7, 105, DateTimeKind.Unspecified).AddTicks(7740), new TimeSpan(0, 0, 0, 0, 0)), "Deniz.Aslansu@tasksync.test", null, null, "", "Deniz Aslansu" },
                    { 3, 0, new DateTimeOffset(new DateTime(2025, 10, 20, 10, 58, 7, 105, DateTimeKind.Unspecified).AddTicks(7750), new TimeSpan(0, 0, 0, 0, 0)), "Ali.Balci@tasksync.test", null, null, "", "Ali Balcı" },
                    { 4, 0, new DateTimeOffset(new DateTime(2025, 10, 20, 10, 58, 7, 105, DateTimeKind.Unspecified).AddTicks(7760), new TimeSpan(0, 0, 0, 0, 0)), "Sven.Imker@tasksync.test", null, null, "", "Sven Imker" },
                    { 5, 0, new DateTimeOffset(new DateTime(2025, 10, 20, 10, 58, 7, 105, DateTimeKind.Unspecified).AddTicks(7770), new TimeSpan(0, 0, 0, 0, 0)), "Mina.Koch@tasksync.test", null, null, "", "Mina Koch" },
                    { 6, 0, new DateTimeOffset(new DateTime(2025, 10, 20, 10, 58, 7, 105, DateTimeKind.Unspecified).AddTicks(7800), new TimeSpan(0, 0, 0, 0, 0)), "IntegrationTests.User1@tasksync.test", "integration_tests|01", null, "", "Test User1" }
                });

            migrationBuilder.InsertData(
                table: "ProjectMemberEntity",
                columns: new[] { "Id", "ProjectId", "Role", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "Software Developer", 1 },
                    { 2, 1, "UI / UX", 2 }
                });

            migrationBuilder.InsertData(
                table: "TicketLabels",
                columns: new[] { "Id", "ProjectId", "Text" },
                values: new object[,]
                {
                    { 1, 1, "Need's Design" },
                    { 2, 1, "Need's Discussion" },
                    { 3, 1, "Quick Fix" }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "AssigneeId", "CreatedBy", "CreatedDate", "Description", "ModifiedDate", "ProjectId", "StatusId", "Title", "Type" },
                values: new object[,]
                {
                    { 13, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #13.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 2, "Demo Ticket of type Story #13", 2 },
                    { 14, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #14.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 1, "Demo Ticket of type Task #14", 1 },
                    { 15, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #15.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 2, "Demo Ticket of type Story #15", 2 },
                    { 16, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #16.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 1, "Demo Ticket of type Bug #16", 0 },
                    { 17, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #17.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 3, "Demo Ticket of type Task #17", 1 },
                    { 18, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #18.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 3, "Demo Ticket of type Bug #18", 0 },
                    { 19, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #19.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 1, "Demo Ticket of type Task #19", 1 },
                    { 20, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #20.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 2, "Demo Ticket of type Story #20", 2 },
                    { 21, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #21.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 2, "Demo Ticket of type Story #21", 2 },
                    { 22, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #22.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 1, "Demo Ticket of type Bug #22", 0 },
                    { 23, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #23.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 3, "Demo Ticket of type Story #23", 2 },
                    { 24, null, 0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "{\n  \"type\": \"doc\",\n  \"content\": [\n    {\n      \"type\": \"paragraph\",\n      \"content\": [\n        {\n          \"type\": \"text\",\n          \"text\": \"This is the description of the demo ticket #24.\"\n        }\n      ]\n    },\n    { \"type\": \"paragraph\" }\n  ]\n}", null, 1, 3, "Demo Ticket of type Story #24", 2 }
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
                name: "IX_TicketLabels_ProjectId",
                table: "TicketLabels",
                column: "ProjectId");

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
