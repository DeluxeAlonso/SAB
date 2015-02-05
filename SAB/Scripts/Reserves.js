jQuery(document).ready(function ($) {

    $("#PRS_btnPrev").click(PRS_Prev);
    $("#PRS_btnNext").click(PRS_Next);
    $("#PRS_pageIndexChange").change(PRS_PageIndexChange);
    $("#PRS_btnFirst").click(PRS_First);
    $("#PRS_btnLast").click(PRS_Last);

    $('#submit-search').click(function (e) {
        
        e.preventDefault();
        $("#PRS_pageIndex").val(1);
        $("#ReserveCubicleForm").submit();


    });


});

function PRS_Prev() {
    var index = parseInt($("#PRS_pageIndex").val());
    $("#PRS_pageIndex").val(index - 1);
    $("#ReserveCubicleForm").submit();
}
function PRS_Next() {
    var index = parseInt($("#PRS_pageIndex").val());
    $("#PRS_pageIndex").val(index + 1);
    $("#ReserveCubicleForm").submit();
}
function PRS_PageIndexChange() {
    var index = parseInt($("#PRS_pageIndexChange").val());
    $("#PRS_pageIndex").val(index);
    $("#ReserveCubicleForm").submit();
}

function PRS_First() {

    var index = 1;
    $("#PRS_pageIndex").val(index);
    $("#ReserveCubicleForm").submit();
}
function PRS_Last() {

    var index = 100000;
    $("#PRS_pageIndex").val(index);
    $("#ReserveCubicleForm").submit();
}
