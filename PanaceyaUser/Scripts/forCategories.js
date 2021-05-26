$('.DropCategories').change(function () {
    //Use $option (with the "$") to see that the variable is a jQuery object
    let option = $(this).find('option:selected');
    //Added with the EDIT
    var value = option.val();//to get content of "value" attrib
  
    let object = {
        id: value
    };
    let obj = JSON.stringify(object)
    $.ajax({
        type: 'POST',
        url: '/Medicines/ElementesMedicines',
        contentType: 'application/json; charset=utf-8',
        data: obj,
        success: function (data) {
            $('#ElementsMed').html(data);
        },
        error: function (data) {
            alert(data.responseText);
        }
    });
});

//$(window).on("load", function () {

//    let object = {
//        id: 1
//    };
//    let obj = JSON.stringify(object)
//    $.ajax({
//        type: 'POST',
//        url: '/Medicines/ElementesMedicines',
//        contentType: 'application/json; charset=utf-8',
//        data: obj,
//        success: function (data) {
//            $('#ElementsMed').html(data);
//        },
//        error: function (data) {
//            alert(data.responseText);
//        }
//    });

//});


