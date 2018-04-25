import { UserAgentApplication, Logger, LogLevel } from 'msal';
import { } from './util';
import { Injectable } from '@angular/core';

function loggerCallback(logLevel, message, piiLoggingEnabled) {
  console.log(message);
}

@Injectable()
export class AuthService {
  userAgentApplication: UserAgentApplication;
  applicationConfig: { clientID: string; graphScopes: string[]; };

  constructor() {
    this.applicationConfig = {
      clientID: '0cd9ecf8-f3ec-475e-8882-8292b40e7516',
      graphScopes: ["user.read"]
    };

    var logger = new Logger(loggerCallback, { level: LogLevel.Verbose, correlationId: '12345' }); // level and correlationId are optional parameters.

    this.userAgentApplication = new UserAgentApplication(
      this.applicationConfig.clientID,
      null, //'https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad',
      this.authCallback,
      { logger: logger, cacheLocation: 'localStorage' }); //logger and cacheLocation are optional parameters.
    //userAgentApplication has other optional parameters like redirectUri which can be assigned as shown above.Please refer to the docs to see the full list and their default values.

  }

  authCallback(errorDesc, token, error, tokenType) {
    if (token) {
      // do nothing?
    }
    else {
      console.log(error + ":" + errorDesc);
    }
  }

  acquireToken() {
    if (this.needsToken()) {
      this.userAgentApplication.loginPopup(this.applicationConfig.graphScopes).then(function (tok) {
        this.setToken(tok);
      });
    } else {
      console.log('found: ' + this.getToken());
    }
  }

  getToken() {
    return localStorage.getItem("sltok");
  }

  setToken(tok) {
    console.log('set token: ' + tok);
    return localStorage.setItem("sltok", tok);
  }

  clearToken() {
    return this.setToken('');
  }

  needsToken() {
    return !this.getToken();
  }

}


