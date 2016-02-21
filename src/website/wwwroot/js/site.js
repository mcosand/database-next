// Write your Javascript code.
angular.module('sarDatabase', ['ngMaterial'])

.controller("HeaderCtrl", ['$scope', function($scope) {
  angular.extend($scope, {
    openLoginMenu: function ($mdOpenMenu, ev) {
      originatorEv = ev;
      $mdOpenMenu(ev);
    }
  });
}])

;