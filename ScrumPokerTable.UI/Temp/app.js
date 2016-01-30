
$(function () {

    function enterCredentials() {
        $("#deskname").val(prompt("Enter desk name:", ""));
        $("#displayname").val(prompt("Enter your name:", ""));
        $("#message").focus();
    }


    enterCredentials();
    var deskName = $("#deskname").val();
    var userName = $("#displayname").val();

    var deskHub = $.connection.deskHub;

    function appendLog(name, value) {
        var encodedName = $("<div />").text(name).html();
        var encodedMsg = $("<div />").text(value).html();
        $("#discussion").append("<li><strong>" + encodedName + "</strong>:&nbsp;&nbsp;" + encodedMsg + "</li>");
    }

    deskHub.client.deskChanged = function (desk) { appendLog("desk changed", JSON.stringify(desk)); };

    function loginDesk() {
        deskHub.server
            .joinAsUser(deskName, userName)
            .then(function () {
                appendLog("desk connected");
                $("#status").val($.connection.hub.transport.name);
                $("#sendmessage").click(function () {
                    deskHub
                        .server
                        .setUserCard(deskName, userName, $("#message").val())
                        .then(function(result) { appendLog("result", result); })
                        .fail(function(error) { appendLog("error", JSON.stringify(error)); });
                    ;
                    $("#message").val("").focus();
                });
            })
            .fail(function (error) {
                appendLog("connection error:", JSON.stringify(error));
            });
    }

    $.connection.hub.start().done(loginDesk);
});