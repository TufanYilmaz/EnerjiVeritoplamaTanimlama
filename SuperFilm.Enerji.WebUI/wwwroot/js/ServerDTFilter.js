//$(document).ready(function () {
//    GetAllData();

//});
//function GetAllData() {
  
//}
var table = $('#pageTable').DataTable({
    language: {
        url: '//cdn.datatables.net/plug-ins/2.2.2/i18n/tr.json',
    },
    processing: true,
    serverSide: true,
    filter: true,
    ajax: {
        type: "Post",
        url: '/SayacVeri/FilterSayacVeri',
        dataType: 'json'
        
    },
    columns: [
        /*  { "data": "id", "name": "Id", "autowidth": true },*/
        { "data": "kod", "name": "Kod", "autowidth": true },
        //{ "data": "yil", "name": "Yil", "autowidth": true },
        //{ "data": "ay", "name": "Ay", "autowidth": true },
        //{ "data": "gun", "name": "Gun", "autowidth": true },
        //{ "data": "zaman", "name": "Zaman", "autowidth": true },
        { "data": "deger", "name": "Deger", "autowidth": true },
        //{ "data": "createDate", "name": "CreateDate", "autowidth": true },
        /*  { "data": "sayacId", "name": "SayacId", "autowidth": true },*/
        { "data": "opcNodesId", "name": "OpcNodesId", "autowidth": true },
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

$('#myform').on('submit', function (e) {
    e.preventDefault();
    table.ajax.reload();
});