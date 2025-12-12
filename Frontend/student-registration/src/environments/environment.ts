export const environment = {
  production: true,
  // Lee de variable global inyectada en HTML por Azure o usa valor por defecto
  apiBaseUrl: typeof window !== 'undefined' && (window as any)['APP_API_BASE_URL']
    ? (window as any)['APP_API_BASE_URL']
    : 'http://localhost:5004',
  
  appName: 'Student Registration',
  appVersion: '1.0.0'
};
