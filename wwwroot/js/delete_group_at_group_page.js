let deleteGroupButton = document.querySelector(".wrapper > .delete.group");
let popUpBlock = document.querySelector(".pop-up");
let popUpCancelButton = document.querySelector(".pop-up__buttons > .cancel");

deleteGroupButton?.addEventListener("click", () => {
    popUpBlock.style.display = "flex";
    shadow.classList.add("active");
}, true);

shadow.addEventListener("click", () => {
    popUpBlock.style.display = "none";
    shadow.classList.remove("active");
}, true);

popUpCancelButton.addEventListener("click", () => {
    popUpBlock.style.display = "none";
    shadow.classList.remove("active");
}, true);