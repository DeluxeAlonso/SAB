
var deleteActivoId;

$(document).ready(function () {
    $("#btnAcceptDeleteActivo").click(DeleteActivo);
    $(".DeleteActivo").click(UpdateDeleteActivoId);
    $("#PRS_btnPrev").click(PRS_Prev);
    $("#PRS_btnNext").click(PRS_Next);
    $("#PRS_pageIndexChange").change(PRS_PageIndexChange);
    $("#PRS_btnFirst").click(PRS_First);
    $("#PRS_btnLast").click(PRS_Last);

});



function UpdateDeleteActivoId() {
    deleteActivoId = $(this).data("id");
}


function DeleteActivo() {

    $.ajax({
        url: '/Assets/Delete',
        type: 'POST',
        data: JSON.stringify({ id: deleteActivoId }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            window.location.href = data.Url;
        },
        error: function () {
            alert("error");
        }
    });

}


function PRS_Prev() {
    var index = parseInt($("#PRS_pageIndex").val());
    $("#PRS_pageIndex").val(index - 1);
    $("#PRS_SearchForm").submit();
}
function PRS_Next() {
    var index = parseInt($("#PRS_pageIndex").val());
    $("#PRS_pageIndex").val(index + 1);
    $("#PRS_SearchForm").submit();
}
function PRS_PageIndexChange() {
    var index = parseInt($("#PRS_pageIndexChange").val());
    $("#PRS_pageIndex").val(index);
    $("#PRS_SearchForm").submit();
}

function PRS_First() {

    var index = 1;
    $("#PRS_pageIndex").val(index);
    $("#PRS_SearchForm").submit();
}
function PRS_Last() {

    var index = 100000;
    $("#PRS_pageIndex").val(index);
    $("#PRS_SearchForm").submit();
}

