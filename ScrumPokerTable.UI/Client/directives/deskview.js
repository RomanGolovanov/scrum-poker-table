(function(){
  angular
    .module("ScrumPokerTable")
	.directive("deskView",[function(){
		return {
			restrict: "E",
			scope: {
			    desk: "=",
			    player: "="
			},
            templateUrl: "Client/directives/deskview.html",
			link: function link(scope, element, attr) {

                scope.$watch("desk", function(desk, oldDesk) {
                    if(!desk) return;

                    scope.desk_id = desk.desk_id;

                    players = desk.players;
                    scope.max = desk.players
                        .map(function(p){ return p.card })
                        .filter(function(c){ return c!=null && c!="?" })
                        .map(function(c){ return parseInt(c) })
                        .reduce(function(a,c){ return c > a ? c : a }, 0);
                    scope.min = desk.players
                        .map(function(p){ return p.card })
                        .filter(function(c){ return c!=null && c!="?" })
                        .map(function(c){ return parseInt(c) })
                        .reduce(function(a,c){ return c < a ? c : a }, 99);

                    scope.stateName = {
                        1: "Waiting for players...",
                        2: "Vote in progress...",
                        3: "Vote result"
                    }[desk.state];
                    scope.players = desk.players;
                    scope.complete = desk.state == 3

                }, true);

                scope.show = function(player){
                    return scope.complete || (scope.player && player.name.toLowerCase() === scope.player.toLowerCase());
                }

                scope.getCardStyle = function(player){
                    if(!scope.complete){
                        return "";
                    }

                    if(parseInt(player.card) === scope.min){
                        return { "background-color": "#8f8" };
                    }
                    if(parseInt(player.card) === scope.max){
                        return { "background-color": "#f88" };
                    }
                }

			}

		};
	}])

  ;
})();