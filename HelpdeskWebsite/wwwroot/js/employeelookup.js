$(() => {
    $("#getbutton").on("click", async (e) => {
        try {
            const email = $("#TextBoxEmail").val();
            $("#status").text("please wait...");
            let response = await fetch(`/api/employee/${email}`);
            if (response.ok) {
                let data = await response.json();
                console.log(data);
                if (data.email !== "not found") {
                    $("#title").text(data.title);
                    $("#firstname").text(data.firstname);
                    $("#lastname").text(data.lastname);
                    $("#phone").text(data.phoneNo);
                    $("#status").text("Employee found");
                } else {
                    $("#title").text("not found");
                    $("#firstname").text("not found");
                    $("#lastname").text("not found");
                    $("#phone").text("not found");
                    $("#status").text("Employee NOT found");
                }
            } else if (response.status !== 404) {
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else {
                $("#status").text("No such path on server");
            }
        } catch (error) {
            $("#status").text(error.message);
        }
    })
});

const errorRtn = (problemJson, responseStatus) => {
    if (responseStatus > 499) {
        $("#status").text("Problem server side, see debug console");
    } else {
        let keys = Object.keys(problemJson.errors);
        const problem = {
            status: responseStatus,
            statusText: problemJson.errors[keys[0]][0],
        }
        $("#status").text("Problem client side, see browser console.");
        console.log(problem);
    }
}
