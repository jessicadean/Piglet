var App = (function () {
  'use strict';

  App.formElements = function( ){ 

    //Select2
    $(".select2").select2({
      width: '100%'
    });
    
    //Select2 tags
    $(".tags").select2({tags: true, width: '100%'});

    //Bootstrap Slider
    $('.bslider').bootstrapSlider();

    $("#milestone_slider").slider().on("slide",function(e){
      var v1 = e.value[0];
      var v2 = e.value[1];

      $("#slider-value").html( v1 + "%" + " - " + v2 + "%");

    });

    //Date Picker
    $('.datepicker').datepicker({
      autoclose: true
    });
  };

  return App;
})(App || {});
