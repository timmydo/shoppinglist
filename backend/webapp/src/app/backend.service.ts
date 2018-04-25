import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';

import { MessageService } from './message.service';

import { UserResponse, ListResponse, UserRequest } from './models';
import { AuthService } from './auth.service';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class BackendService {

  private meUrl = 'api/v1/me';

  constructor(
    private http: HttpClient,
    private auth: AuthService,
    private messageService: MessageService) { }

  /** GET heroes from the server */
  getUser(): Observable<UserResponse> {
    return this.http.get<UserResponse>(this.meUrl)
      .pipe(
      tap(user => this.log(`fetched user`)),
      catchError(this.handleError<UserResponse>('getUser'))
      );
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

  /** Log a HeroService message with the MessageService */
  private log(message: string) {
    this.messageService.add('BackendService: ' + message);
  }
}
