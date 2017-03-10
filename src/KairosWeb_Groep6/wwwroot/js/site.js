// Button "Annuleer" bij elk formulier onder een tabel
// verbergt het formulier
$("#closeForm").click(function(event) {
    verbergFormulier();
});

// Button "+" onder elke tabel toont het formulier
$("#openForm").click(function(event) {
    toonFormulier();
});

function verbergFormulier() {
    $("#form").hide(500);
};

function toonFormulier() {
    console.log("Geklikt!");
    $("#form").show(500);
};

// js om de buttons in de laatste kolom te tonen of verbergen
//$("tr").hover(function (event) { // function when mouse on row
//    console.log(this);
//    var options = $(this).getElementsByClassName("options")[0];
//    $(options).show(500);
//});