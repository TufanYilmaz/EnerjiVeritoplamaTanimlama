$(document).ready(function () {
    GetAllData();
});
function GetAllData() {
    $('#tblData').DataTable({
        language: {
            url: '//cdn.datatables.net/plug-ins/2.2.2/i18n/tr.json',
        },
        processing: true,
        serverSide: true,
        filter: true,
        ajax: {
            type: "Post",
            url: '/SayacVeri/GetData',
            dataType: 'json'
        },
        columns: [
 
            { "data": "kod", "name": "Kod", "autowidth": true },
            { "data": "sayacTanimi", "name": "SayacTanimi", "autowidth": true },
            { "data": "description", "name": "Description", "autowidth": true },
            { "data": "deger", "name": "Deger", "autowidth": true },
            { "data": "normalizeDate", "name": "NormalizeDate", "autowidth": true },
        

        ],
        columnDefs: [
            {
                targets: [0],
                /*visible:false,*/
                searchable: false,

            }
        ]

    });
}