using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hotel.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppDayTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDayTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPaymentMethod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPaymentMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRentalPackageCate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRentalPackageCate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRentalTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRentalTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppTime",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTime", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppDateTypeWeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekDay = table.Column<int>(type: "int", nullable: false),
                    IdDayType = table.Column<int>(type: "int", nullable: false),
                    IdHotel = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDateTypeWeeks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppDateTypeWeeks_AppDayTypes_IdDayType",
                        column: x => x.IdDayType,
                        principalTable: "AppDayTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppHolidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: true),
                    IdDayType = table.Column<int>(type: "int", nullable: false),
                    IdHotel = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppHolidays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppHolidays_AppDayTypes_IdDayType",
                        column: x => x.IdDayType,
                        principalTable: "AppDayTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppHMS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    IdGroup = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppHMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppHMS_AppGroup_IdGroup",
                        column: x => x.IdGroup,
                        principalTable: "AppGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppHotel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    City = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    District = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    IdGroup = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppHotel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppHotel_AppGroup_IdGroup",
                        column: x => x.IdGroup,
                        principalTable: "AppGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppPermissions",
                columns: table => new
                {
                    PerCode = table.Column<int>(type: "int", nullable: false),
                    IdGroup = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Table = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPermissions", x => new { x.PerCode, x.IdGroup });
                    table.ForeignKey(
                        name: "FK_AppPermissions_AppGroup_IdGroup",
                        column: x => x.IdGroup,
                        principalTable: "AppGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    IdGroup = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRole_AppGroup_IdGroup",
                        column: x => x.IdGroup,
                        principalTable: "AppGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppRentalPackage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    IdTimeOfPrice = table.Column<int>(type: "int", nullable: false),
                    TrialTime = table.Column<int>(type: "int", nullable: true),
                    IdTimeOfTrial = table.Column<int>(type: "int", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    AccountLimit = table.Column<int>(type: "int", nullable: false),
                    RoomLimit = table.Column<int>(type: "int", nullable: false),
                    UsageTraining = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VisualReport = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    IdRentalPackageCate = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRentalPackage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRentalPackage_AppRentalPackageCate_IdRentalPackageCate",
                        column: x => x.IdRentalPackageCate,
                        principalTable: "AppRentalPackageCate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppAmenities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    IdHotel = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAmenities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppAmenities_AppHotel_IdHotel",
                        column: x => x.IdHotel,
                        principalTable: "AppHotel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppCustHotels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    BirthDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    IdHotel = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCustHotels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCustHotels_AppHotel_IdHotel",
                        column: x => x.IdHotel,
                        principalTable: "AppHotel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "appFloors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FloorNumber = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    IdHotel = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appFloors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_appFloors_AppHotel_IdHotel",
                        column: x => x.IdHotel,
                        principalTable: "AppHotel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppRoomCates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EarlyCheckInFee = table.Column<double>(type: "float", nullable: false),
                    LateCheckOutFee = table.Column<double>(type: "float", nullable: false),
                    StanderAdult = table.Column<int>(type: "int", nullable: false),
                    StanderChildren = table.Column<int>(type: "int", nullable: false),
                    MaxAdult = table.Column<int>(type: "int", nullable: false),
                    MaxChildren = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IdHotel = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoomCates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRoomCates_AppHotel_IdHotel",
                        column: x => x.IdHotel,
                        principalTable: "AppHotel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppSvcCommoCates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdHotel = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSvcCommoCates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSvcCommoCates_AppHotel_IdHotel",
                        column: x => x.IdHotel,
                        principalTable: "AppHotel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppRolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRole = table.Column<int>(type: "int", nullable: false),
                    IdPermission = table.Column<int>(type: "int", nullable: false),
                    IdGroup = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRolePermissions_AppPermissions_IdPermission_IdGroup",
                        columns: x => new { x.IdPermission, x.IdGroup },
                        principalTable: "AppPermissions",
                        principalColumns: new[] { "PerCode", "IdGroup" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppRolePermissions_AppRole_IdRole",
                        column: x => x.IdRole,
                        principalTable: "AppRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    IdGroup = table.Column<int>(type: "int", nullable: false),
                    IdRole = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUser_AppGroup_IdGroup",
                        column: x => x.IdGroup,
                        principalTable: "AppGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUser_AppRole_IdRole",
                        column: x => x.IdRole,
                        principalTable: "AppRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppHMSOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalTime = table.Column<double>(type: "float", nullable: false),
                    IdTime = table.Column<int>(type: "int", nullable: false),
                    TotalFee = table.Column<double>(type: "float", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IdPaymentMethod = table.Column<int>(type: "int", nullable: false),
                    IdRentalPackage = table.Column<int>(type: "int", nullable: false),
                    IdHotel = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppHMSOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppHMSOrder_AppHotel_IdHotel",
                        column: x => x.IdHotel,
                        principalTable: "AppHotel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppHMSOrder_AppRentalPackage_IdRentalPackage",
                        column: x => x.IdRentalPackage,
                        principalTable: "AppRentalPackage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppRentalPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRoomCate = table.Column<int>(type: "int", nullable: false),
                    IdRentalType = table.Column<int>(type: "int", nullable: false),
                    IdDayType = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRentalPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRentalPrices_AppDayTypes_IdDayType",
                        column: x => x.IdDayType,
                        principalTable: "AppDayTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppRentalPrices_AppRentalTypes_IdRentalType",
                        column: x => x.IdRentalType,
                        principalTable: "AppRentalTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppRentalPrices_AppRoomCates_IdRoomCate",
                        column: x => x.IdRoomCate,
                        principalTable: "AppRoomCates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppRoomCateAmenities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAmenity = table.Column<int>(type: "int", nullable: false),
                    IdRoomCate = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoomCateAmenities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRoomCateAmenities_AppAmenities_IdAmenity",
                        column: x => x.IdAmenity,
                        principalTable: "AppAmenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppRoomCateAmenities_AppRoomCates_IdRoomCate",
                        column: x => x.IdRoomCate,
                        principalTable: "AppRoomCates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    UseStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CleanStatus = table.Column<int>(type: "int", nullable: false),
                    IdRoomCate = table.Column<int>(type: "int", nullable: false),
                    IdFloor = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRooms_AppRoomCates_IdRoomCate",
                        column: x => x.IdRoomCate,
                        principalTable: "AppRoomCates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppRooms_appFloors_IdFloor",
                        column: x => x.IdFloor,
                        principalTable: "appFloors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppCommodities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IdSvcCommoCate = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCommodities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCommodities_AppSvcCommoCates_IdSvcCommoCate",
                        column: x => x.IdSvcCommoCate,
                        principalTable: "AppSvcCommoCates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IdSvcCommocate = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppServices_AppSvcCommoCates_IdSvcCommocate",
                        column: x => x.IdSvcCommocate,
                        principalTable: "AppSvcCommoCates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppBookingRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CheckInExpectual = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckInActual = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutExpectual = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutActual = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Adult = table.Column<int>(type: "int", nullable: false),
                    Children = table.Column<int>(type: "int", nullable: false),
                    BookingConfirm = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CheckinConfirm = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CheckoutConfirm = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    IdRentalPrice = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    IdCustommer = table.Column<int>(type: "int", nullable: false),
                    IdRoom = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppBookingRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppBookingRooms_AppCustHotels_IdCustommer",
                        column: x => x.IdCustommer,
                        principalTable: "AppCustHotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppBookingRooms_AppRentalPrices_IdRentalPrice",
                        column: x => x.IdRentalPrice,
                        principalTable: "AppRentalPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppBookingRooms_AppRooms_IdRoom",
                        column: x => x.IdRoom,
                        principalTable: "AppRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppBookingRooms_AppUser_IdUser",
                        column: x => x.IdUser,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRoomCate = table.Column<int>(type: "int", nullable: true),
                    IdServices = table.Column<int>(type: "int", nullable: true),
                    IdCommodity = table.Column<int>(type: "int", nullable: true),
                    IdCustHotel = table.Column<int>(type: "int", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppImages_AppCommodities_IdCommodity",
                        column: x => x.IdCommodity,
                        principalTable: "AppCommodities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppImages_AppCustHotels_IdCustHotel",
                        column: x => x.IdCustHotel,
                        principalTable: "AppCustHotels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppImages_AppRoomCates_IdRoomCate",
                        column: x => x.IdRoomCate,
                        principalTable: "AppRoomCates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppImages_AppServices_IdServices",
                        column: x => x.IdServices,
                        principalTable: "AppServices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppBill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    RoomPrice = table.Column<double>(type: "float", nullable: false),
                    DiscountPrice = table.Column<double>(type: "float", nullable: false),
                    FinalPrice = table.Column<double>(type: "float", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IdBooking = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppBill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppBill_AppBookingRooms_IdBooking",
                        column: x => x.IdBooking,
                        principalTable: "AppBookingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppComodityOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    IdCommodities = table.Column<int>(type: "int", nullable: false),
                    IdBookingRoom = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppComodityOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppComodityOrders_AppBookingRooms_IdBookingRoom",
                        column: x => x.IdBookingRoom,
                        principalTable: "AppBookingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppComodityOrders_AppCommodities_IdCommodities",
                        column: x => x.IdCommodities,
                        principalTable: "AppCommodities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppIncurredFee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    IdBookingRoom = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppIncurredFee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppIncurredFee_AppBookingRooms_IdBookingRoom",
                        column: x => x.IdBookingRoom,
                        principalTable: "AppBookingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppServicesOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    IdServices = table.Column<int>(type: "int", nullable: false),
                    IdBookingRoom = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppServicesOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppServicesOrders_AppBookingRooms_IdBookingRoom",
                        column: x => x.IdBookingRoom,
                        principalTable: "AppBookingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppServicesOrders_AppServices_IdServices",
                        column: x => x.IdServices,
                        principalTable: "AppServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppDayTypes",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedDate", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ngày thường", null, null },
                    { 2, null, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ngày cuối tuần", null, null },
                    { 3, null, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ngày lễ", null, null }
                });

            migrationBuilder.InsertData(
                table: "AppGroup",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedDate", "Desc", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[] { 1, null, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Nhóm Quản trị hệ thống Hotel manager", "Hotel Manager", null, null });

            migrationBuilder.InsertData(
                table: "AppPaymentMethod",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedDate", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "MoMo", null, null },
                    { 2, null, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "PayPal", null, null },
                    { 3, null, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "VNPay", null, null }
                });

            migrationBuilder.InsertData(
                table: "AppRentalPackageCate",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedDate", "Name", "Status", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, null, null, "Phiên bản dùng thử", 0, null, null },
                    { 2, null, null, null, "Phiên bản có phí", 0, null, null }
                });

            migrationBuilder.InsertData(
                table: "AppRentalTypes",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedDate", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Thuê theo giờ", null, null },
                    { 2, null, new DateTime(2024, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Thuê qua đêm", null, null },
                    { 3, null, new DateTime(2024, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Thuê nguyên ngày", null, null }
                });

            migrationBuilder.InsertData(
                table: "AppTime",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedDate", "Name", "Time", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Giờ", 0, null, null },
                    { 2, null, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ngày", 1, null, null },
                    { 3, null, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Tháng", 30, null, null },
                    { 4, null, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Năm", 365, null, null }
                });

            migrationBuilder.InsertData(
                table: "AppRole",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedDate", "Desc", "IdGroup", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[] { 1, null, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quản trị toàn bộ hệ thống", 1, "Quản trị hệ thống", null, null });

            migrationBuilder.InsertData(
                table: "AppUser",
                columns: new[] { "Id", "Avatar", "CreatedBy", "CreatedDate", "DeletedDate", "Email", "IdGroup", "IdRole", "Name", "Password", "Phone", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "/images/manager/avatar_default.png", null, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "quocduyctvn@gmail.com", 1, 1, "admin1", "$2a$10$Pnw.PoTtbbse7XRyHADTdOfYEtYRTqqbaIAM3H6qt6q601vOXrGty", "0901007221", null, null },
                    { 2, "/images/manager/avatar_default.png", null, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "danhkieuhan135@gmail.com", 1, 1, "admin2", "$2a$10$Pnw.PoTtbbse7XRyHADTdOfYEtYRTqqbaIAM3H6qt6q601vOXrGty", "0945255664", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppAmenities_IdHotel",
                table: "AppAmenities",
                column: "IdHotel");

            migrationBuilder.CreateIndex(
                name: "IX_AppBill_IdBooking",
                table: "AppBill",
                column: "IdBooking",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppBookingRooms_IdCustommer",
                table: "AppBookingRooms",
                column: "IdCustommer");

            migrationBuilder.CreateIndex(
                name: "IX_AppBookingRooms_IdRentalPrice",
                table: "AppBookingRooms",
                column: "IdRentalPrice");

            migrationBuilder.CreateIndex(
                name: "IX_AppBookingRooms_IdRoom",
                table: "AppBookingRooms",
                column: "IdRoom");

            migrationBuilder.CreateIndex(
                name: "IX_AppBookingRooms_IdUser",
                table: "AppBookingRooms",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_AppCommodities_IdSvcCommoCate",
                table: "AppCommodities",
                column: "IdSvcCommoCate");

            migrationBuilder.CreateIndex(
                name: "IX_AppComodityOrders_IdBookingRoom",
                table: "AppComodityOrders",
                column: "IdBookingRoom");

            migrationBuilder.CreateIndex(
                name: "IX_AppComodityOrders_IdCommodities",
                table: "AppComodityOrders",
                column: "IdCommodities");

            migrationBuilder.CreateIndex(
                name: "IX_AppCustHotels_IdHotel",
                table: "AppCustHotels",
                column: "IdHotel");

            migrationBuilder.CreateIndex(
                name: "IX_AppDateTypeWeeks_IdDayType",
                table: "AppDateTypeWeeks",
                column: "IdDayType");

            migrationBuilder.CreateIndex(
                name: "IX_appFloors_IdHotel",
                table: "appFloors",
                column: "IdHotel");

            migrationBuilder.CreateIndex(
                name: "IX_AppHMS_IdGroup",
                table: "AppHMS",
                column: "IdGroup",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppHMSOrder_IdHotel",
                table: "AppHMSOrder",
                column: "IdHotel",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppHMSOrder_IdRentalPackage",
                table: "AppHMSOrder",
                column: "IdRentalPackage");

            migrationBuilder.CreateIndex(
                name: "IX_AppHolidays_IdDayType",
                table: "AppHolidays",
                column: "IdDayType");

            migrationBuilder.CreateIndex(
                name: "IX_AppHotel_IdGroup",
                table: "AppHotel",
                column: "IdGroup",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppImages_IdCommodity",
                table: "AppImages",
                column: "IdCommodity");

            migrationBuilder.CreateIndex(
                name: "IX_AppImages_IdCustHotel",
                table: "AppImages",
                column: "IdCustHotel");

            migrationBuilder.CreateIndex(
                name: "IX_AppImages_IdRoomCate",
                table: "AppImages",
                column: "IdRoomCate");

            migrationBuilder.CreateIndex(
                name: "IX_AppImages_IdServices",
                table: "AppImages",
                column: "IdServices");

            migrationBuilder.CreateIndex(
                name: "IX_AppIncurredFee_IdBookingRoom",
                table: "AppIncurredFee",
                column: "IdBookingRoom");

            migrationBuilder.CreateIndex(
                name: "IX_AppPermissions_IdGroup",
                table: "AppPermissions",
                column: "IdGroup");

            migrationBuilder.CreateIndex(
                name: "IX_AppRentalPackage_IdRentalPackageCate",
                table: "AppRentalPackage",
                column: "IdRentalPackageCate");

            migrationBuilder.CreateIndex(
                name: "IX_AppRentalPrices_IdDayType",
                table: "AppRentalPrices",
                column: "IdDayType");

            migrationBuilder.CreateIndex(
                name: "IX_AppRentalPrices_IdRentalType",
                table: "AppRentalPrices",
                column: "IdRentalType");

            migrationBuilder.CreateIndex(
                name: "IX_AppRentalPrices_IdRoomCate",
                table: "AppRentalPrices",
                column: "IdRoomCate");

            migrationBuilder.CreateIndex(
                name: "IX_AppRole_IdGroup",
                table: "AppRole",
                column: "IdGroup");

            migrationBuilder.CreateIndex(
                name: "IX_AppRolePermissions_IdPermission_IdGroup",
                table: "AppRolePermissions",
                columns: new[] { "IdPermission", "IdGroup" });

            migrationBuilder.CreateIndex(
                name: "IX_AppRolePermissions_IdRole",
                table: "AppRolePermissions",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoomCateAmenities_IdAmenity",
                table: "AppRoomCateAmenities",
                column: "IdAmenity");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoomCateAmenities_IdRoomCate",
                table: "AppRoomCateAmenities",
                column: "IdRoomCate");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoomCates_IdHotel",
                table: "AppRoomCates",
                column: "IdHotel");

            migrationBuilder.CreateIndex(
                name: "IX_AppRooms_IdFloor",
                table: "AppRooms",
                column: "IdFloor");

            migrationBuilder.CreateIndex(
                name: "IX_AppRooms_IdRoomCate",
                table: "AppRooms",
                column: "IdRoomCate");

            migrationBuilder.CreateIndex(
                name: "IX_AppServices_IdSvcCommocate",
                table: "AppServices",
                column: "IdSvcCommocate");

            migrationBuilder.CreateIndex(
                name: "IX_AppServicesOrders_IdBookingRoom",
                table: "AppServicesOrders",
                column: "IdBookingRoom");

            migrationBuilder.CreateIndex(
                name: "IX_AppServicesOrders_IdServices",
                table: "AppServicesOrders",
                column: "IdServices");

            migrationBuilder.CreateIndex(
                name: "IX_AppSvcCommoCates_IdHotel",
                table: "AppSvcCommoCates",
                column: "IdHotel");

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_IdGroup",
                table: "AppUser",
                column: "IdGroup");

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_IdRole",
                table: "AppUser",
                column: "IdRole");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppBill");

            migrationBuilder.DropTable(
                name: "AppComodityOrders");

            migrationBuilder.DropTable(
                name: "AppDateTypeWeeks");

            migrationBuilder.DropTable(
                name: "AppHMS");

            migrationBuilder.DropTable(
                name: "AppHMSOrder");

            migrationBuilder.DropTable(
                name: "AppHolidays");

            migrationBuilder.DropTable(
                name: "AppImages");

            migrationBuilder.DropTable(
                name: "AppIncurredFee");

            migrationBuilder.DropTable(
                name: "AppPaymentMethod");

            migrationBuilder.DropTable(
                name: "AppRolePermissions");

            migrationBuilder.DropTable(
                name: "AppRoomCateAmenities");

            migrationBuilder.DropTable(
                name: "AppServicesOrders");

            migrationBuilder.DropTable(
                name: "AppTime");

            migrationBuilder.DropTable(
                name: "AppRentalPackage");

            migrationBuilder.DropTable(
                name: "AppCommodities");

            migrationBuilder.DropTable(
                name: "AppPermissions");

            migrationBuilder.DropTable(
                name: "AppAmenities");

            migrationBuilder.DropTable(
                name: "AppBookingRooms");

            migrationBuilder.DropTable(
                name: "AppServices");

            migrationBuilder.DropTable(
                name: "AppRentalPackageCate");

            migrationBuilder.DropTable(
                name: "AppCustHotels");

            migrationBuilder.DropTable(
                name: "AppRentalPrices");

            migrationBuilder.DropTable(
                name: "AppRooms");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "AppSvcCommoCates");

            migrationBuilder.DropTable(
                name: "AppDayTypes");

            migrationBuilder.DropTable(
                name: "AppRentalTypes");

            migrationBuilder.DropTable(
                name: "AppRoomCates");

            migrationBuilder.DropTable(
                name: "appFloors");

            migrationBuilder.DropTable(
                name: "AppRole");

            migrationBuilder.DropTable(
                name: "AppHotel");

            migrationBuilder.DropTable(
                name: "AppGroup");
        }
    }
}
