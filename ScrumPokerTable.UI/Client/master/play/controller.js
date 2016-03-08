(function() {

    function orderByName(p1, p2) {
        if (p1.name > p2.name) {
            return 1;
        }
        if (p1.name < p2.name) {
            return -1;
        }
        return 0;
    }

    function createDeskReport(desk) {
        if (!desk || !desk.users) return "";

        var header =
            "Desk ID: " + desk.name + "\n" +
                "Time: " + desk.timestamp + "\n" +
                "Players:";

        return desk
            .users
            .sort(orderByName)
            .reduce(function(p, c) { return p + "\n\t" + c.name.capitalize() + ": " + (c.card ? c.card : "?"); }, header);
    }

    window.angular
        .module("ScrumPokerTable")
        .controller("MasterController", [
            "$scope", "$routeParams", "$location", "DeskHubService",
            function($scope, $routeParams, $location, deskHubService) {

                $scope.deskName = $routeParams.desk_id;
                $scope.deskReport = "";

                $scope.beginVote = function () {
                    deskHubService.setDeskState($scope.deskName, 0);
                }

                $scope.endVote = function () {
                    deskHubService.setDeskState($scope.deskName, 1);
                }

                $scope.copySuccess = function() {
                    console.log("desk copied");
                    alert("Copy to clipboard: \n" + $scope.deskReport);
                }

                $scope.copyFail = function(err) {
                    console.error(err);
                    alert("Copy to clipboard: \n" + $scope.deskReport);
                }

                function onDeskChanged(desk) {
                    $scope.desk = desk;
                    $scope.deskReport = createDeskReport(desk);
                }

                deskHubService.getDesk($scope.deskName).then(function (desk) {
                    onDeskChanged(desk);
                    $scope.$on("$destroy", deskHubService.runDeskChangePolling($scope.deskName, onDeskChanged));
                }, function (error) {
                    console.error(error);
                    if (error.status === 404) {
                        $location.path("/master");
                    }
                });

            }
        ]);
})();