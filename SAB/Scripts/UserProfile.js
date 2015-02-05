
var deleteUserProfileId;

$(document).ready(function () {
    $("#btnAcceptDeleteUserProfile").click(DeleteUserProfile);
    $(".DeleteUserProfile").click(UpdateDeleteUserProfileId);
   
    var requiredCheckboxesAct = $('#actionTable input[type="checkbox"]:checked');
    var allCheckboxesAct = $('#actionTable input[type="checkbox"]');

    allCheckboxesAct.change(function () {
        if (allCheckboxesAct.is(':checked')) {
            allCheckboxesAct.removeAttr('required');
        }
        else {
            requiredCheckboxesAct.attr('required', 'required');
        }
    });
    var requiredCheckboxesPub = $('#publicationTable input[type="checkbox"]:checked');
    var allCheckboxesPub = $('#publicationTable input[type="checkbox"]');
    allCheckboxesPub.change(function () {
        if (allCheckboxesPub.is(':checked')) {
            allCheckboxesPub.removeAttr('required');
        }
        else {
            requiredCheckboxesPub.attr('required', 'required');
        }
    });
    var requiredCheckboxesActm = $('#actionTablem input[type="checkbox"]:checked');
    var allCheckboxesActm = $('#actionTablem input[type="checkbox"]');
    allCheckboxesActm.change(function () {
        if (allCheckboxesActm.is(':checked')) {
            allCheckboxesActm.removeAttr('required');
        }
        else {
            allCheckboxesActm.attr('required', 'required');
        }
    });
    var requiredCheckboxesPubm = $('#publicationTablem input[type="checkbox"]:checked');
    var allCheckboxesPubm = $('#publicationTablem input[type="checkbox"]');
    allCheckboxesPubm.change(function () {
        if (allCheckboxesPubm.is(':checked')) {
            allCheckboxesPubm.removeAttr('required');
        }
        else {
            allCheckboxesPubm.attr('required', 'required');
        }
    });


    
});


function UpdateDeleteUserProfileId() {
    deleteUserProfileId = $(this).data("id");
}


function DeleteUserProfile() {

    $.ajax({
        url: '/UserProfile/Delete',
        type: 'POST',
        data: JSON.stringify({ id: deleteUserProfileId }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            window.location.href = data.Url;
        },
        error: function () {
            alert("error");
        }
    });

}



