export abstract class BaseModel {
  id: number;

  protected constructor(id: number = 0) {
    this.id = id;
  }
}
