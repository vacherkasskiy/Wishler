var creatingRows = document.querySelectorAll(".create-row");

// NEW ROW DISCRIPTION

function createNewRow() {

    var newRow = document.createElement("li");
    var newTextarea = document.createElement("textarea");

    newRow.draggable = true;
    newRow.classList.add("tasks__item");
    newTextarea.wrap = "soft";
    newTextarea.name = "text";
    newRow.appendChild(newTextarea);

    return newRow;
}

// POSSIBILITY TO CREATE NEW ROWS

function enableCreatingRow(creatingRow) {

    creatingRow.addEventListener("click", (event) => {
        var target = event.target;
        while (!target.classList.contains("create-row")) {
            target = target.parentNode;
        }
        var toInsert = createNewRow();
        giveTextareaAllStyles(toInsert.childNodes[0]); // src = null_textareas.js
        target.parentNode.insertBefore(toInsert, target);
        var createdRow = target.previousElementSibling;
        createdRow.querySelector("img").click();
    }, true);

}

for (var i = 0; i < creatingRows.length; ++i) {
    enableCreatingRow(creatingRows[i]);
}