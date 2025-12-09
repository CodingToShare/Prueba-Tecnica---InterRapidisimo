using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentRegistration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditPrograms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TotalCreditsRequired = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditPrograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Professors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmployeeCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Credits = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Student"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassOfferings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    ProfessorId = table.Column<int>(type: "int", nullable: false),
                    OfferingCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AcademicPeriod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Schedule = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaxCapacity = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassOfferings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassOfferings_Professors_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "Professors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassOfferings_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StudentNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreditProgramId = table.Column<int>(type: "int", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_CreditPrograms_CreditProgramId",
                        column: x => x.CreditProgramId,
                        principalTable: "CreditPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ClassOfferingId = table.Column<int>(type: "int", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active"),
                    FinalGrade = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollments_ClassOfferings_ClassOfferingId",
                        column: x => x.ClassOfferingId,
                        principalTable: "ClassOfferings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CreditPrograms",
                columns: new[] { "Id", "Code", "CreatedAt", "Description", "IsActive", "Name", "TotalCreditsRequired", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "PROG-STD", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Programa académico estándar con requisitos regulares de créditos", true, "Programa Estándar", 120, null },
                    { 2, "PROG-INT", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Programa intensivo con mayor carga académica y menor duración", true, "Programa Intensivo", 150, null },
                    { 3, "PROG-FLEX", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Programa flexible que permite al estudiante avanzar a su propio ritmo", true, "Programa Flexible", 100, null }
                });

            migrationBuilder.InsertData(
                table: "Professors",
                columns: new[] { "Id", "CreatedAt", "Department", "Email", "EmployeeCode", "FullName", "IsActive", "Specialization", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Matemáticas y Ciencias Básicas", "cmartinez@universidad.edu", "PROF001", "Dr. Carlos Alberto Martínez Ruiz", true, "Álgebra y Cálculo Avanzado", null },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ingeniería de Software", "agarcia@universidad.edu", "PROF002", "Dra. Ana María García Fernández", true, "Programación Orientada a Objetos y Desarrollo Web", null },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sistemas de Información", "lrodriguez@universidad.edu", "PROF003", "Dr. Luis Fernando Rodríguez López", true, "Bases de Datos y Sistemas Distribuidos", null },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Infraestructura y Redes", "mlopez@universidad.edu", "PROF004", "Dra. María del Carmen López Sánchez", true, "Redes de Computadoras y Seguridad Informática", null },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Inteligencia Artificial y Tecnologías Emergentes", "rsanchez@universidad.edu", "PROF005", "Dr. Roberto José Sánchez Torres", true, "Machine Learning y Cloud Computing", null }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Code", "CreatedAt", "Credits", "Description", "IsActive", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "MAT101", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Fundamentos de álgebra, cálculo y matemáticas discretas", true, "Matemáticas Fundamentales", null },
                    { 2, "PROG101", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Introducción a la programación orientada a objetos con C#", true, "Programación I", null },
                    { 3, "DB101", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Diseño, modelado e implementación de bases de datos relacionales", true, "Bases de Datos", null },
                    { 4, "WEB101", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Desarrollo de aplicaciones web con HTML, CSS, JavaScript y frameworks modernos", true, "Desarrollo Web", null },
                    { 5, "ALG101", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Análisis y diseño de algoritmos eficientes", true, "Algoritmos y Estructuras de Datos", null },
                    { 6, "NET101", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Fundamentos de redes, protocolos TCP/IP y arquitecturas de red", true, "Redes de Computadoras", null },
                    { 7, "SEC101", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Principios de seguridad, criptografía y protección de sistemas", true, "Seguridad Informática", null },
                    { 8, "AI101", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Fundamentos de IA, machine learning y deep learning", true, "Inteligencia Artificial", null },
                    { 9, "MOB101", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Desarrollo de aplicaciones móviles multiplataforma", true, "Desarrollo Móvil", null },
                    { 10, "CLOUD101", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Computación en la nube con Azure, AWS y arquitecturas distribuidas", true, "Cloud Computing", null }
                });

            migrationBuilder.InsertData(
                table: "ClassOfferings",
                columns: new[] { "Id", "AcademicPeriod", "CreatedAt", "IsActive", "MaxCapacity", "OfferingCode", "ProfessorId", "Schedule", "SubjectId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "2025-1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 30, "MAT101-PROF001-2025-1", 1, "Lunes y Miércoles 8:00-10:00 AM", 1, null },
                    { 2, "2025-1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 30, "ALG101-PROF001-2025-1", 1, "Martes y Jueves 8:00-10:00 AM", 5, null },
                    { 3, "2025-1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 35, "PROG101-PROF002-2025-1", 2, "Lunes y Miércoles 10:00 AM-12:00 PM", 2, null },
                    { 4, "2025-1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 35, "WEB101-PROF002-2025-1", 2, "Martes y Jueves 10:00 AM-12:00 PM", 4, null },
                    { 5, "2025-1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 30, "DB101-PROF003-2025-1", 3, "Lunes y Miércoles 2:00-4:00 PM", 3, null },
                    { 6, "2025-1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 25, "CLOUD101-PROF003-2025-1", 3, "Martes y Jueves 2:00-4:00 PM", 10, null },
                    { 7, "2025-1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 30, "NET101-PROF004-2025-1", 4, "Lunes y Miércoles 4:00-6:00 PM", 6, null },
                    { 8, "2025-1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 25, "SEC101-PROF004-2025-1", 4, "Martes y Jueves 4:00-6:00 PM", 7, null },
                    { 9, "2025-1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 25, "AI101-PROF005-2025-1", 5, "Lunes y Miércoles 6:00-8:00 PM", 8, null },
                    { 10, "2025-1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 30, "MOB101-PROF005-2025-1", 5, "Martes y Jueves 6:00-8:00 PM", 9, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassOfferings_ProfessorId",
                table: "ClassOfferings",
                column: "ProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassOfferings_SubjectId_ProfessorId_AcademicPeriod",
                table: "ClassOfferings",
                columns: new[] { "SubjectId", "ProfessorId", "AcademicPeriod" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreditPrograms_Code",
                table: "CreditPrograms",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_ClassOfferingId",
                table: "Enrollments",
                column: "ClassOfferingId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId_ClassOfferingId",
                table: "Enrollments",
                columns: new[] { "StudentId", "ClassOfferingId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professors_EmployeeCode",
                table: "Professors",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_CreditProgramId",
                table: "Students",
                column: "CreditProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentNumber",
                table: "Students",
                column: "StudentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_Code",
                table: "Subjects",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "ClassOfferings");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Professors");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "CreditPrograms");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
