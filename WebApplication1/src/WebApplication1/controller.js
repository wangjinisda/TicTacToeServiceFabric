(function () {
    'use strict';

    angular
        .module('app')
        .controller('controller', controller);

    controller.$inject = ['$scope']; 

    function controller($scope) {
        $scope.title = 'Sample title';

        activate();

        function activate() { }
    }
})();
