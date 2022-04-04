(function () {
    'use strict';

    angular.module("umbraco")
        .controller("Diplo.Media.Controller",
            function (navigationService, appState) {
                var vm = this;
                vm.toggleShow = false;
                vm.checked = false;
                vm.currentNode = appState.getMenuState("currentNode");

                // id -1 is the root folder

                if (vm.currentNode.id == -1 || vm.currentNode.metaData.contentType.toLowerCase() === "folder") {
                    vm.toggleShow = true
                    vm.checked = true;
                }

                vm.performDownload = function () {
                    const downloadUrl = "/Umbraco/backoffice/DiploMedia/Media/Download/" + vm.currentNode.id + "?nested=" + vm.checked;
                    document.location.assign(downloadUrl);
                    navigationService.hideDialog();
                };

                vm.cancelDownload = function () {
                    navigationService.hideDialog();
                };

                vm.toggle = function () {
                    vm.checked = !vm.checked;
                }
            });
})();