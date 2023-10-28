
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
        let npcId = $("#npc-id").val();

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

        //ToggleSkillColumn(evt.target.innerHTML)
        FadeToNewColumn(evt.target.innerHTML);

        let npcId = $("#npc-id").val();

        $.ajax({
            type: "POST",
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            url: "/NPCDetails?handler=CycleSkillLevel",
            data: { id: npcId, skillName: evt.target.innerHTML },
            success: function (data) {
                //console.log(data);
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

function FadeToNewColumn(skillName) {
    const skill = $("#" + skillName.toLowerCase().replace(/\s/g, "-"));

    skill.fadeToggle("fast", function () {
        ToggleSkillColumn(skillName);
    });
}


function ToggleSkillColumn(skillName) {

    const skill = $("#" + skillName.toLowerCase().replace(/\s/g, "-"));

    skill.fadeToggle();

    // Setter ny klassetilhørighet
    if (skill.hasClass("base-skill")) {
        skill.removeClass("base-skill");
        skill.addClass("secondary-skill")
        console.log("Moving to secondary");
        AppendAlphabetically(skill, $("#secondary-skills"));
    }
    else if (skill.hasClass("secondary-skill")) {
        skill.removeClass("secondary-skill");
        skill.addClass("primary-skill")
        console.log("Moving to primary");
        AppendAlphabetically(skill, $("#primary-skills"))
    }
    else {
        skill.removeClass("primary-skill");
        skill.addClass("base-skill")
        console.log("Moving to base");
        AppendAlphabetically(skill, $("#base-skills"))
    }
}


function AppendAlphabetically(skill, parent) {
    var skillText = skill.text();

    var inserted = false;
    parent.children().each(function () {
        var childText = $(this).text();
        console.log("Comparing " + skillText + " to " + childText + ": " + (skillText < childText));
        if (skillText < childText) {
            console.log("Inserting before: " + childText);
            skill.insertBefore(this);
            inserted = true;
            return false;
        }
    });

    // If not inserted yet, it's the last in the sort
    if (!inserted) {
        parent.append(skill);
    }
}








