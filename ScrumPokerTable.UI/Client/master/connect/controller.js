(function() {
    window.angular
        .module("ScrumPokerTable")
        .controller("ConnectMasterController", [
            "$scope", "$location", "DeskHubService", function($scope, $location, deskHubService) {
                $scope.deskName = "";
                $scope.connect = function() {
                    if ($scope.deskName === "") {
                        return;
                    }
                    deskHubService.getDesk($scope.deskName).then(function() {
                        $location.path("/master/" + $scope.deskName);
                    });
                }
            }
        ]);
})();