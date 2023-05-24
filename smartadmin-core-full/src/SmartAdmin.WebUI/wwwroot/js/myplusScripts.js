function loadTaskInfo(anchorTag) {
    var js = document.createElement("script");
    js.src = "~/js/formplugins/bootstrap-datepicker/bootstrap-datepicker.js";
    document.body.appendChild(js);

    var js1 = document.createElement("script");
    js1.src = "https://cdn.jsdelivr.net/jquery.validation/1.16.0/jquery.validate.min.js";
    document.body.appendChild(js1);

    var js2 = document.createElement("script");
    js2.src = "https://cdn.jsdelivr.net/jquery.validation/1.16.0/additional-methods.min.js";
    document.body.appendChild(js2);

    var js3 = document.createElement("script");
    js3.src = "~/js/formplugins/select2/select2.bundle.js";
    document.body.appendChild(js3);

    var css = document.createElement("link");
    css.rel = "stylesheet";
    css.media = "screen, print";
    css.href = "~/css/formplugins/bootstrap-datepicker/bootstrap-datepicker.css";
    document.head.appendChild(css);

    var css1 = document.createElement("link");
    css1.rel = "stylesheet";
    css1.media = "screen, print";
    css1.href = "~/css/formplugins/select2/select2.bundle.css";
    document.head.appendChild(css1);

    var controls = {
        leftArrow: '<i class="fal fa-angle-left" style="font-size: 1.25rem"></i>',
        rightArrow: '<i class="fal fa-angle-right" style="font-size: 1.25rem"></i>'
    };

    $('#datepicker-due-date').datepicker({
        todayHighlight: true,
        templates: controls,
        format: "dd/mm/yyyy",
        weekStart: 1,
        autoclose: true
    });

    var anchorTagId = anchorTag.id;
    var taskIdString = anchorTagId.replace("edit-task-id-", "");
    var taskId = parseInt(taskIdString);

    document.getElementById("task-action-string").innerText = document.getElementById("task-action-string-" + taskIdString).innerText;
    document.getElementById("due-date-string").innerText = document.getElementById("due-date-string-" + taskIdString).innerText;
    document.getElementById("employee-link").innerText = document.getElementById("employee-name-string-" + taskIdString).innerText;
    document.getElementById("employee-link").href = document.getElementById("employee-name-string-" + taskIdString).href;
    document.getElementById("task-id").value = taskId;
    var employeeId = parseInt(document.getElementById("employee-id-" + taskIdString).value);

    var xhr = new XMLHttpRequest();
    xhr.open("POST", "HR/GetAllEmployees", true);
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var response = xhr.responseText;
            var allEmployees = $.parseJSON(response);

            var employeeSelect = document.getElementById("employee-select");

            for (var j = 0; j < allEmployees.length; j++) {
                var employeeOption = document.createElement("option");
                employeeOption.setAttribute("value", allEmployees[j].id);
                employeeOption.innerText = allEmployees[j].firstName + ' ' + allEmployees[j].surname;
                if (employeeId == allEmployees[j].id) {
                    employeeOption.setAttribute("selected", "selected");
                }
                employeeSelect.appendChild(employeeOption);
            }

        }
    };
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.send();
    var xhr1 = new XMLHttpRequest();
    xhr1.open("POST", "HR/GetEmployeeTaskInfo", true);
    xhr1.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var response = xhr1.responseText;
            var taskInfo = $.parseJSON(response);
            var dueDateString = taskInfo.dueDateTime.substr(0, taskInfo.dueDateTime.indexOf("T"));
            var dueDate = dueDateString.split('-')[2] + '/' + dueDateString.split('-')[1] + '/' + dueDateString.split('-')[0];
            document.getElementById("datepicker-due-date").value = dueDate;

            var dueDateObject = new Date(dueDateString.split('/')[2] + '-' +
                dueDateString.split('/')[1] + '-' +
                dueDateString.split('/')[0]);
            $('#datepicker-due-date').datepicker("update", dueDateObject);
            document.getElementById("hot-task-checkbox").checked = taskInfo.isHot;
            //document.getElementById("specific-time-checkbox").checked = taskInfo.isTimeSpecific;
            document.getElementById("hot-task-toggle-label").innerText = "this task is hot";
            document.getElementById("complete-toggle-label").innerText = "this task is complete";
            document.getElementById("notes-textarea").value = taskInfo.progressNotes;

            if (taskInfo.completedDateTime == null) {
                document.getElementById("complete-checkbox").checked = false;
            }
            else {
                document.getElementById("complete-checkbox").checked = true;
            }


        }
    };
    xhr1.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr1.send(taskId);

}

function saveTask() {
    var isValid = $("#task-form").valid();
    if (isValid == true) {

        var employeeIdSelected = document.getElementById("employee-select").value;
        var dueDateValue = document.getElementById("datepicker-due-date").value;
        var notes = document.getElementById("notes-textarea").value;
        var hotTask = document.getElementById("hot-task-checkbox").checked;
        var isTaskCompleted = document.getElementById("complete-checkbox").checked;
        var taskId = document.getElementById("task-id").value;

        var stringToSend = taskId + '$' + employeeIdSelected + '$' + dueDateValue + '$' + notes + '$' + hotTask + '$' + isTaskCompleted;

        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                var employeeSelect = document.getElementById("employee-select");
                var employeeNameSelected = employeeSelect.options[employeeSelect.selectedIndex].text;

                var actionString = document.getElementById("task-action-string").innerText;
                var response = xhr.responseText;
                var taskInfo = response;

                $('#edit-task-modal').modal("hide");
                if (taskInfo != null) {
                    document.getElementById("task-updated-header-text").innerText = "Success";
                    document.getElementById("task-updated-success-string").innerText = "Successfully updated " + actionString + " task for " + employeeNameSelected;
                }
                else {
                    document.getElementById("task-updated-header-text").innerText = "Error";
                    document.getElementById("task-updated-warning-string").innerText = "Error occured while updating " + actionString + " task for " + employeeNameSelected;
                }
                $('#task-updated-result-modal').modal("show");
            }
        };
        xhr.open("POST", "HR/UpdateEmployeeTask", true);
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        xhr.send(stringToSend);
    }
}