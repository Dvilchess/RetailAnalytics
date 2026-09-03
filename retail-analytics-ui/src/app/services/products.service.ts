import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { Product } from '../models/product';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  constructor(private api: ApiService) {}

  getProducts(): Observable<Product[]> {
    return this.api.get<Product[]>('products');
  }

  getProduct(id: number): Observable<Product> {
    return this.api.get<Product>(`products/${id}`);
  }

  createProduct(product: Partial<Product>): Observable<Product> {
    return this.api.post<Product>('products', product);
  }
}