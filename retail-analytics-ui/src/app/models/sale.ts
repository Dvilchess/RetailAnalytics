import { Customer } from './customer';
import { Product } from './product';

export interface Sale {
  saleId: number;
  customerId: number;
  customer?: Customer;
  totalAmount: number;
  discount: number;
  saleDate: Date;
  paymentMethod: string;
  status: string;
  saleItems: SaleItem[];
}

export interface SaleItem {
  saleItemId: number;
  saleId: number;
  productId: number;
  product?: Product;
  quantity: number;
  unitPrice: number;
  subtotal: number;
}

export interface SaleCreateDTO {
  customerId: number;
  paymentMethod: string;
  discount: number;
  saleItems: SaleItemDTO[];
}

export interface SaleItemDTO {
  productId: number;
  quantity: number;
}