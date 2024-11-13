import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { BrandSummary } from '../models/brands/brands-summary';

@Injectable({
  providedIn: 'root'
})
export class BrandsService {
  apiUrl = `${environment.apiUrl}brands`;

  constructor(private http: HttpClient) {
  }

  getBrands(): Observable<BrandSummary[]> {
    return this.http.get<BrandSummary[]>(this.apiUrl);
  }
}
