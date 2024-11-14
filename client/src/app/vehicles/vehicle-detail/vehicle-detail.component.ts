import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { BadRequest } from 'src/app/models/badRequest';
import { Vehicle } from 'src/app/models/vehicle';
import { VehiclesService } from 'src/app/services/vehicles.service';

@Component({
  selector: 'app-vehicle-detail',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './vehicle-detail.component.html',
  styleUrl: './vehicle-detail.component.scss'
})
export class VehicleDetailComponent {
  service = inject(VehiclesService);
  route = inject(ActivatedRoute);

  error = signal<BadRequest | null>(null);
  
  item: Vehicle | null = null;

  constructor() {
    const id: any = this.route.snapshot.paramMap.get('id');
    if(id !== null) {
      this.service.getById(id).subscribe({
        next: (data) => {
          this.item = data;
        }
      });
    }
  }
}
