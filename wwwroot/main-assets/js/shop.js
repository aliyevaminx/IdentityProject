
$(document).ready(function () {

    var ul = $("#categories");
    var buttons = ul.find("li");
    buttons.each(function (index,button) {
        $(button).click(function () {

            var categoryId = $(button).find("a").attr("categoryId");
        
            $.ajax({
                url: "/Shop/ShowProducts",
                method: "GET",
                data: {
                    categoryId: categoryId
                },
                contentType: "application/json",
                success: function (response) {

                    $(".products").html(response)
                }
            })
        });
    });
})