var werkgeverView = {
    init: function () {
        $("#formAdd").submit(function () {
            $.post(this.action, $(this).serialize(), function (data) {
                $("#WerkgeversTable").html(data);
                $("#NaamId").val("");
            });
            return false;  //formulier wordt niet gesubmit
        });

    }
};

$(function () {
    werkgeverView.init();
});