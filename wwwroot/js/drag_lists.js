const board = document.querySelector(`.board`);
const columns = board.querySelectorAll(`.column`);

for (const task of columns) {
    task.draggable = true;
}

board.addEventListener(`dragstart`, (event) => {
    if (event.target.classList.contains("column")) {
        event.target.classList.add(`selected_column`);
    }
});

board.addEventListener(`dragend`, (event) => {
    if (event.target.classList.contains("column")) {
        event.target.classList.remove(`selected_column`);

        let lists = board.querySelectorAll(".column");
        let boardId = board.id;

        for (let i = 0; i < lists.length; ++i) {
            let id = lists[i].querySelector("textarea").id;
            let name = lists[i].querySelector("textarea").value;
            let position = getElementIndex(lists[i]);

            addColumnToDb(id, name, boardId, position, lists[i].querySelector("textarea"));
        }
    }
});

board.addEventListener(`dragover`, (event) => {
    event.preventDefault();

    if (document.querySelector(".selected") != null) {
        return;
    }

    const activeElement = board.querySelector(`.selected_column`);
    const currentElement = event.target;
    const isMoveable = activeElement !== currentElement &&
        currentElement.classList.contains(`column`);

    if (!isMoveable) {
        return;
    }

    const nextElement = (currentElement === activeElement.nextElementSibling) ?
        currentElement.nextElementSibling :
        currentElement;

    board.insertBefore(activeElement, nextElement);
});
