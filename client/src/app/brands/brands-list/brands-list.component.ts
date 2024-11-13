import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { BrandSummary } from '../../models/brands/brands-summary';
import { BrandsService } from '../../services/brands.service';

@Component({
  selector: 'app-brands-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './brands-list.component.html',
  styleUrl: './brands-list.component.scss'
})
export class BrandsListComponent {
  brands: BrandSummary[] = [];

  constructor(private brandService: BrandsService) {
  }

  ngOnInit() {
    this.loadBrands();
  }

  loadBrands()
  {
    this.brandService
      .getBrands()
      .subscribe(data => this.brands = data);
  }
}
