(function() {
    angular
        .module("ScrumPokerTable")
        .controller("PlayDeskController", [
            "$scope", "$routeParams", "$location", "DeskHubService",
            function($scope, $routeParams, $location, deskHubService) {

                $scope.deskName = $routeParams.desk_id;
                $scope.deskUrl = $location.absUrl();
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
                });

                $scope.$on("deskChanged", function(event, desk) {
                    $scope.desk = desk;
                });

                $scope.$on("deskHubConnected", function() {
                    deskHubService.joinAsMaster($scope.deskName);
                });
            }
        ]);
})();