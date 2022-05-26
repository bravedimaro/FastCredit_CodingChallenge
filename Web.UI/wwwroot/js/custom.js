$(document).ready(function () {
    $('.user-table').DataTable({
        "deferRender": false,
        "paging": false,
        "lengthChange": false,
        "searching": true,
        "ordering": false,
        "info": false,
        "bDestroy": true,
        "autoWidth": false,
        "sDom": 'lfrtip'


    });
});


// button trigger to call Modal
var SelectedUserId=[];
$(".add_new").on("click", function () {
    $(this).removeAttr("href");
    $("#NewUserModal").modal();
})
//function to select all rows to be deleted
function selects() {
    var ele = document.getElementsByName('chk');
    for (var i = 0; i < ele.length; i++) {
        if (ele[i].type == 'checkbox')
            ele[i].checked = true;
    }
}
//function to deselect all rows to be deleted
function deSelect() {
    var ele = document.getElementsByName('chk');
    for (var i = 0; i < ele.length; i++) {
        if (ele[i].type == 'checkbox')
            ele[i].checked = false;

    }
}
var allIDs = [];
$(".delete_all").on("click", function () {
    $(this).removeAttr("href");
    selects();
    //check for session
    var GetSession = sessionStorage.getItem("Session");
    if (GetSession == "undefined" || GetSession == null) {
        alert("Kindly Active Session first");
        return false;
    }
    var resp = confirm("Are you sure you want to Delete All");
    if (resp) {
        //call posting service
        $.each($("input[name='chk']:checked"), function () {
            allIDs.push($(this).val());
        });
        const str = String(allIDs);
        var settings = {
            "url": "https://api.sataide.com/api/Users/DeleteBySelected/"+str,
            "method": "POST",
            headers: {
                "Authorization": "Bearer" + " " + "" + GetSession + ""
            },
            "timeout": 0,
        };

        $.ajax(settings).done(function (response) {
            if (response.responseCode == "00") {
                alert("" + response.responseMessage + "");
                setTimeout(function () {
                    location.reload();

                }, 3000);
            } else {
                alert("" + response.responseMessage + "");
            }
        });
    } else {
        deSelect()
    }
    
})
$(".activate_session").on("click", function () {
    $(this).removeAttr("href");
    var settings = {
        "url": "https://api.sataide.com/api/Auth/Token/juYSPnPCn4r0TmvhS4DOq6YdfAk0KkLsSIXNBlddKj65q0_MV1ThB1Mk44pdjLMx",
        "method": "POST",
        "timeout": 0,
    };

    $.ajax(settings).done(function (response) {
        if (response.responseCode=="00") {
            alert("Session Activated");
            var SetSession = response.data.access_token;
            sessionStorage.setItem("Session", SetSession);
        } else {
            alert("Session Not Activated, try again");
        }
        
    });
    
})
$("#cupForm").on('submit', function (e) {
    var GetSession = sessionStorage.getItem("Session");
    if (GetSession == "undefined" || GetSession == null) {
        alert("Kindly Active Session first");
        return false;
    }
    var FirstName = $('#FirstName').val();
    var LastName = $('#LastName').val();
    var Email = $('#Email').val();
    var Dob = $('#Dob').val();
    var Nationality = $('#Nationality').val();
   

    if (FirstName== "") {
        alert("FirstName is Reqiured");
        return false;
    }
    if (LastName== '') {
        alert("LastName is Reqiured");
        return false;
    }
    if (Email == '') {
        toastr.error("Email is Reqiured");
        return false;
    }
    if (Dob == '') {
        alert("Date of birth is Reqiured");
        return false;
    }
    if (Nationality == '') {
        alert("Nationality is Reqiured");
        return false;
    }
   

    if ($("#Role")[0].selectedIndex <= 0) {
        alert("Role is Reqiured");
       
        return false;
    }

    if ($("#Gender")[0].selectedIndex <= 0) {
        alert("Gender field is Reqiured");
       
        return false;
    }

    
    e.preventDefault();
    $.ajax({
        type: 'POST',
        url: "https://api.sataide.com/api/Users/create",
        headers: {
            "Authorization": "Bearer"+" "+""+GetSession+"" 
        },
        data: new FormData(this),
        processData: false,
        contentType: false,
        beforeSend: function () {
            $('.add_users_btn').attr("disabled", "disabled");
            $('#cupForm').css("opacity", ".5");
        },
        success: function (response) {
            var Dataobj = response;
            $(Dataobj).each(function (index, item) {
                console.log("response from API is :" + item);
                var getmessage = item.responseMessage;
                var getCode = item.responseCode;
                if (getCode === "00") {
                    $('#cupForm')[0].reset();
                    alert("" + getmessage + "");
                    setTimeout(function () {
                        location.reload();

                    }, 3000);
                }
                if (getCode === "01") {
                    alert("" + getmessage + "");
                }
            });

        },
        complete: function (jqXHR, status) {
            $('#cupForm').css("opacity", "");
            $(".add_users_btn").removeAttr("disabled");
        }
    });
});

$(document).on("click",".delete", function () {
    var id = $(this).attr("id");
    //check for session
    var GetSession = sessionStorage.getItem("Session");
    if (GetSession == "undefined" || GetSession == null) {
        alert("Kindly Active Session first");
        return false;
    }
    var resp = confirm("Are you sure you want to delete this record")
    if (resp) {
        //post to delete endpoint
        var settings = {
            "url": "https://api.sataide.com/api/Users/Delete/"+id,
            "method": "POST",
            headers: {
                "Authorization": "Bearer" + " " + "" + GetSession + ""
            },
            "timeout": 0,
        };

        $.ajax(settings).done(function (response) {
            if (response.responseCode=="00") {
                alert("" + response.responseMessage + "");
                setTimeout(function () {
                    location.reload();

                }, 3000);
            } else {
                alert("" + response.responseMessage + "");
            }
           
        });
    }
})