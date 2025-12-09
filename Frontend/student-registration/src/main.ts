import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';
import 'zone.js'; // Explicitly import zone.js polyfill for Vite

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
