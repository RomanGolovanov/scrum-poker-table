(function(){
  angular
    .module("ScrumPokerTable")
    .controller("ConnectMasterController", ["$scope", "$location", "DeskService", function($scope, $location, deskService){

        $scope.desk_id = "";

        $scope.connect = function(){

            if($scope.desk_id === ""){
                return;
            }

            deskService.get($scope.desk_id).then(function(){
                $location.path("/master/" + $scope.desk_id);
            });
        }
    }])


    ;
})();