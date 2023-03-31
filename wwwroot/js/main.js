window.addEventListener('scroll', function () {
    if (this.window.pageYOffset == 0) {
        header.classList.remove("visible");
        this.setTimeout(() => {
            header.classList.add("invisible");
        }, 600);
    } else {
        header.classList.remove("invisible");
        this.setTimeout(() => {
            header.classList.add("visible");
        }, 0);
    }
});

var hiddenText = document.querySelectorAll(".hidden-text");

for (var i = 0; i < hiddenText.length; ++i) {
    hiddenText[i].querySelector(".click-to-activate").addEventListener("click", (event) => {
        var dropdown = event.target.parentNode.parentNode.querySelector(".hidden-dropdown");
        if (dropdown.style.height == 0 || dropdown.style.height == "0px") {
            dropdown.style.height = dropdown.children[0].offsetHeight + dropdown.children[1].offsetHeight + "px";
        } else {
            dropdown.style.height = 0;
        }
    }, true);
}