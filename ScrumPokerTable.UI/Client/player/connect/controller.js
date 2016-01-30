(function() {
    angular
        .module("ScrumPokerTable")
        .controller("ConnectPlayerController", [
            "$scope", "$routeParams", "$location", "DeskHubService", function($scope, $routeParams, $location, deskHubService) {

                if ($routeParams.desk_id) {
                    $scope.deskName = $routeParams.desk_id;
                    $scope.deskNameLocked = true;
                } else {
                    $scope.deskName = "";
                    $scope.deskNameLocked = false;
                }
                $scope.userName = "";
                $scope.joinAsUser = function() {
                    if ($scope.deskName === "") {
                        return;
                    }
                    if ($scope.userName === "") {
                        return;
                    }
                    deskHubService.joinAsUser($scope.deskName, $scope.userName).then(function() {
                        $location.path("/player/" + $scope.deskName + "/" + $scope.userName);
                    });
                }
            }
        ]);
})();