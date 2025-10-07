import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';

document.addEventListener('DOMContentLoaded', () => {
  (window as any).M?.AutoInit?.();
});

bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));
