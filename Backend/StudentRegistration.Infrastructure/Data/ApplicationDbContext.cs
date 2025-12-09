using Microsoft.EntityFrameworkCore;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Infrastructure.Data;

/// <summary>
/// Contexto de base de datos principal de la aplicación.
/// Configura todas las entidades y sus relaciones usando Entity Framework Core.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // ============================================
    // DbSets - Tablas de la base de datos
    // ============================================

    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<CreditProgram> CreditPrograms { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Professor> Professors { get; set; }
    public DbSet<ClassOffering> ClassOfferings { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    /// <summary>
    /// Configuración de entidades y relaciones usando Fluent API.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ============================================
        // CONFIGURACIÓN DE USER
        // ============================================
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Username único
            entity.HasIndex(e => e.Username)
                .IsUnique();

            // Email único
            entity.HasIndex(e => e.Email)
                .IsUnique();

            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.PasswordSalt)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.Role)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Student");

            // Relación 1:1 con Student
            entity.HasOne(e => e.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ============================================
        // CONFIGURACIÓN DE STUDENT
        // ============================================
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id);

            // StudentNumber único
            entity.HasIndex(e => e.StudentNumber)
                .IsUnique();

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.StudentNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20);

            entity.Property(e => e.Address)
                .HasMaxLength(250);

            // Ignorar FullName ya que es una propiedad calculada
            entity.Ignore(e => e.FullName);

            // Relación N:1 con CreditProgram
            entity.HasOne(e => e.CreditProgram)
                .WithMany(cp => cp.Students)
                .HasForeignKey(e => e.CreditProgramId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación 1:N con Enrollments
            entity.HasMany(e => e.Enrollments)
                .WithOne(en => en.Student)
                .HasForeignKey(en => en.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ============================================
        // CONFIGURACIÓN DE CREDIT PROGRAM
        // ============================================
        modelBuilder.Entity<CreditProgram>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Código único
            entity.HasIndex(e => e.Code)
                .IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.TotalCreditsRequired)
                .IsRequired();
        });

        // ============================================
        // CONFIGURACIÓN DE SUBJECT
        // ============================================
        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Código único
            entity.HasIndex(e => e.Code)
                .IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.Credits)
                .IsRequired()
                .HasDefaultValue(3); // REGLA DE NEGOCIO: Todas las materias valen 3 créditos

            // Relación 1:N con ClassOfferings
            entity.HasMany(e => e.ClassOfferings)
                .WithOne(co => co.Subject)
                .HasForeignKey(co => co.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ============================================
        // CONFIGURACIÓN DE PROFESSOR
        // ============================================
        modelBuilder.Entity<Professor>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Código de empleado único
            entity.HasIndex(e => e.EmployeeCode)
                .IsUnique();

            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.EmployeeCode)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Department)
                .HasMaxLength(100);

            entity.Property(e => e.Specialization)
                .HasMaxLength(100);

            // Relación 1:N con ClassOfferings
            // REGLA DE NEGOCIO: Cada profesor dicta exactamente 2 materias
            entity.HasMany(e => e.ClassOfferings)
                .WithOne(co => co.Professor)
                .HasForeignKey(co => co.ProfessorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ============================================
        // CONFIGURACIÓN DE CLASS OFFERING
        // ============================================
        modelBuilder.Entity<ClassOffering>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Índice compuesto único: una materia solo puede ser dictada por un profesor
            // en un período académico específico (evita duplicados)
            entity.HasIndex(e => new { e.SubjectId, e.ProfessorId, e.AcademicPeriod })
                .IsUnique();

            entity.Property(e => e.OfferingCode)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.AcademicPeriod)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Schedule)
                .HasMaxLength(100);

            // Relación 1:N con Enrollments
            entity.HasMany(e => e.Enrollments)
                .WithOne(en => en.ClassOffering)
                .HasForeignKey(en => en.ClassOfferingId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ============================================
        // CONFIGURACIÓN DE ENROLLMENT
        // ============================================
        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Índice compuesto único: un estudiante no puede inscribirse
            // dos veces en la misma oferta de clase
            entity.HasIndex(e => new { e.StudentId, e.ClassOfferingId })
                .IsUnique();

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Active");

            entity.Property(e => e.FinalGrade)
                .HasPrecision(5, 2); // Ej: 100.00

            entity.Property(e => e.Notes)
                .HasMaxLength(500);

            // IMPORTANTE: Las validaciones de reglas de negocio se harán en la capa de aplicación:
            // - Máximo 3 inscripciones por estudiante
            // - Las 3 inscripciones deben tener profesores diferentes
        });

        // ============================================
        // SEEDING DE DATOS INICIALES
        // ============================================

        // Seed de programas de créditos
        modelBuilder.Entity<CreditProgram>().HasData(DataSeeder.GetCreditPrograms());

        // Seed de materias (10 materias, todas con 3 créditos)
        modelBuilder.Entity<Subject>().HasData(DataSeeder.GetSubjects());

        // Seed de profesores (5 profesores)
        modelBuilder.Entity<Professor>().HasData(DataSeeder.GetProfessors());

        // Seed de ofertas de clase (10 ClassOfferings: 5 profesores × 2 materias)
        modelBuilder.Entity<ClassOffering>().HasData(DataSeeder.GetClassOfferings());
    }

    /// <summary>
    /// Sobrescribe SaveChanges para actualizar automáticamente UpdatedAt.
    /// </summary>
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    /// <summary>
    /// Sobrescribe SaveChangesAsync para actualizar automáticamente UpdatedAt.
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Actualiza automáticamente los campos UpdatedAt de las entidades modificadas.
    /// </summary>
    private void UpdateTimestamps()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is Domain.Common.BaseEntity &&
                       (e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            ((Domain.Common.BaseEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
        }
    }
}
