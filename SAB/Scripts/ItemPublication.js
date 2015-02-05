var deleteItemPublicationID;

jQuery(document).ready(function () {
    $(".DeleteItemPublication").click(UpdateDeleteItemPublicationID);
    $("#btnAcceptDeleteItemPublication").click(DeleteItemPublication);
})

function UpdateDeleteItemPublicationID() {
    deleteItemPublicationID = $(this).data("id");
}

function DeleteItemPublication() {

    $.ajax({
        url: '/Item/Delete',
        type: 'POST',
        data: JSON.stringify({ id: deleteItemPublicationID }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            window.location.href = data.Url;
        },
        error: function () {
            alert("error");
        }
    });

}