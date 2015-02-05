
var deleteTipoActivoId;

$(document).ready(function () {
    $("#btnAcceptDeleteTipoActivo").click(DeleteTipoActivo);
    $(".DeleteTipoActivo").click(UpdateDeleteTipoActivoId);
});



function UpdateDeleteTipoActivoId() {
    deleteTipoActivoId = $(this).data("id");
}


function DeleteTipoActivo() {

    $.ajax({
        url: '/AssetsType/Delete',
        type: 'POST',
        data: JSON.stringify({ id: deleteTipoActivoId }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            window.location.href = data.Url;
        },
        error: function () {
            alert("error");
        }
    });

}



