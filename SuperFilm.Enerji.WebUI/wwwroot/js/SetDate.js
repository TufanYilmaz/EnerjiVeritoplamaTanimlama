
document.getElementById("TimeTypeId").addEventListener("change", function () {
    var selected = this.value;
    var gunInput = document.getElementById("Gun");
    var ayInput = document.getElementById("Ay");

    if (selected == 1) { // Günlük
        gunInput.style.display = "block";
        ayInput.style.display = "none";

    } else if (selected == 2) { // Aylık
        gunInput.style.display = "none";
        ayInput.style.display = "block";
    } else {
        gunInput.style.display = "none";
        ayInput.style.display = "none";
    }

});

