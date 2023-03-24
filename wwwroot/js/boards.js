let createNewBoardBtn = document.querySelector(".new-board");
let formWrapper = document.querySelector(".new-board-box");
let cancelBtn = formWrapper.querySelector(".cancel");

createNewBoardBtn.addEventListener("click", function (event) {
    formWrapper.style.display = 'flex';
}, true);

cancelBtn.addEventListener("click", function (event) {
    formWrapper.style.display = 'none';
}, true);