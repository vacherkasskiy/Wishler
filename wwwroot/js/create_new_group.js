let newGroupForm = document.querySelector(".new-group-form");
let createGroupButton = document.querySelector(".groups__create-new");
let cancelButton = document.querySelector("button.cancel");
let memberButtons = document.querySelectorAll(".member button");
let invitedMembersEmails = document.querySelector(".new-group-form .invited-members")

createGroupButton.addEventListener("click", () => {
    newGroupForm.classList.add("active");
    shadow.classList.add("active");
    createGroupButton.style.display = "none";
}, true);

shadow.addEventListener("click", () => {
    newGroupForm.classList.remove("active");
    shadow.classList.remove("active");
    createGroupButton.style.display = "inline-block";
}, true);

cancelButton.addEventListener("click", () => {
    newGroupForm.classList.remove("active");
    shadow.classList.remove("active");
    createGroupButton.style.display = "inline-block";
}, true);

for (let i = 0; i < memberButtons.length; ++i) {
    memberButtons[i].addEventListener("click", (event) => {
        if (event.target.classList.contains("invite")) {
            event.target.classList.remove("invite");
            event.target.classList.add("delete");
            event.target.innerHTML = "Delete";
            invitedMembersEmails.value += event.target.id + " ";
        } else {
            event.target.classList.add("invite");
            event.target.classList.remove("delete");
            event.target.innerHTML = "Invite";
            invitedMembersEmails.value = "";
            let emails = document.querySelectorAll(".member button.delete");
            for (let i = 0; i < emails.length; ++i) {
                invitedMembersEmails.value += emails[i].id + " ";
            }
        }
    }, true);
}