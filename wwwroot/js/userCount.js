/* JavaScript Client of the SignalR. */

// Create connection
var connectionUserCount = new signalR.HubConnectionBuilder().
    withUrl("/hubs/userCount", signalR.HttpTransportType.WebSockets).build();

// Connect to methods that hub invokes aka recives the notification from hub.
// Called when hub sends the notification to the connected client.
connectionUserCount.on("updateTotalViews", (value) => {
    var newCountSpan = document.getElementById("totalViewsCounter");
    newCountSpan.innerText = value.toString();
});

connectionUserCount.on("updateTotalUsers", (value) => {
    var newUserCountSpan = document.getElementById("totalUsersCounter");
    newUserCountSpan.innerText = value.toString();
});

// Invoke the hub method aka send notification to the hub.
function newWindowLoadedOnClient() {
    connectionUserCount.invoke("NewWindowLoaded", "Guru").then((value) => console.log(value));
}

// Start connection.
function fullFilled() {
    // Do something, once the connection is done (Promise).
    console.log("Connection started");
    newWindowLoadedOnClient();
}

function rejected() {
    // Do something, once the connection did not established (Promise).
    console.log("Connection not started");
}

connectionUserCount.start().then(fullFilled, rejected);