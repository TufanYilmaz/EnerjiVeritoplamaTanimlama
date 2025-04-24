
//$(document).ready(function () {
//	$(".pageTable").DataTable({
//		language: {
//			url: '//cdn.datatables.net/plug-ins/2.2.2/i18n/tr.json',
//		},
//	});

//});

$(document).ready(function () {
    const $table = $("#pageTable");

    const hasRealData = $table.find("tbody tr").filter(function () {
        const td = $(this).find("td");
        return !(td.length === 1 && td.attr("colspan")); // sadece bilgi satırı değilse
    }).length > 0;

    if (hasRealData) {
        $table.DataTable({
            language: {
                url: '//cdn.datatables.net/plug-ins/2.2.2/i18n/tr.json',
            },
        });
    }
});
