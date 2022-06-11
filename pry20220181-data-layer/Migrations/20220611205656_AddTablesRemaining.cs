using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pry20220181_data_layer.Migrations
{
    public partial class AddTablesRemaining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Vaccines",
                newName: "VaccineId");

            migrationBuilder.AddColumn<float>(
                name: "MaxTemperature",
                table: "Vaccines",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "MinTemperature",
                table: "Vaccines",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "Children",
                columns: table => new
                {
                    ChildId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DNI = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Firstname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Lastname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Birthdate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Gender = table.Column<string>(type: "varchar(1)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Children", x => x.ChildId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HealthPersonnel",
                columns: table => new
                {
                    HealthPersonnelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthPersonnel", x => x.HealthPersonnelId);
                    table.ForeignKey(
                        name: "FK_HealthPersonnel_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ubigeo",
                columns: table => new
                {
                    UbigeoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UbigeoCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Region = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Province = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ubigeo", x => x.UbigeoId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VaccinationCampaigns",
                columns: table => new
                {
                    VaccinationCampaignId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinationCampaigns", x => x.VaccinationCampaignId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VaccinationSchemes",
                columns: table => new
                {
                    VaccinationSchemeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InitialAge = table.Column<int>(type: "int", nullable: false),
                    FinalAge = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinationSchemes", x => x.VaccinationSchemeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HealthCenters",
                columns: table => new
                {
                    HealthCenterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UbigeoId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCenters", x => x.HealthCenterId);
                    table.ForeignKey(
                        name: "FK_HealthCenters_Ubigeo_UbigeoId",
                        column: x => x.UbigeoId,
                        principalTable: "Ubigeo",
                        principalColumn: "UbigeoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    ParentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UbigeoId = table.Column<int>(type: "int", nullable: false),
                    DNI = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telephone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.ParentId);
                    table.ForeignKey(
                        name: "FK_Parents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parents_Ubigeo_UbigeoId",
                        column: x => x.UbigeoId,
                        principalTable: "Ubigeo",
                        principalColumn: "UbigeoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VaccinationCampaignDetails",
                columns: table => new
                {
                    VaccinationCampaignDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VaccineId = table.Column<int>(type: "int", nullable: false),
                    VaccinationCampaignId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinationCampaignDetails", x => x.VaccinationCampaignDetailId);
                    table.ForeignKey(
                        name: "FK_VaccinationCampaignDetails_VaccinationCampaigns_VaccinationC~",
                        column: x => x.VaccinationCampaignId,
                        principalTable: "VaccinationCampaigns",
                        principalColumn: "VaccinationCampaignId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VaccinationCampaignDetails_Vaccines_VaccineId",
                        column: x => x.VaccineId,
                        principalTable: "Vaccines",
                        principalColumn: "VaccineId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VaccinationSchemeDetails",
                columns: table => new
                {
                    VaccinationSchemeDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VaccineId = table.Column<int>(type: "int", nullable: false),
                    VaccinationSchemeId = table.Column<int>(type: "int", nullable: false),
                    NumberOfDosesToAdminister = table.Column<int>(type: "int", nullable: false),
                    PossibleEffectsPostVaccine = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinationSchemeDetails", x => x.VaccinationSchemeDetailId);
                    table.ForeignKey(
                        name: "FK_VaccinationSchemeDetails_VaccinationSchemes_VaccinationSchem~",
                        column: x => x.VaccinationSchemeId,
                        principalTable: "VaccinationSchemes",
                        principalColumn: "VaccinationSchemeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VaccinationSchemeDetails_Vaccines_VaccineId",
                        column: x => x.VaccineId,
                        principalTable: "Vaccines",
                        principalColumn: "VaccineId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VaccinationCampaignLocations",
                columns: table => new
                {
                    VaccinationCampaignLocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VaccinationCampaignId = table.Column<int>(type: "int", nullable: false),
                    HealthCenterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinationCampaignLocations", x => x.VaccinationCampaignLocationId);
                    table.ForeignKey(
                        name: "FK_VaccinationCampaignLocations_HealthCenters_HealthCenterId",
                        column: x => x.HealthCenterId,
                        principalTable: "HealthCenters",
                        principalColumn: "HealthCenterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VaccinationCampaignLocations_VaccinationCampaigns_Vaccinatio~",
                        column: x => x.VaccinationCampaignId,
                        principalTable: "VaccinationCampaigns",
                        principalColumn: "VaccinationCampaignId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VaccineInventory",
                columns: table => new
                {
                    VaccineInventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VaccineId = table.Column<int>(type: "int", nullable: false),
                    HealthCenterId = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccineInventory", x => x.VaccineInventoryId);
                    table.ForeignKey(
                        name: "FK_VaccineInventory_HealthCenters_HealthCenterId",
                        column: x => x.HealthCenterId,
                        principalTable: "HealthCenters",
                        principalColumn: "HealthCenterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VaccineInventory_Vaccines_VaccineId",
                        column: x => x.VaccineId,
                        principalTable: "Vaccines",
                        principalColumn: "VaccineId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChildrenParents",
                columns: table => new
                {
                    ChildParentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ChildId = table.Column<int>(type: "int", nullable: false),
                    Relationship = table.Column<string>(type: "varchar(1)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildrenParents", x => x.ChildParentId);
                    table.ForeignKey(
                        name: "FK_ChildrenParents_Children_ChildId",
                        column: x => x.ChildId,
                        principalTable: "Children",
                        principalColumn: "ChildId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChildrenParents_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "ParentId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VaccinationAppointments",
                columns: table => new
                {
                    VaccinationAppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HealthCenterId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    AppointmentDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinationAppointments", x => x.VaccinationAppointmentId);
                    table.ForeignKey(
                        name: "FK_VaccinationAppointments_HealthCenters_HealthCenterId",
                        column: x => x.HealthCenterId,
                        principalTable: "HealthCenters",
                        principalColumn: "HealthCenterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VaccinationAppointments_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "ParentId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DosesDetails",
                columns: table => new
                {
                    DoseDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VaccinationSchemeDetailId = table.Column<int>(type: "int", nullable: false),
                    DoseNumber = table.Column<int>(type: "int", nullable: false),
                    PutWhenNewBorn = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PutWhenHasMonths = table.Column<int>(type: "int", nullable: false),
                    PutMonthsAfterPreviousDosis = table.Column<int>(type: "int", nullable: false),
                    PutBetweenStartMonth = table.Column<int>(type: "int", nullable: false),
                    PutBetweenEndMonth = table.Column<int>(type: "int", nullable: false),
                    PutEveryYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosesDetails", x => x.DoseDetailId);
                    table.ForeignKey(
                        name: "FK_DosesDetails_VaccinationSchemeDetails_VaccinationSchemeDetai~",
                        column: x => x.VaccinationSchemeDetailId,
                        principalTable: "VaccinationSchemeDetails",
                        principalColumn: "VaccinationSchemeDetailId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VaccinesForAppointments",
                columns: table => new
                {
                    VaccineForAppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VaccineId = table.Column<int>(type: "int", nullable: false),
                    VaccinationAppointmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinesForAppointments", x => x.VaccineForAppointmentId);
                    table.ForeignKey(
                        name: "FK_VaccinesForAppointments_VaccinationAppointments_VaccinationA~",
                        column: x => x.VaccinationAppointmentId,
                        principalTable: "VaccinationAppointments",
                        principalColumn: "VaccinationAppointmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VaccinesForAppointments_Vaccines_VaccineId",
                        column: x => x.VaccineId,
                        principalTable: "Vaccines",
                        principalColumn: "VaccineId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdministeredDoses",
                columns: table => new
                {
                    AdministeredDoseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DoseDetailId = table.Column<int>(type: "int", nullable: false),
                    ChildId = table.Column<int>(type: "int", nullable: false),
                    HealthCenterId = table.Column<int>(type: "int", nullable: false),
                    HealthPersonnelId = table.Column<int>(type: "int", nullable: false),
                    DoseDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    VaccinationCampaignId = table.Column<int>(type: "int", nullable: false),
                    VaccinationAppointmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministeredDoses", x => x.AdministeredDoseId);
                    table.ForeignKey(
                        name: "FK_AdministeredDoses_Children_ChildId",
                        column: x => x.ChildId,
                        principalTable: "Children",
                        principalColumn: "ChildId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdministeredDoses_DosesDetails_DoseDetailId",
                        column: x => x.DoseDetailId,
                        principalTable: "DosesDetails",
                        principalColumn: "DoseDetailId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdministeredDoses_HealthCenters_HealthCenterId",
                        column: x => x.HealthCenterId,
                        principalTable: "HealthCenters",
                        principalColumn: "HealthCenterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdministeredDoses_HealthPersonnel_HealthPersonnelId",
                        column: x => x.HealthPersonnelId,
                        principalTable: "HealthPersonnel",
                        principalColumn: "HealthPersonnelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdministeredDoses_VaccinationAppointments_VaccinationAppoint~",
                        column: x => x.VaccinationAppointmentId,
                        principalTable: "VaccinationAppointments",
                        principalColumn: "VaccinationAppointmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdministeredDoses_VaccinationCampaigns_VaccinationCampaignId",
                        column: x => x.VaccinationCampaignId,
                        principalTable: "VaccinationCampaigns",
                        principalColumn: "VaccinationCampaignId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reminders",
                columns: table => new
                {
                    ReminderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Via = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SendDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    VaccinationCampaignId = table.Column<int>(type: "int", nullable: false),
                    VaccinationAppointmentId = table.Column<int>(type: "int", nullable: false),
                    DoseDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminders", x => x.ReminderId);
                    table.ForeignKey(
                        name: "FK_Reminders_DosesDetails_DoseDetailId",
                        column: x => x.DoseDetailId,
                        principalTable: "DosesDetails",
                        principalColumn: "DoseDetailId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reminders_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "ParentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reminders_VaccinationAppointments_VaccinationAppointmentId",
                        column: x => x.VaccinationAppointmentId,
                        principalTable: "VaccinationAppointments",
                        principalColumn: "VaccinationAppointmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reminders_VaccinationCampaigns_VaccinationCampaignId",
                        column: x => x.VaccinationCampaignId,
                        principalTable: "VaccinationCampaigns",
                        principalColumn: "VaccinationCampaignId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AdministeredDoses_ChildId",
                table: "AdministeredDoses",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_AdministeredDoses_DoseDetailId",
                table: "AdministeredDoses",
                column: "DoseDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_AdministeredDoses_HealthCenterId",
                table: "AdministeredDoses",
                column: "HealthCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_AdministeredDoses_HealthPersonnelId",
                table: "AdministeredDoses",
                column: "HealthPersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_AdministeredDoses_VaccinationAppointmentId",
                table: "AdministeredDoses",
                column: "VaccinationAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AdministeredDoses_VaccinationCampaignId",
                table: "AdministeredDoses",
                column: "VaccinationCampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Children_DNI",
                table: "Children",
                column: "DNI",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChildrenParents_ChildId",
                table: "ChildrenParents",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildrenParents_ParentId",
                table: "ChildrenParents",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DosesDetails_VaccinationSchemeDetailId",
                table: "DosesDetails",
                column: "VaccinationSchemeDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCenters_UbigeoId",
                table: "HealthCenters",
                column: "UbigeoId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthPersonnel_UserId",
                table: "HealthPersonnel",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parents_UbigeoId",
                table: "Parents",
                column: "UbigeoId");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_UserId",
                table: "Parents",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_DoseDetailId",
                table: "Reminders",
                column: "DoseDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_ParentId",
                table: "Reminders",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_VaccinationAppointmentId",
                table: "Reminders",
                column: "VaccinationAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_VaccinationCampaignId",
                table: "Reminders",
                column: "VaccinationCampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationAppointments_HealthCenterId",
                table: "VaccinationAppointments",
                column: "HealthCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationAppointments_ParentId",
                table: "VaccinationAppointments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationCampaignDetails_VaccinationCampaignId",
                table: "VaccinationCampaignDetails",
                column: "VaccinationCampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationCampaignDetails_VaccineId",
                table: "VaccinationCampaignDetails",
                column: "VaccineId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationCampaignLocations_HealthCenterId",
                table: "VaccinationCampaignLocations",
                column: "HealthCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationCampaignLocations_VaccinationCampaignId",
                table: "VaccinationCampaignLocations",
                column: "VaccinationCampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationSchemeDetails_VaccinationSchemeId",
                table: "VaccinationSchemeDetails",
                column: "VaccinationSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationSchemeDetails_VaccineId",
                table: "VaccinationSchemeDetails",
                column: "VaccineId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineInventory_HealthCenterId",
                table: "VaccineInventory",
                column: "HealthCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineInventory_VaccineId",
                table: "VaccineInventory",
                column: "VaccineId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinesForAppointments_VaccinationAppointmentId",
                table: "VaccinesForAppointments",
                column: "VaccinationAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinesForAppointments_VaccineId",
                table: "VaccinesForAppointments",
                column: "VaccineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdministeredDoses");

            migrationBuilder.DropTable(
                name: "ChildrenParents");

            migrationBuilder.DropTable(
                name: "Reminders");

            migrationBuilder.DropTable(
                name: "VaccinationCampaignDetails");

            migrationBuilder.DropTable(
                name: "VaccinationCampaignLocations");

            migrationBuilder.DropTable(
                name: "VaccineInventory");

            migrationBuilder.DropTable(
                name: "VaccinesForAppointments");

            migrationBuilder.DropTable(
                name: "HealthPersonnel");

            migrationBuilder.DropTable(
                name: "Children");

            migrationBuilder.DropTable(
                name: "DosesDetails");

            migrationBuilder.DropTable(
                name: "VaccinationCampaigns");

            migrationBuilder.DropTable(
                name: "VaccinationAppointments");

            migrationBuilder.DropTable(
                name: "VaccinationSchemeDetails");

            migrationBuilder.DropTable(
                name: "HealthCenters");

            migrationBuilder.DropTable(
                name: "Parents");

            migrationBuilder.DropTable(
                name: "VaccinationSchemes");

            migrationBuilder.DropTable(
                name: "Ubigeo");

            migrationBuilder.DropColumn(
                name: "MaxTemperature",
                table: "Vaccines");

            migrationBuilder.DropColumn(
                name: "MinTemperature",
                table: "Vaccines");

            migrationBuilder.RenameColumn(
                name: "VaccineId",
                table: "Vaccines",
                newName: "Id");
        }
    }
}
