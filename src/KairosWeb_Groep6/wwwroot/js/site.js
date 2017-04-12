var siteView = {
    init: function () {
        // Button "Annuleer" bij elk formulier onder een tabel
        // verbergt het formulier
        $("#closeForm").click(function (event) {
            siteView.verbergFormulier();
        });

        // Button "+" onder elke tabel toont het formulier
        $("#openForm").click(function (event) {
            siteView.toonFormulier();
        });

        // verberg de intro bij het klikken op de x
        $("#verbergIntro").click(function (event) {
            siteView.verbergIntro();
        });

        // toon de intro opnieuw
        $("#toonIntro").click(function (event) {
            siteView.toonIntro();
        });

        $("#logoToonIntro").click(function (event) {
            siteView.toonIntro();
        });

        siteView.controleerOfIntroGetoondMoetWorden();
    },
    verbergFormulier: function() {
        $("#form").hide(500);
    },

    toonFormulier: function () {
        $("#form").show(500);
    },
    controleerOfIntroGetoondMoetWorden: function () {
        if (localStorage != null) {
            var value = localStorage.getItem("intro");

            if (value !== "verborgen") {
                siteView.toonIntro();
            }
        }
    },
    verbergIntro: function () {
        $(".intro").hide(500);

        if (localStorage != null) {
            localStorage.setItem("intro", "verborgen");
        }
    },

    toonIntro: function () {
        $(".intro").show(500, function() {
            // de hoogte van het loginform gelijk zetten aan de hoogte van de introtekst
            var width = $(document).width();
            if (width >= 768) {
                var introHeight = $("#intro").height();
                $("#login").height(introHeight);
            }
        });

        if (localStorage != null) {
            localStorage.setItem("intro", "");
        }
    },
    initFunctionsKostenEnBaten: function() {
        $("#add").click(function (event) {
            event.preventDefault();
            $.get("/ExtraKosten/VoegToe",
                function (data) {
                    $("#divForm").html(data);
                    siteView.toonFormulier();
                });
        });

        $("#bewerk").click(function (event) {
            event.preventDefault();
            $.get($(this).attr("href"),
                function (data) {
                    $("#divForm").html(data);
                    siteView.toonFormulier();
                });
        });

        $("#verwijder").click(function (event) {
            event.preventDefault();
            $.get($(this).attr("href"),
                function (data) {
                    $("#data").html(data);
                });
        });
    }
};

$(document).ready(function() {
    siteView.init();
});