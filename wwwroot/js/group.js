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

function AddMember(groupId, userId) {
    $.ajax({
        type: "POST",
        url: "/group/addMember",
        data: {
            groupId: groupId,
            userId: userId
        },
    });
}

let wishBlock = document.querySelector(".wish");
let saveButton = document.querySelector(".wish button");
let wishTextarea = document.querySelector(".wish textarea");
let wishLabel = document.querySelector(".wish > form > label");
let newMemberForm = document.querySelector(".new-member__form");
let eventButton = document.querySelector(".event");
let addNewMemberButton = document.querySelector(".add-new-member");
let addMemberButtons = document.querySelectorAll(".member__delete-btn.add-member-btn");
let shadow = document.querySelector(".shadow");

addNewMemberButton?.addEventListener("click", ()=> {
    shadow.classList.add("active");
    wishTextarea.style.display = "none";
    wishLabel.style.display = "none";
    newMemberForm.style.display = "block";
    saveButton.style.display = "none";
    wishBlock.classList.add("active");
},true);

shadow.addEventListener("click", ()=> {
    shadow.classList.remove("active");
    wishTextarea.style.display = "block";
    wishLabel.style.display = "block";
    newMemberForm.style.display = "none";
    saveButton.style.display = "block";
    wishBlock.classList.remove("active");
},true);

for (let i = 0; i < addMemberButtons.length; ++i) {
    addMemberButtons[i].addEventListener("click", (event)=> {
        if (eventButton == null) {
            AddMember(document.querySelector(".event-blocked").id, event.target.id);
        }
        else {
            AddMember(eventButton.id, event.target.id);
        }
        setTimeout(() => {
            location.reload();
        }, 0);
    },true);
}

eventButton?.addEventListener("click", () => {
    if (eventButton.classList.contains("start")) {
        StartEvent(eventButton.id);
    } else {
        CancelEvent(eventButton.id);
    }
    setTimeout(() => {
        location.reload();
    }, 100);
}, true);

saveButton?.addEventListener("click", () => {
    let wish = wishTextarea.value;
    let participantId = wishTextarea.id;
    wishTextarea.style.border = "2.5px solid #3059fa";
    SaveWish(participantId, wish);
}, true);

wishTextarea?.addEventListener("input", () => {
    wishTextarea.style.border = "2.5px solid #a3a3a3";
}, true);
