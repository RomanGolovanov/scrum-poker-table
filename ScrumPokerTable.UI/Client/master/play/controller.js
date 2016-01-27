(function(){

  function orderByName(p1, p2){
    if (p1.name > p2.name) {
        return 1;
    }
    if (p1.name < p1.name) {
        return -1;
    }
    return 0;
  }

  function createDeskReport(desk){
    if(!desk || !desk.players) return "";

    var header =
        "Desk ID: " + desk.desk_id + "\n"+
        "Time: " + new Date(Math.round(desk.modified * 1000)) + "\n" +
        "Players:";

    return desk
        .players
        .sort(orderByName)
        .reduce(function(p, c){ return p + "\n\t" + c.name.capitalize() + ": " + (c.card ? c.card : "?"); }, header);
  }

  angular
    .module("ScrumPokerTable")
    .controller("MasterController", ["$scope", "$routeParams", "$location", "$timeout", "DeskService",
    function($scope, $routeParams, $location, $timeout, deskService){

        $scope.desk_id = $routeParams.desk_id;
        $scope.desk_url = $location.absUrl();
        $scope.desk_report = "";

        $scope.startGame = function(){
            deskService.start($scope.desk_id);
        }

        $scope.finishGame = function(){
            deskService.finish($scope.desk_id);
        }

        $scope.copySuccess = function(){
            alert("Copied!");
        }

        $scope.copyFail = function(err){
            alert("Copy to clipboard: \n" + $scope.desk_report);
            console.error('Error!', err);
        }

        var disposed = false;
        $scope.$on("$destroy", function () { disposed = true; });

        function updateDeskAsync(){
            timeout = $scope.desk ? 10 : null;
            modified = $scope.desk ? $scope.desk.modified : null;
            deskService.get($scope.desk_id, modified, timeout).then(function(desk){
                $scope.desk = desk;
                $scope.desk_report = createDeskReport(desk);

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