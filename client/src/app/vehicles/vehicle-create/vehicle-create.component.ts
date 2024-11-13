import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BrandSummary } from '../../models/brands/brands-summary';
import { BrandsService } from '../../services/brands.service';
import { VehiclesService } from '../../services/vehicles.service';
import { Photo, photosForTesting } from '../../models/photo';
import { VehicleSummary } from '../../models/vehicles/vehicle-summary';

type PhotoType = {
  url: AbstractControl<string | null>,
  id: AbstractControl<number | null>,
};

type FormType = {
  brand: AbstractControl<string | null>,
  model: AbstractControl<string | null>,
  year: AbstractControl<number | null>,
  color: AbstractControl<string | null>,
  photos: FormArray<FormGroup<PhotoType>>,
};

@Component({
  selector: 'app-vehicle-create',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule,
    // , RouterModule
  ],
  templateUrl: './vehicle-create.component.html',
  styleUrl: './vehicle-create.component.scss'
})
export class VehicleCreateComponent {
  private brandsService = inject(BrandsService);
  service = inject(VehiclesService);

  brands: BrandSummary[] = [];

  form: FormGroup<FormType> = new FormGroup<FormType>({
    brand: new FormControl(null) as any,
    model: new FormControl(null) as any,
    year: new FormControl(null) as any,
    color: new FormControl(null) as any,
    photos: new FormArray([]) as any,
  });

  submitted = signal<boolean>(false);

  constructor() {
    this.form.patchValue({
      color: 'White',
      model: 'Corolla',
      brand: 'Toyota',
      year: 2024,
    });

    photosForTesting.forEach(x => {
      this.form.controls.photos.push(new FormGroup({
        url: new FormControl(x.url) as any,
        id: new FormControl(x.id) as any,
      }));
    })
  }

  deletePhoto(idx: number) {
    this.form.controls.photos.removeAt(idx);
  }

  ngOnInit() {
    this.loadBrands();
  }

  loadBrands() {
    this.brandsService
      .getBrands()
      .subscribe({
        next: data => {
          this.brands = data;
          this.form.controls.brand.patchValue(this.brands[0].name);
        }
      });
  }

  addPhoto() {
    const formGroup = new FormGroup<PhotoType>({
      url: new FormControl(null) as any,
      id: new FormControl(null) as any,
    });

    this.form.controls.photos.push(formGroup);

    this.form.updateValueAndValidity();
  }

  removePhoto(index: number) {

  }

  onSubmit() {
    this.submitted.set(true);

    this.service.create(this.form.value).subscribe({
      next: (response: VehicleSummary) => {
        this.submitted.set(false);
      },
    })
  }
}
