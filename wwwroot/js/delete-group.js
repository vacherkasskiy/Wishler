let deleteGroupButtons = document.querySelectorAll(".groups .delete");
let popUpBlock = document.querySelector(".pop-up");
let popUpDeleteButton = document.querySelector(".pop-up__buttons > .delete");

for (let i = 0; i < deleteGroupButtons.length; ++i) {
    deleteGroupButtons[i].addEventListener("click", (event) => {
        popUpBlock.style.display = "flex";
        
    }, true);
}