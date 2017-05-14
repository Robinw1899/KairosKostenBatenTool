var siteView = {
    init: function() {
        // Button "Annuleer" bij elk formulier onder een tabel
        // verbergt het formulier
        $("#closeForm").click(function(event) {
            siteView.verbergFormulier();
        });

        // Button "+" onder elke tabel toont het formulier
        $("#openForm").click(function(event) {
            siteView.toonFormulier();
        });

        // verberg de intro bij het klikken op de x
        $("#verbergIntro").click(function(event) {
            siteView.verbergIntro();
        });

        // toon de intro opnieuw
        $("#toonIntro").click(function(event) {
            siteView.toonIntro();
        });

        $("#logoToonIntro").click(function(event) {
            siteView.toonIntro();
        });

        siteView.controleerOfIntroGetoondMoetWorden();
    },
    verbergFormulier: function() {
        $("#form").hide(500);
    },

    toonFormulier: function() {
        $("#form").show(500);
    },
    controleerOfIntroGetoondMoetWorden: function() {
        if (localStorage != null) {
            var value = localStorage.getItem("intro");

            if (value !== "verborgen") {
                siteView.toonIntro();
            }
        }
    },
    verbergIntro: function() {
        $(".intro").hide(500);

        if (localStorage != null) {
            localStorage.setItem("intro", "verborgen");
        }
    },
    toonIntro: function() {
        $(".intro").show(500,
            function() {
                // de hoogte van het loginform gelijk zetten aan de hoogte van de introtekst
                var width = $(document).width();
                if (width >= 768) {
                    var introHeight = $("#intro").height() + 60;
                    var loginHeight = $("#login").height() + 60; // + 60 voor de padding

                    if (loginHeight < introHeight) {
                        $("#login").css("min-height", introHeight);
                    } else {
                        $("#intro").css("min-height", loginHeight);
                    }
                }
            });

        if (localStorage != null) {
            localStorage.setItem("intro", "");
        }
    },
    initFunctionsKostenEnBaten: function () {
        $("a#add").click(function(event) {
            event.preventDefault();
            siteView.toonFormulier();
        });

        $("a#bewerk").click(function(event) {
            event.preventDefault();
            $.get($(this).attr("href"),
                function(data) {
                    $("#divForm").html(data);
                    siteView.toonFormulier();
                    siteView.initFunctionsKostenEnBaten();
                });
        });

        $("a#verwijder").click(function(event) {
            event.preventDefault();
            var url = $(this).attr("href");
            $.get(url,
                function(data) {
                    $("#data").html(data);
                    siteView.initFunctionsKostenEnBaten();
                });
        });

        $("#form").submit(function (event) {
            $.post(this.action,
                $(this).serialize(),
                function (data) {
                    siteView.plaatsDataKostenBatenInHtml(data);
                    siteView.initFunctionsKostenEnBaten();
                });
            return false;
        });
    },
    resetFormulierKostenBaten: function () {
        var url = $("a#add").attr("href");
        $.get(url, function(data) {
            $("#divForm").html(data);
        });
    },
    plaatsDataKostenBatenInHtml: function(data) {
        if (data.includes("form")) {
            $("#divForm").html(data);
            siteView.toonFormulier();
            siteView.initFunctionsKostenEnBaten();
        }

        if (data.includes("table")) {
            var url = $("#add").attr("href");
            $.get(url,
                function (formulier) {
                    $("#data").html(data);
                    $("#divForm").html(formulier);
                    siteView.verbergFormulier();
                });
        }
    },
    initKlikbareRijen: function() {
        $(".klikbareRij").click(function(event) {
            var href = $(this).data("href");

            if (href) {
                window.location.href = href;
            }
        });
    },
    toonMeer: function() {
        $("#toonmeer").click(function(event) {
            event.preventDefault();
            var url = $(this).attr("href");
            $.get(url,
                function(data) {
                    $("#data").html(data);
                });
        });
    },
    laadAnalyses: function () {
        var url = $("#analyses").data("href");

        $.get(url, function (data) {
            $("#loader").slideUp(500);
            $("#analyses").html(data);        
        });
    },
    laadActionsVorigeVolgende: function() {
        // actions instellen
        $("a#vorige").click(function (event) {
            event.preventDefault();
            $("#analyses").html("");
            $("#loader").slideDown(300);

            $.get($(this).attr("href"),
                function (data) {
                    $("#loader").slideUp(500);
                    $("#analyses").html(data);
                });
        });

        $("a#volgende").click(function (event) {
            event.preventDefault();
            $("#analyses").html("");
            $("#loader").slideDown(500);

            $.get($(this).attr("href"),
                function (data) {
                    $("#loader").slideUp(500);
                    $("#analyses").html(data);
                });
        });
    }
};

$(document).ready(function() {
    siteView.init();
});