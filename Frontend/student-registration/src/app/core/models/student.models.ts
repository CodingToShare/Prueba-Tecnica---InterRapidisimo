/**
 * DTO con la información resumida de un estudiante, ideal para listas.
 */
export interface StudentDto {
  id: number;
  fullName: string;
  studentNumber: string;
  email: string;
  creditProgramName: string;
}

/**
 * DTO con los detalles completos del perfil de un estudiante.
 */
export interface StudentDetailsDto {
  id: number;
  userId: string;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  fullName: string;
  studentNumber: string;
  dateOfBirth: string; // ISO 8601 date string
  phoneNumber: string;
  address: string;
  creditProgramId: number;
  creditProgramName: string;
  creditProgramCode: string;
  totalCreditsRequired: number;
  currentEnrollmentsCount: number;
}
