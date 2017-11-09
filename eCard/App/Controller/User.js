angular.module("user", ["angular-growl"])

.controller("userController", ["$scope", "$location", "$http", "growl", function ($scope, $location, $http, growl) {
    var vm = this;

    ErrorMessage = function (message) {
        growl.error(message, { title: "Error!", ttl: 4000 });
    }

    SuccessMessage = function (message) {
        growl.success(message, { ttl: 3000 });
    }

    $scope.init = function () {
        $http({
            method: "POST",
            url: "/User/GetAll",
            arguments: { "Content-Type": "application/json" }
        }).then(function (data) {
            if (data.data.error != "") {
                ErrorMessage(data.data.error);
            }
            else {
                vm.Users = data.data.users;
            }
        });
    }
}]);