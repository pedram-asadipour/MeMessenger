const contactChats = $("#contact-chat-items");
const Chats = $("#Chats");
const currentUser = $("#user-detail .current-user");
const userChat = $("#user-detail .user-chat");
const chatsSection = document.querySelector("#Chats ul");


$(function () {

  //#region Go To End Scroll
  chatsSection.scrollTop = chatsSection.scrollHeight;
  //#endregion

});

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
