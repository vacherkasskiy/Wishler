let changeInfoButton = document.querySelector(".change-info-button");
let changeInfoForm = document.querySelector(".change-info-form");
let shadow = document.querySelector(".shadow");
let cancelButton = document.querySelector(".change-info-form .buttons .cancel-btn");
let backgroundImages = document.querySelector(".avatars-images").children;
let body = document.querySelector("body");

changeInfoButton.addEventListener("click", (event) => {
    changeInfoButton.classList.remove("active");
    changeInfoForm.classList.add("active");
    shadow.classList.add("active");
    body.classList.add("noscroll");
}, true);

shadow.addEventListener("click", (event) => {
    changeInfoButton.classList.add("active");
    changeInfoForm.classList.remove("active");
    shadow.classList.remove("active");
    body.classList.remove("noscroll")
}, true);

cancelButton.addEventListener("click", (event) => {
    changeInfoButton.classList.add("active");
    changeInfoForm.classList.remove("active");
    shadow.classList.remove("active");
    body.classList.remove("noscroll")
}, true);

for (let i = 0; i < backgroundImages.length; ++i) {
    backgroundImages[i].addEventListener("click", (event) => {
        document.getElementById("avatar_id").value = event.target.id;
        document.querySelector(".selected")?.classList.remove("selected");
        event.target.classList.add("selected");
    }, true);
}
