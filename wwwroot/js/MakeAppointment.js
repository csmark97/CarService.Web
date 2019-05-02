//Input_AppointmentDay
$(function () {
    var selected;

    $(".calendar-button").click(function () {

        $('.btn').each(function () {
            $(this).removeClass('btn-warning');
        });
        if ($(this).not(selected)) {
            selected = $(this);
            $(this).addClass('btn-warning');
        }

        $("#Input_AppointmentDay").val($(this).prop('title'));
        $("#Input_AppointmentTime").val($(this).text());
    });

    //$("#Select_Car").change(function () {
    //    $('#Input_Car').text($(this).selected().val());
    //});
});