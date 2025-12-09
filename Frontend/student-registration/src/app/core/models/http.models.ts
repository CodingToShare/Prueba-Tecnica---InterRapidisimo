/**
 * Representa un error estructurado basado en la especificación RFC 7807 (Problem Details for HTTP APIs).
 * Es el formato estándar que el backend .NET utiliza para comunicar errores.
 */
export interface ProblemDetails {
  /**
   * Un identificador URI que identifica el tipo de problema.
   */
  type: string;

  /**
   * Un resumen corto y legible del tipo de problema.
   */
  title: string;

  /**
   * El código de estado HTTP generado por el servidor de origen para esta ocurrencia del problema.
   */
  status: number;

  /**
   * Una explicación legible por humanos específica de esta ocurrencia del problema.
   */
  detail: string;

  /**
   * Una referencia URI que identifica la ocurrencia específica del problema.
   */
  instance: string;

  /**
   * Puede contener información adicional sobre el error, como errores de validación de campos.
   */
  errors?: Record<string, string[]>;
}
