var werkgeverView = {
    init: function () {
        $("#formAdd").submit(function () {//#formAdd is het id van de form
            $.post(this.action, $(this).serialize(), function (data) {
                $("#WerkgeversTable").html(data); // is het id van de div voor de partial view te laden
                $("#NaamId").val(""); //is het id van de zoekterm
            });
            return false;  //formulier wordt niet gesubmit
        });

    }
};

$(function () {
    werkgeverView.init();
});