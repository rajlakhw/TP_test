function showDeleteSicknessModal(deleteAnchorTag) {
    var sicknessRequestIdString = deleteAnchorTag.id;
    sicknessRequestIdString = sicknessRequestIdString.replace("sickness-delete-icon-", "");

    var sicknessId = parseInt(sicknessRequestIdString);

    var xhr = new XMLHttpRequest();
    xhr.open("POST", "HR/GetSicknessDetails", true);
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var response = xhr.responseText;
            var sicknessInfo = $.parseJSON(response);

            document.getElementById('sickness-deletion-confirmation-string').innerText = "Are you sure you want to delete this sickness from ";
            document.getElementById('sickness-id').value = sicknessInfo.id;

            var dateOption = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
            var sicknessStartDate = new Date(sicknessInfo.sicknessStartDateTime);

            var startDateHalfDayString = "";
            if (sicknessInfo.startDateAmorPmorFullDay == 1) {
                startDateHalfDayString = " (Morning)";
            }
            else if (sicknessInfo.startDateAmorPmorFullDay == 2) {
                startDateHalfDayString = " (Afternoon)";
            }

            var startDateString = sicknessStartDate.toLocaleDateString("en-GB", dateOption) + startDateHalfDayString;

            var endDateHalfDayString = "";
            if (sicknessInfo.endDateAmorPmorFullDay == 1) {
                endDateHalfDayString = " (Morning)";
            }
            else if (sicknessInfo.endDateAmorPmorFullDay == 2) {
                endDateHalfDayString = " (Afternoon)";
            }

            var sicknessEndDate = new Date(sicknessInfo.sicknessEndDateTime);
            var endDateString = sicknessEndDate.toLocaleDateString("en-GB", dateOption) + endDateHalfDayString;

            document.getElementById('deletion-sickness-dates').innerText = ' ' + startDateString + ' to ' + endDateString;

        }
    };
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.send(sicknessId);
}

function deleteSickness() {
    var sicknessId = document.getElementById('sickness-id').value;

    var xhr = new XMLHttpRequest();
    xhr.open("POST", "HR/DeleteSickness", true);
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            $('#sickness-deletion-modal').modal("hide");

            var response = xhr.responseText;
            var sicknessInfo = $.parseJSON(response);
            if (sicknessInfo != null) {
                document.getElementById('success-div').hidden = false;
                document.getElementById('error-div').hidden = true;

                var dateOption = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
                var sicknessStartDate = new Date(sicknessInfo.sicknessStartDateTime);

                var startDateHalfDayString = "";
                if (sicknessInfo.startDateAmorPmorFullDay == 1) {
                    startDateHalfDayString = " (Morning)";
                }
                else if (sicknessInfo.startDateAmorPmorFullDay == 2) {
                    startDateHalfDayString = " (Afternoon)";
                }

                var startDateString = sicknessStartDate.toLocaleDateString("en-GB", dateOption) + startDateHalfDayString;

                var endDateHalfDayString = "";
                if (sicknessInfo.endDateAmorPmorFullDay == 1) {
                    endDateHalfDayString = " (Morning)";
                }
                else if (sicknessInfo.endDateAmorPmorFullDay == 2) {
                    endDateHalfDayString = " (Afternoon)";
                }

                var sicknessEndDate = new Date(sicknessInfo.sicknessEndDateTime);
                var endDateString = sicknessEndDate.toLocaleDateString("en-GB", dateOption) + endDateHalfDayString;

                document.getElementById('sickness-deletion-success-dates').innerText = ' ' + startDateString + ' to ' + endDateString;

            }
            document.getElementById('header-text').innerText = "Success"
            $('#sickness-deletion-result-modal').modal("show");
        }
        else {
            document.getElementById('header-text').innerText = "Error"
            document.getElementById('success-div').hidden = true;
            document.getElementById('error-div').hidden = false;
        }

    };
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.send(sicknessId);

}