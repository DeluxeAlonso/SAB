var deletePublicationId;
var selectLibraryId;
var file;

jQuery(document).ready(function ()
{
    $(".DeletePublication").click(UpdateDeletePublicationId);
    $("#btnAcceptDeletePublication").click(DeletePublication);

    $(".SelectLibrary").click(UpdateSelectedLibraryId);

    $("#btnAddEditorial").click(AddEditorial);
    $("#btnAddAuthor").click(AddAuthor);
    $("#btnAddTopic").click(AddTopic);

    $(".deleteEntry").click(DeleteEntry);

    $("#btnAuthorSearch").click(AuthorSearch);
    $("#btnEditorialSearch").click(EditorialSearch);
    $("#btnTopicSearch").click(TopicSearch);
   
    $("#imgInp").change(function () {
        readURL(this);
    });

    $("#PRS_btnPrev").click(PRS_Prev);
    $("#PRS_btnNext").click(PRS_Next);
    $("#PRS_pageIndexChange").change(PRS_PageIndexChange);
    $("#PRS_btnFirst").click(PRS_First);
    $("#PRS_btnLast").click(PRS_Last);

    $("#btnAuthorSearchResut").click(function () { $("#PRS_pageIndex").val(1); });

    OnClickAuthorSearch();

});

// Esta funcion ayuda a redireccionar las cunciones de la paginacion del modal
// buscar autor segun el boton que se seleccione: Ultimo, <<, >>, Primero

function OnClickAuthorSearch() {
    $("#PRS_btnPrev").on("click", function () { PAT_Prev(); })
    $("#PRS_btnNext").on("click", function () { PAT_Next(); });
    $("#PRS_pageIndexChange").on("change", function () { PAT_PageIndexChange(); });
    $("#PRS_btnFirst").on("click", function () { PAT_First(); });
    $("#PRS_btnLast").on("click", function () { PAT_Last(); });
}

// Esta funcion ayuda a capturar el id de una publicacion seleccionada

function UpdateSelectedLibraryId() {
    selectLibraryId = $(this).data("id");
}

// Esta funcion ayuda a capturar el id de una publicacion seleccionada

function UpdateDeletePublicationId() {
    deletePublicationId = $(this).data("id");
}

// Esta funcion ayuda a eliminar una publicacion con el id anterior capturado

function DeletePublication() {

    $.ajax({
        url: '/Publication/Delete',
        type: 'POST',
        data: JSON.stringify({ id: deletePublicationId }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            window.location.href = data.Url;
        },
        error: function () {
            alert("error");
        }
    });

}

// Estas funciones ayudan jalar los datos seleccionados en los modals

function AddEditorial() {
    $("#editorialTable input[type='radio']:checked").each(function () {
        var x = $(this).closest("tr");
        var id = x.find(":nth-child(1)").text();
        var name = x.find(":nth-child(2)").text();
        $("#Id_Editorial").val(id);
        $("#nameEditorial").val(name.toString());
    });
}

function AddAuthor() {
    $("#authorTable input[type='radio']:checked").each(function () {
        var x = $(this).closest("tr");
        var id = x.find(":nth-child(1)").text();
        var name = x.find(":nth-child(2)").text();
        $("#Id_Author").val(id);
        $("#nameAuthor").val(name.toString());
    });
}

// Estas funciones ayudan jalar y quitar los datos seleccionados del modals de Temas

function AddTopic() {

    $("#topicTable input[type='radio']:checked").each(function () {
        var x = $(this).closest("tr");
        var id = x.find(":nth-child(1)").text();
        var name = x.find(":nth-child(2)").text();
        var description = x.find(":nth-child(3)").text();

        console.log(name);
        var html =
            '<tr>' +
                '<td>' + id + '<input type="text" name="topicIds" value=' + id + ' hidden>' + '</td>' +
                '<td>' + name + '<input type="text" name="topicNames" value="' + name + '" hidden>' + '</td>' +
                '<td>' + description + '<input type="text" name="topicDescriptions" value="' + description + '" hidden>' + '</td>' +
                '<td class= "text-center"><a class="deleteEntry"><i class="margin-right-icon glyphicon glyphicon-remove"></i></a></td>' +
            '</tr>';

        var alreadyAdded = false;
        $("#AddedTopicTable tr").each(function ()
        {
            if ($(this).find(":nth-child(1)").text() == id) {
                alreadyAdded = true;
                return;
            }
        });

        if (alreadyAdded == false)
        {
            $("#AddedTopicTable").append(html);
            $("#AddedTopicTable").off('click', '.deleteEntry', DeleteEntry);
            $("#AddedTopicTable").on('click', '.deleteEntry', DeleteEntry);
            $("#AddedTopicTable").on('click', '.deleteEntry', DeleteEntry);
        }
    });
}

// Esta funcion ayuda a eliminar el tema asociado

function DeleteEntry() {
    var x = $(this).closest("tr");
    var id = x.find(":nth-child(1)").text();
    $(this).closest("tr").remove();
}


// Esta funcion ayuda a realizar el preview de la portada de una 
// publicacion y verificar que realimente se una imagen

function readURL(input) {

    var fileName = input.value

    if (fileName == "")
    {
        // cuando no carga nada
        $('#imgInp').val("");
        $('#changeFront').attr('src', "/images/book.gif");
        return false;
    }
    else if (fileName.split(".")[1].toUpperCase() == "PNG" || fileName.split(".")[1].toUpperCase() == "JPG"
            || fileName.split(".")[1].toUpperCase() == "GIF")
    {
        // cuando si carga una imagen
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#changeFront').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
        file = fileName;
        return true;
    }        
    else
    {
        // cuando lo cargado no es una imagen
        $('#imgInp').val("");
        $('#changeFront').attr('src', "/images/book.gif");
        alert("El archivo " + fileName.split(".")[1] + " es tiene una extensión inválida. Solo se puede subir imagenes png, jpg y gif.");
       
        return false;
    }
    return true;
}

// Estas funciones ayudan a realizar la paginacion de las pantallas de búsqueda
// Los botones: Primero, <<, >>, Ultimo

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



// Estas funciones ayudan a realizar la busqueda de los modals 

function AuthorSearch(event) {
    event.preventDefault();

    var data = $("#authorForm").serialize();
    $.post("/Publication/AuthorSearch", data, function (datos) {
        console.log(data);
        $("#resultAuthor").html(datos);
        OnClickAuthorSearch();
    });
}

function EditorialSearch(event) {
    event.preventDefault();

    var data = $("#editorialForm").serialize();
    $.post("/Publication/EditorialSearch", data, function (datos) {
        console.log(data);
        $("#resultEditorial").html(datos);
    });
}

function TopicSearch(event) {
    event.preventDefault();

    var data = $("#topicForm").serialize();
    $.post("/Publication/TopicSearch", data, function (datos) {
        console.log(data);
        $("#resultTopic").html(datos);
    });
}

// Estas funciones ayudan a realizar la paginacion de los modals de búsqueda


function PAT_Prev() {

    event.preventDefault();

    var index = parseInt($("#PRS_pageIndex").val());
    $("#PRS_pageIndex").val(index - 1);
    event.preventDefault();
    var data = $("#authorForm").serialize();
    $.post("/Publication/AuthorSearch", data, function (datos) {
        console.log(data);
        $("#resultAuthor").html(datos);
        OnClickAuthorSearch();
    });
}

function PAT_Next() {

    var index = parseInt($("#PRS_pageIndex").val());
    $("#PAT_pageIndex").val(index + 1);
    event.preventDefault();
    var data = $("#authorForm").serialize();
    $.post("/Publication/AuthorSearch", data, function (datos) {
        console.log(data);
        $("#resultAuthor").html(datos);
        OnClickAuthorSearch();
    });
}

function PAT_PageIndexChange() {
    event.preventDefault();
    var index = parseInt($("#PRS_pageIndexChange").val());
    $("#PRS_pageIndex").val(index);
    var data = $("#authorForm").serialize();
    $.post("/Publication/AuthorSearch", data, function (datos) {
        console.log(data);
        $("#resultAuthor").html(datos);
        OnClickAuthorSearch();
    });
}

function PAT_First() {
    event.preventDefault();
    var index = 1;
    $("#PRS_pageIndex").val(index);
    var data = $("#authorForm").serialize();
    $.post("/Publication/AuthorSearch", data, function (datos) {
        console.log(data);
        $("#resultAuthor").html(datos);
        OnClickAuthorSearch();
    });
}

function PAT_Last() {
    event.preventDefault();
    var index = 100000;
    $("#PRS_pageIndex").val(index);
    var data = $("#authorForm").serialize();
    $.post("/Publication/AuthorSearch", data, function (datos) {
        console.log(data);
        $("#resultAuthor").html(datos);
        OnClickAuthorSearch();
    });
}