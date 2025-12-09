using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Infrastructure.Data;

/// <summary>
/// Clase estática que contiene los datos de seeding inicial para la base de datos.
/// Proporciona datos para: CreditPrograms, Subjects, Professors y ClassOfferings.
/// </summary>
public static class DataSeeder
{
    /// <summary>
    /// Periodo académico actual usado en el seeding.
    /// </summary>
    public const string CurrentAcademicPeriod = "2025-1";

    /// <summary>
    /// Fecha estática para el seeding de datos.
    /// IMPORTANTE: Usar fecha estática en lugar de DateTime.UtcNow para evitar cambios en el modelo.
    /// </summary>
    private static readonly DateTime SeedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    // ============================================
    // PROGRAMAS DE CRÉDITOS
    // ============================================

    /// <summary>
    /// Obtiene los programas de créditos iniciales.
    /// </summary>
    public static List<CreditProgram> GetCreditPrograms()
    {
        return new List<CreditProgram>
        {
            new CreditProgram
            {
                Id = 1,
                Code = "PROG-STD",
                Name = "Programa Estándar",
                Description = "Programa académico estándar con requisitos regulares de créditos",
                TotalCreditsRequired = 120,
                CreatedAt = SeedDate,
                IsActive = true
            },
            new CreditProgram
            {
                Id = 2,
                Code = "PROG-INT",
                Name = "Programa Intensivo",
                Description = "Programa intensivo con mayor carga académica y menor duración",
                TotalCreditsRequired = 150,
                CreatedAt = SeedDate,
                IsActive = true
            },
            new CreditProgram
            {
                Id = 3,
                Code = "PROG-FLEX",
                Name = "Programa Flexible",
                Description = "Programa flexible que permite al estudiante avanzar a su propio ritmo",
                TotalCreditsRequired = 100,
                CreatedAt = SeedDate,
                IsActive = true
            }
        };
    }

    // ============================================
    // MATERIAS (SUBJECTS)
    // REGLA DE NEGOCIO: Exactamente 10 materias, todas valen 3 créditos
    // ============================================

    /// <summary>
    /// Obtiene las 10 materias del catálogo académico.
    /// REGLA DE NEGOCIO: Todas las materias valen 3 créditos.
    /// </summary>
    public static List<Subject> GetSubjects()
    {
        return new List<Subject>
        {
            new Subject
            {
                Id = 1,
                Code = "MAT101",
                Name = "Matemáticas Fundamentales",
                Description = "Fundamentos de álgebra, cálculo y matemáticas discretas",
                Credits = 3,
                CreatedAt = SeedDate,
                IsActive = true
            },
            new Subject
            {
                Id = 2,
                Code = "PROG101",
                Name = "Programación I",
                Description = "Introducción a la programación orientada a objetos con C#",
                Credits = 3,
                CreatedAt = SeedDate,
                IsActive = true
            },
            new Subject
            {
                Id = 3,
                Code = "DB101",
                Name = "Bases de Datos",
                Description = "Diseño, modelado e implementación de bases de datos relacionales",
                Credits = 3,
                CreatedAt = SeedDate,
                IsActive = true
            },
            new Subject
            {
                Id = 4,
                Code = "WEB101",
                Name = "Desarrollo Web",
                Description = "Desarrollo de aplicaciones web con HTML, CSS, JavaScript y frameworks modernos",
                Credits = 3,
                CreatedAt = SeedDate,
                IsActive = true
            },
            new Subject
            {
                Id = 5,
                Code = "ALG101",
                Name = "Algoritmos y Estructuras de Datos",
                Description = "Análisis y diseño de algoritmos eficientes",
                Credits = 3,
                CreatedAt = SeedDate,
                IsActive = true
            },
            new Subject
            {
                Id = 6,
                Code = "NET101",
                Name = "Redes de Computadoras",
                Description = "Fundamentos de redes, protocolos TCP/IP y arquitecturas de red",
                Credits = 3,
                CreatedAt = SeedDate,
                IsActive = true
            },
            new Subject
            {
                Id = 7,
                Code = "SEC101",
                Name = "Seguridad Informática",
                Description = "Principios de seguridad, criptografía y protección de sistemas",
                Credits = 3,
                CreatedAt = SeedDate,
                IsActive = true
            },
            new Subject
            {
                Id = 8,
                Code = "AI101",
                Name = "Inteligencia Artificial",
                Description = "Fundamentos de IA, machine learning y deep learning",
                Credits = 3,
                CreatedAt = SeedDate,
                IsActive = true
            },
            new Subject
            {
                Id = 9,
                Code = "MOB101",
                Name = "Desarrollo Móvil",
                Description = "Desarrollo de aplicaciones móviles multiplataforma",
                Credits = 3,
                CreatedAt = SeedDate,
                IsActive = true
            },
            new Subject
            {
                Id = 10,
                Code = "CLOUD101",
                Name = "Cloud Computing",
                Description = "Computación en la nube con Azure, AWS y arquitecturas distribuidas",
                Credits = 3,
                CreatedAt = SeedDate,
                IsActive = true
            }
        };
    }

    // ============================================
    // PROFESORES (PROFESSORS)
    // REGLA DE NEGOCIO: Exactamente 5 profesores
    // ============================================

    /// <summary>
    /// Obtiene los 5 profesores del catálogo académico.
    /// REGLA DE NEGOCIO: Cada profesor dicta exactamente 2 materias.
    /// </summary>
    public static List<Professor> GetProfessors()
    {
        return new List<Professor>
        {
            new Professor
            {
                Id = 1,
                EmployeeCode = "PROF001",
                FullName = "Dr. Carlos Alberto Martínez Ruiz",
                Email = "cmartinez@universidad.edu",
                Department = "Matemáticas y Ciencias Básicas",
                Specialization = "Álgebra y Cálculo Avanzado",
                CreatedAt = SeedDate,
                IsActive = true
            },
            new Professor
            {
                Id = 2,
                EmployeeCode = "PROF002",
                FullName = "Dra. Ana María García Fernández",
                Email = "agarcia@universidad.edu",
                Department = "Ingeniería de Software",
                Specialization = "Programación Orientada a Objetos y Desarrollo Web",
                CreatedAt = SeedDate,
                IsActive = true
            },
            new Professor
            {
                Id = 3,
                EmployeeCode = "PROF003",
                FullName = "Dr. Luis Fernando Rodríguez López",
                Email = "lrodriguez@universidad.edu",
                Department = "Sistemas de Información",
                Specialization = "Bases de Datos y Sistemas Distribuidos",
                CreatedAt = SeedDate,
                IsActive = true
            },
            new Professor
            {
                Id = 4,
                EmployeeCode = "PROF004",
                FullName = "Dra. María del Carmen López Sánchez",
                Email = "mlopez@universidad.edu",
                Department = "Infraestructura y Redes",
                Specialization = "Redes de Computadoras y Seguridad Informática",
                CreatedAt = SeedDate,
                IsActive = true
            },
            new Professor
            {
                Id = 5,
                EmployeeCode = "PROF005",
                FullName = "Dr. Roberto José Sánchez Torres",
                Email = "rsanchez@universidad.edu",
                Department = "Inteligencia Artificial y Tecnologías Emergentes",
                Specialization = "Machine Learning y Cloud Computing",
                CreatedAt = SeedDate,
                IsActive = true
            }
        };
    }

    // ============================================
    // OFERTAS DE CLASE (CLASS OFFERINGS)
    // REGLA DE NEGOCIO: Exactamente 10 ClassOfferings (5 profesores × 2 materias)
    // ============================================

    /// <summary>
    /// Obtiene las 10 ofertas de clase del periodo académico.
    /// REGLA DE NEGOCIO: Cada profesor dicta exactamente 2 materias.
    ///
    /// Distribución:
    /// - Profesor 1 (Dr. Carlos): MAT101, ALG101
    /// - Profesor 2 (Dra. Ana): PROG101, WEB101
    /// - Profesor 3 (Dr. Luis): DB101, CLOUD101
    /// - Profesor 4 (Dra. María): NET101, SEC101
    /// - Profesor 5 (Dr. Roberto): AI101, MOB101
    /// </summary>
    public static List<ClassOffering> GetClassOfferings()
    {
        return new List<ClassOffering>
        {
            // Profesor 1: Dr. Carlos - MAT101 y ALG101
            new ClassOffering
            {
                Id = 1,
                SubjectId = 1, // MAT101
                ProfessorId = 1, // Dr. Carlos
                OfferingCode = "MAT101-PROF001-2025-1",
                AcademicPeriod = CurrentAcademicPeriod,
                Schedule = "Lunes y Miércoles 8:00-10:00 AM",
                MaxCapacity = 30,
                CreatedAt = SeedDate,
                IsActive = true
            },
            new ClassOffering
            {
                Id = 2,
                SubjectId = 5, // ALG101
                ProfessorId = 1, // Dr. Carlos
                OfferingCode = "ALG101-PROF001-2025-1",
                AcademicPeriod = CurrentAcademicPeriod,
                Schedule = "Martes y Jueves 8:00-10:00 AM",
                MaxCapacity = 30,
                CreatedAt = SeedDate,
                IsActive = true
            },

            // Profesor 2: Dra. Ana - PROG101 y WEB101
            new ClassOffering
            {
                Id = 3,
                SubjectId = 2, // PROG101
                ProfessorId = 2, // Dra. Ana
                OfferingCode = "PROG101-PROF002-2025-1",
                AcademicPeriod = CurrentAcademicPeriod,
                Schedule = "Lunes y Miércoles 10:00 AM-12:00 PM",
                MaxCapacity = 35,
                CreatedAt = SeedDate,
                IsActive = true
            },
            new ClassOffering
            {
                Id = 4,
                SubjectId = 4, // WEB101
                ProfessorId = 2, // Dra. Ana
                OfferingCode = "WEB101-PROF002-2025-1",
                AcademicPeriod = CurrentAcademicPeriod,
                Schedule = "Martes y Jueves 10:00 AM-12:00 PM",
                MaxCapacity = 35,
                CreatedAt = SeedDate,
                IsActive = true
            },

            // Profesor 3: Dr. Luis - DB101 y CLOUD101
            new ClassOffering
            {
                Id = 5,
                SubjectId = 3, // DB101
                ProfessorId = 3, // Dr. Luis
                OfferingCode = "DB101-PROF003-2025-1",
                AcademicPeriod = CurrentAcademicPeriod,
                Schedule = "Lunes y Miércoles 2:00-4:00 PM",
                MaxCapacity = 30,
                CreatedAt = SeedDate,
                IsActive = true
            },
            new ClassOffering
            {
                Id = 6,
                SubjectId = 10, // CLOUD101
                ProfessorId = 3, // Dr. Luis
                OfferingCode = "CLOUD101-PROF003-2025-1",
                AcademicPeriod = CurrentAcademicPeriod,
                Schedule = "Martes y Jueves 2:00-4:00 PM",
                MaxCapacity = 25,
                CreatedAt = SeedDate,
                IsActive = true
            },

            // Profesor 4: Dra. María - NET101 y SEC101
            new ClassOffering
            {
                Id = 7,
                SubjectId = 6, // NET101
                ProfessorId = 4, // Dra. María
                OfferingCode = "NET101-PROF004-2025-1",
                AcademicPeriod = CurrentAcademicPeriod,
                Schedule = "Lunes y Miércoles 4:00-6:00 PM",
                MaxCapacity = 30,
                CreatedAt = SeedDate,
                IsActive = true
            },
            new ClassOffering
            {
                Id = 8,
                SubjectId = 7, // SEC101
                ProfessorId = 4, // Dra. María
                OfferingCode = "SEC101-PROF004-2025-1",
                AcademicPeriod = CurrentAcademicPeriod,
                Schedule = "Martes y Jueves 4:00-6:00 PM",
                MaxCapacity = 25,
                CreatedAt = SeedDate,
                IsActive = true
            },

            // Profesor 5: Dr. Roberto - AI101 y MOB101
            new ClassOffering
            {
                Id = 9,
                SubjectId = 8, // AI101
                ProfessorId = 5, // Dr. Roberto
                OfferingCode = "AI101-PROF005-2025-1",
                AcademicPeriod = CurrentAcademicPeriod,
                Schedule = "Lunes y Miércoles 6:00-8:00 PM",
                MaxCapacity = 25,
                CreatedAt = SeedDate,
                IsActive = true
            },
            new ClassOffering
            {
                Id = 10,
                SubjectId = 9, // MOB101
                ProfessorId = 5, // Dr. Roberto
                OfferingCode = "MOB101-PROF005-2025-1",
                AcademicPeriod = CurrentAcademicPeriod,
                Schedule = "Martes y Jueves 6:00-8:00 PM",
                MaxCapacity = 30,
                CreatedAt = SeedDate,
                IsActive = true
            }
        };
    }
}
