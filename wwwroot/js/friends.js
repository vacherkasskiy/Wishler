let requests = document.querySelectorAll(".accept-request");

function acceptRequest(requestId) {
    $.ajax({
        type: "POST",
        url: "/AcceptRequest",
        data: {
            requestId: requestId
        }
    });
}

for (let i = 0; i < requests.length; ++i) {
    requests[i].addEventListener("click", (event) => {
        acceptRequest(event.target.id);
    } ,true);
}