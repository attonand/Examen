import { BaseParams } from "./baseParams";

export class VehicleParms extends BaseParams {
  term: string | null = "";
  year: number | null = 0;

  constructor(init?: Partial<VehicleParms>) {
    super();
    Object.assign(this, init);
  }
}
