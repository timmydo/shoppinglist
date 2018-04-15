(function () {
  var Msal = require('msal');
  var util = require('./util');

  var applicationConfig = {
    clientID: '0cd9ecf8-f3ec-475e-8882-8292b40e7516',
    graphScopes: ["user.read"]
  };

  var logger = new Msal.Logger(loggerCallback, { level: Msal.LogLevel.Verbose, correlationId: '12345' }); // level and correlationId are optional parameters.
  //Logger has other optional parameters like piiLoggingEnabled which can be assigned as shown aabove. Please refer to the docs to see the full list and their default values.

  function loggerCallback(logLevel, message, piiLoggingEnabled) {
    util.log(message);
  }

  var userAgentApplication = new Msal.UserAgentApplication(
    applicationConfig.clientID,
    null, //'https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad',
    authCallback,
    { logger: logger, cacheLocation: 'localStorage' }); //logger and cacheLocation are optional parameters.
  //userAgentApplication has other optional parameters like redirectUri which can be assigned as shown above.Please refer to the docs to see the full list and their default values.
  function authCallback(errorDesc, token, error, tokenType) {
    if (token) {
      // do nothing?
    }
    else {
      util.log(error + ":" + errorDesc);
    }
  }


  function acquireToken() {
    userAgentApplication.acquireTokenSilent(applicationConfig.graphScopes).then(function (accessToken) {
      //setToken(accessToken);
    }, function (error) {
      //AcquireToken Failure, send an interactive request.
      util.log(error);
      userAgentApplication.acquireTokenPopup(applicationConfig.graphScopes).then(function (accessToken) {
        //setToken(accessToken);
      }, function (error) {
        util.log(error);
      });
    });
  }

  function initAuth() {
    if (needsToken()) {
      userAgentApplication.loginPopup(applicationConfig.graphScopes).then(function (tok) {
        setToken(tok);
      });
    } else {
      util.log('found: ' + getToken());
    }
  }

  function getToken() {
    return localStorage.getItem("sltok");
  }

  function setToken(tok) {
    util.log('set token: ' + tok);
    return localStorage.setItem("sltok", tok);
  }

  function clearToken(tok) {
    return setToken('');
  }

  function needsToken() {
    return !getToken();
  }

  module.exports.initAuth = initAuth;
  module.exports.getToken = getToken;

})();