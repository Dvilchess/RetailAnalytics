import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
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
          <mat-card-header><mat-card-title>Productos</mat-card-title></mat-card-header>
          <mat-card-content><h2>{{ totalProducts }}</h2></mat-card-content>
        </mat-card>
        <mat-card>
          <mat-card-header><mat-card-title>Ventas</mat-card-title></mat-card-header>
          <mat-card-content><h2>{{ totalSales }}</h2></mat-card-content>
        </mat-card>
        <mat-card>
          <mat-card-header><mat-card-title>Ingresos</mat-card-title></mat-card-header>
          <mat-card-content><h2>{{ totalRevenue | currency:'CLP':'symbol':'1.0-0' }}</h2></mat-card-content>
        </mat-card>
      </div>
    </div>
  `,
  styles: [`
    .dashboard-container { padding: 20px; }
    .metrics-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(250px, 1fr)); gap: 20px; }
    h2 { font-size: 2rem; color: #3f51b5; }
  `]
})
export class DashboardComponent implements OnInit {
  totalProducts = 0;
  totalSales = 0;
  totalRevenue = 0;

  constructor(
    private productsService: ProductsService,
    private salesService: SalesService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    console.log('Dashboard ngOnInit ejecutado');
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    console.log('Cargando datos...');
    
    this.productsService.getProducts().subscribe({
      next: (products) => {
        console.log('Productos recibidos:', products);
        this.totalProducts = Array.isArray(products) ? products.length : 0;
        this.cdr.detectChanges();
        console.log('totalProducts actualizado a:', this.totalProducts);
      },
      error: (err) => console.error('Error productos:', err)
    });

    this.salesService.getSales().subscribe({
      next: (sales) => {
        console.log('Ventas recibidas:', sales);
        this.totalSales = Array.isArray(sales) ? sales.length : 0;
        if (Array.isArray(sales)) {
          this.totalRevenue = sales.reduce((sum, sale) => sum + sale.totalAmount, 0);
        }
        this.cdr.detectChanges();
        console.log('totalSales actualizado a:', this.totalSales);
        console.log('totalRevenue actualizado a:', this.totalRevenue);
      },
      error: (err) => console.error('Error ventas:', err)
    });
  }
}