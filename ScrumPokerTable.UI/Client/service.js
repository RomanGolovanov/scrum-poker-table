(function () {
    window.angular
        .module("ScrumPokerTable")
        .factory("DeskHubService", [
            "$rootScope", "$q", "$http", "$timeout", "Hub", function($rootScope, $q, $http, $timeout, Hub) {

                var connected = false;

                $.connection.hub.transportConnectTimeout = 1000;



                var deskHub = new Hub("deskHub", {
                    listeners: {
                        "deskChanged": function (desk) {
                            $rootScope.$applyAsync(function () {
                                $rootScope.$broadcast("deskChanged", desk);
                            });
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
                        $rootScope.$applyAsync(function() {
                            console.error(error);
                            connected = false;
                            $rootScope.$broadcast("deskHubConnectionState", "error");
                        });
                    },

                    stateChanged: function(state) {
                        switch (state.newState) {
                            case $.signalR.connectionState.connecting:
                                $rootScope.$applyAsync(function () {
                                    console.log("Connecting");
                                    connected = false;
                                    $rootScope.$broadcast("deskHubConnectionState", "connecting");
                                });
                                break;
                            case $.signalR.connectionState.connected:
                                $rootScope.$applyAsync(function() {
                                    console.log("Connected");
                                    connected = true;
                                    $rootScope.$broadcast("deskHubConnectionState", "connected");
                                });
                                break;
                            case $.signalR.connectionState.reconnecting:
                                $rootScope.$applyAsync(function() {
                                    console.log("Reconnecting");
                                    connected = false;
                                    $rootScope.$broadcast("deskHubConnectionState", "reconnecting");
                                });
                                break;
                            case $.signalR.connectionState.disconnected:
                                $rootScope.$applyAsync(function() {
                                    console.log("Disconnected");
                                    connected = false;
                                    $rootScope.$broadcast("deskHubConnectionState", "disconnected");
                                    scheduleReconnect();
                                });
                                break;
                        }
                    }
                });

                function scheduleReconnect() {
                    console.log("Reconnect scheduled...");
                    $timeout(function () {
                        console.log("Call deskHub.connect()");
                        deskHub.connect();
                    }, 500);

                }

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
                    }
                };
            }
        ]);

})();