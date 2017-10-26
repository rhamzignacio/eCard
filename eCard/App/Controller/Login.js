angular.module("login", ["angular-growl"])

.controller("loginController", ["$scope", "$location", "$http", "growl", function ($scope, $locaiton, $http, growl) {

    var vm = this;

    $scope.TryLogin = function (username, password) {
        $http({
            method: "POST",
            url: "/Home/TryLogin",
            data: {
                _username: username,
                _password: password

            }
        }).then(function (data) {
            if (data.data != "") {
                growl.error(data.data, { ttl: 3000 });
            }
            else {
                window.location.href = "/Home/Index";
            }
        });
    }
}]);