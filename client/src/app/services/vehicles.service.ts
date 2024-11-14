import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { environment } from 'src/environments/environment';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';
import { PaginatedResult } from 'src/app/models/pagination';
import { Vehicle } from 'src/app/models/vehicle';
import { VehicleParms } from 'src/app/models/vehicleParams';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VehiclesService {
  private http = inject(HttpClient);

  baseUrl = `${environment.apiUrl}vehicles/`;
  paginatedResult = signal<PaginatedResult<Vehicle[]> | null>(null);
  cache = new Map();
  params = signal<VehicleParms>(new VehicleParms());

  resetParams() {
    this.params.set(new VehicleParms());
  }

  getPagedList() {
    const response = this.cache.get(Object.values(this.params()).join('-'));

    if (response) return setPaginatedResponse(response, this.paginatedResult);

    let params = setPaginationHeaders(this.params().pageNumber, this.params().pageSize);


    params = params.append('term', this.params().term as string);
    params = params.append('year', this.params().year as number);

    return this.http.get<Vehicle[]>(this.baseUrl, {observe: 'response', params}).subscribe({
      next: response => {

  
        setPaginatedResponse(response, this.paginatedResult);
        this.cache.set(Object.values(this.params()).join('-'), response);
      }
    })
  }

  getById(id: number) {
    const vehicle: Vehicle = [...this.cache.values()]
      .reduce((arr, elem) => arr.concat(elem.body), [])
      .find((m: Vehicle) => m.id === id);

    if (vehicle) return of(vehicle);

    return this.http.get<Vehicle>(`${this.baseUrl}${id}`);
  }

  update(id: number, model: any): Observable<Vehicle> {
    this.cache.clear();
    return this.http.put<Vehicle>(`${this.baseUrl}${id}`, model);
  }

  delete(id: number) {
    this.cache.clear();
    return this.http.delete(`${this.baseUrl}${id}`);
  }

  create(model: any): Observable<Vehicle> {
    this.cache.clear();
    return this.http.post<Vehicle>(`${this.baseUrl}`, model);
  }
}
