import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, MatToolbarModule],
  template: `
    <mat-toolbar color="primary">
      <span>Retail Analytics Dashboard</span>
      <span class="spacer"></span>
      <nav>
        <a routerLink="/dashboard" routerLinkActive="active">Dashboard</a>
        <a routerLink="/products" routerLinkActive="active">Productos</a>
        <a routerLink="/sales" routerLinkActive="active">Ventas</a>
      </nav>
    </mat-toolbar>
    <router-outlet></router-outlet>
  `,
  styles: [`
    .spacer { flex: 1 1 auto; }
    nav a {
      color: white;
      text-decoration: none;
      margin: 0 10px;
      padding: 5px 10px;
    }
    nav a.active {
      background-color: rgba(255, 255, 255, 0.2);
      border-radius: 4px;
    }
  `]
})
export class AppComponent {
  title = 'retail-analytics-ui';
}