let isConnect = false;
const contactChats = $("#contact-chat-items");
const Chats = $("#Chats");
const currentUser = $("#user-detail .current-user");
const userChat = $("#user-detail .user-chat");
const userAvatar = $("#user-avatar")
const chatMessagesMenu = document.querySelector("#Chats ul");
const chatsMenu = $("#contact-chat-items ul");
const searchChatsInput = $("#contact-chat-items .search-input");
const serverConnectionStatus = $("#server-connection-status");
const userConnectionStatus = $("#user-connection-status");
const ChatMessageBox = $("#Chats .chat-message-box");
const ChatMessageForm = $("#Chats .chat-message-box form");

$(function () {
    GoToEndScroll();
    UpdateUserProfile();
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
    GetChatInfo({
        chatId,
        accountId
    });

    if (!contactChats.hasClass("disable-element")) {
        contactChats.addClass("disable-element");
        Chats.removeClass("disable-element");

        currentUser.addClass("disable-element");
        userChat.removeClass("disable-element");

        $("#page-back").addClass("inline-element");
        $("#page-back").removeClass("disable-element");
    }
}

function back() {
    if (contactChats.hasClass("disable-element")) {
        contactChats.removeClass("disable-element");
        Chats.addClass("disable-element");

        currentUser.removeClass("disable-element");
        userChat.addClass("disable-element");


        $("#page-back").removeClass("inline-element");
        $("#page-back").addClass("disable-element");
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

async function GetChats(connection) {
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

        await $.ajax(settings).done(function (response) {
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
            : `uploads/${x.profileImage}`;
        var body;

        if (x.isFile) {
            body = FileMessageGenerator(x);
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

function FileMessageGenerator(meesage) {

    var exe = meesage.body.split('.').pop();

    switch (exe) {
        case "jpg":
        case "jpeg":
        case "png":
        case "gif":
            return `<img class="file-size-in-chat" loading="lazy" src="uploads/${meesage.body}" alt="${meesage.body}">`;
            break;
        case "mp4":
        case "mkv":
        case "webm":
        case "avi":
            return `<video class="file-size-in-chat" controls>
                        <source src="uploads/${meesage.body}" type="video/${exe}" codecs="avc1.42E01E, mp4a.40.2">
                        مرورگر شما از این فرمت پشتیبانی نمی کند.
                    </video>`;
            break;
        case "mp3":
            return `<audio class="audio-size-in-chat" controls>
                        <source src="uploads/${meesage.body}" type="audio/${exe}">
                        مرورگر شما از این فرمت پشتیبانی نمی کند.
                    </audio>`;
            break;
        default:
            return `<a href="uploads/${meesage.body}" target="_blank">فایل</a>`;;
            break;
    }

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
        "error": function () {
            ChatListGenerator(null);
        }
    };

    $.ajax(settings).done(function (response) {
        ChatListGenerator(response);
    });
}

function GetChatMessage(chatId, isCheckChatId = true) {

    const messagesUl = $(chatMessagesMenu);

    if (isCheckChatId && messagesUl.attr("current-chat-id") == chatId) {
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
            "error": function (e) {
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

function GetChatInfo(chatInfo, isCheckChatId = true) {

    const chatId = chatInfo.chatId;

    if (isCheckChatId && userChat.attr("current-chat-id") == chatId) {
        return;
    }

    if (isConnect) {
        const settings = {
            "url": "/Index?handler=GetChatInfo",
            "method": "Post",
            "timeout": 0,
            "headers": {
                "Content-Type": "application/json"
            },
            "data": JSON.stringify(chatInfo),
            "error": function (e) {
                console.log(e);
            }
        };

        $.ajax(settings).done(function (response) {

            userChat.attr("current-chat-id", chatId);

            const profile = response.image == null ? "/img/default-avatar.jpg" : `uploads/${response.image}`;

            let status = "";
            if (response.isGroupOrChannel) {
                status = "<spam class='text-white'>.</spam>";
            } else {
                status = response.isOnline ? "آنلاین" : "آفلاین";
            }

            const element =
                `<img id="user-chat-avatar" src="${profile}" class="d-inline-block align-top" alt="${response.title}">
                 <div class="connection-status mr-3">
                      <p id="user-chat-info">${response.title}</p>
                      <p id="user-chat-connection-status">${status}</p>
                 </div>`;

            userChat.html(element);
        });
    }
}

function GetModalContent() {

    var url = window.location.hash.toLowerCase();

    window.location.hash = "";

    if (!url.startsWith("#showmodal")) {
        return;
    }
    url = url.split("showmodal=")[1];
    $.get(url,
        null,
        function (htmlPage) {
            $("#ModalContent").html(htmlPage);

            const container = document.getElementById("ModalContent");
            const forms = container.getElementsByTagName("form");
            const newForm = forms[forms.length - 1];

            //Validation For Display : None Item
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(newForm);

            $("#MainModal").modal("show");
        }).fail(function () {
            swal("خطا", "لطفا دوباره تلاش کنید", "warning");
        });
};

function UpdateUserProfile() {

    const settings = {
        "url": "/Index?handler=GetProfileImage",
        "method": "Post",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "error": function (e) {
            swal("خطا", "لطفا دوباره تلاش کنید", "warning");
        }
    };

    $.ajax(settings).done(function (response) {
        if (response != null) {
            userAvatar.attr("src", `uploads/${response}`);
        }
    });
}

function OperationEvent(operation) {
    operation = operation.split("=")[1];
    const chatId = $(chatMessagesMenu).attr("current-chat-id");

    if (operation == "EditProfile") {
        UpdateUserProfile();
        if (chatId != 0) {
            GetChatMessage(chatId, false);
        }
    }

    if (operation == "AddGroupChannel") {
        GetChats(isConnect);
    }
}

async function SendNotification(title, message, img) {

    if (Notification.permission === "granted") {

        const notification = new Notification(title,
            {
                body: message,
                icon: img
            });

    } else {
        swal("توجه توجه", "با فعال کردن اعلانات مرورگر از اخرین پیام هایی که دریافت می کنید مطلع می شود", "warning");
        const permission = await Notification.requestPermission();

        if (permission === "granted")
            SendNotification(title, message, img);
    }

}

//Search Chats
searchChatsInput.on("keyup",
    function () {
        SearchChats(this.value);
    });

//Send Message
ChatMessageForm.on("submit",
    function (e) {
        e.preventDefault();

        const messageInput = $(this[2]);

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
            "error": function (e) {
                swal("خطا", "لطفا دوباره تلاش کنید", "warning");
            }
        };

        $.ajax(settings).done(function (response) {
            let messages = new Array();
            messages.push(response);
            ChatMessagesGenerator(messages, true);
            $(ChatMessageForm[0][2]).val("");

            let chatId = $(ChatMessageForm[0][0]).val();
            let accountId = $(ChatMessageForm[0][1]).val();

            $(this).trigger("reset");
            messageInput.prop("disabled", false);

            ResetChat(chatId, accountId);
        });
    });

//input file change event
$(ChatMessageForm[0][3]).on("change",
    function () {
        const messageInput = ChatMessageForm[0][2];
        const file = $(this).val();

        if (file != "") {
            messageInput.disabled = true;
            $(messageInput).val("فایل با موفقیت انتخاب شد(برای لغو دوبار ضربه بزنید)");
        }
    });

//reset message box form
$(ChatMessageForm[0]).dblclick(function (e) {
    e.preventDefault();
    $(this).trigger("reset");
    const messageInput = ChatMessageForm[0][2];
    messageInput.disabled = false;
});

// hash change event
$(window).on("hashchange",
    function (e) {
        GetModalContent();
    });

// operation form event
$(document).on("submit",
    'form[data-operation="true"]',
    function (e) {
        e.preventDefault();

        var form = $(this);
        const method = form.attr("method").toLocaleLowerCase();
        const url = form.attr("action");
        var action = form.attr("data-action");


        if (method === "get") {
            const data = form.serializeArray();
            $.get(url,
                data,
                function (data) {
                    CallBackHandler(data, action, form);
                });
        } else {
            const formData = new FormData(this);
            $.ajax({
                url: url,
                type: "post",
                data: formData,
                enctype: "multipart/form-data",
                dataType: "json",
                processData: false,
                contentType: false,
                success: function (response) {
                    $("#MainModal").modal("hide");
                    swal("", response.message, "success");
                    OperationEvent(url);
                },
                error: function () {
                    $("#MainModal").modal("hide");
                    swal("خطا", "لطفا دوباره تلاش کنید", "warning");
                }
            });
        }
        return false;
    });

jQuery.validator.addMethod("fileExtensionLimit",
    function (value, element, params) {

        if (element.files[0] == null)
            return true;

        var fileExe = element.files[0].name.split(".");
        fileExe = `.${fileExe[fileExe.length - 1]}`;

        const exe = element.attributes.getNamedItem("data-val-extensions").value.split(",");

        return exe.includes(fileExe);
    });
jQuery.validator.unobtrusive.adapters.addBool("fileExtensionLimit");