(function(){
  angular
    .module("ScrumPokerTable")
    .controller("ConnectPlayerController", ["$scope", "$routeParams", "$location", "PlayerService", function($scope, $routeParams, $location, playerService){

        if($routeParams.desk_id){
            $scope.desk_id = $routeParams.desk_id;
            $scope.desk_id_locked = true;
        }else{
            $scope.desk_id = "";
            $scope.desk_id_locked = false;
        }

        $scope.player_id = "";

        $scope.connect = function(desk_id, player_id){

            if($scope.desk_id === ""){
                return;
            }

            if($scope.player_id === ""){
                return;
            }

            playerService.connect($scope.desk_id, $scope.player_id).then(function(){
                $location.path("/player/" + $scope.desk_id + "/" + $scope.player_id);
            });
        }
    }])


    ;
})();