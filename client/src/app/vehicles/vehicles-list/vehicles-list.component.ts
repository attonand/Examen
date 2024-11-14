import { Component, effect, inject, signal } from '@angular/core';
import { VehiclesService } from 'src/app/services/vehicles.service';
import { Vehicle } from 'src/app/models/vehicle';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { PaginatedResult } from 'src/app/models/pagination';
import { FaIconLibrary, FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCoffee } from '@fortawesome/free-solid-svg-icons';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ToastService } from 'src/app/services/toast.service';
import { DomSanitizer } from '@angular/platform-browser';
import { debounceTime, Subject, tap } from 'rxjs';

@Component({
  selector: 'app-vehicles-list',
  standalone: true,
  imports: [FormsModule, PaginationModule, CommonModule, RouterModule, FontAwesomeModule],
  templateUrl: './vehicles-list.component.html',
})
export class VehiclesListComponent {
  paginatedResult = signal<PaginatedResult<Vehicle[]> | null>(null);
  private library = inject(FaIconLibrary); 
  private service = inject(VehiclesService);
  toastService = inject(ToastService);
  sanitizer = inject(DomSanitizer);
  private searchTerms = new Subject<string>();

  
  term: string = "";
  year: number | null = null;

  constructor() {
    this.library.addIconPacks(fas);
    this.library.addIcons(faCoffee);

    this.service.getPagedList();

    effect(() => {
      this.paginatedResult.set(this.service.paginatedResult());

    }, { allowSignalWrites: true });

    this.searchTerms.pipe(
      debounceTime(600), 
      tap((_) => {
        this.applyFilters();
      })
    ).subscribe();
  }

  applyFilters() {
  const term = this.term.trim();
    if(term != this.service.params().term) {
      this.service.params().term = term;
      this.service.getPagedList();
    }

    if(this.year != this.service.params().year) {
      this.service.params().year = this.year ?? 0;
      this.service.getPagedList();
    }
  }

  resetFilters() {
    this.service.resetParams();
    this.service.getPagedList();
  }

  onSearchChange() {
    this.searchTerms.next(this.term);
  }

  pageChanged(event: any) {
    if (this.service.params().pageNumber != event.page) {
      this.service.params().pageNumber = event.page;
      this.service.getPagedList();
    }
  }

  removeItem(id: number) {
    this.service.delete(id).subscribe({
      next: (_) => {
        this.toastService.add(this.sanitizer.bypassSecurityTrustHtml(`El <b>auto</b> fué eliminado con éxito.`), "success");
        this.service.getPagedList();
      },
    });
  }
}
