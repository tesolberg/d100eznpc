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

        DrawEntity(npcObj, entryNumber);
    }
}

function DrawEntity(npcObj, entryNumber) {
    // Checks if entry already on page
    let entry = document.getElementById("entry-" + entryNumber);

    // If it is, deletes content
    if (entry != null) {
        entry.innerHTML = "";
    }

    // If not creates new entry container
    else {
        entry = document.createElement("div");
        entry.className = "tracker-entry";
        entry.id = "entry-" + entryNumber;
        $("#tracker_entries").append(entry);
    }

    // Buttons for add/remove npc
    const addRemove = document.createElement("div");
    addRemove.className = "tracker_add-remove tracker__section";
    entry.appendChild(addRemove);

    const removeEntryBtn = document.createElement("button");
    removeEntryBtn.className = "btn";
    removeEntryBtn.id = "remove-btn-" + entry.id;
    addRemove.appendChild(removeEntryBtn);

    const removeIcon = document.createElement("i");
    removeIcon.className = ("fa-solid fa-x");
    removeIcon.style = "color: #b87575;";
    removeEntryBtn.appendChild(removeIcon);

    // Initiative
    const init = document.createElement("div");
    init.className = "tracker_init tracker__section";
    init.innerHTML = "-";
    entry.appendChild(init);

    // Name
    const name = document.createElement("div");
    name.className = "tracker_name tracker__section";
    name.innerHTML = npcObj.Name;
    entry.appendChild(name);

    // Health and armor
    const hitPointsContainer = document.createElement("div");
    hitPointsContainer.setAttribute("class", "tracker_hp-container tracker__section");
    entry.appendChild(hitPointsContainer);

    // Subtract hitpoints button
    const subHPbtn = document.createElement("button");
    subHPbtn.setAttribute("class", "sub-hp-btn hp_btn btn-danger");
    subHPbtn.id = "sub-hp-btn-" + entryNumber;
    subHPbtn.innerHTML = "-";
    hitPointsContainer.appendChild(subHPbtn);

    // Hitpoints text
    const healthText = document.createElement("div");
    healthText.id = "hit-points-" + entry.id;
    healthText.className = "tracker_hp_text";
    healthText.innerHTML = npcObj.HitPoints;
    hitPointsContainer.appendChild(healthText);

    // Add hitpoints button
    const addHPbtn = document.createElement("button");
    addHPbtn.setAttribute("class", "add-hp-btn hp_btn btn-success");
    addHPbtn.id = "add-hp-btn-" + entryNumber;
    addHPbtn.innerHTML = "+";
    hitPointsContainer.appendChild(addHPbtn);


    // Skills
    const skills = document.createElement("div");
    skills.className = "tracker_skills tracker__section";
    let skillText = "";
    for (key in npcObj.Skills) {
        const skill = npcObj.Skills[key];
        skillText += skill.Name + "(" + skill.Value + "), ";
    }
    skills.innerHTML = skillText;
    entry.appendChild(skills);

    // Bind functions to buttons on this tracker entry
    BindFunctionsToButtons(entryNumber);
}

function BindFunctionsToButtons(entryId) {
    $("#remove-btn-entry-" + entryId).on("click", (evt) => {
        RemoveFromTracker(entryId);
    })
    $("#add-hp-btn-" + entryId).on("click", (evt) => {
        ChangeHitPoints(1, entryId);
    })
    $("#sub-hp-btn-" + entryId).on("click", (evt) => {
        ChangeHitPoints(-1, entryId);
    })
}


function ChangeHitPoints(value, entryId) {
    // Getting tracker object from local storage and extracting NPC object
    const trackerObj = JSON.parse(localStorage.getItem("tracker"));
    const npcObj = JSON.parse(trackerObj[entryId]);

    // Changing HP and saving to tracker object
    npcObj["HitPoints"] = npcObj["HitPoints"] + value;
    trackerObj[entryId] = JSON.stringify(npcObj);

    // Updating local storage with modified tracker data
    localStorage.setItem("tracker", JSON.stringify(trackerObj));

    // Updates HP text on page
    DrawEntity(npcObj, entryId);
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

    // Saves new tracker entry to tracker
    const entryNumber = GetNewTrackerEntryNumber();
    trackerObj[entryNumber] = npc;
    trackerStr = JSON.stringify(trackerObj);
    localStorage.setItem("tracker", trackerStr);

    // Renders the new entry
    DrawEntity(npcObj, entryNumber);
}


function RemoveFromTracker(trackerId) {
    let trackerStr = localStorage.getItem("tracker");
    let trackerObj = JSON.parse(trackerStr);

    delete trackerObj[trackerId];

    trackerStr = JSON.stringify(trackerObj);
    localStorage.setItem("tracker", trackerStr);

    $("#entry-" + trackerId).remove();
    console.log("Deleting " + trackerId);
}


function GetNewTrackerEntryNumber() {
    const nextId = parseInt(localStorage.getItem("nextId"));
    localStorage.setItem("nextId", nextId + 1);
    return nextId;
}
