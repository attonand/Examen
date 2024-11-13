import { SelectOption } from "src/app/models/selectOption";

export class Vehicle {
  id: number | null = null;
  model: string | null = null;
  year: number | null = null;
  color: string | null = null;
  brand: SelectOption | null = null;

  constructor(init?: Partial<Vehicle>) {
    Object.assign(this, init);
  }
}
