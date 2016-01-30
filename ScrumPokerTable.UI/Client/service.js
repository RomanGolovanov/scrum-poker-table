(function () {
    angular
        .module("ScrumPokerTable")
        .factory("DeskHubService", [
            "$rootScope", "$q", "$http", "Hub", function($rootScope, $q, $http, Hub) {

                var deskHub = new Hub("deskHub", {
                    listeners: {
                        "deskChanged": function (desk) {
                            console.log(desk);
                            $rootScope.$broadcast("deskChanged", desk);
                            $rootScope.$apply();
                        }
                    },

                    methods: [
                        "createDesk",
                        "deleteDesk",
                        "joinAsUser",
                        "joinAsMaster",
                        "leave",
                        "setUserCard",
                        "setDeskState"
                    ],

                    //transport: ["longPolling"],
                    queryParams: { "api": "1.0" },
                    errorHandler: function(error) {
                         console.error(error);
                    },

                    stateChanged: function(state) {
                        console.log(state);
                        switch (state.newState) {
                            case $.signalR.connectionState.connecting:
                                $rootScope.$broadcast("deskHubConnecting");
                                break;
                            case $.signalR.connectionState.connected:
                                $rootScope.$broadcast("deskHubConnected");
                                break;
                            case $.signalR.connectionState.reconnecting:
                                $rootScope.$broadcast("deskHubReconnecting");
                                break;
                            case $.signalR.connectionState.disconnected:
                                $rootScope.$broadcast("deskHubDisconnected");
                                break;
                        }
                    }
                });

                return {
                    createDesk: function(cards) {
                        return $q.when(deskHub.createDesk(cards));
                    },

                    deleteDesk: function(deskName) {
                        return $q.when(deskHub.deleteDesk(deskName));
                    },

                    joinAsUser: function (deskName, userName) {
                        return $q.when(deskHub.joinAsUser(deskName, userName));
                    },

                    joinAsMaster: function (deskName) {
                        return $q.when(deskHub.joinAsMaster(deskName));
                    },

                    leave: function (deskName) {
                        return $q.when(deskHub.leave(deskName));
                    },

                    getDesk: function (deskName) {
                        return $http.get("api/1.0/desk/" + deskName).then(function (response) {
                            return response.data;
                        });
                    },

                    setUserCard: function (deskName, userName, card) {
                        return $q.when(deskHub.setUserCard(deskName, userName, card));
                    },

                    setDeskState: function (deskName, newState) {
                        return $q.when(deskHub.setDeskState(deskName, newState));
                    }

                };
            }
        ]);

})();