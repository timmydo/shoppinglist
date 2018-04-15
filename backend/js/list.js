(function () {
  var auth = require('./auth');
  var util = require('./util');
  var $ = require('jquery');


  function getList(retry) {
    $.ajax({
      type: "GET",
      url: "/api/v1/me",
      headers: {
        'Authorization': 'Bearer ' + auth.getToken()
      },
      statusCode: {
        401: function (xhr) {
          log('retry on 400');
          auth.clearToken();
          auth.initAuth();
          if (retry) {
            getList(false);
          }
        }
      }
    }).done(function (data) {
      util.log(data);
    });
  }

  module.exports.getList = getList;

})();