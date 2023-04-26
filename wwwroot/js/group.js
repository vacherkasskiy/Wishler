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

let saveButton = document.querySelector(".wish button");
let wishTextarea = document.querySelector(".wish textarea");
let startEventButton = document.querySelector(".start-event");

startEventButton.addEventListener("click", () => {
    
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
