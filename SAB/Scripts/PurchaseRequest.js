var deletePurchaseRequestID;

jQuery(document).ready(function () {

    $(".DeletePurchaseRequest").click(UpdateDeletePurchaseRequestID);
    $("#btnAcceptDeletePurchaseRequest").click(DeletePurchaseRequest);
    $("#addPublication").click(AddPublication);
    $("#btnPublicationSearch").click(PublicationSearch);
    $("#addFila").click(AddFila);
    $("#PRS_btnPrev").click(PRS_Prev);
    $("#PRS_btnNext").click(PRS_Next);
    $("#PRS_pageIndexChange").change(PRS_PageIndexChange);
    $("#PRS_btnFirst").click(PRS_First);
    $("#PRS_btnLast").click(PRS_Last);

    $("#btnPurchaseRequestSearch").click(function () { $("#PRS_pageIndex").val(1); });

    OnClickPublicationSearch();
});
function OnClickPublicationSearch() {
    $("#PT_btnPrev").on("click", function () { PT_Prev(); })
    $("#PT_btnNext").on("click", function () { PT_Next(); });
    $("#PT_pageIndexChange").on("change", function () { PT_PageIndexChange(); });
    $("#PT_btnFirst").on("click", function () { PT_First(); });
    $("#PT_btnLast").on("click", function () { PT_Last(); });
}


function UpdateDeletePurchaseRequestID() {
    deletePurchaseRequestID = $(this).data("id");
}
function DeleteEntry() {
    var x = $(this).closest("tr");

    var id = x.find(":nth-child(1)").text();


    $(this).closest("tr").remove();
}

function DeletePurchaseRequest() {

    $.ajax({
        url: '/PurchaseRequest/DeletePurchaseRequest',
        type: 'POST',
        data: JSON.stringify({ id: deletePurchaseRequestID }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            window.location.href = data.Url;
        },
        error: function () {
            alert("error");
        }
    });

}


function AddPublication() {

    $("#publicationTable input[type='radio']:checked").each(function () {
        var x = $(this).closest("tr");
        var id = x.find(":nth-child(1)").text();
        var name = x.find(":nth-child(2)").text();

        console.log(name);
        var html = '<tr>' +
            '<td>' + id + '<input type="text" name="publicationIDs" value=' + id + ' hidden>' + '</td>' +
            '<td>' + name + '<input type="text" name="publicationNames" value="' + name + '" hidden>' + '</td>' +
            '<td><input name= "publicationQuantities" type="number" class="form-control" placeholder="" min="1" max="100" required></td>' +
            '<td class= "text-center"><a class="deleteEntry"><i class="margin-right-icon glyphicon glyphicon-remove"></i></a></td>' +
            +'</tr>';

        var alreadyAdded = false;
        $("#AddedPublicationTable tr").each(function () {
            if ($(this).find(":nth-child(1)").text() == id) {
                alreadyAdded = true;
                return;
            }
        });
        if (alreadyAdded == false) {

            $("#AddedPublicationTable").append(html);
            $("#AddedPublicationTable").off('click', '.deleteEntry', DeleteEntry);
            $("#AddedPublicationTable").on('click', '.deleteEntry', DeleteEntry);

        }
    });
}

function PublicationSearch(event) {
    event.preventDefault();
    
    var data = $("#publicationForm").serialize();
    $.post("/PurchaseOrder/PublicationSearch", data, function (datos) {
        console.log(data);

        $("#resultPublication").html(datos);
        OnClickPublicationSearch();

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
function PT_Prev() {

    event.preventDefault();

    var index = parseInt($("#PT_pageIndex").val());
    $("#PT_pageIndex").val(index - 1);
    event.preventDefault();
    var data = $("#publicationForm").serialize();
    $.post("/PurchaseOrder/PublicationSearch", data, function (datos) {
        console.log(data);
        $("#resultPublication").html(datos);
        OnClickPublicationSearch();
    });
}
function PT_Next() {

    var index = parseInt($("#PT_pageIndex").val());
    $("#PT_pageIndex").val(index + 1);
    event.preventDefault();
    var data = $("#publicationForm").serialize();
    $.post("/PurchaseOrder/PublicationSearch", data, function (datos) {
        console.log(data);
        $("#resultPublication").html(datos);
        OnClickPublicationSearch();
    });
}
function PT_PageIndexChange() {
    event.preventDefault();
    var index = parseInt($("#PT_pageIndexChange").val());
    $("#PT_pageIndex").val(index);
    var data = $("#publicationForm").serialize();
    $.post("/PurchaseOrder/PublicationSearch", data, function (datos) {
        console.log(data);
        $("#resultPublication").html(datos);
        OnClickPublicationSearch();
    });
}

function PT_First() {
    event.preventDefault();
    var index = 1;
    $("#PT_pageIndex").val(index);
    var data = $("#publicationForm").serialize();
    $.post("/PurchaseOrder/PublicationSearch", data, function (datos) {
        console.log(data);
        $("#resultPublication").html(datos);
        OnClickPublicationSearch();
    });
}
function PT_Last() {
    event.preventDefault();
    var index = 100000;
    $("#PT_pageIndex").val(index);
    var data = $("#publicationForm").serialize();
    $.post("/PurchaseOrder/PublicationSearch", data, function (datos) {
        console.log(data);
        $("#resultPublication").html(datos);
        OnClickPublicationSearch();
    });
}


function AddFila() {

   
        console.log(name);
        var html = '<tr>' +
                                    '<td><input type="text" name="ISBNs" pattern="[0-9]*" title="Solo números" required></td>' +
                                    '<td><input type="text" name="publicaciones"></td>' +
                                    '<td><input type="text" name="proveedores"></td>' +
                                    '<td><input type="number" step="0.01" name="precios"></td>' +
                                    '<td class= "text-center"><a class="deleteEntry"><i class="margin-right-icon glyphicon glyphicon-remove"></i></a></td>'+
                                '</tr>';

        
        

            $("#AddedPublicationTable").append(html);
            $("#AddedPublicationTable").off('click', '.deleteEntry', DeleteEntry);
            $("#AddedPublicationTable").on('click', '.deleteEntry', DeleteEntry);

      
}