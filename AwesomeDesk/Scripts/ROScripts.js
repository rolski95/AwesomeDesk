
$(function () {
    $(".ticketContent").hide();

});
function showhide(id) {
    // document.getElementById(id).style.display = 'none';
    if ($('#' + id).is(":visible")) {
        $('#' + id).slideUp(200);
    }
    else {
        $('#' + id).slideDown(200);
    }
};
function showhideall() {
    var $vis = 0;
    var $nonvis = 0;
    $('.ticketContent').each(function () {

        if ($(this).is(":visible")) {
            $vis++;
        }
        else {
            $nonvis++;
        }
    })
    $('.ticketContent').each(function () {
        if ($vis > $nonvis) {
            $(this).slideUp(200);
        }
        else {
            $(this).slideDown(200);
        }
    })
};
//komunikaty z errorami
$(".alert").delay(4000).slideUp(200, function () {
    $(this).alert('close');
});
//czas pracy
$(document).ready(function () {
    $("#timeStart").datetimepicker(
        {
            showTodayButton: true,
            format: 'DD-MM-YYYY HH:mm',
            showClose: true,               
            stepping: 1
        });
    $("#timeEnd").datetimepicker({
        showTodayButton: true,
        format: 'DD-MM-YYYY HH:mm',        
        toolbarPlacement: 'top',
        stepping: 1
        
    });
});


function setspendMinutes() {

    



    var timestart = moment( $("#timeStart").datetimepicker('date'));
    
    var timeend = moment( $("#timeEnd").datetimepicker('date'));

    timestart.set({ s: 0, ms: 0 });
    timeend.set({ s: 0, ms: 0 });
    var tmp = timeend.diff(timestart, 'minutes');    
    var min = tmp % 60;
    var h = Math.floor(tmp / 60);


    document.getElementById("timeMinutes").value = (min<0)?0:min;
    document.getElementById("timeHours").value = (h < 0) ? 0: h;

};
function diff_minutes(dt2, dt1) {

    var diff = (dt2.getTime() - dt1.getTime()) / 1000;
    diff /= 60;
    return Math.abs(Math.round(diff));

};
$(document).ready(function () {

    // Setup - add a text input to each footer cell

    $('.ro-datatable tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" class="form-control text-box " />');
    });
    // DataTable
    var table = $('.ro-datatable').DataTable({
        "oLanguage": {
            "sProcessing": "Przetwarzanie...",
            "sLengthMenu": 'Pokaż <select class="custom-select">' +

                '<option value="50" selected="selected" >50</option>' +
                '<option value="100">200</option>' +
                '<option  value="200">200</option>' +
                '<option value="300">300</option>' +
                '<option value="500">500</option>' +
                '<option value="-1">All</option>' +
                '</select> pozycji ',
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
        "order": [[0, "desc"]],
        "dom": '<"top">rt<"bottom"flpi><"clear">'
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
    $('.ro-datatable tfoot tr').appendTo('.ro-datatable thead');
});


$(function () {
    var $tabButtonItem = $('#tab-button li'),
        $tabSelect = $('#tab-select'),
        $tabContents = $('.tab-contents'),
        activeClass = 'is-active';

    $tabButtonItem.first().addClass(activeClass);
    $tabContents.not(':first').hide();

    $tabButtonItem.find('a').on('click', function (e) {
        var target = $(this).attr('href');

        $tabButtonItem.removeClass(activeClass);
        $(this).parent().addClass(activeClass);
        $tabSelect.val(target);
        $tabContents.hide();
        $(target).show();
        e.preventDefault();
    });

    $tabSelect.on('change', function () {
        var target = $(this).val(),
            targetSelectNum = $(this).prop('selectedIndex');

        $tabButtonItem.removeClass(activeClass);
        $tabButtonItem.eq(targetSelectNum).addClass(activeClass);
        $tabContents.hide();
        $(target).show();
    });
});