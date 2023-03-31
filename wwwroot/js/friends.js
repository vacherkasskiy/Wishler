let requestsBtn = document.querySelectorAll(".request__btn");
let friendsBtn = document.querySelectorAll(".friend__btn");

function acceptRequest(requestId) {
    $.ajax({
        type: "POST",
        url: "/AcceptRequest",
        data: {
            requestId: requestId
        }
    });
}

function declineRequest(requestId) {
    $.ajax({
        type: "DELETE",
        url: "/DeclineRequest",
        data: {
            requestId: requestId
        }
    });
}

function deleteFriend(friendId) {
    $.ajax({
        type: "DELETE",
        url: "/DeleteFriend",
        data: {
            friendId: friendId
        }
    });
}

for (let i = 0; i < requestsBtn.length; ++i) {
    requestsBtn[i].addEventListener("click", (event) => {
        if (event.target.classList.contains("accept-request")) {
            acceptRequest(event.target.id);
            declineRequest(event.target.id);
        } else {
            declineRequest(event.target.id);
        }

        setTimeout(() => {
            window.location.reload();
        }, 100);
    }, true);
}

for (let i = 0; i < friendsBtn.length; ++i) {
    friendsBtn[i].addEventListener("click", (event) => {
        deleteFriend(event.target.id);

        setTimeout(() => {
            window.location.reload();
        }, 100);
    }, true);
}