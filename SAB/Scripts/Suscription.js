jQuery(document).ready(function () {
    $("#addPublication").click(AddPublication);
    $("#btnPublicationSearch").click(PublicationSearch);

});

function AddPublication() {
    $("#publicationTable input[type='radio']:checked").each(function () {
        var x = $(this).closest("tr");
        
        var id = x.find(":nth-child(1)").text();
        var name = x.find(":nth-child(2)").text();

        var idEdit = x.attr("data-idEditorial");
        var idType = x.attr("data-idType");
        console.log(x);
        console.log(idEdit);
        $("#publicationID").val(id);
        $("#publicationName").val(name.toString());
        $("#editorialID").val(idEdit);
        $("#typeID").val(idType);
        console.log(id);
    });
}

function PublicationSearch(event) {
    event.preventDefault();

    var data = $("#publicationForm").serialize();
    $.post("/PurchaseOrder/PublicationSearch", data, function (datos) {
        console.log(data);

        $("#resultPublication").html(datos);


    });

}