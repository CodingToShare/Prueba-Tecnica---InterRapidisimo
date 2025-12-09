-- Script de verificación del seeding de datos
-- Ejecutar en SQL Server Management Studio o Azure Data Studio contra StudentRegistrationDB

USE StudentRegistrationDB;
GO

PRINT '========================================';
PRINT 'VERIFICACIÓN DE SEEDING DE DATOS';
PRINT '========================================';
PRINT '';

-- Verificar CreditPrograms
PRINT 'PROGRAMAS DE CRÉDITOS (Debe haber 3):';
SELECT Id, Code, Name, TotalCreditsRequired FROM CreditPrograms;
PRINT '';

-- Verificar Subjects
PRINT 'MATERIAS (Debe haber 10, todas con 3 créditos):';
SELECT Id, Code, Name, Credits FROM Subjects ORDER BY Id;
PRINT '';

-- Verificar Professors
PRINT 'PROFESORES (Debe haber 5):';
SELECT Id, EmployeeCode, FullName, Department FROM Professors ORDER BY Id;
PRINT '';

-- Verificar ClassOfferings
PRINT 'OFERTAS DE CLASE (Debe haber 10 - 5 profesores × 2 materias):';
SELECT
    co.Id,
    co.OfferingCode,
    s.Code as SubjectCode,
    s.Name as SubjectName,
    p.FullName as ProfessorName,
    co.Schedule
FROM ClassOfferings co
INNER JOIN Subjects s ON co.SubjectId = s.Id
INNER JOIN Professors p ON co.ProfessorId = p.Id
ORDER BY co.Id;
PRINT '';

-- Verificar distribución: cada profesor debe tener exactamente 2 materias
PRINT 'DISTRIBUCIÓN DE MATERIAS POR PROFESOR (Cada uno debe tener 2):';
SELECT
    p.FullName as Professor,
    COUNT(*) as NumberOfSubjects,
    STRING_AGG(s.Name, ', ') as Subjects
FROM ClassOfferings co
INNER JOIN Professors p ON co.ProfessorId = p.Id
INNER JOIN Subjects s ON co.SubjectId = s.Id
GROUP BY p.Id, p.FullName
ORDER BY p.Id;
PRINT '';

-- Verificar que no hay estudiantes aún (se crearán en Meta 4)
PRINT 'USUARIOS Y ESTUDIANTES (Debe estar vacío hasta Meta 4):';
SELECT COUNT(*) as TotalUsers FROM Users;
SELECT COUNT(*) as TotalStudents FROM Students;
SELECT COUNT(*) as TotalEnrollments FROM Enrollments;
PRINT '';

PRINT '========================================';
PRINT 'VERIFICACIÓN COMPLETADA';
PRINT '========================================';
