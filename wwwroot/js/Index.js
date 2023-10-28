
$ (function (){
    CheckLocalStorage();
})


function CheckLocalStorage() {
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
}


function RemoveFromTracker(trackerId) {

    console.log("Removing " + trackerId);
    let trackerStr = localStorage.getItem("tracker");
    let trackerObj = JSON.parse(trackerStr);

    delete trackerObj[trackerId];

    trackerStr = JSON.stringify(trackerObj);
    localStorage.setItem("tracker", trackerStr);
}


function GetNewTrackerEntryNumber() {
    const nextId = parseInt(localStorage.getItem("nextId"));
    localStorage.setItem("nextId", nextId + 1);
    return nextId;
}
