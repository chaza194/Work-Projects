(function () {
    var app = angular.module('WebDemo', []);
    app.controller('Logincontroller', function ($scope) {
        $scope.Username = "";
        $scope.Password = "";
    });
})();
