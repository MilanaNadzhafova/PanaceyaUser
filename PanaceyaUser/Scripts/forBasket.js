$('.DropAmount').change(function () {
    //Use $option (with the "$") to see that the variable is a jQuery object
    let option = $(this).find('option:selected');
    //Added with the EDIT
    var value = option.val();//to get content of "value" attrib
    let idMedicine = $(this).attr("id");
    let object = {
        idMed: idMedicine,
        amount: value
    };
    let obj = JSON.stringify(object)
    $.ajax({
        type: 'POST',
        url: '/Basket_Consist/ChangeElement',
        contentType: 'application/json; charset=utf-8',
        data: obj,
        success: function (data) {
            
            $(".DropAmount#" + idMedicine + "").next().next().text(data.newPrice)
            let PricesMassive = $(".ForPrice");
            let AllPrice = 0;
            for (let i = 0; i < PricesMassive.length; i++) {
                
                AllPrice += parseFloat($(PricesMassive[i]).text());
            }
            $("#ForAllPrice").text(AllPrice);

        },
        error: function (data) {
            alert(data.responseText);
        }
    });
});

function AddOrder() {
    if ($("#ForAllPrice").text()!=0) {
         let optionPoint = $("#DropIssuePoints").find('option:selected');
        var valuePoint = optionPoint.val();
        let optionMethod = $("#DropPayMethod").find('option:selected');
        var valueMethod = optionMethod.val();
        let object = {
            pay: valueMethod,
            point: valuePoint
        };
        let obj = JSON.stringify(object)
        $.ajax({
            type: 'POST',
            url: '/Basket_Consist/CreateOrder',
            contentType: 'application/json; charset=utf-8',
            data: obj,
            success: function (data) {
                alert("Успешно")
                location.reload();

            },
            error: function (data) {
                alert(data.responseText);
            }
        });
    }
   

}