function settingsPopUp() {
    // load settings

    $.ajax({
        url: "/api/ProfileSettingsApi/",
        type: "GET",
        contentType: 'application/json',
        success: function (data) {
            $("#anniversariresSwitch").prop('checked', data.showAnniversaries);
            $("#likesSwitch").prop('checked', data.showLikesAndComments);
        }
    });

}

function saveProfileSettings() {
    console.log('SAVE SETTINGS');

    var showAnniversaries = $("#anniversariresSwitch").is(":checked");
    var showLikesAndComments = $("#likesSwitch").is(":checked");

    var json = {
        showAnniversaries,
        showLikesAndComments
    };

    $.ajax({
        url: "/api/ProfileSettingsApi",
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(json),
        success: function (data) {

        }
    });
}
