import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { Sale, SaleCreateDTO } from '../models/sale';

@Injectable({
  providedIn: 'root'
})
export class SalesService {
  constructor(private api: ApiService) {}

  getSales(): Observable<Sale[]> {
    return this.api.get<Sale[]>('sales');
  }

  getSale(id: number): Observable<Sale> {
    return this.api.get<Sale>(`sales/${id}`);
  }

  createSale(sale: SaleCreateDTO): Observable<Sale> {
    return this.api.post<Sale>('sales', sale);
  }

  getAnalytics(): Observable<any[]> {
    return this.api.get<any[]>('sales/analytics');
  }
}