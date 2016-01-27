(function(){
  angular
    .module("ScrumPokerTable")
    .controller("NewDeskController", ["$scope", "$location", "DeskService", function($scope, $location, deskService){

        $scope.desk_types = {
            "Natural numbers" : ["?", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15"],
            "Fibonacchi numbers" : ["?", "1", "2", "3", "5", "8", "13", "20", "40"]
        };
        $scope.state = {
            desk_type: "Fibonacchi numbers"
        };

        $scope.connect = function(desk_id){
            deskService.connect(desk_id).then(function(desk_id){
                if(!desk_id) return;
                $location.path("/desk/" + desk_id);
            });
        }
        $scope.create = function(){
            deskService.create($scope.desk_types[$scope.state.desk_type]).then(function(desk_id){
                $location.path("/desk/" + desk_id);
            });
        }
    }])
  ;
})();