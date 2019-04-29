$(function () {
    if ($("#Input_UserType").val() === "CLIENT") {
        $(".address").show();
        $(".privatekey").hide();
        $(".opening").hide();
        $(".opening-day").prop('disabled', true);
    }
    if ($("#Input_UserType").val() === "WORKER") {
        $(".address").hide();
        $(".privatekey").show();
        $(".opening").hide();
        $(".opening-day").prop('disabled', true);
    }
    if ($("#Input_UserType").val() === "COMPANY") {
        $(".address").show();
        $(".privatekey").show();
        $(".opening").show();
        $(".opening").prop('disabled', false);
    }
    $("#Input_UserType").on('change', function () {
        if ($("#Input_UserType").val() === "CLIENT") {
            $(".address").show();
            $(".privatekey").hide();
            $(".opening").hide();
            $(".opening-day").prop('disabled', true);
        }
        if ($("#Input_UserType").val() === "WORKER") {
            $(".address").hide();
            $(".privatekey").show();
            $(".opening").hide();
            $(".opening-day").prop('disabled', true);
        }
        if ($("#Input_UserType").val() === "COMPANY") {
            $(".address").show();
            $(".privatekey").show();
            $(".opening").show();
            $(".opening-day").prop('disabled', false);
        }
    });    

    if ($('#Input_SaturdayOpen').prop('checked') === false) {
        $('#Input_StartSaturday').attr('disabled', 'true');
        $('#Input_EndSaturday').attr('disabled', 'true');
    }

    if ($('#Input_SaturdayOpen').prop('checked')) {
        $('#Input_StartSaturday').removeAttr('disabled');
        $('#Input_EndSaturday').removeAttr('disabled');
    }

    $('#Input_SaturdayOpen').change(function () {
        if ($(this).prop('checked') === false) {
            $('#Input_StartSaturday').attr('disabled', 'true').trigger('change');
            $('#Input_EndSaturday').attr('disabled', 'true').trigger('change');
        }

        if ($(this).prop('checked')) {
            $('#Input_StartSaturday').removeAttr('disabled').trigger('change');
            $('#Input_EndSaturday').removeAttr('disabled').trigger('change');
        }
    });

    if ($('#Input_SundayOpen').prop('checked') === false) {
        $('#Input_StartSunday').attr('disabled', 'true').trigger('change');
        $('#Input_EndSunday').attr('disabled', 'true').trigger('change');
    }

    if ($('#Input_SundayOpen').prop('checked')) {
        $('#Input_StartSunday').removeAttr('disabled').trigger('change');
        $('#Input_EndSunday').removeAttr('disabled').trigger('change');
    }

    $("#Input_SundayOpen").on('checked', function () {
        if ($(this).prop('checked') === false) {
            $('#Input_StartSunday').attr('disabled', 'true').trigger('change');
            $('#Input_EndSunday').attr('disabled', 'true').trigger('change');
        }

        if ($(this).prop('checked')) {
            $('#Input_StartSunday').removeAttr('disabled').trigger('change');
            $('#Input_EndSunday').removeAttr('disabled').trigger('change');
        }
    });
});
