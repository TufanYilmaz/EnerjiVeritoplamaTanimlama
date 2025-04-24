

// Kullanıcı seçim yapınca alanları göster/gizle




document.addEventListener("DOMContentLoaded", function () {
   
 var opcNodesSelect = document.getElementById("OpcNodesSelect");
    var sayacSelect = document.getElementById("SayacSelect");
    var dataTypeIdSelect = document.getElementById("dataTypeId");
    // Form gönderilirken hangi ID seçildiyse onu aktar
    const form = document.querySelector("form");
    form.addEventListener("submit", function () {
        const selectedDataTypeId = dataTypeIdSelect.value;
        let selectedValue = "";

        if (selectedDataTypeId == 1) {
            selectedValue = document.getElementById("sayacID").value;
        } else if (selectedDataTypeId == 2) {
            selectedValue = document.getElementById("opcNodesID").value;
        }

        document.getElementById("sayacOpcNodesID").value = selectedValue;
    });
document.getElementById("dataTypeId").addEventListener("change", function () {
    var selected = this.value;

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



