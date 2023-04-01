let newBoardForm = document.querySelector(".new-board-form");
let createCancelButtons = document.querySelector(".new-board-form .buttons").children;
let backgroundImages = document.querySelector(".new-board__backgrounds").children;
let newBoardButton = document.querySelector(".new-board");
let shadow = document.querySelector(".shadow");

newBoardButton.addEventListener("click", (event) => {
    newBoardButton.classList.add("hidden");
    shadow.classList.add("active");
    newBoardForm.classList.add("active");
}, true);

shadow.addEventListener("click", (event) => {
    newBoardForm.classList.remove("active");
    newBoardButton.classList.remove("hidden");
    event.target.classList.remove("active");
}, true);

for (let i = 0; i < createCancelButtons.length; ++i) {
    createCancelButtons[i].addEventListener("click", (event) => {
        newBoardForm.classList.remove("active");
        newBoardButton.classList.remove("hidden");
        shadow.classList.remove("active");
    }, true);
}

for (let i = 0; i < backgroundImages.length; ++i) {
    backgroundImages[i].addEventListener("click", (event) => {
        document.getElementById("ps_input").value = event.target.id;
        document.querySelector(".selected")?.classList.remove("selected");
        event.target.classList.add("selected");
    }, true);
}