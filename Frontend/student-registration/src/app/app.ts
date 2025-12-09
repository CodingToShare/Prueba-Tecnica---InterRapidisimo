import { Component } from '@angular/core'; // Removed signal
import { RouterOutlet } from '@angular/router';
import { LoadingOverlayComponent } from './shared/components/loading-overlay/loading-overlay.component'; // Import LoadingOverlayComponent

@Component({
  selector: 'app-root',
  standalone: true, // App component should also be standalone
  imports: [
    RouterOutlet,
    LoadingOverlayComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  // protected readonly title = signal('student-registration'); // Removed
}
