(function(){
  angular
    .module("ScrumPokerTable")
    .factory("PlayerService", ["$http", "$q", function($http, $q){
        return {
            connect: function(desk_id, player_id){
                return $http.post("api/desk/" + desk_id + "/player/" + player_id);
            },
            send: function(desk_id, player_id, card){
                return $http.put("api/desk/" + desk_id + "/player/" + player_id, { card: card });
            }
        };
    }])
  ;
})();