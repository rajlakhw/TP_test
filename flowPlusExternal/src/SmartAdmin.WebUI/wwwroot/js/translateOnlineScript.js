
function signOffJobItem(jobItemId) {

    var js1 = document.createElement("script");
    js1.src = "https://cdn.jsdelivr.net/jquery.validation/1.16.0/jquery.validate.min.js";
    document.body.appendChild(js1);

    var js2 = document.createElement("script");
    js2.src = "https://cdn.jsdelivr.net/jquery.validation/1.16.0/additional-methods.min.js";
    document.body.appendChild(js2);

    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {

            var response = xhr.responseText;
            var signOffDetails = $.parseJSON(response);

            var signOffButton = document.getElementById('confirm-signoff-button');
            signOffButton.addEventListener("click", function () { confirmSignOff(jobItemId); });

            document.getElementById('job-item-id').innerText = signOffDetails.jobItemId;
            document.getElementById('translations-changed-label').innerText = signOffDetails.translationsChanged;
            document.getElementById('translation-not-started-label').hidden = signOffDetails.supplierAcceptedJobItem;

            $('#signoff-modal').modal('show');
        }
    };
    xhr.open("POST", "translateOnline/GetSignOffItemDetails", true);
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.send(jobItemId);

}

function confirmSignOff(jobItemId) {

    var comments = document.getElementById("comment-textarea").value;
    var stringToSend = jobItemId + '$' + comments;
    $.ajax({
        url: `translateOnline/ApproveTranslation`,
        type: 'POST',
        data: stringToSend,
        contentType: 'application/x-www-form-urlencoded',
        success: function (data) {
            console.log("Translation approved");
            window.location = `/TranslateOnline/translateOnlineStatus` + '?signoffConfirmed=' + jobItemId;

            //$('#confirm-review-signoff-modal').modal('show');
        }
    });
    //e.preventDefault();
}

