import { Component, inject } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { BrandsService } from 'src/app/services/brands.service';
import { ToastService } from 'src/app/services/toast.service';
import { AlertModule } from 'ngx-bootstrap/alert';
import { ToastType } from 'src/app/_types/toastType';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterModule, AlertModule],
  template: `
  <nav class="navbar navbar-expand-lg navbar-light bg-light mb-4">
  <div class="container-fluid">
      <ul class="navbar-nav me-auto mb-2">
        <li class="nav-item">
          <a class="nav-link" routerLink="/Vehicles" routerLinkActive="active">Autos</a>
        </li>
        <li class="nav-item">
          <a class="nav-link" routerLink="/Brands" routerLinkActive="active">Marcas</a>
        </li>
      </ul>
  </div>
</nav>

<div class="container-fluid w-50">
  <div class="row">
    @for(alert of toastService.toasts(); let idx = $index; track idx) {
      <div class="d-flex justify-content-center">
        <alert [type]="alert.type" [dismissible]="alert.dismissible" (onClosed)="onClosed(alert)">
          <span [innerHtml]="alert.message"></span>    
        </alert>
      </div>      
    }
  </div>
</div>
<router-outlet />
  `,
})
export class AppComponent {
  private brandsService = inject(BrandsService);
  toastService = inject(ToastService);

  onClosed(dismissedToast: ToastType): void {
    this.toastService.remove(dismissedToast.id);
  }

  constructor() {
    this.brandsService.getOptions().subscribe();
  }
}
