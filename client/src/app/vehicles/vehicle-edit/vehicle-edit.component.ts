import { Component, effect, inject, signal } from '@angular/core';
import { AbstractControl, FormArray, FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { createId } from '@paralleldrive/cuid2';
import { BadRequest } from 'src/app/models/badRequest';
import { SelectOption } from 'src/app/models/selectOption';
import { BrandsService } from 'src/app/services/brands.service';
import { VehiclesService } from 'src/app/services/vehicles.service';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Vehicle } from 'src/app/models/vehicle';
import { ToastService } from 'src/app/services/toast.service';
import { DomSanitizer } from '@angular/platform-browser';

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
  selector: 'app-vehicle-edit',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './vehicle-edit.component.html',
  styleUrl: './vehicle-edit.component.scss'
})
export class VehicleEditComponent {
  router = inject(Router);
  error = signal<BadRequest | null>(null);
  toastService = inject(ToastService);
  sanitizer = inject(DomSanitizer);
  id = createId();
  url: string | null = null;

  private brandsService = inject(BrandsService);
  service = inject(VehiclesService);
  route = inject(ActivatedRoute);

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
    const id = this.route.snapshot.paramMap.get('id');
    if(id !== null) {
      this.service.getById(parseInt(id)).subscribe({
        next: (data) => {
          data.photos.forEach(x => {
            this.form.controls.photos.push(new FormGroup({
              url: new FormControl(x.url) as any,
              id: new FormControl(x.id) as any,
            }));
          });

          this.form.patchValue(data);
        }
      });
    }
    
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

    this.form.controls.photos.push(formGroup);

    this.form.updateValueAndValidity();
  }

  onPhotoChange(url: string | null) {
    this.url = url;
  }


  onSubmit() {
    this.submitted.set(true);

    const id = this.route.snapshot.paramMap.get('id');
    if(id !== null) {
      this.service.update(parseInt(id), this.form.value).subscribe({
        next: (response: Vehicle) => {
          this.submitted.set(false);
          this.router.navigate(['/Vehicles', response.id]);
          this.toastService.add(this.sanitizer.bypassSecurityTrustHtml(`El <b>auto</b> con <b>ID ${response.id}</b> se guardó con éxito.`), "success");
        },
        error: (error: BadRequest) => {
          this.error.set(error);
          this.toastService.add(this.sanitizer.bypassSecurityTrustHtml(`Ocurrió un error al registrar el <b>auto</b>, inténtelo más tarde.`), "danger");  
        }
      });
    }
  }

  optionChanged(option: string) {
    const value: SelectOption = JSON.parse(option);
    this.form.controls.brand.patchValue(new SelectOption({...value}));
  }
}
