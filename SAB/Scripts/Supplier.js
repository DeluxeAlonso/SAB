
var deleteSupplierId;

$(document).ready(function () {
    $("#btnAcceptDeleteSupplier").click(DeleteSupplier);
    $(".DeleteSupplier").click(UpdateDeleteSupplierId);
});



function UpdateDeleteSupplierId() {
    deleteSupplierId = $(this).data("id");
}


function DeleteSupplier() {

    $.ajax({
        url: '/Supplier/Delete',
        type: 'POST',
        data: JSON.stringify({ id: deleteSupplierId }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            window.location.href = data.Url;
        },
        error: function () {
            alert("error");
        }
    });

}



