/**
 * Información de la materia (curso).
 */
interface SubjectInfo {
  subjectId: number;
  subjectCode: string;
  subjectName: string;
  subjectDescription: string;
  credits: number;
}

/**
 * Información del profesor que imparte una materia.
 */
interface ProfessorInfo {
  professorId: number;
  professorFullName: string;
  professorEmail: string;
  professorDepartment: string;
}

/**
 * DTO que representa la oferta de una clase para un período académico.
 * Estructura plana devuelta por el backend para getClassOfferings().
 */
export interface ClassOfferingDto {
  id: number;
  offeringCode: string;
  academicPeriod: string;
  schedule: string;
  maxCapacity: number;
  currentEnrollmentCount: number;
  hasAvailableSpots: boolean;
  isActive: boolean;
  // Flattened subject properties
  subjectId: number;
  subjectCode: string;
  subjectName: string;
  subjectDescription: string;
  credits: number;
  // Flattened professor properties
  professorId: number;
  professorFullName: string;
  professorEmail: string;
  professorDepartment: string;
  // UI display logic properties
  canEnroll?: boolean;
  reasonCannotEnroll?: string;
}

/**
 * DTO con estructura anidada de la oferta de clase (usado en endpoints detallados).
 * Combina la información de la materia y el profesor en objetos anidados.
 */
export interface ClassOfferingWithDetailsDto {
  id: number;
  offeringCode: string;
  academicPeriod: string;
  schedule: string;
  maxCapacity: number;
  currentEnrollmentCount: number;
  hasAvailableSpots: boolean;
  isActive: boolean;
  subject: SubjectInfo;
  professor: ProfessorInfo;
  canEnroll?: boolean;
  reasonCannotEnroll?: string;
}

/**
 * DTO para crear una nueva inscripción.
 */
export interface CreateEnrollmentDto {
  classOfferingId: number;
  notes?: string;
}

/**
 * DTO con los detalles de una inscripción de un estudiante.
 * Estructura plana devuelta por el backend para getMyEnrollments().
 */
export interface EnrollmentDetailsDto {
  id: number;
  enrollmentDate: string; // ISO 8601 date string
  status: 'Active' | 'Cancelled' | 'Completed' | 'Dropped';
  notes?: string;
  subjectName: string;
  professorFullName: string;
  subjectCode: string;
  credits: number;
  academicPeriod: string;
  schedule: string;
  classOffering?: {
    professor: {
      professorId: number;
    };
    subject: {
      subjectId: number;
    };
  };
}

/**
 * DTO extendido de una inscripción que incluye una lista de nombres de compañeros de clase.
 * Estructura plana devuelta por getMyClassesDetails().
 */
export interface EnrollmentWithClassmatesDto {
  id: number;
  classOfferingId: number;
  studentId: number;
  studentNumber: string;
  studentFullName: string;
  enrollmentDate: string;
  status: 'Active' | 'Cancelled' | 'Completed' | 'Dropped';
  notes?: string;
  finalGrade?: number;
  isActive: boolean;
  createdAt: string;
  // Flattened subject properties
  subjectId: number;
  subjectCode: string;
  subjectName: string;
  subjectDescription: string;
  credits: number;
  // Flattened professor properties
  professorId: number;
  professorFullName: string;
  professorEmail: string;
  professorDepartment: string;
  // Flattened offering properties
  offeringCode: string;
  academicPeriod: string;
  schedule: string;
  // Classmates list
  classmates: string[];
}
