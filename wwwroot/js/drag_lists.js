const board = document.querySelector(`.board`);
const columns = board.querySelectorAll(`.column`);

for (const task of columns) {
  task.draggable = true;
}

board.addEventListener(`dragstart`, (evt) => {
  if (evt.target.classList.contains("column")) {
    evt.target.classList.add(`selected_column`);
  }
});

board.addEventListener(`dragend`, (evt) => {
  if (evt.target.classList.contains("column")) {
    evt.target.classList.remove(`selected_column`);
    
    var lists = board.querySelectorAll(".column");
    var boardId = 1; // change it later

    for (let i = 0; i < lists.length; ++i) {
      var id = lists[i].querySelector("textarea").id;
      var name = lists[i].querySelector("textarea").value;
      var position = getElementIndex(lists[i]);

      addColumnToDb(id, name, boardId, position, lists[i].querySelector("textarea"));
    }
  }
});

board.addEventListener(`dragover`, (evt) => {
  evt.preventDefault();
  
  if (document.querySelector(".selected") != null) {
    return;
  }
  
  const activeElement = board.querySelector(`.selected_column`);
  const currentElement = evt.target;
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
