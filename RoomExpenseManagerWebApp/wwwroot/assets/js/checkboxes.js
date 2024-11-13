$(".dropdown dt a").on('click', function () {
    $(".dropdown dd ul").slideDown('fast');
});

$(".dropdown dd ul li a").on('click', function () {
    $(".dropdown dd ul").hide();
});

function getSelectedValue(id) {
    return $("#" + id).find("dt a span.value").html();
}

$(document).bind('click', function (e) {
    var $clicked = $(e.target);
    if (!$clicked.parents().hasClass("dropdown")) $(".dropdown dd ul").hide();
});


//$('.mutliSelect ul').on('click', function () {

//    var title = $(this).closest('.mutliSelect').find('input[type="checkbox"]').val(),
//        title = title + ",";

//   /* if ($(this).is('checked')) {*/
//        var html = title
//        $('.multiSel').val(html);
//        $(".hida").hide();
//    //}
//    //else {
//    //    $('span[title="' + title + '"]').remove();
//    //    var ret = $(".hida");
//    //    $('.dropdown dt a').append(ret);

//    //}
//});