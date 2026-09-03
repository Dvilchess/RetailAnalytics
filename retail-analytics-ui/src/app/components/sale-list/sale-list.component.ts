import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { SalesService } from '../../services/sales.service';
import { Sale } from '../../models/sale';

@Component({
  selector: 'app-sale-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatCardModule],
  template: `
    <mat-card>
      <mat-card-header>
        <mat-card-title>Historial de Ventas</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <table mat-table [dataSource]="sales" class="mat-elevation-z8">
          <ng-container matColumnDef="id">
            <th mat-header-cell *matHeaderCellDef> ID </th>
            <td mat-cell *matCellDef="let sale"> {{ sale.saleId }} </td>
          </ng-container>

          <ng-container matColumnDef="customer">
            <th mat-header-cell *matHeaderCellDef> Cliente </th>
            <td mat-cell *matCellDef="let sale"> {{ sale.customer?.name }} </td>
          </ng-container>

          <ng-container matColumnDef="date">
            <th mat-header-cell *matHeaderCellDef> Fecha </th>
            <td mat-cell *matCellDef="let sale"> {{ sale.saleDate | date:'dd/MM/yyyy HH:mm' }} </td>
          </ng-container>

          <ng-container matColumnDef="items">
            <th mat-header-cell *matHeaderCellDef> Productos </th>
            <td mat-cell *matCellDef="let sale"> {{ sale.saleItems.length }} </td>
          </ng-container>

          <ng-container matColumnDef="total">
            <th mat-header-cell *matHeaderCellDef> Total </th>
            <td mat-cell *matCellDef="let sale"> {{ sale.totalAmount | currency:'CLP':'symbol':'1.0-0' }} </td>
          </ng-container>

          <ng-container matColumnDef="status">
            <th mat-header-cell *matHeaderCellDef> Estado </th>
            <td mat-cell *matCellDef="let sale"> {{ sale.status }} </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
        </table>
      </mat-card-content>
    </mat-card>
  `,
  styles: [`
    table { width: 100%; }
    mat-card { margin: 20px; }
  `]
})
export class SaleListComponent implements OnInit {
  sales: Sale[] = [];
  displayedColumns: string[] = ['id', 'customer', 'date', 'items', 'total', 'status'];

  constructor(private salesService: SalesService) {}

  ngOnInit(): void {
    this.loadSales();
  }

  loadSales(): void {
    this.salesService.getSales().subscribe({
      next: (data) => {
        this.sales = data;
        console.log('Ventas cargadas:', data);
      },
      error: (error) => {
        console.error('Error al cargar ventas:', error);
      }
    });
  }
}
