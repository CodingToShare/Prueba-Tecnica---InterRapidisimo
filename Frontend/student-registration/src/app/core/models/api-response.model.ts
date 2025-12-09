/**
 * Wrapper genérico para las respuestas del servidor.
 * El backend envuelve todas las respuestas en este formato.
 */
export interface ApiResponse<T> {
  message: string;
  data: T;
}
