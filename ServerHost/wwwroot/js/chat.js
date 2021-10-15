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
const ChatMessageBox = $("#Chats .chat-message-box");
const ChatMessageForm = $("#Chats .chat-message-box form");

$(function() {
    GoToEndScroll();
});

function GoToEndScroll() {
    chatMessagesMenu.scrollTop = chatMessagesMenu.scrollHeight;
}

function openChat(element) {

    const chatId = element.attributes.getNamedItem("chat-id").value;
    const accountId = element.attributes.getNamedItem("account-id").value;;

    ChatMessageBox.css("display", "block");

    //set chat id in message form
    ChatMessageForm[0][0].value = chatId;
    //
    //set account id in private message
    ChatMessageForm[0][1].value = accountId;
    //

    GetChatMessage(chatId);

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

async  function GetChats(connection) {
    if (connection) {
        const settings = {
            "url": "/Index?handler=GetChats",
            "method": "Post",
            "timeout": 0,
            "headers": {
                "Content-Type": "application/json"
            },
            "error": function() {
                ChatListGenerator(null);
            }
        };

    await $.ajax(settings).done(function(response) {
            ChatListGenerator(response);
        });
    } else {
        ChatListGenerator(null);
    }
}

function ChatMessagesGenerator(messages, isOneAdd = false) {

    if (messages == null || messages.length == 0) {
        $(chatMessagesMenu).html("");
        return;
    }

    const chatId = $(chatMessagesMenu).attr("current-chat-id");

    if (chatId != messages[0].chatId)
        return;

    var message = "";

    messages.forEach(x => {
        var owner = x.isOwner ? "chat-box-right" : "chat-box-left";
        var profileImage = (x.profileImage == null || x.profileImage == "")
            ? "/img/default-avatar.jpg"
            : x.profileImage;
        var body;

        if (x.isFile) {
            body = `<a href="uploads/${x.body}" target="_blank">فایل</a>`;
        } else {
            body = x.body;
        }

        message += `<li message-id="${x.id}" class="chat-box ${owner}">
                        <img src="${profileImage}" alt="${x.username}">
                        <div class="message-box">
                            <p class="username">${x.username}</p>
                            <p class="message">${body}</p>
                        </div>
                        <span class="send-time">${x.sendDate}</span>
                    </li>`;

    });

    if (!isOneAdd)
        $(chatMessagesMenu).html("");

    $(chatMessagesMenu).append(message);
    GoToEndScroll();
}

function ChatListGenerator(chats) {

    if (chats == null || chats.length == 0) {
        isConnect
            ? chatsMenu.html("<li class='text-center mt-5' style='margin-right: -3rem;'>موردی یافت نشد</li>")
            : chatsMenu.html(
                "<li class='text-center mt-5' style='margin-right: -3rem;'>در حال تلاش برای بارگذاری ...</li>");
        return;
    }

    var chatItems = "";

    chats.forEach(x => {
        var privateDetail = "";
        var image = (x.image == "" || x.image == null) ? "/img/default-avatar.jpg" : `uploads/${x.image}`;

        if (x.isPrivate) {
            if (x.isOnline) {
                privateDetail = `<i class="status-point fa fa-circle text-success"></i>
                            <span class="status-alert">آنلاین</span>`;
            } else {
                privateDetail = `<i class="status-point fa fa-circle text-danger"></i>
                            <span class="status-alert">آفلاین</span>`;
            }
        }

        chatItems += `
            <li chat-id="${x.chatId}" account-id="${x.accountId}"
                        onclick="openChat(this)" class="chat-item">
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

async function SearchChats(search) {

    if (search == "" && isConnect) {
        await GetChats(isConnect);
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

function GetChatMessage(chatId) {

    const messagesUl = $(chatMessagesMenu);
    if (messagesUl.attr("current-chat-id") == chatId) {
        return;
    }

    if (isConnect) {
        const settings = {
            "url": "/Index?handler=GetChatMessages",
            "method": "Post",
            "timeout": 0,
            "headers": {
                "Content-Type": "application/json"
            },
            "data": JSON.stringify(chatId),
            "error": function(e) {
                ChatMessagesGenerator(null);
            }
        };

        $.ajax(settings).done(function (response) {
            messagesUl.attr("current-chat-id", chatId);
            ChatMessagesGenerator(response);
        });
    } else {
        ChatMessagesGenerator(null);
    }
}

async function ResetChat(chatId, accountId) {
    if (chatId == 0 || accountId == 0) {

        searchChatsInput.val("");
        await GetChats(isConnect);

        let chatElementItem = "";

        if (chatId != 0)
            chatElementItem = document.querySelector(`li[chat-id='${chatId}']`);

        if (accountId != 0)
            chatElementItem = document.querySelector(`li[account-id='${accountId}']`);

        openChat(chatElementItem);
    }
}

//Search Chats
searchChatsInput.on("keyup",
    function() {
        SearchChats(this.value);
    });

//Send Message
ChatMessageForm.on("submit",
    function(e) {
        e.preventDefault();

        const messageInput = $(this[1]);

        if (messageInput.val() == "")
            return;

        const formData = new FormData(this);

        const settings = {
            "url": "/Index?handler=SendMessage",
            "method": "Post",
            "timeout": 0,
            "data": formData,
            "contentType": false,
            "processData": false,
            "error": function(e) {
                console.log(e);
            }
        };

        $.ajax(settings).done(function(response) {
            let messages = new Array();
            messages.push(response);
            ChatMessagesGenerator(messages, true);
            $(ChatMessageForm[0][2]).val("");

            let chatId = $(ChatMessageForm[0][0]).val();
            let accountId = $(ChatMessageForm[0][1]).val();

            ResetChat(chatId, accountId);
        });
    });

//input file change event
$(ChatMessageForm[0][3]).on("change",
    function() {
        const messageInput = ChatMessageForm[0][2];
        const file = $(this).val();

        if (file != "") {
            messageInput.disabled = true;
            $(messageInput).val("فایل با موفقیت انتخاب شد(برای لغو دوبار ضربه بزنید)");
        }
    });

//reset message box form
$(ChatMessageForm[0]).dblclick(function(e) {
    e.preventDefault();
    $(this).trigger("reset");
});
