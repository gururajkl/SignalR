var hubConnection = new signalR.HubConnectionBuilder().withUrl("/hubs/houseGroupHub").build();

let lbl_houseJoined = document.getElementById("lbl_houseJoined");

let btn_un_gryffindor = document.getElementById("btn_un_gryffindor");
let btn_un_slytherin = document.getElementById("btn_un_slytherin");
let btn_un_hufflepuff = document.getElementById("btn_un_hufflepuff");
let btn_un_ravenclaw = document.getElementById("btn_un_ravenclaw");

let btn_gryffindor = document.getElementById("btn_gryffindor");
let btn_slytherin = document.getElementById("btn_slytherin");
let btn_hufflepuff = document.getElementById("btn_hufflepuff");
let btn_ravenclaw = document.getElementById("btn_ravenclaw");

let trigger_gryffindor = document.getElementById("trigger_gryffindor");
let trigger_slytherin = document.getElementById("trigger_slytherin");
let trigger_hufflepuff = document.getElementById("trigger_hufflepuff");
let trigger_ravenclaw = document.getElementById("trigger_ravenclaw");

// Click listener for the subs button.
btn_gryffindor.addEventListener("click", (event) => {
    event.preventDefault();
    hubConnection.send("JoinHouse", "Gryffindor");
});
btn_slytherin.addEventListener("click", (event) => {
    event.preventDefault();
    hubConnection.send("JoinHouse", "Slytherin");
});
btn_hufflepuff.addEventListener("click", (event) => {
    event.preventDefault();
    hubConnection.send("JoinHouse", "Hufflepuff");
});
btn_ravenclaw.addEventListener("click", (event) => {
    event.preventDefault();
    hubConnection.send("JoinHouse", "Ravenclaw");
});

// Click listener for the Unsubs button.
btn_un_gryffindor.addEventListener("click", (event) => {
    event.preventDefault();
    hubConnection.send("LeaveHouse", "Gryffindor");
});
btn_un_slytherin.addEventListener("click", (event) => {
    event.preventDefault();
    hubConnection.send("LeaveHouse", "Slytherin");
});
btn_un_hufflepuff.addEventListener("click", (event) => {
    event.preventDefault();
    hubConnection.send("LeaveHouse", "Hufflepuff");
});
btn_un_ravenclaw.addEventListener("click", (event) => {
    event.preventDefault();
    hubConnection.send("LeaveHouse", "Ravenclaw");
});

// Trigger button.
trigger_gryffindor.addEventListener("click", (event) => {
    event.preventDefault();
    hubConnection.send("TriggerNofitication", "Gryffindor");
});
trigger_hufflepuff.addEventListener("click", (event) => {
    event.preventDefault();
    hubConnection.send("TriggerNofitication", "Hufflepuff")
});
trigger_ravenclaw.addEventListener("click", (event) => {
    event.preventDefault();
    hubConnection.send("TriggerNofitication", "Ravenclaw")
});
trigger_slytherin.addEventListener("click", (event) => {
    event.preventDefault();
    hubConnection.send("TriggerNofitication", "Slytherin")
});

hubConnection.on("subscriptionStatus", (strGroupsJoined, houseName, hasSubscribed) => {
    lbl_houseJoined.innerText = strGroupsJoined;

    if (hasSubscribed) {
        // Subscribed
        switch (houseName) {
            case 'slytherin':
                btn_slytherin.style.display = "none";
                btn_un_slytherin.style.display = "";
                break;
            case 'ravenclaw':
                btn_ravenclaw.style.display = "none";
                btn_un_ravenclaw.style.display = "";
                break;
            case 'hufflepuff':
                btn_hufflepuff.style.display = "none";
                btn_un_hufflepuff.style.display = "";
                break;
            case 'gryffindor':
                btn_gryffindor.style.display = "none";
                btn_un_gryffindor.style.display = "";
                break;
            default: break;
        }
        toastr.success(`You have subscribed to ${houseName}`);
    } else {
        // UnSubscribed
        switch (houseName) {
            case 'slytherin':
                btn_slytherin.style.display = "";
                btn_un_slytherin.style.display = "none";
                break;
            case 'ravenclaw':
                btn_ravenclaw.style.display = "";
                btn_un_ravenclaw.style.display = "none";
                break;
            case 'hufflepuff':
                btn_hufflepuff.style.display = "";
                btn_un_hufflepuff.style.display = "none";
                break;
            case 'gryffindor':
                btn_gryffindor.style.display = "";
                btn_un_gryffindor.style.display = "none";
                break;
            default: break;
        }
        toastr.success(`You have Unsubscribed to ${houseName}`);
    }
});

hubConnection.on("subscriptionStatusToast", (value) => {
    toastr.success(`Memeber as subscribed to ${value}`)
});

hubConnection.on("unSubscriptionStatusToast", (value) => {
    toastr.warning(`Memeber as Unsubscribed to ${value}`)
});

hubConnection.on("triggerNotification", (value) => {
    toastr.success(`Notification has been triggered for ${value}`);
    console.log("Triggered");
});

function success() {
    console.log("Connection Success");
}

function failure() {
    console.log("Connection Failure");
}

hubConnection.start().then(success, failure);