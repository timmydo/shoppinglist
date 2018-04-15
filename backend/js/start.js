
(function () {
  var auth = require('./auth');
  var list = require('./list');

  auth.initAuth();
  list.getList();
})();
