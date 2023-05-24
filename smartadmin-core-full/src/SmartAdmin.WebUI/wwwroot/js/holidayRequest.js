
function showHolidayModal() {
    var js = document.createElement("script");
    js.src = "/js/formplugins/bootstrap-datepicker/bootstrap-datepicker.js";
    document.body.appendChild(js);

    var js1 = document.createElement("script");
    js1.src = "https://cdn.jsdelivr.net/jquery.validation/1.16.0/jquery.validate.min.js";
    document.body.appendChild(js1);

    var js2 = document.createElement("script");
    js2.src = "https://cdn.jsdelivr.net/jquery.validation/1.16.0/additional-methods.min.js";
    document.body.appendChild(js2);

    var css = document.createElement("link");
    css.rel = "stylesheet";
    css.media = "screen, print";
    css.href = "/css/formplugins/bootstrap-datepicker/bootstrap-datepicker.css";
    document.head.appendChild(css);

    var forms = document.getElementsByClassName('needs-validation');
    // Loop over them and prevent submission
    var validation = Array.prototype.filter.call(forms, function (form) {
        form.addEventListener('submit', function (event) {
            if (form.checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    });


    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var controls = {
                leftArrow: '<i class="fal fa-angle-left" style="font-size: 1.25rem"></i>',
                rightArrow: '<i class="fal fa-angle-right" style="font-size: 1.25rem"></i>'
            };

            var today = new Date();
            $('#datepicker-start-date').datepicker({
                todayHighlight: true,
                templates: controls,
                format: "dd/mm/yyyy",
                startDate: today,
                daysOfWeekDisabled: [0, 6],
                weekStart: 1,
                autoclose: true
            });


            $('#datepicker-end-date').datepicker({
                todayHighlight: true,
                templates: controls,
                format: "dd/mm/yyyy",
                startDate: today,
                daysOfWeekDisabled: [0, 6],
                weekStart: 1,
                autoclose: true
            });

            var response = xhr.responseText;
            var holidayInfo = $.parseJSON(response);
            document.getElementById('remaining-holiday').innerText = holidayInfo.holidaysRemaining;
            document.getElementById('total-holiday').innerText = holidayInfo.totalAnnualHolidays;
            document.getElementById('holiday-year').innerText = holidayInfo.year;

            document.getElementById('datepicker-start-date').value = holidayInfo.nextAvailableHolidayDate;
            document.getElementById('datepicker-end-date').value = holidayInfo.nextAvailableHolidayDate;

            var nextDateObject = new Date(holidayInfo.nextAvailableHolidayDate.split('/')[2] + '-' +
                holidayInfo.nextAvailableHolidayDate.split('/')[1] + '-' +
                holidayInfo.nextAvailableHolidayDate.split('/')[0]);
            $('#datepicker-start-date').datepicker("update", nextDateObject);


            $('#datepicker-end-date').datepicker("update", nextDateObject);

            document.getElementById('last-day-radio-div').hidden = true;

            document.getElementById('first-day-back').innerText = holidayInfo.nextWorkingDayAfterHolidayString;
            document.getElementById('holiday-total-days').innerText = holidayInfo.totalHolidaysForCurrentRequest;

        }
    };
    xhr.open("POST", "HR/HolidayRequest", true);
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.send();

}

function startDateUpdated() {

    var startDate = document.getElementById('datepicker-start-date').value;
    var endDate = document.getElementById('datepicker-end-date').value;
    var isFullDayBankHOliday = false;
    var isHalfDayBankHOliday = false;

    //debugger;

    //first check if the start date is bank holiday
    var xhr1 = new XMLHttpRequest();
    xhr1.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            //debugger;

            if (xhr1.responseText != 'false') {
                var nextDate = xhr1.responseText;
                //debugger;
                var bankHolidayString = document.getElementById('datepicker-start-date').value;
                var bankHolidayObject = new Date(bankHolidayString.split('/')[2] + '-' + bankHolidayString.split('/')[1] + '-' + bankHolidayString.split('/')[0]);

                document.getElementById('datepicker-start-date').value = nextDate;
                startDate = document.getElementById('datepicker-start-date').value;

                var nextDateObject = new Date(nextDate.split('/')[2] + '-' + nextDate.split('/')[1] + '-' + nextDate.split('/')[0]);
                $('#datepicker-start-date').datepicker("update", nextDateObject);

                $('#datepicker-start-date').datepicker('hide', true);
                document.getElementById('public-holiday-date-string').innerText = bankHolidayObject.toDateString();
                $('#is-bank-holiday-modal-alert').modal('show');
                return;

                //debugger;                                                                                                                              
                //show an alert that it is a bank holiday


                //document.getElementById("is-bank-holiday-modal-alert").showModal();



            }

            //then check if the start date is the half day bank holiday
            //debugger;
            var xhr2 = new XMLHttpRequest();
            xhr2.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {

                    var response = xhr2.responseText;
                    if (response == 'true') {
                        document.getElementById('start-date-full-day').disabled = true;
                        document.getElementById('start-date-afternoon').disabled = true;
                        document.getElementById('start-date-morning').checked = true;
                        document.getElementById('last-day-div').hidden = true;
                        document.getElementById('last-day-radio-div').hidden = true;
                    }
                    else {
                        document.getElementById('start-date-full-day').disabled = false;
                        document.getElementById('start-date-afternoon').disabled = false;

                        if (startDate == endDate) {
                            document.getElementById('last-day-radio-div').hidden = true;
                        }
                        else {
                            document.getElementById('last-day-radio-div').hidden = false;
                        }
                        document.getElementById('last-day-div').hidden = false;
                    }

                    var startDateFullOrAMOrPM = -1;
                    if (document.getElementById('start-date-full-day').checked == true) {
                        startDateFullOrAMOrPM = 0;
                    }
                    else if (document.getElementById('start-date-morning').checked == true) {
                        startDateFullOrAMOrPM = 1;
                    }
                    else if (document.getElementById('start-date-afternoon').checked == true) {
                        startDateFullOrAMOrPM = 2;
                    }

                    var endDateFullOrAMOrPM = -1;
                    if (lastDayRadioVisible == true) {
                        if (document.getElementById('end-date-full-day').checked == true) {
                            endDateFullOrAMOrPM = 0;
                        }
                        else if (document.getElementById('end-date-morning').checked == true) {
                            endDateFullOrAMOrPM = 1;
                        }
                    }


                    var endDateObject = new Date(endDate.split('/')[2] + '-' + endDate.split('/')[1] + '-' + endDate.split('/')[0]);
                    var startDateObject = new Date(startDate.split('/')[2] + '-' + startDate.split('/')[1] + '-' + startDate.split('/')[0]);

                    //if last day is smaller than start date, then just update the last date to the same as first date
                    if (endDateObject < startDateObject) {
                        document.getElementById('datepicker-end-date').value = startDate;
                        $('#datepicker-end-date').datepicker("update", startDateObject);
                        endDate = startDate;
                    }


                    //if start and end dates are same 
                    if (startDate == endDate) {
                        if (startDateFullOrAMOrPM == 1) {
                            document.getElementById('last-day-div').hidden = true;
                        }
                        else {
                            document.getElementById('last-day-div').hidden = false;
                        }
                        document.getElementById('last-day-radio-div').hidden = true;
                    }
                    else {
                        if (startDateFullOrAMOrPM != 1) {
                            document.getElementById('last-day-div').hidden = false;
                            document.getElementById('last-day-radio-div').hidden = false;
                        }
                        else {
                            document.getElementById('last-day-div').hidden = true;
                            document.getElementById('last-day-radio-div').hidden = true;
                        }
                    }

                    var lastDayRadio = document.getElementById('last-day-radio-div');
                    var lastDayRadioVisible = true;
                    if (lastDayRadio.hidden == true) {
                        lastDayRadioVisible = false;
                    }

                    var stringToSend = lastDayRadioVisible + '$' + startDateFullOrAMOrPM + '$' + endDateFullOrAMOrPM + '$' + startDate + '$' + endDate;


                    var xhr = new XMLHttpRequest();
                    xhr.onreadystatechange = function () {
                        if (this.readyState == 4 && this.status == 200) {

                            var response = xhr.responseText;
                            var holidayInfo = $.parseJSON(response);
                            document.getElementById('remaining-holiday').innerText = holidayInfo.holidaysRemaining;
                            document.getElementById('total-holiday').innerText = holidayInfo.totalAnnualHolidays;
                            document.getElementById('holiday-year').innerText = holidayInfo.year;

                            //document.getElementById('datepicker-start-date').value = holidayInfo.nextAvailableHolidayDate;
                            //document.getElementById('datepicker-end-date').value = holidayInfo.nextAvailableHolidayDate;

                            document.getElementById('first-day-back').innerText = holidayInfo.nextWorkingDayAfterHolidayString;
                            document.getElementById('holiday-total-days').innerText = holidayInfo.totalHolidaysForCurrentRequest;

                        }
                    };
                    //debugger;
                    xhr.open("POST", "HR/HolidayRequest", true);
                    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                    xhr.send(stringToSend);

                }
            };
            //debugger;
            xhr2.open("POST", "HR/IsDateHalfDayBankHoliday", true);
            xhr2.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');

            xhr2.send(startDate);
        }
    };
    //debugger;
    xhr1.open("POST", "HR/IsDateBankHoliday", true);
    xhr1.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');

    xhr1.send(startDate);



}

function endDateUpdated() {

    //debugger;
    var startDate = document.getElementById('datepicker-start-date').value;
    var endDate = document.getElementById('datepicker-end-date').value;

    //check if the end date selected is a bank holiday
    var xhr1 = new XMLHttpRequest();
    xhr1.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            //debugger;

            if (xhr1.responseText != 'false') {
                var nextDate = xhr1.responseText;

                var bankHolidayString = document.getElementById('datepicker-end-date').value;
                var bankHolidayObject = new Date(bankHolidayString.split('/')[2] + '-' + bankHolidayString.split('/')[1] + '-' + bankHolidayString.split('/')[0]);
                //debugger;
                document.getElementById('datepicker-end-date').value = nextDate;
                endDate = document.getElementById('datepicker-end-date').value;

                var nextDateObject = new Date(nextDate.split('/')[2] + '-' + nextDate.split('/')[1] + '-' + nextDate.split('/')[0]);
                $('#datepicker-end-date').datepicker("update", nextDateObject);

                //show an alert that it is a bank holiday
                $('#datepicker-end-date').datepicker('hide', true);
                document.getElementById('public-holiday-date-string').innerText = bankHolidayObject.toDateString();
                $('#is-bank-holiday-modal-alert').modal('show');

                return;
            }

            //then check if the start date is the half day bank holiday
            //debugger;
            var xhr2 = new XMLHttpRequest();
            xhr2.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {

                    var response = xhr2.responseText;
                    if (response == 'true') {
                        document.getElementById('end-date-full-day').disabled = true;
                        document.getElementById('end-date-morning').checked = true;
                    }
                    else {
                        document.getElementById('end-date-full-day').disabled = false;
                    }




                    var endDateObject = new Date(endDate.split('/')[2] + '-' + endDate.split('/')[1] + '-' + endDate.split('/')[0]);
                    var startDateObject = new Date(startDate.split('/')[2] + '-' + startDate.split('/')[1] + '-' + startDate.split('/')[0]);

                    //if last day is smaller than start date, then just update the first date to the same as last date
                    if (endDateObject < startDateObject) {
                        document.getElementById('datepicker-start-date').value = endDate;
                        $('#datepicker-start-date').datepicker("update", endDateObject);
                        startDate = endDate;
                    }

                    if (startDate == endDate) {
                        document.getElementById('last-day-radio-div').hidden = true;
                    }
                    else {
                        document.getElementById('last-day-radio-div').hidden = false;
                    }

                    var startDateFullOrAMOrPM = -1;
                    if (document.getElementById('start-date-full-day').checked == true) {
                        startDateFullOrAMOrPM = 0;
                    }
                    else if (document.getElementById('start-date-morning').checked == true) {
                        startDateFullOrAMOrPM = 1;
                    }
                    else if (document.getElementById('start-date-afternoon').checked == true) {
                        startDateFullOrAMOrPM = 2;
                    }

                    var endDateFullOrAMOrPM = -1;
                    if (lastDayRadioVisible == true) {
                        if (document.getElementById('end-date-full-day').checked == true) {
                            endDateFullOrAMOrPM = 0;
                        }
                        else if (document.getElementById('end-date-morning').checked == true) {
                            endDateFullOrAMOrPM = 1;
                        }
                        //else if (document.getElementById('end-date-afternoon').checked == true) {
                        //    endDateFullOrAMOrPM = 2;
                        //}
                    }


                    var lastDayRadio = document.getElementById('last-day-radio-div');
                    var lastDayRadioVisible = true;
                    if (lastDayRadio.hidden == true) {
                        lastDayRadioVisible = false;
                    }

                    var stringToSend = lastDayRadioVisible + '$' + startDateFullOrAMOrPM + '$' + endDateFullOrAMOrPM + '$' + startDate + '$' + endDate;;


                    var xhr = new XMLHttpRequest();
                    xhr.onreadystatechange = function () {
                        if (this.readyState == 4 && this.status == 200) {

                            var response = xhr.responseText;
                            var holidayInfo = $.parseJSON(response);
                            document.getElementById('remaining-holiday').innerText = holidayInfo.holidaysRemaining;
                            document.getElementById('total-holiday').innerText = holidayInfo.totalAnnualHolidays;
                            document.getElementById('holiday-year').innerText = holidayInfo.year;

                            document.getElementById('first-day-back').innerText = holidayInfo.nextWorkingDayAfterHolidayString;
                            document.getElementById('holiday-total-days').innerText = holidayInfo.totalHolidaysForCurrentRequest;

                        }
                    };
                    //debugger;
                    xhr.open("POST", "HR/HolidayRequest", true);
                    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                    xhr.send(stringToSend);


                }
            };
            //debugger;
            xhr2.open("POST", "HR/IsDateHalfDayBankHoliday", true);
            xhr2.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');

            xhr2.send(endDate);
        }
    };
    xhr1.open("POST", "HR/IsDateBankHoliday", true);
    xhr1.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');

    xhr1.send(endDate);


}

function startRadioButtonChanged() {


    var startDate = document.getElementById('datepicker-start-date').value;
    var endDate = document.getElementById('datepicker-end-date').value;

    var startDateFullOrAMOrPM = -1;
    if (document.getElementById('start-date-full-day').checked == true) {
        startDateFullOrAMOrPM = 0;
    }
    else if (document.getElementById('start-date-morning').checked == true) {
        startDateFullOrAMOrPM = 1;
    }
    else if (document.getElementById('start-date-afternoon').checked == true) {
        startDateFullOrAMOrPM = 2;
    }

    var endDateFullOrAMOrPM = -1;


    if (startDateFullOrAMOrPM == 1) {
        document.getElementById('last-day-div').hidden = true;
        document.getElementById('last-day-radio-div').hidden = true;
    }
    else {
        document.getElementById('last-day-div').hidden = false;
        if (startDate == endDate) {
            document.getElementById('last-day-radio-div').hidden = true;
        }
        else {
            document.getElementById('last-day-radio-div').hidden = false;
        }
    }

    var lastDayRadio = document.getElementById('last-day-radio-div');
    var lastDayRadioVisible = true;
    if (lastDayRadio.hidden == true) {
        lastDayRadioVisible = false;
    }

    if (lastDayRadioVisible == true) {
        if (document.getElementById('end-date-full-day').checked == true) {
            endDateFullOrAMOrPM = 0;
        }
        else if (document.getElementById('end-date-morning').checked == true) {
            endDateFullOrAMOrPM = 1;
        }
        //else if (document.getElementById('end-date-afternoon').checked == true) {
        //    endDateFullOrAMOrPM = 2;
        //}
    }




    var stringToSend = lastDayRadioVisible + '$' + startDateFullOrAMOrPM + '$' + endDateFullOrAMOrPM + '$' + startDate + '$' + endDate;;


    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {

            var response = xhr.responseText;
            var holidayInfo = $.parseJSON(response);
            document.getElementById('remaining-holiday').innerText = holidayInfo.holidaysRemaining;
            document.getElementById('total-holiday').innerText = holidayInfo.totalAnnualHolidays;
            document.getElementById('holiday-year').innerText = holidayInfo.year;

            document.getElementById('first-day-back').innerText = holidayInfo.nextWorkingDayAfterHolidayString;
            document.getElementById('holiday-total-days').innerText = holidayInfo.totalHolidaysForCurrentRequest;

        }
    };
    //debugger;
    xhr.open("POST", "HR/HolidayRequest", true);
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.send(stringToSend);
}

function endRadioButtonChanged() {


    var startDate = document.getElementById('datepicker-start-date').value;
    var endDate = document.getElementById('datepicker-end-date').value;


    var startDateFullOrAMOrPM = -1;
    if (document.getElementById('start-date-full-day').checked == true) {
        startDateFullOrAMOrPM = 0;
    }
    else if (document.getElementById('start-date-morning').checked == true) {
        startDateFullOrAMOrPM = 1;
    }
    else if (document.getElementById('start-date-afternoon').checked == true) {
        startDateFullOrAMOrPM = 2;
    }



    var lastDayRadio = document.getElementById('last-day-radio-div');
    var lastDayRadioVisible = true;
    if (lastDayRadio.hidden == true) {
        lastDayRadioVisible = false;
    }

    var endDateFullOrAMOrPM = -1;
    if (lastDayRadioVisible == true) {
        if (document.getElementById('end-date-full-day').checked == true) {
            endDateFullOrAMOrPM = 0;
        }
        else if (document.getElementById('end-date-morning').checked == true) {
            endDateFullOrAMOrPM = 1;
        }
        //else if (document.getElementById('end-date-afternoon').checked == true) {
        //    endDateFullOrAMOrPM = 2;
        //}
    }

    var stringToSend = lastDayRadioVisible + '$' + startDateFullOrAMOrPM + '$' + endDateFullOrAMOrPM + '$' + startDate + '$' + endDate;;


    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {

            var response = xhr.responseText;
            var holidayInfo = $.parseJSON(response);
            document.getElementById('remaining-holiday').innerText = holidayInfo.holidaysRemaining;
            document.getElementById('total-holiday').innerText = holidayInfo.totalAnnualHolidays;
            document.getElementById('holiday-year').innerText = holidayInfo.year;

            document.getElementById('first-day-back').innerText = holidayInfo.nextWorkingDayAfterHolidayString;
            document.getElementById('holiday-total-days').innerText = holidayInfo.totalHolidaysForCurrentRequest;

        }
    };
    //debugger;
    xhr.open("POST", "HR/HolidayRequest", true);
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.send(stringToSend);
}

function submitHoliday() {
    var isValid = $("#holiday-request-form").valid();
    if (isValid == true) {
        var startDate = document.getElementById('datepicker-start-date').value;
        var endDate = document.getElementById('datepicker-end-date').value;

        var lastDayRadio = document.getElementById('last-day-radio-div');
        var lastDayRadioVisible = true;
        if (lastDayRadio.hidden == true) {
            lastDayRadioVisible = false;
        }

        var startDateFullOrAMOrPM = -1;
        if (document.getElementById('start-date-full-day').checked == true) {
            startDateFullOrAMOrPM = 0;
        }
        else if (document.getElementById('start-date-morning').checked == true) {
            startDateFullOrAMOrPM = 1;
        }
        else if (document.getElementById('start-date-afternoon').checked == true) {
            startDateFullOrAMOrPM = 2;
        }

        var endDateFullOrAMOrPM = -1;
        if (lastDayRadioVisible == true) {
            if (document.getElementById('end-date-full-day').checked == true) {
                endDateFullOrAMOrPM = 0;
            }
            else if (document.getElementById('end-date-morning').checked == true) {
                endDateFullOrAMOrPM = 1;
            }
        }

        var totalDays = document.getElementById('holiday-total-days').innerText;

        var employeeId = "";

        
        //if (document.getElementById('employee-hidden-field') != null) {
        //    employeeId = document.getElementById('employee-hidden-field').value;
        //}


        var stringToSend = lastDayRadioVisible + '$' + startDateFullOrAMOrPM + '$' + endDateFullOrAMOrPM + '$' + startDate + '$' + endDate + '$' + totalDays + '$' + employeeId;
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "HR/CreateHolidayRequest", true);
        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                //hide main holiday request modal
                $('#holiday-request-modal').modal("hide");

                //show holiday request successful modal 
                var response = xhr.responseText;
                var headerText = response.split("$")[0];
                document.getElementById('header-text').innerText = headerText;
                if (headerText == "Success") {
                    document.getElementById('holiday-creation-success-string').hidden = false;
                    document.getElementById('holiday-creation-success-dates').innerText = response.split("$")[1];
                    document.getElementById('holiday-creation-warning-string').innerText = "";
                }
                else {
                    document.getElementById('holiday-creation-success-string').hidden = true;
                    document.getElementById('holiday-creation-success-dates').innerText = "";
                    document.getElementById('holiday-creation-warning-string').innerText = response.split("$")[1];
                }
                $('#holiday-creation-result-modal').modal("show");


            }
            else {
                //if the holiday could not be added ok, then notify the user
            }
        };
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        xhr.send(stringToSend);
        e.preventDefault();
    }
}

function showDeleteHolidayModal(deleteAnchorTag) {
    var holidayRequestIdString = deleteAnchorTag.id;
    holidayRequestIdString = holidayRequestIdString.replace("holiday-delete-icon-", "");

    var holidayReqId = parseInt(holidayRequestIdString);

    var xhr = new XMLHttpRequest();
    xhr.open("POST", "HR/GetHolidayRequestDetails", true);
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var response = xhr.responseText;
            var holidayReqInfo = $.parseJSON(response);

            document.getElementById('holiday-deletion-confirmation-string').innerText = "Are you sure you want to delete this holiday request from ";
            document.getElementById('holiday-request-id').value = holidayReqInfo.id;

            var dateOption = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
            var holidayStartDate = new Date(holidayReqInfo.holidayStartDateTime);

            var startDateHalfDayString = "";
            if (holidayReqInfo.startDateFullOrAMOrPM == 1) {
                startDateHalfDayString = " (Morning)";
            }
            else if (holidayReqInfo.startDateFullOrAMOrPM == 2) {
                startDateHalfDayString = " (Afternoon)";
            }

            var startDateString = holidayStartDate.toLocaleDateString("en-GB", dateOption) + startDateHalfDayString;

            var endDateHalfDayString = "";
            if (holidayReqInfo.endDateFullOrAMOrPM == 1) {
                endDateHalfDayString = " (Morning)";
            }
            else if (holidayReqInfo.endDateFullOrAMOrPM == 2) {
                endDateHalfDayString = " (Afternoon)";
            }

            var holidayEndDate = new Date(holidayReqInfo.holidayEndDateTime);
            var endDateString = holidayEndDate.toLocaleDateString("en-GB", dateOption) + endDateHalfDayString;

            document.getElementById('deletion-holiday-dates').innerText = ' ' + startDateString + ' to ' + endDateString;

        }
    };
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.send(holidayReqId);

}

function deleteHolidayRequest() {

    var holidayRequestId = document.getElementById('holiday-request-id').value;

    var xhr = new XMLHttpRequest();
    xhr.open("POST", "HR/DeleteHolidayRequest", true);
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            $('#holiday-deletion-modal').modal("hide");

            var response = xhr.responseText;
            var holidayReqInfo = $.parseJSON(response);
            if (holidayReqInfo != null) {
                document.getElementById('success-div').hidden = false;
                document.getElementById('error-div').hidden = true;

                var dateOption = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
                var holidayStartDate = new Date(holidayReqInfo.holidayStartDateTime);

                var startDateHalfDayString = "";
                if (holidayReqInfo.startDateFullOrAMOrPM == 1) {
                    startDateHalfDayString = " (Morning)";
                }
                else if (holidayReqInfo.startDateFullOrAMOrPM == 2) {
                    startDateHalfDayString = " (Afternoon)";
                }

                var startDateString = holidayStartDate.toLocaleDateString("en-GB", dateOption) + startDateHalfDayString;

                var endDateHalfDayString = "";
                if (holidayReqInfo.endDateFullOrAMOrPM == 1) {
                    endDateHalfDayString = " (Morning)";
                }
                else if (holidayReqInfo.endDateFullOrAMOrPM == 2) {
                    endDateHalfDayString = " (Afternoon)";
                }

                var holidayEndDate = new Date(holidayReqInfo.holidayEndDateTime);
                var endDateString = holidayEndDate.toLocaleDateString("en-GB", dateOption) + endDateHalfDayString;

                document.getElementById('holiday-deletion-success-dates').innerText = ' ' + startDateString + ' to ' + endDateString;

            }
            document.getElementById('header-text').innerText = "Success"
            $('#holiday-deletion-result-modal').modal("show");
        }
        else {
            document.getElementById('header-text').innerText = "Error"
            document.getElementById('success-div').hidden = true;
            document.getElementById('error-div').hidden = false;
        }

    };
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.send(holidayRequestId);

}

function refreshPage() {
    window.location.reload(true);
}

function loadApprovalInfo(anchorTag) {

    var holidayRequestIdString = anchorTag.id;
    if (holidayRequestIdString.includes("approval") == true) {
        holidayRequestIdString = holidayRequestIdString.replace("holiday-approval-icon-", "");
        var holidayReqId = parseInt(holidayRequestIdString);

        document.getElementById('approve-button').innerText = "Approve";
        document.getElementById('approve-button').setAttribute('onclick', 'approveHolidayRequest(' + holidayRequestIdString + ')');
        document.getElementById('holiday-approval-header-text').innerText = "Confirm approval";
        var employeeName = document.getElementById('employee-fullname-hidden-field').value;
        document.getElementById('holiday-approval-confirmation-string').innerText = "Are you sure you want to approve " + employeeName + "'s holiday from ";


        var xhr = new XMLHttpRequest();
        xhr.open("POST", "HR/GetHolidayRequestDetails", true);
        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                var response = xhr.responseText;
                var holidayReqInfo = $.parseJSON(response);

                if (holidayReqInfo.status == 1 || holidayReqInfo.status == 3 || holidayReqInfo.status == 4) {
                    document.getElementById('approve-button').hidden = true;
                    document.getElementById('cancel-button').innerText = "Close";

                    var approverName = "";
                    var xhr2 = new XMLHttpRequest();
                    xhr2.open("POST", "HR/GetEmployeeObjectById", false);
                    xhr2.onreadystatechange = function () {
                        if (this.readyState == 4 && this.status == 200) {
                            var reponseReturned = xhr2.responseText;
                            var employeeObj = $.parseJSON(reponseReturned);
                            approverName = employeeObj.firstName + ' ' + employeeObj.surname;
                        }
                    };
                    xhr2.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                    xhr2.send(parseInt(holidayReqInfo.approvedByEmployeeId));

                    var dateOption = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
                    var approvalDate = new Date(holidayReqInfo.approvedDateTime).toLocaleDateString("en-GB", dateOption);

                    if (holidayReqInfo.status == 4) {
                        document.getElementById('approval-label-2').innerText = ' and ' + employeeName + ' is now on holiday.';
                    }
                    else if (holidayReqInfo.status == 3) {
                        document.getElementById('approval-label-2').innerText = ' and ' + employeeName + ' has already taken this holiday.';
                    }

                    document.getElementById('holiday-approval-confirmation-string').innerText = "This request has already been approved by " + approverName;
                    document.getElementById('approval-holiday-dates').innerText = "on " + approvalDate;
                }
                else if (holidayReqInfo.status == 2) {
                    document.getElementById('approve-button').hidden = true;
                    document.getElementById('cancel-button').innerText = "Close";

                    var approverName = "";
                    var xhr2 = new XMLHttpRequest();
                    xhr2.open("POST", "HR/GetEmployeeObjectById", false);
                    xhr2.onreadystatechange = function () {
                        if (this.readyState == 4 && this.status == 200) {
                            var reponseReturned = xhr2.responseText;
                            var employeeObj = $.parseJSON(reponseReturned);
                            approverName = employeeObj.firstName + ' ' + employeeObj.surname;
                        }
                    };
                    xhr2.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                    xhr2.send(parseInt(allHolidayRequests[i].rejectedByEmployeeId));

                    var dateOption = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
                    var approvalDate = new Date(holidayReqInfo.rejectedDateTime).toLocaleDateString("en-GB", dateOption);

                    document.getElementById('holiday-approval-confirmation-string').innerText = "This request has already been declined by " + approverName;
                    document.getElementById('approval-holiday-dates').innerText = "on " + approvalDate;
                }
                else {
                    document.getElementById('approve-button').hidden = false;
                    document.getElementById('cancel-button').innerText = "Cancel";
                    document.getElementById('approval-label-2').innerText = "?"

                    var dateOption = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
                    var holidayStartDate = new Date(holidayReqInfo.holidayStartDateTime);

                    var startDateHalfDayString = "";
                    if (holidayReqInfo.startDateFullOrAMOrPM == 1) {
                        startDateHalfDayString = " (Morning)";
                    }
                    else if (holidayReqInfo.startDateFullOrAMOrPM == 2) {
                        startDateHalfDayString = " (Afternoon)";
                    }

                    var startDateString = holidayStartDate.toLocaleDateString("en-GB", dateOption) + startDateHalfDayString;

                    var endDateHalfDayString = "";
                    if (holidayReqInfo.endDateFullOrAMOrPM == 1) {
                        endDateHalfDayString = " (Morning)";
                    }
                    else if (holidayReqInfo.endDateFullOrAMOrPM == 2) {
                        endDateHalfDayString = " (Afternoon)";
                    }

                    var holidayEndDate = new Date(holidayReqInfo.holidayEndDateTime);
                    var endDateString = holidayEndDate.toLocaleDateString("en-GB", dateOption) + endDateHalfDayString;

                    document.getElementById('approval-holiday-dates').innerText = ' ' + startDateString + ' to ' + endDateString;

                    //check if there are any holiday request in the team for  the same dates
                    var xhr1 = new XMLHttpRequest();
                    xhr1.open("POST", "HR/GetAllTeamHolidaysForDateRange", true);
                    xhr1.onreadystatechange = function () {
                        if (this.readyState == 4 && this.status == 200) {
                            var response = xhr1.responseText;
                            var allHolidayRequests = $.parseJSON(response);
                            if (allHolidayRequests.length > 0) {
                                var teamsHolidayTable = document.getElementById('teams-holiday-table');
                                if (teamsHolidayTable.rows.length > 2) {
                                    for (var j = teamsHolidayTable.rows.length - 1; j >= 2; j--) {
                                        teamsHolidayTable.deleteRow(j);
                                    }
                                }
                                document.getElementById('teams-holiday-div').hidden = false;
                                for (var i = 0; i < allHolidayRequests.length; i++) {
                                    var currentRowIndex = teamsHolidayTable.rows.length;
                                    var currentRow = teamsHolidayTable.insertRow(currentRowIndex);
                                    currentRow.setAttribute("align", "center");

                                    //first column value
                                    var currentCell = currentRow.insertCell(-1);
                                    var employeeLink = document.createElement("a");
                                    employeeLink.setAttribute("href", "HR/EmployeeProfile/" + allHolidayRequests[i].employeeId);

                                    var xhr2 = new XMLHttpRequest();
                                    xhr2.open("POST", "HR/GetEmployeeObjectById", false);
                                    xhr2.onreadystatechange = function () {
                                        if (this.readyState == 4 && this.status == 200) {
                                            var reponseReturned = xhr2.responseText;
                                            var employeeObj = $.parseJSON(reponseReturned);
                                            employeeLink.innerText = employeeObj.firstName + ' ' + employeeObj.surname;
                                        }
                                    };
                                    xhr2.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                                    xhr2.send(parseInt(allHolidayRequests[i].employeeId));

                                    currentCell.appendChild(employeeLink);

                                    //second column value
                                    currentCell = currentRow.insertCell(-1);
                                    var firstDayLabel = document.createElement("label");

                                    var dateOption = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
                                    var holidayStartDate = new Date(allHolidayRequests[i].holidayStartDateTime);

                                    var startDateHalfDayString = "";
                                    if (allHolidayRequests[i].startDateFullOrAMOrPM == 1) {
                                        startDateHalfDayString = " (Morning)";
                                    }
                                    else if (allHolidayRequests[i].startDateFullOrAMOrPM == 2) {
                                        startDateHalfDayString = " (Afternoon)";
                                    }

                                    var startDateString = holidayStartDate.toLocaleDateString("en-GB", dateOption) + startDateHalfDayString;
                                    firstDayLabel.innerText = startDateString;

                                    currentCell.appendChild(firstDayLabel);

                                    //third column value
                                    currentCell = currentRow.insertCell(-1);
                                    var lastDayLabel = document.createElement("label");
                                    var endDateHalfDayString = "";
                                    if (allHolidayRequests[i].endDateFullOrAMOrPM == 1) {
                                        endDateHalfDayString = " (Morning)";
                                    }
                                    else if (allHolidayRequests[i].endDateFullOrAMOrPM == 2) {
                                        endDateHalfDayString = " (Afternoon)";
                                    }

                                    var holidayEndDate = new Date(allHolidayRequests[i].holidayEndDateTime);
                                    var endDateString = holidayEndDate.toLocaleDateString("en-GB", dateOption) + endDateHalfDayString;

                                    lastDayLabel.innerText = endDateString;
                                    currentCell.appendChild(lastDayLabel);

                                    //fourth column value
                                    currentCell = currentRow.insertCell(-1);
                                    var totalDaysLabel = document.createElement("label");
                                    totalDaysLabel.innerText = allHolidayRequests[i].totalDays;
                                    currentCell.appendChild(totalDaysLabel);

                                    //fifth column value
                                    currentCell = currentRow.insertCell(-1);
                                    var statusLabel = document.createElement("label");
                                    var statusIcon = document.createElement("i");
                                    statusIcon.setAttribute("style", "vertical-align:text-bottom;");
                                    if (allHolidayRequests[i].status == 0) {
                                        statusLabel.setAttribute("class", "form-label text-info");
                                        statusLabel.innerText = "Pending approval";
                                        statusIcon.setAttribute("class", "ni ni-info fa-1x text-info");
                                    }
                                    else if (allHolidayRequests[i].status == 1) {
                                        statusLabel.setAttribute("class", "form-label text-success");
                                        statusLabel.innerText = "Approved";
                                        statusIcon.setAttribute("class", "ni ni-check fa-1x text-success");
                                    }
                                    else if (allHolidayRequests[i].status == 2) {
                                        statusLabel.setAttribute("class", "form-label text-warning");
                                        statusLabel.innerText = "Declined";
                                        statusIcon.setAttribute("class", "ni ni-ban fa-1x text-warning");
                                    }
                                    else if (allHolidayRequests[i].status == 3) {
                                        statusLabel.setAttribute("class", "form-label text-success");
                                        statusLabel.innerText = "Taken";
                                        statusIcon.setAttribute("class", "ni ni-check fa-1x text-success");
                                    }
                                    else if (allHolidayRequests[i].status == 4) {
                                        statusLabel.setAttribute("class", "form-label text-success");
                                        statusLabel.innerText = "On holiday";
                                        statusIcon.setAttribute("class", "ni ni-check fa-1x text-success");
                                    }

                                    currentCell.appendChild(statusIcon);
                                    currentCell.appendChild(statusLabel);
                                }

                                //debugger;
                            }
                            else {
                                document.getElementById('teams-holiday-div').hidden = true;
                            }
                        }
                    };
                    xhr1.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                    xhr1.send(holidayReqId);
                }
            }
        };
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        xhr.send(holidayReqId);
    }
    else if (holidayRequestIdString.includes("decline") == true) {
        holidayRequestIdString = holidayRequestIdString.replace("holiday-decline-icon-", "");
        var holidayReqId = parseInt(holidayRequestIdString);

        document.getElementById('approve-button').innerText = "Decline";
        document.getElementById('approve-button').setAttribute('onclick', 'declineHolidayRequest(' + holidayRequestIdString + ')');
        document.getElementById('holiday-approval-header-text').innerText = "Declining of holiday request";
        var employeeName = document.getElementById('employee-fullname-hidden-field').value;
        document.getElementById('holiday-approval-confirmation-string').innerText = "Are you sure you want to decline " + employeeName + "'s holiday from ";

        var xhr = new XMLHttpRequest();
        xhr.open("POST", "HR/GetHolidayRequestDetails", true);
        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                var response = xhr.responseText;
                var holidayReqInfo = $.parseJSON(response);

                if (holidayReqInfo.status == 1 || holidayReqInfo.status == 3 || holidayReqInfo.status == 4) {
                    document.getElementById('approve-button').hidden = true;
                    document.getElementById('cancel-button').innerText = "Close";

                    var approverName = "";
                    var xhr2 = new XMLHttpRequest();
                    xhr2.open("POST", "HR/GetEmployeeObjectById", false);
                    xhr2.onreadystatechange = function () {
                        if (this.readyState == 4 && this.status == 200) {
                            var reponseReturned = xhr2.responseText;
                            var employeeObj = $.parseJSON(reponseReturned);
                            approverName = employeeObj.firstName + ' ' + employeeObj.surname;
                        }
                    };
                    xhr2.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                    xhr2.send(parseInt(holidayReqInfo.approvedByEmployeeId));

                    var dateOption = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
                    var approvalDate = new Date(holidayReqInfo.approvedDateTime).toLocaleDateString("en-GB", dateOption);

                    if (holidayReqInfo.status == 4) {
                        document.getElementById('approval-label-2').innerText = ' and ' + employeeName + ' is now on holiday.';
                    }
                    else if (holidayReqInfo.status == 3) {
                        document.getElementById('approval-label-2').innerText = ' and ' + employeeName + ' has already taken this holiday.';
                    }

                    document.getElementById('holiday-approval-confirmation-string').innerText = "This request has already been approved by " + approverName;
                    document.getElementById('approval-holiday-dates').innerText = "on " + approvalDate;
                }
                else if (holidayReqInfo.status == 2) {
                    document.getElementById('approve-button').hidden = true;
                    document.getElementById('cancel-button').innerText = "Close";

                    var approverName = "";
                    var xhr2 = new XMLHttpRequest();
                    xhr2.open("POST", "HR/GetEmployeeObjectById", false);
                    xhr2.onreadystatechange = function () {
                        if (this.readyState == 4 && this.status == 200) {
                            var reponseReturned = xhr2.responseText;
                            var employeeObj = $.parseJSON(reponseReturned);
                            approverName = employeeObj.firstName + ' ' + employeeObj.surname;
                        }
                    };
                    xhr2.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                    xhr2.send(parseInt(allHolidayRequests[i].rejectedByEmployeeId));

                    var dateOption = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
                    var approvalDate = new Date(holidayReqInfo.rejectedDateTime).toLocaleDateString("en-GB", dateOption);

                    document.getElementById('holiday-approval-confirmation-string').innerText = "This request has already been declined by " + approverName;
                    document.getElementById('approval-holiday-dates').innerText = "on " + approvalDate;
                }
                else {
                    var dateOption = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
                    var holidayStartDate = new Date(holidayReqInfo.holidayStartDateTime);

                    var startDateHalfDayString = "";
                    if (holidayReqInfo.startDateFullOrAMOrPM == 1) {
                        startDateHalfDayString = " (Morning)";
                    }
                    else if (holidayReqInfo.startDateFullOrAMOrPM == 2) {
                        startDateHalfDayString = " (Afternoon)";
                    }

                    var startDateString = holidayStartDate.toLocaleDateString("en-GB", dateOption) + startDateHalfDayString;

                    var endDateHalfDayString = "";
                    if (holidayReqInfo.endDateFullOrAMOrPM == 1) {
                        endDateHalfDayString = " (Morning)";
                    }
                    else if (holidayReqInfo.endDateFullOrAMOrPM == 2) {
                        endDateHalfDayString = " (Afternoon)";
                    }

                    var holidayEndDate = new Date(holidayReqInfo.holidayEndDateTime);
                    var endDateString = holidayEndDate.toLocaleDateString("en-GB", dateOption) + endDateHalfDayString;

                    document.getElementById('approval-holiday-dates').innerText = ' ' + startDateString + ' to ' + endDateString;

                    //check if there are any holiday request in the team for  the same dates
                    var xhr1 = new XMLHttpRequest();
                    xhr1.open("POST", "HR/GetAllTeamHolidaysForDateRange", true);
                    xhr1.onreadystatechange = function () {
                        if (this.readyState == 4 && this.status == 200) {
                            var response = xhr1.responseText;
                            var allHolidayRequests = $.parseJSON(response);
                            if (allHolidayRequests.length > 0) {
                                var teamsHolidayTable = document.getElementById('teams-holiday-table');
                                if (teamsHolidayTable.rows.length > 2) {
                                    for (var j = teamsHolidayTable.rows.length - 1; j >= 2; j--) {
                                        teamsHolidayTable.deleteRow(j);
                                    }
                                }
                                document.getElementById('teams-holiday-div').hidden = false;
                                for (var i = 0; i < allHolidayRequests.length; i++) {
                                    var currentRowIndex = teamsHolidayTable.rows.length;
                                    var currentRow = teamsHolidayTable.insertRow(currentRowIndex);
                                    currentRow.setAttribute("align", "center");

                                    //first column value
                                    var currentCell = currentRow.insertCell(-1);
                                    var employeeLink = document.createElement("a");
                                    employeeLink.setAttribute("href", "HR/EmployeeProfile/" + allHolidayRequests[i].employeeId);

                                    var xhr2 = new XMLHttpRequest();
                                    xhr2.open("POST", "HR/GetEmployeeObjectById", false);
                                    xhr2.onreadystatechange = function () {
                                        if (this.readyState == 4 && this.status == 200) {
                                            var reponseReturned = xhr2.responseText;
                                            var employeeObj = $.parseJSON(reponseReturned);
                                            employeeLink.innerText = employeeObj.firstName + ' ' + employeeObj.surname;
                                        }
                                    };
                                    xhr2.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                                    xhr2.send(parseInt(allHolidayRequests[i].employeeId));

                                    currentCell.appendChild(employeeLink);

                                    //second column value
                                    currentCell = currentRow.insertCell(-1);
                                    var firstDayLabel = document.createElement("label");

                                    var dateOption = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
                                    var holidayStartDate = new Date(allHolidayRequests[i].holidayStartDateTime);

                                    var startDateHalfDayString = "";
                                    if (allHolidayRequests[i].startDateFullOrAMOrPM == 1) {
                                        startDateHalfDayString = " (Morning)";
                                    }
                                    else if (allHolidayRequests[i].startDateFullOrAMOrPM == 2) {
                                        startDateHalfDayString = " (Afternoon)";
                                    }

                                    var startDateString = holidayStartDate.toLocaleDateString("en-GB", dateOption) + startDateHalfDayString;
                                    firstDayLabel.innerText = startDateString;

                                    currentCell.appendChild(firstDayLabel);

                                    //third column value
                                    currentCell = currentRow.insertCell(-1);
                                    var lastDayLabel = document.createElement("label");
                                    var endDateHalfDayString = "";
                                    if (allHolidayRequests[i].endDateFullOrAMOrPM == 1) {
                                        endDateHalfDayString = " (Morning)";
                                    }
                                    else if (allHolidayRequests[i].endDateFullOrAMOrPM == 2) {
                                        endDateHalfDayString = " (Afternoon)";
                                    }

                                    var holidayEndDate = new Date(allHolidayRequests[i].holidayEndDateTime);
                                    var endDateString = holidayEndDate.toLocaleDateString("en-GB", dateOption) + endDateHalfDayString;

                                    lastDayLabel.innerText = endDateString;
                                    currentCell.appendChild(lastDayLabel);

                                    //fourth column value
                                    currentCell = currentRow.insertCell(-1);
                                    var totalDaysLabel = document.createElement("label");
                                    totalDaysLabel.innerText = allHolidayRequests[i].totalDays;
                                    currentCell.appendChild(totalDaysLabel);

                                    //fifth column value
                                    currentCell = currentRow.insertCell(-1);
                                    var statusLabel = document.createElement("label");
                                    var statusIcon = document.createElement("i");
                                    statusIcon.setAttribute("style", "vertical-align:text-bottom;");

                                    if (allHolidayRequests[i].status == 0) {
                                        statusLabel.setAttribute("class", "form-label text-info");
                                        statusLabel.innerText = "Pending approval";
                                        statusIcon.setAttribute("class", "ni ni-info fa-1x text-info");
                                    }
                                    else if (allHolidayRequests[i].status == 1) {
                                        statusLabel.setAttribute("class", "form-label text-success");
                                        statusLabel.innerText = "Approved";
                                        statusIcon.setAttribute("class", "ni ni-check fa-1x text-success");
                                    }
                                    else if (allHolidayRequests[i].status == 2) {
                                        statusLabel.setAttribute("class", "form-label text-warning");
                                        statusLabel.innerText = "Declined";
                                        statusIcon.setAttribute("class", "ni ni-ban fa-1x text-warning");
                                    }
                                    else if (allHolidayRequests[i].status == 3) {
                                        statusLabel.setAttribute("class", "form-label text-success");
                                        statusLabel.innerText = "Taken";
                                        statusIcon.setAttribute("class", "ni ni-check fa-1x text-success");
                                    }
                                    else if (allHolidayRequests[i].status == 4) {
                                        statusLabel.setAttribute("class", "form-label text-success");
                                        statusLabel.innerText = "On holiday";
                                        statusIcon.setAttribute("class", "ni ni-check fa-1x text-success");
                                    }

                                    currentCell.appendChild(statusIcon);
                                    currentCell.appendChild(statusLabel);
                                }

                                //debugger;
                            }
                            else {
                                document.getElementById('teams-holiday-div').hidden = true;
                            }
                        }
                    }


                };
                xhr1.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                xhr1.send(holidayReqId);

            }
        };
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        xhr.send(holidayReqId);
    }

}


function approveHolidayRequest(holidayRequestId) {

    var xhr = new XMLHttpRequest();
    xhr.open("POST", "HR/ApproveHolidayRequest", true);
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            $('#holiday-approval-modal').modal("hide");

            var response = xhr.responseText;
            var holidayReqInfo = $.parseJSON(response);
            if (holidayReqInfo != null) {
                document.getElementById('approval-success-div').hidden = false;
                document.getElementById('approval-error-div').hidden = true;

                var dateOption = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
                var holidayStartDate = new Date(holidayReqInfo.holidayStartDateTime);

                var startDateHalfDayString = "";
                if (holidayReqInfo.startDateFullOrAMOrPM == 1) {
                    startDateHalfDayString = " (Morning)";
                }
                else if (holidayReqInfo.startDateFullOrAMOrPM == 2) {
                    startDateHalfDayString = " (Afternoon)";
                }

                var startDateString = holidayStartDate.toLocaleDateString("en-GB", dateOption) + startDateHalfDayString;

                var endDateHalfDayString = "";
                if (holidayReqInfo.endDateFullOrAMOrPM == 1) {
                    endDateHalfDayString = " (Morning)";
                }
                else if (holidayReqInfo.endDateFullOrAMOrPM == 2) {
                    endDateHalfDayString = " (Afternoon)";
                }

                var holidayEndDate = new Date(holidayReqInfo.holidayEndDateTime);
                var endDateString = holidayEndDate.toLocaleDateString("en-GB", dateOption) + endDateHalfDayString;

                document.getElementById('holiday-approval-success-dates').innerText = ' ' + startDateString + ' to ' + endDateString;

            }
            document.getElementById('approval-header-text').innerText = "Success"
            $('#holiday-approval-result-modal').modal("show");
        }
        else {
            document.getElementById('approval-header-text').innerText = "Error"
            document.getElementById('approval-success-div').hidden = true;
            document.getElementById('approval-error-div').hidden = false;
        }

    };
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.send(holidayRequestId);

}


function declineHolidayRequest(holidayRequestId) {

    var xhr = new XMLHttpRequest();
    xhr.open("POST", "HR/DeclineHolidayRequest", true);
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            $('#holiday-approval-modal').modal("hide");

            var response = xhr.responseText;
            var holidayReqInfo = $.parseJSON(response);
            if (holidayReqInfo != null) {
                document.getElementById('decline-success-div').hidden = false;
                document.getElementById('decline-error-div').hidden = true;

                var dateOption = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
                var holidayStartDate = new Date(holidayReqInfo.holidayStartDateTime);

                var startDateHalfDayString = "";
                if (holidayReqInfo.startDateFullOrAMOrPM == 1) {
                    startDateHalfDayString = " (Morning)";
                }
                else if (holidayReqInfo.startDateFullOrAMOrPM == 2) {
                    startDateHalfDayString = " (Afternoon)";
                }

                var startDateString = holidayStartDate.toLocaleDateString("en-GB", dateOption) + startDateHalfDayString;

                var endDateHalfDayString = "";
                if (holidayReqInfo.endDateFullOrAMOrPM == 1) {
                    endDateHalfDayString = " (Morning)";
                }
                else if (holidayReqInfo.endDateFullOrAMOrPM == 2) {
                    endDateHalfDayString = " (Afternoon)";
                }

                var holidayEndDate = new Date(holidayReqInfo.holidayEndDateTime);
                var endDateString = holidayEndDate.toLocaleDateString("en-GB", dateOption) + endDateHalfDayString;

                document.getElementById('holiday-decline-success-dates').innerText = ' ' + startDateString + ' to ' + endDateString;

            }
            document.getElementById('decline-header-text').innerText = "Success"
            $('#holiday-decline-result-modal').modal("show");
        }
        else {
            document.getElementById('decline-header-text').innerText = "Error"
            document.getElementById('decline-success-div').hidden = true;
            document.getElementById('decline-error-div').hidden = false;
        }

    };
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.send(holidayRequestId);

}