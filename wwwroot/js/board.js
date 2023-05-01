let accessSettingsButton = document.querySelector(".board__access-settings");
let changeVisibilityBlock = document.querySelector(".board__change-visibility");
let shadow = document.querySelector(".shadow");
let changeVisibilityButtons = document.querySelectorAll(".change-visibility__status");

accessSettingsButton.addEventListener("click", ()=> {
    changeVisibilityBlock.style.display = "flex";
    shadow.classList.add("active");
},true);

shadow.addEventListener("click", ()=> {
    changeVisibilityBlock.style.display = "none";
    shadow.classList.remove("active");
},true);

for (let i = 0; i < changeVisibilityButtons.length; ++i) {
    changeVisibilityButtons[i].addEventListener("click", (event)=> {
        
    },true);
}