let requests = document.querySelectorAll(".request__btn");

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

for (let i = 0; i < requests.length; ++i) {
    requests[i].addEventListener("click", (event) => {
        if (event.target.classList.contains("accept-request")) {
            acceptRequest(event.target.id);
        }
        declineRequest(event.target.id);
        event.target.parentNode.remove();
    }, true);
}