import { Component, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { Observable, Subject, switchMap, startWith, shareReplay, tap } from 'rxjs';
import { Product } from '../../data-access/product.model';
import { ProductService } from '../../data-access/product.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-list',
  standalone: true,
   imports: [CommonModule, RouterModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent {
  private svc = inject(ProductService);
  private router = inject(Router);

  private refresh$ = new Subject<void>();

  products$: Observable<Product[]> = this.refresh$.pipe(
    startWith(void 0),
    switchMap(() => this.svc.list()),
    shareReplay(1)
  );

  remove(id: string) {
    this.svc.delete(id).pipe(
      tap(() => {
        // @ts-ignore
        M.toast({ html: 'Produto excluído', classes: 'green' });
        this.refresh$.next();
      })
    ).subscribe();
  }

  edit(id: string) {
    this.router.navigate(['/products', id]);
  }
}
