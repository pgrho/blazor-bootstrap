var Shipwreck;
(function (Shipwreck) {
    var BlazorBootstrap;
    (function (BlazorBootstrap) {
        function initModal(element, obj, show) {
            $(element).on('show.bs.modal', function () {
                obj.invokeMethodAsync('SetShowing');
            }).on('shown.bs.modal', function () {
                obj.invokeMethodAsync('SetShown');
            }).on('hide.bs.modal', function () {
                obj.invokeMethodAsync('SetHiding');
            }).on('hidden.bs.modal', function () {
                obj.invokeMethodAsync('SetHidden');
            }).on('hidePrevented.bs.modal', function () {
                obj.invokeMethodAsync('SetHidePrevented');
            }).modal(show ? 'show' : 'hide');
        }
        BlazorBootstrap.initModal = initModal;
        function invokeModal(element, name) {
            $(element).modal(name);
        }
        BlazorBootstrap.invokeModal = invokeModal;
    })(BlazorBootstrap = Shipwreck.BlazorBootstrap || (Shipwreck.BlazorBootstrap = {}));
})(Shipwreck || (Shipwreck = {}));