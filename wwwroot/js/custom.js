console.log("Custom script loaded!");

$(document).ready(function() {

    function setAppDateTime() {
        var now = new Date();
        var offset = now.getTimezoneOffset();
        now.setMinutes(now.getMinutes() - offset);
        var formattedDateTime = now.toISOString().slice(0, 16);
        $('.set-datetime').val(formattedDateTime);
    }
    
    setAppDateTime();

    $(".app-resched, .bttn-add").on("click", function(){
        setAppDateTime();
    });
});
