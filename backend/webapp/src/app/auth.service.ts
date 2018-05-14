import { UserAgentApplication, Logger, LogLevel } from 'msal';
import { } from './util';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

function loggerCallback(logLevel, message, piiLoggingEnabled) {
  console.log(message);
}

@Injectable()
export class AuthService {

  userAgentApplication: UserAgentApplication;
  applicationConfig: { clientID: string; graphScopes: string[]; };
  hashPrefix: string;

  constructor() {
    this.hashPrefix = '#id_token=';

    this.applicationConfig = {
      clientID: '0cd9ecf8-f3ec-475e-8882-8292b40e7516',
      graphScopes: ["user.read"]
    };

    var logger = new Logger(loggerCallback, { level: LogLevel.Verbose, correlationId: '12345' }); // level and correlationId are optional parameters.

    this.userAgentApplication = new UserAgentApplication(
      this.applicationConfig.clientID,
      null, //'https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad',
      (a, b, c, d) => this.authCallback(a,b,c,d),
      { logger: logger, cacheLocation: 'localStorage' }); //logger and cacheLocation are optional parameters.
    //userAgentApplication has other optional parameters like redirectUri which can be assigned as shown above.Please refer to the docs to see the full list and their default values.

    this.checkLocationForToken();
  }

  checkLocationForToken(): any {
    if (this.userAgentApplication.isCallback(window.location.hash)) {
      if (window.location.hash.startsWith(this.hashPrefix)) {
        this.setToken(window.location.hash.substring(this.hashPrefix.length));
        window.location.hash = '';
      }
    }
  }

  authCallback(errorDesc, token, error, tokenType) {
    if (tokenType !== "id_token") {
      return;
    }

    if (token) {
      console.log("authCallback " + token);
      this.setToken(token);
    }
    else {
      console.log("authCallback: " + error + ":" + errorDesc);
    }
  }

  acquireToken() {
    return new Observable<string>((evt) => {
      if (this.needsToken()) {
        console.log('needs token');
        this.checkLocationForToken();

        if (this.needsToken()) {
          console.log('needs token, not in url');
          this.userAgentApplication.loginRedirect(this.applicationConfig.graphScopes);
        }
      }

      console.log('found: ' + this.getToken());
      evt.next(this.getToken());
    });
  }

  getToken() {
    return localStorage.getItem("sltok");
  }

  setToken(tok) {
    console.log('set token: ' + tok);
    return localStorage.setItem("sltok", tok);
  }

  clearToken() {
    console.log('clear token');
    return this.setToken('');
  }

  needsToken() {
    return !this.getToken();
  }

}


