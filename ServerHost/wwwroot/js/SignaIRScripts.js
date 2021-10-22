const connection = new signalR
    .HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

async function start() {
    try {
        await connection.start();

        isConnect = true;
        UserConnectionGenerator(true);
        GetChats(true);
        searchChatsInput.prop("disabled", false);
        searchChatsInput.css("background", "#fff");
        Chats.css("display", "block");

    } catch (e) {
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
    function (result) {
        const message = [];
        message.push(result);

        if (message[0].chatName == "") {

            SendNotification(message[0].username,
                message[0].body,
                `/uploads/${message[0].profileImage}`);

        } else {

            const body = `${message[0].username} : ${message[0].body}`;
            SendNotification(message[0].chatName,
                body,
                `/uploads/${message[0].chatImage}`);

        }

        ChatMessagesGenerator(message, true);
    });

connection.on("UserStatus",
    function (result) {

        const element = $(`li[account-id='${result.id}']`);
        const chatConnectionStatus = $(userChat).find("p#user-chat-connection-status")[0];
 

        if (element.length != 0) {

            const statusPoint = element.find("i.status-point");
            const statusAlert = element.find("span.status-alert");
            const chatId = element.attr("chat-id");

            if (result.isOnline) {
                $(statusPoint).removeClass("text-danger");
                $(statusPoint).addClass("text-success");
                $(statusAlert).html("آنلاین");

                if (userChat.attr("current-chat-id") == chatId) {
                    chatConnectionStatus.innerHTML = "آنلاین";
                }

            } else {
                $(statusPoint).removeClass("text-success");
                $(statusPoint).addClass("text-danger");
                $(statusAlert).html("آفلاین");

                if (userChat.attr("current-chat-id") == chatId) {
                    chatConnectionStatus.innerHTML = "آفلاین";
                }
            }
        }
    });

$(function() {

    start();

});