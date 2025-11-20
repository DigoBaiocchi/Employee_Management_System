$(() => {
    const getAll = async (msg) => {
        try {
            $("#employeeList").text("Finding Employee Information...");
            let response = await fetch(`api/employee`);
            if (response.ok) {
                let payload = await response.json(); // this returns a promise, so we await it
                buildEmployeeList(payload);
                msg === "" ? // are we appending to an existing message
                    $("#status").text("Employees Loaded") : $("#status").text(`${msg} - Employees Loaded`);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("$#status").text("no such path on server");
            } // else
            // get department data
            response = await fetch(`api/department`);
            if (response.ok) {
                let divs = await response.json(); // this returns a promise, so we await it
                sessionStorage.setItem("alldepartments", JSON.stringify(divs));
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) {
            $("#status").text(error.message);
        } // end try / catch
    } // get All

    const buildEmployeeList = (data) => {
        $("#employeeList").empty();
        div = $(`<div class="list-group-item text-white bg-secondary row d-flex" id="status">Employee Info</div>
                    <div class="list-group-item row d-flex text-center" id="heading">
                    <div class="col-4 h4">Title</div>
                    <div class="col-4 h4">First</div>
                    <div class="col-4 h4">Last</div>
                </div>`);
        div.appendTo($("#employeeList"));
        sessionStorage.setItem("allemployees", JSON.stringify(data));
        btn = $(`<button class="list-group-item row d-flex" id="0">...click to add employee</button>`);
        btn.appendTo($("#employeeList"));
        data.forEach(emp => {
            btn = $(`<button class="list-group-item row d-flex" id="${emp.id}">`);
            btn.html(`<div class="col-4" id="employeetitle${emp.id}">${emp.title}</div>
                        <div class="col-4" id="employeefname${emp.id}">${emp.firstname}</div>
                        <div class="col-4" id="employeelname${emp.id}">${emp.lastname}</div>`);
            btn.appendTo($("#employeeList"));
        }); // forEach
    }; // buildEmployeeList

    getAll(""); // first grab the data from the server

    $("#employeeList").on("click", e => {
        if (!e) e = new Event();
        let id = e.target.parentNode.id;
        if (id === "employeeList" || id === "") {
            id = e.target.id;
        } // clicked on row somewhere else
        if (id !== "status" && id !== "heading") {
            let data = JSON.parse(sessionStorage.getItem("allemployees"));
            id === "0" ? setupForAdd() : setupForUpdate(id, data);
        } else {
            return false; // ignore if they clicked on heading or status
        }
    });

    //$("#actionbutton").on('click', async (e) => {
    const update = async (e) => {
        // action button click event handler
        try {
            // set up a new client side instance of employee
            let emp = JSON.parse(sessionStorage.getItem("employee"));
            // populate the properties
            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirst").val();
            emp.lastname = $("#TextBoxSurname").val();
            emp.email = $("#TextBoxEmail").val();
            emp.phoneno = $("#TextBoxPhone").val();
            emp.departmentId = parseInt($("#ddlDepartments").val());
            // send the updated back to the server asynchronously using HTTP PUT
            let response = await fetch("api/employee", {
                method: "PUT",
                headers: { "Content-type": "application/json; charset=utf-8" },
                body: JSON.stringify(emp),
            });

            if (response.ok) {
                // or check for response.status
                let payload = await response.json();
                getAll(payload.msg);
            } else if (response.status !== 404) {
                // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, resposne.status);
            } else {
                // else 404 not found
                $("#status").text("no such path on server");
            } // end else
        } catch (error) {
            $("#status").text(error.message);
            console.table(error)
        }
        $("#theModal").modal("toggle");
    };

    const clearModalFields = () => {
        loadDepartmentDDL(-1);
        $("#TextBoxTitle").val("");
        $("#TextBoxFirst").val("");
        $("#TextBoxSurname").val("");
        $("#TextBoxEmail").val("");
        $("#TextBoxPhone").val("");
        sessionStorage.removeItem("employee");
        $("#theModal").modal("toggle");
    }; // clearModalFields

    const setupForAdd = () => {
        $("#actionbutton").val("add");
        $("#actionbutton").text("Add");
        $("#theModalLabel").html("<h4>Add employee</h4>");
        $("#theModal").modal("toggle");
        $("#modalstatus").text("add new employee");
        $("theModalLabel").text("Add");
        $("#deletebutton").hide();
        clearModalFields();
    }; // setupForAdd

    const setupForUpdate = (id, data) => {
        $("#actionbutton").val("update");
        $("#actionbutton").text("Update");
        $("#theModalLabel").html("<h4>update employee</h4>");

        clearModalFields();
        data.forEach(employee => {
            if (employee.id === parseInt(id)) {
                $("#TextBoxTitle").val(employee.title);
                $("#TextBoxFirst").val(employee.firstname);
                $("#TextBoxSurname").val(employee.lastname);
                $("#TextBoxEmail").val(employee.email);
                $("#TextBoxPhone").val(employee.phoneno);
                sessionStorage.setItem("employee", JSON.stringify(employee));
                $("#modalstatus").text("update data");
                $("#theModal").modal("toggle");
                $("#theModalLabel").text("Update");
                $("#deletebutton").show();
                loadDepartmentDDL(employee.departmentId);
            }
        })
    }; // setupForUpdate

    const add = async () => {
        try {
            emp = new Object();
            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirst").val();
            emp.lastname = $("#TextBoxSurname").val();
            emp.email = $("#TextBoxEmail").val();
            emp.phoneno = $("#TextBoxPhone").val();
            emp.departmentId = parseInt($("#ddlDepartments").val()); // hard code it for now
            emp.id = -1;
            emp.timer = null;
            emp.staffPicture64 = null;
            // send the employee info to the server asynchronously using POST
            let response = await fetch("api/employee", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(emp)
            });
            if (response.ok) // or check for response.status
            {
                let data = await response.json();
                getAll(data.msg);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                getAll(data.msg);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            }
        } catch (error) {
            $("#status").text(error.message);
        }

        $("#theModal").modal("toggle");
    }// add

    const _delete = async () => {
        let employee = JSON.parse(sessionStorage.getItem("employee"));
        try {

            let response = await fetch(`api/employee/${employee.id}`, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            });
            if (response.ok) // or check for response.status
            {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $('#status').text(`Status - ${response.status}, Problem on delete server side, see server console`);
            } // else
            $('#theModal').modal('toggle');
        } catch (error) {
            $('#status').text(error.message);
        }
    }; // _delete

    const loadDepartmentDDL = (deptdiv) => {
        html = '';
        $('#ddlDepartments').empty();
        let allDepartments = JSON.parse(sessionStorage.getItem('alldepartments'));
        allDepartments.forEach((dept) => { html += `<option value="${dept.id}">${dept.name}</option>` });
        $('#ddlDepartments').append(html);
        $('#ddlDepartments').val(deptdiv);
    }; // loadDepartmentDDL

    $("#deletebutton").on("click", () => {
        $("#dialog").show();
    }); // deletebutton click

    $("#actionbutton").on("click", () => {
        $("#actionbutton").val() === "update" ? update() : add();
    }); // action button click

    $("#dialog").hide();

    $("#nobutton").on("click", (e) => {
        $("#dialog").hide();
        $("#modalstatus").text("delete cancelled");
    });

    $("#yesbutton").on("click", () => {
        $("#dialog").hide();
        _delete();
    });
}); // jQuery ready method

// server was reached but server had a problem with the call
const errorRtn = (problemJson, status) => {
    if (status > 499) {
        $("#status").text("Problem server side, see debug console");
    } else {
        let keys = Object.keys(problemJson.errors);
        problem = {
            status: status,
            statusText: problemJson.errors[keys[0]][0], // first error
        };
        $("#status").text("Problem client side, see browser console");
        console.log(problem);
    } // end if else
}