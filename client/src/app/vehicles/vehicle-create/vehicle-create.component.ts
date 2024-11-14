import { CommonModule } from '@angular/common';
import { Component, effect, inject, signal } from '@angular/core';
import { AbstractControl, FormArray, FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SelectOption } from 'src/app/models/selectOption';
import { BadRequest } from 'src/app/models/badRequest';
import { createId } from '@paralleldrive/cuid2';
import { BrandsService } from 'src/app/services/brands.service';
import { VehiclesService } from 'src/app/services/vehicles.service';
import { photosForTesting } from 'src/app/models/photo';
import { Vehicle } from 'src/app/models/vehicle';

type PhotoType = {
  url: AbstractControl<string | null>,
  id: AbstractControl<number | null>,
};

type FormType = {
  brand: AbstractControl<SelectOption | null>,
  model: AbstractControl<string | null>,
  year: AbstractControl<number | null>,
  color: AbstractControl<string | null>,
  photos: FormArray<FormGroup<PhotoType>>,
};

@Component({
  selector: 'app-vehicle-create',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule],
  templateUrl: './vehicle-create.component.html',
})
export class VehicleCreateComponent {
  error = signal<BadRequest | null>(null);
  id = createId();
  
  private brandsService = inject(BrandsService);
  service = inject(VehiclesService);

  brandOptions: SelectOption[] = [];

  form: FormGroup<FormType> = new FormGroup<FormType>({
    brand: new FormControl<SelectOption | null>(null) as any,
    model: new FormControl<string | null>(null) as any,
    year: new FormControl<number | null>(null) as any,
    color: new FormControl<string | null>(null) as any,
    photos: new FormArray([]) as any,
  });

  submitted = signal<boolean>(false);

  constructor() {
    this.brandsService.getOptions().subscribe();
    
    this.form.patchValue({
      color: 'White',
      model: 'Corolla',
      brand: null,
      year: 2024,
    });

    photosForTesting.forEach(x => {
      this.form.controls.photos.push(new FormGroup({
        url: new FormControl(x.url) as any,
        id: new FormControl(x.id) as any,
      }));
    });

    effect(() => {
      this.brandOptions = this.brandsService.options();

      if (this.brandOptions.length > 0) {
        this.form.controls.brand.patchValue(this.brandOptions[0]);
      }
    });
  }

  deletePhoto(idx: number) {
    this.form.controls.photos.removeAt(idx);
  }

  addPhoto() {
    const formGroup = new FormGroup<PhotoType>({
      url: new FormControl(null) as any,
      id: new FormControl(null) as any,
    });

    debugger;
    this.form.controls.photos.push(formGroup);
    this.form.updateValueAndValidity();
  }

  onSubmit() {
    debugger;

    this.submitted.set(true);

    this.service.create(this.form.value).subscribe({
      next: (response: Vehicle) => {
        this.submitted.set(false);
      },
      error: (error: BadRequest) => {
        this.error.set(error);
      }
    })
  }

  optionChanged(option: string) {
    const value: SelectOption = JSON.parse(option);
    this.form.controls.brand.patchValue(new SelectOption({...value}));
  }
}
