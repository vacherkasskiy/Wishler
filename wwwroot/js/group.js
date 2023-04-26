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

saveButton.addEventListener("click", (event) => {
    let wish = document.querySelector(".wish textarea").value;
    let participantId = document.querySelector(".wish textarea").id;
    SaveWish(participantId, wish);
},true);