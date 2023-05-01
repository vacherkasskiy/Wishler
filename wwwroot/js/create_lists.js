var creatingList = document.querySelector(".create-column");

// NEW LIST DISCRIPTION

function createNewColumn() {

    var newColumn = document.createElement("section");
    var header = document.createElement("textarea");
    var newList = document.createElement("ul");
    var createNewRowButton = document.createElement("div");
    var createNewRowText = document.createElement("p");

    newColumn.classList.add("column");
    header.classList.add("column__name");
    header.maxLength = 25;
    newList.classList.add("tasks__list");
    createNewRowButton.classList.add("new");
    createNewRowButton.classList.add("create-row");

    createNewRowText.innerHTML = "Create new row";
    createNewRowButton.appendChild(createNewRowText);
    newList.appendChild(createNewRowButton);
    newColumn.appendChild(header);
    newColumn.appendChild(newList);

    // document.querySelector("body").appendChild(newColumn);

    return newColumn;
}

// POSSIBILITY TO CREATE NEW LISTS

function enableCreatingList(list) {
    list.addEventListener("click", (event) => {
        var newColumn = createNewColumn();
        newColumn.draggable = true;
        enableCreatingRow(newColumn.querySelector(".create-row")); // src = create-items.js
        makeListItemsDraggable(newColumn.querySelector(".tasks__list"));
        var target = event.target;
        while (!target.classList.contains("create-column")) {
            target = target.parentNode;
        }
        target.parentNode.insertBefore(newColumn, target);
        giveTextareaAllStyles(newColumn.querySelector(".column__name"));
        newColumn.querySelector(".column__name").focus();
    }, true);
}

enableCreatingList(creatingList);