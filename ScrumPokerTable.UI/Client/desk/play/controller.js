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

                function onDeskChanged(desk) {
                    $scope.desk = desk;
                }

                deskHubService.getDesk($scope.deskName).then(function (desk) {
                    onDeskChanged(desk);
                    $scope.$on("$destroy", deskHubService.runDeskChangePolling($scope.deskName, onDeskChanged));
                }, function (error) {
                    console.error(error);
                    if (error.status === 404) {
                        $location.path("/desk");
                    }
                });


            }
        ]);
})();
