import { CommonModule } from '@angular/common';
import { Component, effect, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { Brand } from 'src/app/models/brands';
import { PaginatedResult } from 'src/app/models/pagination';
import { BrandsService } from 'src/app/services/brands.service';

@Component({
  selector: 'app-brands-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PaginationModule],
  templateUrl: './brands-list.component.html',
})
export class BrandsListComponent {
  paginatedResult = signal<PaginatedResult<Brand[]> | null>(null);

  private service = inject(BrandsService);

  constructor() {
    this.service.getPagedList();

    effect(() => {
      this.paginatedResult.set(this.service.paginatedResult());
    }, { allowSignalWrites: true })
  }

  resetFilters() {
    this.service.resetParams();
  }

  pageChanged(event: any) {
    if (this.service.params().pageNumber != event.page) {
      this.service.params().pageNumber = event.page;
      this.service.getPagedList();
    }
  }
}
