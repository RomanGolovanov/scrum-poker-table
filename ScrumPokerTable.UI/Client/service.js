(function () {
    window.angular
        .module("ScrumPokerTable")
        .factory("DeskHubService", [
            "$rootScope", "$q", "$http", "$timeout", "Hub", function($rootScope, $q, $http, $timeout, Hub) {

                var connected = false;
                var deskHub = new Hub("deskHub", {
                    listeners: {
                        "deskChanged": function (desk) {
                            console.log(desk);
                            $rootScope.$broadcast("deskChanged", desk);
                            $rootScope.$apply();
                        }
                    },

                    methods: [
                        "joinAsUser",
                        "joinAsMaster",
                        "leave",
                        "setUserCard",
                        "setDeskState"
                    ],

                    transport: ["longPolling"],
                    queryParams: { "api": "1.0" },
                    errorHandler: function(error) {
                        console.error(error);
                        $rootScope.$broadcast("deskHubConnectionState", "error");
                        $rootScope.$apply();
                    },

                    stateChanged: function(state) {
                        connected = false;
                        console.info(state);
                        switch (state.newState) {
                            case $.signalR.connectionState.connecting:
                                $rootScope.$broadcast("deskHubConnectionState", "connecting");
                                break;
                            case $.signalR.connectionState.connected:
                                connected = true;
                                $rootScope.$broadcast("deskHubConnectionState", "connected");
                                $rootScope.$apply();
                                break;
                            case $.signalR.connectionState.reconnecting:
                                $rootScope.$broadcast("deskHubConnectionState", "reconnecting");
                                $rootScope.$apply();
                                break;
                            case $.signalR.connectionState.disconnected:
                                $rootScope.$broadcast("deskHubConnectionState", "disconnected");
                                $rootScope.$apply();
                                break;
                        }
                    }
                });

                return {

                    //WebAPI methods

                    createDesk: function (cards) {
                        return $http.put("api/1.0/desk/", cards).then(function (response) {
                            return response.data;
                        });
                    },

                    deleteDesk: function (deskName) {
                        return $http.delete("api/1.0/desk/" + deskName).then(function (response) {
                            return response.data;
                        });
                    },

                    getDesk: function (deskName) {
                        return $http.get("api/1.0/desk/" + deskName).then(function (response) {
                            return response.data;
                        });
                    },

                    //Hub methods

                    joinAsUser: function (deskName, userName) {
                        return $q.when(deskHub.joinAsUser(deskName, userName));
                    },

                    joinAsMaster: function (deskName) {
                        return $q.when(deskHub.joinAsMaster(deskName));
                    },

                    leave: function (deskName) {
                        return $q.when(deskHub.leave(deskName));
                    },

                    setUserCard: function (deskName, userName, card) {
                        return $q.when(deskHub.setUserCard(deskName, userName, card));
                    },

                    setDeskState: function (deskName, newState) {
                        return $q.when(deskHub.setDeskState(deskName, newState));
                    },

                    hasConnection: function () {
                        return connected;
                    },

                    reconnect: function() {
                        $timeout(function () {
                            deskHub.connect();
                        }, 2000);
                        
                    }

                };
            }
        ]);

})();