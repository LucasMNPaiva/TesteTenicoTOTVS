export interface Product {
  id: string;
  name: string;
  price: number;
  stock: number;
}
export interface CreateProductRequest {
  name: string;
  price: number;
  stock: number;
}