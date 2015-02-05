
var arrayPetitioners = [];
var arrayPublications = [];
var proveedor = {};
var deletePurchaseOrderID;
var numPublicaciones = 0;
jQuery(document).ready(function () {

    
    $("#addPublication").click(AddPublication);
    $("#btnAddProvider").click(AddProvider);
    $(".DeletePurchaseOrder").click(UpdateDeletePurchaseOrderID);
    $("#btnAcceptDeletePurchaseOrder").click(DeletePurchaseOrder);

   
    $("#btnPublicationSearch").click(PublicationSearch);
    $("#btnProviderSearch").click(ProviderSearch);
    
    $("#PO_btnPrev").click(PO_Prev);
    $("#PO_btnNext").click(PO_Next);
    $("#PO_pageIndexChange").change(PO_PageIndexChange);
    $("#PO_btnFirst").click(PO_First);
    $("#PO_btnLast").click(PO_Last);
    
   
    $("#btnAddPublicacionModal").click(PublicationSearch);

    if ($("#providerName").val() == "") {
        $("#btnAddSolicitudModal").attr("disabled", "disabled");
        $("#btnAddPublicacionModal").attr("disabled", "disabled");
    }

    
    $("#btnPurchaseOrderSearch").click(function () { $("#PO_pageIndex").val(1); });

    OnClickPublicationSearch();
    
    OnClickProviderSearch();
});

function OnClickProviderSearch() {
    $("#PRO_btnPrev").on("click", function () { PRO_Prev(); })
    $("#PRO_btnNext").on("click", function () { PRO_Next(); });
    $("#PRO_pageIndexChange").on("change", function () { PRO_PageIndexChange(); });
    $("#PRO_btnFirst").on("click", function () { PRO_First(); });
    $("#PRO_btnLast").on("click", function () { PRO_Last(); });
}
function OnClickPublicationSearch() {
    $("#PT_btnPrev").on("click", function () { PT_Prev();})
    $("#PT_btnNext").on("click", function () { PT_Next(); });
    $("#PT_pageIndexChange").on("change", function () { PT_PageIndexChange(); });
    $("#PT_btnFirst").on("click", function () { PT_First(); });
    $("#PT_btnLast").on("click", function () { PT_Last(); });
}


function UpdateDeletePurchaseOrderID() {
    deletePurchaseOrderID = $(this).data("id");
}

function DeletePurchaseOrder() {

    $.ajax({
        url: '/PurchaseOrder/Delete',
        type: 'POST',
        data: JSON.stringify({id:deletePurchaseOrderID}),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            window.location.href = data.Url;
        },
            error: function () {
                alert("error");
            }
    });
    
}


function DeleteEntry() {
    var x = $(this).closest("tr");
    
    var id = x.find(":nth-child(1)").text();

    
    $(this).closest("tr").remove();
}

function AddPublication() {

    $("#publicationTable input[type='radio']:checked").each(function () {
        var x = $(this).closest("tr");
        var id = x.find(":nth-child(1)").text();
        var name = x.find(":nth-child(2)").text();
        arrayPublications.push(id);

        console.log(name);
        var html = '<tr>' +
            '<td>' + id + '<input type="text" name="publicationIDs" value=' + id + ' hidden>' + '</td>' +
            '<td>' + name + '<input type="text" name="publicationNames" value="'+name+'" hidden>' + '</td>' +
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
function AddProvider() {
    $("#providerTable input[type='radio']:checked").each(function () {
        var x = $(this).closest("tr");
        var id = x.find(":nth-child(1)").text();
        var name = x.find(":nth-child(2)").text();
        if (id != "" && name != "") {
            $("#providerID").val(id);
            $("#providerName").val(name.toString());
            $("#btnAddSolicitudModal").removeAttr("disabled");
            $("#btnAddPublicacionModal").removeAttr("disabled");
        }
    });
}




function PublicationSearch(event) {
    event.preventDefault();
    $("#PO_pageIndex.val").val(1);
    var data = $("#publicationForm").serialize();
    var providerID = $("#providerID").val();
    data += "&idProvider=" + providerID;
    $.post("/PurchaseOrder/PublicationSearch", data, function (datos) {
        console.log(data);

        $("#resultPublication").html(datos);
        OnClickPublicationSearch();

    });

}

function ProviderSearch(event) {
    event.preventDefault();

    var data = $("#providerForm").serialize();
    $.post("/PurchaseOrder/ProviderSearch", data, function (datos) {
        console.log(data);

        $("#resultProvider").html(datos);

        OnClickProviderSearch();
    });

}

function PO_Prev() {
    var index =parseInt($("#PO_pageIndex").val());
    $("#PO_pageIndex").val(index - 1);
    $("#PO_SearchForm").submit();
}

function PO_Next() {
    var index = parseInt($("#PO_pageIndex").val());    
    $("#PO_pageIndex").val(index + 1);
    $("#PO_SearchForm").submit();
}

function PO_PageIndexChange() {
    var index = parseInt($("#PO_pageIndexChange").val());
    $("#PO_pageIndex").val(index);
    $("#PO_SearchForm").submit();
}

function PO_First() {   
    var index = 1;
    $("#PO_pageIndex").val(index);
    $("#PO_SearchForm").submit();
}

function PO_Last() {
    var index = 100000;
    $("#PO_pageIndex").val(index);
    $("#PO_SearchForm").submit();
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

function PRO_Prev() {

    event.preventDefault();

    var index = parseInt($("#PRO_pageIndex").val());
    $("#PRO_pageIndex").val(index - 1);
    event.preventDefault();
    var data = $("#providerForm").serialize();
    $.post("/PurchaseOrder/ProviderSearch", data, function (datos) {
        console.log(data);
        $("#resultProvider").html(datos);
        OnClickProviderSearch();
    });
}
function PRO_Next() {

    var index = parseInt($("#PRO_pageIndex").val());
    $("#PRO_pageIndex").val(index + 1);
    event.preventDefault();
    var data = $("#providerForm").serialize();
    $.post("/PurchaseOrder/ProviderSearch", data, function (datos) {
        console.log(data);
        $("#resultProvider").html(datos);
        OnClickProviderSearch();
    });
}
function PRO_PageIndexChange() {
    event.preventDefault();
    var index = parseInt($("#PRO_pageIndexChange").val());
    $("#PRO_pageIndex").val(index);
    var data = $("#providerForm").serialize();
    $.post("/PurchaseOrder/ProviderSearch", data, function (datos) {
        console.log(data);
        $("#resultProvider").html(datos);
        OnClickProviderSearch();
    });
}

function PRO_First() {
    event.preventDefault();
    var index = 1;
    $("#PRO_pageIndex").val(index);
    var data = $("#providerForm").serialize();
    $.post("/PurchaseOrder/ProviderSearch", data, function (datos) {
        console.log(data);
        $("#resultProvider").html(datos);
        OnClickProviderSearch();
    });
}
function PRO_Last() {
    event.preventDefault();
    var index = 100000;
    $("#PRO_pageIndex").val(index);
    var data = $("#providerForm").serialize();
    $.post("/PurchaseOrder/ProviderSearch", data, function (datos) {
        console.log(data);
        $("#resultProvider").html(datos);
        OnClickProviderSearch();
    });
}