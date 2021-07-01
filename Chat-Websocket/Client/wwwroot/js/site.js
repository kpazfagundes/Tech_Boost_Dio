// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/* Métodos para manipulação da view
 * Há apenas uma instância de CHatService
 */

var chatService = null;

var messageList = document.getElementById("messages");
var connectButton = document.getElementById("connectButton");
var sendButton = document.getElementById("sendButton");
var textName = document.getElementById("textName");
var helpName = document.getElementById("helpName");

connectButton.addEventListener("click", () => {
    if (connectButton.innerText == "Entrar")
        connect();
    else//Sair
        disconnect();

    connectButton.disabled = "disabled";
});

textName.addEventListener("keyup", function (event) {
    if (event.keyCode === 13) {
        event.preventDefault();
        connectButton.click();
    }
});

sendButton.addEventListener("click", function () {
    chatService.sendMessage(textMessage.value);
    textMessage.value = "";
});

textMessage.addEventListener("keyup", function (event) {
    if (event.keyCode === 13) {
        event.preventDefault();
        sendButton.click();
    }
});

//Abre uma conexão com o Websocket e configura os listeners
function connect() {
    var options = {
        username: textName.value,

        onOpen: (evt) => {
            textName.disabled = 'disabled';
            connectButton.innerHTML = "Sair";
            connectButton.disabled = "";

            textMessage.disabled = "";
            sendButton.disabled = "";
        },
        onClose: (etv) => {
            textName.disabled = '';
            connectButton.innerHTML = "Entrar";
            connectButton.disabled = "";
            textMessage.disabled = "disabled";
            sendButton.disabled = "disabled";

            appendMessage("Você saiu da sala #geral.", ["text-danger", "font-weight-bold"]);
        },
        onMessage: (evt) => {
            message = ChatService.parse(evt.data);

            switch (message.command) {

                case ChatService.commands.loginError:
                    textName.disabled = '';
                    connectButton.innerHTML = "Entrar";
                    connectButton.disabled = "";
                    textMessage.disabled = "disabled";
                    sendButton.disabled = "disabled";

                    helpName.innerHTML = message.content;
                    helpName.style = "display:block";
                    break;

                case ChatService.commands.enteredRoom:
                    appendMessage(message.content, ["text-success", "font-weight-bold"]);
                    helpName.innerHTML = "";
                    helpName.style = "display:none";
                    break;

                case ChatService.commands.exitedRoom:
                    appendMessage(message.content, ["text-warning", "font-weight-bold"]);
                    break;

                case ChatService.commands.receiveMessage:
                    appendMessage(message.content);
                    break;

                case ChatService.commands.receiveMessagePrivate:
                    appendMessage(message.content, ["text-primary", "font-weight-bold"]);
                    break;

                case ChatService.commands.mentionError:
                    appendMessage(message.content, ["text-danger"]);
                    break;
            }
        }
    }
    chatService = new ChatService(options);
    chatService.connect();
}

//Fecha a conexão com o Websocket
function disconnect() {
    if (chatService != undefined)
        chatService.disconnect();
}

//Adiciona uma mensagem à lista de mensagens
function appendMessage(message,style=[]) {
    var item = document.createElement("p");
    item.classList.add("mb-0");
    style.forEach((s) => {
        item.classList.add(s);
    });

    item.appendChild(document.createTextNode(message));
    messageList.appendChild(item);

    messageList.scrollTop = messageList.scrollHeight;
}
