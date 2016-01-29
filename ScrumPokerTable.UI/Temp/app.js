
$(function () {
    $("#deskname").val(prompt("Enter desk name:", ""));
    $("#displayname").val(prompt("Enter your name:", ""));
    $("#message").focus();


    var deskName = $("#deskname").val();
    var userName = $("#displayname").val();

    var deskHub = $.connection.deskHub;

    deskHub.client.deskChanged = function (name, message) {
        var encodedName = $("<div />").text(name).html();
        var encodedMsg = $("<div />").text(message).html();
        $("#discussion").append("<li><strong>" + encodedName + "</strong>:&nbsp;&nbsp;" + encodedMsg + "</li>");
    };

    $.connection.hub.start().done(function () {
        deskHub.server.connectDesk(deskName, userName);
        $("#sendmessage").click(function () {
            deskHub.server.updateDeskUser(deskName, userName, $("#message").val());
            $("#message").val("").focus();
        });
    });
});