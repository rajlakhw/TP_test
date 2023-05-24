

function saveFlowPlusLicencingDetails() {
    document.getElementById('licence-save-button').setAttribute('disabled', 'true');
    var dataToSend = $('#flow-plus-licencing').serialize();
    var partData = $('#flow-plus-licencing')[0].action;

    partData = partData.substring(partData.indexOf("Id="));
    if (partData.indexOf("&CreateSingleOrderForAllLicences=") > -1) {
        partData = partData.substr(0, partData.indexOf("&CreateSingleOrderForAllLicences="));
    }
    partData += "&";
    partData = partData.replace("True", "true").replace("False", "false");

    dataToSend = partData + dataToSend;

    $.ajax({
        url: "Organisation/UpdateFlowPlusLicencingInfo",
        method: 'POST',
        dataType: 'json',
        data: dataToSend, // serializes the form's elements.
        success: function (data) {
            document.getElementById('licence-save-button').removeAttribute('disabled');
            //reload form
            if (data == true) {
                $("#order-created-confirmation-modal").modal('show');
            }

            $("flow-plus-licencing")[0].reset();
        }
    });

}

function enableFlowPlusControls(flowPlusSwitch) {
    var allControls = document.getElementsByClassName('flowpluscontrol');

    var reviewplusSwitchBox = document.getElementById('reviewplusSwitchBox');
    var translateOnlineSwitchBox = document.getElementById('translateOnlineSwitchBox');
    var designPlusSwitchBox = document.getElementById('designPlusSwitchBox');
    var AIOrMTSwitchBox = document.getElementById('AIOrMTSwitchBox');
    var CMSSwitchBox = document.getElementById('CMSSwitchBox');

    var reviewDemoSwicthBox = document.getElementById('reviewplusDemoSwitchBox');
    var translateOnlineDemoSwitchBox = document.getElementById('translateOnlineDemoSwitchBox');
    var designPlusDemoSwitchBox = document.getElementById('designPlusDemoSwitchBox');
    var aiOrMTDemoSwitchBox = document.getElementById('AIOrMTDemoSwitchBox');
    var cmsDemoSwitchBox = document.getElementById('CMSDemoSwitchBox');

    if (flowPlusSwitch.checked == true) {
        for (var i = 0; i < allControls.length; i++) {
            allControls[i].removeAttribute('disabled');
        }

        if (reviewplusSwitchBox.checked == true) {
            reviewDemoSwicthBox.disabled = !(flowPlusSwitch.checked);
            if (reviewDemoSwicthBox.checked == false) {
                document.getElementById('reviewplus-appcost').disabled = !(flowPlusSwitch.checked);

                if (document.getElementById('reviewplus-org-select') != null) {
                    document.getElementById('reviewplus-org-select').removeAttribute('disabled');
                }
                document.getElementById('reviewplus-contact-select').removeAttribute('disabled');
            }
        }

        if (translateOnlineSwitchBox.checked == true) {
            translateOnlineDemoSwitchBox.disabled = !(flowPlusSwitch.checked);
            if (translateOnlineDemoSwitchBox.checked == false) {
                document.getElementById('translateonline-appcost').disabled = !(flowPlusSwitch.checked);

                if (document.getElementById('translateonline-org-select') != null) {
                    document.getElementById('translateonline-org-select').removeAttribute('disabled');
                }
                document.getElementById('translateonline-contact-select').removeAttribute('disabled');
            }

        }

        if (designPlusSwitchBox.checked == true) {
            designPlusDemoSwitchBox.disabled = !(flowPlusSwitch.checked);
            if (designPlusDemoSwitchBox.checked == false) {
                document.getElementById('designplus-appcost').disabled = !(flowPlusSwitch.checked);

                if (document.getElementById('designplus-org-select') != null) {
                    document.getElementById('designplus-org-select').removeAttribute('disabled');
                }
                document.getElementById('designplus-contact-select').removeAttribute('disabled');
            }

        }

        if (AIOrMTSwitchBox.checked == true) {
            aiOrMTDemoSwitchBox.disabled = !(flowPlusSwitch.checked);
            if (aiOrMTDemoSwitchBox.checked == false) {
                document.getElementById('aiormt-appcost').disabled = !(flowPlusSwitch.checked);

                if (document.getElementById('aiormt-org-select') != null) {
                    document.getElementById('aiormt-org-select').removeAttribute('disabled');
                }
                document.getElementById('aiormt-contact-select').removeAttribute('disabled');
            }

        }

        if (CMSSwitchBox.checked == true) {
            cmsDemoSwitchBox.disabled = !(flowPlusSwitch.checked);
            if (cmsDemoSwitchBox.checked == false) {
                document.getElementById('cms-appcost').disabled = !(flowPlusSwitch.checked);

                if (document.getElementById('cms-org-select') != null) {
                    document.getElementById('cms-org-select').removeAttribute('disabled');
                }
                document.getElementById('cms-contact-select').removeAttribute('disabled');
            }

        }

    }
    else {

        for (var i = 0; i < allControls.length; i++) {
            allControls[i].setAttribute('disabled', 'true');
        }


        var flowPlusDemoSwitch = document.getElementById('flowplusDemoSwitchBox');
        flowPlusDemoSwitch.checked = false;

        reviewDemoSwicthBox.disabled = !(flowPlusSwitch.checked);
        translateOnlineDemoSwitchBox.disabled = !(flowPlusSwitch.checked);
        designPlusDemoSwitchBox.disabled = !(flowPlusSwitch.checked);
        aiOrMTDemoSwitchBox.disabled = !(flowPlusSwitch.checked);
        cmsDemoSwitchBox.disabled = !(flowPlusSwitch.checked);

        document.getElementById('reviewplus-appcost').disabled = !(flowPlusSwitch.checked);
        document.getElementById('translateonline-appcost').disabled = !(flowPlusSwitch.checked);
        document.getElementById('designplus-appcost').disabled = !(flowPlusSwitch.checked);
        document.getElementById('aiormt-appcost').disabled = !(flowPlusSwitch.checked);
        document.getElementById('cms-appcost').disabled = !(flowPlusSwitch.checked);


        if (document.getElementById('reviewplus-org-select') != null) {
            document.getElementById('reviewplus-org-select').setAttribute('disabled', 'true');
        }
        document.getElementById('reviewplus-contact-select').setAttribute('disabled', 'true');

        if (document.getElementById('translateonline-org-select') != null) {
            document.getElementById('translateonline-org-select').setAttribute('disabled', 'true');
        }
        document.getElementById('translateonline-contact-select').setAttribute('disabled', 'true');

        if (document.getElementById('designplus-org-select') != null) {
            document.getElementById('designplus-org-select').setAttribute('disabled', 'true');
        }
        document.getElementById('designplus-contact-select').setAttribute('disabled', 'true');

        if (document.getElementById('aiormt-org-select') != null) {
            document.getElementById('aiormt-org-select').setAttribute('disabled', 'true');
        }
        document.getElementById('aiormt-contact-select').setAttribute('disabled', 'true');

        if (document.getElementById('cms-org-select') != null) {
            document.getElementById('cms-org-select').setAttribute('disabled', 'true');
        }
        document.getElementById('cms-contact-select').setAttribute('disabled', 'true');


    }

    document.getElementById('consolidateInvoice').disabled = !(flowPlusSwitch.checked);

    reviewplusSwitchBox.disabled = !(flowPlusSwitch.checked);
    translateOnlineSwitchBox.disabled = !(flowPlusSwitch.checked);
    //designPlusSwitchBox.disabled = !(flowPlusSwitch.checked);
    AIOrMTSwitchBox.disabled = !(flowPlusSwitch.checked);
    //CMSSwitchBox.disabled = !(flowPlusSwitch.checked);




}

function enableFlowPlusDemoControls(flowPlusDemoSwitch) {

    var allControls = document.getElementsByClassName('flowplusDemoControl');
    var allReviewPlusControls = document.getElementsByClassName('reviewpluscontrol');
    var allTranslateOnlineControls = document.getElementsByClassName('translateonlinecontrol');
    var allDesignPlusControls = document.getElementsByClassName('designpluscontrol');
    var allAIOrMTControls = document.getElementsByClassName('aicontrol');
    var allCMSControls = document.getElementsByClassName('cmscontrol');

    if (flowPlusDemoSwitch.checked == false) {
        for (var i = 0; i < allControls.length; i++) {
            allControls[i].removeAttribute('disabled');
        }

        document.getElementById('reviewplusDemoSwitchBox').checked = false;
        if (document.getElementById('reviewplusSwitchBox').checked == true) {
            for (var i = 0; i < allReviewPlusControls.length; i++) {
                allReviewPlusControls[i].removeAttribute('disabled');
            }
        }

        document.getElementById('translateOnlineDemoSwitchBox').checked = false;
        if (document.getElementById('translateOnlineSwitchBox').checked == true) {
            for (var i = 0; i < allTranslateOnlineControls.length; i++) {
                allTranslateOnlineControls[i].removeAttribute('disabled');
            }
        }

        document.getElementById('designPlusDemoSwitchBox').checked = false;
        if (document.getElementById('designPlusSwitchBox').checked == true) {
            for (var i = 0; i < allDesignPlusControls.length; i++) {
                allDesignPlusControls[i].removeAttribute('disabled');
            }
        }

        document.getElementById('AIOrMTDemoSwitchBox').checked = false;
        if (document.getElementById('AIOrMTSwitchBox').checked == true) {
            for (var i = 0; i < allAIOrMTControls.length; i++) {
                allAIOrMTControls[i].removeAttribute('disabled');
            }
        }

        document.getElementById('CMSDemoSwitchBox').checked = false;
        if (document.getElementById('CMSSwitchBox').checked == true) {
            for (var i = 0; i < allCMSControls.length; i++) {
                allCMSControls[i].removeAttribute('disabled');
            }
        }

    }
    else {
        for (var i = 0; i < allControls.length; i++) {
            allControls[i].setAttribute('disabled', 'true');
        }

        document.getElementById('reviewplusDemoSwitchBox').checked = true;
        for (var i = 0; i < allReviewPlusControls.length; i++) {
            allReviewPlusControls[i].setAttribute('disabled', 'true');
        }

        document.getElementById('translateOnlineDemoSwitchBox').checked = true;
        for (var i = 0; i < allTranslateOnlineControls.length; i++) {
            allTranslateOnlineControls[i].setAttribute('disabled', 'true');
        }

        document.getElementById('designPlusDemoSwitchBox').checked = true;
        for (var i = 0; i < allDesignPlusControls.length; i++) {
            allDesignPlusControls[i].setAttribute('disabled', 'true');
        }

        document.getElementById('AIOrMTDemoSwitchBox').checked = true;
        for (var i = 0; i < allAIOrMTControls.length; i++) {
            allAIOrMTControls[i].setAttribute('disabled', 'true');
        }

        document.getElementById('CMSDemoSwitchBox').checked = true;
        for (var i = 0; i < allCMSControls.length; i++) {
            allCMSControls[i].setAttribute('disabled', 'true');
        }

    }

}

function showHideConsolidatedOptions(consolidateCheckBox) {
    var allControls = document.getElementsByClassName('non-consolidated');
    if (consolidateCheckBox.checked == false) {
        for (var i = 0; i < allControls.length; i++) {
            allControls[i].removeAttribute('hidden');
        }
    }
    else {
        for (var i = 0; i < allControls.length; i++) {
            allControls[i].setAttribute('hidden', 'true');
        }
    }
}


function enableReviewPlusControls(reviewPlusSwitch) {
    if (document.getElementById('flowplusDemoSwitchBox').checked == false) {
        var allControls = document.getElementsByClassName('reviewpluscontrol');
        if (reviewPlusSwitch.checked == true) {
            for (var i = 0; i < allControls.length; i++) {
                allControls[i].removeAttribute('disabled');
            }
        }
        else {
            for (var i = 0; i < allControls.length; i++) {
                allControls[i].setAttribute('disabled', 'true');
            }

        }
    }
}

function enableReviewPlusDemoControls(reviewPlusDemoSwitch) {
    var allControls = document.getElementsByClassName('reviewplusDemocontrol');
    if (reviewPlusDemoSwitch.checked == false) {
        for (var i = 0; i < allControls.length; i++) {
            allControls[i].removeAttribute('disabled');
        }
    }
    else {
        for (var i = 0; i < allControls.length; i++) {
            allControls[i].setAttribute('disabled', 'true');
        }


    }
}



function enabletranslateOnlineControls(translateOnlineSwitch) {
    if (document.getElementById('flowplusDemoSwitchBox').checked == false) {
        var allControls = document.getElementsByClassName('translateonlinecontrol');
        if (translateOnlineSwitch.checked == true) {
            for (var i = 0; i < allControls.length; i++) {
                allControls[i].removeAttribute('disabled');
            }
        }
        else {
            for (var i = 0; i < allControls.length; i++) {
                allControls[i].setAttribute('disabled', 'true');
            }

        }
    }
}

function enabletranslateOnlineDemoControls(translateOnlineDemoSwitch) {
    var allControls = document.getElementsByClassName('translateonlineDemocontrol');
    if (translateOnlineDemoSwitch.checked == false) {
        for (var i = 0; i < allControls.length; i++) {
            allControls[i].removeAttribute('disabled');
        }
    }
    else {
        for (var i = 0; i < allControls.length; i++) {
            allControls[i].setAttribute('disabled', 'true');
        }


    }
}



function enabledesignPlusControls(designPlusSwitch) {
    if (document.getElementById('flowplusDemoSwitchBox').checked == false) {
        var allControls = document.getElementsByClassName('designpluscontrol');
        if (designPlusSwitch.checked == true) {
            for (var i = 0; i < allControls.length; i++) {
                allControls[i].removeAttribute('disabled');
            }
        }
        else {
            for (var i = 0; i < allControls.length; i++) {
                allControls[i].setAttribute('disabled', 'true');
            }
        }
    }
}

function enabledesignPlusDemoControls(designPlusDemoSwitch) {
    var allControls = document.getElementsByClassName('designplusDemocontrol');
    if (designPlusDemoSwitch.checked == false) {
        for (var i = 0; i < allControls.length; i++) {
            allControls[i].removeAttribute('disabled');
        }
    }
    else {
        for (var i = 0; i < allControls.length; i++) {
            allControls[i].setAttribute('disabled', 'true');
        }


    }
}



function enableAIControls(aiSwitch) {
    if (document.getElementById('flowplusDemoSwitchBox').checked == false) {
        var allControls = document.getElementsByClassName('aicontrol');
        if (aiSwitch.checked == true) {
            for (var i = 0; i < allControls.length; i++) {
                allControls[i].removeAttribute('disabled');
            }
        }
        else {
            for (var i = 0; i < allControls.length; i++) {
                allControls[i].setAttribute('disabled', 'true');
            }

        }
    }
}

function enableAIDemoControls(aiDemoSwitch) {
    var allControls = document.getElementsByClassName('aiDemocontrol');
    if (aiDemoSwitch.checked == false) {
        for (var i = 0; i < allControls.length; i++) {
            allControls[i].removeAttribute('disabled');
        }
    }
    else {
        for (var i = 0; i < allControls.length; i++) {
            allControls[i].setAttribute('disabled', 'true');
        }



    }
}



function enableCMSControls(cmsSwitch) {
    if (document.getElementById('flowplusDemoSwitchBox').checked == false) {
        var allControls = document.getElementsByClassName('cmscontrol');
        if (cmsSwitch.checked == true) {
            for (var i = 0; i < allControls.length; i++) {
                allControls[i].removeAttribute('disabled');
            }
        }
        else {
            for (var i = 0; i < allControls.length; i++) {
                allControls[i].setAttribute('disabled', 'true');
            }

        }
    }
}

function enableCMSDemoControls(cmsDemoSwitch) {
    var allControls = document.getElementsByClassName('cmsDemocontrol');
    if (cmsDemoSwitch.checked == false) {
        for (var i = 0; i < allControls.length; i++) {
            allControls[i].removeAttribute('disabled');
        }
    }
    else {
        for (var i = 0; i < allControls.length; i++) {
            allControls[i].setAttribute('disabled', 'true');
        }



    }
}


function showContactForOrg(orgSelect, appName) {

    var orgId = orgSelect.value;
    var contactSelect = document.getElementById(appName + '-contact-select');
    var xhr = new XMLHttpRequest();

    xhr.onreadystatechange = function () {

        if (this.readyState == 4 && this.status == 200) {
            var response = xhr.responseText;
            var allContacts = $.parseJSON(response);

            contactSelect.innerHTML = '';
            var defaultOption = document.createElement("option");
            contactSelect.appendChild(defaultOption);

            for (var i = 0; i < allContacts.length; i++) {
                var contactOption = document.createElement("option");
                contactOption.setAttribute("value", allContacts[i].split(" - ")[0]);
                contactOption.innerText = allContacts[i];
                contactSelect.appendChild(contactOption);
            }

            var xhr1 = new XMLHttpRequest();
            xhr1.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    var currencySymbol = xhr1.responseText;
                    document.getElementById(appName + '-currency').innerText = currencySymbol;
                }
            }
            xhr1.open("POST", "Organisation/GetCurrencySymbol", true);
            xhr1.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            xhr1.send(orgId);
        }
    }
    xhr.open("POST", "Contact/GetAllContactsForOrg", true);
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.send(orgId);
}

function onflowplusLicenceUpdate() {
    document.getElementById('licence-save-button').style.backgroundColor = "red";
}





