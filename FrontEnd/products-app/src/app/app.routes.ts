import { Routes } from '@angular/router';
import { ProductListComponent } from './features/products/ui/product-list/product-list.component';
import { ProductFormComponent } from './features/products/ui/product-form/product-form.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'products' },
  { path: 'products', component: ProductListComponent },
  { path: 'products/new', component: ProductFormComponent },
  { path: 'products/:id', component: ProductFormComponent },
  { path: '**', redirectTo: 'products' }
];
