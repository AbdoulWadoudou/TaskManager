import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, MatToolbarModule, MatButtonModule, MatIconModule],
  template: `
    <mat-toolbar color="primary">
      <span>Task Manager</span>
      <span class="spacer"></span>
      <button mat-raised-button color="accent" routerLink="/tasks/new">
        <mat-icon>add</mat-icon>
        Nouvelle Tâche
      </button>
    </mat-toolbar>

    <div class="container">
      <router-outlet></router-outlet>
    </div>
  `,
  styles: [`
    .spacer { flex: 1 1 auto; }
    .container { padding: 20px; max-width: 1200px; margin: 0 auto; }
  `]
})
export class AppComponent {}
