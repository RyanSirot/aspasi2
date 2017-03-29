var app = angular.module('UserApp', []);

app.controller('UserController', function ($scope, $http) {
    $scope.GetUsers = function () {
        alert("hi");
    }
});