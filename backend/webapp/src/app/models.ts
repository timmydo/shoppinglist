

export class UserRequest {
  a: ListDescriptorObject[];
  r: ListDescriptorObject[]
}

export class MarkRequest {
  r: string;
  l: string;
  i: ListItemObject;
}

export class ListRequest {
  g: string[];
  a: ListDescriptorObject[];
  m: MarkRequest[];
}

export class ListItemObject {
  n: string;
  s: number;
}
export enum MarkResponseReasonCode {
  None = 0,
  ListNotFound = 1,
  InvalidState = 2,
  MarkItemMissing = 3,
  WriteFailed = 4,
}

export class MarkResponse {
  id: string;
  s: boolean;
  c: MarkResponseReasonCode;
}

export class ListData {
  id: string;
  i: ListItemObject[];
}

export class ListResponse {
  public l: ListData[];
  public m: MarkResponse[];

}
export class ListDescriptorObject {
  id: string;
  n: string;
}

export class UserResponse {
  public l: ListDescriptorObject[];
}

export class ListAndItems {
  public list: ListDescriptorObject;
  public items: ListItemObject[];
}

export class ApplicationState {
  public lists: ListAndItems[];
}
