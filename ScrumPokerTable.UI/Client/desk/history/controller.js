(function() {

    function orderByName(p1, p2) {
        if (p1.name > p2.name) {
            return 1;
        }
        if (p1.name < p2.name) {
            return -1;
        }
        return 0;
    }

    window.angular
        .module("ScrumPokerTable")
        .controller("PlayDeskHistoryController", [
            "$scope", "$routeParams", "$location", "$timeout", "DeskService",
            function($scope, $routeParams, $location, $timeout, deskService) {

                $scope.desk_id = $routeParams.desk_id;
                $scope.desk_url = $location.absUrl();

                deskService.get_history($scope.desk_id).then(function(deskHistory) {
                    $scope.desk_history = deskHistory.map(function(h) {
                        h.modified = new Date(Math.round(h.modified * 1000)).toLocaleString();

                        var max = h.state
                            .map(function(p) { return p.card })
                            .filter(function(c) { return c != null && c !== "?" })
                            .map(function(c) { return parseInt(c) })
                            .reduce(function(a, c) { return c > a ? c : a }, 0);
                        var min = h.state
                            .map(function(p) { return p.card })
                            .filter(function(c) { return c != null && c !== "?" })
                            .map(function(c) { return parseInt(c) })
                            .reduce(function(a, c) { return c < a ? c : a }, 99);

                        h.state = h.state
                            .sort(orderByName)
                            .map(function(c) {

                                if (parseInt(c.card) === min) {
                                    c.styles = "label-success";
                                } else if (parseInt(c.card) === max) {
                                    c.styles = "label-danger";
                                } else {
                                    c.styles = "label-default";
                                }

                                c.name = c.name.capitalize();
                                c.card = (c.card ? c.card : "?");

                                return c;
                            });
                        return h;
                    });
                });
            }
        ]);
})();