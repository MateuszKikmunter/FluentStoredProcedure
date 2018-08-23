var BootstrapModalController = function () {
    var onModalClose;

    var appendModalLarge = function (isModalLarge) {
        if (isModalLarge) {
            $("#crud-modal-dialog").addClass("modal-lg");
        }
    };

    var attachEventsToControls = function () {
        $("#save-modal-btn").click(function () {
            if (validateForm()) {
                saveModal();
            }
        });
    };

    var failed = function (xhr) {
        let message = xhr.status === 422 ? xhr.message : xhr.statusText;
        alert(`Something went wrong... Error: ${xhr.status}. Message: ${message}.`);
    };

    var getActionUrl = function () {
        let form = getModalForm();
        let modalAction = form.attr("data-form-action");
        return modalAction === "Create" ? form.attr("data-create-url") : form.attr("data-edit-url");
    };

    var getHttpVerb = function () {
        return getModalForm().attr("data-form-action") === "Create" ? "POST" : "PUT";
    };

    var getModal = function (isModalLarge, ajaxData) {
        appendModalLarge(isModalLarge);
        getModalContent(ajaxData);
    };

    var getModalContent = function (ajaxData) {
        $.ajax({
            type: "GET",
            url: getModalUri(),
            data: ajaxData,
            dataType: "HTML",
            contentType: "application/json"
        }).done(function (data) {
            setModalContent(data);
            showModal();
        });
    };

    var getModalForm = function () {
        return getModalInstance().find("form");
    };

    var getModalInstance = function () {
        return $("#crud-modal");
    };

    var getModalUri = function () {
        return getModalInstance().attr("data-modal-uri");
    };

    var hideModal = function () {
        getModalInstance().modal("hide");
    };

    var initialize = function (modalUri) {
        getModalInstance().attr("data-modal-uri", modalUri);
        attachEventsToControls();
    };

    var saveModal = function () {
        let form = getModalForm().serializeObject();
        $.ajax({
            url: getActionUrl(),
            headers: {
                RequestVerificationToken: form.__RequestVerificationToken
            },
            type: getHttpVerb(),
            data: JSON.stringify(form),
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            complete: function (xhr) {
                xhr.status === 200 ? hideModal() : failed(xhr);
                if (onModalClose) {
                    onModalClose();
                }
            }
        });
    };

    var setModalContent = function (data) {
        $("#crud-modal-body-container").html(data);
    };

    var setModalHeaderText = function (text) {
        getModalInstance().find(".modal-title").text(text);
    };

    var setOnModalCloseCallback = function (callback) {
        onModalClose = callback;
    };

    var showModal = function () {
        getModalInstance().modal("show");
    };

    var validateForm = function () {
        let form = getModalForm();
        $.validator.unobtrusive.parse(form);
        form.validate();
        return form.valid();
    };

    return {
        getModal: getModal,
        hideModal: hideModal,
        initialize: initialize,
        setModalHeaderText: setModalHeaderText,
        setOnModalCloseCallback: setOnModalCloseCallback
    };
}();