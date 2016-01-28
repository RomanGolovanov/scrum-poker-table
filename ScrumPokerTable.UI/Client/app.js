
(function () {
    angular
      .module("ScrumPokerTable", ["ngRoute", "ui.bootstrap", "monospaced.qrcode", "angular-clipboard"])

      .config(["$routeProvider", function ($routeProvider) {

          $routeProvider
            .when("/desk", { templateUrl: "Client/desk/new/template.html", controller: "NewDeskController", reloadOnSearch: false })
            .when("/desk/:desk_id", { templateUrl: "Client/desk/play/template.html", controller: "PlayDeskController", reloadOnSearch: false })
            .when("/desk/history/:desk_id", { templateUrl: "Client/desk/history/template.html", controller: "PlayDeskHistoryController", reloadOnSearch: false })

            .when("/player/", { templateUrl: "Client/player/connect/template.html", controller: "ConnectPlayerController", reloadOnSearch: false })
            .when("/player/:desk_id", { templateUrl: "Client/player/connect/template.html", controller: "ConnectPlayerController", reloadOnSearch: false })
            .when("/player/:desk_id/:player_id", { templateUrl: "Client/player/play/template.html", controller: "PlayerController", reloadOnSearch: false })

            .when("/master/", { templateUrl: "Client/master/connect/template.html", controller: "ConnectMasterController", reloadOnSearch: false })
            .when("/master/:desk_id", { templateUrl: "Client/master/play/template.html", controller: "MasterController", reloadOnSearch: false })

            .when("/about", { templateUrl: "Client/about/about.html" })
            .otherwise("/desk");
      }])

      .controller("HeaderController", ["$scope", "$location", function ($scope, $location) {
          $scope.isActive = function (viewLocation) {
              return $location.path().startsWith(viewLocation);
          };
      }])
    ;
})();