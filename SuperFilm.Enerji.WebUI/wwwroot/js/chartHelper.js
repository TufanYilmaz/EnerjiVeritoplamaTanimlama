/**
 * Sayaç Veri Grafiği Yardımcı Fonksiyonları
 */

// Grafiği yeniden oluşturma fonksiyonu
function renderChart(chartData, timeTypeId, chartTitle, minValue, maxValue, interval) {
    if (!chartData || chartData.length === 0) {
        console.error("Grafik verisi bulunamadı!");
        return;
    }

    var chartContainer = document.getElementById("chartContainer");
    if (!chartContainer) {
        console.error("Grafik konteyneri bulunamadı!");
        return;
    }

    // Grafik verilerini konsola yazdır (hata ayıklama için)
    console.log("Grafik verileri:", chartData);

    // Eğer min-ma    
    // Eğer min-max-interval parametreleri verilmemişse, hesapla
    if (minValue === undefined || maxValue === undefined || interval === undefined) {
        // Verilerin min ve max değerlerini bul
        var values = chartData.map(function(item) { return item.Deger; });
        var calculatedMin = Math.min.apply(Math, values);
        var calculatedMax = Math.max.apply(Math, values);
        
        // Uygun aralık hesapla
        var range = calculatedMax - calculatedMin;
        
        if (range < 1) {
            // Eğer aralık çok küçükse
            minValue = calculatedMin * 0.9;
            maxValue = calculatedMax * 1.1;
        } else {
            // Aralığı genişlet
            minValue = calculatedMin - (range * 0.1);
            maxValue = calculatedMax + (range * 0.1);
        }
        
        // Aralık adımını hesapla
        interval = calculateYAxisInterval(minValue, maxValue);
    }

    try {
        // Syncfusion chart kütüphanesi yüklü mü kontrol et
        if (typeof ej !== 'undefined' && ej.charts && ej.charts.Chart) {
            // Var olan grafik örneğini temizle
            chartContainer.innerHTML = "";

            // Yeni chart örneği oluştur
            var chart = new ej.charts.Chart({
                primaryXAxis: {
                    valueType: 'Category',
                    title: timeTypeId == 1 ? 'Saat' : 'Gün',
                    labelIntersectAction: 'Rotate45',
                    majorGridLines: { width: 0 }
                },
                primaryYAxis: {
                    title: 'Değer',
                    majorTickLines: { width: 0 },
                    minorTickLines: { width: 0 },
                    minimum: minValue,
                    maximum: maxValue,
                    interval: interval,
                    labelFormat: '{value}'
                },
                tooltip: { 
                    enable: true,
                    format: '${point.x}: <b>${point.y}</b>',
                    textStyle: {
                        fontWeight: 'bold',
                        color: '#333333',
                        size: '14px'
                    },
                    fill: '#ffffff',
                    opacity: 0.9,
                    border: { color: '#009688', width: 1 }
                },
                series: [{
                    type: 'Line',
                    dataSource: chartData,
                    xName: 'Zaman',
                    yName: 'Deger',
                    name: 'Sayaç Değerleri',
                    width: 2,
                    marker: {
                        visible: true,
                        width: 8,
                        height: 8,
                        shape: 'Circle',
                        isFilled: true
                    },
                    animation: { enable: true },
                    tooltipMappingName: 'Deger'
                }],
                title: chartTitle || 'Sayaç Verileri',
                legendSettings: { visible: true },
                width: '100%',
                height: '400px',
                chartArea: { border: { width: 0 } },
                load: function(args) {
                    args.chart.theme = "Material";
                }
            });

            // Chart'ı konteynere ekle
            chart.appendTo(chartContainer);
            console.log("Grafik başarıyla oluşturuldu!");
        } else {
            console.error("Syncfusion chart kütüphanesi bulunamadı! CDN üzerinden yükleniyor...");
            
            // Syncfusion CDN'den yüklemeyi dene
            var script = document.createElement('script');
            script.src = 'https://cdn.syncfusion.com/ej2/dist/ej2.min.js';
            script.onload = function() {
                console.log("Syncfusion kütüphanesi yüklendi, grafik yeniden oluşturuluyor...");
                setTimeout(function() {
                    renderChart(chartData, timeTypeId, chartTitle, minValue, maxValue, interval);
                }, 500);
            };
            document.head.appendChild(script);
        }
    } catch (error) {
        console.error("Grafik oluşturma hatası:", error);
    }
}

// Y ekseni için uygun aralık hesapla
function calculateYAxisInterval(min, max) {
    // Y ekseni için uygun aralık hesapla
    var range = max - min;
    
    if (range <= 0)
        return 1;
        
    // Uygun basamak ve aralık belirle
    var exponent = Math.floor(Math.log10(range));
    var magnitude = Math.pow(10, exponent);
    
    // Genellikle 5-10 aralık olsun
    var normalized = range / magnitude;
    
    if (normalized < 2.5)
        return magnitude / 5;
    else if (normalized < 5)
        return magnitude / 2;
    else if (normalized < 10)
        return magnitude;
    else
        return magnitude * 2;
}

// Min-max değer ve interval hesaplamaları
function calculateChartParameters(data) {
    // Verilerin min ve max değerlerini bul
    var minValue = Number.MAX_VALUE;
    var maxValue = Number.MIN_VALUE;
    
    data.forEach(function(item) {
        var value = parseFloat(item.Deger);
        if (!isNaN(value)) {
            minValue = Math.min(minValue, value);
            maxValue = Math.max(maxValue, value);
        }
    });
    
    // Minimum değeri tamamen 0 yapmak yerine, 
    // değerler birbirine çok yakınsa veri aralığını genişlet
    if (maxValue - minValue < 1) {
        minValue = Math.max(0, minValue - 0.5);
        maxValue = maxValue + 0.5;
    } else if (minValue > 0 && minValue / maxValue > 0.95) {
        // Değerler birbirine yakın ve sıfırdan uzaksa, Y ekseni aralığını genişlet
        var range = maxValue - minValue;
        minValue = Math.max(0, minValue - range * 0.1);
        maxValue = maxValue + range * 0.1;
    } else {
        // Değerler arasında yeterli fark varsa, görsel açıdan iyileştir
        minValue = Math.max(0, minValue - (maxValue - minValue) * 0.05);
        maxValue = maxValue + (maxValue - minValue) * 0.05;
    }
    
    // Interval hesaplama: Değer aralığı ve ölçek için uygun aralık belirle
    var range = maxValue - minValue;
    var interval = calculateOptimalInterval(range);
    
    return {
        minValue: minValue,
        maxValue: maxValue,
        interval: interval
    };
}

// Optimal interval hesaplaması
function calculateOptimalInterval(range) {
    // Grafikte 5-8 arası çizgi olacak şekilde interval hesapla
    var targetGridLines = 6;
    
    if (range <= 0) return 1; // Hata durumu
    
    // Basamak sayısını belirle
    var magnitude = Math.pow(10, Math.floor(Math.log10(range)));
    var normalized = range / magnitude;
    
    var interval;
    if (normalized < 1.5) interval = magnitude / 5;
    else if (normalized < 3) interval = magnitude / 2;
    else if (normalized < 7) interval = magnitude;
    else interval = magnitude * 2;
    
    // İnterval çok küçükse yuvarla (ondalık sorunlarını önlemek için)
    if (interval < 0.1) interval = Math.ceil(interval * 100) / 100;
    
    return interval;
}

// Sayfa yüklendiğinde çalışacak fonksiyon
document.addEventListener("DOMContentLoaded", function() {
    // Eğer grafik verileri tanımlı değilse çıkış yap
    if (typeof chartData === 'undefined' || !chartData || !chartData.length) {
        return;
    }
    
    // Değişkenleri kontrol et
    var timeTypeId = typeof window.timeTypeId !== 'undefined' ? window.timeTypeId : 1;
    var chartTitle = typeof window.chartTitle !== 'undefined' ? window.chartTitle : 'Sayaç Verileri';
    var minValue = typeof window.minValue !== 'undefined' ? window.minValue : undefined;
    var maxValue = typeof window.maxValue !== 'undefined' ? window.maxValue : undefined;
    var interval = typeof window.interval !== 'undefined' ? window.interval : undefined;
    
    // Eğer grafik container yüklü değilse bekleme işlemi
    var checkExist = setInterval(function() {
        var chartContainer = document.getElementById("chartContainer");
        if (chartContainer) {
            clearInterval(checkExist);
            
            // 500ms sonra renderChart fonksiyonunu çağır
            setTimeout(function() {
                renderChart(chartData, timeTypeId, chartTitle, minValue, maxValue, interval);
            }, 500);
        }
    }, 100);
});
