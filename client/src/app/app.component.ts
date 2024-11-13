import { Component, inject } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { BrandsService } from 'src/app/services/brands.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterModule],
  template: `
  <nav class="navbar navbar-expand-lg navbar-light bg-light mb-4">
  <div class="container-fluid">
      <ul class="navbar-nav me-auto mb-2">
        <li class="nav-item">
          <a class="nav-link" routerLink="/Vehicles" routerLinkActive="active">Vehicles</a>
        </li>
        <li class="nav-item">
          <a class="nav-link" routerLink="/Brands" routerLinkActive="active">Brands</a>
        </li>
      </ul>
  </div>
</nav>
<router-outlet />
  `,
})
export class AppComponent {
  private brandsService = inject(BrandsService);

  constructor() {
    this.brandsService.getOptions().subscribe();
  }
  
}
