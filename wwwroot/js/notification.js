var hubConnection = new signalR.HubConnectionBuilder().withUrl("/hubs/notificationHub").build();

document.getElementById("sendButton").disabled = true;

document.getElementById("sendButton").addEventListener("click", (event) => {
    hubConnection.send("SendMessage", document.getElementById("notificationInput").value).then(() => {
        document.getElementById("notificationInput").value = "";
    });
    event.preventDefault();
});

hubConnection.on("loadNotification", (messages, count) => {
    document.getElementById("messageList").innerHTML = "";
    document.getElementById("notificationCounter").innerHTML = "<span>(" + count + ")</span>";

    for (let i = messages.length - 1; i >= 0; i--) {
        var li = document.createElement("li");
        li.textContent = "Notification - " + messages[i];
        document.getElementById("messageList").appendChild(li);
    }
});

function success() {
    document.getElementById("sendButton").disabled = false;
    hubConnection.send("LoadMessages");
}

function failure() {

}

hubConnection.start().then(success, failure);