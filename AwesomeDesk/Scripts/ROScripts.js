
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
//komunikaty z errorami
$(".alert").delay(4000).slideUp(200, function () {
    $(this).alert('close');
});


$(document).ready(function () {

    // Setup - add a text input to each footer cell

    $('#example tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" class="form-control text-box " />');
    });
    // DataTable
    var table = $('#example').DataTable({
        "language": {
            "sProcessing": "Przetwarzanie...",
            "sLengthMenu": "Pokaż _MENU_ pozycji",
            "sZeroRecords": "Nie znaleziono pasujących pozycji",
            "sInfoThousands": " ",
            "sInfo": "Pozycje od _START_ do _END_ z _TOTAL_ łącznie",
            "sInfoEmpty": "Pozycji 0 z 0 dostępnych",
            "sInfoFiltered": "(filtrowanie spośród _MAX_ dostępnych pozycji)",
            "sInfoPostFix": "",
            "sSearch": "Szukaj:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Pierwsza",
                "sPrevious": "Poprzednia",
                "sNext": "Następna",
                "sLast": "Ostatnia"
            }
         
        },       
        "order": [[0, "desc"]]
     
    

        
       
    });

    // Apply the search 
    //bez entera
    table.columns().every(function () {
        var that = this;

        $('input', this.footer()).on('keyup change clear', function () {
            if (that.search() !== this.value) {
                that
                    .search(this.value)
                    .draw();
            }
        });
    });
    $('#example tfoot tr').appendTo('#example thead');
});


