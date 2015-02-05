

$(function () {
    
  $(".add_row").on("click",function () {
  	template = document.getElementById(this.dataset.tpl).innerHTML;
  	$(this).before(template);
  })
  
/*
  $("*").on("click",".remove_row", function (){
  	$(this).closest("tr").remove();
  })*/
    /*
  $("*").on("click",".close", function (){
    $(this).closest(".row").slideUp(function () {$(this).remove();});
  })*/

    /*
  $("*").on("change",".conocimiento_select",function(){
  	var optlabel = $(this.selectedOptions).parent().attr("label");
  	$(this).closest("tr").find("input").val(optlabel);
  })*/

  /*$(".nav.nav-second-level.collapse.in li").hover(function () {
      console.log("dasdsa");
  }, function () {
      $(this).closest("ul").removeClass("in");
  });*/

    $('.deleteElementClick').click(function (e) {
        e.preventDefault();
        var id = $(this).data('id');

        $('.deleteElementInput').val(id);
    });

    $('.reserveCubicle').click(function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var hour = $(this).data('inihour');
        var canthour = $(this).data('canthour');
        var day = $(this).data('date');

        $('.deleteElementInput').val(id);
        $('.hourInput').val(hour);
        $('.cantHourInput').val(canthour);
        $('.dayInput').val(day);
    });


    $("#selected-estado").change(function () {
        var elemento = $(this);

        if (elemento.val() == "Sin Ubicar") {
            $("#label-selected").attr("readonly", "readonly");

        }
        else {
            $("#label-selected").removeAttr("readonly");

        }
    });

    $('.generic-datepicker').datepicker({
        language: "es",
        todayHighlight: true,
        autoclose: true,
        format: "dd/mm/yyyy"
    });
})
