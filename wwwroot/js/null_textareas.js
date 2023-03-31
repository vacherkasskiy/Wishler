var textareas = document.querySelectorAll("textarea");

function addRowToDb(id, columnId, position, text, target) {
    $.ajax({
        type: "POST",
        url: "/EditOrCreateRow",
        data: {
            id: id,
            columnId: columnId,
            position: position,
            text: text,
        },
        success: function (data) {
            target.id = data.cellId;
        }
    });
}

function addColumnToDb(id, name, boardId, position, target) {
    $.ajax({
        type: "POST",
        url: "/EditOrCreateColumn",
        data: {
            id: id,
            name: name,
            boardId: boardId,
            position: position,
        },
        success: function (data) {
            target.id = data.colId;
        }
    });
}

function deleteRow(id) {
    $.ajax({
        type: "DELETE",
        url: "/board/deleteRow",
        data: {
            id: id,
        },
    });
}

function deleteColumn(id) {
    $.ajax({
        type: "DELETE",
        url: "/board/deleteColumn",
        data: {
            id: id,
        },
    });
}

function getElementIndex(elem) {
    elem = elem.tagName ? elem : document.querySelector(elem)
    return [].indexOf.call(elem.parentNode.children, elem)
}

function giveTextareaAllStyles(textarea) {

    if (textarea.parentNode.classList.contains("tasks__item")) {
        textarea.readOnly = true;
        var img = document.createElement("img");
        img.src = "https://cdn-icons-png.flaticon.com/512/1159/1159633.png";

        img.addEventListener("click", (event) => {
            event.target.previousElementSibling.readOnly = false;
            event.target.previousElementSibling.focus();
            event.target.parentNode.style.zIndex = 1000;
            document.querySelector(".shadow").classList.add("active");
        }, true);

        textarea.addEventListener("contextmenu", (event) => {
            event.preventDefault();
            event.target.readOnly = false;
            event.target.focus();
            event.target.parentNode.style.zIndex = 1000;
            document.querySelector(".shadow").classList.add("active");
        }, true);

        textarea.parentNode.addEventListener("contextmenu", (event) => {
            event.preventDefault();
            var target = event.target.childNodes[0];
            target.readOnly = false;
            target.focus();
            target.parentNode.style.zIndex = 1000;
            document.querySelector(".shadow").classList.add("active");
        }, true);

        textarea.addEventListener("focusout", (event) => {
            event.target.readOnly = true;
            event.target.parentNode.style.zIndex = 0;
            document.querySelector(".shadow").classList.remove("active");
            if (event.target.value === "") {
                event.target.parentNode.remove();
                deleteRow(event.target.id);
            } else {
                let position = getElementIndex(event.target.parentNode);
                let columnId = event.target.parentNode.parentNode.parentNode.querySelector("textarea").id;
                addRowToDb(event.target.id, columnId, position, event.target.value, event.target);
            }
        }, true);

        textarea.addEventListener("keypress", (event) => {
            if (event.key === "Enter") {
                event.preventDefault();
                event.target.readOnly = true;
                event.target.parentNode.style.zIndex = 0;
                document.querySelector(".shadow").classList.remove("active");
                event.target.blur();
            }
        }, true);

        textarea.parentNode.appendChild(img);

    } else {

        textarea.addEventListener("focusin", (event) => {
            event.target.style.zIndex = 1000;
            document.querySelector(".shadow").style.zIndex = 999;
        }, true);

        textarea.addEventListener("focusout", (event) => {
            if (event.target.value === "") {
                var column = event.target.parentNode;
                if (column.querySelectorAll("li").length == 0) {
                    document.querySelector(".shadow").style.zIndex = -1;
                    event.target.parentNode.remove();
                    deleteColumn(event.target.id);
                    return;
                }
                setTimeout(event.target.focus(), 1);
            } else {
                event.target.style.zIndex = 0;
                document.querySelector(".shadow").style.zIndex = -1;
                let position = getElementIndex(event.target.parentNode);
                let boardId = event.target.parentNode.parentNode.id;

                addColumnToDb(event.target.id, event.target.value, boardId, position, event.target);
            }
        }, true);

        textarea.addEventListener("keypress", (event) => {
            if (event.key === "Enter") {
                event.preventDefault();
                var column = event.target.parentNode;
                if (event.target.value !== "" || column.querySelectorAll("li").length == 0) {
                    event.target.blur();
                }
            }
        }, true);

    }

    textarea.style.height = textarea.scrollHeight + 1 + "px";
    textarea.addEventListener("input", (event) => {
        event.target.style.height = "";
        event.target.style.height = event.target.scrollHeight + 1 + "px";
    }, true);

}

for (var i = 0; i < textareas.length; ++i) {
    giveTextareaAllStyles(textareas[i]);
}