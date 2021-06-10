//$('.DropAmount').change(function () {
//    //Use $option (with the "$") to see that the variable is a jQuery object
//    let option = $(this).find('option:selected');
//    //Added with the EDIT
//    var value = option.text();//to get content of "value" attrib
//    let amount = parseInt(value);
//    let object = {
//        Amount: amount
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
function AddInBasket() {
    
    let option = $(".DropAmount").find('option:selected');
    var value = option.text();//to get content of "value" attrib
    let amount = parseInt(value);

    let price = parseFloat($("#price").text());
    
    let IDMedicines = parseInt($("#idMedicine").text());

    $.ajax({
        type: 'POST',
        url: '/Home/CheckAuthenticated',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            //if (data.check) { window.location.href = "" }
            if (data.check) {
                let object = {
                    idMedicines: IDMedicines,
                    Price: price,
                    Amount: amount
                };
                let obj = JSON.stringify(object)
                $.ajax({
                    type: 'POST',
                    url: '/Basket_Consist/Create',
                    contentType: 'application/json; charset=utf-8',
                    data: obj,
                    success: function (data) {
                        //if (data.check) { window.location.href = "" }
                        if (data.Presence) {
                            if (data.AcceptableAmount === 0) {
                                $('#ModalOfBasket').modal('show');
                            }
                            else {
                                $('#limit').text(data.AcceptableAmount) 
                                $('#ModalLimit').modal('show');
                            }
                        }
                        else {
                            $('#ModalOfError').modal('show');
                        }
                    },
                    error: function (data) {
                        alert(data.responseText);
                    }
                });
            }
            else {
                $('#exampleModalCenter').modal('show');
                $('.CompleteReg').after($("<p>", {
                    'class': " text-center RegComplete mt-4 mb-md-0",
                    'style': "font-size:18px; color: red",
                    'text': 'Пожалуйста, войдите!'
                }));
                setTimeout(function () {
                    $('.RegComplete').remove()
                }, 4000);
            }
        },
        error: function (data) {
            alert(data.responseText);
        }
    });
    
}


function DeleteProduct(item) {
    let id = item.id
    $("#DeleteProduct").modal("show")
    $(".sureDelete").attr("id", id)
    
}

function CancelDelete() {
    $(".sureDelete").removeAttr("id")
    $("#DeleteProduct").modal("hide")
}

function EndDelete(item) {
    let id = item.id
    let object = {
        ID: id
    };
    let obj = JSON.stringify(object)
    $.ajax({
        type: 'POST',
        url: '/Basket_Consist/DeleteConfirmed',
        contentType: 'application/json; charset=utf-8',
        data: obj,
        success: function (data) {
            $(".sureDelete").removeAttr("id")
            $("#DeleteProduct").modal("hide")
            location.reload();
        },
        error: function (data) {
            alert(data.responseText);
        }
    });
   
}