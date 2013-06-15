KitchenSink.Overlays = function (params) {
    var viewModel = {
        loadPanel: {
            visible: ko.observable(false),
            startLoading: function() {
                viewModel.loadPanel.visible(true);
                setTimeout(viewModel.loadPanel.finishLoading, 3000);
            },
            finishLoading: function() {
                viewModel.loadPanel.visible(false);
            }
        },
        popup: {
            showPopup: function() {
                var popup = $("#popup").data("dxPopup");
                popup.show();
            },
            hidePopup: function() {
                var popup = $("#popup").data("dxPopup");
                popup.hide();
            }
        },
        actionsheet: {
            showActionSheet: function() {
                var actionSheet = $("#action_sheet").data("dxActionSheet");
                actionSheet.show();
            },
            items: [
                {
                    text: "Delete",
                    clickAction: function() { alert("Delete"); },
                    type: "danger"
                },
                {
                    text: "Reply",
                    clickAction: function() { alert("Reply"); }
                },
                {
                    text: "Forward",
                    clickAction: function() { alert("Forward"); }
                },
                {
                    text: "Save Image",
                    clickAction: function() { alert("Save Image"); },
                    disabled: true
                }
            ]
        },
        toast: {
            showInfo: function() {
                var toast = $("#toast-info").data("dxToast");
                toast.show();
            },
            showError: function() {
                var toast = $("#toast-error").data("dxToast");
                toast.show();
            },
            showSuccess: function() {
                var toast = $("#toast-success").data("dxToast");
                toast.show();
            },
            showWarning: function() {
                var toast = $("#toast-warning").data("dxToast");
                toast.show();
            },
            showCustom: function() {
                var toast = $("#toast-custom").data("dxToast");
                toast.show();
            }
        }
    };
    return viewModel;
};