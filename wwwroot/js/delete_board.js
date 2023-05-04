let deleteBoardButtons = document.querySelectorAll(".board .delete");

for (let i = 0; i < deleteBoardButtons.length; ++i) {
    deleteBoardButtons[i].addEventListener("click", (event) => {
        popUpObjName.innerHTML = event.target.id;
        let groupId = event.target.parentNode.parentElement.id;
        popUpDeleteButton.href = "boards/delete/" + groupId;
        popUpBlock.style.display = "flex";
        shadow.classList.add("active");
    }, true);
}