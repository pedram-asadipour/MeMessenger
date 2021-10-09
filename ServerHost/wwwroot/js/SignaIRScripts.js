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

    } catch (err) {
        console.log(err);

        isConnect = false;
        UserConnectionGenerator(false);
        GetChats(false);
        searchChatsInput.prop("disabled", true);
        searchChatsInput.css("background", "#ccc");
        setTimeout(start, 1500);
    }
};

connection.onclose(async () => {
    setTimeout(await start(), 1500);
});


connection.on("ReceiveChats",
    function(response) {
        console.log(response);
    });


$(function() {

    start();

});