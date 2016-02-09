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

                $scope.canVote = function() {
                    return $scope.desk != null && $scope.desk.state === 0;
                }

                deskHubService.getDesk($scope.deskName).then(function (desk) {
                    $scope.desk = desk;
                    var user = desk.users.filter(function (u) {
                        return (u.name || "").toLowerCase() === $scope.userName.toLowerCase();
                    })[0];
                    $scope.selectedCard = user.card;
                }, function (error) {
                    console.error(error);
                    if (error.status === 404) {
                        $location.path("/player");
                    }
                });

                $scope.$on("deskChanged", function (event, desk) {
                    $scope.desk = desk;
                    var user = desk.users.filter(function (u) {
                        return (u.name || "").toLowerCase() === $scope.userName.toLowerCase();
                    })[0];
                    $scope.selectedCard = user.card;
                });

                $scope.$on("deskHubConnected", function() {
                    deskHubService.joinAsUser($scope.deskName, $scope.userName);
                });

                $scope.$on("deskHubConnectionState", function (event, state) {
                    if (state === "disconnected" || state === "error") {
                        deskHubService.reconnect();
                    };
                });

                $scope.$on("$destroy", function () {
                    if (deskHubService.hasConnection()) {
                        deskHubService.leave($scope.deskName);
                    }
                });

                if (deskHubService.hasConnection()) {
                    deskHubService.joinAsUser($scope.deskName, $scope.userName);
                }
            }
        ]);
})();