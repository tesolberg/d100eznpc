
$(function () {
    AddSkillButtonListeners();
    AddEditAndSaveButtonListener();
    AddUniqueToggleListener();
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


// Unique button
function AddUniqueToggleListener() {
    $("#unique-toggle").change("click", (evt) => {

        // Gets the value of the checkbox
        const uniqueChecked = $("#unique-toggle").prop("checked");
        const npcId = $("#npc-id").val();

        // Requests change to unique status
        $.ajax({
            type: "POST",
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            url: "/NPCDetails?handler=SetUnique",
            data: { id: npcId, unique: uniqueChecked },
            success: function (data) {
                //console.log(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("AJAX error: " + textStatus + ", " + errorThrown);
                console.log("Error response: " + jqXHR.responseText);
                $("#unique-toggle").prop("checked") = !uniqueChecked;
            }
        })

    });
}


// Skill buttons
function AddSkillButtonListeners() {
    $('.skill-button').on('click', function (evt) {

        let npcId = $("#npc-id").val();
        const skillName_ = evt.target.innerHTML.slice(0, -7);

        $.ajax({
            type: "POST",
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            url: "/NPCDetails?handler=CycleSkillLevel",
            data: { id: npcId, skillName: skillName_ },
            success: function (newSkillValue) {
                evt.target.innerHTML = GetSkillAndValueText(skillName_, newSkillValue);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("AJAX error: " + textStatus + ", " + errorThrown);
                console.log("Error response: " + jqXHR.responseText);
            }
        })
    });
}

function GetSkillAndValueText(skillName, newSkillValue) {
    let result = skillName + " ";
    for (var i = 1; i <= 6; i++) {
        result += newSkillValue >= i ? "☒" : "☐";
    }
    return result;
}







