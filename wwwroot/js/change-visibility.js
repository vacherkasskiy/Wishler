function changeVisibilityStatus(boardId, status) {
    $.ajax({
        type: "PATCH",
        url: "/board/changeVisibility",
        data: {
            boardId: boardId,
            status: status
        },
    });
}

let accessSettingsButton = document.querySelector(".board__access-settings");
let changeVisibilityBlock = document.querySelector(".board__change-visibility");
let shadow = document.querySelector(".shadow");
let changeVisibilityButtons = document.querySelectorAll(".change-visibility__status");
let boardId = document.querySelector(".board").id;

accessSettingsButton.addEventListener("click", ()=> {
    changeVisibilityBlock.style.display = "flex";
    shadow.classList.add("active");
},true);

shadow.addEventListener("click", ()=> {
    changeVisibilityBlock.style.display = "none";
    shadow.classList.remove("active");
},true);

for (let i = 0; i < changeVisibilityButtons.length; ++i) {
    changeVisibilityButtons[i].addEventListener("click", (event)=> {
        document.querySelector(".selected-status").classList.remove("selected-status");
        event.target.classList.add("selected-status");
        
        if (event.target.classList.contains("private")) {
            changeVisibilityStatus(boardId, "private");
        } else if (event.target.classList.contains("for-friends")) {
            changeVisibilityStatus(boardId, "friends");
        } else {
            changeVisibilityStatus(boardId, "everybody");
        }
    },true);
}