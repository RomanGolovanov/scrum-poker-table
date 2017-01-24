(function() {
    window.angular
        .module("ScrumPokerTable")
        .controller("NewDeskController", [
            "$scope", "$location", "DeskHubService", function($scope, $location, deskHubService) {

                $scope.deskTypes = {
                    "Natural numbers": ["?", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "21", "28", "35", "42"],
                    "Fibonacchi numbers": ["?", "1", "2", "3", "5", "8", "13", "20", "40"]
                };
                $scope.state = {
                    deskType: "Natural numbers"
                };

                $scope.connect = function(deskName) {
                    deskHubService.getDesk(deskName).then(function () {
                        $location.path("/desk/" + deskName);
                    }, function(error) {
                         console.error(error);
                    });
                };

                $scope.create = function() {
                    deskHubService.createDesk($scope.deskTypes[$scope.state.deskType]).then(function (deskName) {
                        $location.path("/desk/" + deskName);
                    }, function(error) {
                         console.error(error);
                    });
                };
            }
        ]);
})();