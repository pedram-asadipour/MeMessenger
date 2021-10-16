const connection = new signalR
    .HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");

        isConnect = true;
        UserConnectionGenerator(true);
        GetChats(true);
        searchChatsInput.prop("disabled", false);
        searchChatsInput.css("background", "#fff");
        Chats.css("display", "block");

    } catch (err) {
        console.log(err);

        isConnect = false;
        UserConnectionGenerator(false);
        GetChats(false);
        searchChatsInput.prop("disabled", true);
        searchChatsInput.css("background", "#ccc");
        Chats.css("display", "none");

        setTimeout(start, 1500);
    }
};

connection.onclose(async () => {
    setTimeout(await start(), 1500);
});

connection.on("ReceiveMessage",
    function(result) {
        const message = [];
        message.push(result);
        ChatMessagesGenerator(message, true);
    });

connection.on("UserStatus",
    function(result) {
        const element = $(`li[account-id='${result.id}']`);

        if (element.length != 0) {

            const statusPoint = element.find("i.status-point");
            const statusAlert = element.find("span.status-alert");

            if (result.isOnline) {
                $(statusPoint).removeClass("text-danger");
                $(statusPoint).addClass("text-success");
                $(statusAlert).html("آنلاین");
            } else {
                $(statusPoint).removeClass("text-success");
                $(statusPoint).addClass("text-danger");
                $(statusAlert).html("آفلاین");
            }
        }
    });

$(function() {

    start();

});