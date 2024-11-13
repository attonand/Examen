import { Routes } from '@angular/router';
import { VehiclesListComponent } from './vehicles/vehicles-list/vehicles-list.component';
import { BrandsListComponent } from './brands/brands-list/brands-list.component';
import { VehicleDetailComponent } from './vehicles/vehicle-detail/vehicle-detail.component';
import { VehicleEditComponent } from './vehicles/vehicle-edit/vehicle-edit.component';
import { VehicleCreateComponent } from './vehicles/vehicle-create/vehicle-create.component';

export const routes: Routes = [
  { path: 'Vehicles', component: VehiclesListComponent },

  { path: 'Vehicles/Create', component: VehicleCreateComponent },

  { path: 'Vehicles/:id', component: VehicleDetailComponent },

  { path: 'Vehicles/:id/Edit', component: VehicleEditComponent },

  { path: 'Brands', component: BrandsListComponent },

  { path: '**', redirectTo: 'vehicles' }
];
