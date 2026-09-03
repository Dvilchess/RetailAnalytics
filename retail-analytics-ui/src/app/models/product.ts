export interface Product {
  productId: number;
  name: string;
  sku: string;
  price: number;
  categoryId: number;
  category?: Category;
  stockQuantity: number;
  isActive: boolean;
  createdAt: Date;
}

export interface Category {
  categoryId: number;
  name: string;
  description: string;
  products?: Product[];
}