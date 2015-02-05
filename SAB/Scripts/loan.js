$(document).ready(function () {
    $('.multi-field-wrapper').each(function () {
        var $wrapper = $('.multi-fields', this);
        $(".add-field", $(this)).click(function (e) {
            $('.multi-field:first-child', $wrapper).clone(true).appendTo($wrapper).find('input').val('').focus();
        });
        $('.multi-field .remove-field', $wrapper).click(function () {
            if ($('.multi-field', $wrapper).length > 1)
                $(this).parent('.multi-field').remove();
        });
    });
    $("#PRS_btnPrev").click(PRS_Prev);
    $("#PRS_btnNext").click(PRS_Next);
    $("#PRS_pageIndexChange").change(PRS_PageIndexChange);
    $("#PRS_btnFirst").click(PRS_First);
    $("#PRS_btnLast").click(PRS_Last);
});

function PRS_Prev() {
    var index = parseInt($("#PRS_pageIndex").val());
    $("#PRS_pageIndex").val(index - 1);
    $("#SearchLoanForm").submit();
}
function PRS_Next() {
    var index = parseInt($("#PRS_pageIndex").val());
    $("#PRS_pageIndex").val(index + 1);
    $("#SearchLoanForm").submit();
}
function PRS_PageIndexChange() {
    var index = parseInt($("#PRS_pageIndexChange").val());
    $("#PRS_pageIndex").val(index);
    $("#SearchLoanForm").submit();
}

function PRS_First() {

    var index = 1;
    $("#PRS_pageIndex").val(index);
    $("#SearchLoanForm").submit();
}
function PRS_Last() {

    var index = 100000;
    $("#PRS_pageIndex").val(index);
    $("#SearchLoanForm").submit();
}
