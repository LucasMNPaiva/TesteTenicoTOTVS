import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Product, CreateProductRequest } from './product.model';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private http = inject(HttpClient);
  private base = '/api/products';

  list(): Observable<Product[]> {
    return this.http.get<Product[]>(this.base);
  }
  get(id: string): Observable<Product> {
    return this.http.get<Product>(`${this.base}/${id}`);
  }
  create(payload: CreateProductRequest): Observable<Product> {
    return this.http.post<Product>(this.base, payload).pipe(
      catchError(err => throwError(() => err))
    );
  }
  update(id: string, payload: CreateProductRequest): Observable<void> {
    return this.http.put<void>(`${this.base}/${id}`, payload);
  }
  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.base}/${id}`);
  }
}
