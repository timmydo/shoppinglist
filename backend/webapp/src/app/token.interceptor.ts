import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';
import { map } from 'rxjs/operators';
import { pipe } from 'rxjs/util/pipe';

import { HttpResponse, HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(public auth: AuthService) { }
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let token = this.auth.acquireToken();
    
    return new Observable<HttpEvent<any>>((evt) => {
      token.subscribe((token) => {
        let newRequest = request.clone({
          setHeaders: {
            Authorization: 'Bearer ' + token
          }
        });

        let obs = next.handle(newRequest).do((event: HttpEvent<any>) => {
          if (event instanceof HttpResponse) {
            //fixme todo
            console.log('fixme todo');
          }
        }, (err: any) => {
          if (err instanceof HttpErrorResponse) {
            if (err.status === 401) {
              console.log('clear token');
              this.auth.clearToken();
              this.auth.acquireToken();
            } else {
              console.log('unhandled error in token int');
            }
          }
        });

        obs.subscribe((val) => evt.next(val));
      });
    });
  }

}
