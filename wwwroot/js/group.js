function SaveWish(participantId, wish) {
    $.ajax({
        type: "POST",
        url: "/group/saveWish",
        data: {
            participantId: participantId,
            wish: wish
        },
    });
}

function StartEvent(groupId) {
    $.ajax({
        type: "PATCH",
        url: "/group/startEvent",
        data: {
            groupId: groupId
        },
    });
}

function CancelEvent(groupId) {
    $.ajax({
        type: "PATCH",
        url: "/group/cancelEvent",
        data: {
            groupId: groupId
        },
    });
}

let saveButton = document.querySelector(".wish button");
let wishTextarea = document.querySelector(".wish textarea");
let eventButton = document.querySelector(".event");

eventButton?.addEventListener("click", () => {
    if (eventButton.classList.contains("start")) {
        StartEvent(eventButton.id);
    } else {
        CancelEvent(eventButton.id);
    }
    setTimeout(() => {
        location.reload();
    },100);
},true);

saveButton.addEventListener("click", () => {
    let wish = wishTextarea.value;
    let participantId = wishTextarea.id;
    wishTextarea.style.border = "2.5px solid #3059fa";
    SaveWish(participantId, wish);
},true);

wishTextarea.addEventListener("input", () => {
    wishTextarea.style.border = "2.5px solid #a3a3a3";
},true);
