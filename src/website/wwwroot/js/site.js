// Write your Javascript code.
angular.module('sarDatabase', ['ngMessages', 'ngMaterial'])
  .config(function ($mdThemingProvider) {
    //http://mcg.mbitson.com/
    $mdThemingProvider.definePalette('sar-yellow', {
      '50': '#ffffff',
      '100': '#fce7c6',
      '200': '#f9d190',
      '300': '#f6b44c',
      '400': '#f4a82f',
      '500': '#f39c12',
      '600': '#db8b0b',
      '700': '#be780a',
      '800': '#a16608',
      '900': '#845307',
      'A100': '#ffffff',
      'A200': '#fce7c6',
      'A400': '#f4a82f',
      'A700': '#be780a',
      'contrastDefaultColor': 'light',
      'contrastDarkColors': '50 100 200 300 400 A100 A200 A400 A700'
    })
    .definePalette('sar-green', {
      '50': '#97ffb1',
      '100': '#4bff77',
      '200': '#13ff4d',
      '300': '#00ca32',
      '400': '#00ac2b',
      '500': '#008d23',
      '600': '#006e1b',
      '700': '#005014',
      '800': '#00310c',
      '900': '#001305',
      'A100': '#97ffb1', // hue-1
      'A200': '#008d23', // 
      'A400': '#00ac2b', // hue-2
      'A700': '#005014', // hue-3
      'contrastDefaultColor': 'light',
      'contrastDarkColors': '50 100 200 300 A100 A200'
    });;

    $mdThemingProvider.theme('default')
    .primaryPalette('sar-yellow')
    .accentPalette('sar-green')
    .backgroundPalette('grey');
  })

  .run(['$http', function ($http) {
    var form = document.getElementById("antiForgery");
    if (form) {
      $http.defaults.headers.common['__RequestVerificationToken'] = form.getElementsByTagName('input')[0].value;
    }
  }])


.controller("HeaderCtrl", ['$scope', function($scope) {
  angular.extend($scope, {
    openLoginMenu: function ($mdOpenMenu, ev) {
      originatorEv = ev;
      $mdOpenMenu(ev);
    },
    login: function (ev) {
      alert('start login');
    }
  });
}])

.controller("Account_RegisterCtrl", ['$scope', '$timeout', '$http', function ($scope, $timeout, $http) {
  angular.extend($scope, {
    step: 1,
    disabledStep: function(number) { return $scope.step != number || $scope.working },
    step1: {
      email: null,
      mode: 'member'
    },
    sendCode: function () {
      if ($scope.accountInfoForm.$invalid) return false;

      $scope.working = true;
      $http.post(window.appRoot + "api/account/sendverificationcode", { email: $scope.step1.email, invite: $scope.step1.invite })
      .then(function (response) {
        $scope.step = 2;
      })
      ['finally'](function () { $scope.working = false; });
    },
    verifyCode: function () {
      if ($scope.verifyForm.$invalid) return false;

      $scope.working = true;
      $timeout(function () {
        $scope.step = 2;
        $scope.working = false;
      }, 3000);
    }
  });
}])

;