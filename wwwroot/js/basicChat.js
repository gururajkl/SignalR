var hubConnection = new signalR.HubConnectionBuilder().withUrl("/hubs/basicChat").build();

document.getElementById("sendMessage").disabled = true;

hubConnection.on("messageReceived", (user, message) => {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user} - ${message}`;
});

document.getElementById("sendMessage").addEventListener("click", (event) => {
    var sender = document.getElementById("senderEmail").value;
    var receiver = document.getElementById("receiverEmail").value;
    var message = document.getElementById("chatMessage").value;

    if (receiver.length > 0) {
        hubConnection.send("SendMessageToReceiver", sender, receiver, message);
    } else {
        hubConnection.send("SendMessageToAll", sender, message);
    }

    event.preventDefault();
});

hubConnection.start().then(() => {
    document.getElementById("sendMessage").disabled = false;
});