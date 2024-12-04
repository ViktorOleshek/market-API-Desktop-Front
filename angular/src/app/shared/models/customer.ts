import {BaseModel} from './base.model';

export class Customer extends BaseModel {
  name: string;
  surname: string;
  birthDate: Date;
  discountValue: number;
  receiptsIds: number[];

  constructor(
    id: number = 0,
    name: string = '',
    surname: string = '',
    birthDate: Date = new Date(),
    discountValue: number = 0,
    receiptsIds: number[] = []
  ) {
    super(id);
    this.name = name;
    this.surname = surname;
    this.birthDate = birthDate;
    this.discountValue = discountValue;
    this.receiptsIds = receiptsIds;
  }
}
