var elements = document.querySelectorAll(".redirect-button");
for (var i = 0; i < elements.length; i++) {
    elements[i].onclick = function(){
        window.location.href = 'register.html';
    };
}