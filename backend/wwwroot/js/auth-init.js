var applicationConfig = {
  clientID: '0cd9ecf8-f3ec-475e-8882-8292b40e7516',
  graphScopes: ["openid"]
};

var currentToken = '';

var logger = new Msal.Logger(loggerCallback, { level: Msal.LogLevel.Verbose, correlationId: '12345' }); // level and correlationId are optional parameters.
//Logger has other optional parameters like piiLoggingEnabled which can be assigned as shown aabove. Please refer to the docs to see the full list and their default values.

function loggerCallback(logLevel, message, piiLoggingEnabled) {
  console.log(message);
}

var userAgentApplication = new Msal.UserAgentApplication(applicationConfig.clientID, null, authCallback, { logger: logger, cacheLocation: 'localStorage' }); //logger and cacheLocation are optional parameters.
//userAgentApplication has other optional parameters like redirectUri which can be assigned as shown above.Please refer to the docs to see the full list and their default values.
function authCallback(errorDesc, token, error, tokenType) {
  if (token) {
  }
  else {
    log(error + ":" + errorDesc);
  }
}

function log(item) {
  console.log(item);
}

function msLogin() {
  return userAgentApplication.loginPopup(applicationConfig.graphScopes).then(function (idToken) {
    //Login Success
    log('id: ' + idToken);
    currentToken = idToken;
    userAgentApplication.acquireTokenSilent(applicationConfig.graphScopes).then(function (accessToken) {
      log('at: ' + idToken);
      currentToken = accessToken;

    }, function (error) {
      //AcquireToken Failure, send an interactive request.
      userAgentApplication.acquireTokenPopup(applicationConfig.graphScopes).then(function (accessToken) {
        log('at: ' + accessToken);
        currentToken = accessToken;
      }, function (error) {
        console.log(error);
      });
    })
  }, function (error) {
    console.log(error);
    });
}

//fixme
if (currentToken != '') {
  userAgentApplication.loginPopup(["openid"]).then(function (idToken) {
    //Login Success
    log('id2: ' + idToken);
    currentToken = idToken;

  });
}

