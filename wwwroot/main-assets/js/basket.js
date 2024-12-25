$(function () {
    $('.addToBasket').on("click", function () {
        $.ajax({
            method: "POST",
            url: "/basket/addproduct",
            data: {
                productId: $(this).data('id')
            },
            success: function (response) {
                alert(response)
            },
            error: function (xhr) {
                alert(xhr.responseText)
            }
        })
    })

    $('.increaseCount').on("click", function () {

        var btn = $(this);
        var basketProductId = $(this).data('id');

        $.ajax({
            method: "POST",
            url: "/basket/increasecount",
            data: {
                basketProductId: $(this).data('id')
            },
            success: function (response) {
                $(btn).siblings('#quantity').val(response.count)
                $(`#totalAmountForProduct-${basketProductId}`).text(`$${response.totalAmountForProduct}`)
                $('#totalAmount').children().text(`$${response.totalAmount}`)
            },
            error: function (xhr) {
                alert(xhr.responseText)
            }
        })
    })

    $('.decreaseCount').on("click", function () {
        var btn = $(this);
        var basketProductId = $(this).data('id')

        $.ajax({
            method: "POST",
            url: "/basket/decreasecount",
            data: {
                basketProductId: $(this).data('id')
            },
            success: function (response) {
                $(btn).siblings('#quantity').val(response.count)
                $(`#totalAmountForProduct-${basketProductId}`).text(`$${response.totalAmountForProduct}`)
                $('#totalAmount').children().text(`$${response.totalAmount}`)
            },
            error: function (xhr) {
                alert(xhr.responseText)
            }
        })
    })

    $('.deleteProduct').on("click", function () {
        var btn = $(this)
        var basketProductId = $(this).data('id')

        $.ajax({
            method: "POST",
            url: "/basket/delete",
            data: {
                basketProductId: $(this).data('id')
            },
            success: function (response) {
                $(btn).parent().parent().remove()
                $(`#totalAmountForProduct-${basketProductId}`).parent().remove()
                $('#totalAmount').children().text(`$${response.totalAmount}`)
            },
            error: function (xhr) {
                alert(xhr.responseText)
            }
        })
    })
})