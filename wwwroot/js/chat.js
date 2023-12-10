var hubConnection = new signalR.HubConnectionBuilder().withUrl("/hubs/chat").withAutomaticReconnect([0, 1000, 5000, null]).build();

hubConnection.on("receiveUserConnected", (userId, userName) => {
    addMessage(`${userName} is online`);
});

function addMessage(message) {
    if (message == null || message == '') {
        return;
    }

    var ui = document.getElementById("messagesList");
    var li = document.createElement("li");
    li.innerHTML = message;
    ui.appendChild(li);
}

hubConnection.start();