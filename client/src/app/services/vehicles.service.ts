import { HttpClient } from '@angular/common/http';
import { Injectable, inject, model, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';
import { PaginatedResult } from '../models/pagination';
import { VehicleSummary } from '../models/vehicles/vehicle-summary';
import { VehicleParmas } from '../models/vehicles/vehicle-params';
import { Observable, of } from 'rxjs';
import { VehicleCreate } from '../models/vehicles/vehicle-create';

@Injectable({
  providedIn: 'root'
})
export class VehiclesService {
  private http = inject(HttpClient);

  baseUrl = environment.apiUrl;
  paginatedResult = signal<PaginatedResult<VehicleSummary[]> | null>(null);
  vehicleCache = new Map();
  params = signal<VehicleParmas>(new VehicleParmas());

  resetVehicleParmas() {
    this.params.set(new VehicleParmas());
  }

  getVehicles() {
    const response = this.vehicleCache.get(Object.values(this.params()).join('-'));

    if (response) return setPaginatedResponse(response, this.paginatedResult);

    let params = setPaginationHeaders(this.params().pageNumber, this.params().pageSize);


    params = params.append('term', this.params().term as string);
    params = params.append('year', this.params().year as number);

    return this.http.get<VehicleSummary[]>(this.baseUrl + 'vehicles', {observe: 'response', params}).subscribe({
      next: response => {
        setPaginatedResponse(response, this.paginatedResult);
        this.vehicleCache.set(Object.values(this.params()).join('-'), response);
      }
    })
  }

  getVehicle(id: number) {
    const vehicle: VehicleSummary = [...this.vehicleCache.values()]
      .reduce((arr, elem) => arr.concat(elem.body), [])
      .find((m: VehicleSummary) => m.id === id);

    if (vehicle) return of(vehicle);

    return this.http.get<VehicleSummary>(`${this.baseUrl}'vehicles/${id}`);
  }

  postVehicle(body: VehicleCreate) {
    return this.http.post(`${this.baseUrl}'vehicles`, body);
  }

  updateVehicle(id: number, body: VehicleCreate) {
    return this.http.put(`${this.baseUrl}'vehicles/${id}`, body);
  }

  deleteVehicle(id: number) {
    return this.http.delete(`${this.baseUrl}'vehicles/${id}`);
  }

  create(model: any): Observable<VehicleSummary> {
    return this.http.post<VehicleSummary>(`${this.baseUrl}vehicles`, model);
  }
}
