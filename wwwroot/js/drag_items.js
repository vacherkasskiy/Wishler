var tasksListElements = document.querySelectorAll(".tasks__list");
var taskElements = document.querySelectorAll(".tasks__item");

for (var task of taskElements) {
  task.draggable = true;
}

function makeListItemsDraggable(list) {

    list.addEventListener("dragstart", (event) => {
        if (event.target.classList.contains("tasks__item")) {
            event.target.classList.add("selected");
        }
    });
    
    list.addEventListener("dragend", (event) => {
        event.target.classList.remove("selected");
    });
    
    list.addEventListener("dragover", (event) => {
        event.preventDefault();
        var activeElement = document.querySelector(".selected");
        var currentElement = event.target;
        var isMoveable = activeElement !== currentElement &&
            currentElement.classList.contains("tasks__item") ||
            currentElement.classList.contains("create-row"); 
        
        if (!isMoveable) {
            return;
        }

        const nextElement = (currentElement === activeElement.nextElementSibling) ?
		      currentElement.nextElementSibling :
		      currentElement;
    
        var toInsert = currentElement.parentNode;

        if (currentElement.classList.contains("tasks__item")) {
            toInsert.insertBefore(activeElement, nextElement);
        } else {
            toInsert.insertBefore(activeElement, currentElement);
        }
    });

}

for (var i = 0; i < tasksListElements.length; ++i) {
    makeListItemsDraggable(tasksListElements[i]);
}