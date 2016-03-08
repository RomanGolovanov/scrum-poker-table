(function() {
    window.angular
        .module("ScrumPokerTable")
        .controller("PlayerController", [
            "$scope", "$routeParams", "$location", "DeskHubService",
            function($scope, $routeParams, $location, deskHubService) {

                $scope.deskName = $routeParams.desk_id;
                $scope.userName = $routeParams.player_id;
                $scope.selectedCard = null;
                
                $scope.setUserCard = function (card) {
                    deskHubService.setUserCard($scope.deskName, $scope.userName, card);
                }

                $scope.getCardStyle = function(card) {
                    if (card !== $scope.selectedCard) {
                        return "";
                    }
                    return { "background-color": "#8f8" };
                }

                $scope.showResults = function() {
                    return $scope.desk != null && $scope.desk.state !== 0;
                }

                $scope.showVoteSelection = function() {
                    return $scope.desk != null && $scope.desk.state === 0;
                }

                function onDeskChanged(desk) {
                    $scope.desk = desk;
                    var user = desk.users.filter(function (u) {
                        return (u.name || "").toLowerCase() === $scope.userName.toLowerCase();
                    })[0];
                    $scope.selectedCard = user.card;
                }

                deskHubService.getDesk($scope.deskName).then(function (desk) {
                    onDeskChanged(desk);
                    $scope.$on("$destroy", deskHubService.runDeskChangePolling($scope.deskName, onDeskChanged));
                }, function (error) {
                    console.error(error);
                    if (error.status === 404) {
                        $location.path("/player");
                    }
                });
            }
        ]);
})();