let isConnect = false;
const contactChats = $("#contact-chat-items");
const Chats = $("#Chats");
const currentUser = $("#user-detail .current-user");
const userChat = $("#user-detail .user-chat");
const chatMessagesMenu = document.querySelector("#Chats ul");
const chatsMenu = $("#contact-chat-items ul");
const searchChatsInput = $("#contact-chat-items .search-input");
const serverConnectionStatus = $("#server-connection-status");
const userConnectionStatus = $("#user-connection-status");

$(function() {
    GoToEndScroll();
});

function GoToEndScroll() {
    chatMessagesMenu.scrollTop = chatMessagesMenu.scrollHeight;
}

function openChat(element) {
    if (!contactChats.hasClass("disable-element")) {
        contactChats.addClass("disable-element");
        Chats.removeClass("disable-element");

        currentUser.addClass("disable-element");
        userChat.removeClass("disable-element");
    }
}

function back() {
    if (contactChats.hasClass("disable-element")) {
        contactChats.removeClass("disable-element");
        Chats.addClass("disable-element");

        currentUser.removeClass("disable-element");
        userChat.addClass("disable-element");
    }
}

function UserConnectionGenerator(connection) {
    if (connection) {
        serverConnectionStatus.removeClass("text-danger");
        serverConnectionStatus.addClass("text-success");

        serverConnectionStatus.html("<i class='fas fa-check'></i> متصل");
        userConnectionStatus.html("آنلاین");
    } else {
        serverConnectionStatus.removeClass("text-success");
        serverConnectionStatus.addClass("text-danger");

        serverConnectionStatus.html("در حال اتصال <i class= 'fas fa-circle-notch fa-spin'></i >");
        userConnectionStatus.html("آفلاین");
    }
}

function GetChats(connection) {
    if (connection) {
        const settings = {
            "url": "/Index?handler=GetChats",
            "method": "Post",
            "timeout": 0,
            "headers": {
                "Content-Type": "application/json"
            },
            "error": function () {
                ChatListGenerator(null);
            }
        };

        $.ajax(settings).done(function(response) {
            ChatListGenerator(response);
        });
    } else {
        ChatListGenerator(null);
    }
}

function ChatListGenerator(chats) {

    if (chats == null || chats.length == 0) {
        isConnect ? chatsMenu.html("<li class='text-center mt-5' style='margin-right: -3rem;'>موردی یافت نشد</li>") : chatsMenu.html("<li class='text-center mt-5' style='margin-right: -3rem;'>در حال تلاش برای بارگذاری ...</li>");
        return;
    }

    var chatItems = "";

    chats.forEach(x => {
        var privateDetail;
        var image = (x.image == "" || x.image == null) ? "/img/default-avatar.jpg" : x.image;

        if (x.isPrivate) {
            privateDetail = `<i class="status-point fa fa-circle text-danger"></i>
                            <span class="status-alert">آفلاین</span>`;
        }

        chatItems += `
            <li chat-id="${x.chatId}" onclick="openChat(this)" class="chat-item">
                <img class="chat-avatar" src="${image}" alt="${x.title}">
                ${privateDetail}
                <p class="chat-name">${x.title}</p>
                <span class="last-chat-date ltr">${x.lastMessageDate}</span>
                <p class="last-chat-text">${x.lastMessage}</p>
            </li>`;
    });

    chatsMenu.html("");
    chatsMenu.prepend(chatItems);
}

function SearchChats(search) {

    if (search == "") {
        GetChats(isConnect);
        return;
    }

    const settings = {
        "url": "/Index?handler=SearchChats",
        "method": "Post",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": JSON.stringify(search),
        "error": function() {
            ChatListGenerator(null);
        }
    };

    $.ajax(settings).done(function(response) {
        ChatListGenerator(response);
    });
}

searchChatsInput.on("keyup",function() {
    SearchChats(this.value);
});