// Create connection
var hubConnection = new signalR.HubConnectionBuilder().withUrl("/hubs/deathlyHallow").build();

// Connect to methods that hub invokes aka recives the notification from hub.
// Called when hub sends the notification to the connected client.
hubConnection.on("updateDeathlyHollowsCount", (cloak, stone, wand) => {
    document.getElementById("cloakCounter").innerText = cloak.toString();
    document.getElementById("stoneCounter").innerText = stone.toString();
    document.getElementById("wandCounter").innerText = wand.toString();
});

function success() {
    console.log("Vote success");
    hubConnection.invoke("GetRaceStatus").then((value) => {
        document.getElementById("cloakCounter").innerText = value.cloak.toString();
        document.getElementById("stoneCounter").innerText = value.stone.toString();
        document.getElementById("wandCounter").innerText = value.wand.toString();
    })
}

function fail() {
    console.log("Vote fail");
}

hubConnection.start().then(success, fail);