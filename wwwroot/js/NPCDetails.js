
$(function () {
    AddSkillButtonListeners();
    AddEditAndSaveButtonListener();
});

// Edit NPC
const competenceNumbers = $(".competence-number");
const competenceEdits = $(".competence-edit")

function AddEditAndSaveButtonListener() {
    $("#edit-btn").on("click", (evt) => {
        competenceNumbers.css("display", "none");
        competenceEdits.css("display", "inline-block");
    })
    
    $("#save-btn").on("click", (evt) => {
        competenceEdits.css("display", "none");
        competenceNumbers.css("display", "inline-block");
    })
}





// Skill buttons

function AddSkillButtonListeners() {
    $('.skill-button').on('click', function (evt) {

        ToggleSkillColumn(evt.target.innerHTML)

        let npcId = $("#npc-id").val();

        $.ajax({
            type: "POST",
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            url: "/NPCDetails?handler=CycleSkillLevel",
            data: { id: npcId, skillName: evt.target.innerHTML },
            success: function (data) {
                console.log(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("AJAX error: " + textStatus + ", " + errorThrown);
                console.log("Error response: " + jqXHR.responseText);
                ToggleSkillColumn(evt.target);
                ToggleSkillColumn(evt.target);
            }
        })
    });
}


function ToggleSkillColumn(skillName) {

    const skill = $("#" + skillName.replace(/\s/g, "-"));

    // Setter ny klassetilhørighet
    if (skill.hasClass("base-skill")) {
        skill.removeClass("base-skill");
        skill.addClass("secondary-skill")
        skill.appendTo($("#secondary-skills"))
    }
    else if (skill.hasClass("secondary-skill")) {
        skill.removeClass("secondary-skill");
        skill.addClass("primary-skill")
        skill.appendTo($("#primary-skills"))
    }
    else {
        skill.removeClass("primary-skill");
        skill.addClass("base-skill")
        skill.appendTo($("#base-skills"))
    }
}






