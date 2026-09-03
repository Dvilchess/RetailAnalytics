import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductsService } from '../../services/products.service';
import { Product } from '../../models/product';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div style="padding: 20px;">
      <h1>Productos ({{ products.length }})</h1>
      <table border="1" style="width: 100%; border-collapse: collapse;">
        <thead>
          <tr style="background: #3f51b5; color: white;">
            <th>ID</th>
            <th>Nombre</th>
            <th>SKU</th>
            <th>Precio</th>
            <th>Stock</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let product of products">
            <td>{{ product.productId }}</td>
            <td>{{ product.name }}</td>
            <td>{{ product.sku }}</td>
            <td>{{ product.price | currency:'CLP':'symbol':'1.0-0' }}</td>
            <td>{{ product.stockQuantity }}</td>
          </tr>
          <tr *ngIf="products.length === 0">
            <td colspan="5" style="text-align: center;">No hay productos</td>
          </tr>
        </tbody>
      </table>
    </div>
  `,
  styles: [`td, th { padding: 10px; text-align: left; }`]
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];

  constructor(
    private productsService: ProductsService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    console.log('ProductList ngOnInit ejecutado');
    this.productsService.getProducts().subscribe({
      next: (data) => {
        console.log('Productos cargados:', data);
        this.products = data;
        this.cdr.detectChanges();
        console.log('products actualizado:', this.products.length);
      },
      error: (err) => console.error('Error:', err)
    });
  }
}