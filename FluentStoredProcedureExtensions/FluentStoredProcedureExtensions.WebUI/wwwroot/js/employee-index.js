$(document).ready(function () {
    let dtOptions = {
        processing: true,
        responsive: true,
        select: { style: "single" },
        dom: "Bftrip",
        ajax: {
            url: "/Employee/GetEmployees",
            type: "GET",
            dataType: "JSON"
        },
        language: { processing: "<i class='fa fa-cog fa-spin fa-4x fa-fw'></i>" },
        rowId: "id",
        columns: [
            { data: "id" },
            { data: "name" },
            { data: "email" }
        ],
        buttons: [
            {
                text: "<i class='fa fa-plus' aria-hidden='true'></i> Add",
                className: "btn btn-outline-success",
                action: function () {
                    BootstrapModalController.setModalHeaderText("Create");
                    BootstrapModalController.getModal(false);
                }
            },
            {
                text: "<i class='fa fa-edit' aria-hidden='true'></i> Edit",
                className: "btn btn-outline-primary",
                action: function (e, dt, node, config) {
                    let selectedRows = DataTablesController.getSelectedRows(dt);
                    if (validateSelection(selectedRows)) {
                        BootstrapModalController.setModalHeaderText("Edit");
                        BootstrapModalController.getModal(false, { id: selectedRows[0].id });
                    }
                }
            },
            {
                text: "<i class='fa fa-trash' aria-hidden='true'></i> Delete",
                className: "btn btn-outline-danger",
                action: function (e, dt, node, config) {
                    let selectedRows = DataTablesController.getSelectedRows(dt);
                    if (validateSelection(selectedRows) && confirm("Are you sure?")) {
                        deleteEmployee(selectedRows[0].id);
                    }
                }
            }
        ]
    };

    let validateSelection = function (selectedEntities) {
        return selectedEntities.length;
    };

    let deleteEmployee = function (id) {
        $.ajax({
            url: "/Employee/Delete",
            type: "DELETE",
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(id),
            complete: function (xhr) {
                xhr.status === 200
                    ? DataTablesController.reloadTable("employees-table")
                    : alert(`Something went wrong. Error: ${xhr.status} - ${xhr.statusText}`);
            }
        });
    };

    DataTablesController.initialize("employees-table", dtOptions);
    BootstrapModalController.initialize("/Employee/Get/");
    BootstrapModalController.setOnModalCloseCallback(function () {
        DataTablesController.reloadTable("employees-table");
    });
});