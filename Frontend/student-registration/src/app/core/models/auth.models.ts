/**
 * DTO para la respuesta de autenticación exitosa.
 * Contiene el token JWT y datos básicos del usuario y estudiante.
 */
export interface AuthResponseDto {
  token: string;
  expiresAt: string; // ISO 8601 date string
  tokenType: string;
  userId: string;
  username: string;
  email: string;
  studentId: number;
  studentFullName: string;
  studentNumber: string;
  role: string | string[];
}

/**
 * Wrapper para las respuestas del servidor de autenticación.
 */
export interface AuthApiResponse {
  message: string;
  data: AuthResponseDto;
}

/**
 * DTO para el registro de un nuevo estudiante.
 * Contiene toda la información necesaria para crear el usuario y el perfil de estudiante.
 */
export interface RegisterDto {
  username: string;
  email: string;
  password?: string; // Optional on frontend if backend handles it
  confirmPassword?: string;
  firstName: string;
  lastName: string;
  studentNumber: string;
  dateOfBirth: string; // ISO 8601 date string
  phoneNumber: string;
  address: string;
  creditProgramId: number;
}

/**
 * DTO para el inicio de sesión.
 */
export interface LoginDto {
  usernameOrEmail: string;
  password?: string;
}
