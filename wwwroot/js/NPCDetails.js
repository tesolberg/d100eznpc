
// Scroll position

var scrollPositionInput = document.getElementById('scrollPosition');

var skillButtons = document.querySelectorAll(".btn-skill");
for (var i = 0; i < skillButtons.length; i++) {
    skillButtons[i].addEventListener("click", function (e) {
        scrollPositionInput.value = window.scrollY;
    })
}



const testButton = document.querySelector("#post-test");


$(function () {
    $('#post-test').on('click', function (evt) {
        evt.preventDefault();
        $.post('', $('form').serialize(), function () {
            alert('Posted using jQuery');
        });
    });
});

//testButton.addEventListener("click", testAjax);


function testAjax() {
    $.ajax({
            url: "/NPCDetails?handler=AjaxTest",
            data: { id: "testId"},
            type: "GET",
            success: function (data) {
                console.log("success");
            },
            error: function () {
                console.log("error");
            }
        }
    )
}

