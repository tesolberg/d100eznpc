
var scrollPositionInput = document.getElementById('scrollPosition');

var skillButtons = document.querySelectorAll(".btn-skill");
for (var i = 0; i < skillButtons.length; i++) {
    skillButtons[i].addEventListener("click", function (e) {
        scrollPositionInput.value = window.scrollY;
    })
}
