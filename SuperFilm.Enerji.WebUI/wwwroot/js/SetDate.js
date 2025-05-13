document.getElementById("TimeTypeId").addEventListener("change", function () {
    var selected = this.value;
    var gunInput = document.getElementById("Gun");
    var ayInput = document.getElementById("Ay");

    if (selected == "1") { // Günlük
        gunInput.style.display = "block";
        ayInput.style.display = "none";
    } else if (selected == "2") { // Aylık
        gunInput.style.display = "none";
        ayInput.style.display = "block";
    } else {
        gunInput.style.display = "none";
        ayInput.style.display = "none";
    }
});

// Grafiği kontrol etme ve düzenleme fonksiyonu
function checkAndFixChart() {
    setTimeout(function() {
        var chartContainer = document.getElementById("chartContainer");
        if (chartContainer) {
            // Syncfusion grafik öğeleri var mı kontrol et
            var svgElements = chartContainer.querySelectorAll("svg");
            
            if (svgElements.length === 0 || svgElements[0].clientHeight < 10) {
                console.log("Grafik yüklenemedi, yeniden oluşturuluyor...");
                
                // Grafik container'ı temizle
                chartContainer.innerHTML = "";
                
                // Eğer chartData varsa, grafik sayfada yüklensin
                if (typeof chartData !== 'undefined' && chartData.length > 0) {
                    // Syncfusion nesnesi mevcut mu kontrol et
                    if (typeof ej !== 'undefined' && ej.charts) {
                        console.log("Syncfusion nesnesi mevcut, grafik yeniden oluşturuluyor");
                        
                        var chart = new ej.charts.Chart({
                            primaryXAxis: {
                                valueType: 'Category',
                                title: 'Zaman'
                            },
                            primaryYAxis: {
                                title: 'Değer'
                            },
                            series: [{
                                type: 'Line',
                                dataSource: chartData,
                                xName: 'Zaman',
                                yName: 'Deger',
                                name: 'Sayaç Veri',
                                width: 2,
                                marker: {
                                    visible: true,
                                    width: 7,
                                    height: 7
                                }
                            }],
                            title: 'Sayaç Verileri'
                        });
                        
                        chart.appendTo(chartContainer);
                    }
                }
            } else {
                console.log("Grafik başarıyla yüklendi!");
            }
        }
    }, 1000);
}

// Sayfa yüklendiğinde grafiği kontrol et
document.addEventListener("DOMContentLoaded", function() {
    checkAndFixChart();
});

