var hubConnection = new signalR.HubConnectionBuilder().withUrl("/hubs/chat").withAutomaticReconnect([0, 1000, 5000, null]).build();

hubConnection.on("receiveUserConnected", (userId, userName) => {
    addMessage(`${userName} has a connection open.`);
});

hubConnection.on("receiveUserDisconnected", (userId, userName) => {
    addMessage(`${userName} has closed a connection.`);
});

hubConnection.on("receiveAddRoomMessage", (maxRoom, roomId, roomName, userId, userName) => {
    addMessage(`${userName} has created room ${roomName}.`);
    fillRoomDropDown();
});

hubConnection.on("receiveDeleteRoomMessage", (maxRoom, roomId, roomName, userId, userName) => {
    addMessage(`${userName} has deleted room ${roomName}.`);
    fillRoomDropDown();
});

function deleteRoom() {
    let ddlDeleteRoom = document.getElementById('ddlDelRoom');

    var roomName = ddlDeleteRoom.options[ddlDeleteRoom.selectedIndex].text;

    var text = `Do you want to delete chat room ${roomName}?`;
    if (confirm(text) == false) {
        return;
    }

    if (roomName == null && roomName == '') {
        return;
    }

    var roomId = ddlDeleteRoom.value;

    /* DELETE */
    $.ajax({
        url: `/ChatRooms/DeleteChatRoom/${roomId}`,
        dataType: "json",
        type: "DELETE",
        contentType: 'application/json;',
        async: true,
        processData: false,
        cache: false,
        success: function (json) {
            /* DELETE ROOM COMPLETED SUCCESSFULLY */
            hubConnection.invoke("SendDeleteRoomMessage", json.deleted, json.selected, roomName);
            fillRoomDropDown();
        },
        error: function (xhr) {
            alert('error');
        }
    })
}

function addnewRoom(maxRoom) {
    let createRoomName = document.getElementById('createRoomName');

    var roomName = createRoomName.value;

    if (roomName == null && roomName == '') {
        return;
    }

    /* POST */
    $.ajax({
        url: '/ChatRooms/PostChatRoom',
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ id: 0, name: roomName }),
        async: true,
        processData: false,
        cache: false,
        success: function (json) {
            /* ADD ROOM COMPLETED SUCCESSFULLY */
            hubConnection.invoke("SendAddRoomMessage", maxRoom, json.id, json.name);
            createRoomName.value = '';
        },
        error: function (xhr) {
            alert('error');
        }
    })
}

document.addEventListener('DOMContentLoaded', (event) => {
    fillRoomDropDown();
    fillUserDropDown();
})

function fillUserDropDown() {
    $.getJSON('/ChatRooms/GetChatUser')
        .done(function (json) {
            var ddlSelUser = document.getElementById("ddlSelUser");

            ddlSelUser.innerText = null;

            json.forEach(function (item) {
                var newOption = document.createElement("option");

                newOption.text = item.userName; // item.whateverProperty
                newOption.value = item.id;
                ddlSelUser.add(newOption);
            });

        })
        .fail(function (jqxhr, textStatus, error) {

            var err = textStatus + ", " + error;
            console.log("Request Failed: " + jqxhr.detail);
        });
}

function fillRoomDropDown() {
    $.getJSON('/ChatRooms/GetChatRoom')
        .done(function (json) {
            var ddlDelRoom = document.getElementById("ddlDelRoom");
            var ddlSelRoom = document.getElementById("ddlSelRoom");

            ddlDelRoom.innerText = null;
            ddlSelRoom.innerText = null;

            json.forEach(function (item) {
                var newOption = document.createElement("option");

                newOption.text = item.name;
                newOption.value = item.id;
                ddlDelRoom.add(newOption);

                var newOption1 = document.createElement("option");

                newOption1.text = item.name;
                newOption1.value = item.id;
                ddlSelRoom.add(newOption1);
            });
        })
        .fail(function (jqxhr, textStatus, error) {

            var err = textStatus + ", " + error;
            console.log("Request Failed: " + jqxhr.detail);
        });
}

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