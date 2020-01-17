
$(function () {
    $(".ticketContent").hide();

});
function showhide(id) {
    // document.getElementById(id).style.display = 'none';
    if ($('#' + id).is(":visible")) {
        $('#' + id).slideUp(500);
    }
    else {
        $('#' + id).slideDown(500);
    }    
};

$(".alert").delay(4000).slideUp(200, function () {
    $(this).alert('close');
});