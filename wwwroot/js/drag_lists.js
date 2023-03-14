const board = document.querySelector(`.board`);
const columns = board.querySelectorAll(`.column`);

for (const task of columns) {
  task.draggable = true;
}

board.addEventListener(`dragstart`, (evt) => {
  evt.target.classList.add(`selected_column`);
});

board.addEventListener(`dragend`, (evt) => {
  evt.target.classList.remove(`selected_column`);
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
