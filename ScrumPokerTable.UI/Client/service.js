(function () {


    window.angular
        .module("ScrumPokerTable")
        .factory("DeskHubService", [
            "$rootScope", "$q", "$http", "$timeout", function ($rootScope, $q, $http, $timeout) {
                "use strict";
                
                function PollingWorker(deskName, handler) {

                    var self = this;

                    this.deskName = deskName;
                    this.handler = handler;
                    this.pollingTimeout = 15000;
                    this.normalDelay = 10;
                    this.errorDelay = 1000;

                    this.desk = null;
                    this.timer = null;
                    this.canceled = false;

                    function runDeskChangePolling() {
                        if (self.canceled) {
                            return;
                        }

                        self.timer = null;

                        var config = { headers: {} };

                        if (self.desk) {
                            config.timeout = self.pollingTimeout * 1200;
                            config.headers["X-Polling-Timeout"] = self.pollingTimeout;
                            config.headers["X-Timestamp"] = self.desk.timestamp;
                        };

                        $http.get("api/1.0/desk/" + self.deskName + "?rnd" + (new Date().getTime()), config).then(function (response) {
                            if (self.canceled) {
                                return;
                            }
                            self.desk = response.data;
                            if (self.canceled) {
                                return;
                            }
                            console.log('Desk changed', self.desk);
                            self.handler(self.desk);
                            self.timer = $timeout(runDeskChangePolling, self.normalDelay);
                        }, function (response) {
                            if (self.canceled || response.status === 404) {
                                return;
                            }
                            self.timer = $timeout(runDeskChangePolling, (response.status === 304 ? self.normalDelay : self.errorDelay));
                        });
                    };

                    this.cancel = function () {
                        self.canceled = true;
                    };

                    runDeskChangePolling();

                    return this;
                };


                return {

                    createDesk: function (cards) {
                        return $http.post("api/1.0/desk/", cards).then(function (response) {
                            return response.data;
                        });
                    },

                    deleteDesk: function (deskName) {
                        return $http.delete("api/1.0/desk/" + deskName).then(function (response) {
                            return response.data;
                        });
                    },

                    getHistory: function (deskName) {
                        return $q.when([]);
                    },

                    getDesk: function (deskName) {
                        return $http.get("api/1.0/desk/" + deskName + "?rnd" + (new Date().getTime())).then(function (response) {
                            return response.data;
                        });
                    },

                    joinAsUser: function (deskName, userName) {
                        return $http.post("api/1.0/player/" + deskName, { name: userName }).then(function (response) {
                            return response.data;
                        });
                    },
                    
                    setUserCard: function (deskName, userName, card) {
                        return $http.put("api/1.0/player/" + deskName, { name: userName, card: card }).then(function (response) {
                            return response.data;
                        });
                    },

                    setDeskState: function (deskName, newState) {
                        return $http.post("api/1.0/master/" + deskName, newState).then(function (response) {
                            return response.data;
                        });
                    },

                    runDeskChangePolling: function (deskName, handler) {
                        var pollingWorker = new PollingWorker(deskName, handler);
                        return function() {
                            pollingWorker.cancel();
                        }
                    }

                };
            }
        ]);

})();