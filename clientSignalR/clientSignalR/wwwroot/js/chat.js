"use strict";

//var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:8099/CustomersHub").build();
//var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:5224/CustomersHub").build();
var connection = new signalR.HubConnectionBuilder().withUrl("http://192.168.72.112:5554/CustomersHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;
connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.on("OnRefreshInstrumentBestLimit", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} ${message}`;
});

connection.on("OnChangeTrades", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} ${message}`;
});

connection.on("OnChangeOrderState", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} ${message}`;
});

connection.start().then(function () {
    console.log("socket resetart.");

    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    //return console.error(err.toString());
});
connection.onclose(function () {
    console.log("socket onreconnecting.");

    setInterval(function () {
        connection.start().then(function () {
            console.log("socket resetart.");

            document.getElementById("sendButton").disabled = false;
        }).catch(function (err) {
            //return console.error(err.toString());
        });
    }, 5000);
});
//connection.disconnected().then(function () {
//    console.log("socket disconnected.");

//    setTimeout(function () {
//        connection.start().then(function () {
//            console.log("socket resetart.");
//            document.getElementById("sendButton").disabled = false;

//        }).catch(function (err) {
//            return console.error(err.toString());
//        })
//    }, 5000); // Restart connection after 5 seconds.
//});
document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});