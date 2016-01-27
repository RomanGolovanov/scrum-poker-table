(function(){
  angular
    .module("ScrumPokerTable")

    .factory("DeskPollingService", ["$http", "$timeout", "$q", function($http, $timeout, $q){
        var poll = function(http, tick){
           return http.then(function(r){
                var deferred = $q.defer();
                $timeout(function(){ deferred.resolve(r); }, tick);
                return deferred.promise;
            });
        };

        return{
            poll: poll
        };
    }])

    .factory("DeskService", ["$http", "$q", function($http, $q){
        return {
            get: function(desk_id, modified, timeout){

                var config = {
                    headers:{}
                };
                if(timeout) {
                    config.headers["X-Polling-Timeout"] = timeout;
                    config.timeout = timeout * 1200;
                }
                if(modified) config.headers["X-Modified"] = modified;

                return $http
                    .get("api/desk/" + desk_id + "?rnd" + (new Date().getTime()), config)
                    .then(function(response){
                        return response.data;
                    });
            },

            connect: function(desk_id){
                return $http.get("api/desk/" + desk_id).then(function(response){
                    return response.data.desk_id;
                });
            },

            create: function(desk_cards){
                return $http.post("api/desk", {cards: desk_cards}).then(function(response){
                    return response.data.desk_id;
                });
            },

            start: function(desk_id){
                return $http.post("api/desk/start/" + desk_id, {});
            },

            finish: function(desk_id){
                return $http.post("api/desk/finish/" + desk_id, {});
            },

            remove: function(desk_id){
                return $http.delete("api/desk/" + desk_id).then(function(response){
                    return response.data.desk_id;
                });
            },

            get_history: function(desk_id){
                return $http
                    .get("api/desk/history/" + desk_id + "?rnd" + new Date().getTime())
                    .then(function(response){
                        return response.data;
                    });
            }
        };
    }])
  ;
})();