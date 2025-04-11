
document.getElementById("IsletmeID").addEventListener("change", function () {
    var selectedIsletmeId = this.value;
    var isyeriSelect = document.getElementById("IsYeriId");

    for (var i = 1; i < isyeriSelect.options.length; i++) {
        var option = isyeriSelect.options[i];
        var isletmeId = option.getAttribute("data-isletme");

        option.style.display = (isletmeId === selectedIsletmeId) ? "block" : "none";
    }

    isyeriSelect.value = "";
});