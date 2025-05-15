

// Kullanıcı seçim yapınca alanları göster/gizle




document.addEventListener("DOMContentLoaded", function () {

    document.getElementById("dataTypeId").addEventListener("change", function () {
        var selected = this.value;
        var opcNodesSelect = document.getElementById("OpcNodesSelect");
        var sayacSelect = document.getElementById("SayacSelect");

        if (selected == 1) {
            opcNodesSelect.style.display = "none";
            sayacSelect.style.display = "block";
        } else if (selected == 2) {
            opcNodesSelect.style.display = "block";
            sayacSelect.style.display = "none";
        } else {
            opcNodesSelect.style.display = "none";
            sayacSelect.style.display = "none";
        }
    });

});



