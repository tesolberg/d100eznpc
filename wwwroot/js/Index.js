
// Consts for constructing the HP/AP elements
const locations = ["head", "left-arm", "torso", "right-arm", "abdomen", "left-leg", "right-leg"];
const containers = ["head-container", "torso-container", "torso-container", "torso-container", "abdomen-container", "leg-container", "leg-container"];


$(function () {
    InitLocalStorage();
    DrawTrackedEntities();
})


function DrawTrackedEntities() {
    const trackerStr = localStorage.getItem("tracker");
    const trackerObj = JSON.parse(trackerStr);

    for (let key in trackerObj) {
        const npcObj = JSON.parse(trackerObj[key])
        const entryNumber = key.toString();

        const entry = document.createElement("div");
        entry.className = "tracker-entry";
        entry.id = "entry-" + entryNumber;
        $("#tracker_entries").append(entry);

        // Add and remove buttons
        const addRemove = document.createElement("div");
        addRemove.className = "tracker_add-remove";
        entry.appendChild(addRemove);

        const removeEntryBtn = document.createElement("button");
        removeEntryBtn.className = "btn";
        removeEntryBtn.id= "remove-btn-" + entry.id;
        addRemove.appendChild(removeEntryBtn);

        const removeIcon = document.createElement("i");
        removeIcon.className = ("fa-solid fa-x");
        removeIcon.style = "color: #b87575;";
        removeEntryBtn.appendChild(removeIcon);

        // Initiative
        const init = document.createElement("div");
        init.className = "tracker_init";
        init.innerHTML = "-";
        entry.appendChild(init);

        // Name
        const name = document.createElement("div");
        name.className = "tracker_name";
        name.innerHTML = npcObj.Name;
        entry.appendChild(name);

        // Health and armor

        // Generates temporary div to place structure in
        const healthAndArmorTemp = document.createElement("div");
        healthAndArmorTemp.id = "health-and-armor-" + entry.id;
        healthAndArmorTemp.className = "tracker_hpap";
        entry.appendChild(healthAndArmorTemp);

        $.ajax({
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            url: '/Index?handler=DrawHealthAndArmor',
            data: { id: key.toString() },
            success: function (data) {
                healthAndArmorTemp.innerHTML = data;
                SetHealthAndArmorStructure(entry.id);
                BindFunctionsToButtons(entryNumber);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("AJAX error: " + textStatus + ", " + errorThrown);
                console.log("Error response: " + jqXHR.responseText);
            }
        });

        // Skills
        const skills = document.createElement("div");
        skills.className = "tracker_skills";
        let skillText = "";
        for (key in npcObj.Skills) {
            const skill = npcObj.Skills[key];
            skillText += skill.Name + "(" + skill.Value + "), ";
        }
        skills.innerHTML = skillText;
        entry.appendChild(skills);
    }
}

function SetHealthAndArmorStructure(entryId) {
    for (let i = 0; i < locations.length; i++) {
        const loc = $("#" + locations[i] + "-" + entryId);
        const container = $("#" + containers[i] + "-" + entryId);
        container.append(loc);
    }
}

function BindFunctionsToButtons(entryId) {
    $("#remove-btn-entry-" + entryId).on("click", (evt) => {
        RemoveFromTracker(entryId);
    })
}


function InitLocalStorage() {
    //console.log("Checking tracker obj: " + localStorage.getItem("tracker"));
    if (localStorage.getItem("tracker") == null) localStorage.setItem("tracker", JSON.stringify({}));
    if (localStorage.getItem("nextId") == null) localStorage.setItem("nextId", "0");
}

// Gets a fresh copy of the npc data and passes the data to AddToTracker
function GetNpcAndAddToTracker(id) {
    $.ajax({
        type: "POST",
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        url: "/Index?handler=AddToTracker",
        data: { id: id },
        success: function (data) {
            AddToTracker(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("AJAX error: " + textStatus + ", " + errorThrown);
            console.log("Error response: " + jqXHR.responseText);
        }
    })
}

// Retrieves combat tracker from local storage, adds new npc, then saves to local storage
function AddToTracker(npc) {
    let trackerStr = localStorage.getItem("tracker");
    let trackerObj = JSON.parse(trackerStr);

    // Checks if npc is unique AND already in tracker
    const npcObj = JSON.parse(npc);
    const uniqueNpc = npcObj["Unique"];
    if (uniqueNpc) {
        const uniqueNpcId = parseInt(npcObj["Id"]);
        // Checks if npcId is in tracker already
        // Only proceeds if not unique and already in tracker
        for (let key in trackerObj) {
            const trackedNpcObj = JSON.parse(trackerObj[key]);
            if (uniqueNpcId == trackedNpcObj["Id"]) {
                console.log(npcObj["Name"] + " is unique and already tracked");
                return;
            }
        }
    }

    console.log("Adding " + npcObj["Name"] + " to tracker");

    const entryNumber = GetNewTrackerEntryNumber();
    trackerObj[entryNumber] = npc;
    trackerStr = JSON.stringify(trackerObj);
    localStorage.setItem("tracker", trackerStr);

    // TODO: Oppdater så vi unngår ny GET
    location.reload();
}


function RemoveFromTracker(trackerId) {
    let trackerStr = localStorage.getItem("tracker");
    let trackerObj = JSON.parse(trackerStr);

    delete trackerObj[trackerId];

    trackerStr = JSON.stringify(trackerObj);
    localStorage.setItem("tracker", trackerStr);

    $("#entry-" + trackerId).remove();
}


function GetNewTrackerEntryNumber() {
    const nextId = parseInt(localStorage.getItem("nextId"));
    localStorage.setItem("nextId", nextId + 1);
    return nextId;
}
