(function() {
    window.angular
        .module("ScrumPokerTable")
        .controller("PlayerController", [
            "$scope", "$routeParams", "$location", "DeskHubService",
            function($scope, $routeParams, $location, deskHubService) {

                $scope.deskName = $routeParams.desk_id;
                $scope.userName = $routeParams.player_id;
                $scope.selectedCard = null;
                $scope.connected = deskHubService.hasConnection();
                
                $scope.setUserCard = function (card) {
                    deskHubService.setUserCard($scope.deskName, $scope.userName, card);
                }

                $scope.getCardStyle = function(card) {
                    if (card !== $scope.selectedCard) {
                        return "";
                    }
                    return { "background-color": "#8f8" };
                }

                $scope.showConnectionAlert = function() {
                    return !$scope.connected;
                }

                $scope.showResults = function() {
                    return $scope.desk != null && $scope.desk.state !== 0;
                }

                $scope.showVoteSelection = function() {
                    return $scope.connected && ($scope.desk != null && $scope.desk.state === 0);
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

                $scope.$on("deskHubConnectionState", function (event, state) {
                    $scope.connected = deskHubService.hasConnection();
                    if ($scope.connected) {
                        deskHubService.joinAsUser($scope.deskName, $scope.userName);
                    }
                });

                $scope.$on("$destroy", function () {
                    if (deskHubService.hasConnection()) {
                        deskHubService.leave($scope.deskName);
                    }
                });

                if ($scope.connected) {
                    console.log("Register as player");
                    deskHubService.joinAsUser($scope.deskName, $scope.userName);
                } else {
                    console.log("Desk not connected, wait....");
                }
            }
        ]);
})();