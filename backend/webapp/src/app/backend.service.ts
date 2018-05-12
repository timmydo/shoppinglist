import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';

import { MessageService } from './message.service';

import { UserResponse, ListResponse, UserRequest, ApplicationState, ListRequest, ListDescriptorObject, ListAndItems, ListItemObject, MarkRequest, MarkRequestState, MarkResponse, ListData, MarkResponseReasonCode } from './models';
import { AuthService } from './auth.service';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class BackendService {
  dataStore: { state: ApplicationState; };

  private meUrl = '/api/v1/me';
  private listUrl = '/api/v1/list';
  private userUrl = '/api/v1/user';
  private localStorageStateKey = 'backend_state';
  private _state: BehaviorSubject<ApplicationState>;

  constructor(private http: HttpClient, private auth: AuthService, private messageService: MessageService) {
    let initialString = localStorage.getItem(this.localStorageStateKey);
    let initial: ApplicationState = {
      lists: [],
      pendingMarks: [],
    };

    if (initialString) {
      initial = JSON.parse(initialString);
    }

    if (!initial.lists) {
      initial.lists = [];
    }

    if (!initial.pendingMarks) {
      initial.pendingMarks = [];
    }

    this._state = <BehaviorSubject<ApplicationState>>new BehaviorSubject(initial);
    this.dataStore = { state: JSON.parse(JSON.stringify(initial)) };
  }

  getState(): Observable<ApplicationState> {
    return this._state.asObservable();
  }

  private saveState(): void {
    localStorage.setItem(this.localStorageStateKey, JSON.stringify(this.dataStore.state));
  }

  fetch(): void {
    this.http.get<UserResponse>(this.meUrl)
      .subscribe(user => {
        this.mergeUserToState(user);
        this.saveState();
        this._state.next(this.dataStore.state);
        if (user.l) {
          let lr = new ListRequest(user.l.map(li => li.id), []);
          this.listRequest(lr);
        }
      },
        catchError(this.handleError<UserResponse>('getUser'))
      );
  }

  private mergeUserToState(user: UserResponse): void {
    this.log('mergeUserToState');
    this.log(user);
    let current = this.dataStore.state.lists;
    this.dataStore.state.lists = user.l.map(li => new ListAndItems(new ListDescriptorObject(li.id, li.n), this.getCurrentItemsOrEmpty(li.id, current)));
    this.log('datastore state');
    this.log(this.dataStore.state);
  }

  private getCurrentItemsOrEmpty(id: string, array: ListAndItems[]): ListItemObject[] {
    let match = array.find(ai => ai.list.id == id);
    if (match && match.items && match.items.map) {
      return match.items.map(li => new ListItemObject(li.n, li.s));
    }

    return [];
  }

  private processFetchList(ld: ListData): void {
    this.dataStore.state.lists = this.dataStore.state.lists.map((li) => {
      if (ld.id == li.list.id) {
        return new ListAndItems(li.list, ld.i);
      } else {
        return li;
      }
    });
  }

  private mergeListToState(list: ListResponse): any {
    let failedWrites = list.m.filter((mr) => mr.c == MarkResponseReasonCode.WriteFailed).map((mr) => mr.id);
    this.dataStore.state.pendingMarks = this.dataStore.state.pendingMarks.filter((val) => {
      return failedWrites.indexOf(val.r) >= 0
    });
    list.l.forEach((lr) => this.processFetchList(lr));
    this.saveState();
  }

  private newId(): string {
    return Math.random().toString(36).substring(2);
  }

  private listRequest(body: ListRequest): void {
    let marksToMake = this.dataStore.state.pendingMarks.concat(body.m);
    body.m = marksToMake;
    this.saveState();
    this.http.post<ListResponse>(this.listUrl, body)
      .subscribe(response => {
        this.mergeListToState(response);
        this.saveState();
        this._state.next(this.dataStore.state);
      },
        catchError(this.handleError<UserResponse>('getUser'))
      );
  }

  private userRequest(body: UserRequest): void {
    this.http.post<UserResponse>(this.userUrl, body)
      .subscribe(response => {
        this.mergeUserToState(response);
        this.saveState();
        this._state.next(this.dataStore.state);
      },
        catchError(this.handleError<UserResponse>('getUser'))
      );
  }


  addList(shareName: string): void {
    var id = this.newId();
    if (shareName) {
      id = shareName;
    }
    var newList = new ListDescriptorObject(id, '');
    var userRequest = new UserRequest([newList], []);
    this.userRequest(userRequest);
  }

  addItem(listId: string, title: string): void {
    var listItem = new ListItemObject(title, MarkRequestState.Active);
    var mark = new MarkRequest(this.newId(), listId, listItem);
    var req = new ListRequest([listId], [mark]);
    this.listRequest(req);
  }

  renameList(id: string, name: string): any {
    var listToRename = new ListDescriptorObject(id, name);
    var userRequest = new UserRequest([listToRename], [listToRename]);
    this.userRequest(userRequest);
  }

  deleteList(id: string): any {
    var listToDelete = new ListDescriptorObject(id, '');
    var userRequest = new UserRequest([], [listToDelete]);
    this.userRequest(userRequest);
  }


  changeItemState(listId: string, title: string, state: MarkRequestState): void {
    var listItem = new ListItemObject(title, state);
    var mark = new MarkRequest(this.newId(), listId, listItem);
    var req = new ListRequest([listId], [mark]);
    this.listRequest(req);
  }

  /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  private log(message: any) {
    this.messageService.add('BackendService: ' + JSON.stringify(message));
  }
}
