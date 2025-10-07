import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { switchMap, tap, of, Observable, map } from 'rxjs';
import { ProductService } from '../../data-access/product.service';
import { CreateProductRequest } from '../../data-access/product.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './product-form.component.html'
})
export class ProductFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private svc = inject(ProductService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  isEdit = false;
  id: string | null = null;

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(120)]],
    price: [0, [Validators.required, Validators.min(0)]],
    stock: [0, [Validators.required, Validators.min(0)]],
  });

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
    this.isEdit = !!this.id;

    if (this.isEdit && this.id) {
      this.svc.get(this.id).subscribe(p => this.form.patchValue(p));
    }

    // re-ativa labels no Materialize após render/patch (UX)
    setTimeout(() => {
      // @ts-ignore
      M.updateTextFields?.();
    },100);
  }

  invalid(control: string) {
    const c = this.form.get(control);
    return c && c.invalid && (c.dirty || c.touched);
  }

  save() {
    if (this.form.invalid) return;

    const payload = this.form.value as CreateProductRequest;

    const op$: Observable<void> = this.isEdit && this.id
  ? this.svc.update(this.id, payload)                      // já é void
  : this.svc.create(payload).pipe(map(() => void 0));      // vira void

    op$.subscribe(() => this.router.navigate(['/products']));
  }

  private toast(msg: string) {
    // @ts-ignore
    M.toast({ html: msg, classes: 'blue' });
  }
}
