import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { ProductsService } from '../../services/products.service';
import { SalesService } from '../../services/sales.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule],
  template: `
    <div class="dashboard-container">
      <h1>Dashboard</h1>
      
      <div class="metrics-grid">
        <mat-card>
          <mat-card-header>
            <mat-card-title>Productos</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <h2>{{ totalProducts }}</h2>
            <p>Total en inventario</p>
          </mat-card-content>
        </mat-card>

        <mat-card>
          <mat-card-header>
            <mat-card-title>Ventas</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <h2>{{ totalSales }}</h2>
            <p>Total de ventas</p>
          </mat-card-content>
        </mat-card>

        <mat-card>
          <mat-card-header>
            <mat-card-title>Ingresos</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <h2>{{ totalRevenue | currency:'CLP':'symbol':'1.0-0' }}</h2>
            <p>Ingresos totales</p>
          </mat-card-content>
        </mat-card>

        <mat-card>
          <mat-card-header>
            <mat-card-title>Valor Promedio</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <h2>{{ averageOrder | currency:'CLP':'symbol':'1.0-0' }}</h2>
            <p>Por venta</p>
          </mat-card-content>
        </mat-card>
      </div>
    </div>
  `,
  styles: [`
    .dashboard-container {
      padding: 20px;
    }
    .metrics-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 20px;
      margin-top: 20px;
    }
    mat-card {
      text-align: center;
    }
    h2 {
      font-size: 2.5rem;
      margin: 10px 0;
      color: #3f51b5;
    }
    p {
      color: #666;
      margin: 0;
    }
  `]
})
export class DashboardComponent implements OnInit {
  totalProducts = 0;
  totalSales = 0;
  totalRevenue = 0;
  averageOrder = 0;

  constructor(
    private productsService: ProductsService,
    private salesService: SalesService
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.productsService.getProducts().subscribe({
      next: (products) => {
        this.totalProducts = products.length;
      }
    });

    this.salesService.getSales().subscribe({
      next: (sales) => {
        this.totalSales = sales.length;
        this.totalRevenue = sales.reduce((sum, sale) => sum + sale.totalAmount, 0);
        this.averageOrder = this.totalSales > 0 ? this.totalRevenue / this.totalSales : 0;
      }
    });
  }
}