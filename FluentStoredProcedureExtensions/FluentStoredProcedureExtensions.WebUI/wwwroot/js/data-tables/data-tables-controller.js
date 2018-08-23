var DataTablesController = function () {

    var initialize = function (tableId, options) {
        $(`#${tableId}`).DataTable(options);
    };

    var getSelectedRows = function (table) {
        return table.rows(".selected").data().map(row => row);
    };

    var reloadTable = function (tableId) {
        $(`#${tableId}`).DataTable().ajax.reload();
    };

    return {
        initialize: initialize,
        getSelectedRows: getSelectedRows,
        reloadTable: reloadTable
    };
}();