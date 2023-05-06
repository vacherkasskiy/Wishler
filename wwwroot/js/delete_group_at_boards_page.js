let deleteGroupButtons = document.querySelectorAll(".groups .delete");
let popUpBlock = document.querySelector(".pop-up");
let popUpDeleteButton = document.querySelector(".pop-up__buttons > .delete");
let popUpCancelButton = document.querySelector(".pop-up__buttons > .cancel");
let popUpObjName = document.querySelector(".pop-up > p > span");

for (let i = 0; i < deleteGroupButtons.length; ++i) {
    deleteGroupButtons[i].addEventListener("click", (event) => {
        popUpObjName.innerHTML = event.target.id;
        let groupId = event.target.parentNode.parentElement.id;
        popUpDeleteButton.href = "group/delete/" + groupId;
        popUpBlock.style.display = "flex";
        shadow.classList.add("active");
    }, true);
}

shadow.addEventListener("click", () => {
    popUpBlock.style.display = "none";
    shadow.classList.remove("active");
}, true);

popUpCancelButton.addEventListener("click", () => {
    popUpBlock.style.display = "none";
    shadow.classList.remove("active");
}, true);