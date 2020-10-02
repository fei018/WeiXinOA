using Microsoft.EntityFrameworkCore.Migrations;

namespace WeiXinOA.Migrations
{
    public partial class CreateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tbl_Elder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 5, nullable: false),
                    Sex = table.Column<string>(nullable: false),
                    Age = table.Column<string>(maxLength: 2, nullable: false),
                    BirthDate = table.Column<string>(maxLength: 11, nullable: false),
                    IdNumber = table.Column<string>(maxLength: 18, nullable: false),
                    Marital = table.Column<string>(maxLength: 5, nullable: false),
                    Education = table.Column<string>(maxLength: 5, nullable: false),
                    Phone = table.Column<string>(maxLength: 15, nullable: false),
                    MinZu = table.Column<string>(maxLength: 5, nullable: false),
                    JiGuan = table.Column<string>(maxLength: 10, nullable: false),
                    HuJiAddress = table.Column<string>(maxLength: 40, nullable: false),
                    HomeAddress = table.Column<string>(maxLength: 40, nullable: false),
                    WorkUnit = table.Column<string>(maxLength: 10, nullable: true),
                    InCome = table.Column<string>(maxLength: 6, nullable: false),
                    RetirementDate = table.Column<string>(maxLength: 11, nullable: true),
                    HealthState = table.Column<string>(maxLength: 10, nullable: false),
                    MentalState = table.Column<string>(maxLength: 10, nullable: false),
                    ChangBeiYaoWu = table.Column<string>(maxLength: 20, nullable: true),
                    AiHao = table.Column<string>(maxLength: 10, nullable: true),
                    DietaryHabit = table.Column<string>(maxLength: 10, nullable: false),
                    Faith = table.Column<string>(maxLength: 5, nullable: false),
                    FaithTime = table.Column<string>(maxLength: 4, nullable: false),
                    XiuXingFaMen = table.Column<string>(maxLength: 10, nullable: true),
                    HomeWork = table.Column<string>(maxLength: 10, nullable: true),
                    Remark = table.Column<string>(maxLength: 50, nullable: true),
                    AddTime = table.Column<string>(nullable: true),
                    IdPhotoPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Elder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_ElderFamily",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 5, nullable: false),
                    Sex = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(maxLength: 15, nullable: false),
                    Relationship = table.Column<string>(maxLength: 5, nullable: false),
                    Age = table.Column<string>(maxLength: 2, nullable: false),
                    Profession = table.Column<string>(maxLength: 6, nullable: false),
                    Education = table.Column<string>(maxLength: 5, nullable: false),
                    IdNumber = table.Column<string>(maxLength: 18, nullable: false),
                    HomeAddress = table.Column<string>(maxLength: 40, nullable: false),
                    ElderIdNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_ElderFamily", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_LoginUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginName = table.Column<string>(maxLength: 10, nullable: false),
                    Password = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_LoginUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Volunteer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 5, nullable: false),
                    Sex = table.Column<string>(nullable: false),
                    Age = table.Column<string>(maxLength: 2, nullable: false),
                    Phone = table.Column<string>(maxLength: 15, nullable: false),
                    BirthDate = table.Column<string>(maxLength: 11, nullable: false),
                    IdNumber = table.Column<string>(maxLength: 18, nullable: false),
                    MinZu = table.Column<string>(maxLength: 5, nullable: false),
                    Marital = table.Column<string>(maxLength: 5, nullable: false),
                    PoliticalStatus = table.Column<string>(maxLength: 5, nullable: false),
                    JiGuan = table.Column<string>(maxLength: 10, nullable: false),
                    Education = table.Column<string>(maxLength: 3, nullable: false),
                    ProfessinalAbility = table.Column<string>(maxLength: 10, nullable: true),
                    GraduateInstitutions = table.Column<string>(maxLength: 10, nullable: true),
                    HealthStatus = table.Column<string>(maxLength: 5, nullable: false),
                    WorkUnit = table.Column<string>(maxLength: 10, nullable: true),
                    ProfessionStatus = table.Column<string>(maxLength: 5, nullable: false),
                    HuJiAddress = table.Column<string>(maxLength: 40, nullable: false),
                    HomeAddress = table.Column<string>(maxLength: 40, nullable: false),
                    WorkHistory = table.Column<string>(maxLength: 50, nullable: false),
                    ServiceTerm = table.Column<string>(nullable: false),
                    AddTime = table.Column<string>(nullable: true),
                    IdPhotoPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Volunteer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_VolunteerFamily",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 5, nullable: false),
                    Sex = table.Column<string>(nullable: false),
                    Age = table.Column<string>(maxLength: 2, nullable: false),
                    Relationship = table.Column<string>(maxLength: 5, nullable: false),
                    Phone = table.Column<string>(maxLength: 15, nullable: false),
                    HomeAddress = table.Column<string>(maxLength: 40, nullable: false),
                    VolunteerIdNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_VolunteerFamily", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tbl_Elder");

            migrationBuilder.DropTable(
                name: "Tbl_ElderFamily");

            migrationBuilder.DropTable(
                name: "Tbl_LoginUser");

            migrationBuilder.DropTable(
                name: "Tbl_Volunteer");

            migrationBuilder.DropTable(
                name: "Tbl_VolunteerFamily");
        }
    }
}
