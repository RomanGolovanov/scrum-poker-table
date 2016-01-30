(function() {
    angular
        .module("ScrumPokerTable")
        .directive("deskView", [
            function() {
                return {
                    restrict: "E",
                    scope: {
                        desk: "=",
                        user: "="
                    },
                    templateUrl: "Client/directives/deskview.html",
                    link: function link(scope, element, attr) {

                        scope.$watch("desk", function(desk, oldDesk) {

                            if (!desk) return;

                            scope.max = desk.users
                                .map(function(p) { return p.card })
                                .filter(function(c) { return c != null && c != "?" })
                                .map(function(c) { return parseInt(c) })
                                .reduce(function(a, c) { return c > a ? c : a }, 0);
                            scope.min = desk.users
                                .map(function(p) { return p.card })
                                .filter(function(c) { return c != null && c != "?" })
                                .map(function(c) { return parseInt(c) })
                                .reduce(function(a, c) { return c < a ? c : a }, 99);

                            scope.users = desk.users;
                            scope.complete = desk.state = 1;
                            scope.stateName = { 0: "Vote in progress...", 1: "Vote result" }[desk.state];

                        }, true);

                        scope.show = function(user) {
                            return scope.complete || (scope.user && user.name.toLowerCase() === scope.user.toLowerCase());
                        }

                        scope.getCardStyle = function(user) {
                            if (!scope.complete) {
                                return "";
                            }

                            if (parseInt(user.card) === scope.min) {
                                return { "background-color": "#8f8" };
                            }
                            if (parseInt(user.card) === scope.max) {
                                return { "background-color": "#f88" };
                            }
                        }

                    }

                };
            }
        ]);
})();