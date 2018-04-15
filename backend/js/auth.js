(function () {
  var Msal = require('msal');
  var $ = require('jquery');
  var applicationConfig = {
    clientID: '0cd9ecf8-f3ec-475e-8882-8292b40e7516',
    graphScopes: ["user.read"]
  };

  var logger = new Msal.Logger(loggerCallback, { level: Msal.LogLevel.Verbose, correlationId: '12345' }); // level and correlationId are optional parameters.
  //Logger has other optional parameters like piiLoggingEnabled which can be assigned as shown aabove. Please refer to the docs to see the full list and their default values.

  function loggerCallback(logLevel, message, piiLoggingEnabled) {
    console.log(message);
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
      log(error + ":" + errorDesc);
    }
  }

  function log(item) {
    console.log(item);
  }

  function acquireToken() {
    userAgentApplication.acquireTokenSilent(applicationConfig.graphScopes).then(function (accessToken) {
      //setToken(accessToken);
    }, function (error) {
      //AcquireToken Failure, send an interactive request.
      log(error);
      userAgentApplication.acquireTokenPopup(applicationConfig.graphScopes).then(function (accessToken) {
        //setToken(accessToken);
      }, function (error) {
        log(error);
      });
    });
  }

  function initAuth2() {
    var user = userAgentApplication.getUser();
    if (!user) {
      userAgentApplication.loginPopup(applicationConfig.graphScopes).then(function () {
        acquireToken();
      });
    } else {
      acquireToken();
    }
  }

  function initAuth() {
    if (needsToken()) {
      userAgentApplication.loginPopup(applicationConfig.graphScopes).then(function (tok) {
        setToken(tok);
      });
    } else {
      log('found: ' + getToken());
    }
  }

  function getToken() {
    return localStorage.getItem("sltok");
  }

  function setToken(tok) {
    log('set token: ' + tok);
    return localStorage.setItem("sltok", tok);
  }

  function clearToken(tok) {
    return setToken('');
  }

  function needsToken() {
    return !getToken();
  }

  function getList(retry) {
    $.ajax({
      type: "GET",
      url: "/api/v1/me",
      headers: {
        'Authorization': 'Bearer ' + getToken()
      },
      statusCode: {
        401: function (xhr) {
          log('retry on 400');
          clearToken();
          initAuth();
          if (retry) {
            getList(false);
          }
        }
      }
    }).done(function (data) {
      log(data);
    });
  }

	module.exports.getList = getList;
	module.exports.initAuth = initAuth;

})();