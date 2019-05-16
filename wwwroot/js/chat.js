$(function () {
    $("#messagebox").animate({ scrollTop: $('#messagebox').prop("scrollHeight") }, 750);

    function GoDown() {
        var d = $('#messagebox2');
        d.scrollTop(d.prop("scrollHeight"));
    }    
});