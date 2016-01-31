(function() {
    window.angular
        .module("ScrumPokerTable")
        .controller("PlayDeskController", [
            "$scope", "$routeParams", "$location", "DeskHubService",
            function($scope, $routeParams, $location, deskHubService) {

                $scope.deskName = $routeParams.desk_id;
                $scope.deskUrl = $location.absUrl().replace("/desk/","/player/");
                $scope.desk = null;

                $scope.deleteDesk = function() {
                    deskHubService.deleteDesk($scope.deskName);
                    $location.path("/desk?stopped");
                }

                $scope.joinAsUser = function() {
                    $location.path("/player/" + $scope.deskName);
                }

                $scope.joinAsMaster = function() {
                    $location.path("/master/" + $scope.deskName);
                }

                $scope.showHistory = function() {
                    $location.path("/desk/history/" + $scope.deskName);
                }

                deskHubService.getDesk($scope.deskName).then(function (desk) {
                    $scope.desk = desk;
                }, function (error) {
                    console.error(error);
                    if (error.status === 404) {
                        $location.path("/desk");
                    }
                });

                $scope.$on("deskChanged", function(event, desk) {
                    $scope.desk = desk;
                });

                $scope.$on("deskHubConnected", function() {
                    deskHubService.joinAsMaster($scope.deskName);
                });

                $scope.$on("$destroy", function() {
                    if (deskHubService.hasConnection()) {
                        deskHubService.leave($scope.deskName);
                    }
                });

                if (deskHubService.hasConnection()) {
                    deskHubService.joinAsMaster($scope.deskName);
                }
            }
        ]);
})();
