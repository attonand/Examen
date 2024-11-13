import { Component, effect, inject, signal } from '@angular/core';
import { VehiclesService } from '../../services/vehicles.service';
import { VehicleSummary } from '../../models/vehicles/vehicle-summary';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { PaginatedResult } from '../../models/pagination';

@Component({
  selector: 'app-vehicles-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './vehicles-list.component.html',
  styleUrl: './vehicles-list.component.scss'
})
export class VehiclesListComponent {
  paginatedResult = signal<PaginatedResult<VehicleSummary[]> | null>(null);

  private service = inject(VehiclesService);

  constructor() {
    this.service.getVehicles();

    effect(() => {
      this.paginatedResult.set(this.service.paginatedResult());

      console.log(this.service.paginatedResult());
    }, { allowSignalWrites: true })
  }

  resetFilters() {
    this.service.resetVehicleParmas();
  }

  pageChanged(event: any) {
    if (this.service.params().pageNumber != event.page) {
      this.service.params().pageNumber = event.page;
    }
  }
}
