import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SalesService } from '../../services/sales.service';
import { Sale } from '../../models/sale';

@Component({
  selector: 'app-sale-list',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div style="padding: 20px;">
      <h1>Ventas ({{ sales.length }})</h1>
      <table border="1" style="width: 100%; border-collapse: collapse;">
        <thead>
          <tr style="background: #3f51b5; color: white;">
            <th>ID</th>
            <th>Cliente</th>
            <th>Fecha</th>
            <th>Productos</th>
            <th>Total</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let sale of sales">
            <td>{{ sale.saleId }}</td>
            <td>{{ sale.customer?.name }}</td>
            <td>{{ sale.saleDate | date:'dd/MM/yyyy HH:mm' }}</td>
            <td>{{ sale.saleItems.length }}</td>
            <td>{{ sale.totalAmount | currency:'CLP':'symbol':'1.0-0' }}</td>
          </tr>
          <tr *ngIf="sales.length === 0">
            <td colspan="5" style="text-align: center;">No hay ventas</td>
          </tr>
        </tbody>
      </table>
    </div>
  `,
  styles: [`td, th { padding: 10px; text-align: left; }`]
})
export class SaleListComponent implements OnInit {
  sales: Sale[] = [];

  constructor(
    private salesService: SalesService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    console.log('SaleList ngOnInit ejecutado');
    this.salesService.getSales().subscribe({
      next: (data) => {
        console.log('Ventas cargadas:', data);
        this.sales = data;
        this.cdr.detectChanges();
        console.log('sales actualizado:', this.sales.length);
      },
      error: (err) => console.error('Error:', err)
    });
  }
}