

export class UserRequest {
  a: ListDescriptorObject[];
  r: ListDescriptorObject[]
  constructor(listsToAdd: ListDescriptorObject[], listsToRemove: ListDescriptorObject[] ) {
    this.a = listsToAdd;
    this.r = listsToRemove;
  }
}

export class MarkRequest {
  r: string;
  l: string;
  i: ListItemObject;

  constructor(requestId: string, listId: string, listItem: ListItemObject) {
    this.r = requestId;
    this.l = listId;
    this.i = listItem;
  }
}

export class ListRequest {
  g: string[];
  m: MarkRequest[];

  constructor(listsToGet: string[], marks: MarkRequest[]) {
    this.g = listsToGet;
    this.m = marks;
  }
}

export class ListItemObject {
  n: string;
  s: number;

  constructor(name: string, state: number) {
    this.n = name;
    this.s = state;
  }
}

export enum MarkRequestState {
  None = 0,
  Active = 1,
  Complete = 2,
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

  constructor(identifier: string, name: string) {
    this.id = identifier;
    this.n = name;
  }
}

export class UserResponse {
  public l: ListDescriptorObject[];
}

export class ListAndItems {
  public list: ListDescriptorObject;
  public items: ListItemObject[];
  public showCompleted: boolean;

  constructor(list: ListDescriptorObject, items: ListItemObject[], showCompleted: boolean) {
    this.list = list;
    this.items = items;
    this.showCompleted = showCompleted;
  }
}

export class ApplicationState {
  public lists: ListAndItems[];
  public pendingMarks: MarkRequest[];
}
