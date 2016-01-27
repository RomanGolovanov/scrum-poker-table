(function(){
  angular
    .module("ScrumPokerTable")
    .controller("PlayerController", ["$scope", "$routeParams", "$location", "$timeout", "DeskService", "PlayerService",
    function($scope, $routeParams, $location, $timeout, deskService, playerService){

        $scope.desk_id = $routeParams.desk_id;
        $scope.player_id = $routeParams.player_id;
        $scope.selected_card = null;

        $scope.send = function(card){
            playerService.send($scope.desk_id, $scope.player_id, card);
        }

        $scope.getCardStyle = function(card){
            if(card !== $scope.selected_card){
                return "";
            }
            return { "background-color": "#8f8" };
        }

        var disposed = false;
        $scope.$on("$destroy", function () { disposed = true; });

        function updateDeskAsync(){
            timeout = $scope.desk ? 10 : null;
            modified = $scope.desk ? $scope.desk.modified : null;
            deskService.get($scope.desk_id, modified, timeout).then(function(desk){
                $scope.desk = desk;
                var player = desk.players.filter(function(p){ return p.name === $scope.player_id.toLowerCase() })[0];
                $scope.selected_card = player.card;
                if(!disposed) $timeout(updateDeskAsync, 10);

            }, function(response){
                if(response.status === 304){
                    if(!disposed) $timeout(updateDeskAsync, 10);
                }else{
                    if(!disposed) $timeout(updateDeskAsync, 1000);
                }
            });
        }
        updateDeskAsync();
    }])
  ;
})();